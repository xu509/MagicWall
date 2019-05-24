using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//
//  场景管理器
//  ---
//  1. 场景问题
//  2. 背景问题
//  3. 过场动画问题
//
public class MagicSceneManager : Singleton<MagicSceneManager>
{
    //
    //  paramater
    //
    bool _hasInit = false;


    List<IScene> _scenes; // 场景列表
    int _index; // 当前的索引
    private MagicWallManager _manager; // 主管理器

    //
    // Awake instead of Constructor
    //
    private void Awake()
    {
        _manager = MagicWallManager.Instance;

        Init();
    }

    private void Init() {

        //  初始化场景列表
        _scenes = new List<IScene>();

        //  初始化当前场景索引
        _index = 0;

        //  加载场景
        // - 加载开始场景
        _scenes.Add(new StartScene());

        // - 加载普通场景
        List<SceneConfig> sceneConfigs = DaoService.Instance.GetShowConfigs();
        for (int i = 0; i < sceneConfigs.Count; i++)
        {
            CommonScene commonScene = new CommonScene();
            commonScene.DoConfig(sceneConfigs[i]);
            _scenes.Add(commonScene);
        }

        // 初始化管理器标志
        _manager.CurrentScene = _scenes[0];

        _hasInit = true;
    }



    //
    //  Constructor
    //
    protected MagicSceneManager() { }


    //
    // 开始场景调整
    //
    public void Run() {

        if (!_hasInit) {
            return;
        }

        // 背景始终运行
        BackgroundManager.Instance.run();

        // 运行场景
        if (!_scenes[_index].Run())
        {
            // 返回为false时，表示场景已运行结束
            if (_index == _scenes.Count - 1)
            {
                _index = 0;
            }
            else
            {
                _index++;
            }
            _manager.SceneIndex  = _manager.SceneIndex + 1;
            _manager.CurrentScene = _scenes[_index];
        }
   
    }

    /// <summary>
    ///  重启场景
    /// </summary>
    public void Reset() {
        _hasInit = false;

        Init();
    }






}