using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


// 企业场景
public class EnvScene : IScene
{
    private FlockAgent itemPrefab;

    public override void DoInit(MagicWallManager magicWall,CutEffect cutEffect)
    {
        // 设置动画时间
        StartTime = 10f;
        Durtime = 30;

        // 设置预制体
        itemPrefab = magicWall.agentPrefab;


        //从左往右，从下往上
        Debug.Log("Load Env Scene Success !");

        cutEffect.run(itemPrefab, magicWall);

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
