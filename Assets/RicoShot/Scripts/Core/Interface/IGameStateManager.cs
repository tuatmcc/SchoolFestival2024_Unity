using System;
using RicoShot.InputActions;

namespace RicoShot.Core.Interface
{
    public interface IGameStateManager
    {
        public event Action OnTitleSceneStarted;
        public event Action OnMatchingSceneStarted;
        public event Action OnPlaySceneStarted;
        public event Action OnResultSceneStarted;

        public CoreInputs CoreInputs { get; }

        public void NextScene();
    }
}