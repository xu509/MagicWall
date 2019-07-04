using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 过场效果 2 中间散开 
public class MidDisperseCutEffect : CutEffect
{


    private float _startDelayTime = 0f;  //启动的延迟时间
    private float _startingTimeWithOutDelay;
    private float _timeBetweenStartAndDisplay = 0f; //完成启动动画与表现动画之间的时间

    private DisplayBehaviorConfig _displayBehaviorConfig;   //  Display Behavior Config


    //
    //  Init
    //
    public override void Init(MagicWallManager manager)
    {
        //  初始化 manager
        _manager = manager;
        _agentManager = manager.agentManager;
        _daoService = DaoService.Instance;

        //  获取持续时间
        StartingDurTime = 0.5f;
        _startingTimeWithOutDelay = StartingDurTime;
        DestoryDurTime = 0.5f;

        //  设置显示的时间
        string t = _daoService.GetConfigByKey(AppConfig.KEY_CutEffectDuring_MidDisperseAdjust).Value;
        DisplayDurTime = AppUtils.ConvertToFloat(t);

        // 获取Display的动画
        DisplayBehavior = new GoDownDisplayBehavior();

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
        for (int i = 0; i < _agentManager.Agents.Count; i++) {
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
                if (!agent.isCreateSuccess) {
                    agent.NextVector2 = ori_vector2;
                    agent.isCreateSuccess = true;
                }

                continue;
            }

            float t = time / run_time;
            Vector2 to = Vector2.Lerp(agent_vector2, ori_vector2, t);           
            agent.NextVector2 = to;


            //float a = Mathf.Lerp(0f, 1f, t);
            //agent.GetComponent<Image>().color = new Color(1, 1, 1, a);

        }

    }

    public override void OnStartingCompleted(){
        //  初始化表现形式
        _displayBehaviorConfig.SceneContentType = sceneContentType;
        _displayBehaviorConfig.ItemsFactory = ItemsFactory;
        _displayBehaviorConfig.DisplayTime = DisplayDurTime;
        _displayBehaviorConfig.Manager = _manager;
        _displayBehaviorConfig.sceneUtils = _sceneUtil;
        DisplayBehavior.Init(_displayBehaviorConfig);

    }



    private void CreateAgency(DataType dataType) {
        int _column = _manager.managerConfig.Column;
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
            int ori_x = Mathf.RoundToInt(_sceneUtil.GetXPositionByFixedWidth(_itemWidth, j));

            while (gen_y_position < _manager.mainPanel.rect.height)
            {
                // 获取数据
                FlockData data = _daoService.GetFlockData(dataType);
                Sprite coverSprite = data.GetCoverSprite();
                float itemHeigth = AppUtils.GetSpriteHeightByWidth(coverSprite, _itemWidth);

                int ori_y = Mathf.RoundToInt(gen_y_position + itemHeigth / 2);

                int middleX = _column / 2;
                float delay = System.Math.Abs(middleX - j) * 0.05f;

                if (delay > _timeBetweenStartAndDisplay)
                {
                    _timeBetweenStartAndDisplay = delay;
                }

                float gen_x, gen_y;

                gen_x = middleX * (_itemWidth + gap) + (_itemWidth / 2);
                gen_y = ori_y + _itemWidth;

                // 创建agent
                FlockAgent go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y, row, j,
                         _itemWidth, itemHeigth, data, AgentContainerType.MainPanel);
                go.Delay = delay;

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
        // 调整启动动画的时间
        StartingDurTime = StartingDurTime + _startDelayTime + 0.1f;

    }

    public override string GetID()
    {
        return "MidDisperseCutEffect";
    }
}
