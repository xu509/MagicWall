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
        [SerializeField] SliceScrollAgent _sliceScrollAgent;
        [SerializeField] RectTransform _buttomTool;

        private List<string> _envCards = new List<string>();
        private OperateCardDataSlide _data;


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


        public override void InitData(OperateCardData operateCardData)
        {
            OperateCardDataSlide operateCardDataSlide = (OperateCardDataSlide)operateCardData;
            //Debug.Log("Do In slice: " + operateCardDataSlide.Title);
            _data = operateCardDataSlide;

            InitUI();

            // 初始化卡片块
            InitAgency();
            _questionTypeEnum = QuestionTypeEnum.SliceCard;

            _sliceScrollAgent.Init(_data, (data, scrollDirection) => {
                UpdateDescription(data.Description);

            }, OnClickScale, DoVideo);

        }



        public void SwitchScaleMode(Texture texture)
        {
            //scaleController.SetImage(texture);
            //scaleController.OpenScaleBox();
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



        private void OnClickScale(string str)
        {
            var tex = TextureResource.Instance.GetTexture(MagicWallManager.FileDir + str);
            InitScaleAgent(tex);
        }


        public override void FullDisplayAfterGoFront()
        {

            Debug.Log("Full Display After Go Front");

            // 初始化组件
            _sliceScrollAgent.CompleteInit();

            // 显示标题
            _title.text = _data.Title;

        }


        /// <summary>
        /// 调整卡片UI
        /// </summary>
        private void InitUI() {
            MagicWallManager manager =  GameObject.Find("MagicWall").GetComponent<MagicWallManager>();

            var position = _titleContainer.anchoredPosition;
            //Debug.Log("position : " + position);


            if (manager.screenTypeEnum == ScreenTypeEnum.Screen1080P)
            {
                // 调整标题位置，标题大小
                var titlePosition1080 = new Vector2(0.0f, -165.0f);
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