using System;
using RicoShot.InputActions;
using UnityEngine.InputSystem;

namespace RicoShot.Core.Interface
{
    public interface IGameStateManager
    {
        event Action<GameState> OnExitGameState;
        event Action<GameState> OnGameStateChanged;
        event Action OnReset;

        GameState GameState { get; }

        CoreInputs CoreInputs { get; }
        NetworkMode NetworkMode { get; set; }
        GameConfig GameConfig { get; }
        bool ReadyToReset { set; }

        void NextScene();
        void ForceReset();
    }
}