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
using Cysharp.Threading.Tasks;

namespace RicoShot.Core
{
    public class NetworkController : NetworkBehaviour, INetworkController
    {
        // 説明はインターフェース(INetworkManager)を参照
        public event Action<bool> OnAllClientsReadyChanged;
        public NetworkClassList<ClientData> ClientDatas { get; private set; } = new();
        public NetworkVariable<bool> AllClientsReady { get; } = new NetworkVariable<bool>(false);

        [SerializeField] private int maxClientCount = 4;
        private bool callbackRegisted = false;

        [Inject] private readonly IGameStateManager gameStateManager;
        [Inject] private readonly ILocalPlayerManager localPlayerManager;

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
        private void InitializeNetwork(GameState gameState)
        {
            if (gameState != GameState.Matching) return;

            // サーバーまたはクライアントのコールバックを登録
            if (!callbackRegisted)
            {
                if (gameStateManager.NetworkMode == NetworkMode.Server) // サーバーのとき
                {
                    NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck; // 接続チェック
                    NetworkManager.Singleton.OnClientConnectedCallback += ResetAllReadyState; // 接続時
                    NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect; // 接続解除時
                }
                else if (gameStateManager.NetworkMode == NetworkMode.Client) // クライアントのとき
                {
                    NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected; // 接続時
                }
                gameStateManager.OnReset += ResetNetwork;
                callbackRegisted = true;
            }


            // サーバーまたはクライアントのスタート
            if (gameStateManager.NetworkMode == NetworkMode.Server)
            {
                NetworkManager.Singleton.StartServer();
                ClientDatas.Initialize(this);
                AllClientsReady.Initialize(this);
                Debug.Log("[Server] Server started");
            }
            else if (gameStateManager.NetworkMode == NetworkMode.Client)
            {
                NetworkManager.Singleton.StartClient();
                Debug.Log("[Client] Client started");
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

        // (サーバー)接続解除時の挙動
        private void OnClientDisconnect(ulong clientId)
        {
            var clientData = GetClientDataFromClientID(clientId);
            ClientDatas.Remove(clientData);
            Debug.Log($"[Server] Disconnected -> ID: {clientId}");

            CheckAllReadyAndNotify();
        }

        // (サーバー)AllReadyをリセットする関数
        private void ResetAllReadyState(ulong clientId)
        {
            AllClientsReady.Value = false;
            ReadyStatusChangedRpc(false);
        }

        // (クライアント)接続時の挙動
        private void OnClientConnected(ulong clientId)
        {
            Debug.Log($"[Client] Connected server as ID:{clientId}");
            AddClientDataRpc(new ClientData(localPlayerManager.LocalPlayerUUID, clientId));
        }

        // (クライアント→サーバー)サーバーにクライアント情報の追加
        [Rpc(SendTo.Server)]
        private void AddClientDataRpc(ClientData clientData)
        {
            ClientDatas.Add(clientData);
            Debug.Log($"[Server] Registed ClientData: {clientData}");
        }

        // (クライアント→サーバー)クライアントのチーム情報を更新
        [Rpc(SendTo.Server)]
        public void UpdateTeamRpc(Team team, RpcParams rpcParams = default)
        {
            var clientData = GetClientDataFromClientID(rpcParams.Receive.SenderClientId);
            if (clientData.Team == team || clientData.IsReady) return;
            clientData.SetTeam(team);
            Debug.Log($"[Server] Client data changed -> {clientData}");
        }
        
        // (クライアント→サーバー)クライアントのReady情報を更新
        [Rpc(SendTo.Server)]
        public void UpdateReadyStatusRpc(bool isReady, RpcParams rpcParams = default)
        {
            var clientData = GetClientDataFromClientID(rpcParams.Receive.SenderClientId);
            if (clientData.IsReady == isReady) return;
            clientData.SetReadyStatus(isReady);
            Debug.Log($"[Server] Client ready status changed -> ID: {clientData.ClientID}, IsReady: {clientData.IsReady}");

            CheckAllReadyAndNotify();
        }

        // (クライアント→サーバー)サーバーにプレイ開始を要求
        [Rpc(SendTo.Server)]
        public void StartPlayRpc()
        {
            // 入れ違いで全員がReadyになっていなければ何もしない
            if (AllClientsReady.Value && gameStateManager.GameState == GameState.Matching)
            {
                gameStateManager.NextScene();
            }
        }

        // リセット用メソッド
        private void ResetNetwork()
        {
            if (gameStateManager.NetworkMode == NetworkMode.Client)
            {
                UniTask.Create(async () =>
                {
                    NetworkManager.Singleton.Shutdown();
                    await UniTask.WaitUntil(() => !NetworkManager.Singleton.IsClient);
                    ClientDatas.Clear();
                    gameStateManager.ReadyToReset = true;
                    Debug.Log($"Shutdown Client completed");
                }).Forget();
            }
            else if (gameStateManager.NetworkMode == NetworkMode.Server)
            {
                DisconnectClientRpc();
                UniTask.Create(async () =>
                {
                    await UniTask.WaitUntil(() => NetworkManager.Singleton.ConnectedClientsList.Count == 0);
                    NetworkManager.Singleton.Shutdown();
                    await UniTask.WaitUntil(() => !NetworkManager.Singleton.IsServer);
                    ClientDatas.Clear();
                    gameStateManager.ReadyToReset = true;
                    Debug.Log($"Shutdown Server completed");
                }).Forget();
            }
        }

        // (サーバー→クライアント)サーバーのシャットダウンを通知
        [Rpc(SendTo.NotServer)]
        private void DisconnectClientRpc()
        {
            gameStateManager.ForceReset();
        }

        // (サーバー)すべてのクライアントがReadyか確認して全体に通知する関数
        private void CheckAllReadyAndNotify()
        {
            bool allReady = true;
            foreach (var data in ClientDatas)
            {
                if (!data.IsReady)
                {
                    allReady = false;
                    break;
                }
            }
            if (allReady == AllClientsReady.Value) return;
            AllClientsReady.Value = allReady;
            ReadyStatusChangedRpc(allReady);
        }

        // (サーバー→全体)全員がReady状態になったことを通知
        [Rpc(SendTo.Everyone)]
        private void ReadyStatusChangedRpc(bool allReady)
        {
            OnAllClientsReadyChanged?.Invoke(allReady);
        }

        // ClientIDを基にClientDataを返す
        private ClientData GetClientDataFromClientID(ulong clientID)
        {
            foreach (var clientData in ClientDatas)
            {
                if(clientData.ClientID == clientID) return clientData;
            }
            Debug.Log($"ID: {clientID} not found");
            throw new InvalidOperationException();
        }
    }
}
