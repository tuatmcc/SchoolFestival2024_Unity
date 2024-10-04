using RicoShot.Core;
using RicoShot.Core.Interface;
using RicoShot.InputActions;
using RicoShot.Matching.Interface;
using System;
using UnityEngine.InputSystem;
using Zenject;

namespace RicoShot.Matching.Tests
{
    public class TestMatchingSceneManager : IMatchingSceneManager, IInitializable, IDisposable
    {
        public MatchingInputs MatchingInputs { get; private set; }

        [Inject] IGameStateManager gameStateManager;
        [Inject] INetworkController networkController;

        TestMatchingSceneManager()
        {
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
        
        public void Dispose()
        {
            MatchingInputs.Dispose();
        }
    }
}
