using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 过场效果 5，左右校准
public class LeftRightAdjustCutEffect : CutEffect
{
    private MagicWallManager _manager;

    private int _page;  // 页码

    private DisplayBehaviorConfig _displayBehaviorConfig;   //  Display Behavior Config

    private float _startingTimeWithOutDelay;
    private float _timeBetweenStartAndDisplay = 0.5f; //完成启动动画与表现动画之间的时间


    //
    //  Init
    //
    protected override void Init()
    {
        //  获取持续时间
        StartingDurTime = 2f;
        _startingTimeWithOutDelay = StartingDurTime;
        DestoryDurTime = 0.5f;

        //  设置显示的时间
        string t = DaoService.Instance.GetConfigByKey(AppConfig.KEY_CutEffectDuring_LeftRightAdjust).Value;
        DisplayDurTime = AppUtils.ConvertToFloat(t);

        // 获取Display的动画
        DisplayBehavior = new GoLeftDisplayBehavior();

        // 获取销毁的动画
        DestoryBehavior = new FadeOutDestoryBehavior();

        //  初始化 manager
        _manager = MagicWallManager.Instance;

        //  初始化 config
        _displayBehaviorConfig = new DisplayBehaviorConfig();
    }

    //
    //  创建产品 | Logo 
    //
    protected override void CreateProductOrLogo()
    {
        int _row = _manager.Row;
        int _column = ItemsFactory.GetSceneColumn();
        float _itemWidth = ItemsFactory.GetItemWidth();
        float _itemHeight = ItemsFactory.GetItemHeight();
        float gap = ItemsFactory.GetSceneGap();

        for (int j = 0; j < _column; j++)
        {
            for (int i = 0; i < _row; i++)
            {
                // 定义源位置
                //float ori_x = j * (_itemHeight + gap) + _itemHeight / 2;
                //float ori_y = i * (_itemWidth + gap) + _itemWidth / 2;

                Vector2 vector2 = ItemsFactory.GetOriginPosition(i, j);
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
                    gen_x = (_column + j) * (_itemWidth + gap) + _itemWidth / 2;
                }
                else
                {
                    delay = (System.Math.Abs(middleY - i) + 1) * 0.3f;
                    gen_x = -(_column - j) * (_itemWidth + gap) + _itemWidth / 2;
                }
                gen_y = ori_y; //纵坐标不变

                // 定义出生位置与目标位置
                Vector2 ori_position = new Vector2(ori_x, ori_y);
                Vector2 gen_position = new Vector2(gen_x, gen_y);

                // 生成 agent
                FlockAgent go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y, i, j , _itemWidth, _itemHeight, DaoService.Instance.GetEnterprise());
                go.Delay = delay;
                go.DelayTime = delay;

                // 装载进 pagesAgents
                int colUnit = Mathf.CeilToInt(_column * 1.0f / 4);
                _page = Mathf.CeilToInt((j + 1) * 1.0f / colUnit);
                _displayBehaviorConfig.AddFlockAgentToAgentsOfPages(_page, go);
            }
        }

    }

    //
    //  创建活动
    //
    protected override void CreateActivity()
    {
        // 获取栅格信息
        int _row = _manager.Row;
        int _column = ItemsFactory.GetSceneColumn();
        float _itemWidth = ItemsFactory.GetItemWidth();
        float _itemHeight = ItemsFactory.GetItemHeight();
        float gap = ItemsFactory.GetSceneGap();


        for (int j = 0; j < _column; j++)
        {
            for (int i = 0; i < _row; i++)
            {
                // 定义源位置
                float ori_x = j * (_itemHeight + gap) + _itemHeight / 2;
                float ori_y = i * (_itemWidth + gap) + _itemWidth / 2;

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
                    gen_x = (_column + j) * (_itemWidth + gap) + _itemWidth / 2;
                }
                else
                {
                    delay = (System.Math.Abs(middleY - i) + 1) * 0.3f;
                    gen_x = -(_column - j) * (_itemWidth + gap) + _itemWidth / 2;
                }
                gen_y = ori_y; //纵坐标不变

                // 生成 agent
                FlockAgent go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y, i , j , _itemWidth, _itemHeight, DaoService.Instance.GetEnterprise());
                go.Delay = delay;
                go.DelayTime = delay;
            }
        }
    }


    public override void Starting() {

        for (int i = 0; i < AgentManager.Instance.Agents.Count; i++)
        {
            FlockAgent agent = AgentManager.Instance.Agents[i];
            Vector2 agent_vector2 = agent.GenVector2;
            Vector2 ori_vector2 = agent.OriVector2;

            // 延时的时间
            float delay_time = agent.DelayTime;

            // 获取此 agent 需要的动画时间
            float run_time = _startingTimeWithOutDelay - delay_time - _timeBetweenStartAndDisplay;

            // 当前总运行的时间;
            float time = Time.time - StartTime;
            
            // 如果总动画时间超出 agent 需要的动画时间，则不进行处理
            if (time > StartingDurTime || time < delay_time)
            {
                continue;
                //Debug.Log(agent.name);
            }

            float t = (time - delay_time) / run_time;
            Vector2 to = Vector2.Lerp(agent_vector2, ori_vector2, t);

            agent.NextVector2 = to;
            agent.updatePosition();
        }


        //  初始化表现形式
        int _row = _manager.Row;
        int _column = ItemsFactory.GetSceneColumn();
        float _itemWidth = ItemsFactory.GetItemWidth();
        float _itemHeight = ItemsFactory.GetItemHeight();

        _displayBehaviorConfig.Row = _row;
        _displayBehaviorConfig.Column = _column;
        _displayBehaviorConfig.ItemWidth = _itemWidth;
        _displayBehaviorConfig.ItemHeight = _itemHeight;
        _displayBehaviorConfig.SceneContentType = sceneContentType;
        _displayBehaviorConfig.Page = _page;
        _displayBehaviorConfig.ItemsFactory = ItemsFactory;

        DisplayBehavior.Init(_displayBehaviorConfig);

    }

    public override void OnStartingCompleted(){
        AgentManager.Instance.UpdateAgents();
    }

}
