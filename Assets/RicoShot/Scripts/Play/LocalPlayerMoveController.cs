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
        [SerializeField] private NetworkObject Bullet;
        [SerializeField] private Transform ShootPoint;
        [SerializeField] private float BulletForce = 20;

        private Transform TPSCam;

        private Rigidbody rb;
        private float _speed;
        private float rotationSpeed;
        private Vector2 moveInput;
        private Animator _animator;
        private int bullet_fire_count = 0;
        private bool OnCooltime = false;
        private const int COOLTIME = 2;
        private float _rotationVelocity = 20;
        private const float RotationSmoothTime = 0.1f;
        private const float Acceleration = 10f;

        private bool LT_pressed = false;
        private bool setUpFinished = false;

        [SerializeField] private float moveSpeedConst = 1.0f;
        [SerializeField] private float rotationSpeedConst = 5.0f;

        //[Inject] IBulletObjectPoolManager bulletObjectPoolManager;
        [Inject] private readonly IPlaySceneManager playSceneManager;
        [Inject] private readonly IPlaySceneTester playSceneTester;

        // animator parameters and constants
        private readonly int _animIDSpeed = Animator.StringToHash("Speed");
        private readonly int _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        private float _animationBlend;
        private const float SpeedMultiplierForAnimation = 5.0f;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();

            SetUpEvents().Forget();
            SetUpTestEvents().Forget();
        }

        // SpawnとInjectを待って、ClientかつOwnerなら入力を取るイベントを登録、それ以外ならスクリプトを無効化
        private async UniTask SetUpEvents()
        {
            await UniTask.WaitUntil(() => IsSpawned && playSceneManager != null,
                cancellationToken: destroyCancellationToken);
            if (IsOwner && IsClient)
            {
                // playSceneManager.PlayInputs.Main.Move.performed += OnMove;
                // playSceneManager.PlayInputs.Main.Move.canceled += OnMove;
                playSceneManager.PlayInputs.Main.Fire.performed += OnFire;
                //playSceneManager.PlayInputs.Main.Camera.started += SetRotationCam;
                //playSceneManager.PlayInputs.Main.Camera.canceled += SetRotationCam;
                TPSCam = playSceneManager.VCamTransform;
                setUpFinished = true;
                Debug.Log("Local player set up finished");
            }
            else if (IsServer)
            {
            }
            else
            {
                enabled = false;
            }
        }

        // playSceneTesterのInjectを待って入力イベントを登録
        private async UniTask SetUpTestEvents()
        {
            await UniTask.WaitUntil(() => playSceneTester != null, cancellationToken: destroyCancellationToken);
            if (playSceneTester.IsTest)
            {
                NetworkObject.SynchronizeTransform = false;
                rb.isKinematic = false;
                playSceneManager.PlayInputs.Main.Fire.performed += OnFire;
                TPSCam = playSceneManager.VCamTransform;
                setUpFinished = true;
            }
        }

        private void Update()
        {
            if (!setUpFinished) return;

            Move();
            Rotate();
        }

        private void LateUpdate()
        {
            // AnimatorControllerのパラメータを更新
            _animator.SetFloat(_animIDSpeed, _animationBlend * SpeedMultiplierForAnimation);
            _animator.SetFloat(_animIDMotionSpeed, moveInput.magnitude);
        }

        // 今のところ、移動のみを行い、回転は行わない. deltaTimeはフレーム間の調整に使う
        private void Move()
        {
            moveInput = playSceneManager.PlayInputs.Main.Move.ReadValue<Vector2>();

            // 移動速度の変化を平滑化
            var targetSpeed = moveInput.magnitude * moveSpeedConst;
            var currentSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
            _speed = Mathf.Abs(targetSpeed - currentSpeed) < 0.1f
                ? Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * Acceleration)
                : targetSpeed;

            // 移動方向をカメラの向きを基準に決定
            var dir = TPSCam.rotation * new Vector3(moveInput.x, 0, moveInput.y).normalized;
            dir.y = 0;
            rb.velocity = dir * _speed;

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * Acceleration);
        }

        private void Rotate()
        {
            var targetRotation = 0.0f;
            if (LT_pressed)
            {
                targetRotation = TPSCam.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _rotationVelocity,
                    RotationSmoothTime / 100);

                // rotate to face input direction relative to camera position
                this.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
            else
            {
                if (moveInput != Vector2.zero)
                {
                    targetRotation = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg +
                                     TPSCam.eulerAngles.y;
                    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation,
                        ref _rotationVelocity,
                        RotationSmoothTime);

                    // rotate to face input direction relative to camera position
                    this.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                }
            }
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

        private void OnFire(InputAction.CallbackContext context)
        {
            if (!OnCooltime)
            {
                //Gamepad.current.SetMotorSpeeds(1f, 1f);
                //await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
                //Gamepad.current.SetMotorSpeeds(0f, 0f);
                // GameObject currentBullet = Bullets[bullet_fire_count % 5];
                //var currentBullet = bulletObjectPoolManager.Shot();
                //currentBullet.transform.position = ShootPoint.position;
                //GameObject currentBullet = Instantiate(Bullet, ShootPoint.position, this.transform.rotation, this.transform);
                //currentBullet.GetComponent<Rigidbody>().AddForce(this.transform.forward * BulletForce, ForceMode.Impulse);
                //currentBullet.transform.parent = null;
                //await UniTask.Delay(TimeSpan.FromSeconds(COOLTIME));
                OnCooltime = true;
                FireAsync().Forget();
            }

            Debug.Log("Fire");
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                //footStepAudio.Play();
            }
        }

        private async UniTask FireAsync()
        {
            if (playSceneTester.IsTest) return;
            OnCooltime = true;
            ShotBulletRpc((NetworkManager.LocalTime - NetworkManager.ServerTime).TimeAsFloat);
            await UniTask.WaitForSeconds(COOLTIME);
            OnCooltime = false;
        }

        [Rpc(SendTo.Server)]
        private void ShotBulletRpc(float rag)
        {
            var bullet = Instantiate(Bullet,
                transform.position + Vector3.up * 0.5f + transform.forward * 0.2f + 5f * rag * rb.velocity,
                Quaternion.identity);
            var clientDataHolder = GetComponent<IClientDataHolder>();
            bullet.SpawnAsPlayerObject(clientDataHolder.ClientData.ClientID);
            var bulletController = bullet.GetComponent<BulletController>();
            bulletController.SetShooterUUIDRpc(clientDataHolder.ClientData.UUID);
        }
    }
}