using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class CrossCardAgent : CardAgent
{
    #region Data Parameter
    private string _data_id;    //  企业id
    private string _data_name; // 企业名
    private string _data_card; // 企业名片
    private string[] _data_catalog; //  企业的 catalog

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


