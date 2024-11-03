using Cysharp.Threading.Tasks;
using RicoShot.Play;
using RicoShot.Play.Interface;
using Supabase.Storage;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace RicoShot.Play
{
    public class AgentPlayer : Agent
    {
        [SerializeField] private int coolTime = 1;
        [SerializeField] private NetworkObject bulletPrefab;
        [SerializeField] private float agentSpeed = 10f;

        private LocalPlayerMoveController playerMoveController;
        private Rigidbody rb;
        private bool onCoolTime = false;

        [Inject] private readonly IPlaySceneManager playSceneManager;

        protected override void Awake()
        {
            base.Awake();
            playerMoveController = GetComponent<LocalPlayerMoveController>();
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {

        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(transform.position);
            sensor.AddObservation(rb.velocity.x);
            sensor.AddObservation(rb.velocity.z);
        }

        public override void OnActionReceived(ActionBuffers actionsBuffer)
        {
            if (playerMoveController == null) return;
            Debug.Log("called");
            ActionSegment<int> act = actionsBuffer.DiscreteActions;
            //前進 or　後退
            var WS = act[0];
            //左 or 右
            var SD = act[1];
            //首振り
            var viewPoint = act[2];
            //撃つ
            var Fire = act[3];

            //それぞれの方向への移動をベクトルで足して最後に反映
            var dirToGo = Vector3.zero;
            var rotateDir = Vector3.zero;

            if (WS == 1 || WS == 2)
            {
                AddReward(0.03f);
            }
            if (SD == 1 || SD == 2)
            {
                AddReward(0.01f);
            }

            switch (WS)
            {
                case 1:
                    dirToGo += Vector3.forward;
                    break;
                case 2:
                    dirToGo += Vector3.back;
                    break;
            }

            switch (SD)
            {
                case 1:
                    dirToGo += Vector3.left;
                    break;
                case 2:
                    dirToGo += Vector3.right;
                    break;
            }


            //新しい形式に合わせる必要性あり


            switch (Fire)
            {
                case 1:
                    OnFire();
                    break;
            }

            rb.AddForce(dirToGo, ForceMode.VelocityChange);
        }

        private void OnFire()
        {
            if (!onCoolTime)
            {
                FireAsync().Forget();
            }
        }

        private async UniTask FireAsync()
        {
            onCoolTime = true;
            var bullet = Instantiate(bulletPrefab,
                transform.position + Vector3.up * 0.5f + transform.forward,
                Quaternion.identity);
            var clientDataHolder = GetComponent<IClientDataHolder>();
            bullet.Spawn();
            var bulletController = bullet.GetComponent<BulletController>();
            bulletController.SetShooterDataRpc(transform.position, transform.forward, clientDataHolder.ClientData);
            await UniTask.WaitForSeconds(coolTime);
        }
    }
}
