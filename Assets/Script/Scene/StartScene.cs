using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//
//   启动的场景 
//  - 需在此处完成数据模块的加载
//
public class StartScene : IScene
{
    Transform logo;

    private void Awake()
    {
        logo = GameObject.Find("Background/logo").GetComponent<Transform>();
    }


    public override void DoDestorying()
    {
        // 消失logo

        logo.gameObject.GetComponent<RawImage>().DOFade(0, 2);
        //magicWall.DoDestory();
    }

	public override void DoStarting ()
	{
		
	}


	public override void DoInit()
    {
        Debug.Log("Load Start Scene now !");

        // 加载 Config.xml
        Debug.Log("加载 Config.xml 成功");

        // 根据 tid 获取信息
        Debug.Log("根据 tid 获取信息列表成功 ");

        // 加载关联资源
        Debug.Log("加载关联的资源");

        // 完成数据加载, 提供字典
        Debug.Log("完成数据加载,提供字典");

    }

    public override void DoDisplaying()
    {
        throw new System.NotImplementedException();
    }


}
