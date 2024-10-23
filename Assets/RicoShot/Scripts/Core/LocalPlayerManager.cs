using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RicoShot.Core.Interface;

namespace RicoShot.Core
{
    public class LocalPlayerManager : ILocalPlayerManager
    {
        public string LocalPlayerUUID { get; set; }
    }
}
