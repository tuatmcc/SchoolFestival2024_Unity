using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using RicoShot.Play.Interface;
using RicoShot.Utils;
using Supabase.Storage;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using Zenject;
namespace RicoShot.Play
{
    public class BulletController : NetworkBehaviour
    {
        [SerializeField] private int max_reflect_num = 3;
        [SerializeField] private int bulletForce = 20;
        private Vector3 velocity;
        private Rigidbody rb;
        private NetworkTransform networkTransform;
        private Vector3 normal;
        private int reflect_count = 0;

        [Inject] private readonly IPlaySceneManager playSceneManager;

        void Start()
        {
            GetComponent<Renderer>().enabled = false;
            gameObject.AddComponent<ZenAutoInjecter>();
            rb = this.GetComponent<Rigidbody>();
            SpawnBullet().Forget();
        }

        // Spawnを待ってBulletをセット
        private async UniTask SpawnBullet()
        {
            await UniTask.WaitUntil(() => playSceneManager != null && IsSpawned, cancellationToken: destroyCancellationToken);
            if (IsOwner)
            {
                var localPlayerTransform = playSceneManager.LocalPlayer.transform;
                transform.position = localPlayerTransform.position + Vector3.up * 0.5f;
                rb.AddForce(localPlayerTransform.forward * bulletForce, ForceMode.Impulse);
                Debug.Log("Shoted");
            }
            GetComponent<Renderer>().enabled = true;
        }

        private void FixedUpdate()
        {
            velocity = rb.velocity;
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (IsOwner)
            {
                Debug.Log("衝突");
                if (collision.gameObject.CompareTag("Border"))
                {
                    if (velocity.magnitude <= 0.1)
                    {
                        rb.velocity = new Vector3(0, 0, 0);
                        this.transform.position = new Vector3(0, -0.4f, 0);
                        reflect_count = 0;
                    }
                    reflect_count++;
                    normal = collision.contacts[0].normal;

                    Vector3 result = Vector3.Reflect(velocity, normal);

                    rb.velocity = result;

                    // directionの更新
                    velocity = rb.velocity;
                }
                else if (collision.gameObject.CompareTag("Enemy"))
                {
                    Debug.Log("敵にヒット");
                    rb.velocity = new Vector3(0, 0, 0);
                    this.transform.position = new Vector3(0, -0.4f, 0);
                    reflect_count = 0;
                    //scoreManager.AddScore(100);
                    DestroyThisRpc();
                }
                if (reflect_count >= max_reflect_num + 1)
                {
                    rb.velocity = new Vector3(0, 0, 0);
                    this.transform.position = new Vector3(0, -0.4f, 0);
                    reflect_count = 0;
                    DestroyThisRpc();
                }
            }
        }

        // (クライアント→サーバー)このBulletの削除をする関数
        [Rpc(SendTo.Server)]
        private void DestroyThisRpc()
        {
            Destroy(gameObject);
        }    
    }

}