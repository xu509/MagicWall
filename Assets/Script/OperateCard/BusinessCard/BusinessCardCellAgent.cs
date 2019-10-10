using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

namespace MagicWall
{
    public class BusinessCardCellAgent : MonoBehaviour
    {
        int _index; // 当前索引，从0开始

        private BusinessCardData _businessCardData;

        private Vector2 backVectorRight;
        private Vector2 backVectorLeft;

        /// <summary>
        ///  Component
        /// </summary>
        [SerializeField] Image _image;

        void Awake()
        {
            float w = GetComponent<RectTransform>().rect.width;
            backVectorRight = new Vector2(w, 0);
            backVectorLeft = new Vector2(-w, 0);
        }


        void Update()
        {

        }

        public void UpdateContent(BusinessCardData businessCardData)
        {
            _businessCardData = businessCardData;

            _image.sprite = SpriteResource.Instance.GetData(MagicWallManager.FileDir + businessCardData.address);
            _index = businessCardData.Index;

            if (_index > 0)
            {

                GetComponent<RectTransform>().anchoredPosition = backVectorRight;

            }

        }

        public void GoFront(Action action)
        {
            GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, 1f).OnComplete(() => DoGoFrontComplete(action));
            GetComponent<RectTransform>().SetAsLastSibling();
        }

        public void GoBackLeft()
        {
            GetComponent<RectTransform>().DOAnchorPos(backVectorLeft, 1f);
        }

        public void GoBackRight()
        {
            GetComponent<RectTransform>().DOAnchorPos(backVectorRight, 1f);
        }

        private void DoGoFrontComplete(Action action)
        {
            action.Invoke();
        }


    }


}