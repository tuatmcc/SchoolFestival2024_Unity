using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class AgentEnvController : MonoBehaviour
{
    //インスペクターでプレイヤーを分ける必要性あり
    [SerializeField]
    private List<AgentChan> AgentChanList = new List<AgentChan>();

    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 25000;


    private SimpleMultiAgentGroup m_WhiteChanGroup;
    private SimpleMultiAgentGroup m_RedChanGroup;

    private int m_ResetTimer;

    // Start is called before the first frame update
    void Start()
    {
        m_WhiteChanGroup = new SimpleMultiAgentGroup();
        m_RedChanGroup = new SimpleMultiAgentGroup();
        foreach(var agent in AgentChanList)
        {
            if(agent.team == Team.White)
            {
                m_WhiteChanGroup.RegisterAgent(agent);
            }
            else
            {
                m_RedChanGroup.RegisterAgent(agent);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_ResetTimer+=1;
        if(m_ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            m_WhiteChanGroup.GroupEpisodeInterrupted();
            m_RedChanGroup.GroupEpisodeInterrupted();
        }
    }

    public void Hitted(Team hittedteam)
    {
        // Debug.LogWarning("hitted");
        if(hittedteam == Team.White)
        {
            m_WhiteChanGroup.AddGroupReward(-1.0f);
            m_RedChanGroup.AddGroupReward(2.0f);

        }
        else
        {
            m_WhiteChanGroup.AddGroupReward(2.0f);
            m_RedChanGroup.AddGroupReward(-1.0f);
        }
    }
}
