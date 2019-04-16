using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//
//	向左移动
//
public class GoDownDisplayBehavior : CutEffectDisplayBehavior
{
    private SceneContentType _sceneContentType;

    //
    //  初始化 （参数：内容类型，row）
    //
    public void Init(SceneContentType sceneContentType)
    {
        _sceneContentType = sceneContentType;
    }

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

        UpdateAgents();

    }

    private void UpdateAgents() {
        // 查看是否需要生成新的组件、是否有组件需要作废

        // 组件需要了解是哪个
    }



}
