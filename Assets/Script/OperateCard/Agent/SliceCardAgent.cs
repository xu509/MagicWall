﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;


//
//  滑动卡片代理，用于product和activity
//
namespace MagicWall
{
    public class SliceCardAgent : CardAgent
    {

        [SerializeField, Header("SliceCardAgent UI")] Text _title;
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

            InitAgency();


            // 获取产品标题
            _title.text = _operateCardDataSlide.Title;


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







            //List<SliceCardCellData> cellDatas;
            //if (type == MWTypeEnum.Product)
            //{
            //    Product product = _manager.daoService.GetProductDetail(DataId);

            //    // 获取产品标题
            //    _title.text = product.Name;

            //    // 获取产品所属公司信息
            //    InitComponents(_manager.daoService.GetEnvCards(product.Ent_id).Count > 0);

            //    // 获取产品详细（图片，描述）
            //    cellDatas = new List<SliceCardCellData>();
            //    for (int i = 0; i < product.ProductDetails.Count; i++)
            //    {
            //        SliceCardCellData cellData = new SliceCardCellData();
            //        cellData.Type = 0;
            //        cellData.sliceCardAgent = this;
            //        cellData.magicWallManager = _manager;
            //        cellData.LoadProductDetail(product.ProductDetails[i]);
            //        cellDatas.Add(cellData);
            //    }
            //}
            //else {
            //    // 初始化活动信息
            //    Activity activity = _manager.daoService.GetActivityDetail(DataId);

            //    _title.text = activity.Name;

            //    // 获取产品所属公司信息
            //    InitComponents(_manager.daoService.GetEnvCards(activity.Ent_id).Count > 0);

            //    // 获取产品详细（图片，描述）
            //    cellDatas = new List<SliceCardCellData>();
            //    for (int i = 0; i < activity.ActivityDetails.Count; i++)
            //    {
            //        SliceCardCellData cellData = new SliceCardCellData();
            //        cellData.Type = 0;
            //        cellData.sliceCardAgent = this;
            //        cellData.magicWallManager = _manager;
            //        cellData.LoadActivityDetail(activity.ActivityDetails[i]);
            //        cellDatas.Add(cellData);
            //    }
            //}


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

            //_description.GetComponent<RectTransform>().anchoredPosition = Description_Origin_Position;
            //_description.DOFade(0, 0f);

            //_description.GetComponent<RectTransform>().DOAnchorPos(Description_Go_Position, 0.5f);
            //_description.DOFade(1, 0.5f);

        }

        private void UpdateToolComponent()
        {


            //_buttomTool.GetComponent<RectTransform>().anchoredPosition = ButtomTool_Origin_Position;
            //_buttomTool.GetComponent<CanvasGroup>().DOFade(0, Time.deltaTime);

            //_buttomTool.GetComponent<RectTransform>().DOAnchorPos(ButtomTool_Go_Position, 0.5f);
            //_buttomTool.GetComponent<CanvasGroup>().DOFade(1, 0.5f);


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



    }


}