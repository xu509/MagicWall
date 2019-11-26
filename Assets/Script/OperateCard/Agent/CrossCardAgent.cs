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
        [SerializeField] Image _backgroundImg;
        [SerializeField] Image _scrollImg;



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

        [SerializeField] RectTransform _buttomTool;


        private Vector2 Description_Origin_Position = Vector2.zero + new Vector2(0, 20);
        private Vector2 Description_Go_Position = Vector2.zero;
        private Vector2 ButtomTool_Origin_Position = new Vector2(0, 100);
        private Vector2 ButtomTool_Go_Position = new Vector2(0, 50);

        #endregion


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

            //UpdateToolComponent();

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
                DoUpdate();
                Debug.Log("Has Changed : [NAV]" + navtype + "[data]" + data);
                UpdateDescription(data.Description);

                if (scrollDirection == ScrollDirectionEnum.Left)
                {
                    _scrollBarAgent.TurnLeft();
                }
                else if (scrollDirection == ScrollDirectionEnum.Right) {
                    _scrollBarAgent.TurnRight();
                }

            }, OnClickScale, DoVideo);

            InitUI();

            _scrollBarAgent.Init(_navList,(dir)=> {

            });

        }


        public override void FullDisplayAfterGoFront() {

            Debug.Log("Full Display After Go Front");

            // 初始化组件
            _crossScrollAgent.CompleteInit();

            // 显示标题
            _title.text = _cardData.Title;

            DoUpdate();

        }


        private void OnClickScale(string str) {
            var tex = TextureResource.Instance.GetTexture(MagicWallManager.FileDir + str);
            InitScaleAgent(tex);
        }


        private void InitUI() {
            MagicWallManager manager = GameObject.Find("MagicWall").GetComponent<MagicWallManager>();
            if (manager.screenTypeEnum == ScreenTypeEnum.Screen1080P)
            {
                // 提示按钮
                _questionContainer.anchoredPosition = new Vector2(391, 243);

                // 描述模块

            }
            else {
                _questionContainer.anchoredPosition = new Vector2(391, 243);

            }

            /// 设置主题相关
             
            // 设置遮罩图片
            _backgroundImg.sprite = _manager.themeManager.GetService().GetCardBackShade(FlockCardTypeEnum.CrossCard);
            // 设置标题
            _title.color = _manager.themeManager.GetService().GetFontColor();
            // 设置描述
            _description.color = _manager.themeManager.GetService().GetFontColor();
            // 设置scroll
            _scrollImg.sprite = _manager.themeManager.GetService().GetScrollBarSprite();
        }




    }
}





