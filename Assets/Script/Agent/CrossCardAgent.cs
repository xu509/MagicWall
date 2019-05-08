using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class CrossCardAgent : CardAgent
{

    #region Data Parameter

    int _id;

    int _likes;
    public int Likes { set { _likes = value; } get { return _likes; } }



    bool _hasCard = true; // 企业名片
    bool _hasCatalog; // Catalog
    bool _hasProduct; // 产品
    bool _hasActivity;  //  活动
    bool _hasVideo; //  视频

    List<CrossCardCellData> _cellDatas;

    #endregion

    #region Component Parameter
    [SerializeField, Header("十字卡片 - 标题")] Text _title;
    [SerializeField, Header("十字卡片 - 描述")] Text _description;


    [SerializeField] CrossCardScrollViewController crossCardScrollViewController;
    [SerializeField] CrossCardScrollBar crossCardScrollBar;


    #endregion


    //
    //  初始化数据
    //
    public void InitData()
    {
        DaoService daoService = DaoService.Instance; 
        EnterpriseDetail enterpriseDetail = daoService.GetEnterprisesDetail();

        // 设置ID
        _id = enterpriseDetail.Enterprise.Ent_id;

        //  设置标题
        _title.text = enterpriseDetail.Enterprise.Name;

        //  设置描述
        UpdateDescription(enterpriseDetail.Enterprise.Description);

        // 设置喜欢数
        Likes = enterpriseDetail.Enterprise.likes;

        //// 判断几个类型
        _hasCatalog = enterpriseDetail.catalog.Count > 0;
        _hasProduct = enterpriseDetail.products.Count > 0;
        _hasActivity = enterpriseDetail.activities.Count > 0;
        _hasVideo = enterpriseDetail.videos.Count > 0;

        int index = 0;
        
        _cellDatas = new List<CrossCardCellData>();

        CrossCardCellData item2 = new CrossCardCellData();
        item2.crossCardAgent = this;
        item2.EnvId = _id;
        item2.Likes = Likes;
        item2.Category = CrossCardCategoryEnum.INDEX;
        item2.Index = index;
        item2.Title = "公司名片";
        index++; 
        _cellDatas.Add(item2);
        //_cardScrollCells.Add(CreateCardScrollCell(scrollView, item2));

        if (_hasProduct) {
            CrossCardCellData item = new CrossCardCellData();
            item.Category = CrossCardCategoryEnum.PRODUCT;
            item.crossCardAgent = this;
            item.EnvId = _id;
            item.Likes = Likes;
            item.Index = index;
            item.Title = "产品";
            _cellDatas.Add(item);
            index++;
            //_cardScrollCells.Add(CreateCardScrollCell(scrollView, item));
        }
        if (_hasActivity)
        {
            CrossCardCellData item = new CrossCardCellData();
            item.Category = CrossCardCategoryEnum.ACTIVITY;
            item.crossCardAgent = this;
            item.EnvId = _id;
            item.Likes = Likes;
            item.Index = index;
            item.Title = "活动";
            _cellDatas.Add(item);
            index++;
            //_cardScrollCells.Add(CreateCardScrollCell(scrollView, item));
        }

        if (_hasVideo) {
            CrossCardCellData item = new CrossCardCellData();
            item.Category = CrossCardCategoryEnum.VIDEO;
            item.crossCardAgent = this;
            item.EnvId = _id;
            item.Likes = Likes;
            item.Index = index;
            item.Title = "视频";
            _cellDatas.Add(item);
            index++;
            //_cardScrollCells.Add(CreateCardScrollCell(scrollView, item));
        }

        if (_hasCatalog)
        {
            CrossCardCellData item = new CrossCardCellData();
            item.Category = CrossCardCategoryEnum.CATALOG;
            item.crossCardAgent = this;
            item.EnvId = _id;
            item.Likes = Likes;
            item.Index = index;
            item.Title = "CATALOG";
            _cellDatas.Add(item);
            index++;
            //_cardScrollCells.Add(CreateCardScrollCell(scrollView, item));
        }

        //  设置动画
        //for (int i = 0; i < _cardScrollCells.Count; i++) {
        //    CardScrollCell cardScrollCell = _cardScrollCells[i];
        //    cardScrollCell.UpdatePosition(CalculatePostion(cardScrollCell.Index));
        //}

        // TODO Updatedata
        crossCardScrollViewController.OnSelectionChanged(OnSelectionChanged);
        crossCardScrollViewController.UpdateData(_cellDatas);
        crossCardScrollViewController.SelectCell(0);

        crossCardScrollBar.UpdateData(_cellDatas);
        crossCardScrollBar.SelectCell(0);
        crossCardScrollBar.OnSelectionChanged(OnBarSelectionChanged);


    }

    void OnSelectionChanged(int index) {
        GameObject obj = GameObject.Find("CrossCardScrollCell" + index);
        obj.GetComponent<RectTransform>().SetAsLastSibling();

        crossCardScrollBar.SelectCell(index);
    }

    void OnBarSelectionChanged(int index)
    {
        crossCardScrollViewController.SelectCell(index);

        // TODO 左侧标记移动

        // TODO 左右增加符号 | 


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


    void UpdateDescription(string description) {
        _description.text = description;
    }

}


