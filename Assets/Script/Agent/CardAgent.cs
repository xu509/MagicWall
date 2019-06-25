using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;

public class CardAgent : FlockAgent,IBeginDragHandler, IEndDragHandler, IDragHandler,IPointerClickHandler
{
    #region Parameter
    private float _recentActiveTime = 0f;   //  最近次被操作的时间点
    private float _destoryFirstStageCompleteTime = 0f;   //  第一阶段关闭的时间点
    private float _activeFirstStageDuringTime = 7f;   //  最大的时间
    private float _activeSecondStageDuringTime = 2f;   //  第二段缩小的时间
    private bool _showDetail = false;   //  显示企业卡片
    private bool _doMoving = false;     //  移动
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

    protected CardStatusEnum _cardStatus;   // 状态   
    protected FlockAgent _originAgent;  // 原组件

    protected bool _hasListBtn;  //  有列表按钮
    protected bool hasInitBusinessCard = false; // 是否已生成business card
    protected BusinessCardAgent businessCardAgent;

    [SerializeField] RectTransform _main_container;    //  主框体
    [SerializeField] RectTransform _scale_container;    //  缩放容器
    [SerializeField] ScaleAgent _scale_prefab;    //  缩放 prefab
    [SerializeField] RectTransform _searchContainer;    //  搜索容器
    [SerializeField] SearchAgent _searchAgentPrefab;    //  搜索 prefab
    [SerializeField] RectTransform _move_mask; // 移动蒙板
    [SerializeField] RectTransform _move_reminder_container; // 移动提醒容器
    [SerializeField] RectTransform _business_card_container;    // 企业卡片容器
    [SerializeField] BusinessCardAgent _business_card_prefab;    // 企业卡片 control
    [SerializeField] Animator _list_animator;    // list animator
    [SerializeField] Button _btn_search;
    [SerializeField] Button _btn_list;
    [SerializeField] Button _btn_move;
    [SerializeField] Button _btn_close;

    [SerializeField] VideoAgent videoAgentPrefab;   // Video Agent prefab
    [SerializeField] RectTransform normalContainer; // 正常显示的框体
    [SerializeField] RectTransform videoContainer;  // video的安放框体

    [SerializeField] float radius;// Circle Collider2D radius


    Action OnCreatedCompletedAction; 


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
            // 缩小一半
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

    }

    //
    //  Click Button
    //
    public void DoClick() {
        Debug.Log("Click");

        DoRecover();

    }

    //
    //  Click Button
    //
    public void DoDrag()
    {

        Debug.Log("DoDrag");

        DoRecover();

    }

    //
    //  当卡片被操作
    //
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
    //
    //  第一步的销毁
    //
    private void DoDestoriedForFirstStep() {

        //  缩放至2倍大
        Vector3 scaleVector3 = new Vector3(0.7f, 0.7f, 0.7f);
        _cardStatus = CardStatusEnum.DESTORING_STEP_FIRST;

        GetComponent<RectTransform>().DOScale(scaleVector3, 2f)
            .OnUpdate(() =>
             {
                    Width = GetComponent<RectTransform>().sizeDelta.x;
                    Height = GetComponent<RectTransform>().sizeDelta.y;
               }).OnComplete(() => {
                   IsRecovering = false;
                   IsChoosing = true;

                   // 设置第一次缩小的点
                   _destoryFirstStageCompleteTime = Time.time;

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

            rect.DOAnchorPos3D(to, 1f).OnComplete(() => {
                //  使卡片消失

                OriginAgent.DoRecoverAfterChoose();


                _agentManager.RemoveItemFromEffectItems(this);


                //gameObject.SetActive(false);
                //DestoryAgency();
                //Destroy(gameObject);

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
                    //_agentManager.UpdateAgents();
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

        Vector3 scaleVector3 = new Vector3(1f, 1f, 1f);

        GetComponent<RectTransform>().DOScale(scaleVector3, 0.5f)
           .OnUpdate(() =>
           {
               Width = GetComponent<RectTransform>().sizeDelta.x;
               Height = GetComponent<RectTransform>().sizeDelta.y;
               DoUpdate();
           }).OnComplete(() => {
               CardStatus = CardStatusEnum.NORMAL;
               IsChoosing = false;
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
        }
        else
        {
            DoUpdate();
            _doMoving = false;
            _move_reminder_container.gameObject.SetActive(false);
            _move_mask.gameObject.SetActive(false);
        }
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


    // 生成企业卡片
    private void InitEnvCard() {
        if (!hasInitBusinessCard) {

            //  创建 Agent
            businessCardAgent = Instantiate(
                                        _business_card_prefab,
                                        _business_card_container
                                        ) as BusinessCardAgent;
            businessCardAgent.Init(this);

            businessCardAgent.gameObject.SetActive(false);
            hasInitBusinessCard = true;
        }
    }

    //  初始化组件显示的状态
    protected void InitComponents(bool hasListBtn) {
        _hasListBtn = hasListBtn;
        if (_hasListBtn)
        {
            // 显示四组按钮
            _btn_search.GetComponent<RectTransform>().anchoredPosition = new Vector2(-127,0);
            _btn_list.GetComponent<RectTransform>().anchoredPosition = new Vector2(-42, 0);
            _btn_move.GetComponent<RectTransform>().anchoredPosition = new Vector2(42, 0);
            _btn_close.GetComponent<RectTransform>().anchoredPosition = new Vector2(127, 0);

            _btn_list.gameObject.SetActive(true);
            InitEnvCard();
        }
        else {
            // 显示三个按钮
            _btn_list.gameObject.SetActive(false);

            _btn_search.GetComponent<RectTransform>().anchoredPosition = new Vector2(-80, 0);
            _btn_move.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            _btn_close.GetComponent<RectTransform>().anchoredPosition = new Vector2(80, 0);
        }

        // 不显示移动提示
        _move_reminder_container.gameObject.SetActive(false);
        // 关闭移动蒙版
        _move_mask.gameObject.SetActive(false);

    }

    public void CloseBusinessCard() {
        TurnOffKeepOpen();
        _list_animator.ResetTrigger("Highlighted");
        _list_animator.SetTrigger("Normal");
        businessCardAgent.gameObject.SetActive(false);
        _showDetail = false;
    }

    public void OpenBusinessCard()
    {
        KeepOpen = true;
        _list_animator.ResetTrigger("Normal");
        _list_animator.SetTrigger("Highlighted");
        businessCardAgent.gameObject.SetActive(true);
        _showDetail = true;
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
                circle.radius = radius;
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
            CircleCollider2D[] circles = FindObjectsOfType<CircleCollider2D>();
            foreach (CircleCollider2D circle in circles)
            {
                circle.radius = 0;
            }

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

        GetComponent<RectTransform>().anchoredPosition = to;

    }

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

    public void DoVideo(string address, string description)
    {
        //显示 video 的框框
        OpenVideoContainer(address, description);

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
    private void OpenVideoContainer(string address, string description)
    {
        videoContainer.gameObject.SetActive(true);
        _videoAgent = Instantiate(videoAgentPrefab, videoContainer);
        _videoAgent.SetData(address, description, this);
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
                // 执行完成后动画
                DoOnCreatedCompleted();


            }).SetEase(Ease.OutBack);
    }
}


