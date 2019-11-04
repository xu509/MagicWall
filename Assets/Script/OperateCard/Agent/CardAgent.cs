using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;




namespace MagicWall
{
    public class CardAgent : MonoBehaviour, CollisionEffectAgent, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler, MoveSubject
    {
        //[SerializeField,Range(0f,30f)] float _destoryForSpaceTime = 7f;
        [SerializeField, Header("Protect")] ProtectAgent _protectAgent;  //   保护层代理，当开始关闭时，出现保护层

        protected FlockTweenerManager _tweenerManager;
        public FlockTweenerManager flockTweenerManager { get { return _tweenerManager; } }


        protected MagicWallManager _manager;
        protected int _dataId;
        private float _width;
        public float width {
            get {
                return GetComponent<RectTransform>().rect.width;
            }
        }
        private float _height;
        public float height
        {
            get
            {
                return GetComponent<RectTransform>().rect.height;
            }
        }

        protected DataTypeEnum dataType;


        private DaoTypeEnum _daoTypeEnum;
        public DaoTypeEnum daoTypeEnum { set { _daoTypeEnum = value; } get { return _daoTypeEnum; } }


        private List<ExtraCardData> _extraCardDatas;
        private int _sceneIndex;


        /// <summary>
        ///     碰撞体移动表现器
        /// </summary>
        private ICollisionMoveBehavior _collisionMoveBehavior;


        #region Parameter
        private float _recentActiveTime = 0f;   //  最近次被操作的时间点
        private float _destoryFirstStageCompleteTime = 0f;   //  第一阶段关闭的时间点
        private float _activeFirstStageDuringTime = 7f;   //  最大的时间
        private float _activeSecondStageDuringTime = 2f;   //  第二段缩小的时间
        private bool _showDetail = false;   //  显示企业卡片
        private bool _hasChangeSize = false;    //  大小改变的标志位
        private bool _keepOpen = false;     //  保持开启
        protected bool KeepOpen { set { _keepOpen = value; } get { return _keepOpen; } }
        private bool _inExtendOpen = false;
        protected bool InExtendOpen { set { _inExtendOpen = value; } get { return _inExtendOpen; } }

        private ScaleAgent _scaleAgent;     // 缩放代理
        private VideoAgent _videoAgent;
        private SearchAgent _searchAgent;

        private Vector2 _moveStartPosition = Vector2.zero;  //移动的开始位置
        private Vector2 _moveStartOffset = Vector2.zero;  //移动偏差量
        private float _panel_top;
        private float _panel_bottom;
        private float _panel_left;
        private float _panel_right;
        private float _safe_distance_width;
        private float _safe_distance_height;
        private CardRecoverStatus _cardRecoverStatus = CardRecoverStatus.Init;

        // 提示
        protected QuestionTypeEnum _questionTypeEnum;   // 提示组件所需的提示类型
        private bool _showQuestion = false;
        private QuestionAgent _questionAgent;

        private RectTransform parentRtf;

        private bool _isPhysicsMoving = false;
        public bool isPhysicsMoving { set { _isPhysicsMoving = value; } get { return _isPhysicsMoving; } }

        private bool _isPrepared = false;
        public bool isPrepared { set { _isPrepared = value; } get { return _isPrepared; } }

        /// <summary>
        /// 第一次销毁动画的 DoTween 代理
        /// </summary>
        private Tweener _destory_first_scale_tweener;

        private float radiusFactor;// Colider 半径系数

        private List<MoveBtnObserver> _moveBtnObservers;    // 移动按钮的观察者

        private bool _disableEffect = false;
        public bool disableEffect{ get { return _disableEffect; } }



        public CardStatusEnum _cardStatus;   // 状态   
        protected FlockAgent _originAgent;  // 原组件

        protected bool _hasListBtn;  //  有列表按钮
        protected bool hasInitBusinessCard = false; // 是否已生成business card
        protected BusinessCardAgent businessCardAgent;

        [SerializeField, Range(0f, 1f)] float _heightFactor; //      高度比例，如当屏幕高度100，卡片高度50时，则高度比为 0.5
        [SerializeField, Header("UI")] RectTransform _main_container;    //  主框体
        [SerializeField] RectTransform _tool_bottom_container; //   按钮工具栏（4项）
        [SerializeField] RectTransform _tool_bottom_three_container; //   按钮工具栏（3项）
        [SerializeField] RectTransform normalContainer; // 正常显示的框体

        [SerializeField, Header("Scale")] RectTransform _scale_container;    //  缩放容器
        [SerializeField] ScaleAgent _scale_prefab;    //  缩放 prefab

        [SerializeField, Header("Search")] RectTransform _searchContainer;    //  搜索容器
        [SerializeField] SearchAgent _searchAgentPrefab;    //  搜索 prefab

        [SerializeField, Header("Move")] MoveAgent _moveAgent;
        [SerializeField] MoveButtonComponent _moveBtnComponent;
        [SerializeField] MoveButtonComponent _moveBtnComponentInThree;

