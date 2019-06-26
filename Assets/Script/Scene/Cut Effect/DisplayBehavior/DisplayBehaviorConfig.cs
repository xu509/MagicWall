using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//
//	Display 配置
//
public class DisplayBehaviorConfig
{
    private MagicWallManager _manager;
    private int _row;
    private int _column;    // 最右侧的列数
    private float _itemWidth;
    private float _itemHeight;
    private SceneContentType _sceneContentType;
    private Dictionary<int, List<FlockAgent>> _agentsOfPages = new Dictionary<int, List<FlockAgent>>();
    private int _page = 0;
    private ItemsFactory _itemsFactory;
    private float _displayTime;
    private SceneUtils _sceneUtils;

    /// <summary>
    /// 根据行数的数据字典
    /// </summary>
    private Dictionary<int, ItemPositionInfoBean> _rowAgentsDic = new Dictionary<int, ItemPositionInfoBean>();
    /// <summary>
    /// 根据行数的数据字典
    /// </summary>
    public Dictionary<int, ItemPositionInfoBean> rowAgentsDic { set { _rowAgentsDic = value; } get { return _rowAgentsDic; } }

    /// <summary>
    /// 根据列数的数据字典
    /// </summary>
    private Dictionary<int, ItemPositionInfoBean> _columnAgentsDic = new Dictionary<int, ItemPositionInfoBean>();
    /// <summary>
    /// 根据列数的数据字典
    /// </summary>
    public Dictionary<int, ItemPositionInfoBean> columnAgentsDic { set { _columnAgentsDic = value; } get { return _columnAgentsDic; } }


    public ItemsFactory ItemsFactory
    {
        set { _itemsFactory = value; }
        get { return _itemsFactory; }
    }

    public MagicWallManager Manager
    {
        set { _manager = value; }
        get { return _manager; }
    }

    public int Row {
        set { _row = value; }
        get { return _row; }
    }

    /// <summary>
    /// 最右侧的列数
    /// </summary>
    public int Column
    {
        set { _column = value; }
        get { return _column; }
    }

    public float ItemWidth
    {
        set { _itemWidth = value; }
        get { return _itemWidth; }
    }

    public float ItemHeight
    {
        set { _itemHeight = value; }
        get { return _itemHeight; }
    }

    public SceneContentType SceneContentType
    {
        set { _sceneContentType = value; }
        get { return _sceneContentType; }
    }

    public int Page
    {
        set { _page = value; }
        get { return _page; }
    }

    public Dictionary<int, List<FlockAgent>> AgentsOfPages
    {
        set { _agentsOfPages = value; }
        get { return _agentsOfPages; }
    }

    public float DisplayTime
    {
        set { _displayTime = value; }
        get { return _displayTime; }
    }

    public SceneUtils sceneUtils
    {
        set { _sceneUtils = value; }
        get { return _sceneUtils; }
    }







    //
    //  _agentsOfPages
    //
    public void AddFlockAgentToAgentsOfPages(int page, FlockAgent agent) {

        if (_agentsOfPages.ContainsKey(page))
        {
            _agentsOfPages[page].Add(agent);
        }
        else
        {
            List<FlockAgent> flockAgents = new List<FlockAgent>();
            flockAgents.Add(agent);
            _agentsOfPages.Add(page, flockAgents);
        }
    }



}
