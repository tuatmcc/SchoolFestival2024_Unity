using System;
using RicoShot.InputActions;

namespace RicoShot.Core.Interface
{
    public interface IGameStateManager
    {
        event Action<GameState> OnGameStateChanged;

        CoreInputs CoreInputs { get; }

        void NextScene();
    }
}