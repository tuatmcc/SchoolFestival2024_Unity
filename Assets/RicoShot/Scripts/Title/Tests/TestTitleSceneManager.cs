using Cysharp.Threading.Tasks;
using RicoShot.Core.Interface;
using RicoShot.InputActions;
using RicoShot.Title.Interface;
using System;
using UnityEngine.InputSystem;
using Zenject;

namespace RicoShot.Title.Tests
{
    public class TestTitleSceneManager : ITitleSceneManager, IInitializable, IDisposable
    {
        public event Action<TitleState> OnTitleStateChanged;

        public TitleInputs TitleInputs { get; private set; }

        public TitleState TitleState
        {
            get => _titleState;
            set
            {
                _titleState = value;
                OnTitleStateChanged?.Invoke(_titleState);
            }
        }

        private TitleState _titleState;

        [Inject] IGameStateManager gameStateManager;

        TestTitleSceneManager()
        {
            TitleState = TitleState.Reading;
            TitleInputs = new();
            TitleInputs.Enable();
        }

        public void Initialize()
        {
            TitleInputs.Test.Enter.performed += OnEnter;
        }

        private void OnEnter(InputAction.CallbackContext context)
        {
            gameStateManager.NextScene();
        }

        public void Dispose()
        {
            TitleInputs.Disable();
        }
    }
}
