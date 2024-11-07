using UnityEngine;

namespace RicoShot.Play
{
    public class HitDetector : MonoBehaviour
    {
        // Animatorコンポーネントの参照
        private Animator _animator;

        bool isHit = false;

        void Start()
        {
            // Animatorコンポーネントを取得
            _animator = GetComponent<Animator>();
        }

        // 衝突時に呼び出されるメソッド
        private void OnCollisionEnter(Collision collision)
        {
            // 当たった場合にアニメーションを再生
            if (collision.gameObject.CompareTag("AlphaBullet") || collision.gameObject.CompareTag("BravoBullet"))
            {
                isHit = true;
            }
        }

        void LateUpdate()
        {
            _animator.SetBool("Hit", isHit);
            isHit = false;
        }
    }
}