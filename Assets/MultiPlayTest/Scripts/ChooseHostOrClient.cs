using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MultiPlayTest.Scripts
{
    public class ChooseHostOrClient : MonoBehaviour
    {
        [SerializeField]
        private string nextScene = "RollingBall";
        public void StartHost()
        {
            // ホスト開始
            NetworkManager.Singleton.StartHost();
            // シーンを切り替え
            NetworkManager.Singleton.SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        }

        public void StartClient()
        {
            // ホストに接続
            NetworkManager.Singleton.StartClient();
        }
    }
}