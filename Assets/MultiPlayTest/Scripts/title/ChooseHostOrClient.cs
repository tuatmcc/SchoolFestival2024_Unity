using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MultiPlayTest.Scripts.Title
{
    public class ChooseHostOrClient : MonoBehaviour
    {
        [SerializeField]
        private string nextScene = "RollingBall";
        public void StartHost()
        {
            // 接続承認コールバック
            NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
            // ホスト開始
            NetworkManager.Singleton.StartHost();
            // シーンを切り替え
            NetworkManager.Singleton.SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        }

        public void StartClient()
        {
            // ホストに接続
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
        [SerializeField]
        private int maxPlayer = 4;
        private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
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
            response.Approved = true;//接続を許可

            //PlayerObjectを生成するかどうか
            response.CreatePlayerObject = false;

            // //生成するPrefabハッシュ値。nullの場合NetworkManagerに登録したプレハブが使用される
            // response.PlayerPrefabHash = null;
            //
            // //PlayerObjectをスポーンする位置(nullの場合Vector3.zero)
            // var position = new Vector3(-3.26f, 5.07f, 1.69f);
            // position.x += 5 * (NetworkManager.Singleton.ConnectedClients.Count % 3);
            // response.Position = position;
            //
            // //PlayerObjectをスポーン時の回転 (nullの場合Quaternion.identity)
            // response.Rotation = Quaternion.identity;

            response.Pending = false;
        }
    }
}