using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 过场效果 2 中间散开 
public class CutEffect2 : CutEffect
{

    private MagicWallManager the_magicWallManager;
    private int row;
    private int column;
    private float the_time;
    private float dur_time; // 持续时间

    //
    //	初始化 MagicWallManager
    //
    public override void init(MagicWallManager magicWallManager) {
        DurTime = 6f;
        this.dur_time = DurTime;


        row = magicWallManager.row;
        column = magicWallManager.column;
        the_magicWallManager = magicWallManager;

        int h = (int)magicWallManager.mainPanel.rect.height;
        int gap = 10;

        int itemWidth = h / row - gap;
        int itemHeight = itemWidth;
        the_magicWallManager.ItemWidth = itemWidth;


        //从下往上，从左往右
        for (int j = 0; j < column; j++) { 
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

                string name = "Agent" + (x + 1) + "-" + (y + 1);
                Vector2 ori_position = new Vector2(ori_x, ori_y);
                Vector2 gen_position = new Vector2(x, y);

                //				FlockAgent go = AgentGenerator.GetInstance ().generator (name, gen_position, ori_position, magicWallManager);
                FlockAgent go = magicWallManager.CreateNewAgent(ori_x, ori_y, x, y, i, j);
                go.transform.SetSiblingIndex(Mathf.Abs(middleX - j));
                go.Delay = delay;

                go.GetComponentInChildren<Image>().DOFade(0, dur_time + delay).From();

            }
        }

        // 初始化完成后更新时间
        the_time = Time.time;

    }


    public override void run() {
        for (int i = 0; i < the_magicWallManager.Agents.Count; i++) {
            FlockAgent agent = the_magicWallManager.Agents[i];
            Vector2 agent_vector2 = agent.GenVector2;
            Vector2 ori_vector2 = agent.OriVector2;

            // 获取总运行时间
            float run_time = (dur_time + agent.Delay) - 1f;

            // 当前已运行的时间;
            float time = Time.time - the_time;
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

    public override void OnCompleted(){
        the_magicWallManager.updateAgents();
    }


	//
	public void CutEffectUpdateCallback(FlockAgent go){
		if (go.name == "Agent(1,1)") {
			Debug.Log (go.GetComponent<RectTransform> ().anchoredPosition);
		}


//		go.updatePosition ();
	}

}
