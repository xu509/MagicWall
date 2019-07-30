using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 过场效果 1 ，曲线麻花效果
public class CurveStaggerCutEffect : CutEffect
{

    private DisplayBehaviorConfig _displayBehaviorConfig;   //  Display Behavior Config
    private float _startDelayTime = 0f;  //启动的延迟时间
    private float _startingTimeWithOutDelay;
    private float _timeBetweenStartAndDisplay = 0.5f; //完成启动动画与表现动画之间的时间

    //
    //  Init
    //
    public override void Init(MagicWallManager manager)
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();

        //  初始化 manager
        _manager = manager;
        _agentManager = manager.agentManager;
        _daoService = manager.daoService;

        //  获取持续时间
        StartingDurTime = 3f;
        _startingTimeWithOutDelay = StartingDurTime;
        DestoryDurTime = 0.5f;

        //  设置显示的时间
        string t = _daoService.GetConfigByKey(AppConfig.KEY_CutEffectDuring_CurveStagger).Value;
        DisplayDurTime = AppUtils.ConvertToFloat(t);

        // 获取Display的动画
        DisplayBehavior = new GoLeftDisplayBehavior();

        // 获取销毁的动画
        DestoryBehavior = new FadeOutDestoryBehavior();
        DestoryBehavior.Init(_manager,DestoryDurTime);

        //  初始化 config
        _displayBehaviorConfig = new DisplayBehaviorConfig();

        sw.Stop();
        //    打印内容
        //Debug.Log("Time : " + sw.ElapsedMilliseconds / 1000f);

    }


    #region 动画实现
    public override void Starting() {
        for (int i = 0; i < _agentManager.Agents.Count; i++) {
            FlockAgent agent = _agentManager.Agents[i];
            //Vector2 agent_vector2 = agent.GetComponent<RectTransform> ().anchoredPosition;
            Vector2 agent_vector2 = agent.GenVector2;
            Vector2 ori_vector2 = agent.OriVector2;

            float run_time = (_startingTimeWithOutDelay - agent.DelayX + agent.DelayY); // 动画运行的总时间

            //Ease.InOutQuad
            float time = Time.time - StartTime;  // 当前已运行的时间;

            // 此时有两种特殊状态，还未移动的与已移动完成的
            if (time > run_time) {

                // 此时可能未走完动画
                if (!agent.isCreateSuccess)
                {
                    agent.NextVector2 = ori_vector2;
                    agent.isCreateSuccess = true;
                }
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
    #endregion

    public override void OnStartingCompleted(){
        //  初始化表现形式

        _displayBehaviorConfig.dataType = dataType;
        _displayBehaviorConfig.ItemsFactory = ItemsFactory;
        _displayBehaviorConfig.DisplayTime = DisplayDurTime;
        _displayBehaviorConfig.Manager = _manager;
        _displayBehaviorConfig.sceneUtils = _sceneUtil;
        DisplayBehavior.Init(_displayBehaviorConfig);
    }

    #region 企业浮动块创建    
    protected override void CreateLogo()
    {
        CreateItem(DataType.env);
      
    }
    #endregion

    #region 产品浮动块创建
    protected override void CreateProduct()
    {
        CreateItem(DataType.product);
    }
    #endregion

    #region 活动浮动块创建
    protected override void CreateActivity()
    {
        CreateItem(DataType.activity);
    }
    #endregion


    private void CreateItem(DataType dataType) {

        // 固定高度
        int _row = _manager.Row;
        int _itemHeight = _sceneUtil.GetFixedItemHeight();
        float gap = _sceneUtil.GetGap();

        int _nearColumn = Mathf.RoundToInt(_manager.mainPanel.rect.width / (_itemHeight + gap));
        float w = _manager.mainPanel.rect.width;


        // 从上至下，生成
        for (int row = 0; row < _row; row++) {
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

                int ori_y = Mathf.RoundToInt(_sceneUtil.GetYPositionByFixedHeight(_itemHeight,row));
                int ori_x = Mathf.RoundToInt(gen_x_position + itemWidth / 2 + gap / 2);

                int middleY = _row / 2;
                int middleX = _nearColumn / 2;

                float delayX = column * 0.06f;
                float delayY;

                // 定义源位置
                float gen_x, gen_y;

                if (row < middleY)
                {
                    delayY = System.Math.Abs(middleY - row) * 0.3f;
                    gen_x = w + (middleY - row) * 500;
                    gen_y = w - ori_x + (_row - 1) * _itemHeight;
                }
                else
                {
                    delayY = (System.Math.Abs(middleY - row) + 1) * 0.3f;
                    gen_x = w + (row - middleY + 1) * 500;
                    gen_y = -(w - ori_x) + 2 * _itemHeight;
                }

                //生成 agent
                FlockAgent go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y
                    , row , column, itemWidth, _itemHeight, data, AgentContainerType.MainPanel);
                //go.NextVector2 = new Vector2(gen_x, gen_y);


                // 装载延迟参数
                go.DelayX = delayX;
                go.DelayY = delayY;

                // 生成透明度动画
                go.GetComponentInChildren<Image>().DOFade(0, StartingDurTime - delayX + delayY).From();

                // 获取启动动画的延迟时间
                if ((delayY - delayX) > _startDelayTime)
                {
                    _startDelayTime = delayY - delayX;
                }

                gen_x_position = Mathf.RoundToInt(gen_x_position + itemWidth + gap / 2);
                _displayBehaviorConfig.rowAgentsDic[row].xposition = gen_x_position;
                _displayBehaviorConfig.rowAgentsDic[row].xPositionMin = 0;
                _displayBehaviorConfig.rowAgentsDic[row].column = column;

                column++;
            }
        }

        StartingDurTime += _startDelayTime;
    }

    public override string GetID()
    {
        return "CurveStaggerCutEffect";
    }
}
