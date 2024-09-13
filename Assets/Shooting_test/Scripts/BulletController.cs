using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Vector3 velocity;
    private Rigidbody rb;
    private Vector3 normal;
    private int reflect_count = 0;
    private int max_reflect_num = 3;
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
        Debug.Log("壁衝突");
        if (collision.gameObject.CompareTag("Border"))
        {
            if (velocity.magnitude <= 0.1)
            {
                Destroy(this.gameObject);
            }
            reflect_count++;
            normal = collision.contacts[0].normal;

            Vector3 result = Vector3.Reflect(velocity, normal);

            rb.velocity = result;

            // directionの更新
            velocity = rb.velocity;
        }
        if (reflect_count == max_reflect_num+1)
        {
            Destroy(this.gameObject);
        }
    }
}
