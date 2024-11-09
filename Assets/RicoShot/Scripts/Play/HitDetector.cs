using RicoShot.Play.Interface;
using Unity.Netcode;
using UnityEngine;

namespace RicoShot.Play
{
    public class HitDetector : NetworkBehaviour
    {
        // Animatorコンポーネントの参照
        private Animator _animator;
        private IClientDataHolder _clientDataHolder;

        bool isHit = false;

        void Start()
        {
            // Animatorコンポーネントを取得
            _animator = GetComponent<Animator>();
        }

        [Rpc(SendTo.Everyone)]
        // 衝突時に呼び出されるメソッド
        public void SetAnimationFlagRpc()
        {
           isHit = true;
        }

        void LateUpdate()
        {
            _animator.SetBool("Hit", isHit);
            isHit = false;
        }
    }
}