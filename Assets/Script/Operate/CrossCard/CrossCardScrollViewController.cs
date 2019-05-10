using System;
using System.Collections.Generic;
using UnityEngine;
using EasingCore;


public class CrossCardScrollViewController : CrossCardBaseController<CrossCardCellData, CrossCardScrollViewContext>
{
    IList<CrossCardCellData> _items;
    int _envId; // env id;

    [SerializeField] Scroller scroller = default;
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


        CrossCardBaseCell<CrossCardCellData, CrossCardScrollViewContext> baseCell = GetCell(index);
        baseCell.GetComponent<RectTransform>().SetAsLastSibling();

        baseCell.UpdateBtnLikeStatus();
    }

    public void UpdateItemData(CrossCardAgent agent)
    {
        // 此时数据传递
        _cardAgent = agent;

    }

    public void UpdateData(IList<CrossCardCellData> items)
    {
        // 此时数据传递
        _items = items;
        UpdateContents(items);
        scroller.SetTotalCount(items.Count);
    }

    //
    //  设置 Env Id
    //
    public void UpdateEnvId(int env_id) {
        this._envId = env_id;
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