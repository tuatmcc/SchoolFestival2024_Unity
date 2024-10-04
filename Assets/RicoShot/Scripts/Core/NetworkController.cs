using RicoShot.Core;
using RicoShot.Core.Interface;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Zenject;
using System;

namespace RicoShot.Core
{
    public class NetworkController : NetworkBehaviour, INetworkController
    {
        public event Action OnClientDatasChanged;
        public NetworkList<ClientData> ClientDatas { get; } = new();

        [SerializeField] private int maxClientCount = 4;

        [Inject] IGameStateManager gameStateManager;
        [Inject] ILocalPlayerManager localPlayerManager;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            ProjectContext.Instance.Container.BindInterfacesTo<NetworkController>().FromInstance(this).AsSingle();
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
                if (gameStateManager.NetworkMode == NetworkMode.Server) // サーバーのとき
                {
                    NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck; // 接続チェック
                    NetworkManager.Singleton.StartServer();
                    Debug.Log("[Server] Server started");
                }
                else if (gameStateManager.NetworkMode == NetworkMode.Client) // クライアントのとき
                {
                    NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected; // 接続時
                    NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect; // 接続解除時
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
            AddClientDataRpc(new ClientData(localPlayerManager.LocalPlayerUUID, clientId));
        }

        // (クライアント)接続解除時の挙動
        private void OnClientDisconnect(ulong clientId)
        {
            Debug.Log($"[Client] Disconnect");
            DeleteClientDataRpc(clientId);
        }

        // (クライアント→サーバー)サーバーにクライアント情報の追加
        [Rpc(SendTo.Server)]
        private void AddClientDataRpc(ClientData clientData)
        {
            ClientDatas.Add(clientData);
            OnClientDatasChanged?.Invoke();
            Debug.Log($"[Server] Registed ClientData: {clientData}");
        }

        // (クライアント→サーバー)クライアントのチーム情報を更新
        [Rpc(SendTo.Server)]
        public void UpdateTeamRpc(Team team, RpcParams rpcParams = default)
        {
            foreach (var clientData in ClientDatas)
            {
                if (clientData.ClientID ==  rpcParams.Receive.SenderClientId)
                {
                    clientData.UpdateTeam(team);
                    Debug.Log($"[Server] Client changed -> {clientData}");
                    break;
                }
            }
            NotifyListChangedRpc();
        }

        // (クライアント→サーバー)クライアント情報を削除
        [Rpc(SendTo.Server)]
        private void DeleteClientDataRpc(ulong clientId)
        {
            foreach (var clientData in ClientDatas)
            {
                if (clientData.ClientID == clientId)
                {
                    Debug.Log($"[Server] Client data delete -> {clientData}");
                    ClientDatas.Remove(clientData);
                    break;
                }
            }
            NotifyListChangedRpc();
        }

        // (サーバー→クライアント)クライアント情報の変更を通知
        [Rpc(SendTo.NotServer)]
        private void NotifyListChangedRpc()
        {
            OnClientDatasChanged?.Invoke();
        }
    }
}
