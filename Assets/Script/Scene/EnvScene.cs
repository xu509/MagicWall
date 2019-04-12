using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// 企业场景
public class EnvScene : IScene
{
    //
    //  Parameter
    //
    private FlockAgent itemPrefab;
    DaoService daoService;

    //
    //  Init
    //
	public override void DoInit()
    {        
        // 初始化展示时间
        string durtimeValue = DaoService.GetInstance().GetConfigByKey(AppConfig.KEY_THEME_ID).Value;
        float durtime = 60f; // 默认10秒
        if (float.TryParse(durtimeValue, out durtime)) {
            Durtime = durtime;
        }

        Durtime = 30;

        // 初始化销毁的时间
        DestoryDurTime = 2f; //默认

        // 初始化过场效果
        TheCutEffect = CutEffectFactory.Instance.GetByScenes(SceneType.env);

        // 启动过场效果
        TheCutEffect.init();

        Debug.Log("AGENTS: " + AgentManager.Instance.Agents.Count);

        //      //  设置动画时间
        //      Durtime = 30;
        //      DeleteDurTime = 2f;

        //      //  设置预制体
        //      itemPrefab = magicWall.agentPrefab;
        //theCutEffect = cutEffect;
        //theMagicWallManager = magicWall;

        //      //  设置agent类型
        //      theMagicWallManager.TheItemType = AgentType.env;

        //      //  初始化过场效果
        //      cutEffect.init(magicWall);
    }

    public override void DoStarting(){
		TheCutEffect.run();
	}

    public override void DoDisplaying()
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

        //Debug.Log("Update Env Scene Success !");
    }

    public override void DoDestorying()
    {
        // 销毁的动画
    }

}
