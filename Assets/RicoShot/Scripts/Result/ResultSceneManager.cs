using System;
using R3;
using RicoShot.Core;
using RicoShot.Core.Interface;
using RicoShot.InputActions;
using RicoShot.Result.Interface;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace RicoShot.Result
{
    public class ResultSceneManager : IResultSceneManager, IInitializable, IDisposable
    {
        [Inject] private readonly IGameStateManager _gameStateManager;

        [Inject] private readonly ILocalPlayerManager _localPlayerManager;

        private GameObject _localPlayer;
        private readonly CompositeDisposable disposables = new();

        private ResultSceneManager()
        {
            ResultInputs = new ResultInputs();
        }

        public GameObject LocalPlayer
        {
            get => _localPlayer;
            set
            {
                _localPlayer = value;
                OnLocalPlayerSpawned?.Invoke(_localPlayer);
            }
        }

        public void Dispose()
        {
            disposables.Dispose();
            ResultInputs.Disable();
        }

        public void Initialize()
        {
            ResultInputs.Enable();
            CharacterParams = _localPlayerManager.CharacterParams;

            Observable.FromEvent<InputAction.CallbackContext>(h => ResultInputs.Main.Next.performed += h,
                h => ResultInputs.Main.Next.performed -= h).Subscribe(
                _ => _gameStateManager.NextScene()
            ).AddTo(disposables);
        }

        public ResultInputs ResultInputs { get; }
        public CharacterParams CharacterParams { get; set; }
        public event Action<GameObject> OnLocalPlayerSpawned;
    }
}