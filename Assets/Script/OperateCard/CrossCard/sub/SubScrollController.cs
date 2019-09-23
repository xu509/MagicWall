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
        // 获取资料，更新


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
        List<int> lis = new List<int>();
        int d = 0;

        for (int i = 0; i < Pool.Count; i++) {
            int Index = GetCell(i).Index;

            GetCell(i).SetupContext(Context);

            if (Index == CurrentIndex)
            {
                GetCell(i).UpdateComponentStatus();
            }
            else {
                GetCell(i).ClearComponentStatus();
            }

            // 调整一次位置
            int dis = Mathf.Abs(Index - CurrentIndex);
            lis.Add(dis);
        }

        // 略微调整下位置
        lis.Sort((a,b) => b.CompareTo(a));

        if (Pool.Count > 5) {
            GetCell(lis[1]).SetAsLastPosition();
            GetCell(lis[0]).SetAsLastPosition();
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
            GetCell(i).ClearComponentStatus();
        }
    }

    public string GetCurrentDescription() {
 
        string str = "";

        str = GetCell(_currentIndex)?.GetData().Description;
        return str;
    }
}