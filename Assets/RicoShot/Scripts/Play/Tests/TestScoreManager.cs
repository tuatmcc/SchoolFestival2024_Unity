using RicoShot.Core;
using RicoShot.Core.Interface;
using RicoShot.Play.Interface;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace RicoShot.Play.Tests
{
    public class TestScoreManager : NetworkBehaviour, INetworkScoreManager
    {
        public NetworkClassList<ScoreData> ScoreList { get; } = new();

        [Inject] private readonly INetworkController networkController;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        [Rpc(SendTo.Server)]
        public void RegistScoreListRpc(FixedString64Bytes uuid)
        {
            ScoreList.Add(new ScoreData(uuid));
        }

        public void AddScore(FixedString64Bytes uuid, int score)
        {
            var scoreData = GetScoreDataFromUUID(uuid);
            scoreData.Score += score;
        }

        private ScoreData GetScoreDataFromUUID(FixedString64Bytes uuid)
        {
            foreach(var scoreData in ScoreList)
            {
                if (scoreData.UUID == uuid)
                {
                    return scoreData;
                }
            }
            return null;
        }
    }
}
