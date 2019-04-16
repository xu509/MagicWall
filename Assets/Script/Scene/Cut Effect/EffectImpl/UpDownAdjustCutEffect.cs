using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 过场效果 4，上下校准
public class UpDownAdjustCutEffect : CutEffect
{

    MagicWallManager manager;
    private int row;
    private int column;

    private float _startingTimeWithOutDelay;
    private float _timeBetweenStartAndDisplay = 0.5f; //完成启动动画与表现动画之间的时间


    //
    //  Init
    //
    protected override void Init()
    {
        //  获取持续时间
        StartingDurTime = 1f;
        _startingTimeWithOutDelay = StartingDurTime;
        DestoryDurTime = 0.5f;

        //  设置显示的时间
        string t = DaoService.Instance.GetConfigByKey(AppConfig.KEY_CutEffectDuring_UpDownAdjust).Value;
        DisplayDurTime = AppUtils.ConvertToFloat(t);

        // 获取Display的动画
        DisplayBehavior = new GoDownDisplayBehavior();
        DisplayBehavior.Init(sceneContentType);

        DestoryBehavior = new FadeOutDestoryBehavior();

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

        for (int j = 0; j < column; j++)
        {
            for (int i = 0; i < row; i++)
            {
                float ori_x = j * (itemHeight + gap) + itemHeight / 2;
                float ori_y = i * (itemWidth + gap) + itemWidth / 2;

                // 获取出生位置
                float gen_x, gen_y;

                // 计算移动的目标位置
                if (j % 2 == 0)
                {
                    //偶数列向下偏移itemHeight
                    gen_y = ori_y - itemHeight + gap;
                }
                else
                {
                    //奇数列向上偏移itemHeight
                    gen_y = ori_y + itemHeight + gap + i * gap;
                }
                gen_x = ori_x; //横坐标不变


                // 生成 agent
                FlockAgent go = AgentManager.Instance.CreateNewAgent(gen_x, gen_y, ori_x, ori_y, i + 1, j + 1, itemWidth, itemHeight);

                // agent 一定时间内从透明至无透明
                go.GetComponentInChildren<Image>().DOFade(0, StartingDurTime).From();
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

        for (int j = 0; j < column; j++)
        {
            for (int i = 0; i < row; i++)
            {
                float ori_x = j * (itemHeight + gap) + itemHeight / 2;
                float ori_y = i * (itemWidth + gap) + itemWidth / 2;

                // 获取出生位置
                float gen_x, gen_y;

                // 计算移动的目标位置
                if (j % 2 == 0)
                {
                    //偶数列向下偏移itemHeight
                    gen_y = ori_y - itemHeight + gap;
                }
                else
                {
                    //奇数列向上偏移itemHeight
                    gen_y = ori_y + itemHeight + gap + i * gap;
                }
                gen_x = ori_x; //横坐标不变

                // 定义出生位置与目标位置
                Vector2 ori_position = new Vector2(ori_x, ori_y);
                Vector2 gen_position = new Vector2(gen_x, gen_y);

                // 生成 agent
                FlockAgent go = AgentManager.Instance.CreateNewAgent(gen_x, gen_y, ori_x, ori_y, i + 1, j + 1, itemWidth, itemHeight);

                // agent 一定时间内从透明至无透明
                go.GetComponentInChildren<Image>().DOFade(0, StartingDurTime).From();
            }
        }
    }

    public override void Starting() {

        for (int i = 0; i < AgentManager.Instance.Agents.Count; i++)
        {
            FlockAgent agent = AgentManager.Instance.Agents[i];
            Vector2 agent_vector2 = agent.GenVector2;
            Vector2 ori_vector2 = agent.OriVector2;

            // 获取此 agent 需要的动画时间
            float run_time = StartingDurTime - _timeBetweenStartAndDisplay;

            // 当前总运行的时间;
            float time = Time.time - StartTime;
            
            // 如果总动画时间超出 agent 需要的动画时间，则不进行处理
            if (time > run_time)
            {
                continue;
                //Debug.Log(agent.name);
            }

            float t = (Time.time - StartTime) / run_time;
            Vector2 to = Vector2.Lerp(agent_vector2, ori_vector2, t);

            agent.NextVector2 = to;
            agent.updatePosition();
        }
    }

    public override void OnStartingCompleted(){
        AgentManager.Instance.UpdateAgents();
    }


	public void DOAnchorPosCompleteCallback(FlockAgent agent)
    {
        RectTransform rect = agent.GetComponent<RectTransform>();
        Image image = agent.GetComponentInChildren<Image>();

        rect.DOScale(1.5f, 0.2f);
        image.DOFade(0, 0.2F).OnComplete(() => DOFadeCompleteCallback(agent));

    }

    public void DOFadeCompleteCallback(FlockAgent agent)
    {
        agent.gameObject.SetActive(false);
        RectTransform rect = agent.GetComponent<RectTransform>();
        Image image = agent.GetComponentInChildren<Image>();
        rect.DOScale(1f, Time.deltaTime);
        image.DOFade(1, Time.deltaTime);

    }

}
