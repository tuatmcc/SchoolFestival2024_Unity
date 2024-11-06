using RicoShot.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace RicoShot.Play.Interface
{
    public interface INetworkScoreManager
    {
        public NetworkClassList<ScoreData> ScoreList { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }

        public void RegistCharacter(FixedString64Bytes uuid, Team team, bool isNpc);
        public void AddScoreRpc(FixedString64Bytes uuid, int score);
        public bool IsWin(Team team);
    }
}

