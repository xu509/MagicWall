using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnvFactory : MonoBehaviour,ItemsFactory
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

    public EnvFactory() {
        _operationPanel = GameObject.Find("OperatePanel").GetComponent<RectTransform>();
        _manager = MagicWallManager.Instance;
        _agentManager = AgentManager.Instance;
        _daoService = DaoService.Instance;

        int _row = _manager.Row;

        int h = (int)_manager.mainPanel.rect.height;    // 高度
        int w = (int)_manager.mainPanel.rect.width; //宽度
        _itemHeight = (h - _gap * 7) / _row;
        _itemWidth = _itemHeight;

        _column = Mathf.CeilToInt(w / (_itemWidth + _gap));

    }


    //
    //  生成env的浮动组件
    //  - 长宽相等
    //  - 生成在动画前
    //  - 生成在动画后
    //
    public FlockAgent Generate(float gen_x, float gen_y, float ori_x, float ori_y, int row, int column, float width, float height)
    {
        //  创建 Agent
        FlockAgent newAgent = Instantiate(
                                    _manager.agentPrefab,
                                    _manager.mainPanel
                                    );
        //  命名
        newAgent.name = "Agent(" + (row + 1) + "," + (column + 1) + ")";

        //  获取rect引用
        RectTransform rectTransform = newAgent.GetComponent<RectTransform>();

        //  定出生位置
        Vector2 postion = new Vector2(gen_x, gen_y);
        rectTransform.anchoredPosition = postion;

        //  定面板位置
        Vector2 ori_position = new Vector2(ori_x, ori_y);
        newAgent.GenVector2 = postion;

        //  初始化内容
        newAgent.Initialize(ori_position, postion, row + 1, column + 1);
        newAgent.Width = width;
        newAgent.Height = height;

        // 调整agent的长与宽
        Vector2 sizeDelta = new Vector2(width, height);
        rectTransform.sizeDelta = sizeDelta;

        // 初始化 数据
        Enterprise env = _daoService.GetEnterprise();
        newAgent.DataId = env.Ent_id;
        newAgent.DataIsCustom = env.IsCustom;
        newAgent.DataImg = env.Logo;
        newAgent.DataType = 0;

        // 初始化显示图片
        //rectTransform.gameObject.GetComponentInChildren<RawImage>().texture = AppUtils.LoadPNG(MagicWallManager.URL_ASSET + "1.jpg");
        newAgent.GetLogo().GetComponentInChildren<RawImage>().texture = env.Texture;

        // 调整 collider
        BoxCollider2D boxCollider2D = newAgent.GetComponent<BoxCollider2D>();
        boxCollider2D.size = new Vector2(width, height);

        //  添加到组件袋
        _agentManager.Agents.Add(newAgent);

        return newAgent;
    }

    public Vector2 GetOriginPosition(int row, int column)
    {
        //float x = j * (itemWidth + gap) + itemWidth / 2 + gap;
        //float y = i * (itemHeight + gap) + itemHeight / 2 + gap;

        int h = (int)_manager.mainPanel.rect.height;
        int w = (int)_manager.mainPanel.rect.width;

        float itemHeight = (h - _gap * 7) / _manager.row;
        float itemWidth = itemHeight;


        float x = column * (itemWidth + _gap) + itemWidth / 2 + _gap;
        float y = row * (itemHeight + _gap) + itemHeight / 2 + _gap;

        _itemWidth = itemWidth;
        _itemHeight = itemHeight;

        return new Vector2(x, y);
    }

    public float GetItemWidth()
    {
        return _itemWidth;
    }

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
