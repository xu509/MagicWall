using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SearchResultAgent : MonoBehaviour
{
    [SerializeField] Text _title;   //  标题
    [SerializeField] RectTransform _ScrollViewItemContainer;    //  列表内容容器
    [SerializeField] SearchResultItemAgent _searchResultItemAgentPrefab;    //  搜索 item 代理

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
    public void InitData(List<SearchBean> searchBeans,string title) {
        _title.text = title;

        // 根据 search beans 进行初始化内容
        for (int i = 0; i < searchBeans.Count; i++) {
            CreateItem(searchBeans[i]);
        }
    }

    /// <summary>
    /// 新建Item
    /// </summary>
    /// <param name="searchBean"></param>
    private void CreateItem(SearchBean searchBean) {
        SearchResultItemAgent searchResultItemAgent = Instantiate(_searchResultItemAgentPrefab, _ScrollViewItemContainer)
            as SearchResultItemAgent;
        searchResultItemAgent.Init();
        searchResultItemAgent.InitData(searchBean);
    }


}
