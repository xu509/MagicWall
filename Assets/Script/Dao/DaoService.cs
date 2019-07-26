
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
                activity.ActivityDetails = GetActivityDetails(activity.Id);
            }
        }
        return activities;
    }

    public Activity GetActivityDetail(int act_id)
    {
        Activity activity = new Activity();
        DataSet dataSet = TheDataSource.GetDataSet("select * from activity where act_id=" + act_id);
        if (dataSet != null)
        {
            DataTable table = dataSet.Tables[0];
            activity.Id = (int)table.Rows[0][0];
            activity.Ent_id = (int)table.Rows[0][1];
            activity.Name = table.Rows[0][7].ToString();
            activity.Image = table.Rows[0][8].ToString();
            activity.ActivityDetails = GetActivityDetails(act_id);
        }
        return activity;
    }

    public List<ActivityDetail> GetActivityDetails(int act_id)
    {
        List<ActivityDetail> activityDetails = new List<ActivityDetail>();
        DataSet dataSet = TheDataSource.GetDataSet("select * from activity where act_id=" + act_id);
        if (dataSet != null)
        {
            DataTable table = dataSet.Tables[0];
            JsonData detailsData = JsonMapper.ToObject(table.Rows[0][10].ToString());
            for (int i = 0; i < detailsData.Count; i++)
            {
                //Debug.Log(data[i]["cuteffect_id"]);
                ActivityDetail activityDetail = new ActivityDetail();
                int type = detailsData[i]["type"].ToString() == "video" ? 1 : 0;
                activityDetail.Type = type;
                if (type == 0)
                {
                    activityDetail.Image = detailsData[i]["path"].ToString();
                }
                else if (type == 1)
                {
                    activityDetail.VideoUrl = detailsData[i]["path"].ToString();
                }
                activityDetail.Description = detailsData[i]["description"].ToString();
                activityDetails.Add(activityDetail);
            }
        }
        return activityDetails;
    }

    public List<Catalog> GetCatalogs(int id)
    {
        List<Catalog> catalogs = new List<Catalog>();
        DataSet dataSet = TheDataSource.GetDataSet("select catalog from company where com_id='" + id + "'");
        DataTable table = dataSet.Tables[0];
        if (table.Rows.Count > 0)
        {
            //查到结果
            //Debug.Log(table.Rows[0][0].ToString());
            JsonData data = JsonMapper.ToObject(table.Rows[0][0].ToString());
            for (int i = 0; i < data.Count; i++)
            {
                Catalog catalog = new Catalog();
                catalog.Img = data[i].ToString();
                //catalog.Description = descriptions[i];
                catalogs.Add(catalog);
            }
        }
        return catalogs;
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
        List<Enterprise> enterprises = new List<Enterprise>();

        DataSet dataSet = TheDataSource.GetDataSet("select * from company");
        if (dataSet != null)
        {
            DataTable table = dataSet.Tables[0];
            foreach (DataRow row in table.Rows)
            {
                Enterprise enterprise = new Enterprise();
                enterprise.Ent_id = (int)row[0];
                enterprise.Name = row[7].ToString();
                bool isCustom = (int)row[16] == 0 ? true : false;
                enterprise.IsCustom = isCustom;
                if (isCustom)
                {
                    enterprise.Logo = row[9].ToString();
                }   else
                {
                    enterprise.Logo = row[8].ToString();
                }
                enterprise.Description = row[12].ToString();
                enterprise.Business_card = row[17].ToString();
                enterprise.EnvCards = GetEnvCards(enterprise.Ent_id);
                enterprises.Add(enterprise);
            }
        }
        return enterprises;
    }

    public Enterprise GetEnterprisesById(int id)
    {
        Enterprise enterprise = new Enterprise();
        DataSet dataSet = TheDataSource.GetDataSet("select * from company where com_id=" + id);
        if (dataSet != null)
        {
            DataTable table = dataSet.Tables[0];
            if (table.Rows.Count > 0)
            {
                enterprise.Ent_id = (int)table.Rows[0][0];
                enterprise.Name = table.Rows[0][7].ToString();
                bool isCustom = (int)table.Rows[0][16] == 0 ? true : false;
                enterprise.IsCustom = isCustom;
                if (isCustom)
                {
                    enterprise.Logo = table.Rows[0][9].ToString();
                }
                else
                {
                    enterprise.Logo = table.Rows[0][8].ToString();
                }
                enterprise.Description = table.Rows[0][12].ToString();
                enterprise.Business_card = table.Rows[0][17].ToString();
                enterprise.EnvCards = GetEnvCards(enterprise.Ent_id);
            }
        }
        return enterprise;
    }

    public EnterpriseDetail GetEnterprisesDetail()
    {
        EnterpriseDetail enterpriseDetail = new EnterpriseDetail();


        return enterpriseDetail;
    }

    public List<string> GetEnvCards(int id)
    {
        List<string> envCards = new List<string>();
        DataSet dataSet = TheDataSource.GetDataSet("select image_card from company where com_id='" + id + "'");
        DataTable table = dataSet.Tables[0];
        if (table.Rows.Count > 0)
        {
            //查到结果
            //Debug.Log(table.Rows[0][0].ToString());
            JsonData data = JsonMapper.ToObject(table.Rows[0][0].ToString());
            for (int i = 0; i < data.Count; i++)
            {
                envCards.Add(data[i].ToString());   
            }
        }
        return envCards;
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
        Product product = new Product();
        DataSet dataSet = TheDataSource.GetDataSet("select * from product where pro_id='" + pro_id + "'");
        if (dataSet != null)
        {
            DataTable table = dataSet.Tables[0];
            product.Pro_id = (int)table.Rows[0][0];
            product.Ent_id = (int)table.Rows[0][1];
            product.Name = table.Rows[0][7].ToString();
            product.Image = table.Rows[0][8].ToString();
            product.ProductDetails = GetProductDetails(pro_id);
        }
        return product;
    }

    public List<ProductDetail> GetProductDetails(int pro_id)
    {
        List<ProductDetail> productDetails = new List<ProductDetail>();
        DataSet dataSet = TheDataSource.GetDataSet("select * from product where pro_id='" + pro_id + "'");
        if (dataSet != null)
        {
            DataTable table = dataSet.Tables[0];
            JsonData detailsData = JsonMapper.ToObject(table.Rows[0][10].ToString());
            for (int i = 0; i < detailsData.Count; i++)
            {
                //Debug.Log(data[i]["cuteffect_id"]);
                ProductDetail productDetail = new ProductDetail();
                int type = detailsData[i]["type"].ToString() == "video" ? 1 : 0;
                productDetail.Type = type;
                if (type == 0)
                {
                    productDetail.Image = detailsData[i]["path"].ToString();
                }
                else if (type == 1)
                {
                    productDetail.VideoUrl = detailsData[i]["path"].ToString();
                }
                productDetail.Description = detailsData[i]["description"].ToString();
                productDetails.Add(productDetail);
            }
        }
        return productDetails;
    }

    public List<Product> GetProducts()
    {
        List<Product> products = new List<Product>();

        DataSet dataSet = TheDataSource.GetDataSet("select * from product");
        if (dataSet != null)
        {
            DataTable table = dataSet.Tables[0];
            foreach (DataRow row in table.Rows)
            {
                Product product = new Product();
                product.Pro_id = (int)row[0];
                product.Ent_id = (int)row[1];
                product.Name = row[7].ToString();
                product.Image = row[8].ToString();
                products.Add(product);
            }
        }
        return products;
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
                    Debug.Log("sceneType:+" + sceneConfig.sceneType + "dataType:" + sceneConfig.dataType);
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
        DataSet dataSet = TheDataSource.GetDataSet("select value from config c where c.key='show_type'");
        if (dataSet != null)
        {
            DataTable table = dataSet.Tables[0];
            if (table.Rows.Count > 0)
            {
                //查到结果
                //Debug.Log(table.Rows[0][0].ToString());
                return (int)table.Rows[0][0] == 1 ? false : true;
            }
        }
        return false;
    }

    public List<SearchBean> Search(string keys)
    {
        throw new System.NotImplementedException();
    }
}
