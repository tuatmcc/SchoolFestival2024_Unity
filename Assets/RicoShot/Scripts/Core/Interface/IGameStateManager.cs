using System;
using RicoShot.InputActions;

namespace RicoShot.Core.Interface
{
    public interface IGameStateManager
    {
        event Action<GameState> OnGameStateChanged;

        GameState GameState { get; }

        CoreInputs CoreInputs { get; }
        NetworkMode NetworkMode { get; set; }

        void NextScene();
    }
}