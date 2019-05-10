
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaoService : Singleton<DaoService>
{
    private List<Enterprise> _enterprises;

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
        int index = Random.Range(0, enterprises.Count - 1);
        return enterprises[index];
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
        // todo

        return null;
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
        // todo
        return null;
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

        SceneConfig sceneConfig1 = new SceneConfig();
        sceneConfig1.CutEffect = new CurveStaggerCutEffect();
        //sceneConfig1.SceneContentType = SceneContentType.env;
        sceneConfig1.SceneContentType = SceneContentType.activity;
   
        sceneConfigs.Add(sceneConfig1);

        //SceneConfig sceneConfig4 = new SceneConfig();
        //sceneConfig4.CutEffect = new StarsCutEffect();
        //sceneConfig4.SceneContentType = SceneContentType.product;
        //sceneConfigs.Add(sceneConfig4);

        //SceneConfig sceneConfig2 = new SceneConfig();
        //sceneConfig2.CutEffect = new LeftRightAdjustCutEffect();
        //sceneConfig2.SceneContentType = SceneContentType.product;
        //sceneConfigs.Add(sceneConfig2);

        //SceneConfig sceneConfig3 = new SceneConfig();
        //sceneConfig3.CutEffect = new MidDisperseCutEffect();
        //sceneConfig3.SceneContentType = SceneContentType.activity;
        ////sceneConfig3.SceneContentType = SceneContentType.env;
        //sceneConfigs.Add(sceneConfig3);



        //SceneConfig sceneConfig5 = new SceneConfig();
        //sceneConfig5.CutEffect = new UpDownAdjustCutEffect();
        //sceneConfig5.SceneContentType = SceneContentType.activity;
        ////sceneConfig5.SceneContentType = SceneContentType.env;
        //sceneConfigs.Add(sceneConfig5);

        //SceneConfig sceneConfig0 = new SceneConfig();
        //sceneConfig0.CutEffect = new FrontBackUnfoldCutEffect();
        //sceneConfig0.SceneContentType = SceneContentType.env;
        //sceneConfigs.Add(sceneConfig0);

        return sceneConfigs;
    }

}
