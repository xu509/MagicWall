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
namespace MagicWall
{
    public class MagicWallManager : MonoBehaviour
    {
        //
        //  Single
        //
        protected MagicWallManager() { }

        #region 可配置项
        public bool switchMode = false;

        [SerializeField] int _row = 12;   //  列数


        [SerializeField, Header("Camera")] Camera _mainCamera;
        public Camera mainCamera { get { return _mainCamera; } }

        [SerializeField] Camera _starCamera;    // 星空camera

        // 定制 INFO 面板
        [SerializeField, Header("UI")] InfoPanelAgent infoPanelAgent;
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
        [SerializeField] Sprite _customLogoSprite;//

        // 场景管理器
        [SerializeField, Header("子控制器")] MagicSceneManager _magicSceneManager;
        // 实体管理器
        [SerializeField] AgentManager _agentManager;
        // 背景管理器
        [SerializeField] BackgroundManager _backgroundManager;

        [SerializeField] OperateMode _operateMode;  //操作模块

        [SerializeField] OperateCardManager _operateCardManager;
        public OperateCardManager operateCardManager { get { return _operateCardManager; } }

        [SerializeField,Header("Collision")] CollisionManager _collisionManager;
        public CollisionManager collisionManager { get { return _collisionManager; } }

        [SerializeField] CollisionBehaviorConfig _collisionBehaviorConfig;
        public CollisionBehaviorConfig collisionBehaviorConfig { get { return _collisionBehaviorConfig; } }

        [SerializeField] CollisionMoveBehaviourFactory _collisionMoveBehaviourFactory;
        public CollisionMoveBehaviourFactory collisionMoveBehaviourFactory { get { return _collisionMoveBehaviourFactory; } }


        [SerializeField, Header("kinect")] MKinectManager _kinectManager;
        public MKinectManager kinectManager { get { return _kinectManager; } }

        [SerializeField] bool _useKinect;
        public bool useKinect { set { _useKinect = value; } get { return _useKinect; } }

        /// 配置面板
        [SerializeField, Header("config")] ManagerConfig _managerConfig;

        // 手写板配置项
        [SerializeField] WritePanelConfig _writePanelConfig;

        [SerializeField, Header("Data Service")] MockDaoService _mockDaoService;
        [SerializeField] MockFeiyueDaoService _mockFeiyueDaoService;
        [SerializeField,Tooltip("智博会 - 飞越体感")] MockZBHFeiyueDaoService _mockZBHFeiyueDaoService;
        [SerializeField,Tooltip("智博会 - 奉贤企业")] MockZBHFengxianDaoService _mockZBHFengxianDaoService;
        [SerializeField] MockZhichengDaoService _mockZhichengDaoService;
        [SerializeField] MockShicunDaoService _mockShicunDaoService;
        [SerializeField] DaoService _realDaoService;

        [SerializeField, Header("Music")] MusicManager _musicManager;

        [SerializeField, Header("Global Data")] GlobalData _globalData;

        [SerializeField, Header("Flock Move Behavior")] FlockBehaviorConfig _flockBehaviorConfig;
        [SerializeField] MoveBehaviourFactory _moveBehaviourFactory;

        [SerializeField, Header("CutEffect Config")] CutEffectConfig _cutEffectConfig;

        [SerializeField, Header("Mock")] bool _isMockData;

        [SerializeField] bool _isMockFeiyueData;

        [SerializeField] bool _isMockZBHFeiyueData;

        [SerializeField] bool _isMockZBHFengxianData;

        [SerializeField] bool _isMockZhichengData;

        [SerializeField] bool _isMockShicunData;
        public bool isMockShicunData { get { return _isMockShicunData; } }

        public bool isMockFeiyueData { get { return _isMockFeiyueData; } }


        [SerializeField] private ScreenTypeEnum _screenType;
        public ScreenTypeEnum screenTypeEnum { set { _screenType = value; } get { return _screenType; } }



        #endregion

        #region 非配置属性

        // 面板的差值
        float panelOffsetX = 0f;
        float panelBackOffsetX = 0f;
        float panelOffsetY = 0f;

        int _sceneIndex = 0; // 场景索引
        private IScene _currentScene; // 当前场景
        private WallStatusEnum status;
        private bool _isLimitFps = false;
        public bool isLimitFps { set { _isLimitFps = value; } get { return _isLimitFps; } }
        bool _reset = false;    // 重置标志

        // 配置选项

        // public static string FileDir = "E:\\workspace\\MagicWall\\Files\\"; // xu pc电脑


        public static string FileDir = "C:\\workspace\\MagicWall\\Files\\"; // 公司开发

         public static string FileDir = "D:\\workspace\\MagicWall\\Files\\"; // xu  笔记本电脑


        //public static string FileDir = "D:\\MagicWall\\Files\\";  // 柯 笔记本电脑

        private int themeCounter = 0; // 主题计数器
        public int ThemeCounter { get { return themeCounter; }}


        #endregion