        [SerializeField, Header("Question")] RectTransform _question_container;
        [SerializeField] QuestionAgent _questionAgentPrefab;

        [SerializeField, Header("Business Card")] RectTransform _business_card_container;    // 企业卡片容器
        [SerializeField] BusinessCardAgent _business_card_prefab;    // 企业卡片 control

        [SerializeField] CircleCollider2D _collider;    // 圆形碰撞体
        [SerializeField, Header("Video")] VideoAgent videoAgentPrefab;   // Video Agent prefab
        [SerializeField] RectTransform videoContainer;  // video的安放框体

        [SerializeField, Header("Physics"), Range(0, 500)] int _physicesEffectFactor;


        Action OnCreatedCompletedAction;
        Action OnGoToFrontFinsihed;

        /// <summary>
        /// 卡片恢复状态
        /// </summary>
        enum CardRecoverStatus
        {
            Init, Recovering
        }

        public CardStatusEnum CardStatus
        {
            set { _cardStatus = value; }
            get { return _cardStatus; }
        }

        public FlockAgent OriginAgent
        {
            set { _originAgent = value; }
            get { return _originAgent; }
        }

        #endregion

        private void DoCardReset()
        {
            _showQuestion = false;
        }


        #region Protected Method


        public virtual void InitData(OperateCardData operateCardData)
        {
            //Debug.Log("Do In Parent");
        }



        /// <summary>
        ///     初始化卡片类型浮动块数据
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="dataId"></param>
        /// <param name="dataType"></param>
        /// <param name="genPosition">生成位置</param>
        /// <param name="scaleVector3">缩放比例</param>
        /// <param name="originAgent">原关联的浮块</param>
        public void InitComponent(MagicWallManager manager, int dataId, DataTypeEnum dataType,
            Vector3 genPosition, FlockAgent originAgent)
        {
            //InitBase(manager, dataId, dataType, true);
            _tweenerManager = new FlockTweenerManager();

            _manager = manager;
            _dataId = dataId;


            // 初始化框体长宽
            UpdateUI();


            //  命名
            if (originAgent != null)
            {
                name = dataType.ToString() + "(" + originAgent.name + ")";


                _daoTypeEnum = originAgent.daoTypeEnum;

                //  添加原组件
                OriginAgent = originAgent;
            }

            //  定出生位置
            GetComponent<RectTransform>().anchoredPosition3D = genPosition;

            //  配置scene
            _sceneIndex = _manager.SceneIndex;

            //Debug.Log("设置Scene Index ： " + _manager.SceneIndex);


            //  初始化长宽字段
            _width = GetComponent<RectTransform>().rect.width;
            _height = GetComponent<RectTransform>().rect.height;

            // 初始化移动组件
            _moveBtnObservers = new List<MoveBtnObserver>();

            _moveBtnComponent?.Init(DoMove, this);
            _moveBtnComponentInThree?.Init(DoMove, this);

            parentRtf = transform.parent as RectTransform;
        }


        //
        //  Init 代理
        //
        protected void InitAgency()
        {
            _recentActiveTime = Time.time;
            _cardStatus = CardStatusEnum.NORMAL;

            //_panel_top = -(_manager.OperationPanel.rect.yMin) + _manager.OperationPanel.rect.yMax;
            //_panel_bottom = 0;
            //_panel_left = 0;
            //_panel_right = -(_manager.OperationPanel.rect.xMin) + _manager.OperationPanel.rect.xMax;


            var opposition = _manager.OperationPanel.position;

            _panel_top = opposition.y + (_manager.OperationPanel.rect.height / 2);
            _panel_bottom = opposition.y - (_manager.OperationPanel.rect.height / 2);
            _panel_left = opposition.x - (_manager.OperationPanel.rect.width / 2);
            _panel_right = opposition.x + (_manager.OperationPanel.rect.width / 2);

            //print("_panel_top:" + _panel_top+ "---_panel_bottom:" + _panel_bottom+ "---_panel_left:" + _panel_left+ "---_panel_right:" + _panel_right);
            //_panel_left = _manager.OperationPanel.rect.xMin;
            //print("_manager.OperationPanel.rect.xMin; : " + _manager.OperationPanel.rect.xMin);
            //_panel_right = _panel_left + -(_manager.OperationPanel.rect.xMin) + _manager.OperationPanel.rect.xMax;


            _safe_distance_width = GetComponent<RectTransform>().rect.width / 3;
            _safe_distance_height = GetComponent<RectTransform>().rect.height / 3;


            DoCardReset();
        }

        //
        //  Update 代理
        //
        protected void UpdateAgency()
        {

            //缩小一半
            if (_cardStatus == CardStatusEnum.NORMAL || _cardStatus == CardStatusEnum.MOVE || _cardStatus == CardStatusEnum.HIDE)
            {
                // 删除逻辑 REF ： https://www.yuque.com/docs/share/da04bf34-f197-41cb-9f3c-80308fe90cf1
                if (EnterToDestoryTime())
                {
                    DoDestoriedForFirstStep();
                }
            }

            // 第二次缩小
            if (_cardStatus == CardStatusEnum.DestoryFirstCompleted)
            {
                if (_destoryFirstStageCompleteTime != 0 && (Time.time - _destoryFirstStageCompleteTime) > _activeSecondStageDuringTime)
                {
                    DoDestoriedForSecondStep();
                }
            }
            

            UpdateColliderRadius();
        }


