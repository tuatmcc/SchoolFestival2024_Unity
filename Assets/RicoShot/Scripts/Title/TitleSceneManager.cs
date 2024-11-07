using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RicoShot.Title.Interface;
using Unity.VisualScripting;
using RicoShot.InputActions;
using System;
using Cysharp.Threading.Tasks;
using RicoShot.Core;
using Zenject;
using RicoShot.Core.Interface;

namespace RicoShot.Title
{
    public class TitleSceneManager : ITitleSceneManager, IDisposable
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
                OnTitleStateChanged?.Invoke(value);
            }
        }

        public string FetchedDisplayName { get; private set; }
        public CharacterParams FetchedCharacterParams { get; private set; }

        private TitleState _titleState;
        private bool fetching = false;
        private string uuid;

        [Inject] private readonly ISupabaseController supabaseController;
        [Inject] private readonly ILocalPlayerManager localPlayerManager;

        TitleSceneManager()
        {
            TitleInputs = new();
            TitleInputs.Enable();
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
                this.uuid = uuid;
                Debug.Log($"Fetched name: {FetchedDisplayName}");
                TitleState = TitleState.Confirming;
            }
            fetching = false;
        }

        public void Dispose()
        {
            localPlayerManager.LocalPlayerUUID = uuid;
            localPlayerManager.CharacterParams = FetchedCharacterParams;
            localPlayerManager.LocalPlayerName = FetchedDisplayName;
            TitleInputs.Disable();
        }
    }
}
