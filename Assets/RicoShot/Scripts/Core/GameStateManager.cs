using RicoShot.Core.Interface;
using RicoShot.InputActions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RicoShot.Core
{
    public class GameStateManager : IGameStateManager, IInitializable, IDisposable
    {

        public event Action OnTitleSceneStarted;
        public event Action OnMatchingSceneStarted;
        public event Action OnPlaySceneStarted;
        public event Action OnResultSceneStarted;
        public CoreInputs CoreInputs { get; private set; } = null;

        private GameState gameState;

        public void Initialize()
        {
            gameState = GameState.Title;
            CoreInputs = new CoreInputs();
            CoreInputs.Enable();
        }

        public void NextScene()
        {
            switch (gameState)
            {
                case GameState.Title:
                    gameState = GameState.Matching;
                    OnMatchingSceneStarted?.Invoke();
                    break;
                case GameState.Matching:
                    gameState = GameState.Play;
                    OnPlaySceneStarted?.Invoke();
                    break;
                case GameState.Play:
                    gameState = GameState.Result;
                    OnResultSceneStarted?.Invoke();
                    break;
                case GameState.Result:
                    gameState = GameState.Title;
                    OnTitleSceneStarted?.Invoke();
                    break;
            }
        }

        public void Dispose()
        {
            CoreInputs.Dispose();
        }
    }
}
