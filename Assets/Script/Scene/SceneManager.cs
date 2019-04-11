using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  场景管理器
//  ---
//  1. 场景问题
//  2. 背景问题
//  3. 过场动画问题
//
public class SceneManager : Singleton<SceneManager>
{
    //
    //  paramater
    //
    List<IScene> Scenes; // 场景列表
    int index; // 当前的索引
    private MagicWallManager magicWallManager; // 主管理器

    float start_time;
    float destoryingDuringTime = 2f; // 销毁的时间
    float destory_time; // Destory_Time

    CutEffect cutEffect; // 当前的过场动画	


    //
    // Awake instead of Constructor
    //
    private void Awake()
    {

        //  初始化场景列表
        this.Scenes = new List<IScene>();

        //  加载场景
        //StartScene startScene = new StartScene(magicWallManager);
        //Scenes.Add(startScene);
        EnvScene envScene = new EnvScene();
        Debug.Log("Do Set EnvScene Wall Manager!");
        Scenes.Add(envScene);

        //  初始化当前场景索引
        this.index = 0;

    }

    //
    //  Constructor
    //
    protected SceneManager() { }


    //
    // 开始场景调整
    //
    public void Run() {

        // 背景始终运行
        BackgroundManager.Instance.run();

        // 运行场景
        if (!Scenes[index].Run())
        {
            // 返回为false时，表示场景已运行结束
            if (index == Scenes.Count - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }
        }
   
    }


}