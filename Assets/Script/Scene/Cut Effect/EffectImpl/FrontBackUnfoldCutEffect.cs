using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 过场效果 前后层展开
public class FrontBackUnfoldCutEffect : CutEffect
{
    private DisplayBehaviorConfig _displayBehaviorConfig;   //  Display Behavior Config
    private float _startDelayTime = 0f;  //启动的延迟时间
    private float _startingTimeWithOutDelay;
    private float _timeBetweenStartAndDisplay = 0.5f; //完成启动动画与表现动画之间的时间

    private int _row;   // 总共的行数
    private int _column;    //总共的列数

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
        StartingDurTime = 1.5f;
        _startingTimeWithOutDelay = StartingDurTime;
        DestoryDurTime = 0.5f;

        //  设置显示的时间
        string t = _daoService.GetConfigByKey(AppConfig.KEY_CutEffectDuring_FrontBackUnfold).Value;
        DisplayDurTime = AppUtils.ConvertToFloat(t);

        // 获取Display的动画
        DisplayBehavior = new FrontBackGoLeftDisplayBehavior();

        // 获取销毁的动画
        DestoryBehavior = new FadeOutDestoryBehavior();
        DestoryBehavior.Init(_manager,DestoryDurTime);

        //  初始化 config
        _displayBehaviorConfig = new DisplayBehaviorConfig();

    }

    //
    private void CutEffectUpdateCallback(FlockAgent go)
    {
        if (go.name == "Agent(1,1)")
        {
            Debug.Log(go.GetComponent<RectTransform>().anchoredPosition);
        }
        //		go.updatePosition ();
    }

    //
    //  创建 Logo 
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

    public override void Starting()
    {
        for (int i = 0; i < _agentManager.Agents.Count; i++)
        {
            FlockAgent agent = _agentManager.Agents[i];
            //Vector2 agent_vector2 = agent.GetComponent<RectTransform> ().anchoredPosition;
            Vector2 agent_vector2 = agent.GenVector2;
            Vector2 ori_vector2 = agent.OriVector2;

            //agent.NextVector2 = agent_vector2;

            float run_time = (_startingTimeWithOutDelay - agent.DelayX + agent.DelayY) - _timeBetweenStartAndDisplay; // 动画运行的总时间

            //Ease.InOutQuad
            float time = Time.time - StartTime;  // 当前已运行的时间;

            if (time > run_time)
            {
                continue;
            }

            // 模拟 DOTWEEN InOutQuad
            if ((time /= run_time * 0.5f) < 1f)
            {
                time = 0.5f * time * time;
            }
            else
            {
                time = -0.5f * ((time -= 1f) * (time - 2f) - 1f);
            }

            Vector2 to = Vector2.Lerp(agent_vector2, ori_vector2, time);

            agent.NextVector2 = to;
        }

    }

    public override void OnStartingCompleted()
    {
        //  初始化表现形式

        _displayBehaviorConfig.SceneContentType = sceneContentType;
        _displayBehaviorConfig.ItemsFactory = ItemsFactory;
        _displayBehaviorConfig.DisplayTime = DisplayDurTime;
        _displayBehaviorConfig.Manager = _manager;
        _displayBehaviorConfig.sceneUtils = _sceneUtil;
        DisplayBehavior.Init(_displayBehaviorConfig);

    }


    /// <summary>
    ///     创建的代理
    /// </summary>
    private void CreateAgency(DataType type) {

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
                FlockData agent = _daoService.GetFlockData(type);
                Sprite coverSprite = agent.GetCoverSprite();
                float imageWidth = coverSprite.rect.width;
                float imageHeight = coverSprite.rect.height;

                // 得到调整后的长宽
                Vector2 imageSize = AppUtils.ResetTexture(new Vector2(imageWidth, imageHeight),
                    _manager.displayFactor);

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
                    go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y, i, column,
                        imageSize.x, imageSize.y, agent, AgentContainerType.MainPanel);
                }
                else
                {
                    //  创建后排
                    go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y, i, column,
                        imageSize.x, imageSize.y, agent, AgentContainerType.BackPanel);
                    go.UpdateImageAlpha(0.2f);
                }
                //go.NextVector2 = new Vector2(gen_x, gen_y);

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
        StartingDurTime += _startDelayTime;

    }

    public override string GetID()
    {
        return "FrontBackUnfoldCutEffect";
    }
}
