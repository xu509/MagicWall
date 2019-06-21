using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 过场效果 5，左右校准
public class LeftRightAdjustCutEffect : CutEffect
{

    private int _page;  // 页码

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
                FlockAgent go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y, i, j , _itemWidth, _itemHeight, _daoService.GetEnterprise(), _manager.mainPanel);
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
        _manager.rowAndRights = new Dictionary<int, float>();
        int _row = _manager.Row;
        int _column = 50;
        float itemWidth = 0;
        float itemHeight = 250 * _manager.displayFactor;
        float gap = ItemsFactory.GetSceneGap();
        float h = _manager.mainPanel.rect.height;
        float w = _manager.mainPanel.rect.width;


        //从左往右，从下往上
        for (int i = 0; i < _row; i++)
        {
            float x = 0;
            for (int j = 0; j < _column; j++)
            {
                if (x < w)
                {
                    float ori_x = x;
                    float ori_y = i * (itemHeight + gap) + itemHeight / 2 + gap;

                    Activity activity = _manager.daoService.GetActivity();
                    //高固定
                    itemWidth = (float)activity.TextureImage.width / (float)activity.TextureImage.height * itemHeight;

                    //print(env.TextureLogo.width+"---"+ env.TextureLogo.height+"---"+itemWidth+"+++"+itemHeight);
                    ori_x = ori_x + itemWidth / 2 + gap;

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
                    FlockAgent go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y, i, j, itemWidth, itemHeight, activity, _manager.mainPanel);
                    //print("gen_x:" + gen_x+ " gen_y:" + gen_y+ " ori_x:" + ori_x+ " ori_y:" + ori_y+" i:"+i+" j:"+j);
                    //print("OriVector2:" + go.OriVector2+ " GenVector2:" + go.GenVector2);
                    go.Delay = delay;
                    go.DelayTime = delay;
                    x = x + go.Width + gap;
                }
            }
            _manager.rowAndRights.Add(i, x);
            x = 0;
        }
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
            if (time > StartingDurTime || time < delay_time)
            {
                //Debug.Log(agent.name);
                agent.updatePosition();

                continue;
            }

            float t = (time - delay_time) / run_time;
            Vector2 to = Vector2.Lerp(agent_vector2, ori_vector2, t);

            agent.NextVector2 = to;
            agent.updatePosition();
        }
    }

    public override void OnStartingCompleted(){
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
        _displayBehaviorConfig.DisplayTime = DisplayDurTime;
        _displayBehaviorConfig.ItemsFactory = ItemsFactory;
        _displayBehaviorConfig.Manager = _manager;
        DisplayBehavior.Init(_displayBehaviorConfig);

    }

    protected override void CreateProduct()
    {
        // 获取栅格信息
        _manager.rowAndRights = new Dictionary<int, float>();
        int _row = _manager.Row;
        int _column = 50;
        float itemWidth = 0;
        float itemHeight = 250 * _manager.displayFactor;
        float gap = ItemsFactory.GetSceneGap();
        float h = _manager.mainPanel.rect.height;
        float w = _manager.mainPanel.rect.width;


        //从左往右，从下往上
        for (int i = 0; i < _row; i++)
        {
            float x = 0;
            for (int j = 0; j < _column; j++)
            {
                if (x < w)
                {
                    float ori_x = x;
                    float ori_y = i * (itemHeight + gap) + itemHeight / 2 + gap;

                    Product product = _daoService.GetProduct();
                    //高固定
                    itemWidth = (float)product.TextureImage.width / (float)product.TextureImage.height * itemHeight;

                    //print(env.TextureLogo.width+"---"+ env.TextureLogo.height+"---"+itemWidth+"+++"+itemHeight);
                    ori_x = ori_x + itemWidth / 2 + gap;

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
                    FlockAgent go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y, i, j, itemWidth, itemHeight, product, _manager.mainPanel);
                    //print("gen_x:" + gen_x+ " gen_y:" + gen_y+ " ori_x:" + ori_x+ " ori_y:" + ori_y+" i:"+i+" j:"+j);
                    //print("OriVector2:" + go.OriVector2+ " GenVector2:" + go.GenVector2);
                    go.Delay = delay;
                    go.DelayTime = delay;
                    x = x + go.Width + gap;
                }
            }
            _manager.rowAndRights.Add(i, x);
            x = 0;
        }
    }


}
