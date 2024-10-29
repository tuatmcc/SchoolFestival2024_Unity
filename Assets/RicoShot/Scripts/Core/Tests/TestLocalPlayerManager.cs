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
        public string LocalPlayerUUID { get; set; } = Guid.NewGuid().ToString();

        public CharacterParams CharacterParams { get; set; } = CharacterParams.GetRandomCharacterParams();

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
