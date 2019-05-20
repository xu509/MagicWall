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

    Texture imageTexture;

    [SerializeField] RawImage image;
    [SerializeField] RectTransform normal_box;
    [SerializeField] RectTransform scale_box;


    void Start() {
        CloseScaleBox();
    }

    public void SetImage(Texture texture) {
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
        Debug.Log("Do Return");
        CloseScaleBox();
    }



}
