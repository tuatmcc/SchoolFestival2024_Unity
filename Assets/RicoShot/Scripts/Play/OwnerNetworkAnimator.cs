using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

namespace RicoShot.Play
{
    /// <summary>
    /// NetworkAnimatorの代わりにこれをつけることでOwnerからアニメーションを制御できるようになる
    /// </summary>
    public class OwnerNetworkAnimator : NetworkAnimator
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}
