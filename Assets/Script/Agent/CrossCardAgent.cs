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

    [SerializeField] List<CrossCardScrollCell> _cardScrollCells;

    #endregion

    #region Component Parameter
    [SerializeField, Header("十字卡片 - 标题")] Text title;
    [SerializeField] RectTransform scrollView;

    [SerializeField] CrossCardScrollView crossCardScrollView;


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

        //// 判断几个类型
        _hasCatalog = enterpriseDetail.catalog.Count > 0;
        _hasProduct = enterpriseDetail.products.Count > 0;
        _hasActivity = enterpriseDetail.activities.Count > 0;
        _hasVideo = enterpriseDetail.videos.Count > 0;

        int index = 0;
        _cardScrollCells = new List<CrossCardScrollCell>();
        List<CrossCardCellData> cellDatas = new List<CrossCardCellData>();



        CrossCardCellData item2 = new CrossCardCellData(index, "公司名片");
        cellDatas.Add(item2);
        //_cardScrollCells.Add(CreateCardScrollCell(scrollView, item2));

        if (_hasProduct) {
            CrossCardCellData item = new CrossCardCellData(index++, "产品");
            cellDatas.Add(item);
            //_cardScrollCells.Add(CreateCardScrollCell(scrollView, item));
        }
        if (_hasActivity)
        {
            CrossCardCellData item = new CrossCardCellData(index++, "活动");
            cellDatas.Add(item);
            //_cardScrollCells.Add(CreateCardScrollCell(scrollView, item));
        }

        if (_hasVideo) {
            CrossCardCellData item = new CrossCardCellData(index++, "视频");
            cellDatas.Add(item);
            //_cardScrollCells.Add(CreateCardScrollCell(scrollView, item));
        }

        if (_hasCatalog)
        {
            CrossCardCellData item = new CrossCardCellData(index++, "CATALOG");
            cellDatas.Add(item);
            //_cardScrollCells.Add(CreateCardScrollCell(scrollView, item));
        }

        //  设置动画
        //for (int i = 0; i < _cardScrollCells.Count; i++) {
        //    CardScrollCell cardScrollCell = _cardScrollCells[i];
        //    cardScrollCell.UpdatePosition(CalculatePostion(cardScrollCell.Index));
        //}

        // TODO Updatedata
        crossCardScrollView.UpdateData(cellDatas);



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

    //private CrossCardScrollCell CreateCardScrollCell(RectTransform scrollPanel, CrossCardCellData item) {
    //    //  创建 Agent
    //    CrossCardScrollCell cardScrollCell = Instantiate(
    //                                cardScrollCellPrefab,
    //                                scrollPanel
    //                                ) as CrossCardScrollCell;
    //    cardScrollCell.name = "ScrollCell" + item.Index;
    //    //cardScrollCell.Initialize(item);

    //    //cardScrollCell.UpdatePosition(CalculatePostion(item.Index));
    //    return cardScrollCell;
    //}


    private float CalculatePostion(int index) {
        float postion = 0.9f;

        if (index == 0)
        {
            postion = 0;
        }
        else if (index == 1) {
            postion = 0.25f;
        }
        else if (index == 2)
        {
            postion = 0.5f;
        }
        else if (index == 3)
        {
            postion = 0.75f;
        }
        else if (index == 4)
        {
            postion = 1f;
        }


        return postion;
    }




}


