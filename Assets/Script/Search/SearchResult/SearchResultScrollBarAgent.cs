using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchResultScrollBarAgent : MonoBehaviour
{

    [SerializeField] private int _itemNumber = 50;   //  Item 的数量
    [SerializeField] private RectTransform _container; //   item 的容器
    [SerializeField] private SearchResultScrollBarItemAgent _itemPrefab;


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

        float midindex;

        // 将 position 修正为只有一位小数点的浮点
        position = Mathf.Floor(position * 10) / 10;

        float k = (1f - position) / 0.1f;
        if (position == 0.8f) {
            Debug.Log("##key：##" + k);
        }

        midindex = k * (_itemNumber / 10);

        // 四舍五入后即可获得中间点 Mathf.RoundToInt(midindex)

        // 中点向上向下分别进行调整一定数目

        // 即将内容进行20等分，分别对应一位小数点的 position 内容

        foreach (var item in _items)
        {
            item.Refresh(position);
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
        searchResultScrollBarItemAgent.Init(index);

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


}