        #region Private Parameter - Data
        // 数据管理器
        IDaoService _daoService;

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
        public Sprite CustomLogoSprite { get { return _customLogoSprite; } }
        public int Row { get { return _row; } }
        public float PanelOffsetX { get { return panelOffsetX; } set { panelOffsetX = value; } }
        public float PanelBackOffsetX { get { return panelBackOffsetX; } set { panelBackOffsetX = value; } }
        public float PanelOffsetY { get { return panelOffsetY; } set { panelOffsetY = value; } }
        public int SceneIndex { get { return _sceneIndex; } set { _sceneIndex = value; } }
        public IScene CurrentScene { get { return _currentScene; } set { _currentScene = value; } }
        public WallStatusEnum SceneStatus { get { return status; } set { status = value; } }
        public AgentManager agentManager { get { return _agentManager; } }
        public BackgroundManager backgroundManager { get { return _backgroundManager; } }
        public IDaoService daoService { get { return _daoService; } }
        public WritePanelConfig writePanelConfig { get { return _writePanelConfig; } }
        public FlockBehaviorConfig flockBehaviorConfig { get { return _flockBehaviorConfig; } }
        public MoveBehaviourFactory moveBehaviourFactory { get { return _moveBehaviourFactory; } }
        public bool IsMockData { get { return _isMockData; } }
        public GlobalData globalData { get { return _globalData; } }
        public CutEffectConfig cutEffectConfig { get { return _cutEffectConfig; } }

        public Camera starCamera { get { return _starCamera; } }


        // 获取文件地址
        #endregion


        UdpServer udpServer;

        private bool _hasInit = false;

        private void Init()
        {

            // 设置项目最高帧率（对编辑器无效）
            Application.targetFrameRate = 60;
            _isLimitFps = true;

            // 识别屏幕状态
            RecognizeScreenType();


            // 初始化数据连接服务
            TheDataSource theDataSource = TheDataSource.Instance;

            // 初始化数据服务
            if (_isMockData)
            {
                if (switchMode)
                {
                    if (themeCounter % 2 == 0)
                    {
                        _daoService = _mockFeiyueDaoService;
                    }
                    else {
                        _daoService = _mockZhichengDaoService;
                    }
                }
                else {
                    if (_isMockFeiyueData)
                    {
                        _daoService = _mockFeiyueDaoService;
                    }
                    else if (_isMockZBHFengxianData) {
                        _daoService = _mockZBHFengxianDaoService;
                    }
                    else if (_isMockZBHFeiyueData)
                    {
                        _daoService = _mockZBHFeiyueDaoService;
                    }
                    else if (_isMockShicunData)
                    {
                        _daoService = _mockShicunDaoService;
                    }
                    else
                    {
                        _daoService = _mockDaoService;
                    }
                }
                
            }
            else
            {
                _daoService = _realDaoService;
                _realDaoService.Init(this);
            }

            // 初始化 Global Data
            _globalData.Init(this);

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
            _magicSceneManager.Init(this, ExchangeTheme, OnStartSceneCompleted);

            // 初始化背景管理器, 此时对象池完成
            _backgroundManager.Init(this);

            // 初始化实体管理器
            _agentManager.Init(this);

            //  初始化操作模块
            _operateMode.Init(this);

            // 初始化操作卡片管理器
            _operateCardManager.Init(this);

            // 初始化音乐服务
            _musicManager.Init();

            _musicManager.Play();

            // 初始化kinect
            if (_useKinect) {
                _kinectManager.Init(this);
            }
           

            // 初始化定制服务
            if (managerConfig.IsCustom)
            {
                infoPanelAgent.Init(this);
            }

            _hasInit = true;


            //StartCoroutine(ExchangeScene(switchTime));

        }





        // Awake - init manager of Singleton
        private void Awake()
        {
            _hasInit = false;
        }


        // Start is called before the first frame update
        void Start()
        {
            Init();
        }

        // Update is called once per frame
        private void Update()
        {
            if (!_hasInit)
                return;

            // 开启场景效果
            _magicSceneManager.Run();

            if (_useKinect) { 
                _kinectManager.Run();
            }

            _agentManager.Run();

            _collisionManager.Run();

            //  启动监听
            udpServer.Listening();
            if (_reset)
            {
                Reset();
            }

            // 自定义屏幕状态更新
            if (managerConfig.IsCustom)
            {
                infoPanelAgent.Run();
            }

            _operateMode.Run();

        }

