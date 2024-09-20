using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DecoyController : NetworkBehaviour 
{
    [SerializeField] float speed = 1f;
    void Start()
    {
        
    }

    void Update()
    {
        if (IsOwner)
        {
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");
            // サーバー(ホスト)のメソッドを呼び出す
            MoveServerRpc(h, v);
        }
        
        if (IsServer)
        {
            // ステージから落ちたら戻してあげる
            if (transform.position.y < -10.0)
            {
                transform.position = new Vector3(Random.Range(-3, 3), 3, Random.Range(-3, 3));
            }
        }
    }
    
    [ServerRpc]
    void MoveServerRpc(float vertical, float horizontal)
    {
        // ここはサーバー(ホスト)でだけ処理される
        var moveInput = new Vector3(horizontal, 0, vertical);
        if (moveInput.magnitude > 0f)
            transform.LookAt(transform.position + moveInput);

        transform.position += speed * moveInput * Time.deltaTime;
    }
}
