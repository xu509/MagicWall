using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


// 企业场景
public class EnvScene : IScene
{
    private FlockAgent itemPrefab;
	private CutEffect theCutEffect;
	private MagicWallManager theMagicWallManager;


	public override void DoInit(MagicWallManager magicWall,CutEffect cutEffect)
    {
        // 设置动画时间
        StartTime = 6f;
        Durtime = 30;

        // 设置预制体
        itemPrefab = magicWall.agentPrefab;
		theCutEffect = cutEffect;
		theMagicWallManager = magicWall;

        //初始化过场效果
		cutEffect.init(itemPrefab, magicWall,StartTime);

    }

	public override void DoStarting(){
		theCutEffect.run();
	}



    public override void DoUpdate(MagicWallManager magicWallManager)
    {

        // 面板向左移动
        float x = magicWallManager.mainPanel.anchoredPosition.x - Time.deltaTime * magicWallManager.MoveFactor_Panel;
        Vector2 to = new Vector2(x, magicWallManager.mainPanel.anchoredPosition.y);
        magicWallManager.mainPanel.DOAnchorPos(to, Time.deltaTime);
        // 面板向左移动结束 

        //Debug.Log("Update Env Scene Success !");

    }

    public override void DoDestory(MagicWallManager magicWall)
    {
        magicWall.DoDestory();
    }

}
