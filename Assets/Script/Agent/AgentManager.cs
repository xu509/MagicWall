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
    List<FlockAgent> agents;
    public List<FlockAgent> Agents { get { return agents; } }

    //  正在操作的 agents
    List<FlockAgent> effectAgent;
    public List<FlockAgent> EffectAgent { get { return effectAgent; } }

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
        agents = new List<FlockAgent>();
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
            agents.Remove(agent);
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
        foreach (FlockAgent ag in agents)
        {
            ag.updatePosition();
        }
    }

    //
    //  选择某个
    //
    public void DoChosenItem(FlockAgent agent)
    {
        if (!agent.IsChoosing)
        {
            agent.IsChoosing = true;
            float offset = MagicWallManager.Instance.PanelOffsetX;

            //  先缩小（向后退）
            RectTransform rect = agent.GetComponent<RectTransform>();
            Vector2 positionInMainPanel = rect.anchoredPosition;

            // 将选中的 agent 放入操作层
            //rect.transform.SetParent(operationPanel);

            //  移到后方、缩小、透明
            rect.DOScale(0.2f, 0.3f);
            Vector3 to = new Vector3(rect.anchoredPosition.x, rect.anchoredPosition.y, 200);
            Vector3 cardGenPosition = new Vector3(rect.anchoredPosition.x - _manager.PanelOffsetX - 1f, rect.anchoredPosition.y - _manager.PanelOffsetY - 1f, 200);

            // 完成缩小与移动后创建十字卡片
            rect.DOAnchorPos3D(to, 0.3f).OnComplete(() => DoChooseCompletedCallBack(agent, cardGenPosition)); ;

            // TODO: 当两个选择框体相近时，需要处理

        }
    }

    //
    //  销毁card
    //
    public void DoDestoryCardAgent(CardAgent cardAgent) {
        //  如果场景没有变，则回到原位置
        if (cardAgent.SceneIndex == _manager.SceneIndex)
        {
            //恢复并归位
            // 缩到很小很小
            RectTransform rect = cardAgent.GetComponent<RectTransform>();

            //  移到后方、缩小、透明
            rect.DOScale(0.2f, 1f);

            //  获取位置
            FlockAgent oriAgent = cardAgent.OriginAgent;

            Vector3 to = new Vector3(oriAgent.OriVector2.x - _manager.PanelOffsetX, oriAgent.OriVector2.y - _manager.PanelOffsetY, 200);

            rect.DOAnchorPos3D(to, 1f).OnComplete(() => DoDestoryCompletedCallBack(cardAgent)); ;


        }
        //  直接消失
        else {
            // 慢慢缩小直到消失
            Vector3 vector3 = Vector3.zero;

            cardAgent.GetComponent<RectTransform>().DOScale(vector3, 1.5f)
                .OnUpdate(() => DoSizeDeltaUpdateCallBack(cardAgent))
                .OnComplete(() => DoScaleCompletedCallBack(cardAgent, vector3));
        }

        cardAgent.OriginAgent.IsChoosing = false;
        
    }

    //
    //  缩放代理
    //
    public void DoScaleAgency(FlockAgent agent, Vector3 scaleVector, float second)
    {
        agent.GetComponent<RectTransform>().DOScale(scaleVector, second)
            .OnUpdate(() => DoSizeDeltaUpdateCallBack(agent));

    }

    /// <summary>
    ///     销毁卡片
    /// </summary>
    public void DestoryCardAgent(CardAgent cardAgent) {
        Destroy(cardAgent.gameObject);
    }
    


    #endregion



    #region Private methods
    void DoSizeDeltaUpdateCallBack(FlockAgent agent)
    {
        //Debug.Log(agent.AgentRectTransform.sizeDelta.x);
        agent.Width = agent.AgentRectTransform.sizeDelta.x;
        agent.Height = agent.AgentRectTransform.sizeDelta.y;
        UpdateAgents();
    }

    void DoScaleCompletedCallBack(FlockAgent agent, Vector3 vector3Scale)
    {
        // 进行销毁
        if (vector3Scale == Vector3.zero) {
            effectAgent.Remove(agent);
            Destroy(agent);

            DestoryCardAgent(agent as CardAgent);
        }

    }

    //
    //  选中后的 Callback
    //
    private void DoDestoryCompletedCallBack(CardAgent agent) {

        //  使卡片消失
        agent.gameObject.SetActive(false);
        effectAgent.Remove(agent);

        //  将原组件启用
        FlockAgent originAgent = agent.OriginAgent;
        originAgent.gameObject.SetActive(true);

        // 调整位置
        RectTransform rect = originAgent.GetComponent<RectTransform>();
        RectTransform cardRect = agent.GetComponent<RectTransform>();

        rect.anchoredPosition3D = new Vector3(cardRect.anchoredPosition3D.x + _manager.PanelOffsetX,
            cardRect.anchoredPosition3D.y + _manager.PanelOffsetY,
            cardRect.anchoredPosition3D.z);

        // 恢复原位
        Vector3 to = new Vector3(originAgent.OriVector2.x, originAgent.OriVector2.y, 0);
        rect.DOAnchorPos3D(to, 0.3f);

        Vector3 scaleVector3 = Vector3.one;
        DoScaleAgency(originAgent, scaleVector3, 1f);

        DestoryCardAgent(agent);
    }

    //
    //  选中后的 DoDestoryCompletedCallBack
    //
    private void DoChooseCompletedCallBack(FlockAgent agent, Vector3 pos)
    {

        // 使原组件消失
        agent.gameObject.SetActive(false);

        // 此处需要区分
        CardAgent cardAgent;
        if (_manager.CurrentScene.GetContentType() == SceneContentType.env)
        {
            cardAgent = CreateCrossAgent(pos, agent);
        }
        else {
            cardAgent = CreateSliceAgent(pos, agent);
        }

        Vector3 to = new Vector3(pos.x, pos.y, 0);
        cardAgent.GetComponent<RectTransform>().DOAnchorPos3D(to, 0.3f);

        Vector3 scaleVector3 = new Vector3(3.68f, 3.68f, 3.68f);
        DoScaleAgency(cardAgent, scaleVector3, 0.5f);

    }


    //
    //  创建十字选择框体
    //
    private CardAgent CreateCrossAgent(Vector3 genPos,FlockAgent flockAgent) {
        //  创建 Agent
        CrossCardAgent crossCardAgent = Instantiate(
                                    _manager.crossCardgent,
                                    _operationPanel
                                    ) as CrossCardAgent;

        //  命名
        crossCardAgent.name = "Choose(" + flockAgent.name +  ")";

        //  获取rect引用
        RectTransform rectTransform = crossCardAgent.GetComponent<RectTransform>();

        //  定出生位置
        rectTransform.anchoredPosition3D = genPos;

        //  定义大小
        Vector2 sizeDelta = new Vector2(flockAgent.Width, flockAgent.Height);
        rectTransform.sizeDelta = sizeDelta;

        //  定义缩放
        Vector3 scaleVector3 = new Vector3(0.2f,0.2f,0.2f);
        rectTransform.localScale = scaleVector3;

        //  初始化内容
        crossCardAgent.Width = rectTransform.rect.width;
        crossCardAgent.Height = rectTransform.rect.height;

        //  添加原组件
        crossCardAgent.OriginAgent = flockAgent;

        //  配置scene
        crossCardAgent.SceneIndex = _manager.SceneIndex;

        // 添加到effect agent
        EffectAgent.Add(crossCardAgent);

        // 初始化 CrossAgent 数据
        crossCardAgent.InitData();

        return crossCardAgent;
    }

    //
    //  创建滑动框体
    //
    private CardAgent CreateSliceAgent(Vector3 genPos, FlockAgent flockAgent)
    {
        //  创建 Agent
        SliceCardAgent sliceCardAgent = Instantiate(
                                    _manager.sliceCardgent,
                                    _operationPanel
                                    ) as SliceCardAgent;

        //  命名
        sliceCardAgent.name = "Choose(" + flockAgent.name + ")";

        //  获取rect引用
        RectTransform rectTransform = sliceCardAgent.GetComponent<RectTransform>();

        //  定出生位置
        rectTransform.anchoredPosition3D = genPos;

        //  定义大小
        Vector2 sizeDelta = new Vector2(flockAgent.Width, flockAgent.Height);
        rectTransform.sizeDelta = sizeDelta;

        //  定义缩放
        Vector3 scaleVector3 = new Vector3(0.2f, 0.2f, 0.2f);
        rectTransform.localScale = scaleVector3;

        //  初始化内容
        sliceCardAgent.Width = rectTransform.rect.width;
        sliceCardAgent.Height = rectTransform.rect.height;

        //  添加原组件
        sliceCardAgent.OriginAgent = flockAgent;

        //  配置scene
        sliceCardAgent.SceneIndex = _manager.SceneIndex;

        // 添加到effect agent
        EffectAgent.Add(sliceCardAgent);

        return sliceCardAgent;
    }

    #endregion

}