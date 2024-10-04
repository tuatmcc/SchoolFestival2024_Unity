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
            }
        }

        private void OnSelectLeft(InputAction.CallbackContext context)
        {
            networkController.UpdateTeamRpc(Team.Alpha);
        }

        private void OnSelectRight(InputAction.CallbackContext obj)
        {
            networkController.UpdateTeamRpc(Team.Bravo);
        }

        public void Dispose()
        {
            MatchingInputs.Dispose();
        }
    }
}
