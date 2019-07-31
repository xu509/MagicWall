using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class SearchResultAgent : MonoBehaviour
{
    Action _onClickMove;
    Action _onClickReturn;
    Action<SearchBean> _onClickSearchResultItem;

    [SerializeField] Text _title;   //  标题
    [SerializeField] RectTransform _ScrollViewItemContainer;    //  列表内容容器
    [SerializeField] SearchResultItemAgent _searchResultItemAgentPrefab;    //  搜索 item 代理
    [SerializeField] SearchResultScrollBarAgent _searchResultScrollBarAgent; // 滚动条代理
    [SerializeField] RectTransform _noResultContainer;

    [SerializeField, Header("Move")] Sprite _sprite_move_active;
    [SerializeField] Sprite _sprite_move;
    [SerializeField] RectTransform _move_rect;
    [SerializeField] MoveButtonComponent _moveBtnComponent;


    private List<SearchResultItemAgent> _resultItems;   //结果 items
    private ItemsFactory _itemsFactory;   //  实体生成器
    private MagicWallManager _manager;
    private CardAgent _cardAgent;

    private float _default_scrollview_height;
    private Vector2 _default_scrollview_anchorposition;

    private float _gap = 10f;
    private float _itemHeight;

    private bool _doMoving = false;


    void Awake()
    {
        Reset();

    }

    private void Reset() {


        _resultItems = new List<SearchResultItemAgent>();

        float defaultViewScrollHeight = _ScrollViewItemContainer.rect.height;

        // 设置grid layout

        float w = _ScrollViewItemContainer.rect.width;
        float h = _ScrollViewItemContainer.rect.height;

        GridLayoutGroup _gridLayoutGroup = _ScrollViewItemContainer.GetComponent<GridLayoutGroup>();
        float width = (w - _gap - 10) / 2;
        _itemHeight = (defaultViewScrollHeight - 2 * _gap) / 3;

        _gridLayoutGroup.cellSize = new Vector2(width, _itemHeight);

        // 设置默认的滑动结果栏高度
        _default_scrollview_height = 3 * (_itemHeight + _gap);
        _ScrollViewItemContainer.sizeDelta = new Vector2(_ScrollViewItemContainer.sizeDelta.x, _default_scrollview_height);
        _default_scrollview_anchorposition = _ScrollViewItemContainer.anchoredPosition;

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
    /// 初始化搜索结果
    /// </summary>
    public void Init() {
        // 初始化移动、回退、帮助按钮
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData(List<SearchBean> searchBeans,string title,MagicWallManager manager,CardAgent cardAgent) {
        Reset();

        _title.text = title;
        _manager = manager;
        _cardAgent = cardAgent;

        _moveBtnComponent.Init(DoMove, _cardAgent);

        int count = searchBeans.Count;

        if (count == 0)
        {
            _noResultContainer.gameObject.SetActive(true);
        }
        else {
            _noResultContainer.gameObject.SetActive(false);

            // 根据 search beans 进行初始化内容
            for (int i = 0; i < searchBeans.Count; i++)
            {
                CreateItem(searchBeans[i]);
            }

            // 获取高度
            SetContentSize();

            // 初始化滚动条
            _searchResultScrollBarAgent.Init();
        }

    }

    #region 事件

    /// <summary>
    /// 新建Item
    /// </summary>
    /// <param name="searchBean"></param>
    private void CreateItem(SearchBean searchBean) {
        SearchResultItemAgent searchResultItemAgent = Instantiate(_searchResultItemAgentPrefab, _ScrollViewItemContainer)
            as SearchResultItemAgent;
        searchResultItemAgent.Init();
        searchResultItemAgent.InitData(searchBean,_manager);
        searchResultItemAgent.SetOnClickItem(OnClickResultItem);

        _resultItems.Add(searchResultItemAgent);
    }

    private void SetContentSize() {
        if (_resultItems.Count > 6) { 
            // 此时动态高度
            float height = (_resultItems.Count / 2) * (_itemHeight + 10);

            //Debug.Log("Item Total : " + _resultItems.Count + " | Height : " + _itemHeight + " | Container Height : " + height);

            float height_offset = height - _default_scrollview_height;
            float anchor_y = _default_scrollview_anchorposition.y - height_offset;

            
            _ScrollViewItemContainer.sizeDelta = new Vector2(_ScrollViewItemContainer.sizeDelta.x, height -_default_scrollview_height + _itemHeight / 2);
            _ScrollViewItemContainer.anchoredPosition = new Vector2(_default_scrollview_anchorposition.x, anchor_y);
        }
    }


    public void DoMove() {

        if (!_doMoving)
        {
            _move_rect.GetComponent<Image>().sprite = _sprite_move_active;
        }
        else
        {
            _move_rect.GetComponent<Image>().sprite = _sprite_move;

        }

        _doMoving = !_doMoving;

        _onClickMove.Invoke();
    }

    public void DoReturn() {
        Debug.Log("DoReturn");
        _onClickReturn.Invoke();
    }

    public void DoSearchResultChanged(Vector2 position) {
        // Position : 1.0 -> 0.0
        float y = position.y;

        if (y < 0) { 
            y = 0f;
        }
        else if ( y > 1){
            y = 1f;
        }
        _searchResultScrollBarAgent.Refresh(y);
    }



    #endregion

    #region  搜索结果项委托
    /// <summary>
    /// 点击搜索结果项
    /// </summary>
    /// <param name="searchBean"></param>
    private void OnClickResultItem(SearchBean searchBean)
    {
        _onClickSearchResultItem.Invoke(searchBean);
    }


    #endregion


    #region 事件代理装载
    public void SetOnClickMoveBtn(Action action) {
        _onClickMove = action;
    }

    public void SetOnClickReturnBtn(Action action) {
        _onClickReturn = action;
    }

    public void SetOnClickSearchResultItem(Action<SearchBean> action) {
        _onClickSearchResultItem = action;
    }


    #endregion
}
