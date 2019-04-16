using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 过场效果 2 中间散开 
public class MidDisperseCutEffect : CutEffect
{
    MagicWallManager manager;

    private int row;
    private int column;
    private float _startDelayTime = 0f;  //启动的延迟时间
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
        string t = DaoService.Instance.GetConfigByKey(AppConfig.KEY_CutEffectDuring_MidDisperseAdjust).Value;
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
        row = manager.row;

        int h = (int)manager.mainPanel.rect.height;
        int w = (int)manager.mainPanel.rect.width;

        int gap = 10;

        int itemWidth = h / row - gap;
        int itemHeight = itemWidth;

        column = Mathf.CeilToInt(w / itemWidth);


        //从下往上，从左往右
        for (int j = 0; j < column; j++)
        {
            for (int i = 0; i < row; i++)
            {
                float x = j * (itemWidth + gap) + itemWidth / 2;
                float y = i * (itemHeight + gap) + itemHeight / 2;
                int middleX = (column - 1) / 2;
                float delay = System.Math.Abs(middleX - i) * 0.05f;

                // ori_x;ori_y
                float ori_x, ori_y;

                ori_x = middleX * (itemWidth + gap) + (itemWidth / 2);
                ori_y = y + itemWidth;

                //				FlockAgent go = AgentGenerator.GetInstance ().generator (name, gen_position, ori_position, magicWallManager);
                FlockAgent go = AgentManager.Instance.CreateNewAgent(ori_x, ori_y, x, y, i, j, itemWidth, itemHeight);
                go.transform.SetSiblingIndex(Mathf.Abs(middleX - j));
                go.Delay = delay;

                go.GetComponentInChildren<Image>().DOFade(0, StartingDurTime + delay).From();

                // 获取启动动画的延迟时间
                if (delay > _startDelayTime)
                {
                    _startDelayTime = delay;
                }
            }
        }

        // 调整启动动画的时间
        StartingDurTime += _startDelayTime;
    }

    //
    //  创建活动
    //
    protected override void CreateActivity()
    {
        row = manager.row;

        int h = (int)manager.mainPanel.rect.height;
        int w = (int)manager.mainPanel.rect.width;

        int gap = 10;

        int itemWidth = h / row - gap;
        int itemHeight = itemWidth;

        column = Mathf.CeilToInt(w / itemWidth);


        //从下往上，从左往右
        for (int j = 0; j < column; j++)
        {
            for (int i = 0; i < row; i++)
            {
                float x = j * (itemWidth + gap) + itemWidth / 2;
                float y = i * (itemHeight + gap) + itemHeight / 2;
                int middleX = (column - 1) / 2;
                float delay = System.Math.Abs(middleX - i) * 0.05f;

                // ori_x;ori_y
                float ori_x, ori_y;

                ori_x = middleX * (itemWidth + gap) + (itemWidth / 2);
                ori_y = y + itemWidth;

                //				FlockAgent go = AgentGenerator.GetInstance ().generator (name, gen_position, ori_position, magicWallManager);
                FlockAgent go = AgentManager.Instance.CreateNewAgent(ori_x, ori_y, x, y, i, j, itemWidth, itemHeight);
                go.transform.SetSiblingIndex(Mathf.Abs(middleX - j));
                go.Delay = delay;

                go.GetComponentInChildren<Image>().DOFade(0, StartingDurTime + delay).From();

                // 获取启动动画的延迟时间
                if (delay > _startDelayTime)
                {
                    _startDelayTime = delay;
                }
            }
        }

        // 调整启动动画的时间
        StartingDurTime += _startDelayTime;
    }

    public override void Starting() {
        for (int i = 0; i < AgentManager.Instance.Agents.Count; i++) {
            FlockAgent agent = AgentManager.Instance.Agents[i];
            Vector2 agent_vector2 = agent.GenVector2;
            Vector2 ori_vector2 = agent.OriVector2;

            // 获取总运行时间
            float run_time = StartingDurTime - _timeBetweenStartAndDisplay;

            // 当前已运行的时间;
            float time = Time.time - StartTime;
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

	//
	public void CutEffectUpdateCallback(FlockAgent go){
		if (go.name == "Agent(1,1)") {
			Debug.Log (go.GetComponent<RectTransform> ().anchoredPosition);
		}
//		go.updatePosition ();
	}

}
