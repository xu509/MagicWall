using System;
using System.Collections.Generic;
using UnityEngine;
using EasingCore;


public class SubScrollController : SubScrollBaseController<CrossCardCellData, CrossCardScrollViewContext>
{
    IList<CrossCardCellData> _items;

    [SerializeField] SubScroller scroller = default;
    [SerializeField] GameObject cellPrefab = default;

    Action<int> onSelectionChanged;

    protected override GameObject CellPrefab => cellPrefab;

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

        Debug.Log("Sub Scroll Controller 初始化数据 INDEX : " + index);

        //FancyScrollViewCell<CrossCardCellData, CrossCardScrollViewContext> viewCell = GetCell(index);
        CrossCardCellData cellData = _items[index];
        IList<CrossCardCellData> cellDatas =  CardItemFactoryInstance.Instance.Generate(cellData.Id, cellData.Category);

        // 初始化celldatas

        SubScrollBaseCell<CrossCardCellData, CrossCardScrollViewContext> baseCell = GetCell(index);
        baseCell.GetComponent<RectTransform>().SetAsLastSibling();





    }



}