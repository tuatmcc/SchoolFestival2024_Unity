using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace MultiPlayTest.Scripts.Lobby
{
    public class Ready : NetworkBehaviour
    {
        // 準備完了ボタンとボタンのテキスト
        [SerializeField] private Button readyButton;
        [SerializeField] private TextMeshProUGUI buttonText;

        // 次のシーン名
        [SerializeField] private string nextScene = "RollingBall";

        // プレイヤーごとの準備完了ステータスを保持する辞書
        private readonly Dictionary<ulong, bool> _playerReadyStatus = new Dictionary<ulong, bool>();

        // 準備完了したプレイヤーの数をネットワーク変数で保持
        private readonly NetworkVariable<int> _readyPlayerCount = new NetworkVariable<int>();

        private void Start()
        {
            // ボタンが設定されている場合、クリックイベントを追加し、初期状態のテキストを設定
            if (readyButton != null)
            {
                readyButton.onClick.AddListener(OnReadyButtonClicked);
                UpdateButtonText(false); // 初期状態は「Unready」
            }
        }

        // 準備完了ボタンがクリックされた時に呼び出される
        private void OnReadyButtonClicked()
        {
            // サーバーに準備状態の切り替えを通知
            ToggleReadyServerRpc();
        }

        // ボタンのテキストと色を更新するメソッド
        private void UpdateButtonText(bool isReady)
        {
            if (buttonText != null)
            {
                // 準備完了かどうかでテキストを変更
                buttonText.text = isReady ? "Ready" : "Unready";
            }

            if (readyButton != null)
            {
                Image buttonImage = readyButton.GetComponent<Image>();
                if (buttonImage != null)
                {
                    // ボタンの色を準備完了で緑、未完了で赤に設定
                    buttonImage.color = isReady ? Color.green : Color.red;
                }
            }
        }

        // サーバーに準備状態の切り替えを通知するServerRpc
        [ServerRpc(RequireOwnership = false)]
        private void ToggleReadyServerRpc(ServerRpcParams serverRpcParams = default)
        {
            // プレイヤーのClientIdを取得
            var clientId = serverRpcParams.Receive.SenderClientId;

            // 現在のステータスを反転（準備未完了 -> 準備完了 または 準備完了 -> 準備未完了）
            bool currentStatus = !_playerReadyStatus.ContainsKey(clientId) || !_playerReadyStatus[clientId];

            // 新しいステータスを保存
            _playerReadyStatus[clientId] = currentStatus;

            // クライアント側にステータスの変更を通知
            UpdateReadyStatusClientRpc(clientId, currentStatus);

            // 準備完了したプレイヤーの数をカウント
            int readyCount = 0;
            foreach (var status in _playerReadyStatus.Values)
            {
                if (status) readyCount++;
            }

            // ネットワーク変数に準備完了プレイヤー数を反映
            _readyPlayerCount.Value = readyCount;

            // 全プレイヤーが準備完了か確認
            CheckAllPlayersReady();
        }

        // クライアント側でプレイヤーの準備状態を更新するClientRpc
        [ClientRpc]
        private void UpdateReadyStatusClientRpc(ulong clientId, bool isReady)
        {
            // プレイヤーのステータスを更新
            _playerReadyStatus[clientId] = isReady;

            // ローカルプレイヤーのボタン表示を更新
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                UpdateButtonText(isReady);
            }
        }

        // 全プレイヤーが準備完了かチェックし、完了ならゲームを開始
        private void CheckAllPlayersReady()
        {
            if (!IsServer) return;

            if (_readyPlayerCount.Value == NetworkManager.Singleton.ConnectedClients.Count)
            {
                NetworkManager.Singleton.SceneManager.LoadScene(nextScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
        }

        // ネットワーク生成時に呼び出される
        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                // クライアントが接続/切断された際のコールバックを設定
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
            }
        }

        // クライアントが接続された時に呼び出される
        private void OnClientConnected(ulong clientId)
        {
            // 新しいプレイヤーは最初は準備未完了状態とする
            _playerReadyStatus[clientId] = false;
        }

        // クライアントが切断された時に呼び出される
        private void OnClientDisconnected(ulong clientId)
        {
            // 切断されたプレイヤーのステータスを削除
            if (_playerReadyStatus.ContainsKey(clientId))
            {
                _playerReadyStatus.Remove(clientId);
                CheckAllPlayersReady(); // 再度全員が準備完了かを確認
            }
        }
    }
}