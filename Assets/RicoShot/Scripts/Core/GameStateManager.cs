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

        public GameState GameState
        { 
            get { return gameState; } 
            set
            {
                gameState = value;
                OnGameStateChanged?.Invoke(gameState);
            } 
        }

        private GameState gameState;

        GameStateManager()
        {
            CoreInputs = new();
            CoreInputs.Enable();
        }

        public void Initialize()
        {
            GameState = GameState.ModeSelect;
            OnGameStateChanged += TransitScene;
        }

        public void NextScene()
        {
            switch (GameState)
            {
                case GameState.ModeSelect:
                    switch (NetworkMode)
                    {
                        case NetworkMode.Client:
                            GameState = GameState.Title;
                            break;
                        case NetworkMode.Server:
                            GameState = GameState.Matching;
                            break;
                    }
                    break;
                case GameState.Title:
                    GameState = GameState.Matching;
                    break;
                case GameState.Matching:
                    GameState = GameState.Play;
                    break;
                case GameState.Play:
                    GameState = GameState.Result;
                    break;
                case GameState.Result:
                    switch (NetworkMode)
                    {
                        case NetworkMode.Client:
                            GameState = GameState.Title;
                            break;
                        case NetworkMode.Server:
                            GameState = GameState.Matching;
                            break;
                    }
                    break;
            }
        }

        private void TransitScene(GameState gameState)
        {
            switch (GameState)
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
