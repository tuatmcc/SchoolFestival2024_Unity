using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RicoShot.Matching.Interface;
using Zenject;
using Unity.Netcode;
using RicoShot.Core.Interface;
using RicoShot.Core;
using System;

namespace RicoShot.Matching
{
    public class MatchingSceneManager : IInitializable
    {
        [Inject] IGameStateManager gameStateManager;

        public void Initialize()
        {

            // Start Server or Client if not started
            if (!NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
            {
                if (gameStateManager.NetworkMode == NetworkMode.Server)
                {
                    NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
                    NetworkManager.Singleton.StartServer();
                }
                else if (gameStateManager.NetworkMode == NetworkMode.Client)
                {
                    NetworkManager.Singleton.StartClient();
                }
            }
        }

        // 接続チェック
        private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            
        }
    }
}
