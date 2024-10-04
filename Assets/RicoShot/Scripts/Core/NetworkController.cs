using RicoShot.Core;
using RicoShot.Core.Interface;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Zenject;
using System;
using UnityEngine.SceneManagement;

namespace RicoShot.Core
{
    public class NetworkController : NetworkBehaviour, INetworkController
    {
        // 説明はインターフェース(INetworkManager)を参照
        public event Action OnClientDatasChanged;
        public event Action OnTeamChanged;
        public event Action OnReadyStatusChanged;
        public event Action OnAllClientsReady;
        public event Action OnAllClientsReadyCancelled;
        public NetworkList<ClientData> ClientDatas { get; } = new();
        public NetworkVariable<bool> AllClientsReady { get; } = new NetworkVariable<bool>(false);

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

            response.Approved = 
                NetworkManager.Singleton.ConnectedClients.Count < maxClientCount &&
                gameStateManager.GameState == GameState.Matching;

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
            Debug.Log($"[Server] Registed ClientData: {clientData}");
            NotifyClientDataChangedRpc();
        }

        // (クライアント→サーバー)クライアントのチーム情報を更新
        [Rpc(SendTo.Server)]
        public void UpdateTeamRpc(Team team, RpcParams rpcParams = default)
        {
            var clientData = GetClientDataFromClientID(rpcParams.Receive.SenderClientId);
            clientData.UpdateTeam(team);
            Debug.Log($"[Server] Client data changed -> {clientData}");
            NotifyClientTeamChangedRpc();
        }
        
        // (クライアント→サーバー)クライアントのReady情報を更新
        [Rpc(SendTo.Server)]
        public void UpdateReadyStatusRpc(bool isReady, RpcParams rpcParams = default)
        {
            var clientData = GetClientDataFromClientID(rpcParams.Receive.SenderClientId);
            clientData.UpdateReadyStatus(isReady);
            Debug.Log($"[Server] Client ready status changed -> ID: {clientData.ClientID}, IsReady: {clientData.IsReady}");
            NotifyReadyStatusChangedRpc();

            bool allReady = true;
            foreach (var data in ClientDatas)
            {
                if (!data.IsReady)
                {
                    allReady = false;
                    break;
                }
            }
            if (allReady)
            {
                AllClientsReady.Value = true;
                OnAllClientsReady?.Invoke();
            }
            else if (!allReady && AllClientsReady.Value)
            {
                AllClientsReady.Value = false;
                OnAllClientsReadyCancelled?.Invoke();
            }
        }

        // (クライアント→サーバー)クライアント情報を削除
        [Rpc(SendTo.Server)]
        private void DeleteClientDataRpc(ulong clientId)
        {
            var clientData = GetClientDataFromClientID(clientId);
            ClientDatas.Remove(clientData);
            Debug.Log($"[Server] Client data delete -> {clientData}");
            NotifyClientDataChangedRpc();
        }

        // (クライアント→サーバー)サーバーにプレイ開始を要求
        [Rpc(SendTo.Server)]
        public void StartPlayRpc()
        {
            NetworkManager.Singleton.SceneManager.LoadScene("Play", LoadSceneMode.Single);
        }

        // (サーバー→クライアント)クライアント情報の変更を通知
        [Rpc(SendTo.NotServer)]
        private void NotifyClientDataChangedRpc()
        {
            OnClientDatasChanged?.Invoke();
            OnTeamChanged?.Invoke();
            OnReadyStatusChanged?.Invoke();
        }

        // (サーバー→クライアント)クライアントのチーム変更を通知
        [Rpc(SendTo.NotServer)]
        private void NotifyClientTeamChangedRpc()
        {
            OnClientDatasChanged?.Invoke();
            OnTeamChanged?.Invoke();
        }

        // (サーバー→クライアント)クライアントのReady状態の変更を通知
        [Rpc(SendTo.NotServer)]
        private void NotifyReadyStatusChangedRpc()
        {
            OnClientDatasChanged?.Invoke();
            OnReadyStatusChanged?.Invoke();
        }

        private ClientData GetClientDataFromClientID(ulong clientID)
        {
            foreach (var clientData in ClientDatas)
            {
                if (clientData.ClientID == clientID) return clientData;
            }
            throw new InvalidOperationException();
        }
    }
}
