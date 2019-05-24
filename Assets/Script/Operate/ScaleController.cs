using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



/// <summary>
/// Scale 管理器
/// </summary>
public class ScaleController : MonoBehaviour
{

    Texture _imageTexture;

    [SerializeField] RawImage image;
    [SerializeField] RectTransform normal_box;
    [SerializeField] RectTransform scale_box;

    [SerializeField] RectTransform tool_box;


    float MAX_WIDTH = 660;
    float MAX_HEIGHT = 950;



    void Start() {
        CloseScaleBox();
    }

    public void SetImage(Texture texture) {
        // 需要防止变形
        _imageTexture = texture;

        SizeToScale();

        image.texture = texture;

       
    }

    public void OpenScaleBox() {
        scale_box.gameObject.SetActive(true);
        normal_box.gameObject.SetActive(false);
    }

    public void CloseScaleBox()
    {
        normal_box.gameObject.SetActive(true);
        scale_box.gameObject.SetActive(false);
    }

    public void DoReturn() {
        CloseScaleBox();
    }



    private void SizeToScale() {
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
        else {
            str = "WIDTH < MAX WIDTH";
        }

        
        // 将mask调整至新的大小
        parent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        parent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);

        parent.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        parent.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);


    }


}
