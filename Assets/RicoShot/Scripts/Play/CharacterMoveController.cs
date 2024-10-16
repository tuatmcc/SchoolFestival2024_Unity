using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using System;
using Zenject;

namespace RicoShot.Play
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
        private int COOLTIME = 2;
        private float _rotationVelocity = 20;
        private float RotationSmoothTime = 0.2f;

        private bool LT_pressed = false;

        [SerializeField] private float moveSpeedConst = 5.0f;
        [SerializeField] private float rotationSpeedConst = 5.0f;
        [SerializeField] Transform TPSCam;
        
        [Inject] private IBulletObjectPoolManager bulletObjectPoolManager;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
        }

        void FixedUpdate()
        {



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

            Debug.Log(rb.velocity);
            //this.transform.rotation = Quaternion.Euler(targetDirection);
            
        }

        private void Update()
        {
            //speed = moveInput.magnitude * moveSpeedConst;
            //rotationSpeed = moveInput.x * rotationSpeedConst;
            float _targetRotation = 0;
            if (LT_pressed)
            {
                _targetRotation = 
                                  TPSCam.eulerAngles.y;
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

        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
            animator.SetFloat("speed", Math.Abs(moveInput.magnitude));
            animator.SetFloat("rotate", moveInput.x);
        }

        public void SetRotation_Cam(InputAction.CallbackContext context)
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

        async public void Fire_Bullet(InputAction.CallbackContext context)
        {
            if (context.performed && ! OnCooltime)
            {
                Gamepad.current.SetMotorSpeeds(1f, 1f);
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
                Gamepad.current.SetMotorSpeeds(0f, 0f);
                OnCooltime = true;
                // GameObject currentBullet = Bullets[bullet_fire_count % 5];
                var currentBullet = bulletObjectPoolManager.Shot();
                currentBullet.transform.position = ShootPoint.position;
                //GameObject currentBullet = Instantiate(Bullet, ShootPoint.position, this.transform.rotation, this.transform);
                currentBullet.GetComponent<Rigidbody>().AddForce(this.transform.forward * BulletForce, ForceMode.Impulse);
                currentBullet.transform.parent = null;
                await UniTask.Delay(TimeSpan.FromSeconds(COOLTIME));
                OnCooltime = false;
            }
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = 4;
            float _speed;
            float SpeedChangeRate = 1;
            float _targetRotation = 0;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (moveInput == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = moveInput.magnitude;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }
            /*
            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;
            */

            // normalise input direction
            Vector3 inputDirection = new Vector3(moveInput.x, 0.0f, moveInput.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (moveInput != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  TPSCam.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player


            this.GetComponent<CharacterController>().Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, 1, 0.0f) * Time.deltaTime);

            // update animator if using character
            /*
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
            */
        }
    }
}