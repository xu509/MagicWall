using System;
using System.Collections.Generic;
using UnityEngine;
using EasingCore;


public class SubScrollController : SubScrollBaseController<CrossCardCellData, CrossCardScrollViewContext>
{
    CrossCardScrollViewCell _crossCardScrollViewCell;
    public CrossCardScrollViewCell crossCardScrollViewCell { set { _crossCardScrollViewCell = value; } get { return _crossCardScrollViewCell; } }

    int _currentIndex;
    public int CurrentIndex { set { _currentIndex = value; } get { return _currentIndex; } }

    IList<CrossCardCellData> _items;

    [SerializeField] SubScroller scroller = default;
    [SerializeField] GameObject cellPrefab = default;

    Action<int> onSelectionChanged;

    protected override GameObject CellPrefab => cellPrefab;

    void Awake()
    {
        Context.OnCellClicked = SelectCell;
        Context.OnScaleClicked = ScaleCell;
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

        CurrentIndex = index;
        Context.SelectedIndex = index;
        Refresh();
        onSelectionChanged?.Invoke(index);

        UpdateComponents();
    }

    public void UpdateData(IList<CrossCardCellData> items)
    {
        _items = items;
        UpdateContents(items);
        scroller.SetTotalCount(items.Count);

        // 此时第一次更新数据
        UpdateComponents();

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

    public void ScaleCell(Texture texture)
    {
        Debug.Log("ScaleCell");
        //_crossCardScrollViewCell

    }

    public void OnSelectionChanged(Action<int> callback)
    {
        onSelectionChanged = callback;
    }

    protected override void UpdateComponents()
    {
        for (int i = 0; i < Pool.Count; i++) {
            int Index = Pool[i].Index;

            if (Index == CurrentIndex)
            {
                Pool[i].UpdateComponentStatus();
            }
            else {
                Pool[i].ClearComponentStatus();
            }
        }
    }

    public override void UpdateAllComponents()
    {
        UpdateComponents();
    }

    public override void ClearAllComponents()
    {
        for (int i = 0; i < Pool.Count; i++)
        {
            Pool[i].ClearComponentStatus();
        }
    }
}