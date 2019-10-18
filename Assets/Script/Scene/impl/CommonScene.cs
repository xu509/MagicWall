using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

// 普通场景
namespace MagicWall
{
    public class CommonScene : IScene
    {
        //
        //  Parameter
        //
        private MagicWallManager _manager;

        // Dao Service
        DaoService _daoService;

        Action _onRunCompleted;
        Action _onRunEndCompleted;

        // run
        private bool _runEntrance = false;
        private bool _runDisplay = false;
        private bool _runDestory = false;


        //  是否正在运行 Destory
        private bool doDestoryCompleting = false;

        //  使用的过场效果
        private CutEffect _theCutEffect;
        private ICutEffect _cutEffect;

        //  使用的类型
        private DataTypeEnum _dataType;

        //  场景开始的时间
        private float _startTime;

        //  场景显示的时间点
        private float _displayTime;

        //  场景开始销毁的时间点
        private float _destoryTime;

        //  场景状态
        SceneStatus status = SceneStatus.PREPARING; //场景状态
        public SceneStatus Status { get { return status; } set { status = value; } }

        MagicSceneEnum _magicSceneEnumStatus;

        public void Init(SceneConfig sceneConfig, MagicWallManager manager)
        {
            _manager = manager;

            _cutEffect = CutEffectFactory.GetCutEffect(sceneConfig.sceneType); // 设置过场效果
            _cutEffect.Init(_manager, sceneConfig
                , ()=> {
                    // on effect end
                },()=> {
                    // on display start
                    _runDisplay = true;
                },()=> {
                    // on start completed
                    _runEntrance = false;
                }, ()=> {
                    // on destory start
                    _runDestory = true;
                }, ()=> {

                    _runEntrance = true;
                    _runDestory = false;
                    _runDisplay = false;
                    // on destoryCompleted
                    _onRunCompleted.Invoke();
                });
            _dataType = sceneConfig.dataType; // 设置类型
            _magicSceneEnumStatus = MagicSceneEnum.Running;

            _runEntrance = true;
        }



        //
        //  Private Methods
        //

        //销毁动画已完成
        private bool DoDestoryCompleted()
        {
            if (!doDestoryCompleting)
            {
                _manager.mainPanel.GetComponent<CanvasGroup>().alpha = 1;
                _manager.mainPanel.GetComponentInChildren<CanvasGroup>().alpha = 1;
                doDestoryCompleting = true;

                // 清理面板
                return _manager.Clear();
            }
            else
            {
                return false;
            }

        }

        //
        //  初始化
        // -- 初始化 Display时间
        // -- 初始化当前的过场效果
        //
        private void DoCreating()
        {
            _theCutEffect.Create(_dataType);

            doDestoryCompleting = false;
        }



        //  运行
        public bool Run()
        {
            if (_runEntrance) {
                _cutEffect.RunEntrance();
            }

            if (_runDisplay) {
                _cutEffect.RunDisplaying();
            }

            if (_runDestory) {
                _cutEffect.RunDestoring();
            }

            _cutEffect.Run();

            return true;
        }

        public DataTypeEnum GetDataType()
        {
            return _dataType;
        }



        #region Implement


        public void SetOnRunCompleted(Action onRunCompleted)
        {
            _onRunCompleted = onRunCompleted;
        }


        public MagicSceneEnum GetSceneStatus()
        {
            return _magicSceneEnumStatus;
        }

        #endregion
    }
}