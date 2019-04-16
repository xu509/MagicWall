using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 过场效果 5，左右校准
public class LeftRightAdjustCutEffect : CutEffect
{
    private MagicWallManager manager;

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
        StartingDurTime = 2f;
        _startingTimeWithOutDelay = StartingDurTime;
        DestoryDurTime = 0.5f;

        //  设置显示的时间
        string t = DaoService.Instance.GetConfigByKey(AppConfig.KEY_CutEffectDuring_LeftRightAdjust).Value;
        DisplayDurTime = AppUtils.ConvertToFloat(t);

        // 获取Display的动画
        DisplayBehavior = new GoLeftDisplayBehavior();
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
                // 定义源位置
                float ori_x = j * (itemHeight + gap) + itemHeight / 2;
                float ori_y = i * (itemWidth + gap) + itemWidth / 2;

                // 获取参照点
                int middleY = row / 2;
                int middleX = column / 2;

                // 定义出生位置
                float gen_x, gen_y;

                // 计算出生位置与延时时间
                float delay;
                if (i < middleY)
                {
                    delay = (System.Math.Abs(middleY - i)) * 0.3f;
                    gen_x = (column + j) * (itemWidth + gap) + itemWidth / 2;
                }
                else
                {
                    delay = (System.Math.Abs(middleY - i) + 1) * 0.3f;
                    gen_x = -(column - j) * (itemWidth + gap) + itemWidth / 2;
                }
                gen_y = ori_y; //纵坐标不变

                // 定义出生位置与目标位置
                Vector2 ori_position = new Vector2(ori_x, ori_y);
                Vector2 gen_position = new Vector2(gen_x, gen_y);

                // 生成 agent
                FlockAgent go = AgentManager.Instance.CreateNewAgent(gen_x, gen_y, ori_x, ori_y, i + 1, j + 1, itemWidth, itemHeight);
                go.Delay = delay;
                go.DelayTime = delay;
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
                // 定义源位置
                float ori_x = j * (itemHeight + gap) + itemHeight / 2;
                float ori_y = i * (itemWidth + gap) + itemWidth / 2;

                // 获取参照点
                int middleY = row / 2;
                int middleX = column / 2;

                // 定义出生位置
                float gen_x, gen_y;

                // 计算出生位置与延时时间
                float delay;
                if (i < middleY)
                {
                    delay = (System.Math.Abs(middleY - i)) * 0.3f;
                    gen_x = (column + j) * (itemWidth + gap) + itemWidth / 2;
                }
                else
                {
                    delay = (System.Math.Abs(middleY - i) + 1) * 0.3f;
                    gen_x = -(column - j) * (itemWidth + gap) + itemWidth / 2;
                }
                gen_y = ori_y; //纵坐标不变

                // 生成 agent
                FlockAgent go = AgentManager.Instance.CreateNewAgent(gen_x, gen_y, ori_x, ori_y, i + 1, j + 1, itemWidth, itemHeight);
                go.Delay = delay;
                go.DelayTime = delay;
            }
        }
    }


    public override void Starting() {

        for (int i = 0; i < AgentManager.Instance.Agents.Count; i++)
        {
            FlockAgent agent = AgentManager.Instance.Agents[i];
            Vector2 agent_vector2 = agent.GenVector2;
            Vector2 ori_vector2 = agent.OriVector2;

            // 延时的时间
            float delay_time = agent.DelayTime;

            // 获取此 agent 需要的动画时间
            float run_time = _startingTimeWithOutDelay - delay_time - _timeBetweenStartAndDisplay;

            // 当前总运行的时间;
            float time = Time.time - StartTime;
            
            // 如果总动画时间超出 agent 需要的动画时间，则不进行处理
            if (time > StartingDurTime || time < delay_time)
            {
                continue;
                //Debug.Log(agent.name);
            }

            float t = (time - delay_time) / run_time;
            Vector2 to = Vector2.Lerp(agent_vector2, ori_vector2, t);

            agent.NextVector2 = to;
            agent.updatePosition();
        }


    }

    public override void OnStartingCompleted(){
        AgentManager.Instance.UpdateAgents();
    }

}
