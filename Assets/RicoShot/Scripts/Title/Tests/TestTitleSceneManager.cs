using Cysharp.Threading.Tasks;
using RicoShot.Core.Interface;
using RicoShot.InputActions;
using RicoShot.Title.Interface;
using System;
using Unity.Collections;
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

        public string ReadUUID
        {
            get => _readUuid;
            set
            {
                _readUuid = value;
                TitleState = TitleState.Fetching;
            }
        }

        private TitleState _titleState;
        private string _readUuid;

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

        private void FetchProfileFromSupabase(TitleState titleState)
        {
            if (titleState == TitleState.Fetching)
            {
                
            }
        }

        public void Dispose()
        {
            TitleInputs.Dispose();
        }
    }
}
