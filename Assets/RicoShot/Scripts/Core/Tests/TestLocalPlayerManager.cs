using RicoShot.Core.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RicoShot.Core.Tests
{
    public class TestLocalPlayerManager : ILocalPlayerManager, IInitializable
    {
        public string LocalPlayerUUID { get => localPlayerUUID; set { } }
        public string LocalPlayerName { get => localPlayerName; set { } }
        public CharacterParams CharacterParams { get => characterParams; set { } }

        private readonly string localPlayerUUID = Guid.NewGuid().ToString();
        private readonly string localPlayerName = "Test Player";
        private readonly CharacterParams characterParams = CharacterParams.GetRandomCharacterParams();

        [Inject] private readonly IGameStateManager gameStateManager;

        public void Initialize()
        {
            gameStateManager.OnGameStateChanged += ResetCharacterParams;
        }

        private void ResetCharacterParams(GameState gameState)
        {
            if(gameState == GameState.Title)
            {
                CharacterParams = CharacterParams.GetRandomCharacterParams();
            }
        }
    }
}
