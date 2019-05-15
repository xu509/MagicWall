using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class BusinessCardCellAgent : MonoBehaviour
{
    int _index; // 当前索引，从0开始

    private BusinessCardData _businessCardData;

    private static Vector2 backVectorRight = new Vector2(792,0);
    private static Vector2 backVectorLeft = new Vector2(-792, 0);



    /// <summary>
    ///  Component
    /// </summary>
    [SerializeField] RawImage _image;


    void Update()
    {
        
    }

    public void UpdateContent(BusinessCardData businessCardData) {
        _businessCardData = businessCardData;

        _image.texture = businessCardData.Image;
        _index = businessCardData.Index;

        if (_index > 0) {

            GetComponent<RectTransform>().anchoredPosition = backVectorRight;

        }
        
    }

    public void GoFront(Action action) {
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

    private void DoGoFrontComplete(Action action) {
        action.Invoke();
    }


}


