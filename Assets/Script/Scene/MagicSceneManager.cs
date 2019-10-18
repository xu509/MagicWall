using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

//
//  场景管理器
//  ---
//  1. 场景问题
//  2. 背景问题
//  3. 过场动画问题
//
namespace MagicWall
{
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


        Action _onSceneEnterLoop;
        Action _onStartCompleted;




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
        public void Init(MagicWallManager manager,Action onSceneEnterLoop,Action onStartCompleted)
        {

            System.Diagnostics.Stopwatch sw2 = new System.Diagnostics.Stopwatch();
            sw2.Start();

            _onSceneEnterLoop = onSceneEnterLoop;
            _onStartCompleted = onStartCompleted;

            _manager = manager;
            _sceneStatus = MagicSceneEnum.Running;

            //  初始化场景列表
            _scenes = new List<IScene>();

            //  初始化当前场景索引
            _index = 0;

            //  加载场景
            // - 加载开始场景
            StartScene startScene = new StartScene();
            startScene.Init(null, manager);
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
                else
                {
                    scene = new CommonScene();
                }

                scene.Init(sceneConfigs[i], _manager);
                _scenes.Add(scene);
            }

            sw2.Stop();
            //Debug.Log(" Scene init : " + sw2.ElapsedMilliseconds / 1000f);

            // 初始化管理器标志
            _manager.CurrentScene = _scenes[0];


            for (int i = 0; i < _scenes.Count; i++)
            {
                _scenes[i].SetOnRunCompleted(OnRunCompleted);
            }


            _hasInit = true;
        }



        //
        // 开始场景调整
        //
        public void Run()
        {
            // 背景始终运行
            _manager.backgroundManager.run();

            if (!_hasInit && _scenes != null)
            {
                return;
            }

            if (_sceneStatus == MagicSceneEnum.Running)
            {
                _scenes[_index].Run();
            }
            else if (_sceneStatus == MagicSceneEnum.RunningComplete)
            {
                _sceneStatus = MagicSceneEnum.Reset;

                // do reset
                _manager.Clear();
                _sceneStatus = MagicSceneEnum.ResetComplete;
            }
            else if (_sceneStatus == MagicSceneEnum.Reset)
            {
            }
            else if (_sceneStatus == MagicSceneEnum.ResetComplete) {
                GoNext();
            }

        }

        /// <summary>
        ///  重启场景
        /// </summary>
        public void Reset()
        {
            _hasInit = false;

            //Init(_manager);
        }


        ///// <summary>
        /////  调整为下一个
        ///// </summary>
        //public void TurnToNext()
        //{
        //    if (_sceneStatus == MagicSceneEnum.Running)
        //    {
        //        _sceneStatus = MagicSceneEnum.RunningEnd;
        //    }
        //}

        ///// <summary>
        ///// 调整至上一个场景
        ///// </summary>
        //public void TurnToPrevious()
        //{
        //    if (_sceneStatus == MagicSceneEnum.Running)
        //    {
        //        _sceneStatus = MagicSceneEnum.RunningEnd;
        //    }
        //}



        private void GoNext()
        {
            if (_index == 0) {
                // 预加载场景结束
                _onStartCompleted.Invoke();

            }

            if (_index == _scenes.Count - 1)
            {
                
                if (!_manager.switchMode)
                {
                    _index = 0;
                }
                else {
                    _onSceneEnterLoop.Invoke();
                    return;
                }
            }
            else
            {
                _index++;
            }

            _manager.SceneIndex = _manager.SceneIndex + 1;
            _manager.CurrentScene = _scenes[_index];

            _sceneStatus = MagicSceneEnum.Running;
        }

        //private void GoPrevious()
        //{
        //    if (_index == 0)
        //    {
        //        _index = _scenes.Count - 1;
        //    }
        //    else
        //    {
        //        _index--;
        //    }

        //    _manager.SceneIndex = _manager.SceneIndex + 1;
        //    _manager.CurrentScene = _scenes[_index];
        //}


        private void OnRunCompleted()
        {
            _sceneStatus = MagicSceneEnum.RunningComplete;
        }


    }
}