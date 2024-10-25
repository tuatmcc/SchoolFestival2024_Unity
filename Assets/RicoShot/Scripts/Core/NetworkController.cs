using RicoShot.Core;
using RicoShot.Core.Interface;
using Unity.Netcode;
using UnityEngine;
using Zenject;
using System;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

namespace RicoShot.Core
{
    /// <summary>
    /// ネットワークのマッチング部分を司る
    /// </summary>
    public class NetworkController : NetworkBehaviour, INetworkController
    {
        // 説明はインターフェース(INetworkManager)を参照
        public event Action<bool> OnAllClientsReadyChanged;
        public NetworkClassList<ClientData> ClientDataList { get; private set; } = new();
        public NetworkVariable<bool> AllClientsReady { get; } = new NetworkVariable<bool>(false);

        [SerializeField] private int maxClientCount = 4;
        private bool callbackRegisted = false;

        [Inject] private readonly IGameStateManager gameStateManager;
        [Inject] private readonly ILocalPlayerManager localPlayerManager;

        private void Awake()
        {
            // 開始時にDontDestroyOnLoadへ
            DontDestroyOnLoad(gameObject);
            // ここでProjectContextに加えてInjectする(Title以降のシーンでInject可)
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
                    NetworkManager.Singleton.OnClientDisconnectCallback += OnConnectionFailed; // 接続失敗時
                }
                // リセット時に呼び出すメソッドを登録
                gameStateManager.OnReset += ResetNetwork;
                callbackRegisted = true;
            }


            // サーバーまたはクライアントのスタート
            if (gameStateManager.NetworkMode == NetworkMode.Server)
            {
                NetworkManager.Singleton.StartServer();
                // NetworkValiableを初期化(再利用するため)
                ClientDataList.Initialize(this);
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

            if (gameStateManager.GameState != GameState.Matching)
            {
                response.Approved = false;
                response.Reason = $"Server is not Matching. Now: {gameStateManager.GameState}";
            }
            else if (NetworkManager.Singleton.ConnectedClients.Count >= maxClientCount)
            {
                response.Approved = false;
                response.Reason = $"Client count reached maxClientCount: {maxClientCount}";
            }
            else
            {
                response.Approved = true;
            }

            Debug.Log($"[Server] Approve client: {response.Approved}");
            response.Pending = false;
        }

        // (サーバー)接続解除時の挙動
        private void OnClientDisconnect(ulong clientId)
        {
            var clientData = GetClientDataFromClientID(clientId);
            // 接続失敗のときclientDataはnull
            if (clientData != null)
            {
                ClientDataList.Remove(clientData);
            }
            Debug.Log($"[Server] Disconnected -> ID: {clientId}");

            if (gameStateManager.GameState == GameState.Matching)
            {
                CheckAllReadyAndNotify();
            }
        }

        // (サーバー)クライアントの新規接続時にAllReadyをリセットする関数
        private void ResetAllReadyState(ulong clientId)
        {
            AllClientsReady.Value = false;
            ReadyStatusChangedRpc(false);
        }

        // (クライアント)接続時の挙動
        private void OnClientConnected(ulong clientId)
        {
            Debug.Log($"[Client] Connected server as ID:{clientId}");
            // 自身のデータを登録
            AddClientDataRpc(new ClientData(localPlayerManager.LocalPlayerUUID, clientId, localPlayerManager.CharacterParams));
        }

         // (クライアント)接続解除時の挙動
        private void OnConnectionFailed(ulong clientId)
        {
            // 接続失敗時に自動でリトライ
            if (gameStateManager.GameState == GameState.Matching)
            {
                Debug.Log($"[Client] Connection failed because: {NetworkManager.Singleton.DisconnectReason}");
                Debug.Log($"[Client] Retry after 5 seconds");
                // 5秒待ってリトライ
                UniTask.Create(async () =>
                {
                    await UniTask.WaitForSeconds(5, cancellationToken: destroyCancellationToken);
                    InitializeNetwork(gameStateManager.GameState);
                }).Forget();
            }
        }

        // (クライアント→サーバー)サーバーにクライアント情報の追加
        [Rpc(SendTo.Server)]
        private void AddClientDataRpc(ClientData clientData)
        {
            ClientDataList.Add(clientData);
            Debug.Log($"[Server] Registed ClientData: {clientData}");
        }

        // (クライアント→サーバー)クライアントのチーム情報を更新
        [Rpc(SendTo.Server)]
        public void UpdateTeamRpc(Team team, RpcParams rpcParams = default)
        {
            var clientData = GetClientDataFromClientID(rpcParams.Receive.SenderClientId);
            // チームが変わっていないかすでにReady状態のときは何もしない
            if (clientData.Team == team || clientData.IsReady) return;
            clientData.Team = team;
            Debug.Log($"[Server] Client data changed -> {clientData}");
        }
        
        // (クライアント→サーバー)クライアントのReady情報を更新
        [Rpc(SendTo.Server)]
        public void UpdateReadyStatusRpc(bool isReady, RpcParams rpcParams = default)
        {
            var clientData = GetClientDataFromClientID(rpcParams.Receive.SenderClientId);
            if (clientData.IsReady == isReady) return;
            clientData.IsReady = isReady;
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
                ChangeGameStateRpc();
            }
        }

        // (サーバー→クライアント)クライアントのGameStateをPlayに変更
        [Rpc(SendTo.NotServer)]
        private void ChangeGameStateRpc()
        {
            gameStateManager.NextScene();
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
                    ClientDataList.Clear();
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
                    ClientDataList.Clear();
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
            foreach (var data in ClientDataList)
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

        // (サーバー→全体)全員がReady状態になったことを通知する関数
        [Rpc(SendTo.Everyone)]
        private void ReadyStatusChangedRpc(bool allReady)
        {
            OnAllClientsReadyChanged?.Invoke(allReady);
        }

        // ClientIDを基にClientDataを返す関数
        private ClientData GetClientDataFromClientID(ulong clientID)
        {
            foreach (var clientData in ClientDataList)
            {
                if(clientData.ClientID == clientID) return clientData;
            }
            return null;
        }
    }
}
