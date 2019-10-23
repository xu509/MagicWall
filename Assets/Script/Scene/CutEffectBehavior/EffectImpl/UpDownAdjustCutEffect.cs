using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using EasingUtil;

// 过场效果 4，上下校准
namespace MagicWall
{
    public class UpDownAdjustCutEffect : ICutEffect
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

                // 获取此 agent 需要的动画时间
                float aniTime = time;

                // 如果总动画时间超出 agent 需要的动画时间，则不进行处理
                if (aniTime > _entranceDisplayTime)
                {
                    // 此时可能未走完动画
                    if (!agent.isCreateSuccess)
                    {
                        agent.SetChangedPosition(ori_vector2);
                        agent.isCreateSuccess = true;
                    }
                }
                else {
                    float t = aniTime / _entranceDisplayTime;
                    Func<float, float> ef = EasingFunction.Get(_manager.cutEffectConfig.UpDownDisplayEaseEnum);
                    t = ef(t);

                    Vector2 to = Vector2.Lerp(agent_vector2, ori_vector2, t);
                    float a = Mathf.Lerp(0f, 1f, t);
                    agent.GetComponent<Image>().color = new Color(1, 1, 1, a);
                    agent.SetChangedPosition(to);
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



        /// <summary>
        /// 创建代理
        /// </summary>
        private void CreateItem(DataTypeEnum dataType)
        {
            //  获取持续时间
            _entranceDisplayTime = _manager.cutEffectConfig.UpDownDisplayDurTime;
            _startingTimeWithOutDelay = _entranceDisplayTime;

            //  初始化 config
            _displayBehaviorConfig = new DisplayBehaviorConfig();
            _sceneUtil = new SceneUtils(_manager, _sceneConfig.isKinect);

            //int _column = _manager.managerConfig.Column;


            int _column = 0;
            if (_sceneConfig.isKinect == 0)
            {
                _column = 15;
            }
            else
            {
                _column = 30;
            }


            int _itemWidth = _sceneUtil.GetFixedItemWidth();
            float gap = _sceneUtil.GetGap();

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

                while (gen_y_position < _manager.mainPanel.rect.height)
                {
                    // 获取数据
                    //FlockData data = _daoService.GetFlockData(dataType);
                    FlockData data = _daoService
                        .GetFlockDataByScene(dataType, _manager.SceneIndex);

                    Sprite coverSprite = data.GetCoverSprite();
                    float itemHeigth = AppUtils.GetSpriteHeightByWidth(coverSprite, _itemWidth);

                    int ori_x = Mathf.RoundToInt(_sceneUtil.GetXPositionByFixedWidth(_itemWidth, j));
                    int ori_y = Mathf.RoundToInt(gen_y_position + itemHeigth / 2);

                    // 获取出生位置
                    float gen_x, gen_y;

                    // 计算移动的目标位置
                    if (j % 2 == 0)
                    {
                        //偶数列向下偏移itemHeight
                        gen_y = ori_y - (itemHeigth + gap);
                    }
                    else
                    {
                        //奇数列向上偏移itemHeight
                        gen_y = ori_y + itemHeigth + gap;
                    }
                    gen_x = ori_x; //横坐标不变

                    // 创建agent
                    //FlockAgent go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y, row, j,
                    //         _itemWidth, itemHeigth, data, AgentContainerType.MainPanel);
                    //go.NextVector2 = new Vector2(gen_x, gen_y);
                    FlockAgent go = FlockAgentFactoryInstance.Generate(_manager, new Vector2(gen_x, gen_y), AgentContainerType.MainPanel
, ori_x, ori_y, row, j, _itemWidth, itemHeigth, data, _sceneConfig.daoTypeEnum);
                    go.flockStatus = FlockStatusEnum.RUNIN;

                    gen_y_position = Mathf.RoundToInt(gen_y_position + itemHeigth + gap);
                    _displayBehaviorConfig.columnAgentsDic[j].yposition = gen_y_position;
                    _displayBehaviorConfig.columnAgentsDic[j].xPositionMin = Mathf.RoundToInt(0 - gap);
                    _displayBehaviorConfig.columnAgentsDic[j].yPositionMin = Mathf.RoundToInt(0 - gap);
                    _displayBehaviorConfig.columnAgentsDic[j].row = row;
                    row++;
                }
            }

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
            return SceneTypeEnum.UpDownAdjustCutEffect;
        }


        private void Reset()
        {
            _hasCallDisplay = false;
            _cutEffectStatus = CutEffectStatus.Init;
        }
    }
}