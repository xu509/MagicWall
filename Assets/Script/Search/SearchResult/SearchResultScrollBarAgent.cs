using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EasingUtil;


public class SearchResultScrollBarAgent : MonoBehaviour
{

    [SerializeField] private int _itemNumber = 50;   //  Item 的数量
    [SerializeField] private RectTransform _container; //   item 的容器
    [SerializeField] private SearchResultScrollBarItemAgent _itemPrefab;

    [SerializeField] private float _minItemWidth = 20f;
    [SerializeField] private float _maxItemWidth = 50f;

    [SerializeField] private EaseEnum _influenceEaseEnum;


    private List<SearchResultScrollBarItemAgent> _items;

    void Awake()
    {
        _items = new List<SearchResultScrollBarItemAgent>();    
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 初始化入口
    /// </summary>
    public void Init() {
        _items = new List<SearchResultScrollBarItemAgent>();

        CreateItems();

    }


    /// <summary>
    /// 刷新 滚动条信息
    /// </summary>
    /// <param name="position">滚动区域的位置 1 -> 0</param>
    public void Refresh(float position) {

        // position 从上往下即是从 1 -> 0

        // 当总共 50 根的话，会影响到 15 根

        // 找到 position 对应的索引中点

        // position = 1 | midpoint = 0
        // position = 0.9 | midpoint = _total / 10 
        // position = 0.8 | midpoint = _total / 10 * 2
        // ...
        // position = 0 | midpoint = _total

        float midindexf;
        
        // 将 position 修正为只有一位小数点的浮点
        position = Mathf.Floor(position * 10) / 10;

        float k = (1f - position) / 0.1f;

        midindexf = k * (_itemNumber / 10);

        // 四舍五入后即可获得中间点 Mathf.RoundToInt(midindex)
        int midindex = Mathf.RoundToInt(midindexf);

        // 获取影响的范围
        int effectRange = GetEffectRange();

        // 中点向上向下 effectRange 分别进行调整一定数目
        // 最短宽度20，最长宽度50
        foreach (var item in _items)
        {
            float widthOffset = CalculateItemWidthOffset(effectRange, midindex, item.Index);
            item.Refresh(widthOffset);
        }
    }


    /// <summary>
    /// 生成所有的缩放条
    /// </summary>
    private void CreateItems() {

        for (int i = 0; i < _itemNumber; i++) {
            CreateItem(i);
        }

    }

    /// <summary>
    /// 生成单个缩放条
    /// </summary>
    /// <param name="index">索引</param>
    private void CreateItem(int index) {
        //Debug.Log("Create Item : " + index);

        SearchResultScrollBarItemAgent searchResultScrollBarItemAgent
            = Instantiate(_itemPrefab, _container);

        // 设置位置
        float y_position = 0 - index * GetGapDistance();
        searchResultScrollBarItemAgent.GetComponent<RectTransform>()
            .anchoredPosition = new Vector2(0,y_position);

        // 初始化 ScrollBarItem 数据
        searchResultScrollBarItemAgent.Init(index,_minItemWidth);

        _items.Add(searchResultScrollBarItemAgent);

    }

    /// <summary>
    ///  获取间隔距离
    /// </summary>
    /// <returns></returns>
    private float GetGapDistance() {

        float height = _container.sizeDelta.y;

        float result = (height - 5) / _itemNumber;

        return result;
    }

    /// <summary>
    /// 获取受影响的范围
    /// </summary>
    /// <returns></returns>
    private int GetEffectRange() {

        float rangef = 7f / 50f * _itemNumber;

        return Mathf.RoundToInt(rangef);
    }


    /// <summary>
    ///     计算宽度的偏移量
    /// </summary>
    /// <param name="effectRange">影响范围</param>
    /// <param name="midIndex">中间索引</param>
    /// <param name="index">item 索引</param>
    /// <returns></returns>
    private float CalculateItemWidthOffset(int effectRange , int midIndex, int index) {

        // 获取索引与 midindex的差值
        int offset = Mathf.Abs(index - midIndex);

        // 判断 Item 是否在影响范围内
        if (offset > effectRange)
        {
            //  在影响范围外，则设置为最小宽度
            return 0;
        }
        else {
            //  根据插值计算出宽度
            // offset : 0 ; width : max_width
            // offset : 1 ; width : ..
            // ...
            // offset :effectRange : width : min_width

            float unitOffset = (_maxItemWidth - _minItemWidth) / effectRange;
            float result = ((effectRange - offset) * unitOffset) / 2;

            return result;
        }
    }

    //private float CalculateItemAnchorX(int effectRange, int midIndex, int index)
    //{

    //    // 获取索引与 midindex的差值
    //    int offset = Mathf.Abs(index - midIndex);

    //    // 判断 Item 是否在影响范围内
    //    if (offset > effectRange)
    //    {
    //        //  在影响范围外，则设置为最小宽度
    //        return 0f;
    //    }
    //    else
    //    {
    //        //  根据插值计算出宽度
    //        // offset : 0 ; width : max_width
    //        // offset : 1 ; width : ..
    //        // ...
    //        // offset :effectRange : width : min_width

    //        float unitOffset = (_maxItemWidth - _minItemWidth) / effectRange;
    //        float result = ((effectRange - offset) * unitOffset) / 2;

    //        //Func<float, float> defaultEasingFunction = EasingFunction.Get(_influenceEaseEnum);
    //        //float k = defaultEasingFunction(result / (_maxItemWidth / effectRange));

    //        return 0 - result / 2;
    //    }
    //}


}
