using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using EasingUtil;
//
//  入口类
//
public class MagicWallManager:MonoBehaviour
{
    //
    //  Single
    //
    protected MagicWallManager() { }

    #region 可配置项
    // 场景管理器
    [SerializeField] MagicSceneManager _magicSceneManager;
    // 实体管理器
    [SerializeField] AgentManager _agentManager;
    // 背景管理器
    [SerializeField] BackgroundManager _backgroundManager;

    [SerializeField] ItemsFactoryAgent _itemsFactoryAgent;


    /// 配置面板
    [SerializeField] ManagerConfig _managerConfig;
    // 定制 INFO 面板
    [SerializeField] InfoPanelAgent infoPanelAgent;
    // MagicWall 面板
    [SerializeField] RectTransform _magicWallPanel;
    // MainPanel 面板
    [SerializeField] RectTransform _mainPanel;
    // Back Panel 面板
    [SerializeField] RectTransform _backPanel;
    // Back ground 面板
    [SerializeField] RectTransform _backgroundPanel;
    // Operate Panle 面板 （操作面板）
    [SerializeField] RectTransform _operationPanel;

    /// 预制体
    // 浮动块
    [SerializeField] FlockAgent _flockAgent;
    //  十字块
    [SerializeField] CardAgent _crossCardgent;
    // 滑动块
    [SerializeField] CardAgent _sliceCardgent;
    // 气泡预制体
    [SerializeField] GameObject _backgroundPrefab;
    // 气泡预制体2 
    [SerializeField] GameObject _backgroundPrefab2;
    // 搜索预制体
    [SerializeField] SearchAgent _searchAgentPrefab;

    /// 背景配置项
    //气泡上升时间
    [SerializeField, Range(10f, 30f)] float _backgroundUpDuration = 20f;
    //生成气泡时间间隔
    [SerializeField, Range(0.1f, 10f)] float _backgroundUubbleInterval = 0.2f;

    /// 卡片物理碰撞效果设置
    // 影响距离
    [SerializeField, Range(0f, 2f), Header("影响距离（当为1时，表示半径）")] float _influenceDistance;
    // 影响移动距离系数
    [SerializeField, Range(0f, 10f)] float _influenceMoveFactor = 0.5f;
    // 卡片动画效果
    [SerializeField] EaseEnum _influenceEaseEnum;

    /// 整体的移动速度
    [SerializeField, Range(1f, 600f)] float _movePanelFactor = 100f;
    
    /// 背景中的图片logo
    [SerializeField] RectTransform _bg_logo; //背景图中的logo

    // 配置的行数
    [SerializeField] int _row = 6;

    #endregion

    #region 非配置属性

    // 面板的差值
    float panelOffsetX = 0f;
    float panelOffsetY = 0f;

    AgentType theItemType;
    int _sceneIndex = 0; // 场景索引
    private IScene _currentScene; // 当前场景
    private WallStatusEnum status;
    bool _reset = false;    // 重置标志

    // 配置选项

    //public static string FileDir = "E:\\workspace\\MagicWall\\Assets\\Files\\"; // xu pc电脑


    // public static string FileDir = "D:\\workspace\\MagicWall\\Assets\\Files\\"; // xu  笔记本电脑

    //public static string FileDir = "D:\\MagicWall\\Assets\\Files\\";
    public static string FileDir = "D:\\MagicWall\\Files\\";  // 柯 笔记本电脑

    #endregion

    #region Private Parameter - Data
    // 数据管理器
    DaoService _daoService;

    //从下往上 列与底
    public Dictionary<int, float> columnAndBottoms;
    //从上往下 列与顶
    public Dictionary<int, float> columnAndTops;
    //行与右
    public Dictionary<int, float> rowAndRights;
    #endregion

    #region 引用
    public ManagerConfig managerConfig { get { return _managerConfig; } }
    public RectTransform magicWallPanel { get { return _magicWallPanel; } }
    public RectTransform mainPanel { get { return _mainPanel; } }
    public RectTransform backPanel { get { return _backPanel; } }//前后层展开效果的后层
    public FlockAgent flockAgent { get { return _flockAgent; } }
    public CardAgent crossCardgent { get { return _crossCardgent; } }
    public CardAgent sliceCardgent { get { return _sliceCardgent; } }
    public GameObject backgroundPrefab { get { return _backgroundPrefab; } }//气泡预制体
    public GameObject backgroundPrefab2 { get { return _backgroundPrefab2; } }//气泡预制体
    public SearchAgent searchAgentPrefab { get { return _searchAgentPrefab; } }// 搜索预制体
    public RectTransform BackgroundPanel { get { return _backgroundPanel; } }
    public RectTransform OperationPanel { get { return _operationPanel; } }
    public float backgroundUpDuration { get { return _backgroundUpDuration; } }//气泡上升时间
    public float backgroundUubbleInterval { get { return _backgroundUubbleInterval; } }//生成气泡时间间隔.
    public float InfluenceDistance { get { return _influenceDistance; } }   // 影响距离
    public float InfluenceMoveFactor { get { return _influenceMoveFactor; } }  // 影响移动距离
    public EaseEnum InfluenceEaseEnum { get { return _influenceEaseEnum; } }
    public float MovePanelFactor { get { return _movePanelFactor; } }
    public RectTransform BgLogo { get { return _bg_logo; } }
    public int Row { get { return _row; } }
    public float PanelOffsetX { get { return panelOffsetX; } set { panelOffsetX = value; } }
    public float PanelOffsetY { get { return panelOffsetY; } set { panelOffsetY = value; } }
    public AgentType TheItemType { set { theItemType = value; } get { return theItemType; } }
    public int SceneIndex { get { return _sceneIndex; } set { _sceneIndex = value; } }
    public IScene CurrentScene { get { return _currentScene; } set { _currentScene = value; } }
    public WallStatusEnum Status { get { return status; } set { status = value; } }
    public AgentManager agentManager { get { return _agentManager; } }
    public BackgroundManager backgroundManager { get { return _backgroundManager; } }
    public DaoService daoService { get { return _daoService; } }
    public ItemsFactoryAgent itemsFactoryAgent { get { return _itemsFactoryAgent; } }

