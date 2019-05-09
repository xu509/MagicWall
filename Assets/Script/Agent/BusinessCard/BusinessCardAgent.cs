using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class BusinessCardAgent : MonoBehaviour
{
    int _index; // 当前索引，从0开始

    List<BusinessCardCellAgent> pool;
    

    /// <summary>
    ///  Component
    /// </summary>
    [SerializeField] Button _btnClose;
    [SerializeField] Button _btnReturn;
    [SerializeField] Button _btnNext;


    void Update()
    {
        
    }

    public void DoClickNext() {

        Debug.Log("Do Click Next");

    }

    public void DoClickClose() {

        Debug.Log("Do Click Close");

    }

    public void DoClickReturn()
    {

        Debug.Log("Do Click Return");

    }


    public void UpdateContents() {
        if (pool == null) {
            pool = new List<BusinessCardCellAgent>();
        }


    }


    //  更新工具信息
    void UpdateToolStatus() {
        // 如果内容只有一张，则只显示xx按钮
        if (pool.Count == 1)
        {
            _btnReturn.gameObject.SetActive(false);
            _btnNext.gameObject.SetActive(false);
        }
        else
        {
            // 如果 Index 为 0 ，则不显示回退按钮
            if (_index == 0)
            {
                _btnReturn.gameObject.SetActive(false);
                if (!_btnNext.gameObject.activeSelf) {
                    _btnNext.gameObject.SetActive(true);
                }
            }
            else if (_index == pool.Count - 1)
            {
                _btnNext.gameObject.SetActive(false);
                if (!_btnReturn.gameObject.activeSelf)
                {
                    _btnReturn.gameObject.SetActive(true);
                }
            }
        }

    }



}


