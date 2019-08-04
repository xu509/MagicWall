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
public class MagicSceneManager : MonoBehaviour
{
    private MagicWallManager _manager;

    //
    //  paramater
    //
    bool _hasInit = false;

    List<IScene> _scenes; // 场景列表
    int _index; // 当前的索引

    private MagicSceneEnum _sceneStatus;



    //
    // Awake instead of Constructor
    //
    private void Awake()
    {

    }

    /// <summary>
    /// 初始化场景状态
    /// </summary>
    /// <param name="manager"></param>
    public void Init(MagicWallManager manager) {

        System.Diagnostics.Stopwatch sw2 = new System.Diagnostics.Stopwatch();
        sw2.Start();

        _manager = manager;
        _sceneStatus = MagicSceneEnum.Running;

        //  初始化场景列表
        _scenes = new List<IScene>();

        //  初始化当前场景索引
        _index = 0;

        //  加载场景
        // - 加载开始场景
        StartScene startScene = new StartScene();
        startScene.Init(null,manager);
        _scenes.Add(startScene);

        // - 加载普通场景
        List<SceneConfig> sceneConfigs = manager.daoService.GetShowConfigs();
        for (int i = 0; i < sceneConfigs.Count; i++)
        {
            //Debug.Log(sceneConfigs[i].ToString());

            IScene scene;
            if (sceneConfigs[i].sceneType == SceneTypeEnum.Stars)
            {
                scene = new StarScene();
            }
            else {
                scene = new CommonScene();
            }

            scene.Init(sceneConfigs[i],_manager);
            _scenes.Add(scene);
        }

        sw2.Stop();
        //Debug.Log(" Scene init : " + sw2.ElapsedMilliseconds / 1000f);

        // 初始化管理器标志
        _manager.CurrentScene = _scenes[0];


        for (int i = 0; i < _scenes.Count; i++) {
            _scenes[i].SetOnRunCompleted(OnRunCompleted);
            _scenes[i].SetOnRunEndCompleted(OnRunEndCompleted);
        }


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

        if (!_hasInit && _scenes != null) {
            return;
        }

        // 背景始终运行
        _manager.backgroundManager.run();

        //return;

        if (_sceneStatus == MagicSceneEnum.Running)
        {
            _scenes[_index].Run();
        }

        else if (_sceneStatus == MagicSceneEnum.RunningComplete)
        {
            _sceneStatus = MagicSceneEnum.RunningEnd;
        }

        else if (_sceneStatus == MagicSceneEnum.RunningEnd)
        {
            _scenes[_index].RunEnd();
        }

        else if (_sceneStatus == MagicSceneEnum.RunningEndComplete)
        {
            GoNext();
            _sceneStatus = MagicSceneEnum.Running;
        }


        //// 运行场景
        //if (!_scenes[_index].Run())
        //{
        //    GoNext();
        //    //// 返回为false时，表示场景已运行结束
        //    //if (_index == _scenes.Count - 1)
        //    //{
        //    //    _index = 0;
        //    //}
        //    //else
        //    //{
        //    //    _index++;
        //    //}

        //    //_manager.SceneIndex = _manager.SceneIndex + 1;
        //    //_manager.CurrentScene = _scenes[_index];
        //}

    }

    /// <summary>
    ///  重启场景
    /// </summary>
    public void Reset() {
        _hasInit = false;

        //Init(_manager);
    }


    /// <summary>
    ///  调整为下一个
    /// </summary>
    public void TurnToNext() {
        if(_sceneStatus == MagicSceneEnum.Running)
        {
            _sceneStatus = MagicSceneEnum.RunningEnd;
        }
    }

    /// <summary>
    /// 调整至上一个场景
    /// </summary>
    public void TurnToPrevious()
    {
        if (_sceneStatus == MagicSceneEnum.Running)
        {
            _sceneStatus = MagicSceneEnum.RunningEnd;
        }
    }



    private void GoNext() {
        if (_index == _scenes.Count - 1)
        {
            _index = 0;
        }
        else
        {
            _index++;
        }

        _manager.SceneIndex = _manager.SceneIndex + 1;
        _manager.CurrentScene = _scenes[_index];
    }

    private void GoPrevious() {
        if (_index == 0)
        {
            _index = _scenes.Count - 1;
        }
        else
        {
            _index--;
        }

        _manager.SceneIndex = _manager.SceneIndex + 1;
        _manager.CurrentScene = _scenes[_index];
    }


    private void OnRunCompleted() {
        _sceneStatus = MagicSceneEnum.RunningComplete;
    }

    private void OnRunEndCompleted()
    {
        _sceneStatus = MagicSceneEnum.RunningEndComplete;
    }

}