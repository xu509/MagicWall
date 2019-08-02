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
    [SerializeField , Header("Prefab")] FlockAgent _flockAgentPrefab;
    
    /// <summary>
    ///     十字浮动块
    /// </summary>
    [SerializeField] CardAgent _crossCardgentPrefab;
    
    /// <summary>
    ///     滑动浮动块
    /// </summary>
    [SerializeField] CardAgent _sliceCardgentPrefab;

    /// <summary>
    ///     单例浮动块
    /// </summary>
    [SerializeField] CardAgent _singleCardPrefab;

    /// <summary>
    /// 普通浮动块容器
    /// </summary>
    [SerializeField , Header("Container")] RectTransform _flockContainer;

    /// <summary>
    ///    后层的浮动块容器
    /// </summary>
    [SerializeField] RectTransform _backContainer;

    /// <summary>
    ///    后层的浮动块容器
    /// </summary>
    [SerializeField] RectTransform _starContainer;


    /// <summary>
    /// 操作卡片块容器
    /// </summary>
    [SerializeField] RectTransform _cardContainer;




    // 主管理器
    private MagicWallManager _manager;

    /// <summary>
    ///     普通浮动块对象池
    /// </summary>
    private FlockAgentPool<FlockAgent> _flockAgentPool;

    /// <summary>
    ///     后层的对象池
    /// </summary>
    private FlockAgentInBackPool<FlockAgent> _flockAgentInBackPool;

    /// <summary>
    ///     Star的对象池
    /// </summary>
    private FlockAgentInStarPool<FlockAgent> _flockAgentInStarPool;

    /// <summary>
    /// 十字展开操作块对象池
    /// </summary>
    private FlockAgentPool<CrossCardAgent> _crossCardAgentPool;

    /// <summary>
    /// 滑动操作块对象池
    /// </summary>
    private FlockAgentPool<SliceCardAgent> _sliceCardAgentPool;

    /// <summary>
    /// 单例操作块对象池
    /// </summary>
    private FlockAgentPool<SingleCardAgent> _singleCardAgentPool;


    #region 业务逻辑相关属性
    //  当前界面的 agents
    List<FlockAgent> _agents;
    public List<FlockAgent> Agents { get { return _agents; } }

    //  正在操作的 agents
    List<FlockAgent> effectAgent;
    public List<FlockAgent> EffectAgent { get { return effectAgent; } }


    private bool runLock = false;
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

        _flockAgentInBackPool = FlockAgentInBackPool<FlockAgent>.GetInstance(_manager.managerConfig.FlockPoolSize / 2);
        _flockAgentInBackPool.Init(_flockAgentPrefab, _backContainer);

        _flockAgentInStarPool = FlockAgentInStarPool<FlockAgent>.GetInstance(_manager.managerConfig.StarEffectAgentsCount);
        _flockAgentInStarPool.Init(_flockAgentPrefab, _starContainer);

        _crossCardAgentPool = FlockAgentPool<CrossCardAgent>.GetInstance(_manager.managerConfig.CardPoolSize);
        _crossCardAgentPool.Init(_crossCardgentPrefab as CrossCardAgent, _cardContainer);

        _sliceCardAgentPool = FlockAgentPool<SliceCardAgent>.GetInstance(_manager.managerConfig.CardPoolSize);
        _sliceCardAgentPool.Init(_sliceCardgentPrefab as SliceCardAgent, _cardContainer);

        _singleCardAgentPool = FlockAgentPool<SingleCardAgent>.GetInstance(_manager.managerConfig.CardPoolSize);
        _singleCardAgentPool.Init(_singleCardPrefab as SingleCardAgent, _cardContainer);
    }


    //
    //  清理所有的agents
    //
    public void ClearAgent(FlockAgent agent)
    {
        if (!agent.IsChoosing)
        {
            if (agent.IsRecovering) {
                Debug.Log("[Recovering]" + agent.name + " Is Destory !");
            }           

            DestoryAgent(agent);
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

        _flockAgentPool.Reset();
        _flockAgentInBackPool.Reset();
        _sliceCardAgentPool.Reset();
        _crossCardAgentPool.Reset();
        _singleCardAgentPool.Reset();
    }



    // 持续更新
    public void Run() {
        if (!runLock) {
            runLock = true;
            if (Agents.Count > 0)
            {
                List<FlockAgent> recycleAgents = new List<FlockAgent>();

                foreach (FlockAgent agent in Agents)
                {
                    agent.updatePosition(); //检测位置并计算

                    // 判断是否需要回收
                    if (agent.CheckIsNeedRecycle()) {
                        recycleAgents.Add(agent);
                    }
                }

                foreach (FlockAgent agent in recycleAgents)
                {
                    ClearAgent(agent);
                }
            }

            // 检测打开的个数大于8个时，关闭早的
            if (EffectAgent.Count > _manager.managerConfig.SelectedItemMaxCount)
            {
                // 此时得到的是CardAgent
                CardAgent effectAgent = EffectAgent[0] as CardAgent;
                if (effectAgent.CardStatus != CardStatusEnum.DESTORYING_STEP_SCEOND
                    && effectAgent.CardStatus != CardStatusEnum.DESTORYED)
                {
                    effectAgent.DoCloseDirect();
                }
                //EffectAgent[0].GetCardAgent.DoCloseDirect();
            }
            runLock = false;
        }
    }

    /// <summary>
    /// 工具型方法，请勿在未判断业务逻辑时直接使用
    /// </summary>
    /// <param name="agent"></param>
    public void DestoryAgent(FlockAgent agent) {
        if (agent.IsCard)
        {
            if (agent.type == MWTypeEnum.Enterprise)
            {
                if (agent.enterpriseType == MWEnterpriseTypeEnum.Cross)
                {
                    _crossCardAgentPool.ReleaseObj(agent as CrossCardAgent);
                }
                else {
                    _singleCardAgentPool.ReleaseObj(agent as SingleCardAgent);
                }
            }
            else if (agent.type == MWTypeEnum.Activity || agent.type == MWTypeEnum.Product)
            {
                _sliceCardAgentPool.ReleaseObj(agent as SliceCardAgent);
            }
        }
        else {
            if (agent.agentContainerType == AgentContainerType.MainPanel)
            {
                _flockAgentPool.ReleaseObj(agent);
            }
            else if (agent.agentContainerType == AgentContainerType.BackPanel) {
                _flockAgentInBackPool.ReleaseObj(agent);
            }
            else {
                _flockAgentInStarPool.ReleaseObj(agent);
            }
        }
    }



    #endregion


    public FlockAgent GetFlockAgent(AgentContainerType type) {
        if (type == AgentContainerType.MainPanel)
        {
            return _flockAgentPool.GetObj();
        }
        else if (type == AgentContainerType.BackPanel) {
            return _flockAgentInBackPool.GetObj();
        }
        else
        {
            return _flockAgentInStarPool.GetObj();
        }
    }


    public CrossCardAgent GetCrossCardAgent()
    {
        return _crossCardAgentPool.GetObj();
    }

    public SliceCardAgent GetSliceCardAgent()
    {
        return _sliceCardAgentPool.GetObj();
    }

    public SingleCardAgent GetSingleCardAgent()
    {
        return _singleCardAgentPool.GetObj();
    }


}