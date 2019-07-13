﻿using System.Collections;
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

    // 定制 INFO 面板
    [SerializeField,Header("UI")] InfoPanelAgent infoPanelAgent;
    // MagicWall 面板
    [SerializeField] RectTransform _magicWallPanel;
    // start effect 面板
    [SerializeField] RectTransform _StarEffectContainer;
    // start content 面板
    [SerializeField] RectTransform _StarEffectContent;
    // MainPanel 面板
    [SerializeField] RectTransform _mainPanel;
    // Back Panel 面板
    [SerializeField] RectTransform _backPanel;
    // Operate Panle 面板 （操作面板）
    [SerializeField] RectTransform _operationPanel;

    /// 背景中的图片logo
    [SerializeField] RectTransform _bg_logo; //背景图中的logo

    // 场景管理器
    [SerializeField, Header("子控制器")] MagicSceneManager _magicSceneManager;
    // 实体管理器
    [SerializeField] AgentManager _agentManager;
    // 背景管理器
    [SerializeField] BackgroundManager _backgroundManager;

    [SerializeField] ItemsFactoryAgent _itemsFactoryAgent;

    [SerializeField] OperateMode _operateMode;  //操作模块

    /// 配置面板
    [SerializeField, Header("config")] ManagerConfig _managerConfig;

    // 手写板配置项
    [SerializeField] WritePanelConfig _writePanelConfig;


    #endregion

    #region 非配置属性
    int _row = 6;   //  列数

    // 面板的差值
    float panelOffsetX = 0f;
    float panelBackOffsetX = 0f;
    float panelOffsetY = 0f;

    int _sceneIndex = 0; // 场景索引
    private IScene _currentScene; // 当前场景
    private WallStatusEnum status;
    bool _reset = false;    // 重置标志

    // 配置选项

   // public static string FileDir = "E:\\workspace\\MagicWall\\Assets\\Files\\"; // xu pc电脑

     public static string FileDir = "D:\\workspace\\MagicWall\\Assets\\Files\\"; // xu  笔记本电脑

   // public static string FileDir = "D:\\MagicWall\\Files\\";  // 柯 笔记本电脑
 

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
    public RectTransform starEffectContainer { get { return _StarEffectContainer; } }
    public RectTransform starEffectContent { get { return _StarEffectContent; } }
    public RectTransform mainPanel { get { return _mainPanel; } }
    public RectTransform backPanel { get { return _backPanel; } }//前后层展开效果的后层
    public RectTransform OperationPanel { get { return _operationPanel; } }
    public RectTransform BgLogo { get { return _bg_logo; } }
    public int Row { get { return _row; } }
    public float PanelOffsetX { get { return panelOffsetX; } set { panelOffsetX = value; } }
    public float PanelBackOffsetX { get { return panelBackOffsetX; } set { panelBackOffsetX = value; } }
    public float PanelOffsetY { get { return panelOffsetY; } set { panelOffsetY = value; } }
    public int SceneIndex { get { return _sceneIndex; } set { _sceneIndex = value; } }
    public IScene CurrentScene { get { return _currentScene; } set { _currentScene = value; } }
    public WallStatusEnum Status { get { return status; } set { status = value; } }
    public AgentManager agentManager { get { return _agentManager; } }
    public BackgroundManager backgroundManager { get { return _backgroundManager; } }
    public DaoService daoService { get { return _daoService; } }
    public ItemsFactoryAgent itemsFactoryAgent { get { return _itemsFactoryAgent; } }
    public WritePanelConfig writePanelConfig { get { return _writePanelConfig; } }

    // 获取文件地址
    #endregion


    UdpServer udpServer;

    private bool _hasInit = false;

    private void Init() {

        // 设置项目最高帧率（对编辑器无效）
        Application.targetFrameRate = 60;



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
        PanelBackOffsetX = 0f;
        PanelOffsetY = 0f;   // 清理两个panel偏移量


        // 初始化场景管理器
        _magicSceneManager.Init(this);

        // 初始化背景管理器, 此时对象池完成
        _backgroundManager.Init(this);

        // 初始化实体管理器
        _agentManager.Init(this);

        // 初始化实体工厂
        _itemsFactoryAgent.Init(this);

        //  初始化操作模块
        _operateMode.Init(this);

        _hasInit = true;
    }





    // Awake - init manager of Singleton
    private void Awake()
    {
        _hasInit = false;

        //  帧数限制
        //Application.targetFrameRate = FPS;

    }


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    //private void Update() { 

    private void Update()
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

        _operateMode.Run();

    }

    #region 清理面板
    public bool Clear() {
        _agentManager.ClearAgents(); //清理 agent 袋
        ResetMainPanel(); //主面板归位
        backPanel.anchoredPosition = new Vector2(-200, 0);
            //Vector2.zero;
        backPanel.SetAsFirstSibling();

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
            Vector2 mainPanelPosition = mainPanel.anchoredPosition;
            Vector2 operationPanelPosition = _operationPanel.anchoredPosition;
            Vector2 backPanelPosition = backPanel.anchoredPosition;

            PanelOffsetX = operationPanelPosition.x - mainPanelPosition.x;
            PanelBackOffsetX = (operationPanelPosition - backPanelPosition - mainPanelPosition).x;
            PanelOffsetY = operationPanelPosition.y - mainPanelPosition.y;
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
            // 此时为什么会偏移
            //mainPanel.anchoredPosition = new Vector2(540, 0);
            //while (mainPanel.anchoredPosition != new Vector2(540, 0))
            //{
            //    StartCoroutine(AfterFixedUpdate());
            //}
            mainPanel.anchoredPosition = Vector2.zero;  //主面板归位
            while (mainPanel.anchoredPosition != Vector2.zero)
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

    public Vector2 GetScreenRect() {
        return new Vector2(Screen.width,Screen.height);
    }


}

