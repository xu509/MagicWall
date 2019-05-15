
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaoService : Singleton<DaoService>
{
    private List<Enterprise> _enterprises;
    private List<Activity> _activities;
    private List<Product> _products;


    void Awake()
    {

    }

    //
    //  Construct
    //
    protected DaoService() { }

    //
    //  加载信息
    //
    public void LoadInformation()
    {

    }

    //
    //  获取首页企业
    //
    public List<Enterprise> GetEnterprises()
    {
        if (_enterprises == null)
        {
            Enterprise env = new Enterprise();

            //  从数据库中获取数据
            _enterprises = new List<Enterprise>();
            for (int i = 0; i < 100; i++)
            {
                _enterprises.Add(env.Generator());
            }

            return _enterprises;
        }
        else
        {
            return _enterprises;
        }

    }

    //
    //  获取首页企业
    //
    public Enterprise GetEnterprise()
    {
        List<Enterprise> enterprises = GetEnterprises();
        int index = Random.Range(0, enterprises.Count);
        return enterprises[index];
    }

    public List<Texture> GetEnvCards(int id) {

        // TODO 
        return GetEnterprise().EnvCards;
    }



    //
    //  获取 catalog
    //
    public Catalog GetCatalog()
    {
        return new Catalog().Generator();
    }



    //
    //  获取企业的详细信息
    //
    public EnterpriseDetail GetEnterprisesDetail()
    {
        //  基础数据（企业名，企业简介，企业点赞数）
        //  企业名片数据
        //  catelog数据
        //  活动数据
        //  产品数据
        //  视频数据
        EnterpriseDetail enterpriseDetail = new EnterpriseDetail().Generator();

        return enterpriseDetail;
    }

    //
    //  获取首页活动
    //
    public List<Activity> GetActivities()
    {
        if (_activities == null)
        {
            Activity activity = new Activity();

            //  从数据库中获取数据
            _activities = new List<Activity>();
            for (int i = 0; i < 100; i++)
            {
                _activities.Add(activity.Generator());
            }

            return _activities;
        }
        else
        {
            return _activities;
        }
    }

    //
    //  获取首页活动
    //
    public Activity GetActivity()
    {
        List<Activity> enterprises = GetActivities();
        int index = Random.Range(0, _activities.Count);
        return enterprises[index];
    }


    //
    //  获取首页活动的详细信息
    //
    public Activity GetActivityDetail()
    {
        Activity activity = new Activity();
        return activity.Generator();
    }

    //
    //  获取首页的产品
    //
    public List<Product> GetProducts()
    {
        if (_products == null)
        {
            Product product = new Product();

            //  从数据库中获取数据
            _products = new List<Product>();
            for (int i = 0; i < 100; i++)
            {
                _products.Add(product.Generator());
            }

            return _products;
        }
        else
        {
            return _products;
        }
    }

    public Product GetProduct()
    {
        List<Product> enterprises = GetProducts();
        int index = Random.Range(0, _products.Count);
        return _products[index];
    }

    //
    //  获取首页详细
    //
    public Product GetProductDetail()
    {
        // todo
        return new Product().Generator();
    }

    public Video GetVideoDetail()
    {
        // todo
        return new Video().Generator();
    }

    //
    //  获取config
    //
    public AppConfig GetConfigByKey(string key)
    {
        AppConfig appConfig = new AppConfig();
        appConfig.Value = "20";

        if (key.Equals(AppConfig.KEY_CutEffectDuring_CurveStagger))
        {
            appConfig.Value = "20";
        }
        else if (key.Equals(AppConfig.KEY_CutEffectDuring_LeftRightAdjust))
        {
            appConfig.Value = "20";
        }
        else if (key.Equals(AppConfig.KEY_CutEffectDuring_MidDisperseAdjust))
        {
            appConfig.Value = "20";
        }
        else if (key.Equals(AppConfig.KEY_CutEffectDuring_Stars))
        {
            appConfig.Value = "20";
        }
        else if (key.Equals(AppConfig.KEY_CutEffectDuring_UpDownAdjust))
        {
            appConfig.Value = "20";
        }
        else
        {

        }
        return appConfig;
    }

    //
    //  获取显示配置
    //
    public List<SceneConfig> GetShowConfigs()
    {


        List<SceneConfig> sceneConfigs = new List<SceneConfig>();

        SceneConfig sceneConfig0 = new SceneConfig();
        sceneConfig0.CutEffect = new CurveStaggerCutEffect();
        sceneConfig0.SceneContentType = SceneContentType.env;
        sceneConfigs.Add(sceneConfig0);

        SceneConfig sceneConfig1 = new SceneConfig();
        sceneConfig1.CutEffect = new MidDisperseCutEffect();
        sceneConfig1.SceneContentType = SceneContentType.env;
        sceneConfigs.Add(sceneConfig1);

        SceneConfig sceneConfig2 = new SceneConfig();
        sceneConfig2.CutEffect = new StarsCutEffect();
        sceneConfig2.SceneContentType = SceneContentType.env;
        sceneConfigs.Add(sceneConfig2);

        SceneConfig sceneConfig3 = new SceneConfig();
        sceneConfig3.CutEffect = new LeftRightAdjustCutEffect();
        sceneConfig3.SceneContentType = SceneContentType.env;
        sceneConfigs.Add(sceneConfig3);

        SceneConfig sceneConfig4 = new SceneConfig();
        sceneConfig4.CutEffect = new FrontBackUnfoldCutEffect();
        sceneConfig4.SceneContentType = SceneContentType.env;
        sceneConfigs.Add(sceneConfig4);

        SceneConfig sceneConfig5 = new SceneConfig();
        sceneConfig5.CutEffect = new CurveStaggerCutEffect();
        sceneConfig5.SceneContentType = SceneContentType.activity;
        sceneConfigs.Add(sceneConfig5);

        SceneConfig sceneConfig6 = new SceneConfig();
        sceneConfig6.CutEffect = new MidDisperseCutEffect();
        sceneConfig6.SceneContentType = SceneContentType.activity;
        sceneConfigs.Add(sceneConfig6);

        SceneConfig sceneConfig7 = new SceneConfig();
        sceneConfig7.CutEffect = new StarsCutEffect();
        sceneConfig7.SceneContentType = SceneContentType.activity;
        sceneConfigs.Add(sceneConfig7);

        SceneConfig sceneConfig8 = new SceneConfig();
        sceneConfig8.CutEffect = new LeftRightAdjustCutEffect();
        sceneConfig8.SceneContentType = SceneContentType.activity;
        sceneConfigs.Add(sceneConfig8);

        SceneConfig sceneConfig9 = new SceneConfig();
        sceneConfig9.CutEffect = new FrontBackUnfoldCutEffect();
        sceneConfig9.SceneContentType = SceneContentType.activity;
        sceneConfigs.Add(sceneConfig9);

        SceneConfig sceneConfig10 = new SceneConfig();
        sceneConfig10.CutEffect = new CurveStaggerCutEffect();
        sceneConfig10.SceneContentType = SceneContentType.product;
        sceneConfigs.Add(sceneConfig10);

        SceneConfig sceneConfig11 = new SceneConfig();
        sceneConfig11.CutEffect = new MidDisperseCutEffect();
        sceneConfig11.SceneContentType = SceneContentType.product;
        sceneConfigs.Add(sceneConfig11);

        SceneConfig sceneConfig12 = new SceneConfig();
        sceneConfig12.CutEffect = new StarsCutEffect();
        sceneConfig12.SceneContentType = SceneContentType.product;
        sceneConfigs.Add(sceneConfig12);

        SceneConfig sceneConfig13 = new SceneConfig();
        sceneConfig13.CutEffect = new LeftRightAdjustCutEffect();
        sceneConfig13.SceneContentType = SceneContentType.product;
        sceneConfigs.Add(sceneConfig13);

        SceneConfig sceneConfig14 = new SceneConfig();
        sceneConfig14.CutEffect = new FrontBackUnfoldCutEffect();
        sceneConfig14.SceneContentType = SceneContentType.product;
        sceneConfigs.Add(sceneConfig14);

        return sceneConfigs;
    }

}
