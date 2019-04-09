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

    DaoService daoService;

    //
    //  Construct
    //
    public EnvScene() {
        daoService = new DaoService();
    }
    

	public override void DoInit(MagicWallManager magicWall,CutEffect cutEffect)
    {
        //  设置动画时间
        Durtime = 30;
        DeleteDurTime = 2f;

        //  设置预制体
        itemPrefab = magicWall.agentPrefab;
		theCutEffect = cutEffect;
		theMagicWallManager = magicWall;

        //  设置agent类型
        theMagicWallManager.TheItemType = ItemType.env;

        //  初始化过场效果
        cutEffect.init(magicWall);
    }

    public override void DoStarting(){
		theCutEffect.run();
	}

    public override void DoUpdate()
    {
        // 面板向左移动
        float x = theMagicWallManager.mainPanel.anchoredPosition.x - Time.deltaTime * theMagicWallManager.MoveFactor_Panel;
        Vector2 to = new Vector2(x, theMagicWallManager.mainPanel.anchoredPosition.y);
        theMagicWallManager.mainPanel.DOAnchorPos(to, Time.deltaTime);

        // 调整panel
        theMagicWallManager.updateOffsetOfCanvas();

        // 调整所有agent
        theMagicWallManager.updateAgents();

        //Debug.Log("Update Env Scene Success !");
    }

    public override void DoDestory()
    {
        //初始化panel
        theMagicWallManager.updateOffsetOfCanvas();
        theMagicWallManager.DoDestory();
    }

    public override void OnStartComplete()
    {
        theMagicWallManager.updateAgents();
    }
}
