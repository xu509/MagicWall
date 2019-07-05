using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class CardAgent : FlockAgent,IBeginDragHandler, IEndDragHandler, IDragHandler,IPointerClickHandler
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

    private float _panel_top;
    private float _panel_bottom;
    private float _panel_left;
    private float _panel_right;
    private float _safe_distance_width;
    private float _safe_distance_height;
    private CardRecoverStatus _cardRecoverStatus = CardRecoverStatus.Init;

    /// <summary>
    /// 第一次销毁动画的 DoTween 代理
    /// </summary>
    private Tweener _destory_first_scale_tweener;


    protected CardStatusEnum _cardStatus;   // 状态   
    protected FlockAgent _originAgent;  // 原组件

    protected bool _hasListBtn;  //  有列表按钮
    protected bool hasInitBusinessCard = false; // 是否已生成business card
    protected BusinessCardAgent businessCardAgent;

    [SerializeField,Range(0f,3f)] float _widthFactor;  //    宽度比例，如当高度100，宽度50时，则宽度比 0.5
    [SerializeField,Range(0f,1f)] float _heightFactor; //      高度比例，如当屏幕高度100，卡片高度50时，则高度比为 0.5
    [SerializeField] RectTransform _main_container;    //  主框体
    [SerializeField] RectTransform _scale_container;    //  缩放容器
    [SerializeField] ScaleAgent _scale_prefab;    //  缩放 prefab
    [SerializeField] RectTransform _searchContainer;    //  搜索容器
    [SerializeField] SearchAgent _searchAgentPrefab;    //  搜索 prefab
    [SerializeField] RectTransform _move_mask; // 移动蒙板
    [SerializeField] RectTransform _move_reminder_container; // 移动提醒容器
    [SerializeField] RectTransform _business_card_container;    // 企业卡片容器
    [SerializeField] BusinessCardAgent _business_card_prefab;    // 企业卡片 control
    [SerializeField] RectTransform _tool_bottom_container; //   按钮工具栏（4项）
    [SerializeField] RectTransform _tool_bottom_three_container; //   按钮工具栏（3项）
    [SerializeField] CircleCollider2D _collider;    // 圆形碰撞体
    [SerializeField] VideoAgent videoAgentPrefab;   // Video Agent prefab
    [SerializeField] RectTransform normalContainer; // 正常显示的框体
    [SerializeField] RectTransform videoContainer;  // video的安放框体
    [SerializeField] float radiusFactor;// Colider 半径系数
    [SerializeField] Sprite _move_icon_active;
    [SerializeField] Sprite _move_icon;
    [SerializeField] RectTransform _btn_move_container;
    [SerializeField] RectTransform _btn_move_container_in_three;


    Action OnCreatedCompletedAction;

    /// <summary>
    /// 卡片恢复状态
    /// </summary>
    enum CardRecoverStatus {
        Init,Recovering
    }

    public CardStatusEnum CardStatus {
        set { _cardStatus = value; }
        get { return _cardStatus; }
    }

    public FlockAgent OriginAgent
    {
        set { _originAgent = value; }
        get { return _originAgent; }
    }

    #endregion

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
    public void InitCardData(MagicWallManager manager,int dataId,MWTypeEnum dataType,
        Vector3 genPosition,Vector3 scaleVector3,FlockAgent originAgent)
    {
        InitBase(manager, dataId, dataType, true);

        // 初始化框体长宽
        float rectHeight = manager.mainPanel.rect.height * _heightFactor;
        float rectWidth = rectHeight * _widthFactor;
        GetComponent<RectTransform>().sizeDelta = new Vector2(rectWidth, rectHeight);

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

    }


    //
    //  Init 代理
    //
    protected void InitAgency() {
        _recentActiveTime = Time.time;
        _cardStatus = CardStatusEnum.NORMAL;

        _panel_top = -(_manager.OperationPanel.rect.yMin) + _manager.OperationPanel.rect.yMax;
        _panel_bottom = 0;
        _panel_left = 0;
        _panel_right = -(_manager.OperationPanel.rect.xMin) + _manager.OperationPanel.rect.xMax;

        _safe_distance_width = GetComponent<RectTransform>().rect.width / 3;
        _safe_distance_height = GetComponent<RectTransform>().rect.height / 3;

        _agentManager = _manager.agentManager;
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
    public void DoUpdate() {
        if (CardStatus == CardStatusEnum.NORMAL) {
            _recentActiveTime = Time.time;
        }
        else if (CardStatus == CardStatusEnum.DESTORING_STEP_FIRST) {
            DoRecover();
        }
    }
    #endregion

    #region Private Methods

    /// <summary>
    /// 销毁第一阶段
    /// </summary>
    private void DoDestoriedForFirstStep() {
        //  缩放至2倍大
        Vector3 scaleVector3 = new Vector3(0.7f, 0.7f, 0.7f);
        _cardStatus = CardStatusEnum.DESTORING_STEP_FIRST;

        _destory_first_scale_tweener = GetComponent<RectTransform>().DOScale(scaleVector3, 2f)
            .OnUpdate(() =>
             {
                    Width = GetComponent<RectTransform>().sizeDelta.x;
                    Height = GetComponent<RectTransform>().sizeDelta.y;
                    _hasChangeSize = true;
             }).OnComplete(() => {
                   IsRecovering = false;
                   IsChoosing = true;

                   // 设置第一次缩小的点
                   _destoryFirstStageCompleteTime = Time.time;
             }).OnKill(() => {
             });

    }

    //
    //  第二步的销毁
    //
    private void DoDestoriedForSecondStep()
    {
        _cardStatus = CardStatusEnum.DESTORYING_STEP_SCEOND;

        //  如果场景没有变，则回到原位置
        if (SceneIndex == _manager.SceneIndex && _originAgent != null)
        {
            //恢复并归位
            // 缩到很小很小
            RectTransform rect = GetComponent<RectTransform>();

            //  移到后方、缩小、透明
            rect.DOScale(0.2f, 1f);

            //  获取位置
            FlockAgent oriAgent = _originAgent;
            Vector3 to = new Vector3(oriAgent.OriVector2.x - _manager.PanelOffsetX
                , oriAgent.OriVector2.y - _manager.PanelOffsetY, 200);

            rect.DOAnchorPos3D(to, 1f)
                .OnComplete(() => {
                    //  使卡片消失na 
                    OriginAgent.DoRecoverAfterChoose();
                    _agentManager.RemoveItemFromEffectItems(this);
            });
        }
        //  直接消失
        else
        {
            // 慢慢缩小直到消失
            Vector3 vector3 = Vector3.zero;

            GetComponent<RectTransform>().DOScale(vector3, 1.5f)
                .OnUpdate(() => {
                    Width = GetComponent<RectTransform>().sizeDelta.x;
                    Height = GetComponent<RectTransform>().sizeDelta.y;
                })
                .OnComplete(() => DoDestoryOnCompleteCallBack(this));
        }

        _cardStatus = CardStatusEnum.DESTORYED;
    }

    //
    //  恢复
    //
    private void DoRecover()
    {
        if (_cardRecoverStatus == CardRecoverStatus.Init) {
            Debug.Log("恢复动画开始");
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
               }).OnComplete(() => {
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
        else {
            CloseBusinessCard();
        }
    }

    public void DoMove()
    {        
        // 移动
        if (!_doMoving)
        {
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


        UpdateMoveBtnPerformance();
    }

    // 生成缩放卡片
    public void InitScaleAgent(Texture texture) {
        _scaleAgent = Instantiate(_scale_prefab,
                                      _scale_container
                                        ) as ScaleAgent;
        _scaleAgent.SetImage(texture);
        _scaleAgent.SetOnCloseClicked(OnScaleClose);
        _scaleAgent.SetOnReturnClicked(OnScaleReturn);
        _scaleAgent.SetOnUpdated(OnScaleUpdate);

        // 显示缩放框体，隐藏普通框体
        if (_main_container.gameObject.activeSelf) {
            _main_container.gameObject.SetActive(false);
        }

        if (!_scaleAgent.gameObject.activeSelf) {
            _scaleAgent.gameObject.SetActive(true);
        }

    }



    //  初始化组件显示的状态
    protected void InitComponents(bool hasListBtn) {
        _hasListBtn = hasListBtn;
        if (_hasListBtn)
        {
            // 显示四组按钮
            _tool_bottom_container.gameObject.SetActive(true);
            _tool_bottom_three_container.gameObject.SetActive(false);
            InitEnvCard();
        }
        else {
            // 显示三个按钮
            _tool_bottom_container.gameObject.SetActive(false);
            _tool_bottom_three_container.gameObject.SetActive(true);

        }

        // 不显示移动提示
        _move_reminder_container.gameObject.SetActive(false);
        // 关闭移动蒙版
        _move_mask.gameObject.SetActive(false);

    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        DoUpdate();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_doMoving) {
            //松手后碰撞
            CircleCollider2D[] circles = FindObjectsOfType<CircleCollider2D>();
            foreach (CircleCollider2D circle in circles)
            {
                circle.radius = radiusFactor;
            }

            DoMove();
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

    // 当点击缩放收回
    private void OnScaleReturn() {
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
        //关闭卡片
        DoDestoriedForFirstStep();

    }

    private void OnScaleUpdate()
    {
        DoUpdate();
    }


    // 移动
    private void Move(PointerEventData eventData) {
        Vector2 nowPostion = GetComponent<RectTransform>().anchoredPosition;

        Vector2 to;
        Vector2 position = eventData.position;

        // 将点击的屏幕坐标转换为移动的目的坐标

        //float x = Screen.width - _manager.OperationPanel

        Vector3 screenPos = Camera.main.WorldToScreenPoint(_manager.OperationPanel.position);
        // 操作框体左下角屏幕坐标
        Vector2 rawanchorPositon = new Vector2(screenPos.x - _manager.OperationPanel.rect.width / 2.0f
        , screenPos.y - _manager.OperationPanel.rect.height / 2.0f);

        // 获取偏移后的移动坐标
        position = position - rawanchorPositon;

        bool overleft = position.x < (_panel_left + _safe_distance_width);
        bool overright = position.x > (_panel_right - _safe_distance_width);
        bool overtop = position.y > (_panel_top - _safe_distance_height);
        bool overbottom = position.y < (_panel_bottom + _safe_distance_height);

        if (overleft || overright) {
            to.x = nowPostion.x;
        } else {
            to.x = position.x;
        }

        if (overtop || overbottom)
        {
            to.y = nowPostion.y;
        }
        else {
            to.y = position.y;
        }



        // 缓慢移动,1.5f 代表拖拽移动延迟
        GetComponent<RectTransform>().DOAnchorPos(to, 1.5f);

    }

    /// <summary>
    /// Event system : 点击事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        DoUpdate();
    }


    public void TurnOffKeepOpen() {
        DoUpdate();
        _keepOpen = false;
    }


    public void DoOnCreatedCompleted() {
        OnCreatedCompletedAction?.Invoke();
    }

    protected void SetOnCreatedCompleted(Action action) {
        OnCreatedCompletedAction = action;
    }


    #region 视频播放功能

    public void DoVideo(string address, string description,string cover)
    {
        //显示 video 的框框
        OpenVideoContainer(address, description,cover);

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
    private void OpenVideoContainer(string address, string description,string cover)
    {
        videoContainer.gameObject.SetActive(true);
        _videoAgent = Instantiate(videoAgentPrefab, videoContainer);
        _videoAgent.SetData(address, description, this,cover);
        _videoAgent.Init();
    }
    #endregion

    /// <summary>
    /// 搜索功能
    /// </summary>
    public void DoSearch() {
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

    private void OnClickSearchReturnBtn() {
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

    private void OnClickSearchReturnMoveBtn() {
        DoMove();
    }


    /// <summary>
    /// 卡片走向前台
    /// </summary>
    public void GoToFront() {
        RectTransform rectTransfrom = GetComponent<RectTransform>();

        gameObject.SetActive(true);

        Vector3 to2 = new Vector3(rectTransfrom.anchoredPosition.x, rectTransfrom.anchoredPosition.y, 0);
        GetComponent<RectTransform>().DOAnchorPos3D(to2, 0.3f);

        Vector3 scaleVector3 = new Vector3(1f, 1f, 1f);

        //float w = _cardAgent.GetComponent<RectTransform>().sizeDelta.x;
        //float h = _cardAgent.GetComponent<RectTransform>().sizeDelta.y;

        GetComponent<RectTransform>()
            .DOScale(scaleVector3, 0.5f)
            .OnUpdate(() =>
            {
                Width = GetComponent<RectTransform>().sizeDelta.x; ;
                Height = GetComponent<RectTransform>().sizeDelta.y;

            }).OnComplete(() => {
                // 完成生成后，碰撞体再改变
                _hasChangeSize = true;

                // 执行完成后动画
                DoOnCreatedCompleted();

            }).SetEase(Ease.OutBack);
    }

    /// <summary>
    ///     更新碰撞体半径
    /// </summary>
    private void UpdateColliderRadius() {
        // 当打开完成后，形成碰撞体
        // 移动过程中，不需要碰撞体
        if (_doMoving)
        {
            _collider.radius = 0;
        }
        else {

            if (_hasChangeSize) {
                float width = GetComponent<RectTransform>().rect.width;
                float height = GetComponent<RectTransform>().rect.width;
                float radius = Mathf.Sqrt(width * width + height * height) / 2;
                _collider.radius = radius;
                _hasChangeSize = false;
            }
            
        }
    }

    /// <summary>
    /// 更新移动的表现形式
    /// </summary>
    private void UpdateMoveBtnPerformance() {

        if (_doMoving)
        {
            if (_hasListBtn)
            {
                _btn_move_container.GetComponent<Image>().sprite = _move_icon_active;
            }
            else {
                _btn_move_container_in_three.GetComponent<Image>().sprite = _move_icon_active;
            }
        }
        else {
            if (_hasListBtn)
            {
                _btn_move_container.GetComponent<Image>().sprite = _move_icon;
            }
            else
            {
                _btn_move_container_in_three.GetComponent<Image>().sprite = _move_icon;
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

            List<string> address = DaoService.Instance.GetEnvCards(DataId);
            Vector2 position = GetComponent<RectTransform>().anchoredPosition;
            businessCardAgent.Init(address.ToArray(),GetComponent<RectTransform>().rect.width
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

}


