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
    //
    //  Paramater
    //

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
    RectTransform operationPanel;


    //
    //  single pattern
    // 
    void Awake() {
        _manager = MagicWallManager.Instance;
        effectAgent = new List<FlockAgent>();
        agents = new List<FlockAgent>();
        operationPanel = GameObject.Find("OperatePanel").GetComponent<RectTransform>();
    }

    //
    //  Constructor
    //
    protected AgentManager() { }



    //
    //  创建一个新Agent
    //
    public FlockAgent CreateNewAgent(float gen_x, float gen_y, float ori_x, float ori_y, int row, int column, float width, float height)
    {
        //  创建 Agent
        FlockAgent newAgent = Instantiate(
                                    _manager.agentPrefab,
                                    _manager.mainPanel
                                    );
        //  命名
        newAgent.name = "Agent(" + (row + 1) + "," + (column + 1) + ")";

        //  获取rect引用
        RectTransform rectTransform = newAgent.GetComponent<RectTransform>();

        //  定出生位置
        Vector2 postion = new Vector2(gen_x, gen_y);
        rectTransform.anchoredPosition = postion;

        //  定面板位置
        Vector2 ori_position = new Vector2(ori_x, ori_y);
        newAgent.GenVector2 = postion;

        //  初始化内容
        newAgent.Initialize(ori_position, postion, row + 1, column + 1);
        newAgent.Width = width;
        newAgent.Height = height;

        // 调整agent的长与宽
        Vector2 sizeDelta = new Vector2(width, height);
        rectTransform.sizeDelta = sizeDelta;

        // 调整显示颜色

        // 调整 collider
        BoxCollider2D boxCollider2D = newAgent.GetComponent<BoxCollider2D>();
        boxCollider2D.size = new Vector2(width, height);


        //  添加到组件袋
        Agents.Add(newAgent);
        return newAgent;
    }

    //
    //  创建items
    //
    public FlockAgent CreateNewAgent(int row, int column)
    {
        row = row - 1;
        column = column - 1;

        // width
        int h = (int)_manager.mainPanel.rect.height;
        //int w = (int)_manager.mainPanel.rect.width;
        int gap = 10;

        float itemHeight = h / _manager.Row - gap;
        float itemWidth = itemHeight;

        float x = column * (itemWidth + gap) + itemWidth / 2;
        float y = row * (itemHeight + gap) + itemHeight / 2;

        return CreateNewAgent(x, y, x, y, row, column, itemWidth, itemHeight);
    }


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
    public void ClearAgents() {
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
            Debug.Log("Click it : " + positionInMainPanel);

            ////  将选中的 agent 放入操作层
            //rect.transform.SetParent(operationPanel);

            //  移到后方
            Debug.Log("New Position: " + rect.anchoredPosition + " [offset] - " + offset);
            Vector3 to = new Vector3(rect.anchoredPosition.x, rect.anchoredPosition.y, 1000);
            rect.DOAnchorPos3D(to, 1f);

            
            //  创建新的对应卡片





            //// 将选中的 agent 放入操作层
            //agent.transform.parent = operationPanel;
            //Vector2 positionInMainPanel = agent.GetComponent<RectTransform>().anchoredPosition;
            //agent.GetComponent<RectTransform>().DOAnchorPos(positionInMainPanel, Time.deltaTime);

            //// 将被选中的 agent 加入列表
            //agent.IsChoosing = true;
            //effectAgent.Add(agent);

            //// 选中后的动画效果，逐渐变大
            //Vector2 newSizeDelta = new Vector2(agent.Width * 3f, agent.Height * 3f);
            //agent.AgentRectTransform.DOSizeDelta(newSizeDelta, 0.3f)
            //    .OnUpdate(() => DoSizeDeltaUpdateCallBack(agent))
            //    .SetEase(Ease.OutQuint);
            //    ;
            //Image i = agent.GetComponentInChildren<Image>();
            //i.DOFade(0.3f, 0.3f);


        }
        else
        {
            agent.transform.parent = _manager.mainPanel;
            agent.IsChoosing = false;
            effectAgent.Remove(agent);

            Vector2 newSizeDelta = new Vector2(agent.Width / 3f, agent.Height / 3f);
            agent.AgentRectTransform.DOSizeDelta(newSizeDelta, 0.3f).OnUpdate(() => DoSizeDeltaUpdateCallBack(agent));
            agent.GetComponent<RectTransform>().DOAnchorPos(agent.OriVector2, 2f);
        }
    }

    void DoSizeDeltaUpdateCallBack(FlockAgent agent)
    {
        //Debug.Log(agent.AgentRectTransform.sizeDelta.x);
        agent.Width = agent.AgentRectTransform.sizeDelta.x;
        agent.Height = agent.AgentRectTransform.sizeDelta.y;
        UpdateAgents();

    }




}