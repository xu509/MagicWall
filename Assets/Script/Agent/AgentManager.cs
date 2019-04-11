using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private MagicWallManager manager;

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
        manager = MagicWallManager.Instance;
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
                                    manager.agentPrefab,
                                    manager.mainPanel
                                    );
        //  命名
        newAgent.name = "Agent(" + row + "," + column + ")";

        //  获取rect引用
        RectTransform rectTransform = newAgent.GetComponent<RectTransform>();

        //  定出生位置
        Vector2 postion = new Vector2(gen_x, gen_y);
        rectTransform.anchoredPosition = postion;

        //  定面板位置
        Vector2 ori_position = new Vector2(ori_x, ori_y);
        newAgent.GenVector2 = postion;

        //  初始化内容
        newAgent.Initialize(ori_position, postion, row, column);
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
    //  清理所有的agents
    //
    public void ClearAgents() {
        foreach (FlockAgent agent in agents)
        {
            if (!agent.IsChoosing)
            {
                Destroy(agent.gameObject);
            }
        }
        agents.Clear(); //清理 agent 袋
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
            // 将选中的 agent 放入操作层
            agent.transform.parent = operationPanel;
            Vector2 positionInMainPanel = agent.GetComponent<RectTransform>().anchoredPosition;
            agent.GetComponent<RectTransform>().DOAnchorPos(positionInMainPanel, Time.deltaTime);

            // 将被选中的 agent 加入列表
            agent.IsChoosing = true;
            effectAgent.Add(agent);

            // 选中后的动画效果，逐渐变大
            Vector2 newSizeDelta = new Vector2(agent.Width * 2f, agent.Height * 2f);
            agent.AgentRectTransform.DOSizeDelta(newSizeDelta, 0.5f)
                .OnUpdate(() => DoSizeDeltaUpdateCallBack(agent))
                .SetEase(Ease.OutQuint);
                ;

            UpdateAgents ();
        }
        else
        {
            agent.transform.parent = manager.mainPanel;
            agent.IsChoosing = false;
            effectAgent.Remove(agent);

            Vector2 newSizeDelta = new Vector2(agent.Width / 2f, agent.Height / 2f);
            agent.AgentRectTransform.DOSizeDelta(newSizeDelta, 1f).OnUpdate(() => DoSizeDeltaUpdateCallBack(agent));
            agent.GetComponent<RectTransform>().DOAnchorPos(agent.OriVector2, 2f);
        }
    }

    void DoSizeDeltaUpdateCallBack(FlockAgent agent)
    {
        //Debug.Log(agent.AgentRectTransform.sizeDelta.x);
        agent.Width = agent.AgentRectTransform.sizeDelta.x;
        agent.Height = agent.AgentRectTransform.sizeDelta.y;


    }




}