using RicoShot.Core.Interface;
using RicoShot.InputActions;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace RicoShot.Core
{
    public class GameStateManager : IGameStateManager, IInitializable, IDisposable
    {

        public event Action<GameState> OnGameStateChanged;

        public CoreInputs CoreInputs { get; private set; }
        public NetworkMode NetworkMode { get; set; }

        private GameState gameState;

        GameStateManager()
        {
            CoreInputs = new();
            CoreInputs.Enable();
        }

        public void Initialize()
        {
            gameState = GameState.ModeSelect;
            OnGameStateChanged += TransitScene;
        }

        public void NextScene()
        {
            switch (gameState)
            {
                case GameState.ModeSelect:
                    switch (NetworkMode)
                    {
                        case NetworkMode.Client:
                            gameState = GameState.Title;
                            break;
                        case NetworkMode.Server:
                            gameState = GameState.Matching;
                            break;
                    }
                    OnGameStateChanged?.Invoke(gameState);
                    break;
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
                    switch (NetworkMode)
                    {
                        case NetworkMode.Client:
                            gameState = GameState.Title;
                            break;
                        case NetworkMode.Server:
                            gameState = GameState.Matching;
                            break;
                    }
                    OnGameStateChanged?.Invoke(gameState);
                    break;
            }
        }

        private void TransitScene(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Title:
                    SceneManager.LoadSceneAsync("Title");
                    break;
                case GameState.Matching:
                    SceneManager.LoadSceneAsync("Matching");
                    break;
                case GameState.Play:
                    if (NetworkMode == NetworkMode.Server)
                    {
                        NetworkManager.Singleton.SceneManager.LoadScene("Play", LoadSceneMode.Single);
                    }
                    break;
                case GameState.Result:
                    if (NetworkMode == NetworkMode.Server)
                    {
                        NetworkManager.Singleton.SceneManager.LoadScene("Result", LoadSceneMode.Single);
                    }
                    break;
            }
        }

        public void Dispose()
        {
            OnGameStateChanged -= TransitScene;
            CoreInputs.Dispose();
        }
    }
}
