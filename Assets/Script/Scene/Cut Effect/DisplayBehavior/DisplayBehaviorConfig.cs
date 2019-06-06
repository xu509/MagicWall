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
    private int _column;
    private float _itemWidth;
    private float _itemHeight;
    private Dictionary<int, float> _lastPositionXDic;
    private Dictionary<int, float> _lastPositionYDic;
    private SceneContentType _sceneContentType;
    private Dictionary<int, List<FlockAgent>> _agentsOfPages = new Dictionary<int, List<FlockAgent>>();
    private int _page = 0;
    private ItemsFactory _itemsFactory;
    private float _displayTime;

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

    public Dictionary<int, float>  LastPositionXDic
    {
        set { _lastPositionXDic = value; }
        get { return _lastPositionXDic; }
    }

    public Dictionary<int, float> LastPositionYDic
    {
        set { _lastPositionYDic = value; }
        get { return _lastPositionYDic; }
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
