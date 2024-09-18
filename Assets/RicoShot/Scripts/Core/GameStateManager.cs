using RicoShot.Core.Interface;
using RicoShot.InputActions;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace RicoShot.Core
{
    public class GameStateManager : IGameStateManager, IInitializable, IDisposable
    {

        public event Action<GameState> OnGameStateChanged;
        public CoreInputs CoreInputs { get; private set; } = null;

        private GameState gameState;

        public void Initialize()
        {
            gameState = GameState.Title;
            CoreInputs = new();
            CoreInputs.Enable();
        }

        public void NextScene()
        {
            switch (gameState)
            {
                case GameState.Title:
                    gameState = GameState.Matching;
                    OnGameStateChanged?.Invoke(gameState);
                    break;
                case GameState.Matching:
                    gameState = GameState.Play;
                    OnGameStateChanged?.Invoke(gameState);
                    break;
                case GameState.Play:
                    gameState = GameState.Result;
                    OnGameStateChanged?.Invoke(gameState);
                    break;
                case GameState.Result:
                    gameState = GameState.Title;
                    OnGameStateChanged?.Invoke(gameState);
                    break;
            }
        }

        public void Dispose()
        {
            CoreInputs.Dispose();
        }
    }
}
