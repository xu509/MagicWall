using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MWMagicWall;

public class CardAgent : FlockAgent, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler, MoveSubject
{
    #region Parameter
    private float _recentActiveTime = 0f;   //  最近次被操作的时间点
    private float _destoryFirstStageCompleteTime = 0f;   //  第一阶段关闭的时间点
    private float _activeFirstStageDuringTime = 7f;   //  最大的时间
    private float _activeSecondStageDuringTime = 2f;   //  第二段缩小的时间
    private bool _showDetail = false;   //  显示企业卡片
    private bool _doMoving = false;     //  移动
    private bool _hasChangeSize = false;    //  大小改变的标志位
    private bool _keepOpen = false;     //  保持开启
    protected bool KeepOpen { set { _keepOpen = value; } get { return _keepOpen; } }

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


    protected CardStatusEnum _cardStatus;   // 状态   
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

    [SerializeField, Header("Move")] RectTransform _move_mask; // 移动蒙板
    [SerializeField] RectTransform _move_reminder_container; // 移动提醒容器
    [SerializeField] RectTransform _btn_move_container;
    [SerializeField] RectTransform _btn_move_container_in_three;
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

    /// <summary>
    ///     初始化卡片类型浮动块数据
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="dataId"></param>
    /// <param name="dataType"></param>
    /// <param name="genPosition">生成位置</param>
    /// <param name="scaleVector3">缩放比例</param>
    /// <param name="originAgent">原关联的浮块</param>
    public void InitCardData(MagicWallManager manager, int dataId, MWTypeEnum dataType,
        Vector3 genPosition, Vector3 scaleVector3, FlockAgent originAgent)
    {
        InitBase(manager, dataId, dataType, true);

        // 初始化框体长宽
        float rectHeight = manager.mainPanel.rect.height * _heightFactor;
        GetComponent<RectTransform>().sizeDelta = new Vector2(rectHeight, rectHeight);

        //  命名
        if (originAgent != null)
        {
            name = dataType.ToString() + "(" + originAgent.name + ")";

            //  添加原组件
            OriginAgent = originAgent;
        }

        //  定出生位置
        GetComponent<RectTransform>().anchoredPosition3D = genPosition;

        //  配置scene
        SceneIndex = _manager.SceneIndex;

        GetComponent<RectTransform>().localScale = scaleVector3;

        //  初始化长宽字段
        Width = GetComponent<RectTransform>().rect.width;
        Height = GetComponent<RectTransform>().rect.height;

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

        _agentManager = _manager.agentManager;

        DoCardReset();
    }

    //
    //  Update 代理
    //
    protected void UpdateAgency()
    {
        if (!_keepOpen && !_doMoving)
        {
            //缩小一半
            if (_cardStatus == CardStatusEnum.NORMAL)
            {
                if ((Time.time - _recentActiveTime) > _activeFirstStageDuringTime)
                {
                    DoDestoriedForFirstStep();
                }
            }

            // 第二次缩小
            if (_cardStatus == CardStatusEnum.DESTORING_STEP_FIRST)
            {
                if (_destoryFirstStageCompleteTime != 0 && (Time.time - _destoryFirstStageCompleteTime) > _activeSecondStageDuringTime)
                {
                    DoDestoriedForSecondStep();
                }
            }
        }

        UpdateColliderRadius();
    }


    /// <summary>
    ///     更新开关,保持卡片被操作时不会自动关闭
    /// </summary>
    public void DoUpdate()
    {
        if (CardStatus == CardStatusEnum.NORMAL)
        {
            _recentActiveTime = Time.time;
        }
        else if (CardStatus == CardStatusEnum.DESTORING_STEP_FIRST)
        {
            DoRecover();
        }
    }
    #endregion

    #region Private Methods

    /// <summary>
    /// 销毁第一阶段
    /// </summary>
    private void DoDestoriedForFirstStep()
    {
        // 如果当前处于移动中，则将移动关闭
        if (_doMoving)
        {
            DoMove();
        }


        //  缩放
        Vector3 scaleVector3 = new Vector3(0.7f, 0.7f, 0.7f);
        _cardStatus = CardStatusEnum.DESTORING_STEP_FIRST;

        _destory_first_scale_tweener = GetComponent<RectTransform>().DOScale(scaleVector3, 2f)
            .OnUpdate(() =>
             {
                 Width = GetComponent<RectTransform>().sizeDelta.x;
                 Height = GetComponent<RectTransform>().sizeDelta.y;
                 _hasChangeSize = true;
             }).OnComplete(() =>
             {
                 IsRecovering = false;
                 IsChoosing = true;

                 // 设置第一次缩小的点
                 _destoryFirstStageCompleteTime = Time.time;
             }).OnKill(() =>
             {
             });

    }

