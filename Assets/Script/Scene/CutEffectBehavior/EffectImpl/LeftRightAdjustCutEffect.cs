using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using EasingUtil;

// 过场效果 5，左右校准
namespace MagicWall
{
    public class LeftRightAdjustCutEffect : ICutEffect
    {
        MagicWallManager _manager;
        private IDaoService _daoService;
        private SceneConfig _sceneConfig;



        private float _entranceDisplayTime;
        private float _startTime;

        private SceneUtils _sceneUtil;
        private DataTypeEnum _dataTypeEnum;
        private CutEffectStatus _cutEffectStatus;

        private DisplayBehaviorConfig _displayBehaviorConfig;   //  Display Behavior Config
        private float _startDelayTime = 0f;  //启动的延迟时间
        private float _startingTimeWithOutDelay;

        private Action _onEffectCompleted;
        private Action _onDisplayStart;
        private Action<DisplayBehaviorConfig> _onCreateAgentCompleted;

        private bool _hasCallDisplay = false;


        private int row_set;


        //
        //  Init
        //
        public void Init(MagicWallManager manager, SceneConfig sceneConfig,
            Action<DisplayBehaviorConfig> OnCreateAgentCompleted,
            Action OnEffectCompleted, Action OnDisplayStart
            )
        {
            //  初始化 manager
            _manager = manager;
            _sceneConfig = sceneConfig;

            _dataTypeEnum = sceneConfig.dataType;

            _daoService = _manager.daoServiceFactory.GetDaoService(sceneConfig.daoTypeEnum);

            if (sceneConfig.isKinect == 0)
            {
                row_set = 6;
            }
            else {
                row_set = 12;
            }


            _onCreateAgentCompleted = OnCreateAgentCompleted;
            _onEffectCompleted = OnEffectCompleted;
            _onDisplayStart = OnDisplayStart;

        }


        public void Starting()
        {
            float time = Time.time - _startTime;  // 当前已运行的时间;

            for (int i = 0; i < _manager.agentManager.Agents.Count; i++)
            {
                FlockAgent agent = _manager.agentManager.Agents[i];
                Vector2 agent_vector2 = agent.GenVector2;
                Vector2 ori_vector2 = agent.OriVector2;

                // 延时的时间
                float delay_time = agent.DelayTime;

                // 获取此 agent 需要的动画时间
                float run_time = _startingTimeWithOutDelay - delay_time;

                float aniTime = Time.time - _startTime - delay_time;

                if (aniTime < 0)
                {
                    continue;
                }
                else if (aniTime > _startingTimeWithOutDelay)
                {
                    // 此时可能未走完动画
                    if (!agent.isCreateSuccess)
                    {
                        agent.SetChangedPosition(ori_vector2);

                        agent.isCreateSuccess = true;
                    }

                    continue;
                }
                else {
                    float t = aniTime / _startingTimeWithOutDelay;

                    Func<float, float> defaultEasingFunction = EasingFunction.Get(_manager.cutEffectConfig.LeftRightDisplayEaseEnum);

                    t = defaultEasingFunction(t);

                    Vector2 to = Vector2.Lerp(agent_vector2, ori_vector2, t);

                    agent.SetChangedPosition(to);
                }
            }

            if ((time - _entranceDisplayTime * 0.8f) > 0)
            {
                if (!_hasCallDisplay)
                {
                    _hasCallDisplay = true;
                    _onDisplayStart.Invoke();
                }
            }

            if ((time - _entranceDisplayTime) > 0)
            {
                Reset();
                _onEffectCompleted.Invoke();
            }
        }




