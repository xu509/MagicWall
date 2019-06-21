using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScaleAgentCell : MonoBehaviour
{

    public ScrollRect scrollRect;
    private RectTransform imgRtf;
    //缩放
    Vector2 oldPos1 = Vector2.zero;
    Vector2 oldPos2 = Vector2.zero;
    //记录单指双指的变换
    private bool isSingleFinger;
    private float originalDistance;
    public ScaleAgent scaleAgent;
    private float startScalePer;//开始缩放时比例

    void Start()
    {
        imgRtf = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
            isSingleFinger = true;
        }
        else if (Input.touchCount > 1)
        {
            if (isSingleFinger)
            {
                oldPos1 = Input.GetTouch(0).position;
                oldPos2 = Input.GetTouch(1).position;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                Vector2 newPos1 = Input.GetTouch(0).position;
                Vector2 newPos2 = Input.GetTouch(1).position;
                float newDistance = Vector2.Distance(newPos1, newPos2);
                float oldDistance = Vector2.Distance(oldPos1, oldPos2);
                float s = startScalePer + (newDistance - oldDistance) / oldDistance;
                scaleAgent.currentScale = s;
                scaleAgent.ResetImage();
                oldPos1 = newPos1;
                oldPos2 = newPos2;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(1).phase == TouchPhase.Ended && isSingleFinger == false)
            {
                if (scaleAgent.currentScale < 1)
                {
                    scaleAgent.currentScale = 1;
                    scaleAgent.ResetImage();
                }
                else if (scaleAgent.currentScale > scaleAgent.maxScale)
                {
                    scaleAgent.currentScale = scaleAgent.maxScale;
                    scaleAgent.ResetImage();
                }
            }
            isSingleFinger = false;

        }

    }



    /*
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!startScale)
        {
            scrollRect.OnBeginDrag(eventData);
            return;
        }
        Vector2 vector2 = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            imgRtf,
            eventData.position,
            eventData.pressEventCamera,
            out vector2);
        second = vector2;
        originalDistance = Vector2.Distance(first, second);
        startScalePer = scaleAgent.currentScale;
    }

    public void OnDrag(PointerEventData eventData)
    {

        if (!startScale)
        {
            scrollRect.OnDrag(eventData);
            return;
        }
        Vector2 vector2 = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            imgRtf,
            eventData.position,
            eventData.pressEventCamera,
            out vector2);
        second = vector2;
        print("old:"+scaleAgent.currentScale);
        float newDistance = Vector2.Distance(first, second);
        float s = startScalePer + (newDistance-originalDistance) / originalDistance;
        print("new:" + s);
        scaleAgent.currentScale = s;
        scaleAgent.ResetImage();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!startScale)
        {
            scrollRect.OnEndDrag(eventData);
            return;
        }
        if (scaleAgent.currentScale < 1)
        {
            scaleAgent.currentScale = 1;
            scaleAgent.ResetImage();
        }   else if (scaleAgent.currentScale > scaleAgent.maxScale)
        {
            scaleAgent.currentScale = scaleAgent.maxScale;
            scaleAgent.ResetImage();
        }
    }
    */
}
