using Cysharp.Threading.Tasks;
using RicoShot.Play.Interface;
using System.Collections;
using System.Collections.Generic;
using R3;
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
        private Subject<Mesh> _onDrawRaySubject;
        private Subject<Unit> _onCancelDrawRaySubject;

        [Inject] private readonly IPlaySceneManager playSceneManager;
        [Inject] private readonly IPlaySceneTester playSceneTester;
        
        public Observable<Mesh> OnDrawRayAsObservable => _onDrawRaySubject;
        public Observable<Unit> OnCancelDrawRayAsObservable => _onCancelDrawRaySubject;

        private void Start()
        {
            linerend = GetComponent<LineRenderer>();
            linerend.positionCount = 0;
            SetUpBulletRay().Forget();
            SetUpBulletRayTest().Forget();
            _onDrawRaySubject = new Subject<Mesh>();
            _onCancelDrawRaySubject = new Subject<Unit>();
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

        private async UniTask SetUpBulletRayTest()
        {
            await UniTask.WaitUntil(() => playSceneTester != null, cancellationToken: destroyCancellationToken);
            if (playSceneTester.IsTest)
            {
                playSceneManager.PlayInputs.Main.DrawRay.performed += OnDrawRay;
                playSceneManager.PlayInputs.Main.DrawRay.canceled += OnCancelDrawRay;
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
            _onCancelDrawRaySubject.OnNext(Unit.Default);
        }

        private void DrawBulletShot()
        {
            var direction = shootPoint.rotation * Vector3.forward;
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
            linerend.startWidth = 1;
            linerend.endWidth = 1;
            linerend.positionCount = 5;

            //線の太さを設定
            linerend.startWidth = 0.01f;
            linerend.endWidth = 0.01f;

            //始点, 終点を設定し, 描画
            linerend.SetPosition(0, start);
            linerend.SetPosition(1, hits[0].point);
            linerend.SetPosition(2, hits[1].point);
            linerend.SetPosition(3, hits[2].point);
            linerend.SetPosition(4, hits[3].point);
            var mesh = new Mesh();
            linerend.BakeMesh(mesh);
            _onDrawRaySubject.OnNext(mesh);
        }

        private void DeleteBulletShot()
        {
            linerend.positionCount = 0;
        }
    }
}