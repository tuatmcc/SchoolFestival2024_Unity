using RicoShot.Core;
using RicoShot.Core.Interface;
using RicoShot.Play.Interface;
using System;
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

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public bool DataUploadFinished { get; private set; } = false;

        [Inject] private readonly IGameStateManager gameStateManager;
        [Inject] private readonly INetworkController networkController;
        [Inject] private readonly IPlaySceneManager playSceneManager;
        [Inject] private readonly IPlaySceneTester playSceneTester;

        private void Start()
        {
            networkController.ScoreManager = this;
            if (playSceneTester.IsTest) return;
            gameStateManager.OnReset += DestroyThisOnReset;
            playSceneManager.OnPlayStateChanged += SetTimeData;
            DontDestroyOnLoad(gameObject);
        }

        private void SetTimeData(PlayState playState)
        {
            switch (playState)
            {
                case PlayState.Playing:
                    StartTime = DateTime.Now;
                    break;
                case PlayState.Finish:
                    EndTime = DateTime.Now;
                    break;
                default:
                    break;
            }
        }

        public void RegistCharacter(FixedString64Bytes uuid, Team team, bool isNpc)
        {
            ScoreList.Add(new ScoreData(uuid, team, isNpc));
        }

        // (キャラクター→サーバー) 各キャラクターのスコアを加算する関数
        [Rpc(SendTo.Server)]
        public void AddScoreRpc(FixedString64Bytes uuid, int score)
        {
            var scoreData = GetScoreDataFromUUID(uuid);
            scoreData.Score += score;
        }

        public bool IsWin(Team team)
        {
            int alphaScore = 0, bravoScore = 0;
            foreach (var scoreData in ScoreList)
            {
                if (scoreData.Team == Team.Alpha)
                {
                    alphaScore += scoreData.Score;
                }
                else
                {
                    bravoScore += scoreData.Score;
                }
            }
            return (team == Team.Alpha && alphaScore >= bravoScore) || (team == Team.Bravo && bravoScore >= alphaScore);
        }

        private ScoreData GetScoreDataFromUUID(FixedString64Bytes uuid)
        {
            foreach (var scoreData in ScoreList)
            {
                if (scoreData.UUID == uuid) return scoreData;
            }
            return null;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (IsServer)
            {
                Destroy(gameObject);
            }
        }

        private void DestroyThisOnReset()
        {
            if (gameStateManager.NetworkMode == NetworkMode.Client)
            {
                Destroy(gameObject);
            }
        }

        public override void OnDestroy()
        {
            gameStateManager.OnReset -= DestroyThisOnReset;
            base.OnDestroy();
        }

        // 終了後Destroy
        private void UploadResult(PlayState playState)
        {
            if (playState == PlayState.Finish)
            {
                DontDestroyOnLoad(gameObject);
                foreach(ScoreData scoreData in ScoreList)
                {
                    Debug.Log($"{scoreData.UUID} -> {scoreData.Score}");
                }
                Debug.Log("Upload result to Supabase");
            }
        }
    }
}
