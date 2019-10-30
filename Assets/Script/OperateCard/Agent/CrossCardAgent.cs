﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System;

namespace MagicWall {
    public class CrossCardAgent : CardAgent
    {
        [SerializeField,Header("Scroll")] CrossScrollAgent _crossScrollAgent;

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


        OperateCardDataCross _cardData;

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
            else
            {
                crossCardScrollBar.UpdateComponents();


            }

        }


        void Awake()
        {
        }

        //
        //  更新
        //
        void Update()
        {
            UpdateAgency();
        }

        void OnEnable()
        {

        }


        public void UpdateDescription(string description)
        {

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
        public void DoUp()
        {
            int index = crossCardScrollViewController.CurrentIndex;

            // 获取上一个 index
            int up_index = index - 1;

            if (up_index < 0)
            {
                up_index = _scrollItemNumber - 1;
            }

            //Debug.Log("上一个，当前： " + index + " 目标： " + up_index);

            crossCardScrollViewController.SelectCell(up_index);

            DoUpdate();

        }

        //
        //  下一张
        //
        public void DoDown()
        {
            int index = crossCardScrollViewController.CurrentIndex;

            // 获取上一个 index
            int down_index = index + 1;
            if (down_index == _scrollItemNumber)
            {
                down_index = 0;
            }

            crossCardScrollViewController.SelectCell(down_index);

           // Debug.Log("下一个，当前： " + index + " 目标： " + down_index);


            DoUpdate();
        }


        private void UpdateToolComponent()
        {
            _buttomTool.GetComponent<RectTransform>().anchoredPosition = ButtomTool_Origin_Position;
            _buttomTool.GetComponent<CanvasGroup>().DOFade(0, Time.deltaTime);

            _buttomTool.GetComponent<RectTransform>().DOAnchorPos(ButtomTool_Go_Position, 0.5f);
            _buttomTool.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        }



        public override void InitData(OperateCardData operateCardData)
        {
            OperateCardDataCross operateCardDataCross = (OperateCardDataCross)operateCardData;
            _cardData = operateCardDataCross;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            InitAgency();
            _questionTypeEnum = QuestionTypeEnum.CrossCard;

            _crossScrollAgent.Init(_cardData);

            //_scrollItemNumber = 0;
            ////  设置标题
            //_title.text = _cardData.Title;

            ////// 判断几个类型
            //if (_cardData.ScrollDic.ContainsKey(CrossCardNavType.CataLog))
            //{
            //    _hasCatalog = _cardData.ScrollDic[CrossCardNavType.CataLog].Count > 0;
            //}
            //else
            //{
            //    _hasCatalog = false;
            //}

            //if (_cardData.ScrollDic.ContainsKey(CrossCardNavType.Product))
            //{
            //    _hasProduct = _cardData.ScrollDic[CrossCardNavType.Product].Count > 0;
            //}
            //else
            //{
            //    _hasProduct = false;
            //}

            //if (_cardData.ScrollDic.ContainsKey(CrossCardNavType.Activity))
            //{
            //    _hasActivity = _cardData.ScrollDic[CrossCardNavType.Activity].Count > 0;
            //}
            //else
            //{
            //    _hasActivity = false;
            //}

            //if (_cardData.ScrollDic.ContainsKey(CrossCardNavType.Video))
            //{
            //    _hasVideo = _cardData.ScrollDic[CrossCardNavType.Video].Count > 0;
            //}
            //else
            //{
            //    _hasVideo = false;
            //}


            //int index = 0;

            //_cellDatas = new List<CrossCardCellData>();

            //CrossCardCellData item2 = new CrossCardCellData();
            //item2.crossCardAgent = this;
            //item2.EnvId = _dataId;
            //item2.Likes = Likes;
            //item2.Category = CrossCardCategoryEnum.INDEX;
            //item2.Index = index;
            //item2.Title = "公司名片";
            //item2.Description = _cardData.Description;
            //item2.magicWallManager = _manager;

            //ScrollData sd = new ScrollData();
            //sd.Cover = _cardData.Cover;
            //sd.Description = _cardData.Description;

            //var cces = TransferScrollData(sd);
            //List<CrossCardCellData> indexData = new List<CrossCardCellData>();
            //indexData.Add(cces);
            //item2.Datas = indexData;

            //index++;
            //_cellDatas.Add(item2);

            //_scrollItemNumber++;
            ////_cardScrollCells.Add(CreateCardScrollCell(scrollView, item2));

            //if (_hasCatalog)
            //{
            //    Debug.Log("创建CATALOG数据");

            //    CrossCardCellData item = new CrossCardCellData();
            //    item.Category = CrossCardCategoryEnum.CATALOG;
            //    item.crossCardAgent = this;
            //    item.EnvId = _dataId;
            //    item.Likes = Likes;
            //    item.Index = index;
            //    item.Title = "CATALOG";
            //    item.magicWallManager = _manager;

            //    item.Datas = Transfer(_cardData.ScrollDic[CrossCardNavType.CataLog]);

            //    _cellDatas.Add(item);

            //    _scrollItemNumber++;
            //    index++;
            //}


            //if (_hasProduct)
            //{
            //    //Debug.Log("_hasProduct");
            //    CrossCardCellData item = new CrossCardCellData();
            //    item.Category = CrossCardCategoryEnum.PRODUCT;
            //    item.crossCardAgent = this;
            //    item.EnvId = _dataId;
            //    item.Likes = Likes;
            //    item.Index = index;
            //    item.Title = "产品";
            //    item.magicWallManager = _manager;
            //    item.Datas = Transfer(_cardData.ScrollDic[CrossCardNavType.Product]);
            //    _cellDatas.Add(item);
            //    _scrollItemNumber++;
            //    index++;
            //}
            //if (_hasActivity)
            //{
            //    CrossCardCellData item = new CrossCardCellData();
            //    item.Category = CrossCardCategoryEnum.ACTIVITY;
            //    item.crossCardAgent = this;
            //    item.EnvId = _dataId;
            //    item.Likes = Likes;
            //    item.Index = index;
            //    item.Title = "活动";
            //    item.magicWallManager = _manager;
            //    item.Datas = Transfer(_cardData.ScrollDic[CrossCardNavType.Activity]);
            //    _cellDatas.Add(item);
            //    _scrollItemNumber++;
            //    index++;
            //}

            //if (_hasVideo)
            //{
            //    CrossCardCellData item = new CrossCardCellData();
            //    item.Category = CrossCardCategoryEnum.VIDEO;
            //    item.crossCardAgent = this;
            //    item.EnvId = _dataId;
            //    item.Likes = Likes;
            //    item.Index = index;
            //    item.Title = "视频";
            //    item.magicWallManager = _manager;
            //    item.Datas = Transfer(_cardData.ScrollDic[CrossCardNavType.Video]);
            //    _cellDatas.Add(item);

            //    _scrollItemNumber++;
            //    index++;
            //}


            //// Updatedata
            //crossCardScrollViewController.SetUpCardAgent(this);
            //crossCardScrollViewController.UpdateData(_cellDatas);

            //crossCardScrollViewController.OnSelectionChanged(OnSelectionChanged);

            //crossCardScrollBar.UpdateData(_cellDatas);
            //crossCardScrollBar.UpdateComponents();
            //crossCardScrollBar.OnSelectionChanged(OnBarSelectionChanged);
            //crossCardScrollBar.SetScrollOperatedAction(OnScrollOperated);

            //// 处理businesscard
            //_hasListBtn = false;

            //// 设置完成回调
            //SetOnCreatedCompleted(OnCreatedCompleted);

            //isPrepared = true;


        }


        private List<CrossCardCellData> Transfer(List<ScrollData> scrollDatas) {
            List<CrossCardCellData> crossCardCellDatas = new List<CrossCardCellData>();

            for (int i = 0; i < scrollDatas.Count; i++) {
                CrossCardCellData crossCardCellData = new CrossCardCellData();
                crossCardCellData.Image = scrollDatas[i].Cover;
                crossCardCellData.Index = i;
                crossCardCellData.Description = scrollDatas[i].Description;
                if (scrollDatas[i].Type == 0)
                {
                    crossCardCellData.IsImage = true;
                }
                else {
                    crossCardCellData.Category = CrossCardCategoryEnum.VIDEO;
                    crossCardCellData.IsImage = false;
                    crossCardCellData.VideoUrl = scrollDatas[i].Src;
                }

                crossCardCellData.magicWallManager = _manager;

                crossCardCellDatas.Add(crossCardCellData);
            }


            return crossCardCellDatas;
        }

        private CrossCardCellData TransferScrollData(ScrollData scrollData)
        {

            CrossCardCellData crossCardCellData = new CrossCardCellData();
            crossCardCellData.Image = scrollData.Cover;
            crossCardCellData.Index = 0;
            crossCardCellData.Description = scrollData.Description;
            if (scrollData.Type == 0)
            {
                crossCardCellData.IsImage = true;
            }
            else
            {
                crossCardCellData.IsImage = false;
                crossCardCellData.VideoUrl = scrollData.Src;
            }

            crossCardCellData.magicWallManager = _manager;


            return crossCardCellData;
        }


        public override void FullDisplayAfterGoFront() {

            Debug.Log("Full Display After Go Front");

            // 初始化组件
            _crossScrollAgent.CompleteInit();

        }




    }
}





