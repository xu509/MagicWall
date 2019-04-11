using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaoService
{
    //
    //  single pattern
    // 
    private static DaoService instance;

    public static DaoService GetInstance()
    {
        if (instance == null)
        {
            instance = new DaoService();
        }
        return instance;
    }

    //
    //  Constructor
    //
    public DaoService() {

    }

    //
    //  获取首页企业
    //
    public List<Enterprise> GetEnterprises()
    {
        // todo

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

        return appConfig;
    }

}
