using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

namespace NPC
{
    public class BulletControllerWithMLAgents : MonoBehaviour
    {
        [SerializeField]
        private GameObject agentSettings;
        private AgentEnvController env;

        private Vector3 velocity;
        private Rigidbody rb;
        private Vector3 normal;
        private int reflect_count = 0;
        private int max_reflect_num = 3;
        // [SerializeField]
        // public GameObject gameManager;
        [SerializeField]
        private string whiteTag;
        [SerializeField]
        private string redTag;
        
        [HideInInspector]
        public AgentEnvController envController;
        // Start is called before the first frame update
        void Start()
        {
            //this.GetComponent<Rigidbody>().AddForce(new Vector3(0,0,4),ForceMode.Impulse);
            rb = GetComponent<Rigidbody>();
            // rb = this.GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogError("Rigidbody is not attached to the Bullet object!");
            }
            velocity = Vector3.zero;
            // else
            // {
            //     Debug.LogError("Correct");
            // }
            GameObject goj = GameObject.Find("GameManager");
            if(goj != null)
            {
                // Debug.LogWarning("goj found");
                envController = goj.GetComponent<AgentEnvController>();
                // if(envController == null)
                // {
                //     Debug.LogWarning("envController is still null");
                // }
                // else 
                // {
                //     Debug.LogWarning("envController is not null");
                // }
            }
            else
            {
                Debug.LogWarning("goj not found");
            }
            // if(envController == null)
            // {
            //     Debug.LogError("cant");
            // }
        }
        // Update is called once per frame
        void Update()
        {
            
        }
        private void FixedUpdate()
        {
            velocity = rb.velocity;
        }

        void OnTriggerEnter(Collider collider)
        {
            if(env = null)
            {
                env = agentSettings.GetComponent<AgentEnvController>();
            }
            if(collider.gameObject.CompareTag(env.AlphaCharacterTag))
            {
                if(this.gameObject.tag == env.AlphaBulletTag) return;

                env.Hitted(env.AlphaCharacterTag);
            }
            else if(collider.gameObject.CompareTag(env.BravoCharacterTag))
            {
                if(this.gameObject.tag == env.BravoBulletTag) return;

                env.Hitted(env.BravoCharacterTag);
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            // if(collision.gameObject.CompareTag(whiteTag))
            // {
            //     if(envController == null)
            //     {
            //         // Debug.LogWarning("envController is null");
            //     }
            //     else
            //     {
            //         // Debug.LogWarning("envController is not null");
            //         envController.Hitted(Team.White);
            //     }
            // }
            // if(collision.gameObject.CompareTag(redTag))
            // {
            //     if(envController == null)
            //     {
            //         // Debug.LogWarning("envController is null");
            //     }
            //     else
            //     {
            //         // Debug.LogWarning("envController is not null");
            //         envController.Hitted(Team.Red);
            //     }
            // }
            // Debug.Log("壁衝突");
            if (collision.gameObject.CompareTag("Border"))
            {
                if (velocity.magnitude <= 0.1)
                {
                    Destroy(this.gameObject);
                }
                reflect_count++;
                normal = collision.contacts[0].normal;
                Vector3 result = Vector3.Reflect(velocity, normal);
                if(rb != null)
                {
                    // Debug.LogError("rb is null");
                    rb.velocity = result;
                    // directionの更新
                    velocity = rb.velocity;
                }
            }
            
            if (reflect_count == max_reflect_num+1)
            {
                Destroy(this.gameObject);
            }
            // Debug.LogWarning(whiteTag + ":" + redTag);
            
        }
    }
}