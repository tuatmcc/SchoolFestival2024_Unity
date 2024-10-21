using Cysharp.Threading.Tasks;
using RicoShot.Play.Interface;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace RicoShot.Play
{
    public class CameraControl : NetworkBehaviour
    {
        //マウス感度
        private readonly float sensitivity = 2f;
        private Vector2 RStick_input = new Vector2(0,0);

        [Inject] private readonly IPlaySceneManager playSceneManager;

        private void Start()
        {
            if (IsClient && IsOwner)
            {
                SetUpCameraControl().Forget();
            }
        }

        private async UniTask SetUpCameraControl()
        {
            await UniTask.WaitUntil(() => playSceneManager != null, cancellationToken: destroyCancellationToken);
            playSceneManager.PlayInputs.Main.Camera.performed += CameraRotateGamepad;
            playSceneManager.PlayInputs.Main.Camera.canceled += CameraRotateGamepad;
            Debug.Log("Camera control setup finished");
        }

        void Update()
        {
            transform.RotateAround(transform.position, Vector3.up, RStick_input.x * sensitivity);
        }

        public void CameraRotateGamepad(InputAction.CallbackContext context)
        {
            RStick_input = context.ReadValue<Vector2>();
        }
    }
}
