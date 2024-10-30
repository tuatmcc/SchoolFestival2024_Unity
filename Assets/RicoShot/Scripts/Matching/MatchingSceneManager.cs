using RicoShot.Matching.Interface;
using System;
using RicoShot.InputActions;
using Zenject;

namespace RicoShot.Matching
{
    public class MatchingSceneManager : IMatchingSceneManager, IInitializable, IDisposable
    {
        public event Action<MatchingState> OnMatchingStateChanged;

        public MatchingInputs MatchingInputs { get; private set; }
        public MatchingState MatchingState
        {
            get => _matchingState;
            set
            {
                _matchingState = value;
                OnMatchingStateChanged?.Invoke(value);
            }
        }

        private MatchingState _matchingState;

        public MatchingSceneManager()
        {
            MatchingInputs = new();
            MatchingInputs.Enable();
        }

        public void Initialize()
        {
            MatchingState = MatchingState.Connecting;
        }

        public void Dispose()
        {
            MatchingInputs.Disable();
        }
    }
}
