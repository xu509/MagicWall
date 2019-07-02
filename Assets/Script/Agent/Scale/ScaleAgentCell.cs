using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScaleAgentCell : MonoBehaviour
{

    public ScrollRect scrollRect;
    private RectTransform imgRtf;
    private float originalDistance;
    private List<Touch> touchs = new List<Touch>();

    //记录单指双指的变换
    private bool isSingleFinger;

    private Touch oldTouch1;  //上次触摸点1(手指1)
    private Touch oldTouch2;  //上次触摸点2(手指2)

    public ScaleAgent scaleAgent;
    private float startScalePer;//开始缩放时比例

    void Start()
    {
        imgRtf = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        for (int i=0; i<Input.touchCount; i++)
        {
            if (isTouchOnImage(Input.GetTouch(i).position))
            {
                if (!touchs.Contains(Input.GetTouch(i)))
                {
                    touchs.Add(Input.GetTouch(i));
                }
            }
        }
        
        if (touchs.Count == 1)
        {
            isSingleFinger = true;
        }
        else if (touchs.Count > 1)
        {
            if (isSingleFinger)
            {
                Vector2 oldPos1 = touchs[0].position;
                Vector2 oldPos2 = touchs[1].position;
                originalDistance = Vector2.Distance(oldPos1, oldPos2);
            }
            if (touchs[0].phase == TouchPhase.Moved || touchs[1].phase == TouchPhase.Moved)
            {
                Vector2 newPos1 = touchs[0].position;
                Vector2 newPos2 = touchs[1].position;
                float newDistance = Vector2.Distance(newPos1, newPos2);
                float s = startScalePer + (newDistance - originalDistance) / originalDistance / 2;
                scaleAgent.currentScale = s;
                scaleAgent.ResetImage();
            }
            else if (touchs[0].phase == TouchPhase.Ended || touchs[1].phase == TouchPhase.Ended && isSingleFinger == false)
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
                touchs = new List<Touch>();
            }
            isSingleFinger = false;

        }
        */
        if (Input.touchCount <= 1)
        {
            return;
        }
        //多点触摸, 放大缩小
        Touch newTouch1 = Input.GetTouch(0);
        Touch newTouch2 = Input.GetTouch(1);
        //第2点刚开始接触屏幕, 只记录，不做处理
        if (newTouch2.phase == TouchPhase.Began)
        {
            Vector2 oldPos1 = newTouch1.position;
            Vector2 oldPos2 = newTouch2.position;
            originalDistance = Vector2.Distance(oldPos1, oldPos2);
            startScalePer = scaleAgent.currentScale;
            return;
        }
        float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);
        float s = startScalePer + (newDistance - originalDistance) / originalDistance / 2;
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


    bool isTouchOnImage(Vector3 touchPosition)
    {
        //return true;
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

}
