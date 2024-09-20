using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using System;

namespace Shooting_test
{
    public class CharacterMoveController : MonoBehaviour
    {
        [SerializeField]
        public GameObject [] Bullets;
        public Transform ShootPoint;
        public float BulletForce = 20;

        private Rigidbody rb;
        private float speed;
        private float rotationSpeed;
        private Vector2 moveInput;
        public Animator animator;
        private int bullet_fire_count = 0;
        private bool OnCooltime = false;
        private int COOLTIME = 3;

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

        private void Update()
        {
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
            animator.SetFloat("speed", moveInput.y);
            animator.SetFloat("rotate", moveInput.x);
        }

        async public void Fire_Bullet(InputAction.CallbackContext context)
        {
            if (context.performed && ! OnCooltime)
            {
                Gamepad.current.SetMotorSpeeds(0.25f, 0.75f);
                OnCooltime = true;
                GameObject currentBullet = Bullets[bullet_fire_count % 5];
                bullet_fire_count++;
                currentBullet.transform.position = ShootPoint.position;
                //GameObject currentBullet = Instantiate(Bullet, ShootPoint.position, this.transform.rotation, this.transform);
                currentBullet.GetComponent<Rigidbody>().AddForce(this.transform.forward * BulletForce, ForceMode.Impulse);
                currentBullet.transform.parent = null;
                await UniTask.Delay(TimeSpan.FromSeconds(COOLTIME));
                OnCooltime = false;
            }
        }
    }
}