using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//
//	向左移动
//
public class GoDownDisplayBehavior : CutEffectDisplayBehavior
{
	public void Run()
	{
		MagicWallManager manager = MagicWallManager.Instance;

		// 面板向下移动
		float y = manager.mainPanel.anchoredPosition.y - Time.deltaTime * manager.MoveFactor_Panel;
		Vector2 to = new Vector2(manager.mainPanel.anchoredPosition.x, y);
		manager.mainPanel.DOAnchorPos(to, Time.deltaTime);

		// 调整panel的差值
		manager.updateOffsetOfCanvas();

		// 调整所有agent
		manager.UpdateAgents();

	}


}
