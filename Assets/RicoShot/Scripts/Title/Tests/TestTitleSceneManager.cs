using Cysharp.Threading.Tasks;
using R3;
using RicoShot.Core;
using RicoShot.Core.Interface;
using RicoShot.InputActions;
using RicoShot.Title.Interface;
using System;
using UnityEditor;
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
        public int FetchedPlayCount { get; private set; }
        public int FetchedHighScore { get; private set; }

        private const string GuestUUID = "599cb64e-fced-4e71-a1f6-e73c4f69f972";
        private const string GuestName = "ゲスト";

        private TitleState _titleState;
        private bool fetching = false;
        private bool transiting = false;
        private string uuid;

        [Inject] private readonly IGameStateManager gameStateManager;
        [Inject] private readonly ISupabaseController supabaseController;
        [Inject] private readonly ILocalPlayerManager localPlayerManager;

        private TestTitleSceneManager()
        {
            TitleState = TitleState.Reading;
            TitleInputs = new TitleInputs();
            TitleInputs.Enable();
        }

        public void Initialize()
        {
            TitleInputs.Main.Skip.performed += StartAsGuest;
        }

        private void OnEnter(InputAction.CallbackContext context)
        {
            if (transiting) return;
            transiting = true;
            gameStateManager.NextScene();
        }

        private void StartAsGuest(InputAction.CallbackContext context)
        {
            uuid = GuestUUID;
            FetchedCharacterParams = CharacterParams.GetRandomCharacterParams();
            FetchedDisplayName = GuestName;
            FetchedHighScore = 0;
            FetchedPlayCount = 0;
            TitleState = TitleState.Confirming;
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
            if (TitleState != TitleState.Reading) return;
            fetching = true;
            TitleState = TitleState.Fetching;
            var (displayName, characterParams, playCount, highScore) = await supabaseController.FetchPlayerProfile(uuid);
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
                FetchedPlayCount = playCount;
                FetchedHighScore = highScore;
                this.uuid = uuid;
                Debug.Log($"Fetched name: {FetchedDisplayName}, characterParams: {characterParams}");
                TitleState = TitleState.Confirming;
            }
            fetching = false;
        }

        public void Dispose()
        {
            localPlayerManager.LocalPlayerUUID = uuid;
            localPlayerManager.CharacterParams = FetchedCharacterParams;
            localPlayerManager.LocalPlayerName = FetchedDisplayName;
            TitleInputs.Main.Skip.performed -= StartAsGuest;
            TitleInputs.Disable();
        }
    }
}