        /// <summary>
        /// 创建代理
        /// </summary>
        private void CreateItem(DataTypeEnum dataType)
        {
            Debug.Log("开始加载左右动画");
            //  获取持续时间
            _entranceDisplayTime = _manager.cutEffectConfig.LeftRightDisplayDurTime;
            _startingTimeWithOutDelay = _entranceDisplayTime;

            //  初始化 config
            _displayBehaviorConfig = new DisplayBehaviorConfig();
            _sceneUtil = new SceneUtils(_manager, _sceneConfig.isKinect);

            // 固定高度
            int _row = row_set;
            int _itemHeight = _sceneUtil.GetFixedItemHeight();
            float gap = _sceneUtil.GetGap();

            int _nearColumn = Mathf.RoundToInt(_manager.mainPanel.rect.width / (_itemHeight + gap));
            float w = _manager.mainPanel.rect.width;

            float _maxDelayTime = 0f;


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
                    FlockData data = _daoService.GetFlockDataByScene(dataType,_manager.SceneIndex);
                    Sprite coverSprite = data.GetCoverSprite();
                    float itemWidth = AppUtils.GetSpriteWidthByHeight(coverSprite, _itemHeight);

                    int ori_y = Mathf.RoundToInt(_sceneUtil.GetYPositionByFixedHeight(_itemHeight, row));
                    int ori_x = Mathf.RoundToInt(gen_x_position + itemWidth / 2 + gap / 2);

                    // 获取参照点
                    int middleY = _row  / 2;
                    int middleX = _nearColumn / 2;

                    // 定义出生位置
                    float gen_x, gen_y;

                    // 计算出生位置与延时时间
                    float delay;

                    int maxYOffset = System.Math.Abs(middleY - _row);
                    int nowYOffset;

                    if (row < middleY)
                    {
                        nowYOffset = System.Math.Abs(middleY - row);
                        //delay = (System.Math.Abs(middleY - row)) * _manager.cutEffectConfig.LeftRightGapTime;
                        gen_x = ori_x + w;
                    }
                    else
                    {
                        nowYOffset = System.Math.Abs(middleY - row) + 1;
                        //delay = (System.Math.Abs(middleY - row) + 1) * _manager.cutEffectConfig.LeftRightGapTime;
                        gen_x = ori_x - w - 500;
                    }

                    float f = (float )nowYOffset / (float) maxYOffset;
                    //Debug.Log("f : " + f);
                    Func<float, float> lrfun = EasingFunction.Get(_manager.cutEffectConfig.LeftRightGapEaseEnum);
                    f = lrfun(f);

                    delay = Mathf.Lerp(0, maxYOffset * _manager.cutEffectConfig.LeftRightGapTime, f);

                    gen_y = ori_y; //纵坐标不变

                    //生成 agent
                    Vector2 genPosition = new Vector2(gen_x, gen_y);
                    FlockAgent go = FlockAgentFactoryInstance.Generate(_manager, genPosition, AgentContainerType.MainPanel
                        , ori_x, ori_y, row, column, itemWidth, _itemHeight, data, _sceneConfig.daoTypeEnum);
                    go.flockStatus = FlockStatusEnum.RUNIN;

                    go.Delay = delay;
                    go.DelayTime = delay;

                    if (delay > _maxDelayTime)
                        _maxDelayTime = delay;

                    gen_x_position = Mathf.RoundToInt(gen_x_position + itemWidth + gap / 2);
                    _displayBehaviorConfig.rowAgentsDic[row].xposition = gen_x_position;
                    _displayBehaviorConfig.rowAgentsDic[row].xPositionMin = 0;
                    _displayBehaviorConfig.rowAgentsDic[row].column = column;

                    column++;
                }
            }
            Debug.Log("_entranceDisplayTime : " + _entranceDisplayTime);

            _entranceDisplayTime += _startDelayTime;

            _displayBehaviorConfig.dataType = _dataTypeEnum;
            _displayBehaviorConfig.Manager = _manager;
            _displayBehaviorConfig.sceneUtils = _sceneUtil;
            _displayBehaviorConfig.sceneConfig = _sceneConfig;


            _onCreateAgentCompleted.Invoke(_displayBehaviorConfig);
        }


        public void Run()
        {
            if (_cutEffectStatus == CutEffectStatus.Init)
            {
                _cutEffectStatus = CutEffectStatus.Preparing;
                _manager.RecoverFromFade();
                _cutEffectStatus = CutEffectStatus.PreparingCompleted;
            }

            if (_cutEffectStatus == CutEffectStatus.PreparingCompleted)
            {
                _cutEffectStatus = CutEffectStatus.Creating;
                CreateItem(_dataTypeEnum);
                _cutEffectStatus = CutEffectStatus.CreatingCompleted;
            }
            if (_cutEffectStatus == CutEffectStatus.CreatingCompleted)
            {
                _cutEffectStatus = CutEffectStatus.Creating;

                _startTime = Time.time;
            }
            if (_cutEffectStatus == CutEffectStatus.Creating)
            {
                Starting();
            }
        }

        public SceneTypeEnum GetSceneType()
        {
            return SceneTypeEnum.LeftRightAdjust;
        }


        private void Reset() {
            _hasCallDisplay = false;
            _cutEffectStatus = CutEffectStatus.Init;
        }

    }
}