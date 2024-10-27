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
    public class BulletRayManager : NetworkBehaviour
    {
        [SerializeField] private Transform shootPoint;

        //PlayerInput playerInput;
        private LineRenderer linerend;
        private float BULLET_RADIUS = 0.15f;
        private bool drawRay = false;

        [Inject] private readonly IPlaySceneManager playSceneManager;

        private void Start()
        {
            linerend = GetComponent<LineRenderer>();
            linerend.positionCount = 0;
            SetUpBulletRay().Forget();
        }

        private async UniTask SetUpBulletRay()
        {
            await UniTask.WaitUntil(() => IsSpawned && playSceneManager != null, cancellationToken: destroyCancellationToken);
            if (IsClient && IsOwner)
            {
                playSceneManager.PlayInputs.Main.DrawRay.performed += OnDrawRay;
                playSceneManager.PlayInputs.Main.DrawRay.canceled += OnCancelDrawRay;
            }
            else
            {
                enabled = false;
            }
        }

        private void Update()
        {
            if (drawRay)
            {
                DrawBulletShot();
            }
            else
            {
                if (linerend.positionCount != 0)
                {
                    DeleteBulletShot();
                }
            }
        }

        private void OnDrawRay(InputAction.CallbackContext context)
        {
            drawRay = true;
        }

        private void OnCancelDrawRay(InputAction.CallbackContext context)
        {
            drawRay = false;
        }

        private void DrawBulletShot()
        {
            var direction = shootPoint.rotation * Vector3.forward;
            Debug.Log(direction);
            Ray ray1 = new Ray(shootPoint.position, direction),
                ray2 = new Ray(shootPoint.position, direction),
                ray3 = new Ray(shootPoint.position, direction),
                ray4 = new Ray(shootPoint.position, direction);
            Ray[] rays = new Ray[4];
            RaycastHit[] hits = new RaycastHit[5];
            rays[0] = new Ray(shootPoint.position, direction);
            if (Physics.SphereCast(rays[0], BULLET_RADIUS, out hits[0]))
            {
                rays[1] = new Ray(hits[0].point, Vector3.Reflect(direction, hits[0].normal));
            }
            if (Physics.SphereCast(rays[1], BULLET_RADIUS, out hits[1]))
            {
                rays[2] = new Ray(hits[1].point, Vector3.Reflect(hits[1].point - hits[0].point, hits[1].normal));
            }
            if (Physics.SphereCast(rays[2], BULLET_RADIUS, out hits[2]))
            {
                rays[3] = new Ray(hits[2].point, Vector3.Reflect(hits[2].point - hits[1].point, hits[2].normal));
            }
            Physics.SphereCast(rays[3], BULLET_RADIUS, out hits[3]);
            DrawRayLine(shootPoint.position, hits);
        }

        private void DrawRayLine(Vector3 start, RaycastHit[] hits)
        {
            //LineRendererコンポーネントの取得
            linerend.startWidth = 3;
            linerend.endWidth = 3;
            linerend.positionCount = 5;

            //線の太さを設定
            linerend.startWidth = 0.1f;
            linerend.endWidth = 0.1f;

            //始点, 終点を設定し, 描画
            linerend.SetPosition(0, start);
            linerend.SetPosition(1, hits[0].point);
            linerend.SetPosition(2, hits[1].point);
            linerend.SetPosition(3, hits[2].point);
            linerend.SetPosition(4, hits[3].point);
        }

        private void DeleteBulletShot()
        {
            linerend.positionCount = 0;
        }
    }
}