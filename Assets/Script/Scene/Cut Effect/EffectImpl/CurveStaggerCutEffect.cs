using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using EasingUtil;
using System;

// 过场效果 1 ，曲线麻花效果
namespace MagicWall
{
    public class CurveStaggerCutEffect : ICutEffect
    {
        MagicWallManager _manager;

        private float _displayTime;
        private float _entranceDisplayTime;
        private float _startTime;

        private CutEffectDisplayBehavior _displayBehavior;
        private CutEffectDestoryBehavior _destoryBehavior;
        private SceneUtils _sceneUtil;
        private DataTypeEnum _dataTypeEnum;
        private CutEffectStatus _cutEffectStatus;


        private DisplayBehaviorConfig _displayBehaviorConfig;   //  Display Behavior Config
        private float _startDelayTime = 0f;  //启动的延迟时间
        private float _startingTimeWithOutDelay;
        private float _timeBetweenStartAndDisplay = 0.05f; //完成启动动画与表现动画之间的时间

        private Action _onStartCompleted;
        private Action _onEffectEnd;
        private Action _onDisplayStart;
        private Action _onDestoryStart;
        private Action _onDestoryCompleted;


        public void Init(MagicWallManager manager, SceneConfig sceneConfig, 
            Action OnEffectEnd, Action OnDisplayStart, Action OnStartCompleted,
            Action OnDestoryStart,Action OnDestoryCompleted)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            //  初始化 manager
            _manager = manager;
            _displayTime = sceneConfig.durtime;
            _dataTypeEnum = sceneConfig.dataType;

            _onEffectEnd = OnEffectEnd;
            _onDisplayStart = OnDisplayStart;
            _onStartCompleted = OnStartCompleted;
            _onDestoryStart = OnDestoryStart;
            _onDestoryCompleted = OnDestoryCompleted;

            //  获取持续时间
            _entranceDisplayTime = manager.cutEffectConfig.CurveStaggerDisplayDurTime;
            _startingTimeWithOutDelay = _entranceDisplayTime;

            // 获取Display的动画
            _displayBehavior = DisplayBehaviorFactory.GetBehavior(sceneConfig.displayBehavior);

            // 获取销毁的动画
            _destoryBehavior = DestoryBehaviorFactory.GetBehavior(sceneConfig.destoryBehavior);
            _destoryBehavior.Init(_manager, () =>
            {
                //结束时
                _cutEffectStatus = CutEffectStatus.Init;
                _onDestoryCompleted.Invoke();
            });

            //  初始化 config
            _displayBehaviorConfig = new DisplayBehaviorConfig();

            _sceneUtil = new SceneUtils(_manager);
            _cutEffectStatus = CutEffectStatus.PreparingCompleted;

            sw.Stop();
        }


        private void CreateItem(DataTypeEnum dataType)
        {

            // 固定高度
            int _row = _manager.Row;
            int _itemHeight = _sceneUtil.GetFixedItemHeight();
            float gap = _sceneUtil.GetGap();

            int _nearColumn = Mathf.RoundToInt(_manager.mainPanel.rect.width / (_itemHeight + gap));
            float w = _manager.mainPanel.rect.width;


            // 从上至下，生成
            for (int row = 0; row < _row; row++)
            {
                int column = 0;

                ItemPositionInfoBean itemPositionInfoBean;
                if (_displayBehaviorConfig.rowAgentsDic.ContainsKey(row))
                {
                    itemPositionInfoBean = _displayBehaviorConfig.rowAgentsDic[row];
                }
                else
                {
                    itemPositionInfoBean = new ItemPositionInfoBean();
                    _displayBehaviorConfig.rowAgentsDic.Add(row, itemPositionInfoBean);
                }

                int gen_x_position = itemPositionInfoBean.xposition;

                while (gen_x_position < _manager.mainPanel.rect.width)
                {
                    // 获取数据
                    //FlockData data = _daoService.GetFlockData(dataType);
                    FlockData data = _manager.daoService.GetFlockDataByScene(dataType,_manager.SceneIndex);
                    Sprite coverSprite = data.GetCoverSprite();
                    float itemWidth = AppUtils.GetSpriteWidthByHeight(coverSprite, _itemHeight);

                    int ori_y = Mathf.RoundToInt(_sceneUtil.GetYPositionByFixedHeight(_itemHeight, row));
                    int ori_x = Mathf.RoundToInt(gen_x_position + itemWidth / 2 + gap / 2);

                    int middleY = _row / 2;
                    int middleX = _nearColumn / 2;

                    float delayX = column * 0.06f;
                    float delayY;

                    // 定义源位置
                    float gen_x, gen_y;

                    if (row < middleY)
                    {
                        delayY = System.Math.Abs(middleY - row) * 0.3f;
                        gen_x = w + (middleY - row) * 500;
                        gen_y = w - ori_x + (_row - 1) * _itemHeight;
                    }
                    else
                    {
                        delayY = (System.Math.Abs(middleY - row) + 1) * 0.3f;
                        gen_x = w + (row - middleY + 1) * 500;
                        gen_y = -(w - ori_x) + 2 * _itemHeight;
                    }

                    //生成 agent
                    Vector2 genPosition = new Vector2(gen_x, gen_y);
                    FlockAgent go = FlockAgentFactoryInstance.Generate(_manager, genPosition, AgentContainerType.MainPanel
                        , ori_x, ori_y, row, column, itemWidth, _itemHeight, data);
                    go.flockStatus = FlockStatusEnum.RUNIN;

                    // 装载延迟参数
                    go.DelayX = delayX;
                    go.DelayY = delayY;

                    // 生成透明度动画
                    go.GetComponentInChildren<Image>().DOFade(0, _entranceDisplayTime - delayX + delayY).From();

                    // 获取启动动画的延迟时间
                    if ((delayY - delayX) > _startDelayTime)
                    {
                        _startDelayTime = delayY - delayX;
                    }

                    gen_x_position = Mathf.RoundToInt(gen_x_position + itemWidth + gap / 2);
                    _displayBehaviorConfig.rowAgentsDic[row].xposition = gen_x_position;
                    _displayBehaviorConfig.rowAgentsDic[row].xPositionMin = 0;
                    _displayBehaviorConfig.rowAgentsDic[row].column = column;

                    column++;
                }
            }

            _entranceDisplayTime += _startDelayTime;

            OnCreateCompleted();
        }


