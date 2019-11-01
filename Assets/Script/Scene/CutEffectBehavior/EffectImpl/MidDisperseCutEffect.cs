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
    public class MidDisperseCutEffect : ICutEffect
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

                // 获取总运行时间
                float run_time = _startingTimeWithOutDelay + agent.Delay;


                if (time > run_time)
                {
                    // 此时可能未走完动画
                    if (!agent.isCreateSuccess)
                    {
                        agent.SetChangedPosition(ori_vector2);
                        agent.flockStatus = FlockStatusEnum.NORMAL;

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

            if ((time - _entranceDisplayTime * 0.5f) > 0)
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



        private void CreateItem(DataTypeEnum dataType)
        {
            //  获取持续时间
            _entranceDisplayTime = _manager.cutEffectConfig.LeftRightDisplayDurTime;
            _startingTimeWithOutDelay = _entranceDisplayTime;

            //  初始化 config
            _displayBehaviorConfig = new DisplayBehaviorConfig();
            _sceneUtil = new SceneUtils(_manager, _sceneConfig.isKinect);

            List<FlockAgent> agents = new List<FlockAgent>();
            _displayBehaviorConfig.dataType = dataType;
            _displayBehaviorConfig.sceneUtils = _sceneUtil;


            int _column = 0;
            if (_sceneConfig.isKinect == 0)
            {
                _column = _manager.managerConfig.Column; 
            }
            else {
                _column = _manager.managerConfig.KinectColumn;
            }

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

                    float minDelay = 0;
                    //float maxDelay = middleX * 0.05f;
                    float maxDelay = _manager.cutEffectConfig.MidDisperseDelayMax;
                    int offset = Mathf.Abs(middleX - j);

                    float k = (float)offset / (float)middleX;
                    Func<float, float> easeFunction = EasingFunction.Get(_manager.cutEffectConfig.MidDisperseMoveEaseEnum);
                    k = easeFunction(k);

                    //float delay = Mathf.Lerp(minDelay, maxDelay, k);
                    float delay = 0;
                    float gen_x, gen_y;

                    gen_x = middleX * (_itemWidth + gap) + (_itemWidth / 2);
                    gen_y = ori_y + itemHeigth / 3
                        + itemHeigth * _manager.cutEffectConfig.MidDisperseHeightFactor;

                    // 创建agent
                    //FlockAgent go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y, row, j,
                    //         _itemWidth, itemHeigth, data, AgentContainerType.MainPanel);

                    FlockAgent go = FlockAgentFactoryInstance.Generate(_manager, new Vector2(gen_x,gen_y), AgentContainerType.MainPanel
    , ori_x, ori_y, row, j, _itemWidth, itemHeigth, data,_sceneConfig.daoTypeEnum);

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
            _entranceDisplayTime = _startingTimeWithOutDelay + _startDelayTime + 0.05f;

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
            return SceneTypeEnum.MidDisperse;
        }


        private void Reset()
        {
            _hasCallDisplay = false;
            _cutEffectStatus = CutEffectStatus.Init;
        }

        public void InitData()
        {
            _cutEffectStatus = CutEffectStatus.Init;
            _hasCallDisplay = false;
        }
    }
}