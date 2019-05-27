﻿using System;
using System.Collections.Generic;
using UnityEngine;
using EasingCore;


public class SliceCardScrollViewController : SliceCardBaseController<SliceCardCellData, SliceCardCellContext>
{
    IList<SliceCardCellData> _items;
    int _envId; // env id;

    int _currentIndex;
    public int CurrentIndex { set { _currentIndex = value; } get { return _currentIndex; } }

    [SerializeField] SliceScroller scroller = default;
    [SerializeField] GameObject cellPrefab = default;

    Action<int> onSelectionChanged;

    protected override GameObject CellPrefab => cellPrefab;

    void Awake()
    {
        Context.OnCellClicked = SelectCell;
        Context.OnScaleClicked = DoScale;
        Context.OnDescriptionChanged = UpdateDescription;
    }

    void Start()
    {
        scroller.OnValueChanged(UpdatePosition);
        scroller.OnSelectionChanged(UpdateSelection);
    }

    void UpdateSelection(int index)
    {
        if (Context.SelectedIndex == index)
        {
            return;
        }
        _currentIndex = index;
        Context.SelectedIndex = index;
        Refresh();
        onSelectionChanged?.Invoke(index);

        UpdateComponents();

    }

    public void UpdateItemData(SliceCardAgent agent)
    {
        // 此时数据传递
        _cardAgent = agent;

    }

    public void UpdateData(IList<SliceCardCellData> items)
    {
        // 此时数据传递
        _items = items;
        UpdateContents(items);
        scroller.SetTotalCount(items.Count);

        UpdateComponents();
    }

    //
    //  设置 Env Id
    //
    public void UpdateEnvId(int env_id) {
        this._envId = env_id;
    }


    public void DoScale(Texture texture)
    {

        _cardAgent.InitScaleAgent(texture);       

    }



    public void SelectCell(int index)
    {
        if (index < 0 || index >= ItemsSource.Count || index == Context.SelectedIndex)
        {
            return;
        }

        UpdateSelection(index);
        scroller.ScrollTo(index, 0.35f, Ease.OutCubic);

    }

    public void OnSelectionChanged(Action<int> callback)
    {
        onSelectionChanged = callback;
    }


    protected override void UpdateComponents()
    {
        for (int i = 0; i < Pool.Count; i++)
        {
            int Index = Pool[i].Index;

            if (Index == CurrentIndex)
            {
                Pool[i].UpdateComponentStatus();
            }
            else
            {
                Pool[i].ClearComponentStatus();
            }
        }

    }


    // 获取当前显示卡片的描述
    public string GetCurrentCardDescription() {
        string str = Pool[_currentIndex].GetCurrentDescription();
        return str;
    }

    public void UpdateDescription(string description) {
        //_cardAgent.UpdateDescription(description);

        Debug.Log("Update Description : " + description);

    }



}