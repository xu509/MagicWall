using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnterpriseDetail : Generator<EnterpriseDetail>
{

    Enterprise _enterprise; //  企业信息
    List<Product> _products; //  产品信息
    List<Activity> _activities; // 活动信息
    List<Video> _videos;    //  视频
    List<Catalog> _catalogs; // catalog 视频

    public Enterprise Enterprise { set { _enterprise = value; } get { return _enterprise; } }

    public List<Product> products { set { _products = value; } get { return _products; } }

    public List<Activity> activities { set { _activities = value; } get { return _activities; } }

    public List<Video> videos { set { _videos = value; } get { return _videos; } }

    public List<Catalog> catalog { set { _catalogs = value; } get { return _catalogs; } }


    public EnterpriseDetail Generator()
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
        enterpriseDetail._products = _products;


        Catalog catalog = new Catalog();
        List<Catalog> _catalogs = new List<Catalog>();
        _catalogs.Add(catalog.Generator());
        _catalogs.Add(catalog.Generator());
        _catalogs.Add(catalog.Generator());
        _catalogs.Add(catalog.Generator());
        enterpriseDetail._catalogs = _catalogs;

        Activity activity = new Activity();
        List<Activity> _activities = new List<Activity>();
        _activities.Add(activity.Generator());
        _activities.Add(activity.Generator());
        _activities.Add(activity.Generator());
        _activities.Add(activity.Generator());
        enterpriseDetail._activities = _activities;

        Video video = new Video();
        List<Video> _videos = new List<Video>();
        _videos.Add(video.Generator());
        //_videos.Add(video.Generator());
        enterpriseDetail._videos = _videos;

        return enterpriseDetail;
    }
}
