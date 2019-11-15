using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using EasingUtil;
using System;

// 过场效果 前后层展开
namespace MagicWall
{
    public class FrontBackUnfoldCutEffect : ICutEffect
    {
        MagicWallManager _manager;
        private SceneConfig _sceneConfig;



        private float _entranceDisplayTime;
        private float _startTime;

        private SceneUtils _sceneUtil;
        private DataTypeEnum _dataTypeEnum;
        private CutEffectStatus _cutEffectStatus;

        private DisplayBehaviorConfig _displayBehaviorConfig;   //  Display Behavior Config
        private float _startDelayTime = 0f;  //启动的延迟时间
        private float _startingTimeWithOutDelay;
        private float _timeBetweenStartAndDisplay = 0.05f; //完成启动动画与表现动画之间的时间

        private int _row;   // 总共的行数
        private int _column;    //总共的列数

        private Action _onEffectCompleted;
        private Action _onDisplayStart;
        private Action<DisplayBehaviorConfig> _onCreateAgentCompleted;

        private bool _hasCallDisplay = false;

        //
        //  Init
        //
        public void Init(MagicWallManager manager, SceneConfig sceneConfig,
            Action<DisplayBehaviorConfig> OnCreateAgentCompleted,
            Action OnEffectCompleted, Action OnDisplayStart)
        {
            //  初始化 manager
            _manager = manager;
            _sceneConfig = sceneConfig;

            _dataTypeEnum = sceneConfig.dataType;

            _onCreateAgentCompleted = OnCreateAgentCompleted;
            _onEffectCompleted = OnEffectCompleted;
            _onDisplayStart = OnDisplayStart;

            //  获取持续时间
            _entranceDisplayTime = manager.cutEffectConfig.FrontBackDisplayDurTime;
            _startingTimeWithOutDelay = _entranceDisplayTime;

            //  初始化 config
            _displayBehaviorConfig = new DisplayBehaviorConfig();

        }


        public void Starting()
        {
            float time = Time.time - _startTime;  // 当前已运行的时间;

            for (int i = 0; i < _manager.agentManager.Agents.Count; i++)
            {
                FlockAgent agent = _manager.agentManager.Agents[i];
                Vector2 agent_vector2 = agent.GenVector2;
                Vector2 ori_vector2 = agent.OriVector2;


                float run_time = (_startingTimeWithOutDelay - agent.DelayX + agent.DelayY) - _timeBetweenStartAndDisplay; // 动画运行的总时间

                //Ease.InOutQuad

                if (time > run_time)
                {
                    continue;
                }

                float t = time / run_time;
                Func<float, float> defaultEasingFunction = EasingFunction.Get(_manager.cutEffectConfig.CurveStaggerDisplayEaseEnum);
                t = defaultEasingFunction(t);

                Vector2 to = Vector2.Lerp(agent_vector2, ori_vector2, t);

                agent.SetChangedPosition(to);
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
        ///     创建的代理
        /// </summary>
        private void CreateItem(DataTypeEnum type)
        {

            //  初始化 config
            _displayBehaviorConfig = new DisplayBehaviorConfig();
            _sceneUtil = new SceneUtils(_manager, _sceneConfig.isKinect);

            _row = _manager.Row;
            int itemHeight = _sceneUtil.GetFixedItemHeight();
            float gap = _sceneUtil.GetGap();

            float w = _manager.mainPanel.rect.width; // 获取总宽度

            int column = 0;     //  列数
            int generate_x = 0; // 生成新 agent 的位置

            // 按列创建， 隔行创建
            while (generate_x < w)
            {

                // 获取列数奇数状态
                bool isOddColumn = column % 2 == 0;

                float generate_x_temp = 0;

                for (int i = 0; i < _row; i++)
                {

                    //  获取行数奇数状态
                    bool isOddRow = i % 2 == 0;

                    //  获取要创建的内容
                    FlockData agent = _manager.daoServiceFactory.GetDaoService(_sceneConfig.daoTypeEnum).GetFlockDataByScene(type,_manager.SceneIndex);
                    Sprite coverSprite = agent.GetCoverSprite();
                    float imageWidth = coverSprite.rect.width;
                    float imageHeight = coverSprite.rect.height;

                    // 得到调整后的长宽
                    Vector2 imageSize = _sceneUtil.ResetTexture(new Vector2(imageWidth, imageHeight));

                    imageSize.x = (imageSize.x * 1.5f);
                    imageSize.y = (imageSize.y * 1.5f);

                    //Vector2 imageSize = AppUtils.ResetTexture(new Vector2(imageWidth, imageHeight),
                    //    _manager.displayFactor);

                    FlockAgent go;
                    float ori_y = _sceneUtil.GetYPositionByFixedHeight(itemHeight, i);

                    float ori_x = generate_x + gap + imageSize.x / 2;

                    if (ori_x + gap + imageSize.x / 2 > generate_x_temp)
                    {
                        generate_x_temp = ori_x + gap + imageSize.x / 2;
                    }

                    // 定义生成位置
                    float gen_x, gen_y;
                    gen_x = _manager.mainPanel.rect.width + imageSize.x + gap;
                    gen_y = ori_y;


                    if ((isOddColumn && isOddRow) || (!isOddRow && !isOddColumn))
                    {
                        // 创建前排
                        //go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y, i, column,
                        //    imageSize.x, imageSize.y, agent, AgentContainerType.MainPanel);

                        go = FlockAgentFactoryInstance.Generate(_manager, new Vector2(gen_x,gen_y), AgentContainerType.MainPanel
    , ori_x, ori_y, i, column, imageSize.x, imageSize.y, agent, _sceneConfig.daoTypeEnum);

                    }
                    else
                    {
                        //  创建后排
                        float width = imageSize.x * 0.6f;
                        float height = imageSize.y * 0.6f;

                        //go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y, i, column,
                        //    imageSize.x, imageSize.y, agent, AgentContainerType.BackPanel);

                        //go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y, i, column,
                        //    width, height, agent, AgentContainerType.BackPanel);


                        go = FlockAgentFactoryInstance.Generate(_manager, new Vector2(gen_x, gen_y), AgentContainerType.BackPanel
    , ori_x, ori_y, i, column, width, height, agent, _sceneConfig.daoTypeEnum);
                    }
                    //go.NextVector2 = new Vector2(gen_x, gen_y);

                    go.flockStatus = FlockStatusEnum.RUNIN;

                    // 装载延迟参数
                    go.DelayX = 0;
                    go.DelayY = 0;

                }

                // 更新 generate_x 的值
                generate_x = Mathf.RoundToInt(generate_x_temp);

                // 第二列的开始在第一列的最右侧
                column++;
            }

            _displayBehaviorConfig.generatePositionX = generate_x;
            _displayBehaviorConfig.generatePositionXInBack = generate_x;
            _displayBehaviorConfig.Column = column;
            _displayBehaviorConfig.ColumnInBack = column;

            // 调整启动动画的时间
            _entranceDisplayTime += _startDelayTime;

            // 创建结束
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
            return SceneTypeEnum.FrontBackUnfold;
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