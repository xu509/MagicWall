using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class CrossCardAgent : CardAgent
{
    #region Data Parameter

    #endregion




    //
    //  初始化数据
    //
    public void InitData()
    {
        Debug.Log("Init Data : " + OriginAgent.DataId);

    }
    

    void Awake() {
        AwakeAgency();
    }

    //
    //  更新
    //
    void Update() {
        UpdateAgency();
    }
   
}