        /// <summary>
        ///     更新开关,保持卡片被操作时不会自动关闭
        /// </summary>
        public void DoUpdate()
        {
            if (_cardStatus == CardStatusEnum.NORMAL || _cardStatus == CardStatusEnum.MOVE)
            {
                _recentActiveTime = Time.time;
                _destoryFirstStageCompleteTime = 0f;
            }

            if (_cardStatus == CardStatusEnum.HIDE) {
                _recentActiveTime = Time.time;
            }

            if (_cardStatus == CardStatusEnum.RECOVER)
            {
                _recentActiveTime = Time.time;
                _destoryFirstStageCompleteTime = 0f;
            }


            //else if (CardStatus == CardStatusEnum.TODESTORY)
            //{
            //    DoRecover();
            //}
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// 销毁第一阶段
        /// </summary>
        private void DoDestoriedForFirstStep()
        {
            //Debug.Log(gameObject.name + " 进行第一次销毁。");

            // 如果当前处于移动中，则将移动关闭
            if (_cardStatus == CardStatusEnum.MOVE)
            {
                DoMove();
            }

            _protectAgent.DoActive(ProtectUpdatedInToDestory);

            //  缩放
            Vector3 scaleVector3 = new Vector3(0.7f, 0.7f, 0.7f);
            _cardStatus = CardStatusEnum.DESTORYINGFIRST;

            _destory_first_scale_tweener = GetComponent<RectTransform>().DOScale(scaleVector3, 2f)
                .OnUpdate(() =>
                 {
                     _width = GetComponent<RectTransform>().sizeDelta.x;
                     _height = GetComponent<RectTransform>().sizeDelta.y;
                     _hasChangeSize = true;
                 }).OnComplete(() =>
                 {
                     // 设置第一次缩小的点
                     _destoryFirstStageCompleteTime = Time.time;

                     _cardStatus = CardStatusEnum.DestoryFirstCompleted;

                     //Debug.Log(_originAgent.flockStatus);

                 }).OnKill(() => {});


            _tweenerManager.Add("d", _destory_first_scale_tweener);           
            //Debug.Log(gameObject.name + " 第一次销毁结束，增加了tweener。");
        }

        //
        //  第二步的销毁
        //
        private void DoDestoriedForSecondStep()
        {

            _cardStatus = CardStatusEnum.DESTORYINGSECOND;

            _protectAgent.SetDisabled();

            GetComponent<CircleCollider2D>().radius = 0;

            if (_originAgent == null) {
                Close();
            }else if (_originAgent.isStarEffect)
            {
                Close();
            }
            else
            {
                //  如果场景没有变，则回到原位置
                if ((_sceneIndex == _manager.SceneIndex) && (_originAgent != null))
                {
                    //恢复并归位
                    // 缩到很小很小
                    RectTransform cardRect = GetComponent<RectTransform>();

                    //  移到后方、缩小、透明
                    Tweener t = cardRect.DOScale(0.1f, 1f);
                   
                    //  获取位置
                    Vector3 to = new Vector3(_originAgent.OriVector2.x - _manager.PanelOffsetX
                        , _originAgent.OriVector2.y - _manager.PanelOffsetY, 200);

                    cardRect.DOAnchorPos3D(to, 1f)
                       .OnComplete(() =>
                       {
                           if ((_sceneIndex == _manager.SceneIndex) && (_originAgent != null)) { 
                               // 恢复
                               _originAgent.DoRecoverAfterChoose(cardRect.position);

                            }                            
                           _cardStatus = CardStatusEnum.OBSOLETE;
                       });

                }
                //  直接消失
                else
                {
                    Close();
                }
            }
        }



        private void Close() {
            // 慢慢缩小直到消失
            Vector3 vector3 = Vector3.zero;

            GetComponent<RectTransform>().DOScale(vector3, 1.5f)
                .OnUpdate(() =>
                {
                    _width = GetComponent<RectTransform>().sizeDelta.x;
                    _height = GetComponent<RectTransform>().sizeDelta.y;
                })
                .OnComplete(() =>
                {
                    _cardStatus = CardStatusEnum.OBSOLETE;

                    if (!(_originAgent.flockStatus == FlockStatusEnum.PREPARED))
                    {
                        _originAgent.flockStatus = FlockStatusEnum.OBSOLETE;
                    }

                    Debug.Log("直接删除");
                });

        }


        //
        //  恢复
        //
        private void DoRecover()
        {            
            _protectAgent.DoClose();
            _cardStatus = CardStatusEnum.RECOVER;

            DoUpdate();

            // 停止销毁动画
            Debug.Log("kill 第一次销毁动画");
            _destory_first_scale_tweener.Kill();
            Vector3 scaleVector3 = new Vector3(1f, 1f, 1f);

            // 此时会出现卡顿现象，可能是因为之前的缩小动画并未执行完毕，所以会与此语句冲突
            GetComponent<RectTransform>().DOScale(scaleVector3, 0.5f)
                .OnUpdate(() =>
                {
                    _width = GetComponent<RectTransform>().sizeDelta.x;
                    _height = GetComponent<RectTransform>().sizeDelta.y;
                    _hasChangeSize = true;
                    DoUpdate();
                }).OnComplete(() =>
                {
                    //IsChoosing = false;
                    _cardStatus = CardStatusEnum.NORMAL;
                    //_cardRecoverStatus = CardRecoverStatus.Init;
                    DoUpdate();
                });


        }


        #endregion

        //
        //  Call Back
        //

        //
        //  关闭
        //
        public void DoClose()
        {
            // 点击关闭
            if (CardStatus == CardStatusEnum.NORMAL)
            {
                DoDestoriedForSecondStep();
            }                
        }

        /// <summary>
        /// 直接关闭 ref ： https://www.yuque.com/docs/share/0c30b857-6019-41b0-b575-e9697bae1df6
        /// </summary>       
        public void DoCloseDirect()
        {
            // 直接关闭的逻辑
            //if (_cardStatus == CardStatusEnum.NORMAL) {
            //    DoDestoriedForSecondStep();
            //}
            _tweenerManager.Reset();

            DoDestoriedForSecondStep();

        }



        //
        //  点击详细按钮
        //
        public void DoDetail()
        {

            // 生成企业卡片
            if (!_showDetail)
            {
                OpenBusinessCard();
            }
            else
            {
                CloseBusinessCard();
            }
            
        }


        // 生成缩放卡片
        public void InitScaleAgent(Texture texture)
        {

            _scaleAgent = Instantiate(_scale_prefab,
                            _scale_container
                            ) as ScaleAgent;
            _scaleAgent.SetImage(texture);
            _scaleAgent.SetOnCloseClicked(OnScaleClose);
            _scaleAgent.SetOnReturnClicked(OnScaleReturn);
            _scaleAgent.SetOnUpdated(OnScaleUpdate);
            _scaleAgent.SetOnOpen(OnScaleOpen);

            // 显示缩放框体，隐藏普通框体
            if (_main_container.gameObject.activeSelf)
            {
                _main_container.gameObject.SetActive(false);
            }

            if (!_scaleAgent.gameObject.activeSelf)
            {
                _scaleAgent.gameObject.SetActive(true);
            }
            
        }



        //  初始化组件显示的状态
        protected void InitComponents(List<ExtraCardData> extraCardDatas)
        {
            _extraCardDatas = extraCardDatas;

            if (extraCardDatas != null && extraCardDatas.Count > 0)
            {
                _hasListBtn = true;
            }
            else
            {
                _hasListBtn = false;
            }

            if (_hasListBtn)
            {
                // 显示四组按钮
                _tool_bottom_container.gameObject.SetActive(true);
                _tool_bottom_three_container.gameObject.SetActive(false);
                //InitEnvCard(extraCardDatas);

                _questionTypeEnum = QuestionTypeEnum.SliceCardFour;


            }
            else
            {
                // 显示三个按钮
                _tool_bottom_container?.gameObject.SetActive(false);
                _tool_bottom_three_container?.gameObject.SetActive(true);

                _questionTypeEnum = QuestionTypeEnum.SliceCard;


            }

            //// 不显示移动提示
            //_move_reminder_container.gameObject.SetActive(false);
            //// 关闭移动蒙版
            //_move_mask.gameObject.SetActive(false);

        }




        // 当点击缩放收回
        private void OnScaleReturn()
        {
            DoUpdate();
            _inExtendOpen = false;


            //TurnOffKeepOpen();


            // 显示主框体，隐藏缩放框体
            if (!_main_container.gameObject.activeSelf)
            {
                _main_container.gameObject.SetActive(true);
            }

            if (_scaleAgent.gameObject.activeSelf)
            {
                // 可考虑直接销毁
                Destroy(_scaleAgent.gameObject);

                //_scaleAgent.gameObject.SetActive(false);
            }
        }

        // 当点击缩放关闭
        private void OnScaleClose()
        {
            InExtendOpen = false;
            //TurnOffKeepOpen();
            //关闭卡片
            DoDestoriedForFirstStep();

        }

        private void OnScaleUpdate()
        {
            DoUpdate();
        }

        private void OnScaleOpen()
        {
            InExtendOpen = true;
            DoUpdate();

            //TurnOnKeepOpen();
        }



        public void DoOnCreatedCompleted()
        {
            OnCreatedCompletedAction?.Invoke();
        }

        protected void SetOnCreatedCompleted(Action action)
        {
            OnCreatedCompletedAction = action;
        }


        #region 视频播放功能

        public void DoVideo(string address, string description, string cover)
        {
            //显示 video 的框框
            OpenVideoContainer(address, description, cover);

            // 隐藏平时的框框
            HideNormalContainer();

        }

        public void DoCloseVideoContainer()
        {
            OpenNormalContainer();

            CloseVideoContainer();

            DoUpdate();

            //TurnOffKeepOpen();
        }


        //
        //  关闭视频
        //
        private void CloseVideoContainer()
        {
            _cardStatus = CardStatusEnum.NORMAL;
            videoContainer.gameObject.SetActive(false);
            _videoAgent?.DoDestory();
            Destroy(_videoAgent?.gameObject);
            DoUpdate();
        }

        // 显示普通的窗口
        private void OpenNormalContainer()
        {
            normalContainer.gameObject.SetActive(true);
        }

        // 隐藏普通的窗口
        private void HideNormalContainer()
        {
            normalContainer.gameObject.SetActive(false);
        }

        // 显示 Video 的窗口
        private void OpenVideoContainer(string address, string description, string cover)
        {
            _cardStatus = CardStatusEnum.HIDE;
            videoContainer.gameObject.SetActive(true);
            _videoAgent = Instantiate(videoAgentPrefab, videoContainer);
            _videoAgent.SetData(address, description, this, cover,()=> {
                //Debug.Log("video agent is update");
                //Debug.Log("current free time : " + GetFreeTime());
                DoUpdate();                
            });
            _videoAgent.Init();
        }
        #endregion

        /// <summary>
        /// 搜索功能
        /// </summary>
        public void DoSearch()
        {
            // 生成搜索angent

            _searchAgent = Instantiate(_searchAgentPrefab,
                                    _searchContainer
                                    ) as SearchAgent;

            //  设置 manager 
            _searchAgent.Init();
            _searchAgent.InitData(_manager, this);
            _searchAgent.OnClickReturn(OnClickSearchReturnBtn);
            _searchAgent.OnClickMove(OnClickSearchReturnMoveBtn);
            _searchAgent.OnUpdate(OnSearchAgentUpdated);
            DoUpdate();
            _inExtendOpen = true;

            // 显示缩放框体，隐藏普通框体
            if (_main_container.gameObject.activeSelf)
            {
                _main_container.gameObject.SetActive(false);
            }

            if (!_searchAgent.gameObject.activeSelf)
            {
                _searchAgent.gameObject.SetActive(true);
            }
        }

        public void RecoverContainerAfterSearch()
        {
            _main_container.gameObject.SetActive(true);
        }



        private void OnClickSearchReturnBtn()
        {
            if (!_main_container.gameObject.activeSelf)
            {
                _main_container.gameObject.SetActive(true);
            }

            if (_searchAgent.gameObject.activeSelf)
            {
                _searchAgent.gameObject.SetActive(false);
            }

            DoUpdate();
            _inExtendOpen = false;

            //TurnOffKeepOpen();
        }

        private void OnSearchAgentUpdated() {
            DoUpdate();
        }

        private void OnClickSearchReturnMoveBtn()
        {
            DoMove();
        }


        /// <summary>
        /// 卡片走向前台
        /// </summary>
        public void GoToFront(Action onFinsihed)
        {
            RectTransform rectTransfrom = GetComponent<RectTransform>();
            gameObject.SetActive(true);

            Vector3 to2 = new Vector3(rectTransfrom.anchoredPosition.x, rectTransfrom.anchoredPosition.y, 0);
            var cardGoToFrontMoveAni = GetComponent<RectTransform>().DOAnchorPos3D(to2, 0.3f);
            _tweenerManager.Add(FlockTweenerManager.Card_GoToFront_Move, cardGoToFrontMoveAni);

            Vector3 scaleVector3 = new Vector3(1f, 1f, 1f);

            var cardGoToFrontScaleAni = GetComponent<RectTransform>()
                .DOScale(scaleVector3, 0.5f)
                .OnUpdate(() =>
                {
                    _width = GetComponent<RectTransform>().sizeDelta.x; ;
                    _height = GetComponent<RectTransform>().sizeDelta.y;
                    _hasChangeSize = true;

                }).OnComplete(() =>
                {
                    // 完成生成后，碰撞体再改变
                    _hasChangeSize = true;

                    // 执行完成后动画
                    DoOnCreatedCompleted();

                    // 进行完整显示
                    FullDisplayAfterGoFront();

                    CardStatus = CardStatusEnum.NORMAL;

                    onFinsihed.Invoke();

                }).SetEase(Ease.OutBack);
            _tweenerManager.Add(FlockTweenerManager.Card_GoToFront_Scale, cardGoToFrontScaleAni);
        }

        public virtual void FullDisplayAfterGoFront()
        {
            //Debug.Log("Do In Parent");
        }




        public void CancelGoToFront(Action onFinsihed)
        {
            var cardGoToFrontMoveAni = _tweenerManager.Get(FlockTweenerManager.Card_GoToFront_Move);
            var cardGoToFrontScaleAni = _tweenerManager.Get(FlockTweenerManager.Card_GoToFront_Scale);

            cardGoToFrontMoveAni.Kill();
            cardGoToFrontScaleAni.Kill();
            _cardStatus = CardStatusEnum.NORMAL;

        }

        /// <summary>
        ///     更新碰撞体半径
        /// </summary>
        private void UpdateColliderRadius()
        {
            // 当打开完成后，形成碰撞体
            // 移动过程中，不需要碰撞体
            if (_cardStatus == CardStatusEnum.MOVE)
            {
                _collider.radius = 0;
            }
            else
            {
                if (_hasChangeSize)
                {
                    float width = GetComponent<RectTransform>().rect.width;
                    float height = GetComponent<RectTransform>().rect.height;
                    float radius = (Mathf.Sqrt(width * width + height * height) / 2) * 0.6f;
                    _collider.radius = radius;
                    _hasChangeSize = false;
                    //Debug.Log("UpdateColliderRadius! " + radius);
                }
            }
        }


        #region Business Card 相关

        // 生成企业卡片
        private void InitEnvCard(List<ExtraCardData> extraCardDatas)
        {
            if (!hasInitBusinessCard)
            {

                //  创建 Agent
                businessCardAgent = Instantiate(
                                            _business_card_prefab,
                                            _business_card_container
                                            ) as BusinessCardAgent;
                int envId;

                // 需要获取企业ID
                Vector2 position = GetComponent<RectTransform>().anchoredPosition;


                string[] addressAry = new string[extraCardDatas.Count];
                for (int i = 0; i < extraCardDatas.Count; i++)
                {
                    addressAry[i] = extraCardDatas[i].Cover;
                }


                businessCardAgent.Init(addressAry, GetComponent<RectTransform>().rect.width
                    , position, OnHandleBusinessUpdate, OnClickBusinessCardClose, OnBusinessCardUpdated);

                businessCardAgent.gameObject.SetActive(false);
                hasInitBusinessCard = true;
            }
        }

        public void CloseBusinessCard()
        {            
            businessCardAgent.gameObject.SetActive(false);
            _inExtendOpen = false;
            _showDetail = false;
        }

        public void OpenBusinessCard()
        {
            InitEnvCard(_extraCardDatas);            
            businessCardAgent.gameObject.SetActive(true);
            _inExtendOpen = true;
            _showDetail = true;
        }


        /// <summary>
        /// 
        /// </summary>
        private void OnClickBusinessCardClose()
        {
            CloseBusinessCard();
        }

        private void OnHandleBusinessUpdate()
        {
            DoUpdate();
        }

        #endregion


        #region 移动功能
        public void DoMove()
        {
            DoUpdate();

            //Debug.Log("DO MOVE BUTTON");


            // 移动
            if (_cardStatus == CardStatusEnum.NORMAL)
            {
                DoUpdate();

                _cardStatus = CardStatusEnum.MOVE;
                _moveAgent.Show();

                NotifyObserver();

            }
            else if (_cardStatus == CardStatusEnum.MOVE)
            {
                DoUpdate();
                _cardStatus = CardStatusEnum.NORMAL;
                _moveAgent.Hide();

                NotifyObserver();
            }

        }




        // 移动
        private void Move(PointerEventData eventData)
        {

            Vector3 worldPoint;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(parentRtf, eventData.position, eventData.pressEventCamera, out worldPoint);
            Vector3 targetPoint = worldPoint + offset;

            Vector2 size = new Vector2(GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height);
            //print("_panel_left:" + _panel_left + "---_panel_right:" + _panel_right + "---targetPoint:" + targetPoint);
            //print("_panel_top:" + _panel_top + "---_panel_bottom:" + _panel_bottom + "---targetPoint:" + targetPoint.y);
            if (targetPoint.x < _panel_left + size.x / 2)
            {
                targetPoint.x = _panel_left + size.x / 2;
            }
            else if (targetPoint.x > _panel_right - size.x / 2)
            {
                targetPoint.x = _panel_right - size.x / 2;
            }
            if (targetPoint.y < size.y / 2)
            {
                targetPoint.y = size.y / 2;
            }
            else if (targetPoint.y > _panel_top - size.y / 2)
            {
                targetPoint.y = _panel_top - size.y / 2;
            }

            transform.position = targetPoint;

            /*
            Vector2 nowPostion = GetComponent<RectTransform>().anchoredPosition;

            Vector2 to;
            Vector2 position = eventData.position;


            // 将点击的屏幕坐标转换为移动的目的坐标

            //float x = Screen.width - _manager.OperationPanel

            //Vector3 screenPos = Camera.main.WorldToScreenPoint(_manager.OperationPanel.position);  // Camera Render - camera
            Vector3 screenPos = _manager.OperationPanel.position;   // Camera Render - Overlay

            // 操作框体左下角屏幕坐标
            Vector2 rawanchorPositon = new Vector2(screenPos.x - _manager.OperationPanel.rect.width / 2.0f
            , screenPos.y - _manager.OperationPanel.rect.height / 2.0f);

            //nowPostion = nowPostion + rawanchorPositon;

            //position = position - rawanchorPositon;

            bool overleft = position.x < (_panel_left + _safe_distance_width);
            bool overright = position.x > (_panel_right - _safe_distance_width);
            bool overtop = position.y > (_panel_top - _safe_distance_height);
            bool overbottom = position.y < (_panel_bottom + _safe_distance_height);

            bool isOver = false;


            if (overleft || overright)
            {
                print("overleft : " + nowPostion);

                to.x = nowPostion.x;
                print(nowPostion.x);

                isOver = true;
            }
            else
            {
                to.x = position.x;
            }

            if (overtop || overbottom)
            {
                //print("overtop : " + nowPostion);
                //print("position : " + position);


                to.y = nowPostion.y;

                isOver = true;
            }
            else
            {
                to.y = position.y;
            }

        */

            // 获取鼠标坐标与卡片坐标的偏移


            //if (isOver)
            //{
            //    GetComponent<RectTransform>().anchoredPosition = to;
            //    Debug.Log(11111111);
            //}
            //else {
            //    GetComponent<RectTransform>().anchoredPosition = to - _moveStartOffset;
            //}

            //GetComponent<RectTransform>().DOAnchorPos(to, 1.5f);

        }

        #endregion


        #region 提示内容
        public void DoQuestion()
        {
            if (CardStatus == CardStatusEnum.NORMAL) {
                if (_showQuestion)
                {
                    _questionAgent?.CloseReminder();
                    _showQuestion = false;

                    _inExtendOpen = false;
                }
                else
                {
                    _questionAgent = Instantiate(_questionAgentPrefab, _question_container);
                    _questionAgent.Init(OnQuestionClose);

                    _questionAgent.ShowReminder(_questionTypeEnum);
                    _showQuestion = true;

                    _inExtendOpen = true;
                }
                DoUpdate();
            }
        }

        private void OnQuestionClose()
        {
            DoUpdate();
            _inExtendOpen = false;
            _showQuestion = false;
        }

        #endregion



        private Vector3 offset;

        #region Event System
        public void OnBeginDrag(PointerEventData eventData)
        {
            //Debug.Log("OnBeginDrag : " + eventData.position);
            if (_cardStatus == CardStatusEnum.MOVE) {
                _moveStartPosition = eventData.position;
                _moveStartOffset = _moveStartPosition - GetComponent<RectTransform>().anchoredPosition;

                Vector3 worldPoint;
                RectTransformUtility
                    .ScreenPointToWorldPointInRectangle(parentRtf, eventData.position, eventData.pressEventCamera, out worldPoint);
                offset = transform.position - worldPoint;

                DoUpdate();
            }

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_cardStatus == CardStatusEnum.MOVE)
            {
                DoMove();

                _hasChangeSize = true;

                DoUpdate();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_cardStatus == CardStatusEnum.MOVE)
            {
                Move(eventData);

                //拖拽时不碰撞
                transform.SetAsLastSibling();

                DoUpdate();
            }
        }

