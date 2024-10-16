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
        public TitleInputs TitleInputs { get; private set; }

        [Inject] IGameStateManager gameStateManager;

        TestTitleSceneManager()
        {
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
            TitleInputs.Dispose();
        }
    }
}
