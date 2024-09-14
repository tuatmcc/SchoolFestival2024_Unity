using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shooting_test
{
    public class CharacterMoveController : MonoBehaviour
    {
        [SerializeField]
        public GameObject Bullet;
        public Transform ShootPoint;
        public float BulletForce = 20;

        private Rigidbody rb;
        private float speed;
        private float rotationSpeed;
        private Vector2 moveInput;
        public Animator animator;

        [SerializeField] private float moveSpeedConst = 5.0f;
        [SerializeField] private float rotationSpeedConst = 5.0f;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
        }

        void FixedUpdate()
        {
            speed = moveInput.magnitude * moveSpeedConst;
            rotationSpeed = moveInput.x * rotationSpeedConst;

            rb.velocity = this.transform.rotation *  new Vector3(moveInput.x,0,moveInput.y) * speed;
            //rb.angularVelocity = new Vector3(0, rotationSpeed, 0);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
            animator.SetFloat("speed", moveInput.y);
            animator.SetFloat("rotate", moveInput.x);
        }

        public void Fire_Bullet(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                GameObject currentBullet = Instantiate(Bullet, ShootPoint.position, this.transform.rotation, this.transform);
                currentBullet.GetComponent<Rigidbody>().AddForce(this.transform.forward * BulletForce, ForceMode.Impulse);
                currentBullet.transform.parent = null;
            }
        }
    }
}