using Unity.Netcode;
using UnityEngine;

namespace MultiPlayTest.Scripts.RollingBall
{
    public class GeneratePlayerOnStart : NetworkBehaviour
    {
        //プレイヤーのプレハブ
        [SerializeField] private NetworkObject mplayerPrefab;

        int _cnt = 0;

        public override void OnNetworkSpawn()
        {
            //ホスト以外の場合
            if (IsServer == false)
            {
                return;
            }

            //すでに存在するクライアント用に関数呼び出す
            foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
            {
                OnClientConnected(client.ClientId);
            }
        }

        public void OnClientConnected(ulong clientId)
        {
            // プレイヤーオブジェクト生成位置
            var position = new Vector3(-3.26f, 5.07f, 1.69f);
            position.x += 2 * (_cnt++ % 3);

            // プレイヤーオブジェクト生成
            NetworkObject playerObject = Instantiate(mplayerPrefab, position, Quaternion.identity);

            // 接続クライアントをOwnerにしてPlayerObjectとしてスポーン
            playerObject.SpawnAsPlayerObject(clientId);
        }
    }
}