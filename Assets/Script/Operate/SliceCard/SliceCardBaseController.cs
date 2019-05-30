using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class SliceCardBaseController<SliceCardCellData, SliceCardCellContext> : MonoBehaviour where SliceCardCellContext : class, new()
{

    protected SliceCardAgent _cardAgent;  // 关联的 card agent;

    public void SetUpCardAgent(SliceCardAgent cardAgent) {
        _cardAgent = cardAgent;
    }


    private SliceCardBaseCell<SliceCardCellData, SliceCardCellContext> currentCell;

    [SerializeField, Range(float.Epsilon, 1f)] protected float cellSpacing = 0.2f;
    [SerializeField, Range(0f, 1f)] protected float scrollOffset = 0.5f;
    [SerializeField] protected bool loop = false;
    [SerializeField] protected Transform cellContainer = default;

    readonly IList<SliceCardBaseCell<SliceCardCellData, SliceCardCellContext>> pool =
        new List<SliceCardBaseCell<SliceCardCellData, SliceCardCellContext>>();


    public SliceCardBaseCell<SliceCardCellData, SliceCardCellContext> GetCell(int index) {
        return pool[index];
    }

    public IList<SliceCardBaseCell<SliceCardCellData, SliceCardCellContext>> Pool
    {
        get {
            return pool;
        }
    }


    float currentPosition;

    protected abstract GameObject CellPrefab { get; }

    protected IList<SliceCardCellData> ItemsSource { get; set; } = new List<SliceCardCellData>();
    protected SliceCardCellContext Context { get; } = new SliceCardCellContext();

    /// <summary>
    /// Updates the contents.
    /// </summary>
    /// <param name="itemsSource">Items source.</param>
    protected void UpdateContents(IList<SliceCardCellData> itemsSource)
    {
        ItemsSource = itemsSource;
        Refresh();
    }


    protected abstract void UpdateComponents();


    /// <summary>
    /// Refreshes the cells.
    /// </summary>
    protected void Refresh() => UpdatePosition(currentPosition, false);


    /// <summary>
    /// Updates the scroll position.
    /// </summary>
    /// <param name="position">Position.</param>
    protected void UpdatePosition(float position) => UpdatePosition(position,false);


    void UpdatePosition(float position, bool forceRefresh)
    {
        currentPosition = position;

        var p = position - scrollOffset / cellSpacing;
        var firstIndex = Mathf.CeilToInt(p);
        var firstPosition = (Mathf.Ceil(p) - p) * cellSpacing;

        if (firstPosition + pool.Count * cellSpacing < 1f)
        {
            ResizePool(firstPosition);
        }

        UpdateCells(firstPosition, firstIndex, forceRefresh);
    }


    void ResizePool(float firstPosition)
    {
        if (CellPrefab == null)
        {
            throw new NullReferenceException(nameof(CellPrefab));
        }
       
        if (cellContainer == null)
        {
            throw new MissingComponentException(nameof(cellContainer));
        }
            
        var addCount = Mathf.CeilToInt((1f - firstPosition) / cellSpacing) - pool.Count;
        for (var i = 0; i < addCount; i++)
        {
            var cell = Instantiate(CellPrefab, cellContainer).GetComponent<SliceCardBaseCell<SliceCardCellData, SliceCardCellContext>>();
            if (cell == null)
            {
                throw new MissingComponentException(
                    $"FancyScrollViewCell<{typeof(SliceCardCellData).FullName}, {typeof(SliceCardCellContext).FullName}> " +
                    $"component not found in {CellPrefab.name}.");
            }
            cell.Index = i;
            cell.SetupContext(Context);
            cell.SetVisible(false);
            cell.InitData();
            pool.Add(cell);
        }
        
    }

    void UpdateCells(float firstPosition, int firstIndex, bool forceRefresh)
    {
        for (var i = 0; i < pool.Count; i++)
        {
            var index = firstIndex + i;
            var position = firstPosition + i * cellSpacing;
            var cell = pool[CircularIndex(index, pool.Count)];

            if (loop)
            {
                index = CircularIndex(index, ItemsSource.Count);
            }

            if (index < 0 || index >= ItemsSource.Count || position > 1f)
            {
                cell.SetVisible(false);
                continue;
            }

            if (forceRefresh || cell.Index != index || !cell.IsVisible)
            {
                cell.Index = index;
                cell.SetVisible(true);
                cell.UpdateContent(ItemsSource[index]);
            }

            cell.UpdatePosition(position);
        }
        
    }

    int CircularIndex(int i, int size) {
        if (size < 1)
        {
            return 0;
        }
        else
        {
            if (i < 0)
            {
                return size - 1 + (i + 1) % size;
            }
            else {
                return i % size;
            }
        }
        //size < 1 ? 0 : i < 0 ? size - 1 + (i + 1) % size : i % size;
    } 


#if UNITY_EDITOR
    bool cachedLoop;
    float cachedCellSpacing, cachedScrollOffset;

    void LateUpdate()
    {
        if (cachedLoop != loop || cachedCellSpacing != cellSpacing || cachedScrollOffset != scrollOffset)
        {
            cachedLoop = loop;
            cachedCellSpacing = cellSpacing;
            cachedScrollOffset = scrollOffset;

            UpdatePosition(currentPosition,false);
        }
    }
#endif
}

public sealed class FancyScrollViewNullContext3
{
}

public abstract class SliceCardBaseController<SliceCardCellData> : CrossCardBaseController<SliceCardCellData, FancyScrollViewNullContext>
{
}