    //
    //  第二步的销毁
    //
    private void DoDestoriedForSecondStep()
    {
        _cardStatus = CardStatusEnum.DESTORYING_STEP_SCEOND;

        //  如果场景没有变，则回到原位置
        if ((SceneIndex == _manager.SceneIndex) && (_originAgent != null))
        {
            //恢复并归位
            // 缩到很小很小
            RectTransform rect = GetComponent<RectTransform>();

            //  移到后方、缩小、透明
            Tweener t = rect.DOScale(0.2f, 1f)
                .OnStart(() =>
                {
                    // 关闭碰撞框
                    rect.GetComponent<CircleCollider2D>().radius = 0;
                });
            _flockTweenerManager.Add(FlockTweenerManager.CardAgent_Destory_Second_DOScale_IsOrigin, t);

            //  获取位置
            FlockAgent oriAgent = _originAgent;
            Vector3 to = new Vector3(oriAgent.OriVector2.x - _manager.PanelOffsetX
                , oriAgent.OriVector2.y - _manager.PanelOffsetY, 200);

            Tweener t2 = rect.DOAnchorPos3D(to, 1f)
                .OnComplete(() =>
                {
                    //  使卡片消失na 
                    OriginAgent.DoRecoverAfterChoose();
                    _agentManager.RemoveItemFromEffectItems(this);
                });
            _flockTweenerManager.Add(FlockTweenerManager.CardAgent_Destory_Second_DOAnchorPos3D_IsOrigin, t2);


        }
        //  直接消失
        else
        {
            // 慢慢缩小直到消失
            Vector3 vector3 = Vector3.zero;

            GetComponent<RectTransform>().DOScale(vector3, 1.5f)
                .OnUpdate(() =>
                {
                    Width = GetComponent<RectTransform>().sizeDelta.x;
                    Height = GetComponent<RectTransform>().sizeDelta.y;
                })
                .OnComplete(() =>
                {
                    _agentManager.RemoveItemFromEffectItems(this);
                });

            // 清除原来的flock

            // 搜索后点开可能存在问题
            _originAgent?.DestoryAgency();

        }

        _cardStatus = CardStatusEnum.DESTORYED;
    }

    //
    //  恢复
    //
    private void DoRecover()
    {
        if (_cardRecoverStatus == CardRecoverStatus.Init)
        {
            _cardRecoverStatus = CardRecoverStatus.Recovering;
            CardStatus = CardStatusEnum.NORMAL;

            // 停止销毁动画
            _destory_first_scale_tweener.Kill();
            Vector3 scaleVector3 = new Vector3(1f, 1f, 1f);


            // 此时会出现卡顿现象，可能是因为之前的缩小动画并未执行完毕，所以会与此语句冲突
            GetComponent<RectTransform>().DOScale(scaleVector3, 0.5f)
               .OnUpdate(() =>
               {
                   Width = GetComponent<RectTransform>().sizeDelta.x;
                   Height = GetComponent<RectTransform>().sizeDelta.y;
                   _hasChangeSize = true;
                   DoUpdate();
               }).OnComplete(() =>
               {
                   IsChoosing = false;
                   _cardRecoverStatus = CardRecoverStatus.Init;
                   DoUpdate();
               });
        }

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
        DoDestoriedForFirstStep();
    }

