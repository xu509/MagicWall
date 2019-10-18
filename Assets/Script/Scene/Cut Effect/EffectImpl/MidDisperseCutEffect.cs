using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using EasingUtil;

// 过场效果 2 中间散开 
namespace MagicWall
{
    public class MidDisperseCutEffect : CutEffect
    {


        private float _startDelayTime = 0f;  //启动的延迟时间
        private float _startingTimeWithOutDelay;
        private float _timeBetweenStartAndDisplay = 0.05f; //完成启动动画与表现动画之间的时间

        private DisplayBehaviorConfig _displayBehaviorConfig;   //  Display Behavior Config


        //
        //  Init
        //
        public override void Init(MagicWallManager manager, SceneConfig sceneConfig)
        {
            //  初始化 manager
            _manager = manager;
            _agentManager = manager.agentManager;
            _daoService = manager.daoService;

            DisplayDurTime = sceneConfig.durtime;

            //  获取持续时间
            //StartingDurTime = 0.35f;
            StartingDurTime = manager.cutEffectConfig.MidDisperseDisplayTime;
            _startingTimeWithOutDelay = StartingDurTime;
            DestoryDurTime = 0.5f;

            ////  设置显示的时间
            //string t = _daoService.GetConfigByKey(AppConfig.KEY_CutEffectDuring_MidDisperseAdjust).Value;
            //DisplayDurTime = AppUtils.ConvertToFloat(t);

            // 获取Display的动画
            DisplayBehavior = DisplayBehaviorFactory.GetBehavior(sceneConfig.displayBehavior);

            // 获取销毁的动画
            DestoryBehavior = DestoryBehaviorFactory.GetBehavior(sceneConfig.destoryBehavior);
            DestoryBehavior.Init(_manager, ()=> {
                //on destory completed
            });

            //  初始化 config
            _displayBehaviorConfig = new DisplayBehaviorConfig();


            InitConfig();
        }


        public override void Starting()
        {

            //DisplayBehavior.Run();

            for (int i = 0; i < _agentManager.Agents.Count; i++)
            {
                FlockAgent agent = _agentManager.Agents[i];
                Vector2 agent_vector2 = agent.GenVector2;
                Vector2 ori_vector2 = agent.OriVector2;

                // 获取总运行时间
                float run_time = _startingTimeWithOutDelay + agent.Delay;

                // 当前已运行的时间;
                float time = Time.time - StartTime;

                if (time > run_time)
                {
                    // 此时可能未走完动画
                    if (!agent.isCreateSuccess)
                    {
                        agent.SetChangedPosition(ori_vector2);

                        agent.isCreateSuccess = true;
                    }
                    continue;
                }

                float t = time / run_time;

                Func<float, float> moveEase = EasingFunction.Get(_manager.cutEffectConfig.MidDisperseMoveEaseEnum);
                t = moveEase(t);


                Vector2 to = Vector2.Lerp(agent_vector2, ori_vector2, t);
                agent.SetChangedPosition(to);


                // 透明度动画
                var distance = Vector2.Distance(ori_vector2, to);

                var agentHeight = agent.GetComponent<RectTransform>().rect.height;


                var maxDistance = agentHeight * _manager.cutEffectConfig.MidDisperseAlphaMinDistanceFactor;

                if (distance < maxDistance)
                {
                    float min_alpha = 0.1f;
                    float max_alpha = 1f;
                    float k = distance / maxDistance;
                    Func<float, float> easeFunction = EasingFunction.Get(_manager.cutEffectConfig.MidDisperseAlphaEaseEnum);
                    k = easeFunction(k);
                    float newAlpha = Mathf.Lerp(max_alpha, min_alpha, k);
                    agent.UpdateImageAlpha(newAlpha);
                }

            }

        }

        public override void OnStartingCompleted()
        {
            //  初始化表现形式
            _displayBehaviorConfig.dataType = dataType;
            _displayBehaviorConfig.DisplayTime = DisplayDurTime;
            _displayBehaviorConfig.Manager = _manager;
            _displayBehaviorConfig.sceneUtils = _sceneUtil;
            DisplayBehavior.Init(_displayBehaviorConfig);

            for (int i = 0; i < _manager.agentManager.Agents.Count; i++)
            {
                if (_manager.agentManager.Agents[i].flockStatus == FlockStatusEnum.RUNIN)
                {
                    _manager.agentManager.Agents[i].flockStatus = FlockStatusEnum.NORMAL;
                }
            }

        }



