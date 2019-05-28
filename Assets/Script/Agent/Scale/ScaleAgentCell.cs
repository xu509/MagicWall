using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScaleAgentCell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public ScrollRect scrollRect;
    private RectTransform imgRtf;
    //缩放
    private Vector2 first = Vector2.zero;//第一次鼠标按键位置
    private Vector2 second = Vector2.zero;//第二次
    private float originalDistance;//初始距离

    private bool startScale;//检测缩放
    public ScaleAgent scaleAgent;

    void Start()
    {
        imgRtf = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt) && startScale==false)
        {
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(imgRtf, Input.mousePosition, MagicWallManager.Instance.MainCamera, out pos))
            {
                startScale = true;
                first = pos;
                print("first:" + first);
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            startScale = false;
        }
    }

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
        print("second:" + second);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(11);

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
        float newDistance = Vector2.Distance(first, second);
        float s = newDistance / originalDistance;
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
}