    // 直接进行关闭
    public void DoCloseDirect()
    {
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
    protected void InitComponents(bool hasListBtn)
    {
        _hasListBtn = hasListBtn;
        if (_hasListBtn)
        {
            // 显示四组按钮
            _tool_bottom_container.gameObject.SetActive(true);
            _tool_bottom_three_container.gameObject.SetActive(false);
            InitEnvCard();
        }
        else
        {
            // 显示三个按钮
            _tool_bottom_container?.gameObject.SetActive(false);
            _tool_bottom_three_container?.gameObject.SetActive(true);

        }

        // 不显示移动提示
        _move_reminder_container.gameObject.SetActive(false);
        // 关闭移动蒙版
        _move_mask.gameObject.SetActive(false);

    }




    // 当点击缩放收回
    private void OnScaleReturn()
    {
        TurnOffKeepOpen();


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
        TurnOffKeepOpen();
        //关闭卡片
        DoDestoriedForFirstStep();

    }

    private void OnScaleUpdate()
    {
        DoUpdate();
    }

    private void OnScaleOpen()
    {
        TurnOnKeepOpen();
    }



    public void TurnOnKeepOpen()
    {
        DoUpdate();
        _keepOpen = true;
    }

    public void TurnOffKeepOpen()
    {
        DoUpdate();
        _keepOpen = false;
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


        // 当打开视频框体时不自动关闭
        KeepOpen = true;

    }

    public void DoCloseVideoContainer()
    {
        OpenNormalContainer();

        CloseVideoContainer();

        TurnOffKeepOpen();
    }


    //
    //  关闭视频
    //
    private void CloseVideoContainer()
    {
        videoContainer.gameObject.SetActive(false);
        _videoAgent?.DoDestory();
        Destroy(_videoAgent?.gameObject);
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
        videoContainer.gameObject.SetActive(true);
        _videoAgent = Instantiate(videoAgentPrefab, videoContainer);
        _videoAgent.SetData(address, description, this, cover);
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
        _keepOpen = true;

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

        TurnOffKeepOpen();
    }

    private void OnClickSearchReturnMoveBtn()
    {
        DoMove();
    }


    /// <summary>
    /// 卡片走向前台
    /// </summary>
    public void GoToFront()
    {
        RectTransform rectTransfrom = GetComponent<RectTransform>();

        gameObject.SetActive(true);

        Vector3 to2 = new Vector3(rectTransfrom.anchoredPosition.x, rectTransfrom.anchoredPosition.y, 0);
        GetComponent<RectTransform>().DOAnchorPos3D(to2, 0.3f);

        Vector3 scaleVector3 = new Vector3(1f, 1f, 1f);

        GetComponent<RectTransform>()
            .DOScale(scaleVector3, 0.5f)
            .OnUpdate(() =>
            {
                Width = GetComponent<RectTransform>().sizeDelta.x; ;
                Height = GetComponent<RectTransform>().sizeDelta.y;

            }).OnComplete(() =>
            {
                // 完成生成后，碰撞体再改变
                _hasChangeSize = true;

                // 执行完成后动画
                DoOnCreatedCompleted();

            }).SetEase(Ease.OutBack);
    }

    /// <summary>
    ///     更新碰撞体半径
    /// </summary>
    private void UpdateColliderRadius()
    {
        // 当打开完成后，形成碰撞体
        // 移动过程中，不需要碰撞体
        if (_doMoving)
        {
            _collider.radius = 0;
        }
        else
        {

            if (_hasChangeSize)
            {
                float width = GetComponent<RectTransform>().rect.width;
                float height = GetComponent<RectTransform>().rect.width;
                float radius = Mathf.Sqrt(width * width + height * height) / 2;
                _collider.radius = radius;
                _hasChangeSize = false;
            }

        }
    }


    #region Business Card 相关

    // 生成企业卡片
    private void InitEnvCard()
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
            if (type == MWTypeEnum.Activity)
            {
                envId = _manager.daoService.GetActivityDetail(DataId).Ent_id;
            }
            else
            {
                envId = _manager.daoService.GetProductDetail(DataId).Ent_id;
            }

            List<string> address = _manager.daoService.GetEnvCards(envId);
            Vector2 position = GetComponent<RectTransform>().anchoredPosition;
            businessCardAgent.Init(address.ToArray(), GetComponent<RectTransform>().rect.width
                , position, OnHandleBusinessUpdate, OnClickBusinessCardClose);

            businessCardAgent.gameObject.SetActive(false);
            hasInitBusinessCard = true;
        }
    }

    public void CloseBusinessCard()
    {
        TurnOffKeepOpen();
        businessCardAgent.gameObject.SetActive(false);
        _showDetail = false;
    }

    public void OpenBusinessCard()
    {
        KeepOpen = true;
        businessCardAgent.gameObject.SetActive(true);

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
        // 移动
        if (!_doMoving)
        {
            DoUpdate();

            _doMoving = true;
            _move_reminder_container.gameObject.SetActive(true);
            _move_mask.gameObject.SetActive(true);
            // 调整图片


        }
        else
        {
            DoUpdate();
            _doMoving = false;
            _move_reminder_container.gameObject.SetActive(false);
            _move_mask.gameObject.SetActive(false);
        }

        NotifyObserver();

        //UpdateMoveBtnPerformance();
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
        }   else if (targetPoint.x > _panel_right - size.x / 2)
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
        if (_showQuestion)
        {
            _questionAgent?.CloseReminder();
            _showQuestion = false;
        }
        else
        {
            _questionAgent = Instantiate(_questionAgentPrefab, _question_container);
            _questionAgent.Init(OnQuestionClose);

            _questionAgent.ShowReminder(_questionTypeEnum);
            _showQuestion = true;

            TurnOnKeepOpen();
        }
    }

    private void OnQuestionClose()
    {
        TurnOffKeepOpen();
        _showQuestion = false;
    }

    #endregion



    private Vector3 offset;

    #region Event System
    public void OnBeginDrag(PointerEventData eventData)
    {
        _moveStartPosition = eventData.position;
        _moveStartOffset = _moveStartPosition - GetComponent<RectTransform>().anchoredPosition;

        Vector3 worldPoint;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(parentRtf, eventData.position, eventData.pressEventCamera, out worldPoint);
        offset = transform.position - worldPoint;

        DoUpdate();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_doMoving)
        {
            DoMove();

            _hasChangeSize = true;

            DoUpdate();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_doMoving)
        {
            Move(eventData);

            //拖拽时不碰撞
            transform.SetAsLastSibling();
        }
    }

    /// <summary>
    /// Event system : 点击事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        _moveStartPosition = Vector2.zero;

        DoUpdate();

        GetComponent<Rigidbody2D>().WakeUp();
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
        int this_index = _agentManager.EffectAgent.IndexOf(this);
        int effect_index = _agentManager.EffectAgent.IndexOf(cardAgent);

        if (this_index < effect_index)
        {
            return true;
        }
        else
        {
            return false;
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
}


