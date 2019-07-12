using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperateMode : MonoBehaviour
{
    public float f_UpdateInterval = 0.5F;
    private float f_LastInterval;
    private int i_Frames = 0;
    private float f_Fps;

    private bool showMode = false;  


    private MagicWallManager _manager;

    public void Init(MagicWallManager manager) {
        _manager = manager;
    }

    void Start()
    {
        f_LastInterval = Time.realtimeSinceStartup;

        i_Frames = 0;
    }


    public void Run() {

        if (Input.GetKeyDown(KeyCode.M))
        {
            // 显示，关闭菜单
            showMode = !showMode;
        }

        // 减速
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _manager.managerConfig.MainPanelMoveFactor = _manager.managerConfig.MainPanelMoveFactor - 1;
        }

        // 加速
        if (Input.GetKeyDown(KeyCode.E))
        {
            _manager.managerConfig.MainPanelMoveFactor = _manager.managerConfig.MainPanelMoveFactor + 1;
        }

        ++i_Frames;

        if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval)
        {
            f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);

            i_Frames = 0;

            f_LastInterval = Time.realtimeSinceStartup;
        }


    }


    void OnGUI()
    {
        if (showMode) {
            // 显示鼠标坐标

            GUIStyle titleStyle2 = new GUIStyle();
            titleStyle2.normal.textColor = Color.black;
            titleStyle2.fontSize = 60;

            GUI.Label(new Rect(30, 10, 300, 300), Input.mousePosition.ToString(), titleStyle2);


            // 显示帧率
            GUIStyle gStyle = new GUIStyle();
            gStyle.normal.textColor = Color.yellow;
            gStyle.fontSize = 100;
            float width = Screen.width / 3;
            float w = Screen.width - (width + 50);
            GUI.Label(new Rect(w, 20, width, width), "FPS:" + f_Fps.ToString("f2"), gStyle);



            // 显示移动速度

            GUIStyle moveTextStyle = new GUIStyle();
            moveTextStyle.normal.textColor = Color.black;
            moveTextStyle.fontSize = 60;

            string moveStr = "当前移动速率： " + _manager.managerConfig.MainPanelMoveFactor;

            GUI.Label(new Rect(30, 90, 300, 300), moveStr, moveTextStyle);

        }

    }


}
