using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

//
//   启动的场景 
//  - 需在此处完成数据模块的加载
//
public class StartScene : IScene
{
    Transform logo;

    private bool _doLoadResourse = false;
    private bool _resourseIsChecked = false;

    private bool _doLoadConfig = false;
    private bool _configIsLoaded = false;

    private bool _doShowLogo = false;
    private bool _doShowLogoComplete = false;

    private bool _doHideLogo = false;
    private bool _doHideLogoComplete = false;


    private bool _hasInit = false;

    private bool _isCompleted = false;


    private float _DuringTime = 3f;
    private float _StartTime = 0f;

    float RunTime {
        get {
            return Time.time - _StartTime;
        }
    }


    private DaoService _daoService;
    private MagicWallManager _manager;

    //
    //  Construct
    //
    public StartScene() {

    }

    public void Init(MagicWallManager manager)
    {
        _resourseIsChecked = false;
        _manager = manager;
        _daoService = manager.daoService;

    }

    private void Init() {
        _StartTime = Time.time;
        _hasInit = true;

        _doLoadResourse = false;
        _resourseIsChecked = false;

        _doShowLogo = false;
        _doShowLogoComplete = false;

        _doHideLogo = false;
        _doHideLogoComplete = false;

        _isCompleted = false;
    }



    public SceneContentType GetContentType()
    {
        return SceneContentType.none;
    }

    public bool Run()
	{
        if (!_hasInit) {
            Init();
        }

        // 读取配置表
        LoadConfig();


        // LOGO 淡入
        if (!_doShowLogo) {
            _doShowLogo = true;
            _manager.BgLogo.GetComponent<RawImage>()
                .DOFade(1, 2f)
                .OnComplete(() => {
                    _doShowLogoComplete = true;
                });
        }

        if (_doShowLogoComplete) {
            // 进行logo隐藏
            if (_resourseIsChecked && (RunTime > _DuringTime)) {
                _manager.BgLogo.GetComponent<RawImage>()
                    .DOFade(0, 2f)
                    .OnComplete(() => {
                        _doHideLogoComplete = true;
                    });
            }
        }

        if (_doHideLogoComplete) {
            _hasInit = false;
            return false;
        }

        // 加载资源
        LoadResource();

        return true;
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


    private void LoadResource() {
        if (_doLoadResourse) {
            return;
        }

        _doLoadResourse = true;

        List<Enterprise> enterprises = _daoService.GetEnterprises();
        foreach (Enterprise env in enterprises)
        {
            string logo = env.Logo;
            string address = MagicWallManager.FileDir + "logo\\" + logo;
            TextureResource.Instance.GetTexture(address);


            // 预加载企业卡片
            for (int i = 0; i < env.EnvCards.Count; i++) {
                TextureResource.Instance.GetTexture(env.EnvCards[i]);
            }

            // 预加载 CATALOGS
            List<Catalog> catalogs =  _daoService.GetCatalogs(env.Ent_id);
            for (int i = 0; i < catalogs.Count; i++)
            {
                string category_address = MagicWallManager.FileDir + "env\\" + catalogs[i].Img;
                TextureResource.Instance.GetTexture(category_address);
            }
        }

        List<Activity> activities = _daoService.GetActivities();
        foreach (Activity activity in activities)
        {
            string img = activity.Image;
            string address = MagicWallManager.FileDir + "activity\\" + img;
            activity.TextureImage = TextureResource.Instance.GetTexture(address);
        }

        List<Product> products = _daoService.GetProducts();
        foreach (Product product in products)
        {
            string img = product.Image;
            string address = MagicWallManager.FileDir + "product\\" + img;
            product.TextureImage = TextureResource.Instance.GetTexture(address);
        }

        // 加载VIDEO
        List<Video> videos = _daoService.GetVideos();
        foreach (Video video in videos)
        {
            string cover = video.Cover;
            string address = MagicWallManager.FileDir + "video\\" + cover;
            TextureResource.Instance.GetTexture(address);

            // TODO 处理视频的加载
        }

        // 加载定制的资源
        if (_manager.managerConfig.IsCustom) {
            // TODO 
            Debug.Log("加载定制资源");

            DaoService.CustomImageType[] types = { DaoService.CustomImageType.LEFT1,
                DaoService.CustomImageType.LEFT2,
                DaoService.CustomImageType.RIGHT };

            foreach (DaoService.CustomImageType customImageType in types) {
                List<string> images = _daoService.GetCustomImage(customImageType);

                foreach (string image in images)
                {
                    string address = MagicWallManager.FileDir + "custom\\" + image;
                    TextureResource.Instance.GetTexture(address);

                }
            }
        }

        // 加载其他资源
        //  - 手写板用的texture
        Texture2D png = new Texture2D(200, 200, TextureFormat.ARGB32, false);
        TextureResource.Instance.Add(TextureResource.Write_Pad_Texture, png);

        Texture2D png2 = new Texture2D(300, 300, TextureFormat.ARGB32, false);
        TextureResource.Instance.Add(TextureResource.Write_Pad_Texture_Big, png2);


        _resourseIsChecked = true;

    }

    private void LoadConfig() {
        if (_doLoadConfig)
        {
            return;
        }

        _doLoadConfig = true;

        //if (DaoService.Instance.IsCustom() && (!_manager.managerConfig.IsCustom)) {
        //    SceneManager.LoadScene("CustomScene");
        //}



        _configIsLoaded = true;
    }

    
}
