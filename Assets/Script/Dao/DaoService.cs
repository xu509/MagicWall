using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaoService : Singleton<DaoService>
{
  
    //
    //  Construct
    //
    protected DaoService() { }

    //
    //  加载信息
    //
    public void LoadInformation() {

    }

    //
    //  获取首页企业
    //
    public List<Enterprise> GetEnterprises()
    {
        // todo
        Enterprise enterprise = new Enterprise();
        



        return null;
    }

    //
    //  获取企业的详细信息
    //
    public Enterprise GetEnterprisesDetail()
    {
        // todo

        return null;
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
        // todo
        return null;
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
        return null;
    }

    //
    //  获取config
    //
    public AppConfig GetConfigByKey(string key) {
        AppConfig appConfig = new AppConfig();
        appConfig.Value = "20";

        if (key.Equals(AppConfig.KEY_CutEffectDuring_CurveStagger))
        {
            appConfig.Value = "10";
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
    public List<SceneConfig> GetShowConfigs() {
        List<SceneConfig> sceneConfigs = new List<SceneConfig>();

        SceneConfig sceneConfig3 = new SceneConfig();
        sceneConfig3.CutEffect = new MidDisperseCutEffect();
        //sceneConfig3.SceneContentType = SceneContentType.activity;
        sceneConfig3.SceneContentType = SceneContentType.env;
        sceneConfigs.Add(sceneConfig3);

        SceneConfig sceneConfig1 = new SceneConfig();
        sceneConfig1.CutEffect = new CurveStaggerCutEffect();
        sceneConfig1.SceneContentType = SceneContentType.env;
        sceneConfigs.Add(sceneConfig1);

        SceneConfig sceneConfig2 = new SceneConfig();
        sceneConfig2.CutEffect = new LeftRightAdjustCutEffect();
        sceneConfig2.SceneContentType = SceneContentType.product;
        sceneConfigs.Add(sceneConfig2);

        SceneConfig sceneConfig4 = new SceneConfig();
        sceneConfig4.CutEffect = new StarsCutEffect();
        sceneConfig4.SceneContentType = SceneContentType.product;
        sceneConfigs.Add(sceneConfig4);

        SceneConfig sceneConfig5 = new SceneConfig();
        sceneConfig5.CutEffect = new UpDownAdjustCutEffect();
        //sceneConfig5.SceneContentType = SceneContentType.activity;
        sceneConfig5.SceneContentType = SceneContentType.env;
        sceneConfigs.Add(sceneConfig5);

        return sceneConfigs;
    }

}
