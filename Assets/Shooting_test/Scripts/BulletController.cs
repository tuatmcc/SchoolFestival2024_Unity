using System.Collections;
using System.Collections.Generic;
using RicoShot.Utils;
using UnityEngine;
using Zenject;
namespace Shooting_test
{
    public class BulletController : PoolManagedMonoObject
    {
        private Vector3 velocity;
        private Rigidbody rb;
        private Vector3 normal;
        private int reflect_count = 0;
        private int max_reflect_num = 3;
        [Inject] private IScoreManager scoreManager;
        // Start is called before the first frame update
        void Start()
        {
            //this.GetComponent<Rigidbody>().AddForce(new Vector3(0,0,4),ForceMode.Impulse);
            rb = this.GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            velocity = rb.velocity;
        }

        private void OnCollisionEnter(Collision collision)
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
                scoreManager.AddScore(100);
                this.ReturnPool();
            }
            if (reflect_count >= max_reflect_num + 1)
            {
                rb.velocity = new Vector3(0, 0, 0);
                this.transform.position = new Vector3(0, -0.4f, 0);
                reflect_count = 0;
                this.ReturnPool();
            }
        }
    }
}