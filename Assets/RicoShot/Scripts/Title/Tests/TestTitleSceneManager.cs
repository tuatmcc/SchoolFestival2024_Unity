using Cysharp.Threading.Tasks;
using RicoShot.Core;
using RicoShot.Core.Interface;
using RicoShot.InputActions;
using RicoShot.Title.Interface;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace RicoShot.Title.Tests
{
    public class TestTitleSceneManager : ITitleSceneManager, IInitializable, IDisposable
    {
        public event Action<TitleState> OnTitleStateChanged;
        public event Action OnReadNotUUID;
        public event Action OnReadUUIDNotExist;

        public TitleInputs TitleInputs { get; private set; }

        public TitleState TitleState
        {
            get => _titleState;
            set
            {
                _titleState = value;
                OnTitleStateChanged?.Invoke(_titleState);
                Debug.Log($"TitleState changed => {_titleState}");
            }
        }

        public string FetchedDisplayName { get; private set; }
        public CharacterParams FetchedCharacterParams { get; private set; }

        private TitleState _titleState;
        private bool fetching = false;

        [Inject] private readonly IGameStateManager gameStateManager;
        [Inject] private readonly ISupabaseController supabaseController;

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

        public void FetchData(string uuid)
        {
            if (Guid.TryParse(uuid, out var _))
            {
                GetProfileAsync(uuid).Forget();
            }
            else
            {
                OnReadNotUUID?.Invoke();
                Debug.Log("Read not UUID");
            }
        }

        private async UniTask GetProfileAsync(string uuid)
        {
            if (fetching && TitleState != TitleState.Reading) return;
            fetching = true;
            TitleState = TitleState.Fetching;
            var (displayName, characterParams) = await supabaseController.FetchPlayerProfile(uuid);
            if (displayName == string.Empty && characterParams == null)
            {
                OnReadUUIDNotExist?.Invoke();
                Debug.Log("UUID not exist");
                TitleState = TitleState.Reading;
            }
            else
            {
                FetchedDisplayName = displayName;
                FetchedCharacterParams = characterParams;
                Debug.Log($"Fetched name: {FetchedDisplayName}");
                TitleState = TitleState.Confirming;
            }
            fetching = false;
        }

        public void Dispose()
        {
            TitleInputs.Disable();
        }
    }
}
