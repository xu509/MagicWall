using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System;
using MagicWall;

/// <summary>
///  单个卡片
/// </summary>
public class SingleCardAgent : CardAgent
{

    #region Data Parameter

    int _likes;
    public int Likes { set { _likes = value; } get { return _likes; } }

    bool _isNormalModel = true;


    private int _scrollItemNumber;  // 滚动插件，滑动块个数

    bool _hasCard = true; // 企业名片
    bool _hasCatalog; // Catalog
    bool _hasProduct; // 产品
    bool _hasActivity;  //  活动
    bool _hasVideo; //  视频

    List<CrossCardCellData> _cellDatas;

    #endregion

    #region Component Parameter
    [SerializeField, Header("UI")] Text _title;
    [SerializeField] Text _description;
    [SerializeField] Image _cover;



    [SerializeField] RectTransform _buttomTool;


    private Vector2 Description_Origin_Position = Vector2.zero + new Vector2(0, 20);
    private Vector2 Description_Go_Position = Vector2.zero;
    private Vector2 ButtomTool_Origin_Position = new Vector2(0, 100);
    private Vector2 ButtomTool_Go_Position = new Vector2(0, 50);

    #endregion


    //
    //  初始化数据
    //
    public void InitSingleCardAgent(Enterprise data)
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        InitAgency();
        _questionTypeEnum = QuestionTypeEnum.SingleCard;

        //  设置标题
        _title.text = data.Name;

        // 设置封面
        _cover.sprite = SpriteResource.Instance.GetData(MagicWallManager.FileDir + data.Logo);
        CanvasExtensions.SizeToParent(_cover);

        // 处理businesscard
        InitComponents(null);

        // 设置完成回调
        SetOnCreatedCompleted(OnCreatedCompleted);

        isPrepared = true;

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

        //UpdateToolComponent();

    }



    public override void InitData(OperateCardData operateCardData)
    {
        OperateCardDataSingle operateCardDataSingle = (OperateCardDataSingle)operateCardData;
        Debug.Log("Do In slice: " + operateCardDataSingle.Title);
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


