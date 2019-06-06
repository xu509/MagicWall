using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



/// <summary>
/// 操作管理 弃用
/// </summary>
public class OperateManager : Singleton<OperateManager>
{

    // 上一次点击的时间
    float lastClickDownTime = 0f;

    // 按下与抬起的间隔
    float clickIntervalTime = 0.5f;


    //  滑块浮层
    int Flock_Layer = 10;
    int Card_Layer = 11;
    int Button_Layer = 12;


    // 选中的agent
    FlockAgent chooseFlockAgent = null;

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;


    //
    //  single pattern
    // 
    void Awake()
    {

    }

    //
    //  Constructor
    //
    protected OperateManager() { }


}
