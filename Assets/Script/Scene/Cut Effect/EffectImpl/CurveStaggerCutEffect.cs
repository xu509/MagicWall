﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 过场效果 1 ，曲线麻花效果
public class CurveStaggerCutEffect : CutEffect
{

    private int _page;  // 页码

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

            //agent.NextVector2 = agent_vector2;

            // 此时有两种特殊状态，还未移动的与已移动完成的
            if (time > run_time) {

                //agent.updatePosition();
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

        _displayBehaviorConfig.Row = _row;
        _displayBehaviorConfig.Column = _column;
        _displayBehaviorConfig.SceneContentType = sceneContentType;
        _displayBehaviorConfig.Page = _page;
        _displayBehaviorConfig.ItemsFactory = ItemsFactory;
        _displayBehaviorConfig.DisplayTime = DisplayDurTime;
        _displayBehaviorConfig.Manager = _manager;
        _displayBehaviorConfig.sceneUtils = _sceneUtil;
        DisplayBehavior.Init(_displayBehaviorConfig);
    }

    #region 企业浮动块创建    
    protected override void CreateLogo()
    {
        _row = _manager.Row;
        int itemWidth = _sceneUtil.GetFixedItemHeight();
        _column = _sceneUtil.GetColumnNumberByFixedWidth(itemWidth);

        float itemHeight = itemWidth;
        float gap = _sceneUtil.GetGap();

        //从左往右，从下往上
        for (int i = 0; i < _row; i++)
        {
            for (int j = 0; j < _column; j++)
            {
                Vector2 vector2 = _sceneUtil.GetPositionOfSquareItem(Mathf.RoundToInt(itemWidth), i, j);
                float x = vector2.x;
                float y = vector2.y;

                int middleY = _row / 2;
                int middleX = _column / 2;

                float delayX = j * 0.06f;
                float delayY;

                // 定义源位置
                float ori_x, ori_y;

                if (i < middleY)
                {
                    delayY = System.Math.Abs(middleY - i) * 0.3f;
                    ori_x = (_column + middleY - i - 1) * (itemWidth + gap) + itemWidth / 2;
                    ori_y = (_column - j - middleY + _row - 1) * (itemHeight + gap) + itemHeight / 2;
                    //the_RectTransform.DOLocalMove(new Vector3((column + middleY - i - 1) * (itemWidth + gap) + itemWidth / 2, (column - j - middleY) * (itemHeight + gap) + itemHeight / 2, 0), dur_time - delayX + delayY).SetEase(Ease.InOutQuad).From();
                }
                else
                {
                    delayY = (System.Math.Abs(middleY - i) + 1) * 0.3f;
                    ori_x = (_column + i - middleY) * (itemWidth + gap) + itemWidth / 2;
                    ori_y = -(_column - j - middleY) * (itemHeight + gap) + itemHeight / 2;
                    //the_RectTransform.DOLocalMove(new Vector3((column + i - middleY) * (itemWidth + gap) + itemWidth / 2, -(column - j - middleY) * (itemHeight + gap) + itemHeight / 2, 0), dur_time - delayX + delayY).SetEase(Ease.InOutQuad).From();
                }

                //生成 agent
                FlockAgent go = ItemsFactory.Generate(ori_x, ori_y, x, y, i, j, itemWidth, itemHeight,
                    _daoService.GetEnterprise(), AgentContainerType.MainPanel);

                // 装载延迟参数
                go.DelayX = delayX;
                go.DelayY = delayY;

                // 调整大小
                RectTransform the_RectTransform = go.GetComponent<RectTransform>();
                the_RectTransform.sizeDelta = new Vector2(itemWidth, itemWidth);

                // 生成透明度动画
                go.GetComponentInChildren<Image>().DOFade(0, StartingDurTime - delayX + delayY).From();

                // 获取启动动画的延迟时间
                if ((delayY - delayX) > _startDelayTime)
                {
                    _startDelayTime = delayY - delayX;
                }

                // 装载进 pagesAgents
                int colUnit = Mathf.CeilToInt(_column * 1.0f / 4);
                _page = Mathf.CeilToInt((j + 1) * 1.0f / colUnit);
                _displayBehaviorConfig.AddFlockAgentToAgentsOfPages(_page, go);

            }
        }

        // 调整启动动画的时间
        StartingDurTime += _startDelayTime;
    }
    #endregion

