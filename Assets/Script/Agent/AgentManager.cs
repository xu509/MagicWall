using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//
//  实体管理器
//
public class AgentManager : Singleton<AgentManager>
{

    #region Parameter

    // 主管理器
    private MagicWallManager _manager;

    //  当前界面的 agents
    List<FlockAgent> _agents;
    public List<FlockAgent> Agents { get { return _agents; } }

    //  正在操作的 agents
    List<FlockAgent> effectAgent;
    public List<FlockAgent> EffectAgent { get { return effectAgent; } }

    //点开的卡片 最多8个
    public List<CardAgent> cardAgents;

    //
    //  Paramater UI
    //
    RectTransform _operationPanel;

    #endregion

    //
    //  single pattern
    // 
    void Awake() {
        _manager = MagicWallManager.Instance;
        effectAgent = new List<FlockAgent>();
        _agents = new List<FlockAgent>();
        cardAgents = new List<CardAgent>();
        _operationPanel = GameObject.Find("OperatePanel").GetComponent<RectTransform>();
    }

    //
    //  Constructor
    //
    protected AgentManager() { }


    #region Public Methods

    //
    //  清理所有的agents
    //
    public void ClearAgent(FlockAgent agent)
    {
        if (!agent.IsChoosing)
        {
            Destroy(agent.gameObject);
            _agents.Remove(agent);
        }
    }

    //
    //  清理所有的agents
    //
    public void ClearAgents()
    {
        foreach (FlockAgent agent in Agents.ToArray())
        {
            ClearAgent(agent);
        }
        //agents.Clear(); //清理 agent 袋
        _agents = new List<FlockAgent>();
    }

    //
    //  清理
    //
    public void ClearAgentsByList(List<FlockAgent> flockAgents)
    {
        foreach (FlockAgent agent in flockAgents)
        {
            ClearAgent(agent);
        }
    }

    //
    // 更新所有的 agent
    //
    public void UpdateAgents()
    {
        foreach (FlockAgent ag in _agents)
        {
            ag.updatePosition();
        }
    }


    //
    //  移除效果列表中的项
    //
    public bool RemoveItemFromEffectItems(CardAgent agent) {
        return effectAgent.Remove(agent);
    }

    //
    //  移除效果列表中的项
    //
    public void AddEffectItem(CardAgent agent)
    {
        effectAgent.Add(agent);
    }


    //
    //  移除效果列表中的项
    //
    public void AddItem(FlockAgent agent)
    {
        _agents.Add(agent);
    }


    // 初始化
    public void Reset() {
        //  清理 Agents
        for (int i = 0; i < Agents.Count; i++) {
            FlockAgent agent = Agents[i];
            Destroy(agent.gameObject);
        }
        _agents = new List<FlockAgent>();

        //  清理 Effect Agents
        for (int i = 0; i < EffectAgent.Count; i++) {
            FlockAgent agent = EffectAgent[i];
            Destroy(agent.gameObject);
        }
        effectAgent = new List<FlockAgent>();

        // 清理 Card Agent
        cardAgents = new List<CardAgent>();

    }


   
    #endregion


    #region Private methods

    #endregion

}