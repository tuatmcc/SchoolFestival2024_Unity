using RicoShot.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RicoShot.Play.Interface
{
    public interface IPlaySceneTester
    {
        public bool IsTest { get; }
        public bool BehaveAsNPC { get; }
        public CharacterParams CharacterParams { get; }
    }
}
