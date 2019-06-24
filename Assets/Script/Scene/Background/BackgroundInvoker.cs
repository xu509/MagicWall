using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public static class BackgroundInvoker
{
    public static void Do(RectTransform rectTransform) {

        Debug.Log(11111);
        rectTransform.DOAnchorPos(Vector2.zero, Time.deltaTime);

    }
   

}
