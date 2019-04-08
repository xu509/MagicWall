using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 过场效果 1 
public class CutEffect1 : CutEffect
{

    private MagicWallManager the_magicWallManager;
    private int row;
    private int column;
    private float the_time;
    private float dur_time; // 持续时间

    private bool isStartingCompleted; //是否已完成

    //
    //	初始化 MagicWallManager
    //
    public override void init(FlockAgent prefab, MagicWallManager magicWallManager) {
        the_magicWallManager = magicWallManager;
        DurTime = 6f;
        this.dur_time = DurTime;

        row = magicWallManager.row;
        column = magicWallManager.column;

        int h = (int)magicWallManager.mainPanel.rect.height;
        int gap = 10;

        int itemWidth = h / row - gap;
        int itemHeight = itemWidth;
        the_magicWallManager.ItemWidth = itemWidth;


        //从左往右，从下往上
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                float x = j * (itemWidth + gap) + itemWidth / 2;
                float y = i * (itemHeight + gap) + itemHeight / 2;

                int middleY = row / 2;
                int middleX = column / 2;

                float delayX = j * 0.06f;
                float delayY;
                
                // 定义源位置
                float ori_x, ori_y;

                if (i < middleY)
                {
                    delayY = System.Math.Abs(middleY - i) * 0.3f;
                    ori_x = (column + middleY - i - 1) * (itemWidth + gap) + itemWidth / 2;
                    ori_y = (column - j - middleY) * (itemHeight + gap) + itemHeight / 2;
                    //the_RectTransform.DOLocalMove(new Vector3((column + middleY - i - 1) * (itemWidth + gap) + itemWidth / 2, (column - j - middleY) * (itemHeight + gap) + itemHeight / 2, 0), dur_time - delayX + delayY).SetEase(Ease.InOutQuad).From();
                }
                else
                {
                    delayY = (System.Math.Abs(middleY - i) + 1) * 0.3f;
                    ori_x = (column + i - middleY) * (itemWidth + gap) + itemWidth / 2;
                    ori_y = -(column - j - middleY) * (itemHeight + gap) + itemHeight / 2;
                    //the_RectTransform.DOLocalMove(new Vector3((column + i - middleY) * (itemWidth + gap) + itemWidth / 2, -(column - j - middleY) * (itemHeight + gap) + itemHeight / 2, 0), dur_time - delayX + delayY).SetEase(Ease.InOutQuad).From();
                }

                //生成 agent
                string name = "Agent" + (x + 1) + "-" + (y + 1);
                Vector2 ori_position = new Vector2(ori_x, ori_y);
                Vector2 gen_position = new Vector2(x, y);
                FlockAgent go = magicWallManager.CreateNewAgent(ori_x, ori_y, x, y, i, j);

                // 装载延迟参数
                go.DelayX = delayX;
                go.DelayY = delayY;

                // 调整大小
                RectTransform the_RectTransform = go.GetComponent<RectTransform>();
                the_RectTransform.sizeDelta = new Vector2(itemWidth, itemWidth);

                // 生成透明度动画
                go.GetComponentInChildren<Image>().DOFade(0, dur_time - delayX + delayY).From();



            }
        }

        // 初始化完成后更新时间
        the_time = Time.time;

    }



    public override void run() {
        for (int i = 0; i < the_magicWallManager.Agents.Count; i++) {
            FlockAgent agent = the_magicWallManager.Agents[i];
            //Vector2 agent_vector2 = agent.GetComponent<RectTransform> ().anchoredPosition;
            Vector2 agent_vector2 = agent.GenVector2;
            Vector2 ori_vector2 = agent.OriVector2;


            float run_time = (dur_time - agent.DelayX + agent.DelayY) - 1f; // 动画运行的总时间

            //Ease.InOutQuad
            float time = Time.time - the_time;  // 当前已运行的时间;

            if (time > run_time) {
                continue;
                //Debug.Log(agent.name);
            }


            // 模拟 DOTWEEN InOutQuad
            if ((time /= run_time * 0.5f) < 1f)
            {
                time = 0.5f * time * time;
            }
            else
            {
                time = -0.5f * ((time -= 1f) * (time - 2f) - 1f);
            }

            Vector2 to = Vector2.Lerp(agent_vector2, ori_vector2, time);

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
