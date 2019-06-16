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

        int _row = _manager.Row;

        int h = (int)_manager.mainPanel.rect.height;    // 高度
        int w = (int)_manager.mainPanel.rect.width; //宽度
        _itemHeight = (h - _gap * 7) / _row;
        _itemWidth = 300;

        _column = Mathf.CeilToInt(w / (_itemWidth + _gap));
    }


    #region 生成浮动块
    //
    //  生成env的浮动组件
    //  - 长宽相等
    //  - 生成在动画前
    //  - 生成在动画后
    //
    public FlockAgent Generate(float gen_x, float gen_y, float ori_x, float ori_y, int row, int column, float width, float height, BaseData data, Transform parent)
    {
        Activity activity = data as Activity;

        //  创建 Agent
        FlockAgent newAgent = Instantiate(
                                    _manager.flockAgent,
                                    parent
                                    );
        //  命名
        newAgent.name = "Agent(" + (row + 1) + "," + (column + 1) + ")";

        //  获取rect引用
        RectTransform rectTransform = newAgent.GetComponent<RectTransform>();

        ////  定出生位置
        Vector2 postion = new Vector2(gen_x, gen_y);
        rectTransform.anchoredPosition = postion;

        // 初始化 数据
        //Enterprise env = _daoService.GetEnterprise();

        //// 初始化显示图片
        ////rectTransform.gameObject.GetComponentInChildren<RawImage>().texture = AppUtils.LoadPNG(MagicWallManager.URL_ASSET + "1.jpg");
        ////newAgent.GetLogo().GetComponentInChildren<RawImage>().texture = env.TextureLogo;
        //newAgent.GetComponent<RawImage>().texture = env.TextureLogo;

        newAgent.GetComponent<RawImage>().texture = activity.TextureImage;
        // 调整agent的长与宽
        Vector2 sizeDelta = new Vector2(width, height);
        rectTransform.sizeDelta = sizeDelta;
        newAgent.Width = width;
        newAgent.Height = height;

        //  定面板位置
        Vector2 ori_position = new Vector2(ori_x, ori_y);
        newAgent.OriVector2 = ori_position;

        // 调整 collider
        BoxCollider2D boxCollider2D = newAgent.GetComponent<BoxCollider2D>();
        boxCollider2D.size = new Vector2(width, height);
        //  初始化内容
        newAgent.Initialize(_manager,ori_position, new Vector2(gen_x, gen_y), row + 1, column + 1,
            width, height, activity.Ent_id, activity.Image, false, 2);

        //  添加到组件袋
        _agentManager.Agents.Add(newAgent);

        return newAgent;
    }
    #endregion

    #region 生成滑动卡片
    public CardAgent GenerateCardAgent(Vector3 genPos, FlockAgent flockAgent, int dataId, bool isActive)
    {

        //  创建 Agent
        SliceCardAgent sliceCardAgent = Instantiate(
                                    _manager.sliceCardgent,
                                    _operationPanel
                                    ) as SliceCardAgent;

        if (flockAgent != null) {
            //  命名
            sliceCardAgent.name = "Activity (" + flockAgent.name + ")";

            //  添加原组件
            sliceCardAgent.OriginAgent = flockAgent;
        }


        //  获取rect引用
        RectTransform rectTransform = sliceCardAgent.GetComponent<RectTransform>();

        //  定出生位置
        rectTransform.anchoredPosition3D = genPos;

        //  定义缩放
        Vector3 scaleVector3 = new Vector3(0.2f, 0.2f, 0.2f);
        rectTransform.localScale = scaleVector3;

        //  初始化内容
        sliceCardAgent.Width = rectTransform.rect.width;
        sliceCardAgent.Height = rectTransform.rect.height;

        //  配置scene
        sliceCardAgent.SceneIndex = _manager.SceneIndex;

        // 添加管理器索引
        sliceCardAgent.manager = _manager;

        //  初始化数据
        sliceCardAgent.InitData(dataId, 1);

        // 添加到effect agent
        _agentManager.AddEffectItem(sliceCardAgent);

        // 设置显示状态
        sliceCardAgent.gameObject.SetActive(isActive);

        return sliceCardAgent;
    }
    #endregion


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

    //public CardAgent GenerateCardAgent(Vector3 genPos, FlockAgent flockAgent, int dataId , bool isActive)
    //{
    //    CardAgent cardAgent = GenerateCardAgent(genPos, flockAgent);
    //    // 设置显示状态
    //    cardAgent.gameObject.SetActive(isActive);

    //    return cardAgent;
    //}


}
