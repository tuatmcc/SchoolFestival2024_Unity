using RicoShot.Core;
using RicoShot.Core.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace RicoShot.Core
{
    public class NetworkController : NetworkBehaviour, INetworkController
    {
        public NetworkList<ClientData> ClientDatas { get; } = new();

        [SerializeField] private int maxClientCount = 4;

        [Inject] IGameStateManager gameStateManager;
        [Inject] ILocalPlayerManager localPlayerManager;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            ProjectContext.Instance.Container.Inject(this);
        }

        private void Start()
        {
            gameStateManager.OnGameStateChanged += InitializeNetwork;
        }

        // ネットワークの初期化(Matching開始時)
        public void InitializeNetwork(GameState gameState)
        {
            if (gameState != GameState.Matching) return;

            // サーバーまたはクライアントをスタート
            if (!NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
            {
                if (gameStateManager.NetworkMode == NetworkMode.Server)
                {
                    NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
                    NetworkManager.Singleton.StartServer();
                    Debug.Log("[Server] Server started");
                }
                else if (gameStateManager.NetworkMode == NetworkMode.Client)
                {
                    NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
                    NetworkManager.Singleton.StartClient();
                    Debug.Log("[Client] Client started");
                }
            }
        }

        // (サーバー)接続チェック
        private void ApprovalCheck(
            NetworkManager.ConnectionApprovalRequest request,
            NetworkManager.ConnectionApprovalResponse response
            )
        {
            response.Pending = true;

            lock (this) // 排他制御で接続の可否をチェック
            {
                response.Approved = 
                    NetworkManager.Singleton.ConnectedClients.Count < maxClientCount ||
                    gameStateManager.GameState == GameState.Matching;
            }

            Debug.Log($"[Server] Approve client: {response.Approved}");
            response.Pending = false;
        }

        // (クライアント)接続時の挙動
        private void OnClientConnected(ulong clientId)
        {
            Debug.Log($"[Client] Connected server as ID:{clientId}");
            RegistClientRpc(new ClientData(localPlayerManager.LocalPlayerUUID, clientId));
        }

        // (クライアント→サーバー)サーバーに情報の登録を要請
        [Rpc(SendTo.Server)]
        private void RegistClientRpc(ClientData clientData)
        {
            ClientDatas.Add(clientData);
            Debug.Log($"[Server] Registed ClientData: {clientData}");
        }
    }
}
