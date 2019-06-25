using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 浮动体块
/// </summary>
public class AgentManager : MonoBehaviour
{
    
    /// <summary>
    ///     普通浮动块
    /// </summary>
    [SerializeField] FlockAgent _flockAgentPrefab;
    
    /// <summary>
    ///     十字浮动块
    /// </summary>
    [SerializeField] CardAgent _crossCardgentPrefab;
    
    /// <summary>
    ///     滑动浮动块
    /// </summary>
    [SerializeField] CardAgent _sliceCardgentPrefab;

    /// <summary>
    /// 普通浮动块容器
    /// </summary>
    [SerializeField] RectTransform _flockContainer;


    /// <summary>
    /// 操作卡片块容器
    /// </summary>
    [SerializeField] RectTransform _cardContainer;




    // 主管理器
    private MagicWallManager _manager;

    /// <summary>
    /// 普通浮动块对象池
    /// </summary>
    private FlockAgentPool<FlockAgent> _flockAgentPool;

    /// <summary>
    /// 十字展开操作块对象池
    /// </summary>
    private FlockAgentPool<CrossCardAgent> _crossCardAgentPool;

    /// <summary>
    /// 滑动操作块对象池
    /// </summary>
    private FlockAgentPool<SliceCardAgent> _sliceCardAgentPool;



    #region 业务逻辑相关属性
    //  当前界面的 agents
    List<FlockAgent> _agents;
    public List<FlockAgent> Agents { get { return _agents; } }

    //  正在操作的 agents
    List<FlockAgent> effectAgent;
    public List<FlockAgent> EffectAgent { get { return effectAgent; } }
    #endregion


    //
    //  single pattern
    // 
    void Awake() {

    }

    //
    //  Constructor
    //
    protected AgentManager() { }


    #region Public Methods

    public void Init(MagicWallManager manager) {
        effectAgent = new List<FlockAgent>();
        _agents = new List<FlockAgent>();
        _manager = manager;

        PrepareAgentPool();
    }

    /// <summary>
    ///     准备对象池
    /// </summary>
    private void PrepareAgentPool() {
        _flockAgentPool = FlockAgentPool<FlockAgent>.GetInstance(_manager.managerConfig.FlockPoolSize);
        _flockAgentPool.Init(_flockAgentPrefab, _flockContainer);

        _crossCardAgentPool = FlockAgentPool<CrossCardAgent>.GetInstance(_manager.managerConfig.CardPoolSize);
        _crossCardAgentPool.Init(_crossCardgentPrefab as CrossCardAgent, _cardContainer);

        _sliceCardAgentPool = FlockAgentPool<SliceCardAgent>.GetInstance(_manager.managerConfig.CardPoolSize);
        _sliceCardAgentPool.Init(_sliceCardgentPrefab as SliceCardAgent, _cardContainer);
    }


    //
    //  清理所有的agents
    //
    public void ClearAgent(FlockAgent agent)
    {
        if (!agent.IsChoosing)
        {
            _flockAgentPool.ReleaseObj(agent);
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
    //  移除效果列表中的项
    //
    public bool RemoveItemFromEffectItems(CardAgent agent) {
        DestoryAgent(agent);
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
            _flockAgentPool.ReleaseObj(agent);
        }
        _agents = new List<FlockAgent>();

        //  清理 Effect Agents
        for (int i = 0; i < EffectAgent.Count; i++) {
            FlockAgent agent = EffectAgent[i];
            DestoryAgent(agent);
        }
        effectAgent = new List<FlockAgent>();
    }



    // 持续更新
    public void Run() {
        if (Agents.Count > 0) {
            foreach (FlockAgent agent in Agents) {
                agent.updatePosition(); //检测位置并计算
            }
        }

        // 检测打开的个数大于8个时，关闭早的
        if (EffectAgent.Count > _manager.managerConfig.SelectedItemMaxCount) {
            // 此时得到的是CardAgent
            CardAgent effectAgent = EffectAgent[0] as CardAgent;
            if (effectAgent.CardStatus != CardStatusEnum.DESTORYING_STEP_SCEOND 
                && effectAgent.CardStatus != CardStatusEnum.DESTORYED) {
                effectAgent.DoCloseDirect();
            }
            //EffectAgent[0].GetCardAgent.DoCloseDirect();
        }

    }

    public void DestoryAgent(FlockAgent agent) {
        if (agent.IsCard)
        {
            if (agent.type == MWTypeEnum.Enterprise)
            {
                _crossCardAgentPool.ReleaseObj(agent as CrossCardAgent);
            }
            else if (agent.type == MWTypeEnum.Activity || agent.type == MWTypeEnum.Product)
            {
                _sliceCardAgentPool.ReleaseObj(agent as SliceCardAgent);
            }
        }
    }



    #endregion


    public FlockAgent GetFlockAgent() {
        return _flockAgentPool.GetObj();
    }

    public CrossCardAgent GetCrossCardAgent()
    {
        return _crossCardAgentPool.GetObj();
    }

    public SliceCardAgent GetSliceCardAgent()
    {
        return _sliceCardAgentPool.GetObj();
    }

}