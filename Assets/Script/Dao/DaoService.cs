
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
            foreach (DataRow row in table.Rows)
            {
                Activity activity = new Activity();
                activity.Id = Convert.ToInt16(row[0]);
                activity.Ent_id = Convert.ToInt16(row[1]);
                activity.Name = row[7].ToString();
                activity.Image = row[8].ToString();
                activity.ActivityDetails = GetActivityDetails(activity.Id);
                activities.Add(activity);
            }
        }
        return activities;
    }

    public Activity GetActivityDetail(int act_id)
    {
        //Debug.Log("act_id:" + act_id);
        Activity activity = new Activity();
        DataSet dataSet = TheDataSource.GetDataSet("select * from activity where act_id=" + act_id);
        if (dataSet != null)
        {
            DataTable table = dataSet.Tables[0];
            activity.Id = Convert.ToInt16(table.Rows[0][0]);
            activity.Ent_id = Convert.ToInt16(table.Rows[0][1]);
            activity.Name = table.Rows[0][7].ToString();
            activity.Image = table.Rows[0][8].ToString();
            activity.ActivityDetails = GetActivityDetails(act_id);
        }
        return activity;
    }

    public List<ActivityDetail> GetActivityDetails(int act_id)
    {
        List<ActivityDetail> activityDetails = new List<ActivityDetail>();
        DataSet dataSet = TheDataSource.GetDataSet("select * from activity where act_id='" + act_id + "'");
        if (dataSet != null)
        {
            DataTable table = dataSet.Tables[0];

            string material = table.Rows[0][10].ToString();
            List<MWMaterial> mWMaterials = (List<MWMaterial>)DaoUtil.ConvertMaterialJson(material);

            for (int i = 0; i < mWMaterials.Count; i++)
            {
                MWMaterial data = mWMaterials[i];
                //Debug.Log(data[i]["cuteffect_id"]);
                ActivityDetail activityDetail = new ActivityDetail();
                if (data.type == "video")
                {
                    activityDetail.Type = 1;
                    activityDetail.Image = data.cover;
                    activityDetail.VideoUrl = data.path;
                }
                else if (data.type == "image")
                {
                    activityDetail.Type = 0;
                    activityDetail.Image = data.path;
                }
                activityDetail.Description = data.description;
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
                enterprise.Ent_id = Convert.ToInt16(row[0]);
                enterprise.Name = row[7].ToString();
                bool isCustom = Convert.ToInt16(row[16]) == 0 ? true : false;
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
                enterprise.Ent_id = Convert.ToInt16(table.Rows[0][0]);
                enterprise.Name = table.Rows[0][7].ToString();
                bool isCustom = Convert.ToInt16(table.Rows[0][16]) == 0 ? true : false;
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

        Enterprise enterprise = new Enterprise();
        enterpriseDetail.Enterprise = enterprise.Generator();

        Product product = new Product();
        List<Product> _products = new List<Product>();
        _products.Add(product.Generator());
        _products.Add(product.Generator());
        _products.Add(product.Generator());
        _products.Add(product.Generator());
        enterpriseDetail.products = _products;

        Catalog catalog = new Catalog();
        List<Catalog> _catalogs = new List<Catalog>();
        _catalogs.Add(catalog.Generator());
        _catalogs.Add(catalog.Generator());
        _catalogs.Add(catalog.Generator());
        _catalogs.Add(catalog.Generator());
        enterpriseDetail.catalog = _catalogs;

        Activity activity = new Activity();
        List<Activity> _activities = new List<Activity>();
        _activities.Add(activity.Generator());
        _activities.Add(activity.Generator());
        _activities.Add(activity.Generator());
        _activities.Add(activity.Generator());
        enterpriseDetail.activities = _activities;

        Video video = new Video();
        List<Video> _videos = new List<Video>();
        _videos.Add(video.Generator());
        //_videos.Add(video.Generator());
        enterpriseDetail.videos = _videos;

        return enterpriseDetail;
    }

    public List<string> GetEnvCards(int id)
    {
        List<string> envCards = new List<string>();
        DataSet dataSet = TheDataSource.GetDataSet("select b.image_card from activity a,company b where a.com_id=b.com_id and a.act_id=" + id);
        DataTable table = dataSet.Tables[0];
        if (table.Rows.Count > 0)
        {
            //查到结果
            Debug.Log(table.Rows[0][0].ToString());
            JsonData data = JsonMapper.ToObject(table.Rows[0][0].ToString());
            for (int i = 0; i < data.Count; i++)
            {
                envCards.Add(data[i].ToString());   
            }
        }
        foreach (string s in envCards)
        {
            Debug.Log(s);
        }
        return envCards;
    }

    public FlockData GetFlockData(DataType type)
    {
        if (type == DataType.env)
        {
            return GetEnterprise();
        }
        else if (type == DataType.product)
        {
            return GetProduct();
        }
        else if (type == DataType.activity)
        {
            return GetActivity();
        }
        return null;
    }

        //
    //  获取首页企业
    //
    public Enterprise GetEnterprise()
    {
        List<Enterprise> enterprises = GetEnterprises();
        int index = UnityEngine.Random.Range(0, enterprises.Count-1);
        return enterprises[index];
    }

    public Product GetProduct()
    {
        List<Product> products = GetProducts();
        int index = UnityEngine.Random.Range(0, products.Count-1);
        return products[index];
    }

    public Activity GetActivity()
    {        

        List<Activity> activities = GetActivities();
        int index = UnityEngine.Random.Range(0, activities.Count-1);

        //Debug.Log("Get activity : " + activities[index].Id);
        return activities[index];
    }

    public int GetLikes(int id, CrossCardCategoryEnum category)
    {
        return 1;
    }

    public int GetLikesByActivityDetail(int id)
    {
        return 1;
    }

    public int GetLikesByProductDetail(int id)
    {
        return 1;
    }

    public Product GetProductDetail(int pro_id)
    {
        Product product = new Product();
        DataSet dataSet = TheDataSource.GetDataSet("select * from product where prod_id=" + pro_id);
        if (dataSet != null)
        {
            DataTable table = dataSet.Tables[0];
            product.Pro_id = Convert.ToInt16(table.Rows[0][0]);
            product.Ent_id = Convert.ToInt16(table.Rows[0][1]);
            product.Name = table.Rows[0][7].ToString();
            product.Image = table.Rows[0][8].ToString();
            product.ProductDetails = GetProductDetails(pro_id);
        }
        return product;
    }

    public List<ProductDetail> GetProductDetails(int pro_id)
    {
        List<ProductDetail> productDetails = new List<ProductDetail>();
        DataSet dataSet = TheDataSource.GetDataSet("select * from product where prod_id=" + pro_id);
        if (dataSet != null)
        {
            DataTable table = dataSet.Tables[0];

            string material = table.Rows[0][13].ToString();
            List<MWMaterial> mWMaterials = (List<MWMaterial>)DaoUtil.ConvertMaterialJson(material);

            for (int i = 0; i < mWMaterials.Count; i++)
            {
                MWMaterial data = mWMaterials[i];
                //Debug.Log(data[i]["cuteffect_id"]);
                ProductDetail productDetail = new ProductDetail();

                if (data.type == "video")
                {
                    productDetail.Type = 1;
                    productDetail.Image = data.cover;
                    productDetail.VideoUrl = data.path;
                }
                else if (data.type == "image")
                {
                    productDetail.Type = 0;
                    productDetail.Image = data.path;
                }
                productDetail.Description = data.description;
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
                product.Pro_id = Convert.ToInt16(row[0]);
                product.Ent_id = Convert.ToInt16(row[1]);
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
            SceneTypeEnum.FrontBackUnfold,
            SceneTypeEnum.LeftRightAdjust,
            SceneTypeEnum.MidDisperse,
            SceneTypeEnum.Stars,
            SceneTypeEnum.UpDownAdjustCutEffect,
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
                    Debug.Log("sceneType:" + sceneConfig.sceneType + "---dataType:" + sceneConfig.dataType + "---durtime:" + sceneConfig.durtime);
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
