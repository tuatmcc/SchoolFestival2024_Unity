using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class DecoyController : NetworkBehaviour 
{
    [SerializeField] float speed = 1f;
    private Vector2 movement;
    void Start()
    {
        if (IsOwner)
        {
            SetPositionServerRpc(new Vector3(Random.Range(-3, 3), 3, Random.Range(-3, 3)));
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (IsOwner)
        {
            SetVelocityServerRpc(context.ReadValue<Vector2>());
        }
    }

    void Update()
    {
        if (IsOwner)
        {
            // サーバー(ホスト)のメソッドを呼び出す
            MoveOnServerRpc();
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
    void SetVelocityServerRpc(Vector2 velocity)
    {
        movement = speed * velocity;
    }

    [ServerRpc]
    void SetPositionServerRpc(Vector3 position)
    {
        transform.position = position;
    }
    
    [ServerRpc]
    void MoveOnServerRpc()
    {
        // ここはサーバー(ホスト)でだけ処理される
        Vector3 moveInput = new Vector3(movement.x, 0, movement.y);
        if (moveInput.magnitude > 0f)
            transform.LookAt(transform.position + moveInput);

        transform.position += moveInput * Time.deltaTime;
    }
}
