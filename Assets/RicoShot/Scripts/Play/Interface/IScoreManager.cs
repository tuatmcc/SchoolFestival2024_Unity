using RicoShot.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RicoShot.Play.Interface
{
    public interface INetworkScoreManager
    {
        NetworkClassList<ScoreData> ScoreList { get; }
    }
}

