using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using Zenject;
using RicoShot.Play.Interface;
using Unity.Netcode;
using System;

namespace RicoShot.Play
{
    public class LocalPlayerMoveController : NetworkBehaviour, IHpHolder
    {
        public event Action<int> OnHpChanged;

        public int Hp
        {
            get => hp;
            set
            {
                hp = value;
                OnHpChanged?.Invoke(hp);
                Debug.Log($"hp changed -> {hp}");
            }
        }

        [SerializeField] private NetworkObject Bullet;
        [SerializeField] private Transform ShootPoint;
        [SerializeField] private float BulletForce = 20;
        [SerializeField] private int hp = 50;
        [SerializeField] private AudioClip[] footStepAudio = new AudioClip[5];


        private Transform TPSCam;

        private Rigidbody rb;
        private float _speed;
        private float rotationSpeed;
        private Vector2 moveInput;
        private Animator _animator;
        private int bullet_fire_count = 0;
        private bool OnCooltime = false;
        private const int CoolTime = 1;
        private float _rotationVelocity = 20;
        private const float RotationSmoothTime = 0.1f;
        private const float Acceleration = 10f;

        private bool LT_pressed = false;
        private bool setUpFinished = false;

        [SerializeField] private float moveSpeedConst = 1.0f;

        //[Inject] IBulletObjectPoolManager bulletObjectPoolManager;
        [Inject] private readonly IPlaySceneManager playSceneManager;
        [Inject] private readonly IPlaySceneTester playSceneTester;

        // animator parameters and constants
        private readonly int _animIDSpeed = Animator.StringToHash("Speed");
        private readonly int _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        private readonly int _animIDThrow = Animator.StringToHash("Throw");
        private float _animationBlend;
        private bool _animateThrow;
        private const float SpeedMultiplierForAnimation = 2.0f;

        //Foot step
        private int recentplayedIndex = -1;

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
                playSceneManager.PlayInputs.Main.Fire.performed += OnFire;
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
            // AnimatorControllerのパラメータを更新. サイズが小さいので実際の速度とアニメーションの差を調整
            _animator.SetFloat(_animIDSpeed, _animationBlend * SpeedMultiplierForAnimation);
            _animator.SetFloat(_animIDMotionSpeed, 1); // input.magnitudeだと遅すぎたため固定値
            _animator.SetBool(_animIDThrow, _animateThrow);

            // フラグをリセット
            _animateThrow = false;
        }

        // 今のところ、移動のみを行い、回転は行わない. 
        private void Move()
        {
            moveInput = playSceneManager.PlayInputs.Main.Move.ReadValue<Vector2>();

            // 移動速度の変化を平滑化
            var targetSpeed = moveInput.magnitude * moveSpeedConst;
            var currentSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
            // Time.deltaTime を掛けているためフレームレートによる移動速度差は発生しない
            _speed = Mathf.Abs(targetSpeed - currentSpeed) < 0.1f
                ? Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * Acceleration)
                : targetSpeed;

            // 移動方向をカメラのXZ平面上の向きを基準に決定
            var dir = TPSCam.forward * moveInput.y + TPSCam.right * moveInput.x;
            dir.y = 0;
            dir.Normalize();
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
                FireAsync().Forget();
                _animateThrow = true;
            }

            Debug.Log("Fire");
        }

        // 歩行アニメーションで呼ばれる
        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                int idx = UnityEngine.Random.Range(0, footStepAudio.Length);
                if(idx == recentplayedIndex)
                {
                    idx = (idx + 1) % footStepAudio.Length;
                }
                AudioSource.PlayClipAtPoint(footStepAudio[idx], transform.position);
            }
        }

        private async UniTask FireAsync()
        {
            if (playSceneTester.IsTest) return;
            OnCooltime = true;
            ShotBulletRpc();
            await UniTask.WaitForSeconds(CoolTime);
            OnCooltime = false;
        }

        [Rpc(SendTo.Server)]
        private void ShotBulletRpc()
        {
            var bullet = Instantiate(Bullet,
                transform.position + Vector3.up * 0.5f + transform.forward,
                Quaternion.identity);
            var clientDataHolder = GetComponent<IClientDataHolder>();
            bullet.SpawnAsPlayerObject(clientDataHolder.ClientData.ClientID);
            bullet.tag = $"{clientDataHolder.ClientData.Team}Bullet";
            var bulletController = bullet.GetComponent<BulletController>();
            bulletController.SetShooterUUIDRpc(clientDataHolder.ClientData.UUID);
        }

        public void DecreaseHp(int damage)
        {
            DecreaseHpRpc(damage);
        }

        [Rpc(SendTo.Owner)]
        private void DecreaseHpRpc(int damage)
        {
            Hp -= damage;
        }
    }
}