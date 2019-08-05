using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.U2D;
using System;

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

    MagicSceneEnum _magicSceneEnumStatus;



    private static bool LOG = true;


    float RunTime {
        get {
            return Time.time - _StartTime;
        }
    }


    private IDaoService _daoService;
    private MagicWallManager _manager;

    Action _onRunCompleted;
    Action _onRunEndCompleted;

    //
    //  Construct
    //
    public StartScene() {

    }

    public void Init(SceneConfig sceneConfig, MagicWallManager manager)
    {
        _manager = manager;
        _daoService = manager.daoService;

        Reset();
    }


    private void Reset() {
        _StartTime = Time.time;
        _hasInit = true;

        _doLoadResourse = false;
        _resourseIsChecked = false;

        _doShowLogo = false;
        _doShowLogoComplete = false;

        _doHideLogo = false;
        _doHideLogoComplete = false;

        _isCompleted = false;

        _magicSceneEnumStatus = MagicSceneEnum.Running;

    }



    public DataType GetDataType()
    {
        return DataType.none;
    }

    public bool Run()
	{
        //DoDebug("Start Scene Is Start!");


        if (!_hasInit) {
            Reset();
        }

        // 读取配置表
        LoadConfig();


        // LOGO 淡入
        if (!_doShowLogo) {
            _doShowLogo = true;
            _manager.BgLogo.gameObject.SetActive(true);
            _manager.BgLogo.GetComponent<Image>().sprite 
                = Resources.Load<SpriteAtlas>("SpriteAtlas").GetSprite("background-logo");
            _manager.BgLogo.GetComponent<Image>()
                .DOFade(1, 1f)
                .OnComplete(() => {
                    _doShowLogoComplete = true;
                });
        }

        if (_doShowLogoComplete) {
            // 进行logo隐藏
            if (_resourseIsChecked && (RunTime > _DuringTime)) {

                OnRunCompleted();


                //_manager.BgLogo.GetComponent<Image>()
                //    .DOFade(0, 1f)
                //    .OnComplete(() => {
                //        _doHideLogoComplete = true;
                //        _manager.BgLogo.gameObject.SetActive(false);
                //    });
            }
        }

        //if (_doHideLogoComplete) {
        //    _hasInit = false;

        //    //DoDebug("Start Scene Is End!");

        //    return false;
        //}

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

        if (_manager.IsMockData)
        {
            DoLoadMock();
        }
        else {
            DoLoad();
        }


        // 加载其他资源
        //  - 手写板用的texture
        Texture2D writePanelWordPanel = new Texture2D(_manager.writePanelConfig.writePanelWordRectWidth, _manager.writePanelConfig.writePanelWordRectHeight, TextureFormat.ARGB32, false);
        TextureResource.Instance.Add(TextureResource.Write_Pad_Texture, writePanelWordPanel);

        Texture2D writePanelTotalPanel = new Texture2D(_manager.writePanelConfig.writePanelTotalRectWidth, _manager.writePanelConfig.writePanelTotalRectHeight, TextureFormat.ARGB32, false);
        TextureResource.Instance.Add(TextureResource.Write_Pad_Texture_Big, writePanelTotalPanel);

        _resourseIsChecked = true;

    }

    private void LoadConfig() {
        if (_doLoadConfig)
        {
            return;
        }

        // 设置配置表

        MWConfig _config =  _daoService.GetConfig();
        _manager.globalData.SetMWConfig(_config);

        _doLoadConfig = true;

        //if (DaoService.Instance.IsCustom() && (!_manager.managerConfig.IsCustom)) {
        //    SceneManager.LoadScene("CustomScene");
        //}
        _configIsLoaded = true;
    }


    // Do Load

    private void DoLoadMock() {


        List<string> addresses = new List<string>();

        Enterprise enterprise = new Enterprise();
        addresses.AddRange(enterprise.GetAssetAddressList());

        Activity activity = new Activity();
        addresses.AddRange(activity.GetAssetAddressList());

        Product product = new Product();
        addresses.AddRange(product.GetAssetAddressList());

        Video video = new Video();
        addresses.AddRange(video.GetAssetAddressList());

        ActivityDetail activityDetail = new ActivityDetail();
        addresses.AddRange(activityDetail.GetAssetAddressList());

        string[] imgs = { "catalog-1-1.png", "catalog-1-2.png", "catalog-1-3.png", "catalog-1-4.png" };
        for (int i = 0; i < imgs.Length; i++)
        {
            addresses.Add("env\\" + imgs[i]);
        }

        ProductDetail productDetail = new ProductDetail();
        addresses.AddRange(productDetail.GetAssetAddressList());


        for (int i = 0; i < addresses.Count; i++)
        {
            SpriteResource.Instance.GetData(MagicWallManager.FileDir + addresses[i]);
        }



        // 加载定制的资源
        if (_manager.managerConfig.IsCustom)
        {
            // TODO 

            CustomImageType[] types = {CustomImageType.LEFT1,
                CustomImageType.LEFT2,
                CustomImageType.RIGHT };

            foreach (CustomImageType customImageType in types)
            {
                List<string> images = _daoService.GetCustomImage(customImageType);

                foreach (string image in images)
                {
                    string address = MagicWallManager.FileDir + image;
                    TextureResource.Instance.GetTexture(address);

                }
            }
        }
    }

    /// <summary>
    ///     加载数据
    /// </summary>
    private void DoLoad()
    {
        _daoService.InitData();

        List<string> addresses = new List<string>();

        List<Enterprise> enterprises = _daoService.GetEnterprises();
        foreach (Enterprise env in enterprises) {
            addresses.Add(env.Logo);

            // 产品详细
            var productDetails = _daoService.GetProductsByEnvId(env.Ent_id);
            for (int i = 0; i < productDetails.Count; i++) {
                addresses.Add(productDetails[i].Image);
            }

            // 活动详细
            var ads = _daoService.GetActivitiesByEnvId(env.Ent_id);
            for (int i = 0; i < ads.Count; i++)
            {
                addresses.Add(ads[i].Image);
            }

            // catalog 详细
            var cs = _daoService.GetCatalogs(env.Ent_id);
            for (int i = 0; i < cs.Count; i++)
            {
                addresses.Add(cs[i].Img);
            }

            // 视频封面
            var vs = _daoService.GetVideosByEnvId(env.Ent_id);
            for (int i = 0; i < vs.Count; i++)
            {
                addresses.Add(vs[i].Cover);
            }

            // 企业卡片
            var es = _daoService.GetEnvCards(env.Ent_id);
            for (int i = 0; i < es.Count; i++)
            {
                addresses.Add(es[i]);
            }

        }

        var activities = _daoService.GetActivities();  
        for (int i = 0; i < activities.Count; i++)
        {
            addresses.Add(activities[i].Image);
        }

        var products = _daoService.GetProducts();
        for (int i = 0; i < products.Count; i++)
        {
            addresses.Add(products[i].Image);
        }


        for (int i = 0; i < addresses.Count; i++)
        {
            SpriteResource.Instance.GetData(MagicWallManager.FileDir + addresses[i]);
        }



        // 加载定制的资源
        if (_manager.managerConfig.IsCustom)
        {
            // TODO 

            CustomImageType[] types = {CustomImageType.LEFT1,
                CustomImageType.LEFT2,
                CustomImageType.RIGHT };

            foreach (CustomImageType customImageType in types)
            {
                List<string> images = _daoService.GetCustomImage(customImageType);

                foreach (string image in images)
                {
                    string address = MagicWallManager.FileDir + image;
                    TextureResource.Instance.GetTexture(address);

                }
            }
        }
    }


    public void OnRunCompleted()
    {
        _manager.BgLogo.GetComponent<Image>()
            .DOFade(0, 1f)
            .OnComplete(() => {
                //_doHideLogoComplete = true;
               // RunEnd();
                _manager.BgLogo.gameObject.SetActive(false);
            });

        _magicSceneEnumStatus = MagicSceneEnum.RunningComplete;

        _onRunCompleted.Invoke();
    }

    public void SetOnRunCompleted(Action onRunCompleted)
    {
        _onRunCompleted = onRunCompleted;
    }

    public void RunEnd()
    {
        _magicSceneEnumStatus = MagicSceneEnum.RunningEnd;


        OnRunEndCompleted();

    }

    public void OnRunEndCompleted()
    {
        _hasInit = false;

        _magicSceneEnumStatus = MagicSceneEnum.RunningEndComplete;


        _onRunEndCompleted.Invoke();

    }

    public void SetOnRunEndCompleted(Action onRunEndCompleted)
    {
        _onRunEndCompleted = onRunEndCompleted;
    }


    private void DoDebug(string message) {
        if (LOG) {
            Debug.Log(message);
        }
    }

    public MagicSceneEnum GetSceneStatus()
    {
        return _magicSceneEnumStatus;
    }
}
