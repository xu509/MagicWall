using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   普通的数据仓库
/// </summary>
public class CommonSubDaoService : IDaoSubService
{
    public List<Activity> GetActivities(int themeId)
    {
        TheDataSource theDataSource = TheDataSource.Instance;

        string sql = " SELECT a.* " 
                    + " FROM activity a"
                    + " LEFT JOIN theme_act_ref atr ON a.act_id = atr.act_id"
                    + " WHERE a.status = 1 AND atr.theme_id = "
                    + themeId
                    + " ORDER BY atr.ordering DESC";
        var results = theDataSource.SelectList(sql);
        var activities = new List<Activity>();

        for (int i = 0; i < results.Count; i++)
        {
            Activity activity = new Activity();
            activity.Id = Convert.ToInt16(results[i]["act_id"]);
            activity.Ent_id = Convert.ToInt16(results[i]["com_id"]);
            activity.Name = results[i]["name"].ToString();
            activity.Image = results[i]["image"].ToString();
            //activity.ActivityDetails = GetActivityDetails(activity.Id);
            activities.Add(activity);
        }

        return activities;
    }

    public List<Enterprise> GetEnterprises(int themeId)
    {
        TheDataSource theDataSource = TheDataSource.Instance;

        string sql = "SELECT c.* "
            + " FROM company c"
            + " LEFT JOIN theme_com_ref ctr ON c.com_id = ctr.com_id"
            +" WHERE c.status = 1 AND ctr.theme_id = "
            + themeId + " "
            + " ORDER BY ctr.ordering DESC";
        var results = theDataSource.SelectList(sql);
        var enterprises = new List<Enterprise>();

        for (int i = 0; i < results.Count; i++)
        {
            Enterprise enterprise = new Enterprise();
            enterprise.Ent_id = Convert.ToInt16(results[i]["com_id"]);
            enterprise.Name = results[i]["name"].ToString();
            enterprise.IsCustom = false;
            enterprise.Logo = results[i]["logo"].ToString();
            enterprise.Description = results[i]["description"].ToString();
            //enterprise.Business_card = row[17].ToString();
            //enterprise.EnvCards = GetEnvCards(enterprise.Ent_id);
            enterprises.Add(enterprise);
        }

        return enterprises;
    }

    public List<Product> GetProducts(int themeId)
    {
        TheDataSource theDataSource = TheDataSource.Instance;

        string sql = "SELECT p.* "
            + " FROM product p"
            + " LEFT JOIN theme_prod_ref ptr ON p.prod_id = ptr.prod_id"
            + " WHERE p.status = 1 AND ptr.theme_id = "
            + themeId + " "
            + " ORDER BY ptr.ordering DESC";
        var results = theDataSource.SelectList(sql);
        var products = new List<Product>();

        for (int i = 0; i < results.Count; i++)
        {
            Product product = new Product();
            product.Pro_id = Convert.ToInt16(results[i]["prod_id"]);
            product.Ent_id = Convert.ToInt16(results[i]["com_id"]);
            product.Name = results[i]["name"].ToString();
            product.Image = results[i]["image"].ToString();
            //product.ProductDetails = GetProductDetails(product.Pro_id);
            products.Add(product);
        }

        return products;
    }
}
