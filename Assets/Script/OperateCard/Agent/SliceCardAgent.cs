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
        [SerializeField] RectTransform _descContainer;
        [SerializeField] RectTransform _buttomsContainer;
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
                DoUpdate();
                UpdateDescription(data.Description);
                AdjustUILocation();
            }, OnClickScale, DoVideo,()=> {
                // scale 部分调整完毕
                //AdjustUI();
                DoUpdate();
                ShowUI();

                var data = operateCardDataSlide.ScrollData[0];
                UpdateDescription(data.Description);
            });

            var extraDatas = operateCardDataSlide.ExtraCardData;
            InitComponents(extraDatas);

        }



        public void UpdateDescription(string description)
        {
            // 从透明到不透明，向下移动
            _description.text = description;

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



        private void ShowUI() {
            // 显示UI
            Debug.Log("显示UI");
            AdjustUILocation();
            //Debug.Log(scrollSize);



        }

        private void AdjustUILocation() {
            // 调整标题位置
            // height : 400 => position 7 : -235
            // 900 => -5

            var scrollSize = _sliceScrollAgent.GetCurrentImage();
            var height = scrollSize.y;

            var offsetTitle = (height - 400) / 500f;
            Vector2 toLocation = Vector2.LerpUnclamped(new Vector2(0, -235), new Vector2(0, -5), offsetTitle);
            _titleContainer.anchoredPosition = toLocation;

            Debug.Log("调整标题位置： " + toLocation);


            // 调整描述位置
            // 400 => 230
            // 900 => -34
            var offsetDescription = (height - 400) / 500f;
            Vector2 toDLocation = Vector2.LerpUnclamped(new Vector2(0, 230), new Vector2(0, -34), offsetDescription);
            _descContainer.anchoredPosition = toDLocation;

            Debug.Log("调整描述位置： " + toDLocation);


            // 调整按钮位置
            // 400 => 190
            // 900 => -67
            var offsetBottoms = (height - 400) / 500f;
            Vector2 toBLocation = Vector2.LerpUnclamped(new Vector2(0, 190), new Vector2(0, -67), offsetBottoms);
            _buttomsContainer.anchoredPosition = toBLocation;
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
                var titleFontSize = 28;
                _title.fontSize = titleFontSize;

                // 调整描述字体大小与位置
                var descriptionFontSize = 28;
                _description.fontSize = descriptionFontSize;

            }
            else {

                // 调整标题位置，标题大小
                var titlePosition720 = new Vector2(0.0f, -165.0f);
                var titleFontSize = 30;
                _title.fontSize = titleFontSize;

                // 调整描述字体大小与位置
                var descriptionFontSize = 20;
                _description.fontSize = descriptionFontSize;
            }

        }



    }


}