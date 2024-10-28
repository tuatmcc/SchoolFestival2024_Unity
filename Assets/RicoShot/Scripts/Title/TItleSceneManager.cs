using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RicoShot.Title.Interface;
using Unity.VisualScripting;
using RicoShot.InputActions;
using System;
using Unity.Collections;

namespace RicoShot.Title
{
    public class TitleSceneManager : ITitleSceneManager, IDisposable
    {
        public event Action<TitleState> OnTitleStateChanged;

        public TitleInputs TitleInputs { get; private set; }
        
        public TitleState TitleState
        {
            get => _titleState;
            set
            {
                _titleState = value;
                OnTitleStateChanged?.Invoke(value);
            }
        }

        public string ReadUUID { get; set; }

        private TitleState _titleState;

        TitleSceneManager()
        {
            TitleState = TitleState.Reading;
            TitleInputs = new();
            TitleInputs.Enable();
        }

        public void Dispose()
        {
            TitleInputs.Dispose();
        }
    }
}
