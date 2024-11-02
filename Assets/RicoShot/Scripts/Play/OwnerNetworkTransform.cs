using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

namespace RicoShot.Play
{
    /// <summary>
    /// NetworkTransformの代わりにこれをつけることでOwnerから動きを制御できるようになる
    /// </summary>
    [DisallowMultipleComponent]
    public class OwnerNetworkTransform : NetworkTransform
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }    
    }
}
