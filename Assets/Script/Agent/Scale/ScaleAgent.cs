using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MagicWall
{
    public class ScaleAgent : MonoBehaviour
    {
        Texture _imageTexture;

        [SerializeField, Header("图片")] RawImage image;
        [SerializeField] RectTransform tool_box;
        [Header("图片放大最大倍数")]
        public float maxScale = 2.0f;//最大倍数
        [Header("图片放大次数")]
        public int plusCount = 5;//放大次数
        [Header("最大高度")]
        public float MAX_HEIGHT = 950;

        Action OnCloseClicked;
        Action _onReturnClicked;
        Action OnUpdate;
        Action _onOpen; //打开时


        private RectTransform imgRtf;
        public float currentScale;//当前缩放倍数
        private float perScale;//每次放大倍数
        private Vector2 originalSize;

        // 提示功能相关
        [SerializeField] RectTransform _questionContainer;
        [SerializeField] QuestionAgent _questionPrefab;
        private bool _showQuestion;
        private QuestionAgent _questionAgent;
        // 提示功能相关 结束



        // Start is called before the first frame update
        void Start()
        {
            _onOpen.Invoke();
            _showQuestion = false;
        }

        void FixedUpdate()
        {

            // 当缩放窗口打开时，保证卡片不被关闭
            //OnUpdate.Invoke();

        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //print(imgRtf.localPosition);

        }

        public void SetImage(Texture texture)
        {

            imgRtf = image.GetComponent<RectTransform>();
            currentScale = 1;
            perScale = (maxScale - 1) / plusCount;

            // 需要防止变形
            _imageTexture = texture;

            SizeToScale();

            image.texture = texture;

        }

        public void SetOnCloseClicked(Action action)
        {
            OnCloseClicked = action;
        }

        public void SetOnReturnClicked(Action action)
        {
            _onReturnClicked = action;
        }

        public void SetOnUpdated(Action action)
        {
            OnUpdate = action;
        }

        public void SetOnOpen(Action action)
        {
            _onOpen = action;
        }


        public void DoReturn()
        {
            _onReturnClicked?.Invoke();
        }

        public void DoClose()
        {
            Debug.Log("关闭缩放窗口");

            OnCloseClicked?.Invoke();
        }

        // 点击放大按钮
        public void DoPlus()
        {
            //Debug.Log("放大图片操作");
            if (currentScale < maxScale)
            {
                currentScale += perScale;
                if (currentScale > maxScale)
                {
                    currentScale = maxScale;
                }
                ResetImage();
            }

        }

        // 点击减少按钮
        public void DoMinus()
        {
            //Debug.Log("缩小图片操作");
            if (currentScale > 1.0f)
            {
                currentScale -= perScale;
                if (currentScale < 1)
                {
                    currentScale = 1;
                }
                ResetImage();
                imgRtf.anchoredPosition = Vector2.zero;
            }
        }


        public void ResetImage()
        {
            //imgRtf.sizeDelta = new Vector2(originalSize.x * currentScale, originalSize.y * currentScale);
            imgRtf.localScale = new Vector3(currentScale, currentScale, currentScale);
        }

        private void SizeToScale()
        {
            //图片原始宽高
            float w = _imageTexture.width, h = _imageTexture.height;
            float r = w / h;
            GetComponent<AspectRatioFitter>().aspectRatio = r;
            RectTransform rtf = GetComponent<RectTransform>();
            if (rtf.sizeDelta.y > MAX_HEIGHT)
            {
                float width = r * MAX_HEIGHT;
                rtf.sizeDelta = new Vector2(width, MAX_HEIGHT);
            }

            //string str = "";

            //if (_imageTexture.width > MAX_WIDTH)
            //{
            //    float radio = _imageTexture.width / _imageTexture.height;
            //    w = MAX_WIDTH;
            //    h = w / radio;

            //    str = "WIDTH > MAX WIDTH";
            //}
            //else
            //{
            //    str = "WIDTH < MAX WIDTH";
            //}

            ////设置图片原始大小
            //originalSize = new Vector2(w, h);

            //// 将mask调整至新的大小
            ////parent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
            ////parent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);

            ////parent.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
            ////parent.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);

            ////imgRtf.sizeDelta = originalSize;
            ////parent.sizeDelta = originalSize;
            ////parent.parent.GetComponent<RectTransform>().sizeDelta = originalSize;

            //imgRtf.sizeDelta = originalSize;
            //GetComponent<RectTransform>().sizeDelta = originalSize;


        }




        #region 提示内容
        public void DoQuestion()
        {
            if (_showQuestion)
            {
                _questionAgent?.CloseReminder();
            }
            else
            {
                _questionAgent = Instantiate(_questionPrefab, _questionContainer);
                _questionAgent.Init(OnQuestionClose);
                _questionAgent.ShowReminder(QuestionTypeEnum.ScalePanel);
                _showQuestion = true;
            }
        }

        private void OnQuestionClose()
        {
            _showQuestion = false;
        }

        #endregion



    }
}