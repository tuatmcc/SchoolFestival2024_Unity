using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using RicoShot.Core.Interface;
using RicoShot.Play.Interface;
using TMPro;
using UnityEngine;
using Zenject;

namespace RicoShot.Play.UI
{
    public class ScorePresenter : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;

        [Inject] private readonly INetworkController _networkController;
        [Inject] private readonly ILocalPlayerManager _localPlayerManager;
        [Inject] private readonly IPlaySceneManager _playSceneManager;

        private void Start()
        {
            Observable.FromEvent<PlayState>(h => _playSceneManager.OnPlayStateChanged += h,
                    h => _playSceneManager.OnPlayStateChanged -= h)
                .Where(state => state == PlayState.Playing)
                .Subscribe(_ => SetScoreUpdater())
                .AddTo(this);
        }

        private void SetScoreUpdater()
        {
            List<ScoreData> scores = new();
            foreach (var score in _networkController.ScoreManager.ScoreList) scores.Add(score);

            var index = scores.FindIndex(x => x.UUID == _localPlayerManager.LocalPlayerUUID);
            Observable.FromEvent(h => _networkController.ScoreManager.ScoreList[index].OnDataChanged += h,
                    h => _networkController.ScoreManager.ScoreList[index].OnDataChanged -= h)
                .Subscribe(_ => scoreText.text = _networkController.ScoreManager.ScoreList[index].Score.ToString())
                .AddTo(this);
        }
    }
}