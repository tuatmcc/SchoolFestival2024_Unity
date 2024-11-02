using RicoShot.Play;
using RicoShot.Play.Interface;
using Supabase.Storage;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace RicoShot.Play
{
    public class AgentPlayer : Agent
    {
        private LocalPlayerMoveController playerMoveController;
        private Rigidbody rb;

        [Inject] private readonly IPlaySceneManager playSceneManager;

        protected override void Awake()
        {
            base.Awake();
            playerMoveController = GetComponent<LocalPlayerMoveController>();
            rb = GetComponent<Rigidbody>();
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(transform.position);
            sensor.AddObservation(rb.velocity.x);
            sensor.AddObservation(rb.velocity.z);
        }

        public override void OnActionReceived(ActionBuffers actionsBuffer)
        {
            if(playerMoveController == null) return;
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
                    playerMoveController.SetValues(Vector2.up);
                    break;
                case 2:
                    playerMoveController.SetValues(Vector2.down);
                    break;
            }

            switch (SD)
            {
                case 1:
                    playerMoveController.SetValues(Vector2.left);
                    break;
                case 2:
                    playerMoveController.SetValues(Vector2.right);
                    break;
            }


            //新しい形式に合わせる必要性あり


            switch (Fire)
            {
                case 1:
                    playerMoveController.OnFire(new InputAction.CallbackContext());
                    break;
            }
        }
    }
}
