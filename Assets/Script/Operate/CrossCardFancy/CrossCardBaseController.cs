using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class CrossCardBaseController<CrossCardCellData, CrossCardScrollViewContext> : MonoBehaviour where CrossCardScrollViewContext : class, new()
{

    protected CrossCardAgent _cardAgent;  // 关联的 card agent;

    public void SetUpCardAgent(CrossCardAgent cardAgent) {
        _cardAgent = cardAgent;
    }


    private CrossCardBaseCell<CrossCardCellData, CrossCardScrollViewContext> currentCell;

    [SerializeField, Range(float.Epsilon, 1f)] protected float cellSpacing = 0.2f;
    [SerializeField, Range(0f, 1f)] protected float scrollOffset = 0.5f;
    [SerializeField] protected bool loop = false;
    [SerializeField] protected Transform cellContainer = default;

    readonly IList<CrossCardBaseCell<CrossCardCellData, CrossCardScrollViewContext>> pool =
        new List<CrossCardBaseCell<CrossCardCellData, CrossCardScrollViewContext>>();


    public CrossCardBaseCell<CrossCardCellData, CrossCardScrollViewContext> GetCell(int index) {

        if (index == pool.Count)
        {
            return pool[0];
        }

        return pool[index];
    }

    public IList<CrossCardBaseCell<CrossCardCellData, CrossCardScrollViewContext>> Pool
    {
        get {
            return pool;
        }
    }


    float currentPosition;

    protected abstract GameObject CellPrefab { get; }

    protected IList<CrossCardCellData> ItemsSource { get; set; } = new List<CrossCardCellData>();
    protected CrossCardScrollViewContext Context { get; } = new CrossCardScrollViewContext();

    /// <summary>
    /// Updates the contents.
    /// </summary>
    /// <param name="itemsSource">Items source.</param>
    protected void UpdateContents(IList<CrossCardCellData> itemsSource)
    {
        ItemsSource = itemsSource;
        Refresh();
    }


    public abstract void UpdateComponents();


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
            var cell = Instantiate(CellPrefab, cellContainer).GetComponent<CrossCardBaseCell<CrossCardCellData, CrossCardScrollViewContext>>();
            if (cell == null)
            {
                throw new MissingComponentException(
                    $"FancyScrollViewCell<{typeof(CrossCardCellData).FullName}, {typeof(CrossCardScrollViewContext).FullName}> " +
                    $"component not found in {CellPrefab.name}.");
            }
            cell.SetupContext(Context);
            cell.SetVisible(false);
            cell.InitData();
            pool.Add(cell);

            CrossCardBaseCell<CrossCardCellData,CrossCardScrollViewContext> cell2 = GetCell(i);

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

public sealed class FancyScrollViewNullContext1
{
}

public abstract class CrossCardBaseController<CrossCardCellData> : CrossCardBaseController<CrossCardCellData, FancyScrollViewNullContext>
{
}