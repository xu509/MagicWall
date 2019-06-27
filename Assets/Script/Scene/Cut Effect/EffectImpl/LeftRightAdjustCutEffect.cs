using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 过场效果 5，左右校准
public class LeftRightAdjustCutEffect : CutEffect
{

    private int _row;   // 总共的行数
    private int _column;    //总共的列数

    private DisplayBehaviorConfig _displayBehaviorConfig;   //  Display Behavior Config

    private float _startingTimeWithOutDelay;
    private float _timeBetweenStartAndDisplay = 0.5f; //完成启动动画与表现动画之间的时间


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
        StartingDurTime = 2f;
        _startingTimeWithOutDelay = StartingDurTime;
        DestoryDurTime = 0.5f;

        //  设置显示的时间
        string t = _daoService.GetConfigByKey(AppConfig.KEY_CutEffectDuring_LeftRightAdjust).Value;
        DisplayDurTime = AppUtils.ConvertToFloat(t);

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
        _row = _manager.Row;
        int itemWidth = _sceneUtil.GetFixedItemHeight();
        _column = _sceneUtil.GetColumnNumberByFixedWidth(itemWidth);

        float itemHeight = itemWidth;
        float gap = _sceneUtil.GetGap();

        for (int j = 0; j < _column; j++)
        {
            for (int i = 0; i < _row; i++)
            {
                Vector2 vector2 = _sceneUtil.GetPositionOfSquareItem(itemWidth,i,j);

                float ori_x = vector2.x;
                float ori_y = vector2.y;

                // 获取参照点
                int middleY = _row / 2;
                int middleX = _column / 2;

                // 定义出生位置
                float gen_x, gen_y;

                // 计算出生位置与延时时间
                float delay;
                if (i < middleY)
                {
                    delay = (System.Math.Abs(middleY - i)) * 0.3f;
                    gen_x = (_column + j) * (itemWidth + gap) + itemWidth / 2;
                }
                else
                {
                    delay = (System.Math.Abs(middleY - i) + 1) * 0.3f;
                    gen_x = -(_column - j) * (itemWidth + gap) + itemWidth / 2;
                }
                gen_y = ori_y; //纵坐标不变

                // 定义出生位置与目标位置
                Vector2 ori_position = new Vector2(ori_x, ori_y);
                Vector2 gen_position = new Vector2(gen_x, gen_y);

                // 生成 agent
                FlockAgent go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y, i, j ,
                    itemWidth, itemWidth, _daoService.GetEnterprise(), AgentContainerType.MainPanel);
                go.Delay = delay;
                go.DelayTime = delay;

            }
        }

    }

    //
    //  创建活动
    //
    protected override void CreateActivity()
    {
        // 行数固定，宽度需缩放调整
        _displayBehaviorConfig.rowAgentsDic = new Dictionary<int, ItemPositionInfoBean>();
        int _row = _manager.Row;
        float itemWidth;
        int itemHeight = _sceneUtil.GetFixedItemHeight();
        int gap = _sceneUtil.GetGap();

        float w = _manager.mainPanel.rect.width;


        //从左往右，从下往上
        for (int i = 0; i < _row; i++)
        {
            int position_x = 0;
            int column = 0;

            while (position_x < w)
            {

                var activity = _daoService.GetActivity();
                itemWidth = AppUtils.GetSpriteWidthByHeight(activity.SpriteImage, itemHeight);

                ItemPositionInfoBean bean;
                if (_displayBehaviorConfig.rowAgentsDic.ContainsKey(i))
                {
                    bean = _displayBehaviorConfig.rowAgentsDic[i];
                }
                else
                {
                    bean = new ItemPositionInfoBean();
                    _displayBehaviorConfig.rowAgentsDic.Add(i, bean);
                }

                // 获取X和Y
                Vector2 position = _sceneUtil.GetPositionOfIrregularItemByFixedHeight(bean, itemHeight, Mathf.RoundToInt(itemWidth), i);


                //print(env.TextureLogo.width+"---"+ env.TextureLogo.height+"---"+itemWidth+"+++"+itemHeight);
                float ori_x = position.x;
                float ori_y = position.y;


                // 获取参照点
                int middleY = _row / 2;
                int middleX = _column / 2;

                // 定义出生位置
                float gen_x, gen_y;

                // 计算出生位置与延时时间
                float delay;
                if (i < middleY)
                {
                    delay = (System.Math.Abs(middleY - i)) * 0.3f;
                    gen_x = ori_x + w;
                }
                else
                {
                    delay = (System.Math.Abs(middleY - i) + 1) * 0.3f;
                    gen_x = ori_x - w - 500;
                }
                gen_y = ori_y; //纵坐标不变

                //生成 agent
                FlockAgent go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y, i, column,
                    itemWidth, itemHeight, activity, AgentContainerType.MainPanel);

                go.Delay = delay;
                go.DelayTime = delay;

                bean.column = column;
                position_x = Mathf.RoundToInt(ori_x + itemWidth / 2 + gap / 2);
                bean.xposition = position_x;
                column++;
            }

        }

    }

    protected override void CreateProduct()
    {
        // 行数固定，宽度需缩放调整
        _displayBehaviorConfig.rowAgentsDic = new Dictionary<int, ItemPositionInfoBean>();
        int _row = _manager.Row;
        float itemWidth;
        int itemHeight = _sceneUtil.GetFixedItemHeight();
        int gap = _sceneUtil.GetGap();

        float w = _manager.mainPanel.rect.width;


        //从左往右，从下往上
        for (int i = 0; i < _row; i++)
        {
            int position_x = 0;
            int column = 0;

            while (position_x < w)
            {

                Product product = _daoService.GetProduct();
                itemWidth = AppUtils.GetSpriteWidthByHeight(product.SpriteImage, itemHeight);

                ItemPositionInfoBean bean;
                if (_displayBehaviorConfig.rowAgentsDic.ContainsKey(i))
                {
                    bean = _displayBehaviorConfig.rowAgentsDic[i];
                }
                else
                {
                    bean = new ItemPositionInfoBean();
                    _displayBehaviorConfig.rowAgentsDic.Add(i, bean);
                }

                // 获取X和Y
                Vector2 position = _sceneUtil.GetPositionOfIrregularItemByFixedHeight(bean, itemHeight, Mathf.RoundToInt(itemWidth), i);


                //print(env.TextureLogo.width+"---"+ env.TextureLogo.height+"---"+itemWidth+"+++"+itemHeight);
                float ori_x = position.x;
                float ori_y = position.y;


                // 获取参照点
                int middleY = _row / 2;
                int middleX = _column / 2;

                // 定义出生位置
                float gen_x, gen_y;

                // 计算出生位置与延时时间
                float delay;
                if (i < middleY)
                {
                    delay = (System.Math.Abs(middleY - i)) * 0.3f;
                    gen_x = ori_x + w;
                }
                else
                {
                    delay = (System.Math.Abs(middleY - i) + 1) * 0.3f;
                    gen_x = ori_x - w - 500;
                }
                gen_y = ori_y; //纵坐标不变

                //生成 agent
                FlockAgent go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y, i, column,
                    itemWidth, itemHeight, product, AgentContainerType.MainPanel);

                go.Delay = delay;
                go.DelayTime = delay;

                bean.column = column;
                position_x = Mathf.RoundToInt(ori_x + itemWidth / 2 + gap / 2);
                bean.xposition = position_x;
                column++;
            }

        }
    }


    public override void Starting() {

        for (int i = 0; i < _agentManager.Agents.Count; i++)
        {
            FlockAgent agent = _agentManager.Agents[i];
            Vector2 agent_vector2 = agent.GenVector2;
            Vector2 ori_vector2 = agent.OriVector2;

            agent.NextVector2 = agent_vector2;

            // 延时的时间
            float delay_time = agent.DelayTime;

            // 获取此 agent 需要的动画时间
            float run_time = _startingTimeWithOutDelay - delay_time - _timeBetweenStartAndDisplay;

            // 当前总运行的时间;
            float time = Time.time - StartTime;

            agent.NextVector2 = agent_vector2;
            // 如果总动画时间超出 agent 需要的动画时间，则不进行处理
            if (time > StartingDurTime)
            {
                continue;
            } else if (time <= delay_time) {
                // 此时该 Agent 还在持续时间内
                continue;
            }

            float t = (time - delay_time) / run_time;
            Vector2 to = Vector2.Lerp(agent_vector2, ori_vector2, t);

            agent.NextVector2 = to;
        }
    }

    public override void OnStartingCompleted(){
        //  初始化表现形式

        _displayBehaviorConfig.SceneContentType = sceneContentType;
        _displayBehaviorConfig.DisplayTime = DisplayDurTime;
        _displayBehaviorConfig.ItemsFactory = ItemsFactory;
        _displayBehaviorConfig.Manager = _manager;
        _displayBehaviorConfig.sceneUtils = _sceneUtil;
        DisplayBehavior.Init(_displayBehaviorConfig);

    }




}
