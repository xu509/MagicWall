using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;

public class CardAgent : FlockAgent,IBeginDragHandler, IEndDragHandler, IDragHandler
{
    #region Parameter
    private float _recentActiveTime = 0f;   //  最近次被操作的时间点
    private float _destoryFirstStageCompleteTime = 0f;   //  第一阶段关闭的时间点
    private float _activeFirstStageDuringTime = 7f;   //  最大的时间
    private float _activeSecondStageDuringTime = 2f;   //  第二段缩小的时间
    private bool _showDetail = false;   // 显示企业卡片
    private bool _doMoving = false;   // 移动
    private ScaleAgent _scaleAgent;     // 缩放代理

    protected int id;
    public int Id { set { id = value; } get { return id; } }

    protected CardStatusEnum _cardStatus;   // 状态   
    protected FlockAgent _originAgent;  // 原组件

    protected bool _hasListBtn;  //  有列表按钮
    protected bool hasInitBusinessCard = false; // 是否已生成business card
    protected BusinessCardAgent businessCardAgent;

    [SerializeField] RectTransform _main_container;    //  主框体
    [SerializeField] RectTransform _scale_container;    //  缩放容器
    [SerializeField] ScaleAgent _scale_prefab;    //  缩放 prefab
    [SerializeField] RectTransform _move_mask; // 移动蒙板
    [SerializeField] RectTransform _move_reminder_container; // 移动提醒容器
    [SerializeField] RectTransform _business_card_container;    // 企业卡片容器
    [SerializeField] BusinessCardAgent _business_card_prefab;    // 企业卡片 control
    [SerializeField] Animator _list_animator;    // list animator
    [SerializeField] Button _btn_search;
    [SerializeField] Button _btn_list;
    [SerializeField] Button _btn_move;
    [SerializeField] Button _btn_close;

    [SerializeField] float radius;// Circle Collider2D radius

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

    //
    //  Awake 代理
    //
    protected void AwakeAgency() {
        _recentActiveTime = Time.time;
        _cardStatus = CardStatusEnum.NORMAL;
    }

    //
    //  Update 代理
    //
    protected void UpdateAgency()
    {

        //Debug.Log("暂时屏蔽缩小");


        // 缩小一半
        //if (_cardStatus == CardStatusEnum.NORMAL)
        //{
        //    if ((Time.time - _recentActiveTime) > _activeFirstStageDuringTime)
        //    {
        //        DoDestoriedForFirstStep();
        //    }
        //}

        // 第二次缩小
        if (_cardStatus == CardStatusEnum.DESTORING)
        {
            if (_destoryFirstStageCompleteTime != 0 && (Time.time - _destoryFirstStageCompleteTime) > _activeSecondStageDuringTime)
            {
                DoDestoriedForSecondStep();
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
        else if (CardStatus == CardStatusEnum.DESTORING) {
            DoRecover();
        }
    }



    #endregion

    #region Private Methods
    //
    //  第一步的销毁
    //
    private void DoDestoriedForFirstStep() {

        Debug.Log("DoDestoriedForFirstStep : " + gameObject?.name);


        //  缩放至2倍大
        Vector3 scaleVector3 = new Vector3(0.7f, 0.7f, 0.7f);
        _cardStatus = CardStatusEnum.DESTORING;

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
        MagicWallManager _manager = MagicWallManager.Instance;

        //  如果场景没有变，则回到原位置
        if (SceneIndex == _manager.SceneIndex)
        {
            //恢复并归位
            // 缩到很小很小
            RectTransform rect = GetComponent<RectTransform>();

            //  移到后方、缩小、透明
            rect.DOScale(0.2f, 1f);

            //  获取位置
            FlockAgent oriAgent = _originAgent;
            Vector3 to = new Vector3(oriAgent.OriVector2.x - _manager.PanelOffsetX, oriAgent.OriVector2.y - _manager.PanelOffsetY, 200);

            rect.DOAnchorPos3D(to, 1f).OnComplete(() => {
                //  使卡片消失
                AgentManager.Instance.RemoveItemFromEffectItems(this);

                gameObject.SetActive(false);
                Destroy(gameObject);

                OriginAgent.DoRecoverAfterChoose();
            }); ;
        }
        //  直接消失
        else
        {

            Debug.Log("慢慢缩小直到消失");

            // 慢慢缩小直到消失
            Vector3 vector3 = Vector3.zero;

            GetComponent<RectTransform>().DOScale(vector3, 1.5f)
                .OnUpdate(() => {
                    Width = GetComponent<RectTransform>().sizeDelta.x;
                    Height = GetComponent<RectTransform>().sizeDelta.y;
                    //AgentManager.Instance.UpdateAgents();
                })
                .OnComplete(() => DoDestoryOnCompleteCallBack(this));

            // 将原
        }

        _cardStatus = CardStatusEnum.DESTORYED;
    }

    //
    //  恢复
    //
    private void DoRecover()
    {

        Vector3 scaleVector3 = new Vector3(3.68f, 3.68f, 3.68f);

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
    //  Private Methods
    //





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
        DoUpdate();
        // 移动
        if (!_doMoving)
        {
            _move_reminder_container.gameObject.SetActive(true);
            _doMoving = true;
            //  可拖动 Card

            _move_mask.gameObject.SetActive(true);
        }
        else
        {
            _move_reminder_container.gameObject.SetActive(false);
            _doMoving = false;

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
        DoUpdate();
        _list_animator.ResetTrigger("Highlighted");
        _list_animator.SetTrigger("Normal");
        businessCardAgent.gameObject.SetActive(false);
        _showDetail = false;
    }

    public void OpenBusinessCard()
    {
        DoUpdate();
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
            //Debug.Log("eventData.position : " + eventData.position);

            DoUpdate();

            GetComponent<RectTransform>().anchoredPosition = eventData.position;

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
}


