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

using Random = UnityEngine.Random;
using System.Xml;

public enum Team
{
    White = 0,
    Red = 1
}
public class AgentChan : Agent
{

    [HideInInspector]
    public Team team;
    BehaviorParameters m_BehavoprParameters;
    public Vector3 initialPos;
    public Rigidbody agentRb;


    //Rewardの設定
    public float agentSpeed     = 0.3f;
    // public float CollisionObj   = -0.1f;
    public float CollisionAlly  = -0.15f;
    public float CollisionEnemy = -0.2f;
    //発射に対しコストが支払われないと連打になると予想
    public float Fired = -0.2f;
    // public float CollisionBullet= -0.8f;
    [SerializeField]
    int maxVelocity = 50;

    [SerializeField]
    public GameObject Bullet;
    public Transform ShootPoint;
    public float BulletForce = 20;

    [SerializeField]
    private string allyTag; //味方タグ
    [SerializeField]
    private string enemyTag; //敵タグ


    private double BulletFiredTime = 0.0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Initialize()
    {
        m_BehavoprParameters = gameObject.GetComponent<BehaviorParameters>();
        if(m_BehavoprParameters.TeamId == (int)Team.White)
        {
            team = Team.White;
            // initialPos = new Vector3(transform.position.x)
        }
        else
        {
            team = Team.Red;
        }

        agentRb = GetComponent<Rigidbody>();
        agentRb.maxAngularVelocity = maxVelocity;

        initialPos = transform.position;
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

        switch(Fire)
        {
            case 1:
                if(Time.realtimeSinceStartup - BulletFiredTime < 2.0)
                {
                    break;
                }  
                BulletFiredTime = Time.realtimeSinceStartup;
                    
                GameObject currentBullet = Instantiate(Bullet, ShootPoint.position, this.transform.rotation, this.transform);
                currentBullet.GetComponent<Rigidbody>().AddForce(this.transform.forward * BulletForce, ForceMode.Impulse);
                currentBullet.transform.parent = null;
                AddReward(Fired);
                break;
        }

        transform.Rotate(rotateDir, Time.deltaTime * 100f);
        //ForceMode.VelocityChange　質量の違いを考慮しない制御
        agentRb.AddForce(dirToGo, ForceMode.VelocityChange);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag(allyTag))
        {
            AddReward(CollisionAlly);
        }
        if(collision.gameObject.CompareTag(enemyTag))
        {
            AddReward(CollisionEnemy);
        }
        if(collision.gameObject.CompareTag(Bullet.tag))
        {
            AddReward(-0.5f);
        }

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        // Debug.LogWarning(discreteActionsOut.Length);
        if(Input.GetKey(KeyCode.W))
        {
            // Debug.Log("W key");
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
