using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 过场效果 3 从后往前, 星空效果
public class StarsCutEffect : CutEffect
{
    MagicWallManager manager;

    private int row;
    private int column;

    private float generate_agent_interval = 0.3f; // 生成的间隔
    private float last_generate_time = 0f; // 最后生成的时间

    //
    //  Init
    //
    protected override void Init()
    {
        //  获取动画的持续时间
        StartingDurTime = 7f;
        DestoryDurTime = 0.5f;

        //  设置显示的时间
        string t = DaoService.Instance.GetConfigByKey(AppConfig.KEY_CutEffectDuring_Stars).Value;
        DisplayDurTime = AppUtils.ConvertToFloat(t);

        //  设置销毁
        DestoryBehavior = new FadeOutDestoryBehavior();

        //  设置运行时间点
        HasDisplaying = false;

        //  初始化 manager
        manager = MagicWallManager.Instance;
    }

    //
    //  创建产品 | Logo 
    //
    protected override void CreateProductOrLogo()
    {
        // 获取栅格信息
        row = manager.row;
        int h = (int)manager.mainPanel.rect.height;
        int w = (int)manager.mainPanel.rect.width;

        int gap = 10;

        int itemWidth = h / row - gap;
        int itemHeight = itemWidth;

        // 从后往前的效果列数不需要很多
        column = w / itemWidth;

        //从左往右，从上往下
        for (int j = 0; j < column; j++)
        {
            for (int i = 0; i < row; i++)
            {
                float x = j * (itemWidth + gap) + itemWidth / 2;
                float y = i * (itemHeight + gap) + itemHeight / 2;

                int middleX = (column - 1) / 2;

                // ori_x;ori_y
                float ori_x, ori_y;
                ori_x = x;
                ori_y = y;

                Vector2 ori_position = new Vector2(ori_x, ori_y);
                Vector2 gen_position = new Vector2(x, y);

                //				FlockAgent go = AgentGenerator.GetInstance ().generator (name, gen_position, ori_position, magicWallManager);
                FlockAgent go = AgentManager.Instance.CreateNewAgent(ori_x, ori_y, x, y, i + 1, j + 1, itemWidth, itemHeight);

                // 将agent的z轴定义在后方
                RectTransform rect = go.GetComponent<RectTransform>();
                Vector3 position = rect.anchoredPosition3D;
                rect.anchoredPosition3D = rect.anchoredPosition3D + new Vector3(0, 0, 300);

                go.gameObject.SetActive(false);
            }
        }
    }

    //
    //  创建活动
    //
    protected override void CreateActivity()
    {
        // 获取栅格信息
        row = manager.row;
        int h = (int)manager.mainPanel.rect.height;
        int w = (int)manager.mainPanel.rect.width;

        int gap = 10;

        int itemWidth = h / row - gap;
        int itemHeight = itemWidth;

        // 从后往前的效果列数不需要很多
        column = w / itemWidth;

        //从左往右，从上往下
        for (int j = 0; j < column; j++)
        {
            for (int i = 0; i < row; i++)
            {
                float x = j * (itemWidth + gap) + itemWidth / 2;
                float y = i * (itemHeight + gap) + itemHeight / 2;

                int middleX = (column - 1) / 2;

                // ori_x;ori_y
                float ori_x, ori_y;
                ori_x = x;
                ori_y = y;

                Vector2 ori_position = new Vector2(ori_x, ori_y);
                Vector2 gen_position = new Vector2(x, y);

                //				FlockAgent go = AgentGenerator.GetInstance ().generator (name, gen_position, ori_position, magicWallManager);
                FlockAgent go = AgentManager.Instance.CreateNewAgent(ori_x, ori_y, x, y, i + 1, j + 1, itemWidth, itemHeight);

                // 将agent的z轴定义在后方
                RectTransform rect = go.GetComponent<RectTransform>();
                Vector3 position = rect.anchoredPosition3D;
                rect.anchoredPosition3D = rect.anchoredPosition3D + new Vector3(0, 0, 300);

                go.gameObject.SetActive(false);
            }
        }
    }

    
    public override void Starting() {
        if (Time.time - last_generate_time > generate_agent_interval) {

            // 随机选择
            int count = AgentManager.Instance.Agents.Count;
            int index = Random.Range(0, count - 1);
            FlockAgent agent = AgentManager.Instance.Agents[index];
            agent.gameObject.SetActive(true);

            Vector3 to = new Vector3(agent.OriVector2.x, agent.OriVector2.y, 0);
            agent.GetComponent<RectTransform>().DOAnchorPos3D(to, 3f).OnComplete(() => DOAnchorPosCompleteCallback(agent));

            last_generate_time = Time.time;
        }

    }

    public override void OnStartingCompleted(){
        AgentManager.Instance.UpdateAgents();
    }


    #region Tween Callback
    public void DOAnchorPosCompleteCallback(FlockAgent agent)
    {
        RectTransform rect = agent.GetComponent<RectTransform>();
        Image image = agent.GetComponentInChildren<Image>();
        if (!agent.IsChoosing)
        {
            rect.DOScale(1.5f, 0.2f);
            image.DOFade(0, 0.2F).OnComplete(() => DOFadeCompleteCallback(agent));
        }
    }

    public void DOFadeCompleteCallback(FlockAgent agent)
    {
        agent.gameObject.SetActive(false);
        RectTransform rect = agent.GetComponent<RectTransform>();
        Image image = agent.GetComponentInChildren<Image>();
        rect.DOScale(1f, Time.deltaTime);
        image.DOFade(1, Time.deltaTime);
    }
	#endregion

}
