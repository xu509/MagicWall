
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
    }

    public FlockData GetFlockData(DataType type)
    {
        throw new System.NotImplementedException();
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
        MySqlCommand mysqlcommand = new MySqlCommand("select * from config;", TheDataSource.mySqlConnection);
        MySqlDataReader reader = mysqlcommand.ExecuteReader();
        try
        {
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    //reader.getstring(0)/getint(0).....
                    Debug.Log("读取config成功");
                }
            }
        }
        catch (Exception)
        {
            Console.WriteLine("failed to select");

        }
        finally
        {
            reader.Close();
        }

        return sceneConfigs;
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
