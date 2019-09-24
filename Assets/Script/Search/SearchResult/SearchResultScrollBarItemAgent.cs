using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


namespace MagicWall
{
    public class SearchResultScrollBarItemAgent : MonoBehaviour
    {

        private int _index;
        private float _width;   // 图片宽度
        private float _default_width;   // 默认宽度
        private float _itemHeight;   // 默认宽度
        private float _minItemWidthFactor;
        private float _maxItemWidthFactor;

        [SerializeField] private Image _image;  //  图片



        #region 引用
        public int Index { get { return _index; } set { _index = value; } }
        #endregion

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init(int index, float minItemWidthFactor, float maxItemWidthFactor
            , float itemHeight)
        {
            _index = index;
            //_total = total;
            _itemHeight = itemHeight;

            _minItemWidthFactor = minItemWidthFactor;
            _maxItemWidthFactor = maxItemWidthFactor;


            GetComponent<RectTransform>().sizeDelta = new Vector2(0, itemHeight);


            SetImageWidth(0);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="factor"> 0 - 1</param>
        public void Refresh(float factor)
        {
            //factor;
            SetImageWidth(factor);

        }

        private void SetImageWidth(float widthOffset)
        {
            float max_x = Mathf.Lerp(_minItemWidthFactor, _maxItemWidthFactor, widthOffset);

            GetComponent<RectTransform>().DOAnchorMin(new Vector2(1 - max_x, 1), 0.2f);
            GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0, GetComponent<RectTransform>().anchoredPosition.y);
        }

    }
}