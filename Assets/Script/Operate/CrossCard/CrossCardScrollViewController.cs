using System;
using System.Collections.Generic;
using UnityEngine;
using EasingCore;


public class CrossCardScrollViewController : CrossCardBaseController<CrossCardCellData, CrossCardScrollViewContext>
{
    IList<CrossCardCellData> _items;

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

        Debug.Log("UpdateSelection !");
        InitDetailData(index);

    }

    public void UpdateData(IList<CrossCardCellData> items)
    {
        _items = items;
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

    public void InitDetailData(int index) {

        Debug.Log("初始化数据 INDEX : " + index);

        //FancyScrollViewCell<CrossCardCellData, CrossCardScrollViewContext> viewCell = GetCell(index);
        CrossCardCellData cellData = _items[index];
        Debug.Log("Cell Data : " + cellData.Title);
        IList<CrossCardCellData> cellDatas =  CardItemFactoryInstance.Instance.Generate(cellData.Id, cellData.Category);

        // 初始化celldatas
        Debug.Log("Cell Datas : " + cellDatas.Count);

       


    }



}