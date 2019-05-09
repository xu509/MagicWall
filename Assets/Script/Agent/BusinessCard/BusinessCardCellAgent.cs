using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class BusinessCardCellAgent : MonoBehaviour
{
    int _index; // 当前索引，从0开始

    private BusinessCardData _businessCardData;


    /// <summary>
    ///  Component
    /// </summary>
    [SerializeField] Button _btnClose;
    [SerializeField] Button _btnReturn;
    [SerializeField] Button _btnNext;


    void Update()
    {
        
    }

    public void UpdateContent(BusinessCardData businessCardData) {
        _businessCardData = businessCardData;
    }


}