        /// <summary>
        /// Event system : 点击事件
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {

            if (_cardStatus == CardStatusEnum.MOVE)
            {
                //取消移动    
                DoMove();

                _moveStartPosition = Vector2.zero;


                GetComponent<Rigidbody2D>().WakeUp();

            }
            else {

            }

            DoUpdate();


        }
        #endregion


        // 新打开的卡片会挤开已有的卡片

        void OnTriggerEnter2D(Collider2D col)
        {
            ToDoMoved(col);

        }
        // 当持续在触发范围内发生时调用
        void OnTriggerStay2D(Collider2D other)
        {
            ToDoMoved(other);
        }


        // 离开触发范围会调用一次
        void OnTriggerExit2D(Collider2D other)
        {
            List<Collider2D> results = new List<Collider2D>();
            Physics2D.OverlapCollider(GetComponent<Collider2D>(), new ContactFilter2D(), results);

            if (results.Count == 0)
            {
                _isPhysicsMoving = false;
                GetComponent<Rigidbody2D>().Sleep();
            }

        }


        private void ToDoMoved(Collider2D other)
        {
            if (other.gameObject.layer == 5)
            {
                GetComponent<Rigidbody2D>().WakeUp();

                CardAgent _refCardAgent = other.gameObject.GetComponent<CardAgent>();

                if (IsDoMoved(_refCardAgent))
                {
                    MoveInvoker(_refCardAgent);
                }
            }
        }


