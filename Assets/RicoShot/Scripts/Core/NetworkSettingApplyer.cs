using RicoShot.Core.Interface;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using Zenject;

namespace RicoShot.Core
{
    public class NetworkSettingApplyer : MonoBehaviour
    {
        [Inject] private readonly IGameStateManager gameStateManager;

        private void Start()
        {
            gameStateManager.OnExitGameState += ApplyNetworkSettings;
        }

        private void ApplyNetworkSettings(GameState gameState)
        {
            if (gameState == GameState.ModeSelect)
            {
                var unityTransport = GetComponent<UnityTransport>();
                Debug.Log(gameStateManager.GameConfig.ServerIPAddress);
                Debug.Log(gameStateManager.GameConfig.ServerPort);
                unityTransport.ConnectionData.Address = gameStateManager.GameConfig.ServerIPAddress;
                unityTransport.ConnectionData.Port = gameStateManager.GameConfig.ServerPort;
            }
        }
    }
}
