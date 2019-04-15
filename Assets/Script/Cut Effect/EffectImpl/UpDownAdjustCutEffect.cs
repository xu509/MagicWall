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
    private float the_time;
    private float dur_time; // 持续时间

    private float generate_agent_interval = 0.3f; // 生成的间隔
    private float last_generate_time = 0f; // 最后生成的时间


    //
    //	初始化 MagicWallManager
    //
    public override void Create() {
        manager = MagicWallManager.Instance;

        // 初始化内容
        StartingDurTime = 1f;
        this.dur_time = StartingDurTime; // 动画时长2秒

        // 获取栅格信息
        row = manager.row;
        int h = (int)manager.mainPanel.rect.height;
        int w = (int)manager.mainPanel.rect.width;

        int gap = 10;

        int itemWidth = h / row - gap;
        int itemHeight = itemWidth;

        // 从后往前的效果列数不需要很多
        column = w / itemWidth;

        for (int j = 0; j < column; j++) { 
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
                else {
                    //奇数列向上偏移itemHeight
                    gen_y = ori_y + itemHeight + gap + i * gap;
                }
                gen_x = ori_x; //横坐标不变

                // 定义出生位置与目标位置
                Vector2 ori_position = new Vector2(ori_x, ori_y);
                Vector2 gen_position = new Vector2(gen_x, gen_y);
                
                // 生成 agent
                FlockAgent go = AgentManager.Instance.CreateNewAgent(gen_x, gen_y, ori_x, ori_y, i + 1, j + 1,itemWidth,itemHeight);

                // agent 一定时间内从透明至无透明
                go.GetComponentInChildren<Image>().DOFade(0, dur_time).From();
            }
        }

        // 初始化完成后更新时间
        the_time = Time.time;

    }


    public override void Starting() {

        for (int i = 0; i < AgentManager.Instance.Agents.Count; i++)
        {
            FlockAgent agent = AgentManager.Instance.Agents[i];
            Vector2 agent_vector2 = agent.GenVector2;
            Vector2 ori_vector2 = agent.OriVector2;

            // 获取此 agent 需要的动画时间
            float run_time = dur_time - 0.1f;

            // 当前总运行的时间;
            float time = Time.time - the_time;
            
            // 如果总动画时间超出 agent 需要的动画时间，则不进行处理
            if (time > run_time)
            {
                continue;
                //Debug.Log(agent.name);
            }

            float t = (Time.time - the_time) / run_time;
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

	public override void Destorying()
	{
		throw new System.NotImplementedException();
	}
}
