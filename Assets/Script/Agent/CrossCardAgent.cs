using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

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
    VideoAgent _videoAgent;

    #endregion

    #region Component Parameter
    [SerializeField, Header("十字卡片 - 标题")] Text _title;
    [SerializeField, Header("十字卡片 - 描述")] Text _description;

    [SerializeField] VideoAgent videoAgentPrefab;

    [SerializeField] RectTransform normalContainer;
    [SerializeField] RectTransform videoContainer;
    [SerializeField] CrossCardScrollViewController crossCardScrollViewController;
    [SerializeField] CrossCardScrollBar crossCardScrollBar;

    #endregion


    //
    //  初始化数据
    //
    public void InitData()
    {
        Timer myTimer = new Timer("生成卡片");
        myTimer.Record();

        DaoService daoService = DaoService.Instance; 
        EnterpriseDetail enterpriseDetail = daoService.GetEnterprisesDetail();

        // 设置ID
        id = enterpriseDetail.Enterprise.Ent_id;

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
        item2.EnvId = id;
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
            item.EnvId = id;
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
            item.EnvId = id;
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
            item.EnvId = id;
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
            item.EnvId = id;
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
        crossCardScrollViewController.SetUpCardAgent(this);

        crossCardScrollBar.UpdateData(_cellDatas);
        crossCardScrollBar.SelectCell(0);
        crossCardScrollBar.OnSelectionChanged(OnBarSelectionChanged);

        // 处理businesscard
        _hasListBtn = DaoService.Instance.GetEnvCards(id).Count > 0;
        InitComponents(_hasListBtn);

        myTimer.Record();
        myTimer.Display();

    }

    void OnSelectionChanged(int index) {
        GameObject obj = GameObject.Find("CrossCardScrollCell" + index);
        obj.GetComponent<RectTransform>().SetAsLastSibling();

        crossCardScrollBar.SelectCell(index);

        // 更新描述
        UpdateDescription(crossCardScrollViewController.GetCurrentCardDescription());

        DoUpdate();

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


    public void UpdateDescription(string description) {
        _description.text = description;
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
    }

    public void DoVideo(string address,string description)
    {
        //显示 video 的框框
        OpenVideoContainer(address,description);

        // 隐藏平时的框框
        HideNormalContainer();

    }

    public void DoCloseVideoContainer() {
        OpenNormalContainer();

        CloseVideoContainer();
    }


    //
    //  关闭视频
    //
    private void CloseVideoContainer() {
        videoContainer.gameObject.SetActive(false);
        _videoAgent?.DoDestory();
        Destroy(_videoAgent?.gameObject);

    }

    // 显示普通的窗口
    private void OpenNormalContainer()
    {
        normalContainer.gameObject.SetActive(true);
    }

    // 隐藏普通的窗口
    private void HideNormalContainer() {
        normalContainer.gameObject.SetActive(false);
    }

    // 显示 Video 的窗口
    private void OpenVideoContainer(string address,string description) {
        videoContainer.gameObject.SetActive(true);
        _videoAgent = Instantiate(videoAgentPrefab, videoContainer);
        _videoAgent.SetData(address,description, this);
        _videoAgent.Init();
    }


}


