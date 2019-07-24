using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System;

public class BusinessCardAgent : MonoBehaviour
{
    int _index; // 当前索引，从0开始

    List<BusinessCardCellAgent> pool;

    bool _doingNext = false;
    bool _doingReturn = false;


    /// <summary>
    ///  Component
    /// </summary>
    [SerializeField] BusinessCardCellAgent _cellPrefab;
    [SerializeField] RectTransform _contentContainer;
    [SerializeField] Button _btnClose;
    [SerializeField] Button _btnReturn;
    [SerializeField] Button _btnNext;
    [SerializeField] float _heightFactor;
    [SerializeField] float _widthFactor;

    private Action _onHandleUpdateAction;
    private Action _onClickCloseAction;


    void Update()
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="images">图片相对路径</param>
    /// <param name="position">生成位置</param>
    /// <param name="onHandleUpdateAction">当操作更新时的回调</param>
    /// <param name="onClickCloseAction">当操作关闭时的回调</param>
    public void Init(string[] images,float cardWidth,Vector2 position, Action onHandleUpdateAction, Action onClickCloseAction)
    {
        _onHandleUpdateAction = onHandleUpdateAction;
        _onClickCloseAction = onClickCloseAction;
        InitComponents(position,cardWidth);
        UpdateContents(images);
        UpdateToolStatus();

    }




    public void DoClickNext()
    {

        Debug.Log("DoClickNext");

        if (!_doingNext) {
            _doingNext = true;

            pool[_index + 1].GoFront(() => {
                pool[_index].GoBackLeft();
                _index++;
                UpdateToolStatus();

                _doingNext = false;
            });
        }
        _onHandleUpdateAction.Invoke();
    }

    public void DoClickClose() {
        //_cardAgent.CloseBusinessCard();
        //_cardAgent.DoUpdate();
        _onClickCloseAction.Invoke();
    }

    public void DoClickReturn()
    {
        if (!_doingReturn)
        {
            _doingReturn = true;
            pool[_index - 1].GoFront(() =>
            {
                pool[_index].GoBackRight();
                _index--;
                UpdateToolStatus();

                _doingReturn = false;
            });
        }
        _onHandleUpdateAction.Invoke();
    }


    public void UpdateContents(string[] images) {
        if (pool == null) {
            pool = new List<BusinessCardCellAgent>();
        }


        for (int i = 0; i < images.Length; i++) {

            //创建card
            BusinessCardCellAgent businessCardCellAgent = Instantiate(
                            _cellPrefab,
                            _contentContainer
                            ) as BusinessCardCellAgent;
            BusinessCardData businessCardData = new BusinessCardData();
            businessCardData.Index = i;
            businessCardData.address = images[i];
            businessCardCellAgent.UpdateContent(businessCardData);
            pool.Add(businessCardCellAgent);
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
                if (!_btnNext.gameObject.activeSelf)
                {
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
            else {
                _btnNext.gameObject.SetActive(true);
                if (!_btnReturn.gameObject.activeSelf)
                {
                    _btnReturn.gameObject.SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// 初始化组件
    /// </summary>
    /// <param name="position">父组件（卡片）位置</param>
    void InitComponents(Vector2 position,float cardWidth) {

        float height = Screen.height * _heightFactor;
        float width = height * _widthFactor;

        // 看情况在左侧，或者右侧
        float w =  position.x + cardWidth / 2 + width / 2 + 50;
        Vector2 genposition;

        if (w > Screen.width)
        {
            genposition = new Vector2(-(cardWidth / 2 + width / 2 + 50), 0);
        }
        else {
            genposition = new Vector2(cardWidth / 2 + width / 2 + 50, 0);
        }

        GetComponent<RectTransform>().anchoredPosition = genposition;
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }


}


