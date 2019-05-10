using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;

public class CardAgent : FlockAgent
{
    #region Parameter
    private float _recentActiveTime = 0f;   //  最近次被操作的时间点
    private float _activeFirstStageDuringTime = 7f;   //  最大的时间
    private float _activeSecondStageDuringTime = 4f;   //  第二段缩小的时间
    private bool _showDetail = false;   // 显示企业卡片

    protected CardStatusEnum _cardStatus;   // 状态   
    protected FlockAgent _originAgent;  // 原组件

    protected bool hasListBtn;  //  有列表按钮
    protected bool hasInitBusinessCard = false; // 是否已生成business card
    protected BusinessCardAgent businessCardAgent;

    [SerializeField] RectTransform _business_card_container;    // 企业卡片容器
    [SerializeField] BusinessCardAgent _business_card_prefab;    // 企业卡片 control
    [SerializeField] Animator _list_animator;    // list animator
    [SerializeField] Button _btn_search;
    [SerializeField] Button _btn_list;
    [SerializeField] Button _btn_move;
    [SerializeField] Button _btn_close;

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
        Debug.Log("暂时屏蔽缩小");

        //// 缩小一半
        //if (_cardStatus == CardStatusEnum.NORMAL)
        //{
        //    if ((Time.time - _recentActiveTime) > _activeFirstStageDuringTime)
        //    {
        //        DoDestoriedForFirstStep();
        //        _cardStatus = CardStatusEnum.DESTORING;
        //    }
        //}

        // 第二次缩小
        if (_cardStatus == CardStatusEnum.DESTORING)
        {
            if ((Time.time - _recentActiveTime) > (_activeFirstStageDuringTime + _activeSecondStageDuringTime))
            {
                DoDestoriedForSecondStep();
                _cardStatus = CardStatusEnum.DESTORYED;
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
        Debug.Log("DO UPDATE");

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

        //  缩放至2倍大
        Vector3 scaleVector3 = new Vector3(0.7f, 0.7f, 0.7f);
        DoScaleAgency(this, scaleVector3, 2f);

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
            // 慢慢缩小直到消失
            Vector3 vector3 = Vector3.zero;

            GetComponent<RectTransform>().DOScale(vector3, 1.5f)
                .OnUpdate(() => {
                    Width = GetComponent<RectTransform>().sizeDelta.x;
                    Height = GetComponent<RectTransform>().sizeDelta.y;
                    AgentManager.Instance.UpdateAgents();
                })
                .OnComplete(() => DoDestoryOnCompleteCallBack(this));

            // 将原
        }

    }

    //
    //  恢复
    //
    private void DoRecover()
    {

        Vector3 scaleVector3 = new Vector3(3.68f, 3.68f, 3.68f);
        DoScaleAgency(this, scaleVector3, 0.5f);

        Debug.Log("恢复");
        CardStatus = CardStatusEnum.NORMAL;
        _recentActiveTime = Time.time;

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

        Debug.Log("Do Closing");
    }

    //
    //  点击详细按钮
    //
    public void DoDetail()
    {
        // 生成企业卡片
        _showDetail = !_showDetail;

        if (_showDetail)
        {
            _list_animator.ResetTrigger("Normal");
            _list_animator.SetTrigger("Highlighted");
            businessCardAgent.gameObject.SetActive(true);



        }
        else {
            _list_animator.ResetTrigger("Highlighted");
            _list_animator.SetTrigger("Normal");

            businessCardAgent.gameObject.SetActive(false);
        }

    }

    // 生成企业卡片
    private void InitEnvCard() {
        if (!hasInitBusinessCard) {

            Debug.Log(" 创建business card ");
            //  创建 Agent
            businessCardAgent = Instantiate(
                                        _business_card_prefab,
                                        _business_card_container
                                        ) as BusinessCardAgent;

            businessCardAgent.gameObject.SetActive(false);

            hasInitBusinessCard = true;
        }
    }

    //  初始化组件显示的状态
    protected void InitComponents() {
        if (hasListBtn)
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
    }


}


