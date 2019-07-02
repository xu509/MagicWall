using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

/// <summary>
/// 喜欢按钮代理
/// </summary>
public class ButtonLikeAgent : MonoBehaviour
{
    [SerializeField] RectTransform _btnLikeWithNumberContainer;
    [SerializeField] Image _btnLikeWithNumberHeartIcon;
    [SerializeField] RectTransform _btnLikeNoNumberContainer;
    [SerializeField] Image _btnLikeNoNumberHeartIcon;
    [SerializeField] Text _textInContainer;
    [SerializeField] Text _textInNoNumberContainer;


    private bool _hasNumber;
    private int _likes;
    private Action _onClickCallBack;

    public void Init(int likes,Action onClick) {
        _onClickCallBack = onClick;
        Refresh(likes);    }

    public void Refresh(int likes) {
        _likes = likes;


        if (likes == 0)
        {
            _hasNumber = false;
            InitComponentNoNumber();
        }
        else {
            _hasNumber = true;
            InitComponent();
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    ///     初始化正常控件
    /// </summary>
    private void InitComponent() {
        _btnLikeWithNumberContainer.gameObject.SetActive(true);
        _btnLikeNoNumberContainer.gameObject.SetActive(false);
        InitLikeNumber();
    }

    /// <summary>
    ///     初始化无数字控件
    /// </summary>
    private void InitComponentNoNumber()
    {
        _btnLikeWithNumberContainer.gameObject.SetActive(false);
        _btnLikeNoNumberContainer.gameObject.SetActive(true);
        InitLikeNumber();
    }


    private void InitLikeNumber() {
        if (_hasNumber)
        {
            _textInContainer.text = GetLikeStr();
        }
        else {
            
        }

    }


    private string GetLikeStr() {
        string result;

        if (_likes > 99)
            result = "99+";
        else {
            result = _likes.ToString();
        }

        return result;
    }


    /// <summary>
    ///     点击事件
    /// </summary>
    public void DoClick() {
        if (_hasNumber)
        {
            _likes = _likes + 1;

            //  喜欢数改变
            _textInContainer.DOText(GetLikeStr(), 1);

            //  图标变红
            _btnLikeWithNumberHeartIcon.DOColor(new Color(23,0,18), 1);

            _onClickCallBack.Invoke();

            Debug.Log("Click Button Like Agent");
        }
        else {

        }
    }


}
