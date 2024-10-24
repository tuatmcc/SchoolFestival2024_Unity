using RicoShot.Core.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RicoShot.Core.Tests
{
    public class TestLocalPlayerManager : ILocalPlayerManager
    {
        public string LocalPlayerUUID
        {
            get => "";
            set { }
        }

        public CharacterParams CharacterParams { get; set; } = new CharacterParams();
    }
}
