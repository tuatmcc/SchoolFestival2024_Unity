using System;
using RicoShot.Core;
using RicoShot.InputActions;
using UnityEngine;

namespace RicoShot.Result.Interface
{
    public interface IResultSceneManager
    {
        public ResultInputs ResultInputs { get; }
        
        public CharacterParams CharacterParams { get; set; }
    }
}