    #region 产品浮动块创建
    protected override void CreateProduct()
    {
        // 行数固定，宽度需缩放调整
        _displayBehaviorConfig.rowAgentsDic = new Dictionary<int, ItemPositionInfoBean>();
        int _row = _manager.Row;
        int _column = 50;
        float itemWidth = 0;
        int itemHeight = _sceneUtil.GetFixedItemHeight();
        int gap = _sceneUtil.GetGap();

        float w = _manager.mainPanel.rect.width;

        //从左往右，从下往上
        for (int i = 0; i < _row; i++)
        {
            float x = 0;
            int column = 0;

            while (x < _manager.mainPanel.rect.width) {

                Product product = _daoService.GetProduct();

                itemWidth = AppUtils.GetSpriteWidthByHeight(product.SpriteImage, itemHeight);

                ItemPositionInfoBean bean;
                if (_displayBehaviorConfig.rowAgentsDic.ContainsKey(i)) {
                    bean = _displayBehaviorConfig.rowAgentsDic[i];
                } else {
                    bean = null;
                }

                // 获取X和Y
                Vector2 position =  _sceneUtil.GetPositionOfIrregularItemByFixedHeight(bean, itemHeight, Mathf.RoundToInt(itemWidth), i);

                float ori_x = position.x;
                float ori_y = position.y;

                int middleY = _row / 2;

                float delayX = column * 0.06f;
                float delayY;

                // 定义出生位置
                float gen_x = ori_x, gen_y = ori_y;

                if (i < middleY)
                {
                    delayY = System.Math.Abs(middleY - i) * 0.3f;
                    gen_x = w + (middleY - i) * 500;
                    gen_y = w - ori_x + (_row - 1) * itemHeight;
                }
                else
                {
                    delayY = (System.Math.Abs(middleY - i) + 1) * 0.3f;
                    gen_x = w + (i - middleY + 1) * 500;
                    gen_y = -(w - ori_x) + 2 * itemHeight;
                }

                //生成 agent
                FlockAgent go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y, i, column,
                    itemWidth, itemHeight, product, AgentContainerType.MainPanel);

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

                

                column++;
            }


            for (int j = 0; j < _column; j++)
            {
                if (x < w)
                {
                    float ori_x = x;
                    float ori_y = i * (itemHeight + gap) + itemHeight / 2 + gap;

                   
                    x = x + go.Width + gap;

                }
            }
            _manager.rowAndRights.Add(i, x);
            x = 0;
        }

        // 调整启动动画的时间
        StartingDurTime += _startDelayTime;
    }
    #endregion

    #region 活动浮动块创建
    protected override void CreateActivity()
    {
        float s = Time.time;

        _manager.rowAndRights = new Dictionary<int, float>();
        int _row = _manager.Row;
        int _column = 50;//宽度自适应，列数变多确保可以铺满整行
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
                    itemWidth = AppUtils.GetSpriteWidthByHeight(activity.SpriteImage, itemHeight);

                    ori_x = ori_x + itemWidth / 2 + gap;

                    int middleY = _row / 2;

                    float delayX = j * 0.06f;
                    float delayY;

                    // 定义出生位置
                    float gen_x = ori_x, gen_y = ori_y;

                    if (i < middleY)
                    {
                        delayY = System.Math.Abs(middleY - i) * 0.3f;
                        gen_x = w + (middleY - i) * 500;
                        gen_y = w - ori_x + (_row - 1) * itemHeight;
                    }
                    else
                    {
                        delayY = (System.Math.Abs(middleY - i) + 1) * 0.3f;
                        gen_x = w + (i - middleY + 1) * 500;
                        gen_y = -(w - ori_x) + 2 * itemHeight;
                    }

                    //生成 agent
                    FlockAgent go = ItemsFactory.Generate(gen_x, gen_y, ori_x, ori_y, i, j,
                        itemWidth, itemHeight, activity, AgentContainerType.MainPanel);

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
                    x = x + go.Width + gap;

                }
            }
            _manager.rowAndRights.Add(i, x);
            x = 0;
        }

        // 调整启动动画的时间
        StartingDurTime += _startDelayTime;

    }
    #endregion

}
