using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScaleAgentCell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public ScrollRect scrollRect;
    private RectTransform imgRtf;
    private float originalDistance;
    private List<Touch> touchs = new List<Touch>();//当前图片的所有Touch

    private bool canScroll;//ScrollView是否可以移动（缩放时不能移动）

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
        touchs = new List<Touch>();
        for (int i=0; i<Input.touchCount; i++)
        {
            if (isTouchOnImage(Input.GetTouch(i).position))
            {
                touchs.Add(Input.GetTouch(i));
            }
        }
        if (touchs.Count <= 1)
        {
            canScroll = true;
            return;
        }
        canScroll = false;
        //多点触摸, 放大缩小
        Touch newTouch1 = touchs[0];
        Touch newTouch2 = touchs[1];
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
        if (canScroll)
        {
            scrollRect.OnBeginDrag(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canScroll)
        {
            scrollRect.OnDrag(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        if (touchs.Count == 1)
        {
            scrollRect.OnEndDrag(eventData);
        }
    }


}
