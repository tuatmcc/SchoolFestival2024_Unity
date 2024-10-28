using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

namespace RicoShot.Play
{
    /// <summary>
    /// NetworkTransformの代わりにこれをつけることでClientから動きを制御できるようになる
    /// </summary>
    [DisallowMultipleComponent]
    public class ClientNetworkTransform : NetworkTransform
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }    
    }
}
