using System;
using RicoShot.Core;
using RicoShot.Core.Interface;
using RicoShot.Result.Interface;
using RicoShot.InputActions;
using UnityEngine;
using Zenject;

namespace RicoShot.Result
{
    public class ResultSceneManager : IResultSceneManager, IInitializable, IDisposable
    {
        public ResultInputs ResultInputs { get; }
        public CharacterParams CharacterParams { get; set; }
        public event Action<GameObject> OnLocalPlayerSpawned;

        private GameObject _localPlayer;

        public GameObject LocalPlayer
        {
            get => _localPlayer;
            set
            {
                _localPlayer = value;
                OnLocalPlayerSpawned?.Invoke(_localPlayer);
            }
        }

        private ResultSceneManager()
        {
            ResultInputs = new();
        }

        public void Initialize()
        {
            ResultInputs.Enable();
        }

        public void Dispose()
        {
            ResultInputs.Disable();
        }
    }
}