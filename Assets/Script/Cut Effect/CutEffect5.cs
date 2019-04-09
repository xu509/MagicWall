using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 过场效果 5，左右校准
public class CutEffect5 : CutEffect
{

    private MagicWallManager the_magicWallManager;
    private int row;
    private int column;
    private float the_time;
    private float dur_time; // 持续时间

    private float generate_agent_interval = 0.3f; // 生成的间隔
    private float last_generate_time = 0f; // 最后生成的时间


    //
    //	初始化 MagicWallManager
    //
    public override void init(MagicWallManager magicWallManager) {
        // 设置动画时间
        DurTime = 4f;
        this.dur_time = DurTime;

        // 初始化 manager
        the_magicWallManager = magicWallManager;

        // 获取栅格信息
        row = magicWallManager.row;
        int h = (int)magicWallManager.mainPanel.rect.height;
        int w = (int)magicWallManager.mainPanel.rect.width;

        int gap = 10;

        int itemWidth = h / row - gap;
        int itemHeight = itemWidth;
        the_magicWallManager.ItemWidth = itemWidth;

        // 从后往前的效果列数不需要很多
        column = w / itemWidth;

        for (int j = 0; j < column; j++) { 
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
                else {
                    delay = (System.Math.Abs(middleY - i) + 1) * 0.3f;
                    gen_x = -(column - j) * (itemWidth + gap) + itemWidth / 2;
                }
                gen_y = ori_y; //纵坐标不变

                // 定义出生位置与目标位置
                Vector2 ori_position = new Vector2(ori_x, ori_y);
                Vector2 gen_position = new Vector2(gen_x, gen_y);
                
                // 生成 agent
                FlockAgent go = magicWallManager.CreateNewAgent(gen_x, gen_y, ori_x, ori_y, i + 1, j + 1);
                go.Delay = delay;
                go.DelayTime = delay;


            }
        }

        // 初始化完成后更新时间
        the_time = Time.time;

    }


    public override void run() {

        for (int i = 0; i < the_magicWallManager.Agents.Count; i++)
        {
            FlockAgent agent = the_magicWallManager.Agents[i];
            Vector2 agent_vector2 = agent.GenVector2;
            Vector2 ori_vector2 = agent.OriVector2;

            // 延时的时间
            float delay_time = agent.DelayTime;

            // 获取此 agent 需要的动画时间
            float run_time = dur_time - agent.Delay - 0.1f;

            // 当前总运行的时间;
            float time = Time.time - the_time;
            
            // 如果总动画时间超出 agent 需要的动画时间，则不进行处理
            if (time > (run_time + delay_time) || time < delay_time)
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

    public override void OnCompleted(){
        the_magicWallManager.updateAgents();
    }


}
