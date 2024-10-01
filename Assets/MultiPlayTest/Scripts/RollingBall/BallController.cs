using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace MultiPlayTest.Scripts.RollingBall
{
    // NetworkBehaviour を継承することで，ネットワーク関連の機能を使えるようになる
    public class BallController : NetworkBehaviour
    {
        [SerializeField] private float moveSpeed = 1;

        private Rigidbody _rigidBody;
        private Vector2 _moveInput = Vector2.zero;

        void Start()
        {
            // Rigidbody を取得
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            // このオブジェクトのオーナーである場合
            if (IsOwner)
            {
                // 移動入力を設定
                SetMoveInputServerRpc(
                    Input.GetAxisRaw("Horizontal"),
                    Input.GetAxisRaw("Vertical")
                );
            }

            //サーバー（ホスト）の場合
            if (IsServer)
            {
                ServerUpdate();
            }
        }

        // =================================================================
        // RPC
        // =================================================================
        // 移動入力をセットするRPC
        [ServerRpc]
        private void SetMoveInputServerRpc(float x, float z)
        {
            _moveInput = new Vector2(x, z);
        }

        // =================================================================
        // サーバー側で行う処理
        // =================================================================
        // サーバーだけで呼び出すUpdate
        private void ServerUpdate()
        {
            // _moveInputから速度を計算
            var velocity = Vector3.zero;
            velocity.x = moveSpeed * _moveInput.normalized.x;
            velocity.y = _rigidBody.velocity.y;
            velocity.z = moveSpeed * _moveInput.normalized.y;
            //移動処理
            _rigidBody.AddForce(velocity);
        }
    }
}