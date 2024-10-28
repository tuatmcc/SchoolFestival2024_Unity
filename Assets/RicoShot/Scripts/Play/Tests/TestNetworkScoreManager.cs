using RicoShot.Core;
using RicoShot.Play.Interface;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace RicoShot.Play.Tests
{
    public class TestNetworkScoreManager : NetworkBehaviour, INetworkScoreManager
    {
        public NetworkClassList<ScoreData> ScoreList { get; } = new();

        [Inject] private readonly IPlaySceneManager playSceneManager;

        private void Start()
        {
            if (!NetworkManager.IsServer)
            {
                playSceneManager.OnPlayStateChanged += UploadResult;
            }
        }

        public void RegistCharacter(FixedString64Bytes uuid, Team team)
        {
            ScoreList.Add(new ScoreData(uuid, team));
        }

        // (キャラクター→サーバー) 各キャラクターのスコアを加算する関数
        [Rpc(SendTo.Server)]
        public void AddScoreRpc(FixedString64Bytes uuid, int score)
        {
            var scoreData = GetScoreDataFromUUID(uuid);
            scoreData.Score += score;
        }

        private ScoreData GetScoreDataFromUUID(FixedString64Bytes uuid)
        {
            foreach (var scoreData in ScoreList)
            {
                if (scoreData.UUID == uuid) return scoreData;
            }
            return null;
        }

        // (サーバー)ここでSupabaseにデータをアップロードする処理
        // 終了後Destroy
        private void UploadResult(PlayState playState)
        {
            if (playState == PlayState.Finish)
            {
                DontDestroyOnLoad(gameObject);
                Debug.Log("Upload result to Supabase");
            }
        }
    }
}
