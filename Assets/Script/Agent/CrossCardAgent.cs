using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System;


public class CrossCardAgent : CardAgent
{

    #region Data Parameter

    int _likes;
    public int Likes { set { _likes = value; } get { return _likes; } }

    bool _isNormalModel = true;


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
    [SerializeField] RectTransform _buttomTool;


    private Vector2 Description_Origin_Position = Vector2.zero + new Vector2(0, 20);
    private Vector2 Description_Go_Position = Vector2.zero;
    private Vector2 ButtomTool_Origin_Position = new Vector2(0, 100);
    private Vector2 ButtomTool_Go_Position = new Vector2(0, 50);

    #endregion


    //
    //  初始化数据
    //
    public void InitCrossCardAgent()
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        InitAgency();

        DaoService daoService = DaoService.Instance; 
        EnterpriseDetail enterpriseDetail = daoService.GetEnterprisesDetail();


        //  设置标题
        _title.text = enterpriseDetail.Enterprise.Name;

        ////  设置描述
        //UpdateDescription(enterpriseDetail.Enterprise.Description);

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
        item2.EnvId = DataId;
        item2.Likes = Likes;
        item2.Category = CrossCardCategoryEnum.INDEX;
        item2.Index = index;
        item2.Title = "公司名片";
        item2.Description = enterpriseDetail.Enterprise.Description;
        index++; 
        _cellDatas.Add(item2);
        //_cardScrollCells.Add(CreateCardScrollCell(scrollView, item2));

        if (_hasProduct) {
            CrossCardCellData item = new CrossCardCellData();
            item.Category = CrossCardCategoryEnum.PRODUCT;
            item.crossCardAgent = this;
            item.EnvId = DataId;
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
            item.EnvId = DataId;
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
            item.EnvId = DataId;
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
            item.EnvId = DataId;
            item.Likes = Likes;
            item.Index = index;
            item.Title = "CATALOG";
            _cellDatas.Add(item);
            index++;
            //_cardScrollCells.Add(CreateCardScrollCell(scrollView, item));
        }


        sw.Start();

        // Updatedata
        //crossCardScrollViewController.SelectCell(0);
        crossCardScrollViewController.SetUpCardAgent(this);
        crossCardScrollViewController.UpdateData(_cellDatas);
        crossCardScrollViewController.OnSelectionChanged(OnSelectionChanged);

        sw.Stop();
        Debug.Log("InitCrossCardAgent Time : " + sw.ElapsedMilliseconds / 1000f);

        //crossCardScrollViewController.SetScrollOperatedAction(OnScrollOperated);

        crossCardScrollBar.UpdateData(_cellDatas);
        //crossCardScrollBar.SelectCell(0);
        crossCardScrollBar.UpdateComponents();
        crossCardScrollBar.OnSelectionChanged(OnBarSelectionChanged);
        crossCardScrollBar.SetScrollOperatedAction(OnScrollOperated);

        // 处理businesscard
        _hasListBtn = DaoService.Instance.GetEnvCards(DataId).Count > 0;
        InitComponents(false);

        // 设置完成回调
        SetOnCreatedCompleted(OnCreatedCompleted);
    }

    void OnSelectionChanged(int index)
    {
        crossCardScrollViewController.UpdateComponents();

        crossCardScrollBar.SelectCell(index);
        
        // 更新描述
        UpdateDescription(crossCardScrollViewController.GetCurrentCardDescription());

        DoUpdate();

    }

    void OnBarSelectionChanged(int index)
    {
        if (index != crossCardScrollViewController.CurrentIndex)
        {
            crossCardScrollViewController.SelectCell(index);

            CrossCardScrollBarCell cell = crossCardScrollBar.GetCell(index) as CrossCardScrollBarCell;
            crossCardScrollBar.UpdateComponents();
        }
        else {
            crossCardScrollBar.UpdateComponents();


        }

    }


    void Awake() {
    }

    //
    //  更新
    //
    void Update() {
        UpdateAgency();
    }

    void OnEnable() {

}


    public void UpdateDescription(string description) {

        // 从透明到不透明，向下移动
        _description.text = description;

        _description.GetComponent<RectTransform>().anchoredPosition = Description_Origin_Position;
        _description.DOFade(0, 0f);

        _description.GetComponent<RectTransform>().DOAnchorPos(Description_Go_Position, 0.5f);
        _description.DOFade(1, 0.5f);

        UpdateToolComponent();

    }

    //
    //  上一张
    //
    public void DoUp() {
        int index = crossCardScrollViewController.CurrentIndex;

        // 获取上一个 index
        int up_index = index - 1;
        if (index == 0) {
            up_index = crossCardScrollViewController.Pool.Count - 1;
        }

        crossCardScrollViewController.SelectCell(up_index);

        DoUpdate();

    }

    //
    //  下一张
    //
    public void DoDown() {
        int index = crossCardScrollViewController.CurrentIndex;

        // 获取上一个 index
        int down_index = index + 1;
        if (down_index == crossCardScrollViewController.Pool.Count)
        {
            down_index = 0;
        }

        crossCardScrollViewController.SelectCell(down_index);

        DoUpdate();
    }


    private void OnScrollOperated() {
        DoUpdate();
    }


    private void UpdateToolComponent()
    {
        _buttomTool.GetComponent<RectTransform>().anchoredPosition = ButtomTool_Origin_Position;
        _buttomTool.GetComponent<CanvasGroup>().DOFade(0, Time.deltaTime);

        _buttomTool.GetComponent<RectTransform>().DOAnchorPos(ButtomTool_Go_Position, 0.5f);
        _buttomTool.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
    }




    private void OnCreatedCompleted()
    {
        //string description = crossCardScrollViewController.GetCurrentCardDescription();

        ////  更新描述
        //UpdateDescription(description);

        ////  更新操作栏
        //UpdateToolComponent();

    }
}