    // 获取文件地址
    #endregion


    UdpServer udpServer;

    private bool _hasInit = false;

    private void Init() {

        // 初始化数据连接服务
        TheDataSource theDataSource = TheDataSource.Instance;

        // 初始化数据服务
        _daoService = DaoService.Instance;

        // 初始化监听服务
        udpServer = UdpServer.Instance;
        udpServer.SetManager(this);

        // 设置 Dotween 插件
        DOTween.logBehaviour = LogBehaviour.ErrorsOnly;

        ResetMainPanel(); //主面板归位

        PanelOffsetX = 0f;   // 清理两个panel偏移量
        PanelOffsetY = 0f;   // 清理两个panel偏移量


        // 初始化场景管理器
        _magicSceneManager.Init(this);

        // 初始化背景管理器
        _backgroundManager.Init(this);

        // 初始化实体管理器
        _agentManager.Init(this);

        // 初始化实体工厂
        _itemsFactoryAgent.Init(this);


        _hasInit = true;
    }



    // Awake - init manager of Singleton
    private void Awake()
    {
        _hasInit = false;

        //  装载内容
        
    }


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!_hasInit)
            return;

        // 开启场景效果
        _magicSceneManager.Run();

        _agentManager.Run();        

        //  启动监听
        udpServer.Listening();
        if (_reset) {
            Reset();
        }

        // 自定义屏幕状态更新
        if (managerConfig.IsCustom) {
            infoPanelAgent.Run();
        }

    }

    #region 清理面板
    public bool Clear() {
        _agentManager.ClearAgents(); //清理 agent 袋
        ResetMainPanel(); //主面板归位
        backPanel.anchoredPosition = Vector2.zero;

        PanelOffsetX = 0f;   // 清理两个panel偏移量
		PanelOffsetY = 0f;   // 清理两个panel偏移量

        return true;
    }
    #endregion

    #region 获取两块画布的偏移量
    public void updateOffsetOfCanvas() {
        if (status == WallStatusEnum.Cutting)
        {
            PanelOffsetX = 0f;
        }
        else {
            Vector2 v1 = mainPanel.anchoredPosition;
            Vector2 v2 = _operationPanel.anchoredPosition;
            float offset = (v2.x - v1.x);
			PanelOffsetX = offset;


			Vector2 v3 = mainPanel.anchoredPosition;
			Vector2 v4 = _operationPanel.anchoredPosition;
			float offsety = (v4.y - v3.y);
			PanelOffsetY = offsety;
		}
		//Debug.Log("OFFSET IS " + PanelOffset);
	}
    #endregion


    IEnumerator AfterFixedUpdate()
    {
        yield return new WaitForFixedUpdate();
    }


    public void Reset() {
        _hasInit = false;
        _reset = false;

        // 当前场景退出动画（淡出）
        CanvasGroup cg = _magicWallPanel.GetComponent<CanvasGroup>();
        cg.DOFade(0, 1).OnComplete(() => {
            //  初始化组件库
            _agentManager.Reset();

            //  初始化场景库
            _magicSceneManager.Reset();

            //  初始化背景库
            _backgroundManager.Reset();

            //  初始化 SOCKET 接收器
            UdpServer.Instance.Reset();

            //Init();

            //cg.DOFade(1, 1);

            SceneManager.LoadScene("Main");

        });
    }

    private void ResetMainPanel() {
        if (managerConfig.IsCustom)
        {
            mainPanel.anchoredPosition = new Vector2(540, 0);
            while (mainPanel.anchoredPosition != new Vector2(540, 0))
            {
                StartCoroutine(AfterFixedUpdate());
            }
        }
        else
        {
            mainPanel.anchoredPosition = Vector2.zero;  //主面板归位
            while (mainPanel.anchoredPosition != Vector2.zero)
            {
                StartCoroutine(AfterFixedUpdate());
            }
        }
    }


    public void SetReset() { _reset = true; }


    void OnGUI()
    {

        GUIStyle titleStyle2 = new GUIStyle();
        titleStyle2.normal.textColor = Color.black;
        titleStyle2.fontSize = 60;


        GUI.Label(new Rect(30, 10, 300, 300), Input.mousePosition.ToString(), titleStyle2);
    }

}

