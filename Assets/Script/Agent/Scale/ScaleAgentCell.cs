﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MagicWall
{
    public class ScaleAgentCell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        public ScrollRect scrollRect;
        private RectTransform imgRtf;
        private float originalDistance;
        //private List<Touch> touchs = new List<Touch>();//当前图片的所有Touch

        private bool canScroll;//ScrollView是否可以移动（缩放时不能移动）
        private Dictionary<int, Vector2> pointIdAndPos;

        //private Touch oldTouch1;  //上次触摸点1(手指1)
        //private Touch oldTouch2;  //上次触摸点2(手指2)

        public ScaleAgent scaleAgent;
        private float startScalePer;//开始缩放时比例

        void Start()
        {
            imgRtf = GetComponent<RectTransform>();
            pointIdAndPos = new Dictionary<int, Vector2>();
        }

        // Update is called once per frame
        void Update()
        {
            //print(Input.touchCount);
            //touchs = new List<Touch>();
            //for (int i = 0; i < Input.touchCount; i++)
            //{
            //    if (isTouchOnImage(Input.GetTouch(i).position))
            //    {
            //        Debug.Log(111111111);
            //        touchs.Add(Input.GetTouch(i));
            //    }
            //}
            //if (touchs.Count <= 1)
            //{
            //    canScroll = true;
            //    return;
            //}
            //canScroll = false;
            ////多点触摸, 放大缩小
            //Touch newTouch1 = touchs[0];
            //Touch newTouch2 = touchs[1];
            ////第2点刚开始接触屏幕, 只记录，不做处理
            //if (newTouch2.phase == TouchPhase.Began)
            //{
            //    Vector2 oldPos1 = newTouch1.position;
            //    Vector2 oldPos2 = newTouch2.position;
            //    originalDistance = Vector2.Distance(oldPos1, oldPos2);
            //    startScalePer = scaleAgent.currentScale;
            //    return;
            //}
            //float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);
            //float s = startScalePer + (newDistance - originalDistance) / originalDistance / 2;
            //if (s <= 1)
            //{
            //    s = 1;
            //}
            //else if (s >= scaleAgent.maxScale)
            //{
            //    s = scaleAgent.maxScale;
            //}
            //scaleAgent.currentScale = s;
            //scaleAgent.ResetImage();
        }


        bool isTouchOnImage(Vector3 touchPosition)
        {
            Vector3 imgScreenPos = Camera.main.WorldToScreenPoint(transform.position);
            Vector2 leftBottom = new Vector2(imgScreenPos.x - imgRtf.sizeDelta.x * 0.5f, imgScreenPos.y - imgRtf.sizeDelta.y * 0.5f);
            if (touchPosition.x >= leftBottom.x && touchPosition.y >= leftBottom.y && touchPosition.x <= leftBottom.x + imgRtf.sizeDelta.x && touchPosition.y <= leftBottom.y + imgRtf.sizeDelta.y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            pointIdAndPos.Add(eventData.pointerId, eventData.position);
            if (pointIdAndPos.Count == 1)
            {
                canScroll = true;
            }   else
            {
                if (canScroll)
                {
                    List<Vector2> poss = new List<Vector2>();
                    foreach (var item in pointIdAndPos.Values)
                    {
                        poss.Add(item);
                    }
                    Vector2 old1 = poss[0];
                    Vector2 old2 = poss[1];
                    originalDistance = Vector2.Distance(old1, old2);
                    startScalePer = scaleAgent.currentScale;
                }
                canScroll = false;
            }

            if (canScroll)
            {
                scrollRect.OnBeginDrag(eventData);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            pointIdAndPos[eventData.pointerId] = eventData.position;
            if (pointIdAndPos.Count > 1)
            {
                List<Vector2> poss = new List<Vector2>();
                foreach (var item in pointIdAndPos.Values)
                {
                    poss.Add(item);
                }
                Vector2 new1 = poss[0];
                Vector2 new2 = poss[1];
                float newDistance = Vector2.Distance(new1, new2);
                float s = startScalePer + (newDistance - originalDistance) / originalDistance;
                if (s <= 1)
                {
                    s = 1;
                }
                else if (s >= scaleAgent.maxScale)
                {
                    s = scaleAgent.maxScale;
                }
                scaleAgent.currentScale = s;
                scaleAgent.ResetImage();
            }
            if (canScroll)
            {
                scrollRect.OnDrag(eventData);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            pointIdAndPos.Remove(eventData.pointerId);
            if (pointIdAndPos.Count == 0)
            {
                scrollRect.OnEndDrag(eventData);
            }
        }


    }
}