        private void CreateAgency(DataTypeEnum dataType)
        {
            List<FlockAgent> agents = new List<FlockAgent>();
            _displayBehaviorConfig.dataType = dataType;
            _displayBehaviorConfig.sceneUtils = _sceneUtil;

            int _column = _manager.managerConfig.Column;
            int _itemWidth = _sceneUtil.GetFixedItemWidth();
            float gap = _sceneUtil.GetGap();


            int middleX = _column / 2;

            //从下往上，从左往右
            for (int j = 0; j < _column; j++)
            {
                int row = 0;

                // 获取该列的 gen_y
                ItemPositionInfoBean itemPositionInfoBean;
                if (_displayBehaviorConfig.columnAgentsDic.ContainsKey(j))
                {
                    itemPositionInfoBean = _displayBehaviorConfig.columnAgentsDic[j];
                }
                else
                {
                    itemPositionInfoBean = new ItemPositionInfoBean();
                    _displayBehaviorConfig.columnAgentsDic.Add(j, itemPositionInfoBean);
                }

                int gen_y_position = itemPositionInfoBean.yposition;
                int ori_x = Mathf.RoundToInt(_sceneUtil.GetXPositionByFixedWidth(_itemWidth, j));

                while (gen_y_position < _manager.mainPanel.rect.height)
                {
                    // 获取数据
                    //FlockData data = _daoService.GetFlockData(dataType);
                    FlockData data = _daoService.GetFlockDataByScene(dataType,_manager.SceneIndex);
                    Sprite coverSprite = data.GetCoverSprite();
                    float itemHeigth = AppUtils.GetSpriteHeightByWidth(coverSprite, _itemWidth);

                    int ori_y = Mathf.RoundToInt(gen_y_position + itemHeigth / 2);

                    //float delay = System.Math.Abs(middleX - j) * 0.05f;

                    float minDelay = 0;
                    //float maxDelay = middleX * 0.05f;
                    float maxDelay = _manager.cutEffectConfig.MidDisperseDelayMax;
                    int offset = Mathf.Abs(middleX - j);

                    float k = (float)offset / (float)middleX;
                    Func<float, float> easeFunction = EasingFunction.Get(_manager.cutEffectConfig.MidDisperseMoveEaseEnum);
                    k = easeFunction(k);

                    //float delay = Mathf.Lerp(minDelay, maxDelay, k);
                    float delay = 0;

                    if (delay > _timeBetweenStartAndDisplay)
                    {
                        _timeBetweenStartAndDisplay = delay;
                    }

                    //Debug.Log("delay ater : " + delay);

                    float gen_x, gen_y;

                    gen_x = middleX * (_itemWidth + gap) + (_itemWidth / 2);
                    gen_y = ori_y + itemHeigth / 3
                        + itemHeigth * _manager.cutEffectConfig.MidDisperseHeightFactor;

                    // 创建agent
                    //FlockAgent go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y, row, j,
                    //         _itemWidth, itemHeigth, data, AgentContainerType.MainPanel);

                    FlockAgent go = FlockAgentFactoryInstance.Generate(_manager, new Vector2(gen_x,gen_y), AgentContainerType.MainPanel
    , ori_x, ori_y, row, j, _itemWidth, itemHeigth, data);

                    go.flockStatus = FlockStatusEnum.RUNIN;

                    // 初始化透明度
                    go.UpdateImageAlpha(0.1f);

                    go.Delay = delay;
                    agents.Add(go);
                    //go.UpdateImageAlpha(1f - Mathf.Abs(j - middleX)*0.05f);
                    if (delay > _startDelayTime)
                    {
                        _startDelayTime = delay;
                    }

                    gen_y_position = Mathf.RoundToInt(gen_y_position + itemHeigth + gap);
                    _displayBehaviorConfig.columnAgentsDic[j].yposition = gen_y_position;
                    _displayBehaviorConfig.columnAgentsDic[j].row = row;
                    row++;
                }

            }

            // 调整显示的前后
            agents.Sort((x, y) =>
            {
                return Mathf.Abs(x.Y - middleX).CompareTo(Mathf.Abs(y.Y - middleX));
            });

            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].GetComponent<RectTransform>().SetAsFirstSibling();
            }



            // 调整启动动画的时间
            StartingDurTime = _startingTimeWithOutDelay + _startDelayTime + 0.05f;
            //Debug.Log("StartingDurTime : " + StartingDurTime);
            //Debug.Log("_startDelayTime : " + _startDelayTime);
            //Debug.Log("_startingTimeWithOutDelay : " + _startingTimeWithOutDelay);
        }

        public override string GetID()
        {
            return "MidDisperseCutEffect";
        }


        private void InitConfig()
        {
            _displayBehaviorConfig.dataType = dataType;
            //_displayBehaviorConfig.DisplayTime = DisplayDurTime;
            _displayBehaviorConfig.Manager = _manager;
            _displayBehaviorConfig.sceneUtils = _sceneUtil;
            DisplayBehavior.Init(_displayBehaviorConfig);
        }

        protected override void CreateAgents(DataTypeEnum dataType)
        {
            CreateAgency(dataType);
        }
    }
}