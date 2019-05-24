using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ScaleAgent : MonoBehaviour
{
    Texture _imageTexture;

    [SerializeField] RawImage image;
    [SerializeField] RectTransform tool_box;


    float MAX_WIDTH = 660;
    float MAX_HEIGHT = 950;

    Action OnCloseClicked;
    Action _onReturnClicked;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetImage(Texture texture)
    {
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
        Debug.Log("放大图片操作");
    }

    // 点击减少按钮
    public void DoMinus()
    {
        // TODO 缩小 图片操作
        Debug.Log("缩小图片操作");
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


        // 将mask调整至新的大小
        parent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        parent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);

        parent.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        parent.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);


    }
}
