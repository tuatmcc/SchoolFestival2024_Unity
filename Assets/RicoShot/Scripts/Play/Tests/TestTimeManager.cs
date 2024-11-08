using Cysharp.Threading.Tasks;
using RicoShot.Core.Interface;
using RicoShot.Play.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace RicoShot.Play.Tests
{
    public class TestTimeManager : NetworkBehaviour, ITimeManager
    {
        public event Action<int> OnCountChanged;
        public event Action<long> OnPlayTimeChanged;

        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                OnCountChanged?.Invoke(value);
                Debug.Log($"Count changed: {value}");
            }
        }

        public long PlayTime
        {
            get => _playTime;
            set
            {
                _playTime = value;
                OnPlayTimeChanged?.Invoke(value);
            }
        }

        private int _count;
        private long _playTime = 180;
        private long _startTime;

        [SerializeField] private int countDownLength = 3;

        private readonly List<float> _lagList = new();

        [Inject] private readonly IPlaySceneManager playSceneManager;
        [Inject] private readonly IGameStateManager gameStateManager;

        private void Start()
        {
            if (IsClient)
                SendLagRpc(NetworkManager.Singleton.LocalTime.TimeAsFloat -
                           NetworkManager.Singleton.ServerTime.TimeAsFloat);
        }

        private void Update()
        {
            if (playSceneManager.PlayState == PlayState.Playing && PlayTime > 0)
                PlayTime = TimeSpan.FromMinutes(0.1).Ticks - (DateTime.Now.Ticks - _startTime);
            if (PlayTime <= 0 && playSceneManager.PlayState == PlayState.Playing)
            {
                playSceneManager.PlayState = PlayState.Finish;
                playSceneManager.PlayState = PlayState.Despawn;
                UniTask.Create(async () =>
                {
                    await UniTask.WaitForSeconds(1, cancellationToken: destroyCancellationToken);
                    gameStateManager.NextScene();
                });
            }
        }

        // (クライアント→サーバー)クライアントとサーバーの時間がどれだけずれているかをサーバーに通知する関数
        [Rpc(SendTo.Server)]
        private void SendLagRpc(float lag)
        {
            _lagList.Add(lag);
            if (_lagList.Count == NetworkManager.Singleton.ConnectedClients.Count)
                // 最も大きいラグ + 1秒後に始まるように設定して全体に通知
                SendStartTimeRpc(_lagList.Max() + 1.0f + NetworkManager.Singleton.ServerTime.TimeAsFloat);
        }

        // (サーバー→全体)カウントダウン開始時間をServerTime基準で送信する関数
        [Rpc(SendTo.Everyone)]
        private void SendStartTimeRpc(float startTime)
        {
            CountDownStartAsync(startTime).Forget();
        }

        // カウントダウン開始まで待ってカウントダウンを始める関数
        private async UniTask CountDownStartAsync(float startTime)
        {
            await UniTask.WaitUntil(() => startTime >= NetworkManager.Singleton.ServerTime.TimeAsFloat);
            playSceneManager.PlayState = PlayState.Countdown;
            Count = countDownLength;
            while (Count > 0)
            {
                await UniTask.WaitForSeconds(1, cancellationToken: destroyCancellationToken);
                Count--;
            }

            playSceneManager.PlayState = PlayState.Playing;
            _startTime = DateTime.Now.Ticks;
        }
    }
}