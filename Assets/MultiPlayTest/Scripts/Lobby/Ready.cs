using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

namespace MultiPlayTest.Scripts.Lobby
{
    public class Ready : MonoBehaviour
    {
        // ボタンがクリックされたときに呼び出されるメソッド
        public void OnLoadSceneButtonClicked()
        {
            // RollingBallシーンをロード
            NetworkManager.Singleton.SceneManager.LoadScene("RollingBall", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }
}
