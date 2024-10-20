using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using System;
using Zenject;
using RicoShot.Play.Interface;
using Unity.Netcode;

namespace RicoShot.Play
{
    public class LocalPlayerMoveController : NetworkBehaviour
    {
        [SerializeField]
        public GameObject [] Bullets;
        public Transform ShootPoint;
        public float BulletForce = 20;

        private Transform TPSCam;

        private Rigidbody rb;
        //private float speed;
        //private float rotationSpeed;
        private Vector2 moveInput;
        //public Animator animator;
        //private int bullet_fire_count = 0;
        private bool OnCooltime = false;
        private int COOLTIME = 2;
        private float _rotationVelocity = 20;
        private float RotationSmoothTime = 0.2f;

        private bool LT_pressed = false;
        private bool setUpFinished = false;

        [SerializeField] private float moveSpeedConst = 5.0f;
        [SerializeField] private float rotationSpeedConst = 5.0f;

        //[Inject] IBulletObjectPoolManager bulletObjectPoolManager;
        [Inject] private readonly IPlaySceneManager playSceneManager;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            //animator = GetComponent<Animator>();

            SetUpEvents().Forget();
        }

        // SpawnとInjectを待って、ClientかつOwnerなら入力を取るイベントを登録、それ以外ならスクリプトを無効化
        private async UniTask SetUpEvents()
        {
            await UniTask.WaitUntil(() => IsSpawned && playSceneManager !=  null, cancellationToken: destroyCancellationToken);
            if (IsOwner && IsClient)
            {
                playSceneManager.PlayInputs.Main.Move.performed += OnMove;
                playSceneManager.PlayInputs.Main.Move.canceled += OnMove;
                playSceneManager.PlayInputs.Main.Fire.performed += OnFire;
                playSceneManager.PlayInputs.Main.Camera.performed += SetRotationCam;
                TPSCam = playSceneManager.VCamTransform;
                setUpFinished = true;
                Debug.Log("Local player set up finished");
            }
            else
            {
                enabled = false;
            }
        }

        void FixedUpdate()
        {
            if(!setUpFinished) return;

            // move the player

            // update animator if using character
            /*
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
            */
            Vector3 v = TPSCam.rotation * new Vector3(4 * moveInput.x, 0, 4 * moveInput.y);
            v.y = 0;
            rb.velocity =v;

            //Debug.Log(rb.velocity);
            //this.transform.rotation = Quaternion.Euler(targetDirection);
            
        }

        private void Update()
        {
            if (!setUpFinished) return;

            //speed = moveInput.magnitude * moveSpeedConst;
            //rotationSpeed = moveInput.x * rotationSpeedConst;
            float _targetRotation = 0;
            if (LT_pressed)
            {
                _targetRotation = TPSCam.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime/100);

                // rotate to face input direction relative to camera position
                this.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
            else
            {
                if (moveInput != Vector2.zero)
                {
                    _targetRotation = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg +
                                      TPSCam.eulerAngles.y;
                    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                        RotationSmoothTime);

                    // rotate to face input direction relative to camera position
                    this.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                }
            }
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
            //animator.SetFloat("speed", Math.Abs(moveInput.magnitude));
            //animator.SetFloat("rotate", moveInput.x);
            Debug.Log($"Move Input: {moveInput}");
        }

        private void SetRotationCam(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                LT_pressed = true;
            }
            else if (context.canceled)
            {
                LT_pressed = false;
            }
        }

        private async void OnFire(InputAction.CallbackContext context)
        {
            //if (context.performed && ! OnCooltime)
            //{
            //    Gamepad.current.SetMotorSpeeds(1f, 1f);
            //    await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            //    Gamepad.current.SetMotorSpeeds(0f, 0f);
            //    OnCooltime = true;
            //    // GameObject currentBullet = Bullets[bullet_fire_count % 5];
            //    var currentBullet = bulletObjectPoolManager.Shot();
            //    currentBullet.transform.position = ShootPoint.position;
            //    //GameObject currentBullet = Instantiate(Bullet, ShootPoint.position, this.transform.rotation, this.transform);
            //    currentBullet.GetComponent<Rigidbody>().AddForce(this.transform.forward * BulletForce, ForceMode.Impulse);
            //    currentBullet.transform.parent = null;
            //    await UniTask.Delay(TimeSpan.FromSeconds(COOLTIME));
            //    OnCooltime = false;
            //}
            Debug.Log("Fire");
        }
    }
}