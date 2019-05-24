using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScaleAgent : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    Texture _imageTexture;

    [SerializeField] RawImage image;
    [SerializeField] RectTransform tool_box;

    public float maxScale = 2.0f;//最大倍数
    public int plusCount = 5;//放大次数

    float MAX_WIDTH = 660;
    float MAX_HEIGHT = 950;

    Action OnCloseClicked;
    Action _onReturnClicked;

    private RectTransform imgRtf;
    private float currentScale;//当前缩放倍数
    private float perScale;//每次放大倍数
    private Vector2 originalSize;

    //拖动
    private Vector2 first = Vector2.zero;
    private Vector2 second = Vector2.zero;

    // Start is called before the first frame update
    void Start()
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

    public void SetOnCloseClicked(Action action) {
        OnCloseClicked = action;
    }

    public void SetOnReturnClicked(Action action)
    {
        _onReturnClicked = action;
    }


    public void DoReturn() {
        _onReturnClicked?.Invoke();
    }

    public void DoClose()
    {
        OnCloseClicked?.Invoke();
    }

    // 点击放大按钮
    public void DoPlus()
    {
        // TODO 放大图片操作
        //Debug.Log("放大图片操作");
        if (currentScale < maxScale)
        {
            currentScale += perScale;
            ResetImage();
        }
        
    }

    // 点击减少按钮
    public void DoMinus()
    {
        // TODO 缩小 图片操作
        //Debug.Log("缩小图片操作");
        if (currentScale > 1.0f)
        {
            currentScale -= perScale;
            ResetImage();
            imgRtf.anchoredPosition = Vector2.zero;
        }
    }

    private void ResetImage()
    {
        imgRtf.sizeDelta = new Vector2(originalSize.x * currentScale, originalSize.y * currentScale);
    }

    private void SizeToScale()
    {
        // 将图片大小定在指定大小
        var parent = image.transform.parent.GetComponent<RectTransform>();


        float w = _imageTexture.width, h = _imageTexture.height;

        string str = "";

        if (_imageTexture.width > MAX_WIDTH)
        {
            float radio = _imageTexture.width / _imageTexture.height;
            w = MAX_WIDTH;
            h = w / radio;

            str = "WIDTH > MAX WIDTH";
        }
        else
        {
            str = "WIDTH < MAX WIDTH";
        }

        //设置图片原始大小
        originalSize = new Vector2(w, h);

        // 将mask调整至新的大小
        //parent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        //parent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);

        //parent.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        //parent.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);

        imgRtf.sizeDelta = originalSize;
        parent.sizeDelta = originalSize;
        parent.parent.GetComponent<RectTransform>().sizeDelta = originalSize;



    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        first = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        second = eventData.position;
        float x = imgRtf.anchoredPosition.x;
        float y = imgRtf.anchoredPosition.y;
        x += second.x - first.x;
        y += second.y - first.y;
        print("x:" + x + " y:" + y);
        Vector2 currentSize = imgRtf.sizeDelta;
        if (Mathf.Abs(x) <= (currentSize.x - originalSize.x)/2 && Mathf.Abs(y) <= (currentSize.y - originalSize.y)/2)
        {
            imgRtf.anchoredPosition = new Vector2(x, y);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }


}
