using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



/// <summary>
/// 操作管理
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

        // Raycaster - event
        GameObject magicWall = MagicWallManager.Instance.gameObject;
        m_Raycaster = magicWall.GetComponent<GraphicRaycaster>();
        m_EventSystem = magicWall.GetComponent<EventSystem>();
    }

    //
    //  Constructor
    //
    protected OperateManager() { }


    /// <summary>
    /// 监听
    /// </summary>
    /// 
    public void DoListening() {

        // 开启手势监听
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("GetMouseButtonDown");
            if (lastClickDownTime == 0f)
            {
                lastClickDownTime = Time.time;
            }
            chooseFlockAgent = GetAgentsByMousePosition();
            //Debug.Log("chooseFlockAgent : " + chooseFlockAgent);
        }

        if (Input.GetMouseButton(0))
        {
            //Debug.Log("GetMouseButton");
            if ((Time.time - lastClickDownTime) > clickIntervalTime)
            {
                Debug.Log("recognize Drag");
                // 此处为拖拽事件
                if (chooseFlockAgent != null)
                {
                    //DoDragItem(chooseFlockAgent);
                    Debug.Log("Do Draging... ");
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log("GetMouseButtonUp");
            if ((Time.time - lastClickDownTime) < clickIntervalTime)
            {
                Debug.Log("recognize click");
                // 此处为点击事件
                if (chooseFlockAgent != null)
                {
                    chooseFlockAgent.DoChoose();
                    
                }
            }
            lastClickDownTime = 0f;
        }

    }

    #region 根据鼠标点击位置获取 agent
    FlockAgent GetAgentsByMousePosition()
    {
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        FlockAgent choseFlockAgent = null;

        foreach (RaycastResult result in results)
        {
            GameObject go = result.gameObject;

            // 通过layer取到agents的子图片
            if (go.layer == Card_Layer) {
                Debug.Log("Click Button_Layer : " + go.name);
            }
            else if (go.layer == Flock_Layer)
            {
                Debug.Log("Click Flock_Layer : " + go.name);


                //Debug.Log(go.name);
                choseFlockAgent = go.transform.parent.GetComponent<FlockAgent>();

                if (go.GetComponent<FlockAgent>() != null)
                {
                    choseFlockAgent = go.GetComponent<FlockAgent>();
                }
            }

        }
        return choseFlockAgent;
    }
    #endregion

}
