using RicoShot.InputActions;
using System;

namespace RicoShot.Matching.Interface
{
    public interface IMatchingSceneManager
    {
        public event Action<MatchingState> OnMatchingStateChanged;

        public MatchingInputs MatchingInputs { get; }
        public MatchingState MatchingState { get; set; }
    }
}