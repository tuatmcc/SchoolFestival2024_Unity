using RicoShot.Core;
using RicoShot.Core.Interface;
using RicoShot.InputActions;
using RicoShot.Matching.Interface;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace RicoShot.Matching.Tests
{
    public class TestMatchingSceneManager : IMatchingSceneManager, IInitializable, IDisposable
    {
        public event Action<MatchingState> OnMatchingStateChanged;

        public MatchingInputs MatchingInputs { get; private set; }
        public MatchingState MatchingState
        {
            get => _matchingState;
            set
            {
                _matchingState = value;
                OnMatchingStateChanged?.Invoke(value);
                Debug.Log($"MatchingState changed: {value}");
            }
        }

        private MatchingState _matchingState;

        [Inject] private readonly IGameStateManager gameStateManager;
        [Inject] private readonly INetworkController networkController;

        TestMatchingSceneManager()
        {
            MatchingState = MatchingState.Connecting;
            MatchingInputs = new();
            MatchingInputs.Enable();
        }


        public void Initialize()
        {
            if (gameStateManager.NetworkMode == NetworkMode.Client)
            {
                MatchingInputs.Test.SelectLeft.performed += OnSelectLeft;
                MatchingInputs.Test.SelectRight.performed += OnSelectRight;
                MatchingInputs.Test.Enter.performed += OnEnter;
                MatchingInputs.Test.Cancel.performed += OnCancel;
            }
        }

        private void OnSelectLeft(InputAction.CallbackContext context)
        {
            networkController.UpdateTeamRpc(Team.Alpha);
        }

        private void OnSelectRight(InputAction.CallbackContext context)
        {
            networkController.UpdateTeamRpc(Team.Bravo);
        }

        private void OnEnter(InputAction.CallbackContext context)
        {
            if (!networkController.AllClientsReady.Value)
            {
                networkController.UpdateReadyStatusRpc(true);
            }
            else
            {
                networkController.StartPlayRpc();
            }
        }
        
        private void OnCancel(InputAction.CallbackContext context)
        {
            networkController.UpdateReadyStatusRpc(false);
        }

        public void Dispose()
        {
            MatchingInputs.Dispose();
        }
    }
}
