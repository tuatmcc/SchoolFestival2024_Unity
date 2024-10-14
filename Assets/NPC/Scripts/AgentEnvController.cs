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
                if(agent.teamID == (int)Team.Alpha)
                {
                    m_AlphaAgentGroup.RegisterAgent(agent.agent);
                }
                else
                {
                    m_BravoAgentGroup.RegisterAgent(agent.agent);
                }
                agent.agent = obj.GetComponent<AgentChan>();
            }
        }
    }

}