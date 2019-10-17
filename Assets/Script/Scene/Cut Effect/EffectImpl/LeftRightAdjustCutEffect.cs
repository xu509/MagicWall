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
    public class LeftRightAdjustCutEffect : CutEffect
    {
        private DisplayBehaviorConfig _displayBehaviorConfig;   //  Display Behavior Config

        private float _startingTimeWithOutDelay;
        private float _timeBetweenStartAndDisplay = 0.05f; //完成启动动画与表现动画之间的时间

        //
        //  Init
        //
        public override void Init(MagicWallManager manager)
        {

            //  初始化 manager
            _manager = manager;
            _agentManager = manager.agentManager;
            _daoService = manager.daoService;

            //  获取持续时间
            //StartingDurTime = 2f;
            StartingDurTime = manager.cutEffectConfig.LeftRightDisplayDurTime;
            _startingTimeWithOutDelay = StartingDurTime;
            DestoryDurTime = 0.5f;

            // 获取Display的动画
            DisplayBehavior = new GoLeftDisplayBehavior();

            // 获取销毁的动画
            DestoryBehavior = new FadeOutDestoryBehavior();
            DestoryBehavior.Init(_manager, DestoryDurTime);

            //  初始化 config
            _displayBehaviorConfig = new DisplayBehaviorConfig();
        }


        public override void Starting()
        {
            for (int i = 0; i < _agentManager.Agents.Count; i++)
            {
                FlockAgent agent = _agentManager.Agents[i];
                Vector2 agent_vector2 = agent.GenVector2;
                Vector2 ori_vector2 = agent.OriVector2;

                // 延时的时间
                float delay_time = agent.DelayTime;

                // 获取此 agent 需要的动画时间
                float run_time = _startingTimeWithOutDelay - delay_time - _timeBetweenStartAndDisplay;

                // 当前总运行的时间;
                float time = Time.time - StartTime;


                float aniTime = Time.time - StartTime - delay_time;

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
        }

        public override void OnStartingCompleted()
        {
            //  初始化表现形式
            Debug.Log("OnStartingCompleted");



            _displayBehaviorConfig.dataType = dataType;
            _displayBehaviorConfig.DisplayTime = DisplayDurTime;
            _displayBehaviorConfig.Manager = _manager;
            _displayBehaviorConfig.sceneUtils = _sceneUtil;
            DisplayBehavior.Init(_displayBehaviorConfig);

            for (int i = 0; i < _manager.agentManager.Agents.Count; i++) {
                _manager.agentManager.Agents[i].flockStatus = FlockStatusEnum.NORMAL;
            }           
        }


        /// <summary>
        /// 创建代理
        /// </summary>
        private void CreateAgency(DataTypeEnum dataType)
        {
            Debug.Log("开始加载左右动画");


            // 固定高度
            int _row = _manager.Row;
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
                        , ori_x, ori_y, row, column, itemWidth, _itemHeight, data);
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

            StartingDurTime += _maxDelayTime;

            Debug.Log("_maxDelayTime : " + _maxDelayTime);
            Debug.Log("StartingDurTime : " + StartingDurTime);


        }

        public override string GetID()
        {
            return "LeftRightAdjustCutEffect";
        }

        protected override void CreateAgents(DataTypeEnum dataType)
        {
            CreateAgency(dataType);
        }
    }
}