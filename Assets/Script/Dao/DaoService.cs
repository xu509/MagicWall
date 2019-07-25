
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using LitJson;
using System.Text;
using System.Text.RegularExpressions;

//
//  数据仓库模块
//

public class DaoService : MonoBehaviour, IDaoService
{
    /// <summary>
    /// 初始化
    /// </summary>
    public void Init() {
        TheDataSource theDataSource = TheDataSource.Instance;

    }


    public List<Activity> GetActivities()
    {
        Debug.Log("GetActivities");
        List<Activity> activities = new List<Activity>();

        DataSet dataSet = TheDataSource.GetDataSet("select * from activity");

        if (dataSet != null)
        {
            DataTable table = dataSet.Tables[0];
            foreach(DataRow row in table.Rows)
            {
                Activity activity = new Activity();
                activity.Id = (int)row[0];
                activity.Ent_id = (int)row[1];
                activity.Name = row[7].ToString();
                activity.Image = row[8].ToString();
                List<ActivityDetail> activityDetails = new List<ActivityDetail>();
                JsonData detailsData = JsonMapper.ToObject(row[10].ToString());

                ActivityDetail activityDetail = new ActivityDetail();
                activityDetails.Add(activityDetail.Generator());
                activityDetails.Add(activityDetail.Generator());
                activityDetails.Add(activityDetail.Generator());
                activity.ActivityDetails = activityDetails;
            }
        }
        return activities;
    }

    public Activity GetActivityDetail(int act_id)
    {
        throw new System.NotImplementedException();
    }

    public List<ActivityDetail> GetActivityDetails(int act_id)
    {
        throw new System.NotImplementedException();
    }

    public List<Catalog> GetCatalogs(int id)
    {
        throw new System.NotImplementedException();
    }

    public AppConfig GetConfigByKey(string key)
    {
        AppConfig appConfig = new AppConfig();

        //DataSet dataSet = TheDataSource.SelectWhere("config", new string[] { "key" }, new string[] { "show_type" }, new string[] { "=" }, new string[] { key });
        DataSet dataSet = TheDataSource.GetDataSet("select value from config c where c.key='"+ key +"'");

        if (dataSet != null)
        {
            DataTable table = dataSet.Tables[0];
            if (table.Rows.Count > 0)
            {
                //查到结果
                appConfig.Value = table.Rows[0][0].ToString();
                //Debug.Log(appConfig.Value);
            }
        }
        return appConfig;
    }

    public List<string> GetCustomImage(CustomImageType type)
    {
        throw new System.NotImplementedException();
    }

    public List<Enterprise> GetEnterprises()
    {
        throw new System.NotImplementedException();
    }

    public Enterprise GetEnterprisesById(int id)
    {
        throw new System.NotImplementedException();
    }

    public EnterpriseDetail GetEnterprisesDetail()
    {
        throw new System.NotImplementedException();
    }

    public List<string> GetEnvCards(int id)
    {
        return new Enterprise().EnvCards;
    }

    public FlockData GetFlockData(DataType type)
    {
        if (type == DataType.env)
        {
            return new Enterprise();
        }
        else if (type == DataType.product)
        {
            return new Product();
        }
        else if (type == DataType.activity)
        {
            return new Activity();
        }
        return null;
    }

    public int GetLikes(int id, CrossCardCategoryEnum category)
    {
        throw new System.NotImplementedException();
    }

    public int GetLikesByActivityDetail(int id)
    {
        throw new System.NotImplementedException();
    }

    public int GetLikesByProductDetail(int id)
    {
        throw new System.NotImplementedException();
    }

    public Product GetProductDetail(int pro_id)
    {
        throw new System.NotImplementedException();
    }

    public List<ProductDetail> GetProductDetails(int pro_id)
    {
        throw new System.NotImplementedException();
    }

    public List<Product> GetProducts()
    {
        throw new System.NotImplementedException();
    }

    public List<SceneConfig> GetShowConfigs()
    {

        List<SceneConfig> sceneConfigs = new List<SceneConfig>();
        DataSet dataSet = TheDataSource.GetDataSet("select value from config c where c.key='show_config'");
        SceneTypeEnum[] sceneTypes = new SceneTypeEnum[]
        {
            SceneTypeEnum.CurveStagger,
            SceneTypeEnum.LeftRightAdjust,
            SceneTypeEnum.MidDisperse,
            SceneTypeEnum.Stars,
            SceneTypeEnum.UpDownAdjustCutEffect,
            SceneTypeEnum.FrontBackUnfold          
        };
        DataType[] dataTypes = new DataType[] {
            DataType.env,
            DataType.activity,
            DataType.product,
        };
        if (dataSet != null)
        {
            DataTable table = dataSet.Tables[0];
            if (table.Rows.Count > 0)
            {
                //查到结果
                //Debug.Log(table.Rows[0][0].ToString());

                JsonData data = JsonMapper.ToObject(table.Rows[0][0].ToString());
                for (int i = 0; i < data.Count; i++)
                {
                    //Debug.Log(data[i]["cuteffect_id"]);
                    SceneConfig sceneConfig = new SceneConfig();
                    sceneConfig.sceneType = sceneTypes[int.Parse(data[i]["cuteffect_id"].ToString())-1];
                    sceneConfig.dataType = dataTypes[int.Parse(data[i]["contcom_type"].ToString()) - 1];

                    // 设置场景时间
                    sceneConfig.durtime = GetSceneDurTime(sceneTypes[i]);
                    sceneConfigs.Add(sceneConfig);
                }
            }
        }

        return sceneConfigs;
    }

    /// <summary>
    ///     获取场景持续时间
    /// </summary>
    /// <param name="sceneTypeEnum"></param>
    /// <returns></returns>
    public float GetSceneDurTime(SceneTypeEnum sceneTypeEnum)
    {
        string key = "";

        if (sceneTypeEnum == SceneTypeEnum.CurveStagger)
        {
            key = AppConfig.KEY_CutEffectDuring_CurveStagger;
        }
        else if (sceneTypeEnum == SceneTypeEnum.FrontBackUnfold)
        {
            key = AppConfig.KEY_CutEffectDuring_FrontBackUnfold;
        }
        else if (sceneTypeEnum == SceneTypeEnum.LeftRightAdjust)
        {
            key = AppConfig.KEY_CutEffectDuring_LeftRightAdjust;
        }
        else if (sceneTypeEnum == SceneTypeEnum.MidDisperse)
        {
            key = AppConfig.KEY_CutEffectDuring_MidDisperseAdjust;
        }
        else if (sceneTypeEnum == SceneTypeEnum.Stars)
        {
            key = AppConfig.KEY_CutEffectDuring_Stars;
        }
        else if (sceneTypeEnum == SceneTypeEnum.UpDownAdjustCutEffect)
        {
            key = AppConfig.KEY_CutEffectDuring_UpDownAdjust;
        }

        string durTime = GetConfigByKey(key).Value;
        float d = AppUtils.ConvertToFloat(durTime);

        return d;
    }


    public Video GetVideoDetail()
    {
        throw new System.NotImplementedException();
    }

    public bool IsCustom()
    {
        throw new System.NotImplementedException();
    }

    public List<SearchBean> Search(string keys)
    {
        throw new System.NotImplementedException();
    }
}
