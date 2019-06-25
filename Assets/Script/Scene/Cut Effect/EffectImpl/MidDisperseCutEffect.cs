using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 过场效果 2 中间散开 
public class MidDisperseCutEffect : CutEffect
{


    private int _page;  // 页码

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
        int _row = _manager.Row;
        int _column = ItemsFactory.GetSceneColumn();
        float _itemWidth = ItemsFactory.GetItemWidth();
        float _itemHeight = ItemsFactory.GetItemHeight();
        float gap = ItemsFactory.GetSceneGap();


        //从下往上，从左往右
        for (int j = 0; j < _column; j++)
        {
            for (int i = 0; i < _row; i++)
            {
                Vector2 vector2 = ItemsFactory.GetOriginPosition(i, j);
                float x = vector2.x;
                float y = vector2.y;

                //float x = j * (_itemWidth + gap) + _itemWidth / 2;
                //float y = i * (_itemHeight + gap) + _itemHeight / 2;


                int middleX = _column / 2;
                float delay = System.Math.Abs(middleX - j) * 0.05f;
                if (delay>_timeBetweenStartAndDisplay)
                {
                    _timeBetweenStartAndDisplay = delay;
                }
                // ori_x;ori_y
                float gen_x, gen_y;

                gen_x = middleX * (_itemWidth + gap) + (_itemWidth / 2);
                gen_y = y + _itemWidth;

                //				FlockAgent go = AgentGenerator.GetInstance ().generator (name, gen_position, ori_position, magicWallManager);
                FlockAgent go = ItemsFactory.Generate(gen_x, gen_y, x, y, i, j, _itemWidth, _itemHeight, _daoService.GetEnterprise(), _manager.mainPanel);

                //go.transform.SetSiblingIndex(Mathf.Abs(middleX - j));
                go.Delay = delay;
                go.Duration = StartingDurTime + delay;

                //go.GetComponent<RectTransform>().DOScale(new Vector3(0, 0, 0), StartingDurTime + delay);

                // 获取启动动画的延迟时间
                if (delay > _startDelayTime)
                {
                    _startDelayTime = delay;
                }

                // 装载进 pagesAgents
                int rowUnit = Mathf.CeilToInt(_row * 1.0f / 3);
                _page = Mathf.CeilToInt((i + 1) * 1.0f / rowUnit);
                 _displayBehaviorConfig.AddFlockAgentToAgentsOfPages(_page, go);
            }
        }

