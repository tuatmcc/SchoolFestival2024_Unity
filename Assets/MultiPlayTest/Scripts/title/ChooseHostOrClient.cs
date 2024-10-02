using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MultiPlayTest.Scripts.Title
{
    public class ChooseHostOrClient : MonoBehaviour
    {
        [SerializeField] private string nextScene;

        public void StartServer()
        {
            // 接続承認コールバック
            NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
            // ホスト開始
            NetworkManager.Singleton.StartServer();
            // シーンを切り替え
            NetworkManager.Singleton.SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        }

        public void StartClient()
        {
            // サーバーに接続
            bool result = NetworkManager.Singleton.StartClient();
            if (result)
            {
                Debug.Log("接続成功");
            }
            else
            {
                Debug.Log("接続失敗");
            }
        }

        /// 接続承認関数
        [SerializeField] private int maxPlayer = 4;

        private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request,
            NetworkManager.ConnectionApprovalResponse response)
        {
            // true から false にすると、接続承認応答が処理される
            response.Pending = true;

            //最大人数をチェック
            if (NetworkManager.Singleton.ConnectedClients.Count >= maxPlayer)
            {
                response.Approved = false; // 接続を許可しない
                response.Pending = false; // 接続承認応答を処理する
                return;
            }

            //ここからは接続成功クライアントに向けた処理
            response.Approved = true; //接続を許可

            //PlayerObjectを生成するかどうか
            response.CreatePlayerObject = false;

            response.Pending = false;
        }
    }
}