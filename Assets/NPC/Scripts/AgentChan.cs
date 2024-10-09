using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Actuators;
using Unity.VisualScripting;
using Cysharp.Threading.Tasks.Triggers;
using TMPro;
using System;
using UnityEditor.Rendering;
using Shapes2D;


using Random = UnityEngine.Random;
using System.Xml;

namespace NPC
{
    public enum Team
    {
        team0 = 0,
        team1 = 1
    }
    public class NewBehaviourScript : Agent
    {
        private Team team;
        BehaviorParameters m_BehaviorParameters;
        
        [SerializeField]
        private Vector3 initialPos;
        [SerializeField]
        private GameObject agentManager;
        private Rigidbody agentRb;
        private float agentSpeed;
        public override void Initialize()
        {
            m_BehaviorParameters = gameObject.GetComponent<BehaviorParameters>();

            //teamはBehaviorParameterのteamIDを元にして設定される。
            if(m_BehaviorParameters.TeamId == (int)Team.team0)
            {
                team = Team.team0;
            }
            else
            {
                team = Team.team1;
            }

            agentRb = GetComponent<Rigidbody>();
            agentRb.maxAngularVelocity = agentManager.GetComponent<AgentEnvController>().maxVelocity;
            agentSpeed = agentManager.GetComponent<AgentEnvController>().agentSpeed;
        }

        public override void OnEpisodeBegin()
        {
            transform.position = initialPos + new Vector3(Random.Range(0.0f, 1.0f),0.0f,Random.Range(0.0f, 1.0f));
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(transform.position);
            sensor.AddObservation(agentRb.velocity.x);
            sensor.AddObservation(agentRb.velocity.z);
        }

        public override void OnActionReceived(ActionBuffers actionsBuffer)
        {
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

            if(WS == 1 || WS==2)
            {
                AddReward(0.03f);
            }
            if(SD == 1 || SD==2)
            {
                AddReward(0.01f);
            }

            switch(WS)
            {
                case 1:
                    dirToGo = transform.forward * agentSpeed;
                    break;
                case 2:
                    dirToGo = transform.forward * (-agentSpeed);
                    break;
            }

            switch(SD)
            {
                case 1:
                    dirToGo = transform.right * agentSpeed;
                    break;
                case 2:
                    dirToGo = transform.right * (-agentSpeed);
                    break;
            }

            switch(viewPoint)
            {
                case 1:
                    rotateDir = transform.up * -1f;
                    break;
                case 2:
                    rotateDir = transform.up * 1f;
                    break;
            }


            //新しい形式に合わせる必要性あり

            
            // switch(Fire)
            // {
            //     case 1:
            //         if(Time.realtimeSinceStartup - BulletFiredTime < 2.0)
            //         {
            //             break;
            //         }  
            //         BulletFiredTime = Time.realtimeSinceStartup;
                        
            //         GameObject currentBullet = Instantiate(Bullet, ShootPoint.position, this.transform.rotation, this.transform);
            //         currentBullet.GetComponent<Rigidbody>().AddForce(this.transform.forward * BulletForce, ForceMode.Impulse);
            //         currentBullet.transform.parent = null;
            //         AddReward(Fired);
            //         break;
            // }

            transform.Rotate(rotateDir, Time.deltaTime * 100f);
            //ForceMode.VelocityChange　質量の違いを考慮しない制御
            agentRb.AddForce(dirToGo, ForceMode.VelocityChange);
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var discreteActionsOut = actionsOut.DiscreteActions;
            // Debug.LogWarning(discreteActionsOut.Length);
            if(Input.GetKey(KeyCode.W))
            {
                Debug.Log("W key");
                discreteActionsOut[0] = 1;
            }
            if(Input.GetKey(KeyCode.S))
            {
                discreteActionsOut[0] = 2;
            }
            if(Input.GetKey(KeyCode.A))
            {
                discreteActionsOut[1] = 1;
            }
            if(Input.GetKey(KeyCode.D))
            {
                discreteActionsOut[1] = 2;
            }
            if(Input.GetKey(KeyCode.E))
            {
                discreteActionsOut[2] = 1;
            }
            if(Input.GetKey(KeyCode.Q))
            {
                discreteActionsOut[2] = 2;
            }
            if(Input.GetKey(KeyCode.Space))
            {
                discreteActionsOut[3] = 1;
            }
        }
    }
}
