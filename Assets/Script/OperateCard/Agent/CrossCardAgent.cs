using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System;

namespace MagicWall {
    public class CrossCardAgent : CardAgent
    {
        [SerializeField,Header("Scroll")] CrossScrollAgent _crossScrollAgent;
        [SerializeField,Header("Scroll Bar")] ScrollBarAgent _scrollBarAgent;

        [SerializeField, Header("UI Container")] RectTransform _questionContainer;


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
            SetUpUI();
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

            //UpdateToolComponent();

        }

        ////
        ////  上一张
        ////
        //public void DoUp()
        //{
        //    int index = crossCardScrollViewController.CurrentIndex;

        //    // 获取上一个 index
        //    int up_index = index - 1;

        //    if (up_index < 0)
        //    {
        //        up_index = _scrollItemNumber - 1;
        //    }

        //    //Debug.Log("上一个，当前： " + index + " 目标： " + up_index);

        //    crossCardScrollViewController.SelectCell(up_index);

        //    DoUpdate();

        //}

        ////
        ////  下一张
        ////
        //public void DoDown()
        //{
        //    int index = crossCardScrollViewController.CurrentIndex;

        //    // 获取上一个 index
        //    int down_index = index + 1;
        //    if (down_index == _scrollItemNumber)
        //    {
        //        down_index = 0;
        //    }

        //    crossCardScrollViewController.SelectCell(down_index);

        //   // Debug.Log("下一个，当前： " + index + " 目标： " + down_index);


        //    DoUpdate();
        //}


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

            var _navList = new List<CrossCardNavType>();
            _navList.Add(CrossCardNavType.Index);

            if (operateCardDataCross.ScrollDic.ContainsKey(CrossCardNavType.CataLog))
            {
                _navList.Add(CrossCardNavType.CataLog);
            }

            if (operateCardDataCross.ScrollDic.ContainsKey(CrossCardNavType.Product))
            {
                _navList.Add(CrossCardNavType.Product);
            }

            if (operateCardDataCross.ScrollDic.ContainsKey(CrossCardNavType.Activity))
            {
                _navList.Add(CrossCardNavType.Activity);
            }

            if (operateCardDataCross.ScrollDic.ContainsKey(CrossCardNavType.Video))
            {
                _navList.Add(CrossCardNavType.Video);
            }

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            InitAgency();
            _questionTypeEnum = QuestionTypeEnum.CrossCard;

            _crossScrollAgent.Init(_cardData,(data,navtype,scrollDirection)=> {
                Debug.Log("Has Changed : [NAV]" + navtype + "[data]" + data);
                UpdateDescription(data.Description);

                if (scrollDirection == ScrollDirectionEnum.Left)
                {
                    _scrollBarAgent.TurnLeft();
                }
                else if (scrollDirection == ScrollDirectionEnum.Right) {
                    _scrollBarAgent.TurnRight();
                }

            }, OnClickScale);
            
            _scrollBarAgent.Init(_navList,(dir)=> {

            });

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

            // 显示标题
            _title.text = _cardData.Title;


        }


        private void OnClickScale(string str) {
            var tex = TextureResource.Instance.GetTexture(MagicWallManager.FileDir + str);
            InitScaleAgent(tex);
        }


        private void SetUpUI() {
            MagicWallManager manager = GameObject.Find("MagicWall").GetComponent<MagicWallManager>();
            if (manager.screenTypeEnum == ScreenTypeEnum.Screen1080P)
            {
                // 提示按钮
                _questionContainer.anchoredPosition = new Vector2(337, 194);

                // 描述模块

            }
            else {



            }

        }




    }
}





