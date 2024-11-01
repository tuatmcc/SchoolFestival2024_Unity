using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using RicoShot.Play.Interface;
using RicoShot.Utils;
using Supabase.Storage;
using Unity.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using Zenject;
namespace RicoShot.Play
{
    public class BulletController : NetworkBehaviour
    {
        [SerializeField] private int score = 10;
        [SerializeField] private int max_reflect_num = 3;
        [SerializeField] private int bulletForce = 20;
        [SerializeField] private int damage = 10;
        private Vector3 velocity;
        private Rigidbody rb;
        private Renderer renderer;
        private NetworkTransform networkTransform;
        private Vector3 normal;
        private int reflect_count = 0;
        private FixedString64Bytes shooterUUID;
        private bool destroying = false;

        [Inject] private readonly IPlaySceneManager playSceneManager;
        [Inject] private readonly INetworkScoreManager scoreManager;

        void Start()
        {
            gameObject.AddComponent<ZenAutoInjecter>();
            rb = this.GetComponent<Rigidbody>();
            renderer = GetComponent<Renderer>();
            renderer.enabled = false;
            networkTransform = GetComponent<NetworkTransform>();
            networkTransform.Interpolate = false;
            SpawnBullet().Forget();
        }

        // Spawnを待ってBulletをセット
        private async UniTask SpawnBullet()
        {
            await UniTask.WaitUntil(() => playSceneManager != null && IsSpawned, cancellationToken: destroyCancellationToken);
            if (IsOwner)
            {
                var localPlayerTransform = playSceneManager.LocalPlayer.transform;
                transform.position = localPlayerTransform.position + Vector3.up * 0.5f + localPlayerTransform.forward * 0.5f;
                rb.AddForce(localPlayerTransform.forward * bulletForce, ForceMode.Impulse);
                renderer.enabled = true;
                EnableRendererRpc();
                Debug.Log("Shot");
            }
            else if (!IsServer)
            {
                EnableInterpolateRpc();
            }
        }

        [Rpc(SendTo.NotOwner)]
        private void EnableRendererRpc()
        {
            renderer.enabled = true;
        }

        [Rpc(SendTo.Owner)]
        private void EnableInterpolateRpc()
        {
            networkTransform.Interpolate = true;
        }

        private void FixedUpdate()
        {
            velocity = rb.velocity;
        }


        private void OnCollisionEnter(Collision other)
        {
            if (IsOwner & !destroying & IsSpawned)
            {
                Debug.Log("衝突");
                if (other.gameObject.CompareTag("Border"))
                {
                    if (velocity.magnitude <= 0.1)
                    {
                        rb.velocity = new Vector3(0, 0, 0);
                        this.transform.position = new Vector3(0, -0.4f, 0);
                        reflect_count = 0;
                    }
                    reflect_count++;
                    normal = other.contacts[0].normal;

                    Vector3 result = Vector3.Reflect(velocity, normal);

                    rb.velocity = result;

                    // directionの更新
                    velocity = rb.velocity;
                }
                else if (other.gameObject.TryGetComponent<IClientDataHolder>(out var clientDataHolder))
                {
                    var clientData = clientDataHolder.ClientData;
                    if (clientData.Team != playSceneManager.LocalPlayer.GetComponent<IClientDataHolder>().ClientData.Team)
                    {                        
                        Debug.Log("敵にヒット");
                        rb.velocity = new Vector3(0, 0, 0);
                        this.transform.position = new Vector3(0, -0.4f, 0);
                        reflect_count = 0;
                        var hpHolder = other.gameObject.GetComponent<IHpHolder>();
                        hpHolder.DecreaseHp(damage);
                        scoreManager.AddScoreRpc(shooterUUID, score);
                        DestroyThisRpc();
                        destroying = true;
                    }
                }
                if (reflect_count >= max_reflect_num + 1)
                {
                    rb.velocity = new Vector3(0, 0, 0);
                    this.transform.position = new Vector3(0, -0.4f, 0);
                    reflect_count = 0;
                    DestroyThisRpc();
                    destroying = true;
                }
            }
        }

        // (サーバー→全体)
        [Rpc(SendTo.Everyone)]
        public void SetShooterUUIDRpc(FixedString64Bytes uuid)
        {
            shooterUUID = uuid;
        }

        // (クライアント→サーバー)このBulletの削除をする関数
        [Rpc(SendTo.Server)]
        private void DestroyThisRpc()
        {
            Destroy(gameObject);
        }    
    }
}