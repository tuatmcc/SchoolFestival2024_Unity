using RicoShot.Matching.Interface;
using System;
using R3;
using RicoShot.Core;
using RicoShot.Core.Interface;
using RicoShot.InputActions;
using UnityEngine.InputSystem;
using Zenject;
using Unity.Netcode;

namespace RicoShot.Matching
{
    public class MatchingSceneManager : IMatchingSceneManager, IInitializable, IDisposable
    {
        [Inject] private IGameStateManager _gameStateManager;
        [Inject] private INetworkController _networkController;
        public event Action<MatchingState> OnMatchingStateChanged;

        public MatchingInputs MatchingInputs { get; private set; }

        public MatchingState MatchingState
        {
            get => _matchingState;
            set
            {
                _matchingState = value;
                OnMatchingStateChanged?.Invoke(value);
            }
        }

        private MatchingState _matchingState;

        public MatchingSceneManager()
        {
            MatchingState = MatchingState.Connecting;

            MatchingInputs = new MatchingInputs();
            MatchingInputs.Enable();
        }

        public void Initialize()
        {
            _networkController.OnServerConnectionCompleted += OnConnected;
            if (_gameStateManager.NetworkMode == NetworkMode.Client)
            {
                Observable.FromEvent<InputAction.CallbackContext>
                    (h => MatchingInputs.Main.Confirm.performed += h,
                        h => MatchingInputs.Main.Confirm.performed -= h)
                    .Subscribe(_ => OnConfirm());

                Observable.FromEvent<InputAction.CallbackContext>
                    (h => MatchingInputs.Main.SelectAlpha.performed += h,
                        h => MatchingInputs.Main.SelectAlpha.performed -= h)
                    .Subscribe(_ => OnSelectAlpha());

                Observable.FromEvent<InputAction.CallbackContext>
                    (h => MatchingInputs.Main.SelectBravo.performed += h,
                        h => MatchingInputs.Main.SelectBravo.performed -= h)
                    .Subscribe(_ => OnSelectBravo());

                Observable.FromEvent<InputAction.CallbackContext>
                    (h => MatchingInputs.Main.Cancel.performed += h,
                        h => MatchingInputs.Main.Cancel.performed -= h)
                    .Subscribe(_ => OnCancel());
            }
        }

        private void OnConnected()
        {
            MatchingState = MatchingState.Connected;
            OnSelectAlpha();
        }

        public void Dispose()
        {
            MatchingInputs.Disable();
        }


        private void OnSelectAlpha()
        {
            if (!NetworkManager.Singleton.IsClient) return;
            _networkController.UpdateReadyStatusRpc(false);
            _networkController.UpdateTeamRpc(Team.Alpha);
            _networkController.UpdateReadyStatusRpc(true);
        }

        private void OnSelectBravo()
        {
            if (!NetworkManager.Singleton.IsClient) return;
            _networkController.UpdateReadyStatusRpc(false);
            _networkController.UpdateTeamRpc(Team.Bravo);
            _networkController.UpdateReadyStatusRpc(true);
        }

        private void OnConfirm()
        {
            if (!NetworkManager.Singleton.IsClient) return;
            _networkController.StartPlayRpc();
        }

        private void OnCancel()
        {
            if (!NetworkManager.Singleton.IsClient) return;
            _gameStateManager.ForceReset();
        }
    }
}