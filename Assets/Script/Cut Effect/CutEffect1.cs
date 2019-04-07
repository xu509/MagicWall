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
	private float dur_time;

	//
	//	初始化 MagicWallManager
	//
	public override void init(FlockAgent prefab,MagicWallManager magicWallManager,float dur_time) {
		row = magicWallManager.row;
		column = magicWallManager.column;
		the_magicWallManager = magicWallManager;
		this.dur_time = dur_time;

		int h= (int)magicWallManager.mainPanel.rect.height;
		int gap = 10;

		int itemWidth = h / row - gap;
		int itemHeight =  itemWidth;

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

				// ori_x;ori_y
				float ori_x,ori_y;

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

				string name = "Agent" + (x + 1) + "-" + (y + 1);
				Vector2 ori_position = new Vector2 (ori_x, ori_y);
				Vector2 gen_position = new Vector2 (x, y);

				//				FlockAgent go = AgentGenerator.GetInstance ().generator (name, gen_position, ori_position, magicWallManager);
				FlockAgent go = magicWallManager.CreateNewAgent(ori_x, ori_y,x,y,i,j);
				RectTransform the_RectTransform = go.GetComponent<RectTransform>();

				// 调整大小
				the_RectTransform.sizeDelta = new Vector2(itemWidth,itemWidth);


//				Tweener t = the_RectTransform.DOAnchorPos(new Vector2(x, y), dur_time - delayX + delayY).SetEase(Ease.InOutQuad)
//					.OnUpdate (() => CutEffectUpdateCallback (go));
////				//				t.OnUpdate (() => CutEffectUpdateCallback (go));
////
//				the_RectTransform.DOScale(0.1f, (dur_time - delayX + delayY)).From();
				//go.GetComponent<RawImage>().DOFade(0, (dur_time - delayX + delayY)).From();
			}
		}

		// 初始化完成后更新时间
		the_time = Time.time;

	}



    public override void run() {

		for (int i = 0; i < the_magicWallManager.Agents.Count; i++) {
			FlockAgent agent = the_magicWallManager.Agents [i];
			Vector2 agent_vector2 = agent.GetComponent<RectTransform> ().anchoredPosition;
			Vector2 ori_vector2 = agent.oriVector2;

			float t = (Time.time - the_time) / dur_time;
			Vector2 to = Vector2.Lerp(agent_vector2,ori_vector2,t);
			agent.NextVector2 = to;
			agent.updatePosition ();
		}

    }


	//
	public void CutEffectUpdateCallback(FlockAgent go){
		if (go.name == "Agent(1,1)") {
			Debug.Log (go.GetComponent<RectTransform> ().anchoredPosition);
		}


//		go.updatePosition ();
	}

}
