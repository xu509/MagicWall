using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 企业工厂
/// </summary>
public class ActivityFactory :MonoBehaviour, ItemsFactory
{
    private float _gap = 58;
    private float _itemWidth;   // Item Width
    private float _itemHeight;  // Item Height
    private int _column;    // 列数


    // Generate Panel
    private RectTransform _operationPanel;

    // Manager
    private MagicWallManager _manager;

    // Agent Manager
    private AgentManager _agentManager;

    // Data
    private DaoService _daoService;


    void Awake() {

    }

    public ActivityFactory() {


    }

    public void Init(MagicWallManager manager)
    {
        _manager = manager;
        _operationPanel = _manager.OperationPanel;
        _agentManager = _manager.agentManager;
        _daoService = _manager.daoService;

        _gap = _gap * manager.displayFactor;

        int _row = _manager.Row;

        int h = (int)_manager.mainPanel.rect.height;    // 高度
        int w = (int)_manager.mainPanel.rect.width; //宽度
        _itemHeight = 250f;
        _itemWidth = _itemHeight;

        _column = Mathf.CeilToInt(w / (_itemWidth + _gap));
    }


    #region 生成浮动块
    //
    //  生成env的浮动组件
    //  - 长宽相等
    //  - 生成在动画前
    //  - 生成在动画后
    //
    public FlockAgent Generate(float gen_x, float gen_y, float ori_x, float ori_y, int row, int column, float width, 
        float height, BaseData data, AgentContainerType agentContainerType)
    {
        width = (int)width;
        height = (int)height;

        Activity activity = data as Activity;

        //  创建 Agent
        FlockAgent newAgent = _agentManager.GetFlockAgent(agentContainerType);

        //  命名
        newAgent.name = "Agent(" + (row + 1) + "," + (column + 1) + ")";


        //  定面板位置
        Vector2 ori_position = new Vector2(ori_x, ori_y);

        //  初始化内容
        newAgent.Initialize(_manager,ori_position, new Vector2(gen_x, gen_y), row + 1, column + 1,
            width, height, activity.Ent_id, activity.Image, false, MWTypeEnum.Activity, agentContainerType);

        //  添加到组件袋
        _agentManager.Agents.Add(newAgent);

        return newAgent;
    }
    #endregion

    #region 生成滑动卡片
    public CardAgent GenerateCardAgent(Vector3 genPos, FlockAgent flockAgent, int dataId, bool isActive)
    {

        //  创建 Agent
        SliceCardAgent sliceCardAgent = _agentManager.GetSliceCardAgent();

        //  定义缩放
        Vector3 scaleVector3 = new Vector3(0.2f, 0.2f, 0.2f);

        //  初始化卡片基础数据
        sliceCardAgent.InitCardData(_manager, dataId, MWTypeEnum.Activity, genPos, scaleVector3, flockAgent);

        //  初始化数据
        sliceCardAgent.InitSliceCard();

        // 添加到effect agent
        _agentManager.AddEffectItem(sliceCardAgent);

        // 设置显示状态
        sliceCardAgent.gameObject.SetActive(isActive);

        return sliceCardAgent;
    }
    #endregion


    /// <summary>
    /// 获取生成的位置
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    public Vector2 GetOriginPosition(int row, int column)
    {
        //float x = j * (itemWidth + gap) + itemWidth / 2 + gap;
        //float y = i * (itemHeight + gap) + itemHeight / 2 + gap;

        int h = (int)_manager.mainPanel.rect.height;
        int w = (int)_manager.mainPanel.rect.width;

        float itemHeight = (h - _gap * 7) / _manager.Row;
        float itemWidth = itemHeight;


        float x = column * (itemWidth + _gap) + itemWidth / 2 + _gap;
        float y = row * (itemHeight + _gap) + itemHeight / 2 + _gap;

        _itemWidth = itemWidth;
        _itemHeight = itemHeight;

        return new Vector2(x, y);
    }

    public Vector2 GoUpGetOriginPosition(int row, int column)
    {
        //float x = j * (itemWidth + gap) + itemWidth / 2 + gap;
        //float y = i * (itemHeight + gap) + itemHeight / 2 + gap;

        int h = (int)_manager.mainPanel.rect.height;
        int w = (int)_manager.mainPanel.rect.width;

        float itemHeight = (h - _gap * 7) / _manager.Row;
        float itemWidth = itemHeight;


        float x = column * (itemWidth + _gap) + itemWidth / 2 + _gap;
        float y = h - (row * (itemHeight + _gap) + itemHeight / 2 + _gap);

        _itemWidth = itemWidth;
        _itemHeight = itemHeight;

        return new Vector2(x, y);
    }

    /// <summary>
    /// 获取固定的长度
    /// </summary>
    /// <returns></returns>
    public float GetItemWidth()
    {
        return _itemWidth;
    }

    /// <summary>
    /// 获取固定的高度
    /// </summary>
    /// <returns></returns>
    public float GetItemHeight()
    {
        return _itemHeight;
    }

    public int GetSceneColumn()
    {
        return _column;
    }

    public float GetSceneGap()
    {
        return _gap;
    }


}