        #region 动画实现
        public void Starting()
        {
            float time = Time.time - _startTime;  // 当前已运行的时间;

            for (int i = 0; i < _manager.agentManager.Agents.Count; i++)
            {
                FlockAgent agent = _manager.agentManager.Agents[i];
                //Vector2 agent_vector2 = agent.GetComponent<RectTransform> ().anchoredPosition;
                Vector2 agent_vector2 = agent.GenVector2;
                Vector2 ori_vector2 = agent.OriVector2;

                float aniTime = (_startingTimeWithOutDelay - agent.DelayX + agent.DelayY); // 动画运行的总时间


                // 此时有两种特殊状态，还未移动的与已移动完成的
                if (time > aniTime)
                {
                    // 此时可能未走完动画
                    if (!agent.isCreateSuccess)
                    {
                        agent.SetChangedPosition(ori_vector2);
                        agent.isCreateSuccess = true;
                    }
                }
                else {
                    float t = time / aniTime;
                    Func<float, float> defaultEasingFunction = EasingFunction.Get(_manager.cutEffectConfig.CurveStaggerDisplayEaseEnum);
                    t = defaultEasingFunction(t);
                    Vector2 to = Vector2.Lerp(agent_vector2, ori_vector2, t);
                    agent.SetChangedPosition(to);
                }

                //if ((time - _entranceDisplayTime * 0.8f) > 0) {
                //    _onDisplayStart.Invoke();
                //}
            }

            if ((time - _entranceDisplayTime) > 0) {
                _onDisplayStart.Invoke();
                _onStartCompleted.Invoke();
                Debug.Log("_onStartCompleted completed");

            }
        }
        #endregion

        public void OnCreateCompleted()
        {
            //  初始化表现形式
            _displayBehaviorConfig.dataType = _dataTypeEnum;
            _displayBehaviorConfig.DisplayTime = _displayTime;
            _displayBehaviorConfig.Manager = _manager;
            _displayBehaviorConfig.sceneUtils = _sceneUtil;
            _displayBehavior.Init(_displayBehaviorConfig);

            for (int i = 0; i < _manager.agentManager.Agents.Count; i++)
            {
                if (_manager.agentManager.Agents[i].flockStatus == FlockStatusEnum.RUNIN)
                {
                    _manager.agentManager.Agents[i].flockStatus = FlockStatusEnum.NORMAL;
                }
            }

            Debug.Log("_displayBehavior init completed");

        }



        public void RunEntrance()
        {
            if (_cutEffectStatus == CutEffectStatus.Init) {
                _cutEffectStatus = CutEffectStatus.Preparing;

                _displayBehaviorConfig = new DisplayBehaviorConfig();

                Debug.Log("RecoverFromFade");

                _manager.RecoverFromFade();
                _cutEffectStatus = CutEffectStatus.PreparingCompleted;
            }

            if (_cutEffectStatus == CutEffectStatus.PreparingCompleted) {
                _cutEffectStatus = CutEffectStatus.Creating;
                CreateItem(_dataTypeEnum);
                _cutEffectStatus = CutEffectStatus.CreatingCompleted;
            }
            if (_cutEffectStatus == CutEffectStatus.CreatingCompleted) {
                _cutEffectStatus = CutEffectStatus.Creating;

                _startTime = Time.time;
            }
            if (_cutEffectStatus == CutEffectStatus.Creating) {
                Starting();
            }

        }

        public void RunDisplaying()
        {
            _displayBehavior.Run();
        }

        public void RunDestoring()
        {
            _destoryBehavior.Run();            
        }

        public SceneTypeEnum GetSceneType()
        {
            return SceneTypeEnum.CurveStagger;
        }

        public void Run()
        {
            var runTime = Time.time - _startTime;
            var time = _entranceDisplayTime + _displayTime;

            if ((runTime - time) > 0) {
                // 开始结束动画
                _onDestoryStart.Invoke();
            }            
        }


    }
}