using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

// 普通场景
namespace MagicWall
{
    public class CommonScene : IScene
    {
        //
        //  Parameter
        //
        private MagicWallManager _manager;

        Action _onRunCompleted;
        Action _onRunEndCompleted;
        Action _onSceneCompleted;

        // run
        private bool _runEntrance = false;        
        private bool _runDisplay = false;
        public bool runDisplay { get { return _runDisplay; } set { _runDisplay = value; } }
        private bool _runDestory = false;

        // 时间点
        private float _displayStartTime;

        // flag
        private bool _hasCallDestory = false;
        private bool _hasInitData = false;

        //  使用的过场效果
        private ICutEffect _cutEffect;

        // 运行效果
        private CutEffectDisplayBehavior _displayBehavior;

        // 关闭效果
        private CutEffectDestoryBehavior _destoryBehavior;

        //  使用的类型
        private DataTypeEnum _dataType;

        private SceneConfig _sceneConfig;

        //  场景状态
        SceneStatus status = SceneStatus.PREPARING; //场景状态
        public SceneStatus Status { get { return status; } set { status = value; } }

        MagicSceneEnum _magicSceneEnumStatus;

        public void Init(SceneConfig sceneConfig, MagicWallManager manager,Action onSceneCompleted)
        {
            _manager = manager;
            _onSceneCompleted = onSceneCompleted;

            _cutEffect = CutEffectFactory.GetCutEffect(sceneConfig.sceneType); // 设置过场效果
            _cutEffect.Init(_manager, sceneConfig
                , OnCutEffectCreateAgentCompleted,
                ()=> {
                    // on effect completed
                    Debug.Log("on effect completed");

                    _runEntrance = false;
                },()=>
                {
                    // on display Start
                    Debug.Log("on display start");

                    _runDisplay = true;
                    _displayStartTime = Time.time;
                }
                );
            _dataType = sceneConfig.dataType; // 设置类型

            //  显示
            _displayBehavior = DisplayBehaviorFactory.GetBehavior(sceneConfig.displayBehavior);

            // 销毁
            _destoryBehavior = DestoryBehaviorFactory.GetBehavior(sceneConfig.destoryBehavior);
            _destoryBehavior.Init(_manager,this, OnDestoryCompleted);

            _sceneConfig = sceneConfig;

            _magicSceneEnumStatus = MagicSceneEnum.Running;

            _runEntrance = true;
        }

        private void InitData() {
             _runEntrance = true;
             _runDisplay = false;
             _runDestory = false;
             _displayStartTime = 0;
             _hasCallDestory = false;
             _hasInitData = true;

            if (_sceneConfig.isKinect == 0)
            {
                _manager.useKinect = false;
            }
            else {
                _manager.useKinect = true;
            }

        }




        //  运行
        public bool Run()
        {
            if (!_hasInitData) {
                InitData();
            }

            if (_runEntrance) {
                _cutEffect.Run();
            }

            if (_runDisplay) {
                _displayBehavior.Run();
            }

            if (_runDestory) {
                _destoryBehavior.Run();
            }

            if (_runDisplay && ((Time.time - _displayStartTime) > _sceneConfig.durtime)) {
                if (!_hasCallDestory) {
                    _hasCallDestory = true;
                    _runDestory = true;
                }
            }

            return true;
        }

        public DataTypeEnum GetDataType()
        {
            return _dataType;
        }

        public MagicSceneEnum GetSceneStatus()
        {
            return _magicSceneEnumStatus;
        }

        private void OnCutEffectCreateAgentCompleted(DisplayBehaviorConfig displayBehaviorConfig) {
            _displayBehavior.Init(displayBehaviorConfig);
        }

        private void OnDestoryCompleted() {
            _hasInitData = false;
            _onSceneCompleted.Invoke();

        }

        public void RunEnd(Action onEndCompleted)
        {
            // 渐暗，清理
            _manager.BgLogo.GetComponent<Image>().sprite = _manager.magicSceneManager.logo;

            _manager.mainPanel.GetComponent<CanvasGroup>().DOFade(0, 1.5f)
                .OnComplete(() => {
                    _hasInitData = false;
                    _manager.Clear();
                    onEndCompleted.Invoke();
            });



        }
    }
}