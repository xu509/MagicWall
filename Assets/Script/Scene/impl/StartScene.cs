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

    private bool _resourseIsChecked = false;

    private DaoService _daoService;
    private MagicWallManager _manager;

    //
    //  Construct
    //
    public StartScene() {
        _resourseIsChecked = false;
        _daoService = DaoService.Instance;
        _manager = MagicWallManager.Instance;
    }


    public SceneContentType GetContentType()
    {
        return SceneContentType.none;
    }

    public bool Run()
	{
        // 加载主要的图片资源
        Debug.Log("Load Start Scene now !");
        List<Enterprise> enterprises = _daoService.GetEnterprises();
        foreach (Enterprise env in enterprises) {
            string logo = env.Logo;
            // 加载图片资源
            env.TextureLogo = AppUtils.LoadPNG(MagicWallManager.URL_ASSET_LOGO + logo);
        }

        _resourseIsChecked = true;

        if (_resourseIsChecked)
        {
            return false;
        }
        else {
            return true;
        }
    }

	private void Awake()
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


}
