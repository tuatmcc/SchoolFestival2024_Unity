using System;
using RicoShot.InputActions;

namespace RicoShot.Core.Interface
{
    public interface IGameStateManager
    {
        event Action OnTitleSceneStarted;
        event Action OnMatchingSceneStarted;
        event Action OnPlaySceneStarted;
        event Action OnResultSceneStarted;

        CoreInputs CoreInputs { get; } 
    }
}