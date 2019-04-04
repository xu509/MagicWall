using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


// Environment 
public class StartScene : IScene
{
    Transform logo;

    private void Awake()
    {
        logo = GameObject.Find("Background/logo").GetComponent<Transform>();
    }


    public override void DoDestory(MagicWallManager magicWall)
    {
        // 消失logo

        logo.gameObject.GetComponent<RawImage>().DOFade(0, 2);
        //magicWall.DoDestory();
    }

    public override void DoInit(MagicWallManager magicWall, CutEffect cutEffect)
    {
        //for (int i = 0; i < magicWall.row * magicWall.column; i++)
        //{
        //    magicWall.CreateNewAgent(i);
        //}

        Durtime = 10;
        // 显示 logo

        logo.gameObject.GetComponent<RawImage>().DOFade(1, 2);

        Debug.Log("Load Start Scene Success !");
    }

    public override void DoUpdate(MagicWallManager magicWallManager)
    {
        //Debug.Log("Update Start Scene Success !");

    }

}
