using System;
using Cysharp.Threading.Tasks;
using RicoShot.Core.Interface;
using RicoShot.InputActions;
using RicoShot.Utils;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Zenject;

namespace RicoShot.Core
{
    public class GameStateManager : IGameStateManager, IInitializable, IDisposable
    {
        private GameState gameState;

        private GameStateManager()
        {
            CoreInputs = new CoreInputs();
            CoreInputs.Enable();
            // Check if Application.dataPath is accessible, for example even on Android
            if (Application.dataPath.Contains("Assets"))
                GameConfig = JsonFileHandler.LoadJson<GameConfig>($"{Application.dataPath}/.env");
            else
                GameConfig = JsonFileHandler.LoadJson<GameConfig>($"{Application.persistentDataPath}/.env");
        }

        public void Dispose()
        {
            OnGameStateChanged -= TransitScene;
            CoreInputs.Disable();
            if (Application.dataPath.Contains("Assets"))
                JsonFileHandler.WriteJson($"{Application.dataPath}/.env", GameConfig);
            else
                JsonFileHandler.WriteJson($"{Application.persistentDataPath}/.env", GameConfig);
        }

        public event Action<GameState> OnExitGameState;
        public event Action<GameState> OnGameStateChanged;
        public event Action OnReset;

        public CoreInputs CoreInputs { get; }
        public NetworkMode NetworkMode { get; set; }
        public GameConfig GameConfig { get; }
        public bool ReadyToReset { private get; set; }

        public GameState GameState
        {
            get => gameState;
            set
            {
                OnExitGameState?.Invoke(gameState);
                gameState = value;
                OnGameStateChanged?.Invoke(gameState);
                Debug.Log($"GameState changed to {value}");
            }
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
                    GameState = GameState.Matching;
                else if (NetworkMode == NetworkMode.Client) GameState = GameState.Title;

                Debug.Log("Completed force reset");
            }).Forget();
        }

        public void Initialize()
        {
            GameState = GameState.ModeSelect;
            OnGameStateChanged += TransitScene;
            CoreInputs.Main.Escape.performed += OnResetInput;
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
    }
}