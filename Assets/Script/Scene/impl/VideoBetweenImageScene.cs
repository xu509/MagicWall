using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

// 6屏幕，视频在中间
namespace MagicWall
{
    public class VideoBetweenImageScene : IScene
    {
        //
        //  Parameter
        //
        private MagicWallManager _manager;

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
            _onSceneCompleted = onSceneCompleted;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData() {
            _durTime = _sceneConfig.durtime;
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