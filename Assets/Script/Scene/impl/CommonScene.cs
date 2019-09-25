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

        //  是否正在运行 Destory
        private bool doDestoryCompleting = false;

        //  使用的过场效果
        private CutEffect _theCutEffect;

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

        //
        //  Private Methods
        //

        //	过场动画
        private void DoStarting()
        {
            _theCutEffect.Starting();

        }

        //  展示动画
        private void DoDisplaying()
        {
            _theCutEffect.Displaying();


        }

        //  销毁动画
        private void DoDestorying()
        {
            _theCutEffect.Destorying();
        }

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


        //
        //	Export Methods
        //


        public void Init(SceneConfig sceneConfig, MagicWallManager manager)
        {
            _manager = manager;

            _theCutEffect = GetCutEffect(sceneConfig); // 设置过场效果
            _theCutEffect.Init(_manager);
            _dataType = sceneConfig.dataType; // 设置类型
            _magicSceneEnumStatus = MagicSceneEnum.Running;
        }


        //  运行
        public bool Run()
        {
            MagicWallManager magicWallManager = _manager;

            // 准备状态
            if (Status == SceneStatus.PREPARING)
            {
                // 进入过场状态
                //Debug.Log("Scene is Cutting");
                _startTime = Time.time; //标记开始的时间

                _manager.SceneStatus = WallStatusEnum.Cutting;   //标记项目进入过场状态

                DoCreating();  //初始化场景

                // 将状态标志设置为开始
                Status = SceneStatus.STARTTING;
            }

            // 过场动画
            if (Status == SceneStatus.STARTTING)
            {
                if ((Time.time - _startTime) > _theCutEffect.StartingDurTime)
                {
                    // 完成开场动画，场景进入展示状态

                    // 调用效果完成回调
                    _theCutEffect.OnStartingCompleted();

                    _displayTime = Time.time;
                    Status = SceneStatus.DISPLAYING;
                    magicWallManager.SceneStatus = WallStatusEnum.Displaying;
                }
                else
                {
                    DoStarting();
                }
            }

            // 正常展示
            if (Status == SceneStatus.DISPLAYING)
            {
                // 过场状态具有运行状态 或 已达到运行的时间
                if (!_theCutEffect.HasDisplaying || (Time.time - _displayTime) > _theCutEffect.DisplayDurTime)
                {
                    // 完成展示阶段，进行销毁
                    OnRunCompleted();


                    //            DoDestorying();
                    //Status = SceneStatus.DESTORING;
                    //            _destoryTime = Time.time;
                }
                else
                {
                    DoDisplaying();
                }
            }

            // 销毁中
            //if (Status == SceneStatus.DESTORING)
            //{
            //          // 达到销毁的时间
            //          if ((Time.time - _destoryTime) > _theCutEffect.DestoryDurTime)
            //	{
            //              OnRunEndCompleted();

            //  //            if (DoDestoryCompleted())
            //		//{
            //  //                Status = SceneStatus.PREPARING;
            //		//	return false;
            //		//}
            //		//else
            //		//{

            //  //                DoDestorying();
            //		//}
            //	}
            //	else
            //	{
            //              RunEnd();
            //           }
            //}
            return true;
        }

        public DataTypeEnum GetDataType()
        {
            return _dataType;
        }



        /// <summary>
        ///     获取过场
        /// </summary>
        /// <param name="sceneTypeEnum"></param>
        /// <returns></returns>
        private CutEffect GetCutEffect(SceneConfig sceneConfig)
        {
            CutEffect cutEffect = null;

            var sceneTypeEnum = sceneConfig.sceneType;

            if (sceneTypeEnum == SceneTypeEnum.CurveStagger)
            {
                cutEffect = new CurveStaggerCutEffect();
            }
            else if (sceneTypeEnum == SceneTypeEnum.FrontBackUnfold)
            {
                cutEffect = new FrontBackUnfoldCutEffect();
            }
            else if (sceneTypeEnum == SceneTypeEnum.LeftRightAdjust)
            {
                cutEffect = new LeftRightAdjustCutEffect();
            }
            else if (sceneTypeEnum == SceneTypeEnum.MidDisperse)
            {
                cutEffect = new MidDisperseCutEffect();
            }
            else if (sceneTypeEnum == SceneTypeEnum.UpDownAdjustCutEffect)
            {
                cutEffect = new UpDownAdjustCutEffect();
            }

            if (cutEffect == null)
            {
                return null;
            }
            else
            {
                cutEffect.DisplayDurTime = sceneConfig.durtime;

                return cutEffect;
            }


        }

        #region Implement

        public void OnRunCompleted()
        {
            _destoryTime = Time.time;
            _onRunCompleted.Invoke();
            Status = SceneStatus.PREPARING;

            _magicSceneEnumStatus = MagicSceneEnum.RunningComplete;

        }

        public void SetOnRunCompleted(Action onRunCompleted)
        {
            _onRunCompleted = onRunCompleted;
        }

        public void RunEnd()
        {
            if ((Time.time - _destoryTime) > _theCutEffect.DestoryDurTime)
            {
                OnRunEndCompleted();
            }
            else
            {
                _magicSceneEnumStatus = MagicSceneEnum.RunningEnd;
                _theCutEffect.Destorying();
            }
        }

        public void OnRunEndCompleted()
        {
            if (!doDestoryCompleting)
            {
                doDestoryCompleting = true;
                _manager.mainPanel.GetComponent<CanvasGroup>().alpha = 1;
                _manager.mainPanel.GetComponentInChildren<CanvasGroup>().alpha = 1;
                _manager.Clear();

                _magicSceneEnumStatus = MagicSceneEnum.RunningComplete;
                _onRunEndCompleted.Invoke();
            }
            else
            {

            }
        }

        public void SetOnRunEndCompleted(Action onRunEndCompleted)
        {
            _onRunEndCompleted = onRunEndCompleted;
        }

        public MagicSceneEnum GetSceneStatus()
        {
            return _magicSceneEnumStatus;
        }

        #endregion
    }
}