        // 调整启动动画的时间
        StartingDurTime += _startDelayTime;
    }

    //
    //  创建活动
    //
    protected override void CreateActivity()
    {
        _manager.columnAndTops = new Dictionary<int, float>();

        // 获取栅格信息
        float gap = ItemsFactory.GetSceneGap();
        float _itemWidth = 300 * _manager.displayFactor;
        float _itemHeight = 0;
        float w = _manager.mainPanel.rect.width;
        float h = _manager.mainPanel.rect.height;
        int _row = _manager.Row;
        int _column = Mathf.CeilToInt(w / (_itemWidth + gap));

        for (int j = 0; j < _column; j++)
        {
            float y = 0;
            for (int i = 0; i < _row + 1; i++)
            {
                if (y < h)
                {
                    float ori_x = j * (_itemWidth + gap) + _itemWidth / 2 + gap;
                    float ori_y = y;

                    Activity activity = _manager.daoService.GetActivity();

                    //宽固定
                    _itemHeight = _itemWidth / activity.TextureImage.width * activity.TextureImage.height;
                    ori_y = ori_y + _itemHeight / 2 + gap;
                    //print("ori_x:"+ori_x+ "ori_y:" + ori_y);
                    // 获取出生位置
                    int middleX = _column / 2;
                    float delay = System.Math.Abs(middleX - j) * 0.05f;
                    if (delay > _timeBetweenStartAndDisplay)
                    {
                        _timeBetweenStartAndDisplay = delay;
                    }
                    // ori_x;ori_y
                    float gen_x, gen_y;

                    gen_x = middleX * (_itemWidth + gap) + (_itemWidth / 2);
                    gen_y = ori_y + _itemWidth;
                    //print(gen_y);
                    // 生成 agent
                    FlockAgent go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y, i, j, _itemWidth, _itemHeight, activity, _manager.mainPanel);
                    go.Delay = delay;
                    go.Duration = StartingDurTime + delay;
                    // 获取启动动画的延迟时间
                    if (delay > _startDelayTime)
                    {
                        _startDelayTime = delay;
                    }
                    y = y + go.Height + gap;
                }
            }
            //print(j + "---" + y);
            _manager.columnAndTops.Add(j, y);
            y = 0;
        }
        // 调整启动动画的时间
        StartingDurTime += _startDelayTime;
    }

    public override void Starting() {
        for (int i = 0; i < _agentManager.Agents.Count; i++) {
            FlockAgent agent = _agentManager.Agents[i];
            Vector2 agent_vector2 = agent.GenVector2;
            Vector2 ori_vector2 = agent.OriVector2;

            // 获取总运行时间
            float run_time = (StartingDurTime+agent.Delay) - _timeBetweenStartAndDisplay;
            // 当前已运行的时间;
            float time = Time.time - StartTime;
            if (time > run_time)
            {
                //agent.updatePosition();
                continue;
                //Debug.Log(agent.name);
            }

            float t = time / run_time;
            Vector2 to = Vector2.Lerp(agent_vector2, ori_vector2, t);
            float a = Mathf.Lerp(0f, 1f, t);

            agent.GetComponent<Image>().color = new Color(1, 1, 1, a);
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

	//
	public void CutEffectUpdateCallback(FlockAgent go){
		if (go.name == "Agent(1,1)") {
			Debug.Log (go.GetComponent<RectTransform> ().anchoredPosition);
		}
//		go.updatePosition ();
	}

    protected override void CreateProduct()
    {
        _manager.columnAndTops = new Dictionary<int, float>();

        // 获取栅格信息
        float gap = ItemsFactory.GetSceneGap();
        float _itemWidth = 300 * _manager.displayFactor;
        float _itemHeight = 0;
        float w = _manager.mainPanel.rect.width;
        float h = _manager.mainPanel.rect.height;
        int _row = _manager.Row;
        int _column = Mathf.CeilToInt(w / (_itemWidth + gap));

        for (int j = 0; j < _column; j++)
        {
            float y = 0;
            for (int i = 0; i < _row + 1; i++)
            {
                if (y < h)
                {
                    float ori_x = j * (_itemWidth + gap) + _itemWidth / 2 + gap;
                    float ori_y = y;

                    Product product = _daoService.GetProduct();

                    Texture t = TextureResource.Instance.GetTexture(MagicWallManager.FileDir + "product\\" + product.Image);

                    //宽固定
                    _itemHeight = _itemWidth / t.width * t.height;
                    ori_y = ori_y + _itemHeight / 2 + gap;

                    // 获取出生位置
                    int middleX = _column / 2;
                    float delay = System.Math.Abs(middleX - j) * 0.05f;
                    if (delay > _timeBetweenStartAndDisplay)
                    {
                        _timeBetweenStartAndDisplay = delay;
                    }

                    float gen_x, gen_y;

                    gen_x = middleX * (_itemWidth + gap) + (_itemWidth / 2);
                    gen_y = ori_y + _itemWidth;
                    //print(gen_y);
                    // 生成 agent
                    FlockAgent go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y, i, j, _itemWidth, _itemHeight, product, _manager.mainPanel);
                    go.Delay = delay;
                    go.Duration = StartingDurTime + delay;
                    // 获取启动动画的延迟时间
                    if (delay > _startDelayTime)
                    {
                        _startDelayTime = delay;
                    }
                    y = y + go.Height + gap;
                }
            }
            //print(j + "---" + y);
            _manager.columnAndTops.Add(j, y);
            y = 0;
        }
        // 调整启动动画的时间
        StartingDurTime += _startDelayTime;
    }

}
