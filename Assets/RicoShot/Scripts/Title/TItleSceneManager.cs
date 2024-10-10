using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RicoShot.Title.Interface;
using Unity.VisualScripting;
using RicoShot.InputActions;
using System;

namespace RicoShot.Title
{
    public class TItleSceneManager : ITitleSceneManager, IDisposable
    {
        public TitleInputs TitleInputs { get; private set; }

        TItleSceneManager()
        {
            TitleInputs = new();
            TitleInputs.Enable();
        }

        public void Dispose()
        {
            TitleInputs.Dispose();
        }
    }
}
