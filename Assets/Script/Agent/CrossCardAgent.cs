using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class CrossCardAgent : CardAgent
{

    #region Data Parameter

    bool _hasCard = true; // 企业名片
    bool _hasCatalog; // Catalog
    bool _hasProduct; // 产品
    bool _hasActivity;  //  活动
    bool _hasVideo; //  视频

    [SerializeField] List<CrossCardIndexItem> _CrossCardIndexItems;

    #endregion

    #region Component Parameter
    [SerializeField, Header("十字卡片 - 标题")] Text title;

    private int _index = 0; //  当前索引




    #endregion


    //
    //  初始化数据
    //
    public void InitData()
    {
        DaoService daoService = DaoService.Instance; 
        Debug.Log("Init Data : " + OriginAgent.DataId);
        EnterpriseDetail enterpriseDetail = daoService.GetEnterprisesDetail();

        // 设置标题
        title.text = enterpriseDetail.Enterprise.Name;

        // 判断几个类型
        _hasCatalog = enterpriseDetail.catalog.Count > 0;
        _hasProduct = enterpriseDetail.products.Count > 0;
        _hasActivity = enterpriseDetail.activities.Count > 0;
        _hasVideo = enterpriseDetail.videos.Count > 0;


        int index = 0;
        _CrossCardIndexItems = new List<CrossCardIndexItem>();
        _CrossCardIndexItems.Add(new CrossCardIndexItem(index, "公司名片"));

        if (_hasProduct)
            _CrossCardIndexItems.Add(new CrossCardIndexItem(index++, "产品"));

        if (_hasActivity)
            _CrossCardIndexItems.Add(new CrossCardIndexItem(index++, "活动"));

        if (_hasVideo)
            _CrossCardIndexItems.Add(new CrossCardIndexItem(index++, "视频"));

        if (_hasCatalog)
            _CrossCardIndexItems.Add(new CrossCardIndexItem(index++, "CATALOG"));

        //  绘制ui

    }


    void Awake() {
        AwakeAgency();

    }

    //
    //  更新
    //
    void Update() {
        UpdateAgency();


    }


    class CrossCardIndexItem {
        int _index;
        string _title;

        public string Title { set { _title = value; } get { return _title; } }
        public int Index { set { _index = value; } get { return _index; } }

        public CrossCardIndexItem(int index,string title) {
            _index = index;
            _title = title;
        }

    }

}


