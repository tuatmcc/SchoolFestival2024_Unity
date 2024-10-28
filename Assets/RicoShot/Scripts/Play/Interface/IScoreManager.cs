using RicoShot.Core;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace RicoShot.Play.Interface
{
    public interface INetworkScoreManager
    {
        public NetworkClassList<ScoreData> ScoreList { get; }

        public void RegistCharacter(FixedString64Bytes uuid, Team team);
        public void AddScoreRpc(FixedString64Bytes uuid, int score);
    }
}

