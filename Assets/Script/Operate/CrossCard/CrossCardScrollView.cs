using System;
using System.Collections.Generic;
using UnityEngine;
using EasingCore;


public class CrossCardScrollView : FancyScrollView1<CrossCardCellData, CrossCardScrollViewContext>
{

    [SerializeField] Scroller scroller = default;
    [SerializeField] GameObject cellPrefab = default;
    [SerializeField] GameObject cellItemPrefab = default;


    Action<int> onSelectionChanged;

    protected override GameObject CellPrefab => cellPrefab;

    protected override GameObject CellItemPrefab => cellItemPrefab;

    void Awake()
    {
        Context.OnCellClicked = SelectCell;
    }

    void Start()
    {
        Debug.Log("Cross Card Scroll View Is Start !");
        scroller.OnValueChanged(UpdatePosition);
        scroller.OnSelectionChanged(UpdateSelection);
    }

    void UpdateSelection(int index)
    {
        if (Context.SelectedIndex == index)
        {
            return;
        }

        Context.SelectedIndex = index;
        Refresh();

        onSelectionChanged?.Invoke(index);
    }

    public void UpdateData(IList<CrossCardCellData> items)
    {
        UpdateContents(items);
        scroller.SetTotalCount(items.Count);
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

}