using System;
using Cysharp.Threading.Tasks;
using R3;
using RicoShot.Core;
using RicoShot.Core.Interface;
using RicoShot.Result.Interface;
using RicoShot.InputActions;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace RicoShot.Result
{
    public class ResultSceneManager : IResultSceneManager, IInitializable, IDisposable
    {
        public ResultInputs ResultInputs { get; }
        public CharacterParams CharacterParams { get; set; }
        public event Action<GameObject> OnLocalPlayerSpawned;

        private GameObject _localPlayer;

        [Inject] private readonly ILocalPlayerManager _localPlayerManager;
        [Inject] private readonly IGameStateManager _gameStateManager;

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
            ResultInputs = new ResultInputs();
        }

        public void Initialize()
        {
            ResultInputs.Enable();
            CharacterParams = _localPlayerManager.CharacterParams;

            Observable.FromEvent<InputAction.CallbackContext>(h => ResultInputs.Main.Next.performed += h,
                h => ResultInputs.Main.Next.performed -= h).Subscribe(
                _ => _gameStateManager.NextScene()
            );
        }

        public void Dispose()
        {
            ResultInputs.Disable();
        }
    }
}