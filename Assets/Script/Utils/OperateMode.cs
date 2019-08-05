using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  操作模块
/// </summary>
public class OperateMode : MonoBehaviour
{
    [SerializeField, Header("UI")] MessageAgent _messageAgent;

    [SerializeField, Header("Scene")] MagicSceneManager _magicSceneManager;

    [SerializeField, Header("FPS")] float f_UpdateInterval = 0.5F;

    private float f_LastInterval;
    private int i_Frames = 0;
    private float f_Fps;

    private bool showMode = false;
    private bool showHelp = false;

    private MagicWallManager _manager;

    Queue<MoveBehaviourType> _moveBehaviourTypeQueue;


    public void Init(MagicWallManager manager) {
        _manager = manager;
    }

    void Start()
    {
        f_LastInterval = Time.realtimeSinceStartup;

        i_Frames = 0;


        _moveBehaviourTypeQueue = new Queue<MoveBehaviourType>();
        _moveBehaviourTypeQueue.Enqueue(MoveBehaviourType.Common);
        _moveBehaviourTypeQueue.Enqueue(MoveBehaviourType.Round);
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

        // 限制帧率
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_manager.isLimitFps)
            {
                Application.targetFrameRate = -1;
            }
            else {
                Application.targetFrameRate = 60;
            }
            _manager.isLimitFps = !_manager.isLimitFps;
        }


        ++i_Frames;

        if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval)
        {
            f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);

            i_Frames = 0;

            f_LastInterval = Time.realtimeSinceStartup;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            showHelp = !showHelp;
            if (showHelp)
            {
                _messageAgent.UpdateMessage("Help \n\n" +
                    " 【1】 ： 切换卡片动画模式 \t" + "\n\n" +
                    " 【N】 ： 切换场景 \t" + "\n\n" +
                    "【H】 ：打开/关闭帮助文档");
            }
            else {
                _messageAgent.Close();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // 切换卡片受影响模式

            var _moveBehaviour = _moveBehaviourTypeQueue.Dequeue();
            _manager.flockBehaviorConfig.MoveBehaviourType = _moveBehaviour;
            _moveBehaviourTypeQueue.Enqueue(_moveBehaviour);

            _messageAgent.UpdateMessage("已更换卡片动画模式，当前模式为： " + _moveBehaviour,5f);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            // 切换卡片受影响模式

            _magicSceneManager.TurnToNext();

            _messageAgent.UpdateMessage("切换场景中",3f);
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

            string showFpsStr = "FPS:" + f_Fps.ToString("f2") + (_manager.isLimitFps ? "（限制帧率）" : "" );

            GUI.Label(new Rect(w, 20, width, width), showFpsStr, gStyle);



            // 显示移动速度

            GUIStyle moveTextStyle = new GUIStyle();
            moveTextStyle.normal.textColor = Color.black;
            moveTextStyle.fontSize = 60;

            string moveStr = "当前移动速率： " + _manager.managerConfig.MainPanelMoveFactor;

            GUI.Label(new Rect(30, 90, 300, 300), moveStr, moveTextStyle);

        }

    }


}
