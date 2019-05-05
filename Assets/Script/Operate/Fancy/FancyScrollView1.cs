using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class FancyScrollView1<TItemData, TContext> : MonoBehaviour where TContext : class, new()
{
    [SerializeField, Range(float.Epsilon, 1f)] protected float cellSpacing = 0.2f;
    [SerializeField, Range(0f, 1f)] protected float scrollOffset = 0.5f;
    [SerializeField] protected bool loop = false;
    [SerializeField] protected Transform cellContainer = default;

    readonly IList<FancyScrollViewCell<TItemData, TContext>> pool =
        new List<FancyScrollViewCell<TItemData, TContext>>();
    readonly IList<FancyScrollViewCell<TItemData, TContext>> pool2 =
    new List<FancyScrollViewCell<TItemData, TContext>>();


    float currentPosition;

    protected abstract GameObject CellPrefab { get; }

    protected abstract GameObject CellItemPrefab { get; }

    protected IList<TItemData> ItemsSource { get; set; } = new List<TItemData>();
    protected IList<TItemData> ItemsSource2 { get; set; } = new List<TItemData>();
    protected TContext Context { get; } = new TContext();

    /// <summary>
    /// Updates the contents.
    /// </summary>
    /// <param name="itemsSource">Items source.</param>
    protected void UpdateContents(IList<TItemData> itemsSource)
    {
        ItemsSource = itemsSource;
        Refresh();
    }


    /// <summary>
    /// Refreshes the cells.
    /// </summary>
    protected void Refresh() => UpdatePosition(currentPosition, false);


    /// <summary>
    /// Updates the scroll position.
    /// </summary>
    /// <param name="position">Position.</param>
    protected void UpdatePosition(float position,bool directionIsHor) => UpdatePosition(position, directionIsHor,false);


    void UpdatePosition(float position,bool directionIsHor, bool forceRefresh)
    {
        currentPosition = position;

        var p = position - scrollOffset / cellSpacing;
        var firstIndex = Mathf.CeilToInt(p);
        var firstPosition = (Mathf.Ceil(p) - p) * cellSpacing;


        if (directionIsHor)
        {
            if (firstPosition + pool2.Count * cellSpacing < 1f)
            {
                ResizePool(firstPosition, directionIsHor);
            }
        }
        else {
            if (firstPosition + pool.Count * cellSpacing < 1f)
            {
                ResizePool(firstPosition, directionIsHor);
            }
        }

        UpdateCells(firstPosition, firstIndex, forceRefresh,directionIsHor);
    }


    void ResizePool(float firstPosition,bool directionIsHor)
    {
        if (CellPrefab == null)
        {
            throw new NullReferenceException(nameof(CellPrefab));
        }

        if (CellItemPrefab == null) {
            throw new NullReferenceException(nameof(CellPrefab));
        }

        if (cellContainer == null)
        {
            throw new MissingComponentException(nameof(cellContainer));
        }

        Debug.Log("directionIsHor : " + directionIsHor);

        if (directionIsHor)
        {
            var addCount = Mathf.CeilToInt((1f - firstPosition) / cellSpacing) - pool2.Count;
            for (var i = 0; i < addCount; i++)
            {
                Transform[] cellContainer2s = CellPrefab.GetComponentsInChildren<Transform>();
                Transform t = null;
                foreach (Transform tt in cellContainer2s) {
                    if (tt.name == "Context") { 
                        t = tt;
                        break;
                    }
                }
                Debug.Log("T == NULL" + t == null);

                if (t == null) {
                    throw new NullReferenceException(nameof(t));
                }

                var cell = Instantiate(CellItemPrefab, t).GetComponent<FancyScrollViewCell<TItemData, TContext>>();
                if (cell == null)
                {
                    throw new MissingComponentException(
                        $"FancyScrollViewCell<{typeof(TItemData).FullName}, {typeof(TContext).FullName}> " +
                        $"component not found in {CellPrefab.name}.");
                }
                cell.SetupContext(Context);
                cell.SetVisible(false);
                pool2.Add(cell);
            }
        }
        else {
            var addCount = Mathf.CeilToInt((1f - firstPosition) / cellSpacing) - pool.Count;
            for (var i = 0; i < addCount; i++)
            {
                var cell = Instantiate(CellPrefab, cellContainer).GetComponent<FancyScrollViewCell<TItemData, TContext>>();
                if (cell == null)
                {
                    throw new MissingComponentException(
                        $"FancyScrollViewCell<{typeof(TItemData).FullName}, {typeof(TContext).FullName}> " +
                        $"component not found in {CellPrefab.name}.");
                }
                cell.SetupContext(Context);
                cell.SetVisible(false);
                pool.Add(cell);
            }
        }
    }

    void UpdateCells(float firstPosition, int firstIndex, bool forceRefresh, bool directionIsHor)
    {
        if (directionIsHor)
        {
            for (var i = 0; i < pool2.Count; i++)
            {
                var index = firstIndex + i;
                var position = firstPosition + i * cellSpacing;
                var cell = pool2[CircularIndex(index, pool.Count)];

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
        else {
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

    }

    int CircularIndex(int i, int size) => size < 1 ? 0 : i < 0 ? size - 1 + (i + 1) % size : i % size;

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

public abstract class FancyScrollView1<TItemData> : FancyScrollView1<TItemData, FancyScrollViewNullContext>
{
}