using RicoShot.Core.Interface;
using RicoShot.InputActions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RicoShot.Core
{
    public class GameStateManager : IGameStateManager
    {

        public event Action OnTitleSceneStarted;
        public event Action OnMatchingSceneStarted;
        public event Action OnPlaySceneStarted;
        public event Action OnResultSceneStarted;
        public CoreInputs CoreInputs => throw new NotImplementedException();

        private GameState gameState
        {
            get { return gameState; }
            set
            {
                gameState = value;
                switch(value)
                {
                    case GameState.Title:
                        OnTitleSceneStarted?.Invoke(); break;
                    case GameState.Matching:
                        OnMatchingSceneStarted?.Invoke(); break;
                    case GameState.Play:
                        OnPlaySceneStarted?.Invoke(); break;
                    case GameState.Result:
                        OnResultSceneStarted?.Invoke(); break;
                }
            }
        }
    }
}
