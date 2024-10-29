using Cysharp.Threading.Tasks;
using RicoShot.Core.Interface;
using RicoShot.InputActions;
using RicoShot.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Zenject;

namespace RicoShot.Core
{
    public class GameStateManager : IGameStateManager, IInitializable, IDisposable
    {
        public event Action<GameState> OnGameStateChanged;
        public event Action OnReset;

        public CoreInputs CoreInputs { get; private set; }
        public NetworkMode NetworkMode { get; set; }
        public GameConfig GameConfig { get; private set; }
        public bool ReadyToReset { private get; set; } = false;

        public GameState GameState
        { 
            get { return gameState; } 
            set
            {
                gameState = value;
                OnGameStateChanged?.Invoke(gameState);
                Debug.Log($"GameState changed to {value}");
            }
        }

        private GameState gameState;

        GameStateManager()
        {
            CoreInputs = new();
            CoreInputs.Enable();
            GameConfig = JsonFileHandler.LoadJson<GameConfig>($"{Application.dataPath}/.env");
        }

        public void Initialize()
        {
            GameState = GameState.ModeSelect;
            OnGameStateChanged += TransitScene;
            CoreInputs.Main.Escape.performed += OnResetInput;
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
                    OnReset?.Invoke();
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
                    Debug.Log("Load Title scene");
                    SceneManager.LoadSceneAsync("Title");
                    break;
                case GameState.Matching:
                    Debug.Log("Load Matching scene");
                    SceneManager.LoadSceneAsync("Matching");
                    break;
                case GameState.Play:
                    if (NetworkMode == NetworkMode.Server)
                    {
                        Debug.Log("[Server] Load Play scene");
                        NetworkManager.Singleton.SceneManager.LoadScene("Play", LoadSceneMode.Single);
                    }
                    break;
                case GameState.Result:
                    if (NetworkMode == NetworkMode.Server)
                    {
                        Debug.Log("[Server] Load Result scene");
                        NetworkManager.Singleton.SceneManager.LoadScene("Result", LoadSceneMode.Single);
                    }
                    break;
            }
        }

        private void OnResetInput(InputAction.CallbackContext context)
        {
            ForceReset();
        }

        public void ForceReset()
        {
            if (GameState == GameState.ModeSelect) return;
            Debug.Log("Start force reset");
            OnReset?.Invoke();
            ReadyToReset = false;
            UniTask.Create(async () =>
            {
                await UniTask.WaitUntil(() => ReadyToReset);
                if (NetworkMode == NetworkMode.Server)
                {
                    GameState = GameState.Matching;
                }
                else if (NetworkMode == NetworkMode.Client)
                {
                    GameState = GameState.Title;
                }
                Debug.Log("Completed force reset");
            }).Forget();
        }

        public void Dispose()
        {
            OnGameStateChanged -= TransitScene;
            CoreInputs.Disable();
            JsonFileHandler.WriteJson($"{Application.dataPath}/.env", GameConfig);
        }
    }
}
