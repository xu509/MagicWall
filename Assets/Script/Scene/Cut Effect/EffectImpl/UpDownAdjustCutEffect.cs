using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 过场效果 4，上下校准
public class UpDownAdjustCutEffect : CutEffect
{

    private int _page;  // 页码

    private float _startingTimeWithOutDelay;
    private float _timeBetweenStartAndDisplay = 0.5f; //完成启动动画与表现动画之间的时间

    private DisplayBehaviorConfig _displayBehaviorConfig;   //  Display Behavior Config

    //
    //  Init
    //
    public override void Init(MagicWallManager manager)
    {
        _manager = manager;
        _agentManager = manager.agentManager;
        _daoService = DaoService.Instance;


        //  获取持续时间
        StartingDurTime = 1f;
        _startingTimeWithOutDelay = StartingDurTime;
        DestoryDurTime = 0.5f;

        //  设置显示的时间
        string t = _daoService.GetConfigByKey(AppConfig.KEY_CutEffectDuring_UpDownAdjust).Value;
        DisplayDurTime = AppUtils.ConvertToFloat(t);

        // 获取Display的动画
        DisplayBehavior = new GoUpDisplayBehavior();

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
                //float ori_x = j * (_itemHeight + gap) + _itemHeight / 2;
                //float ori_y = i * (_itemWidth + gap) + _itemWidth / 2;

                Vector2 vector2 = ItemsFactory.GoUpGetOriginPosition(i, j);
                float ori_x = vector2.x;
                float ori_y = vector2.y;

                // 获取出生位置
                float gen_x, gen_y;

                // 计算移动的目标位置
                if (j % 2 == 0)
                {
                    //偶数列向下偏移itemHeight
                    gen_y = ori_y - (_itemHeight + gap);
                }
                else
                {
                    //奇数列向上偏移itemHeight
                    gen_y = ori_y + _itemHeight + gap;
                }
                gen_x = ori_x; //横坐标不变


                // 生成 agent
                FlockAgent go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y, i , j , _itemWidth, _itemHeight, _daoService.GetEnterprise(), _manager.mainPanel);

                // agent 一定时间内从透明至无透明
                //go.GetComponent<RawImage>().DOFade(0, StartingDurTime).From();

                // 装载进 pagesAgents
                int rowUnit = Mathf.CeilToInt(_row * 1.0f / 3);
                _page = Mathf.CeilToInt((i + 1) * 1.0f / rowUnit);
                _displayBehaviorConfig.AddFlockAgentToAgentsOfPages(_page, go);
            }
        }
    }

    //
    //  创建活动
    //
    protected override void CreateActivity()
    {
        _manager.columnAndBottoms = new Dictionary<int, float>();

        // 获取栅格信息
        float gap = ItemsFactory.GetSceneGap();
        float _itemWidth = 300;
        float _itemHeight = 0;
        float w = _manager.mainPanel.rect.width;
        float h = _manager.mainPanel.rect.height;
        int _row = _manager.Row;
        int _column = Mathf.CeilToInt(w / (_itemWidth + gap));

        for (int j = 0; j < _column; j++)
        {
            float y = h;
            for (int i = 0; i < _row+1; i++)
            {
                if (y>0)
                {
                    float ori_x = j * (_itemWidth + gap) + _itemWidth / 2 + gap;
                    float ori_y = y;

                    Activity activity = _daoService.GetActivity();
                    //宽固定
                    _itemHeight = _itemWidth / activity.TextureImage.width * activity.TextureImage.height;
                    ori_y = ori_y - _itemHeight / 2 - gap;
                    // 获取出生位置
                    float gen_x, gen_y;

                    // 计算移动的目标位置
                    if (j % 2 == 0)
                    {
                        //偶数列向下偏移itemHeight
                        gen_y = ori_y - (_itemHeight + gap);
                    }
                    else
                    {
                        //奇数列向上偏移itemHeight
                        gen_y = ori_y + _itemHeight + gap;
                    }
                    gen_x = ori_x; //横坐标不变        

                    // 生成 agent
                    FlockAgent go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y, j, i, _itemWidth, _itemHeight, activity, _manager.mainPanel);
                    y = y - go.Height - gap;
                }
            }
            //print(j + "---" + y);
            _manager.columnAndBottoms.Add(j, y);
            y = h;
        }
    }

    public override void Starting() {
        for (int i = 0; i < _agentManager.Agents.Count; i++)
        {
            FlockAgent agent = _agentManager.Agents[i];
            Vector2 agent_vector2 = agent.GenVector2;
            Vector2 ori_vector2 = agent.OriVector2;

            // 获取此 agent 需要的动画时间
            float run_time = StartingDurTime - _timeBetweenStartAndDisplay;

            // 当前总运行的时间;
            float time = Time.time - StartTime;
            
            // 如果总动画时间超出 agent 需要的动画时间，则不进行处理
            if (time > run_time)
            {
                agent.updatePosition();
                continue;
                //Debug.Log(agent.name);
            }

            float t = (Time.time - StartTime) / run_time;
            Vector2 to = Vector2.Lerp(agent_vector2, ori_vector2, t);
            float a = Mathf.Lerp(0f, 1f, t);
            agent.GetComponent<RawImage>().color = new Color(1, 1, 1, a);
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
        _displayBehaviorConfig.ItemsFactory = ItemsFactory;
        _displayBehaviorConfig.DisplayTime = DisplayDurTime;
        _displayBehaviorConfig.Manager = _manager;
        DisplayBehavior.Init(_displayBehaviorConfig);

    }

    protected override void CreateProduct()
    {
        _manager.columnAndBottoms = new Dictionary<int, float>();

        // 获取栅格信息
        float gap = ItemsFactory.GetSceneGap();
        float _itemWidth = 300;
        float _itemHeight = 0;
        float w = _manager.mainPanel.rect.width;
        float h = _manager.mainPanel.rect.height;
        int _row = _manager.Row;
        int _column = Mathf.CeilToInt(w / (_itemWidth + gap));

        for (int j = 0; j < _column; j++)
        {
            float y = h;
            for (int i = 0; i < _row + 1; i++)
            {
                if (y > 0)
                {
                    float ori_x = j * (_itemWidth + gap) + _itemWidth / 2 + gap;
                    float ori_y = y;

                    Product product = _daoService.GetProduct();
                    //宽固定
                    _itemHeight = _itemWidth / product.TextureImage.width * product.TextureImage.height;
                    ori_y = ori_y - _itemHeight / 2 - gap;
                    // 获取出生位置
                    float gen_x, gen_y;

                    // 计算移动的目标位置
                    if (j % 2 == 0)
                    {
                        //偶数列向下偏移itemHeight
                        gen_y = ori_y - (_itemHeight + gap);
                    }
                    else
                    {
                        //奇数列向上偏移itemHeight
                        gen_y = ori_y + _itemHeight + gap;
                    }
                    gen_x = ori_x; //横坐标不变        

                    // 生成 agent
                    FlockAgent go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y, j, i, _itemWidth, _itemHeight, product, _manager.mainPanel);
                    y = y - go.Height - gap;
                }
            }
            //print(j + "---" + y);
            _manager.columnAndBottoms.Add(j, y);
            y = h;
        }
    }


}