        /// <summary>
        /// 判断是否是本 card 进行移动
        /// </summary>
        /// <param name="cardAgent"></param>
        /// <returns></returns>
        private bool IsDoMoved(CardAgent cardAgent)
        {
            int this_index = _manager.operateCardManager.EffectAgents.IndexOf(this);
            int effect_index = _manager.operateCardManager.EffectAgents.IndexOf(cardAgent);

            if (this_index < effect_index)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void MoveInvoker(CardAgent refCardAgent)
        {
            if (!_isPhysicsMoving)
            {
                _isPhysicsMoving = true;

                // 获取相对的位置
                Vector2 selfPosition = GetComponent<RectTransform>().anchoredPosition;
                Vector2 refPosition = refCardAgent.GetComponent<RectTransform>().anchoredPosition;
                Vector2 to = (selfPosition - refPosition).normalized;

                Vector2 to2 = to * 100 * _physicesEffectFactor;

                GetComponent<Rigidbody2D>().AddForce(to2);
            }
        }

        public void AddObserver(MoveBtnObserver observer)
        {
            _moveBtnObservers.Add(observer);
        }

        public void RemoveObserver(MoveBtnObserver observer)
        {
            _moveBtnObservers?.Remove(observer);
        }

        public void NotifyObserver()
        {
            for (int i = 0; i < _moveBtnObservers.Count; i++)
            {
                _moveBtnObservers[i].Update();
            }

        }


        /// <summary>
        ///     获取空闲时间,
        ///     当 agent 处于 inExtendOpen 的状态下，总体时间除以二
        /// </summary>
        public float GetFreeTime() {

            float freeTime = Time.time - _recentActiveTime;

            if (_inExtendOpen)
            {
                freeTime = freeTime / 2f;
            }

            return freeTime;
        }

        /// <summary>
        /// 到达开始删除的时间
        /// </summary>
        /// <returns></returns>
        private bool EnterToDestoryTime() {
            //return false;

            float waitTime;

            //if(_cardStatus == CardStatusEnum.MOVE 
            //    || )

            if (_inExtendOpen) {
                waitTime = _manager.managerConfig.OperateCardAutoCloseTime * 3;
            } else {
                waitTime = _manager.managerConfig.OperateCardAutoCloseTime;
            }

            if ((!_keepOpen) && ((Time.time - _recentActiveTime) > waitTime)) {
                return true;
            }
            return false;
        }


        private void ProtectUpdatedInToDestory() {
            DoRecover();
        }

        private void OnBusinessCardUpdated() {
            DoUpdate();
        }


        /* UI 设置 */
        private void UpdateUI() {

            if (_manager.screenTypeEnum == ScreenTypeEnum.Screen1080P)
            {
                //800 
                float rectHeight = 1000f;
                GetComponent<RectTransform>().sizeDelta = new Vector2(rectHeight, rectHeight);
            }
            else {
                Debug.Log("Screen720P");
                float rectHeight = _manager.mainPanel.rect.height * _heightFactor;
                GetComponent<RectTransform>().sizeDelta = new Vector2(rectHeight, rectHeight);
            }
            //// 初始化框体长宽
            //float rectHeight = manager.mainPanel.rect.height * _heightFactor;
            //GetComponent<RectTransform>().sizeDelta = new Vector2(rectHeight, rectHeight);
        }





        /* CollisionEffectAgent impl 实现 */

        public Vector3 GetRefPosition()
        {            
            var pos = GetComponent<RectTransform>().position;
            var screenPosition = RectTransformUtility.WorldToScreenPoint(null, pos);

            //var sposition = _manager.mainCamera.WorldToScreenPoint(pos);

            //Debug.Log(gameObject.name + " | position : " + pos + " - get ref position : " + screenPosition);

            return screenPosition;
        }


        public bool IsEffective()
        {
            
            if (!gameObject.activeSelf)
            {
                return false;
            }

            if (_cardStatus == CardStatusEnum.HIDE
                || _cardStatus == CardStatusEnum.OBSOLETE || _disableEffect)
            {
                return false;
            }


            float effect_width = 300f;
            float effect_height = 300f;

            Vector3 scaleVector3 = GetComponent<RectTransform>().localScale;
            float width = GetComponent<RectTransform>().rect.width;
            float height = GetComponent<RectTransform>().rect.height;

            if (width > effect_width && height > effect_height)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public float GetWidth()
        {

            Vector3 scaleVector3 = GetComponent<RectTransform>().localScale;

            //Debug.Log("card Agent width : " + width + " / scale : " + scaleVector3);

            return width * scaleVector3.x;
        }

        public float GetHeight()
        {
            Vector3 scaleVector3 = GetComponent<RectTransform>().localScale;
            return height* scaleVector3.y;
        }


        public void SetMoveBehavior(ICollisionMoveBehavior moveBehavior)
        {            
            _collisionMoveBehavior = moveBehavior;
        }

        public ICollisionMoveBehavior GetMoveBehavior()
        {
            return _collisionMoveBehavior;
        }

        public string GetName()
        {
            return gameObject.name;
        }

        public float GetEffectDistance()
        {

            CollisionMoveBehaviourFactory collisionMoveBehaviourFactory = GameObject.Find("Collision").GetComponent<CollisionMoveBehaviourFactory>();
            var influenceMoveFactor =  collisionMoveBehaviourFactory.GetMoveEffectDistance();

            var effectDistance = GetWidth() * influenceMoveFactor;
            return effectDistance;
        }

        public void SetDisableEffect(bool disableEffect)
        {
            _disableEffect = disableEffect;            
        }

        /* CollisionEffectAgent 实现 结束*/


    }


}