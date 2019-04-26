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
    private int _sceneIndex;    //  场景的索引

    protected CardStatusEnum _cardStatus;   // 状态   
    protected FlockAgent _originAgent;  // 原组件

    public CardStatusEnum CardStatus {
        set { _cardStatus = value; }
        get { return _cardStatus; }
    }

    public FlockAgent OriginAgent
    {
        set { _originAgent = value; }
        get { return _originAgent; }
    }

    public int SceneIndex
    {
        set { _sceneIndex = value; }
        get { return _sceneIndex; }
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
        // 缩小一半
        if (_cardStatus == CardStatusEnum.NORMAL)
        {
            if ((Time.time - _recentActiveTime) > _activeFirstStageDuringTime)
            {
                DoDestoriedForFirstStep();
                _cardStatus = CardStatusEnum.DESTORING;
            }
        }

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
        //  获取rect引用
        RectTransform rectTransform = GetComponent<RectTransform>();

        //  定义缩放
        Vector3 scaleVector3 = new Vector3(2f, 2f, 2f);
        AgentManager.Instance.DoScaleAgency(this, scaleVector3, 2f);
    }

    //
    //  第二步的销毁
    //
    private void DoDestoriedForSecondStep()
    {
        MagicWallManager _manager = MagicWallManager.Instance;


        //  如果场景没有变，则回到原位置
        if (_sceneIndex == _manager.SceneIndex)
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
                gameObject.SetActive(false);
                AgentManager.Instance.RemoveItemFromEffectItems(this);

                Destroy(this.gameObject);

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
                .OnComplete(() => DoScaleCompletedCallBack(this, vector3));
        }

    }

    //
    //  恢复
    //
    private void DoRecover()
    {

        Vector3 scaleVector3 = new Vector3(3.68f, 3.68f, 3.68f);
        AgentManager.Instance.DoScaleAgency(this, scaleVector3, 0.5f);

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




}