        #region 清理面板
        public bool Clear()
        {
            _agentManager.ClearAll(); //清理 agent 袋
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
        public void updateOffsetOfCanvas()
        {
            if (status == WallStatusEnum.Cutting)
            {
                PanelOffsetX = 0f;
            }
            else
            {
                Vector2 mainPanelPosition = mainPanel.anchoredPosition;
                Vector2 operationPanelPosition = _operationPanel.anchoredPosition;
                Vector2 backPanelPosition = backPanel.anchoredPosition;

                PanelOffsetX = operationPanelPosition.x - mainPanelPosition.x;
                PanelBackOffsetX = (operationPanelPosition - backPanelPosition - mainPanelPosition).x;
                PanelOffsetY = operationPanelPosition.y - mainPanelPosition.y;
            }
            //Debug.Log("OFFSET IS " + PanelOffset);
        }

        public void updateOffsetOfCanvasDirect()
        {
            Vector2 mainPanelPosition = mainPanel.anchoredPosition;
            Vector2 operationPanelPosition = _operationPanel.anchoredPosition;
            Vector2 backPanelPosition = backPanel.anchoredPosition;

            PanelOffsetX = operationPanelPosition.x - mainPanelPosition.x;
            PanelBackOffsetX = (operationPanelPosition - backPanelPosition - mainPanelPosition).x;
            PanelOffsetY = operationPanelPosition.y - mainPanelPosition.y;
        }

        public void RecoverFromFade() {
            if (mainPanel.GetComponent<CanvasGroup>().alpha < 1) {
                mainPanel.GetComponent<CanvasGroup>().alpha = 1;
            }
        }



        #endregion


        IEnumerator AfterFixedUpdate()
        {
            yield return new WaitForFixedUpdate();
        }


        public void Reset()
        {
            _hasInit = false;
            _reset = false;

            // 当前场景退出动画（淡出）
            CanvasGroup cg = _magicWallPanel.GetComponent<CanvasGroup>();
            cg.DOFade(0, 1).OnComplete(() =>
            {
                //  初始化组件库
                _agentManager.Reset();

                //  初始化场景库
                _magicSceneManager.Reset();

                //  初始化背景库
                //_backgroundManager.Reset();

                //  初始化 SOCKET 接收器
                UdpServer.Instance.Reset();

                //Init();

                //cg.DOFade(1, 1);

                SceneManager.LoadScene("Main");

            });
        }

        private void ResetMainPanel()
        {
            if (managerConfig.IsCustom)
            {
                // 此时为什么会偏移
                //mainPanel.anchoredPosition = new Vector2(540, 0);
                //while (mainPanel.anchoredPosition != new Vector2(540, 0))
                //{
                //    StartCoroutine(AfterFixedUpdate());
                //}
                mainPanel.anchoredPosition = Vector2.zero;  //主面板归位
                                                            //mainPanel.offsetMin = new Vector2(2160, 0);
                                                            //mainPanel.offsetMax = new Vector2(-1080, 0);
                                                            //while (mainPanel.anchoredPosition != Vector2.zero)
                                                            //{
                                                            //    StartCoroutine(AfterFixedUpdate());
                                                            //}

            }
            else
            {
                mainPanel.anchoredPosition = Vector2.zero;  //主面板归位
                                                            //while (mainPanel.anchoredPosition != Vector2.zero)
                                                            //{
                                                            //    StartCoroutine(AfterFixedUpdate());
                                                            //}
            }
        }


        public void SetReset() { _reset = true; }

        public Vector2 GetScreenRect()
        {
            Debug.Log(1);

            return new Vector2(Screen.width, Screen.height);
        }

        private void ExchangeTheme() {
            // ExchangeTheme


            System.Diagnostics.Stopwatch sw2 = new System.Diagnostics.Stopwatch();
            sw2.Start();

            //Debug.Log("修改主题");
            infoPanelAgent.Hide();

            _hasInit = false;
            themeCounter++;


            ExchangeThemeInit();

            sw2.Stop();
            //Debug.Log(" ExchangeTheme init : " + sw2.ElapsedMilliseconds / 1000f);


            _hasInit = true;

        }


        private void ExchangeThemeInit() {

            // 初始化数据服务
            if (_isMockData)
            {
                if (switchMode)
                {
                    if (themeCounter % 2 == 0)
                    {
                        _daoService = _mockFeiyueDaoService;
                    }
                    else
                    {
                        _daoService = _mockZhichengDaoService;
                    }
                }
            }

            // 初始化 Global Data
            _globalData.Init(this);

            ResetMainPanel(); //主面板归位

            PanelOffsetX = 0f;   // 清理两个panel偏移量
            PanelBackOffsetX = 0f;
            PanelOffsetY = 0f;   // 清理两个panel偏移量

            // 初始化场景管理器
            _magicSceneManager.Init(this, ExchangeTheme, OnStartSceneCompleted);

            // 初始化定制服务
            if (managerConfig.IsCustom)
            {
                infoPanelAgent.Init(this);
            }

        }





        private void OnStartSceneCompleted() {
            //OnStartSceneCompleted
            if (managerConfig.IsCustom) {
                // 显示
                infoPanelAgent.Show();

            }
        }


        /// <summary>
        ///     识别屏幕类型
        /// </summary>
        private void RecognizeScreenType() {
            var height = Screen.height;

            if (height < 1300)
            {
                _screenType = ScreenTypeEnum.Screen720P;
            }
            else {
                _screenType = ScreenTypeEnum.Screen1080P;
            }
        }



    }

}