using System;
using RicoShot.InputActions;

namespace RicoShot.Core.Interface
{
    public interface IGameStateManager
    {
        event Action<GameState> OnGameStateChanged;
        event Action OnReset;

        GameState GameState { get; }

        CoreInputs CoreInputs { get; }
        NetworkMode NetworkMode { get; set; }
        bool ReadyToReset { set; }

        void NextScene();
    }
}