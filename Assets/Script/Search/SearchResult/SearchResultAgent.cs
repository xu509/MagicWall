﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class SearchResultAgent : MonoBehaviour
{
    Action _onClickMove;
    Action _onClickReturn;
    Action<SearchBean> _onClickSearchResultItem;

    [SerializeField] Text _title;   //  标题
    [SerializeField] RectTransform _ScrollViewItemContainer;    //  列表内容容器
    [SerializeField] SearchResultItemAgent _searchResultItemAgentPrefab;    //  搜索 item 代理

    private ItemsFactory _itemsFactory;   //  实体生成器
    private MagicWallManager _manager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 初始化搜索结果
    /// </summary>
    public void Init() {
        // 初始化移动、回退、帮助按钮
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData(List<SearchBean> searchBeans,string title,MagicWallManager manager) {
        _title.text = title;
        _manager = manager;

        // 根据 search beans 进行初始化内容
        for (int i = 0; i < searchBeans.Count; i++) {
            CreateItem(searchBeans[i]);
        }
    }

    #region 事件

    /// <summary>
    /// 新建Item
    /// </summary>
    /// <param name="searchBean"></param>
    private void CreateItem(SearchBean searchBean) {
        SearchResultItemAgent searchResultItemAgent = Instantiate(_searchResultItemAgentPrefab, _ScrollViewItemContainer)
            as SearchResultItemAgent;
        searchResultItemAgent.Init();
        searchResultItemAgent.InitData(searchBean,_manager);
        searchResultItemAgent.SetOnClickItem(OnClickResultItem);
    }

    public void DoMove() {
        _onClickMove.Invoke();
    }

    public void DoReturn() {
        _onClickReturn.Invoke();
    }

    /// <summary>
    /// 点击搜索结果项
    /// </summary>
    /// <param name="searchBean"></param>
    private void OnClickResultItem(SearchBean searchBean) {
        _onClickSearchResultItem.Invoke(searchBean);
    }

    #endregion


    #region 事件代理装载
    public void SetOnClickMoveBtn(Action action) {
        _onClickMove = action;
    }

    public void SetOnClickReturnBtn(Action action) {
        _onClickReturn = action;
    }

    public void SetOnClickSearchResultItem(Action<SearchBean> action) {
        _onClickSearchResultItem = action;
    }


    #endregion
}