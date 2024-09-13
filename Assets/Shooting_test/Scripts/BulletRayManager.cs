using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shooting_test
{
    public class BulletRayManager : MonoBehaviour
    {
        [SerializeField]
        public Transform shootPoint;

        PlayerInput playerInput;
        LineRenderer linerend;
        private float BULLET_RADIUS = 0.15f;
        private void Start()
        {
            playerInput = this.GetComponent<PlayerInput>();
            linerend = this.GetComponent<LineRenderer>();
            linerend.positionCount = 0;
        }

        private void Update()
        {
            if (playerInput.actions["Draw_Ray"].IsPressed())
            {
                Draw_Bullet_shot(shootPoint);
                Debug.Log("押している");
            }
        }

        public void Draw_Bullet_shot(Transform ShootPoint)
        {
            var direction = shootPoint.rotation * Vector3.forward;
            Ray ray1 = new Ray(ShootPoint.position, direction),
                ray2 = new Ray(ShootPoint.position, direction),
                ray3 = new Ray(ShootPoint.position, direction),
                ray4 = new Ray(ShootPoint.position, direction);
            Ray[] rays = new Ray[4];
            RaycastHit[] hits = new RaycastHit[5];
            rays[0] = new Ray(ShootPoint.position, direction);
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
            DrawRayLine(ShootPoint.position, hits);
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

        public void Delete_BulletShot()
        {
            linerend.positionCount = 0;
        }
    }
}