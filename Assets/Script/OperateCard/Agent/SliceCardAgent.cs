using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;


//
//  滑动卡片代理，用于product和activity
//
namespace MagicWall
{
    public class SliceCardAgent : CardAgent
    {

        [SerializeField, Header("SliceCardAgent UI")] Text _title;
        [SerializeField] RectTransform _titleContainer;
        [SerializeField] Text _description;
        [SerializeField] SliceCardScrollViewController _scrollController;
        [SerializeField] RectTransform _buttomTool;

        private List<string> _envCards = new List<string>();
        private OperateCardDataSlide _operateCardDataSlide;


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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">产品ID或活动ID</param>
        /// <param name="type">类型</param>
        public void InitSliceCard()
        {
            InitUI();

            InitAgency();


            // 获取产品标题
            _title.text = _operateCardDataSlide.Title;
            //_titleText.text = _operateCardDataSlide.Title;

            InitComponents(_operateCardDataSlide.ExtraCardData);

            // 获取产品详细（图片，描述）
            List<SliceCardCellData> cellDatas = new List<SliceCardCellData>();
            for (int i = 0; i < _operateCardDataSlide.ScrollData.Count; i++)
            {
                var data = _operateCardDataSlide.ScrollData[i];

                SliceCardCellData cellData = new SliceCardCellData();
                cellData.Type = 0;
                cellData.sliceCardAgent = this;
                cellData.magicWallManager = _manager;
                cellData.LoadDetail(data);
                cellDatas.Add(cellData);
            }



            _scrollController.SetUpCardAgent(this);
            _scrollController.UpdateData(cellDatas);
            _scrollController.OnSelectionChanged(OnScrollControllerSelectionChanged);
            _scrollController.SetOnScrollerOperated(OnOperationAction);

            SetOnCreatedCompleted(OnCreatedCompleted);

            isPrepared = true;
        }


        public void SwitchScaleMode(Texture texture)
        {
            //scaleController.SetImage(texture);
            //scaleController.OpenScaleBox();
        }


        private void OnScrollControllerSelectionChanged(int index)
        {
            SliceCardBaseCell<SliceCardCellData, SliceCardCellContext> cell = _scrollController.GetCell(index);
            cell.GetComponent<RectTransform>().SetAsLastSibling();

            string description = _scrollController.GetCurrentCardDescription();

            //  更新描述
            UpdateDescription(description);

            // 更新下方操作栏
            UpdateToolComponent();
        }

        public void UpdateDescription(string description)
        {
            // 从透明到不透明，向下移动
            _description.text = description;

        }

        private void UpdateToolComponent()
        {


        }


        private void OnOperationAction()
        {
            DoUpdate();
        }


        private void OnCreatedCompleted()
        {

            string description = _scrollController.GetCurrentCardDescription();

            //  更新描述
            UpdateDescription(description);

            //  更新操作栏
            UpdateToolComponent();

        }

        public override void InitData(OperateCardData operateCardData)
        {
            OperateCardDataSlide operateCardDataSlide = (OperateCardDataSlide)operateCardData;
            //Debug.Log("Do In slice: " + operateCardDataSlide.Title);
            _operateCardDataSlide = operateCardDataSlide;

            InitSliceCard();

        }



        private void InitUI() {
            MagicWallManager manager =  GameObject.Find("MagicWall").GetComponent<MagicWallManager>();

            var position = _titleContainer.anchoredPosition;
            //Debug.Log("position : " + position);


            if (manager.screenTypeEnum == ScreenTypeEnum.Screen1080P)
            {
                // 调整标题位置，标题大小
                var titlePosition1080 = new Vector2(0.0f, -250.0f);
                var titleFontSize = 40;
                _titleContainer.anchoredPosition = titlePosition1080;
                _title.fontSize = titleFontSize;

                // 调整描述字体大小与位置
                var descriptionFontSize = 30;
                _description.fontSize = descriptionFontSize;

            }
            else {

                // 调整标题位置，标题大小
                var titlePosition720 = new Vector2(0.0f, -165.0f);
                var titleFontSize = 30;
                _titleContainer.anchoredPosition = titlePosition720;
                _title.fontSize = titleFontSize;

                // 调整描述字体大小与位置
                var descriptionFontSize = 20;
                _description.fontSize = descriptionFontSize;
            }






        }



    }


}