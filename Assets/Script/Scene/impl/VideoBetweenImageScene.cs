using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
using UnityEngine.Video;

// 6屏幕，视频在中间
namespace MagicWall
{
    public class VideoBetweenImageScene : IScene
    {
        //
        //  Parameter
        //
        private MagicWallManager _manager;
        private MagicSceneManager _magicSceneManager;

        Action _onSceneCompleted;

        /// <summary>
        /// 场景持续时间
        /// </summary>
        float _durTime;

        SceneConfig _sceneConfig;


        private bool _hasInit = false;



        //  场景状态
        SceneStatus status = SceneStatus.PREPARING; //场景状态
        public SceneStatus Status { get { return status; } set { status = value; } }

        MagicSceneEnum _magicSceneEnumStatus;

        public void Init(SceneConfig sceneConfig, MagicWallManager manager,Action onSceneCompleted)
        {
            _manager = manager;
            _sceneConfig = sceneConfig;
            _onSceneCompleted = onSceneCompleted;

            _magicSceneManager = GameObject.Find("Scene").GetComponent<MagicSceneManager>();
            _magicSceneManager._videoBetweenImageController.Init(manager);

        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData() {
            _durTime = _sceneConfig.durtime;
            _magicSceneManager._videoBetweenImageController.StartPlay();
            _hasInit = true;
        }

        //  运行
        public bool Run()
        {
            if (!_hasInit) {
                InitData();
            }

            // 整体淡入

            // 左侧、右侧轮播图片

            // 播放视频

            // 当场景结束调用
            _onSceneCompleted.Invoke();


            return true;
        }


        public DataTypeEnum GetDataType()
        {
            return DataTypeEnum.Video;
        }

        public MagicSceneEnum GetSceneStatus()
        {
            return _magicSceneEnumStatus;
        }


    }
}