using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//
//	向左移动
//
public class GoLeftDisplayBehavior : CutEffectDisplayBehavior
{
	public void Run()
	{
		MagicWallManager manager = MagicWallManager.Instance;

		// 面板向左移动
		float x = manager.mainPanel.anchoredPosition.x - Time.deltaTime * manager.MoveFactor_Panel;
		Vector2 to = new Vector2(x, manager.mainPanel.anchoredPosition.y);
		manager.mainPanel.DOAnchorPos(to, Time.deltaTime);

		// 调整panel的差值
		manager.updateOffsetOfCanvas();

		// 调整所有agent
		manager.UpdateAgents();

	}


}
