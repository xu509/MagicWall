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
    List<string> _catalog; // catalog 视频

    public Enterprise enterprise { set { _enterprise = value; } get { return _enterprise; } }

    public List<Product> products { set { _products = value; } get { return _products; } }

    public List<Activity> activities { set { _activities = value; } get { return _activities; } }

    public List<Video> videos { set { _videos = value; } get { return _videos; } }

    public List<string> catalog { set { _catalog = value; } get { return _catalog; } }


    public EnterpriseDetail Generator()
    {
        EnterpriseDetail enterpriseDetail = new EnterpriseDetail();

        Enterprise enterprise = new Enterprise();
        enterprise = enterprise.Generator();






        return enterpriseDetail;
    }
}
