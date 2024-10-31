using System.Collections;
using System.Collections.Generic;
using Shapes2D;
using Unity.Mathematics;

// using System.Numerics;
using Unity.MLAgents;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace NPC
{
    [System.Serializable]
    public class AgentStruct
    {
        public GameObject obj;
        [HideInInspector]
        public AgentChan agent;
        public Vector3 initPos;
        public Vector3 initRot;
        public int teamID;

        // public AgentStruct(AgentChan agent, Vector3 initPos,Vector3 initRot ,int teamID)
        // {
        //     this.agent = agent;
        //     this.initPos = initPos;
        //     this.initRot = initRot;
        //     this.teamID = teamID;
        // }
    }

    public class AgentEnvController : MonoBehaviour
    {
        //エージェントの能力を決定
        public float agentSpeed=0.3f;
        public float maxVelocity=50.0f;


        
        // public int Alpha
        // //

        public GameObject AlphaBullet;
        public GameObject BravoBullet;
        public string AlphaBulletTag = "AlphaBullet";
        public string BravoBulletTag = "BravoBullet";
        public string AlphaCharacterTag = "AlphaCharacter";
        public string BravoCharacterTag = "BravoCharacter";
        public float FireReward = -0.1f;
        public float WarkReward = 0.01f;
        public float GiveHitReward = 1.0f;
        public float GetHitReward = -1.0f;

        // public List<GameObject> AlphaAgents=new List<GameObject>();
        // public List<GameObject> BravoAgents=new List<GameObject>();
        
        // [SerializeField]
        // public List<AgentStruct> agentlist = new List<AgentStruct>();

        [SerializeField]
        public List<AgentStruct> agentlist = new List<AgentStruct>();

        private SimpleMultiAgentGroup m_AlphaAgentGroup;
        private SimpleMultiAgentGroup m_BravoAgentGroup;

        void Start()
        {
            m_AlphaAgentGroup = new SimpleMultiAgentGroup();
            m_BravoAgentGroup = new SimpleMultiAgentGroup();

            foreach(var agent in agentlist)
            {
                //Quaternionにしないとよくわからないことになる。
                Quaternion qRot = Quaternion.Euler(agent.initRot);
                GameObject obj = Instantiate(agent.obj, agent.initPos, qRot);
                agent.agent = obj.GetComponent<AgentChan>();
                if(agent.teamID == (int)Team.Alpha)
                {
                    m_AlphaAgentGroup.RegisterAgent(agent.agent);
                }
                else
                {
                    m_BravoAgentGroup.RegisterAgent(agent.agent);
                }
            }
        }

        public void Hitted(string Tag)
        {
            if(Tag == AlphaCharacterTag)
            {
                m_AlphaAgentGroup.AddGroupReward(GetHitReward);
                m_BravoAgentGroup.AddGroupReward(GiveHitReward);
            }
            else
            {
                m_AlphaAgentGroup.AddGroupReward(GiveHitReward);
                m_BravoAgentGroup.AddGroupReward(GetHitReward);
            }
        }
    }

}