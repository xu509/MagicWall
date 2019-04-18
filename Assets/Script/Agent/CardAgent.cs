using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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
    //  第一步的销毁
    //
    private void DoDestoriedForFirstStep() {
        //  获取rect引用
        RectTransform rectTransform = GetComponent<RectTransform>();

        //  定义缩放
        Vector3 scaleVector3 = new Vector3(1.5f, 1.5f, 1.5f);
        AgentManager.Instance.DoScaleAgency(this, scaleVector3, 2f);
    }

    //
    //  第二步的销毁
    //
    private void DoDestoriedForSecondStep()
    {
        AgentManager.Instance.DoDestoryCardAgent(this);
    }

    //
    //  恢复
    //
    private void DoRecover()
    {
        Debug.Log("恢复");
    }

    #endregion



}


