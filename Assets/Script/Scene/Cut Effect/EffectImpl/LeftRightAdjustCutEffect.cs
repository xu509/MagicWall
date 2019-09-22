using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using EasingUtil;

// 过场效果 5，左右校准
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

        ////  设置显示的时间
        //string t = _daoService.GetConfigByKey(AppConfig.KEY_CutEffectDuring_LeftRightAdjust).Value;
        //DisplayDurTime = AppUtils.ConvertToFloat(t);

        // 获取Display的动画
        DisplayBehavior = new GoLeftDisplayBehavior();

        // 获取销毁的动画
        DestoryBehavior = new FadeOutDestoryBehavior();
        DestoryBehavior.Init(_manager, DestoryDurTime);

        //  初始化 config
        _displayBehaviorConfig = new DisplayBehaviorConfig();
    }

    //
    //  创建产品 | Logo 
    //
    protected override void CreateLogo()
    {
        CreateAgency(DataType.env);
    }

    //
    //  创建活动
    //
    protected override void CreateActivity()
    {
        CreateAgency(DataType.activity);
    }

    protected override void CreateProduct()
    {
        CreateAgency(DataType.product);
    }


    public override void Starting() {

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

            // 如果总动画时间超出 agent 需要的动画时间，则不进行处理
            if (time > StartingDurTime)
            {
                // 此时可能未走完动画
                if (!agent.isCreateSuccess)
                {
                    agent.NextVector2 = ori_vector2;
                    agent.isCreateSuccess = true;
                }

                continue;
            } else if (time <= delay_time) {
                // 此时该 Agent 还在持续时间内
                continue;
            }

            float t = (time - delay_time) / run_time;

            Func<float, float> defaultEasingFunction = EasingFunction.Get(_manager.cutEffectConfig.LeftRightDisplayEaseEnum);

            t = defaultEasingFunction(t);

            Vector2 to = Vector2.Lerp(agent_vector2, ori_vector2, t);

            agent.NextVector2 = to;
        }
    }

    public override void OnStartingCompleted(){
        //  初始化表现形式

        _displayBehaviorConfig.dataType = dataType;
        _displayBehaviorConfig.DisplayTime = DisplayDurTime;
        _displayBehaviorConfig.ItemsFactory = ItemsFactory;
        _displayBehaviorConfig.Manager = _manager;
        _displayBehaviorConfig.sceneUtils = _sceneUtil;
        DisplayBehavior.Init(_displayBehaviorConfig);

    }


    /// <summary>
    /// 创建代理
    /// </summary>
    private void CreateAgency(DataType dataType) {
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
                FlockData data = _daoService.GetFlockData(dataType);
                Sprite coverSprite = data.GetCoverSprite();
                float itemWidth = AppUtils.GetSpriteWidthByHeight(coverSprite, _itemHeight);

                int ori_y = Mathf.RoundToInt(_sceneUtil.GetYPositionByFixedHeight(_itemHeight, row));
                int ori_x = Mathf.RoundToInt(gen_x_position + itemWidth / 2 + gap / 2);

                // 获取参照点
                int middleY = _row / 2;
                int middleX = _nearColumn / 2;

                // 定义出生位置
                float gen_x, gen_y;

                // 计算出生位置与延时时间
                float delay;
                if (row < middleY)
                {
                    delay = (System.Math.Abs(middleY - row)) * 0.3f;
                    gen_x = ori_x + w;
                }
                else
                {
                    delay = (System.Math.Abs(middleY - row) + 1) * 0.3f;
                    gen_x = ori_x - w - 500;
                }
                gen_y = ori_y; //纵坐标不变

                //生成 agent
                FlockAgent go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y, row, column,
                    itemWidth, _itemHeight, data, AgentContainerType.MainPanel);

                go.Delay = delay;
                go.DelayTime = delay;
                //go.NextVector2 = new Vector2(gen_x, gen_y);


                gen_x_position = Mathf.RoundToInt(gen_x_position + itemWidth + gap / 2);
                _displayBehaviorConfig.rowAgentsDic[row].xposition = gen_x_position;
                _displayBehaviorConfig.rowAgentsDic[row].xPositionMin = 0;
                _displayBehaviorConfig.rowAgentsDic[row].column = column;

                column++;
            }
        }

        //StartingDurTime += _startDelayTime;

    }

    public override string GetID()
    {
        return "LeftRightAdjustCutEffect";
    }
}
