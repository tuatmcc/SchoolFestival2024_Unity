using RicoShot.Matching.Interface;
using System;
using RicoShot.InputActions;

namespace RicoShot.Matching
{
    public class MatchingSceneManager : IMatchingSceneManager, IDisposable
    {
        public MatchingInputs MatchingInputs { get; private set; }

        MatchingSceneManager()
        {
            MatchingInputs = new();
            MatchingInputs.Enable();
        }

        public void Dispose()
        {
            MatchingInputs.Dispose();
        }
    }
}
