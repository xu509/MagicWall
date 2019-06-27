using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

//
//	向左移动
//
public class GoLeftDisplayBehavior : CutEffectDisplayBehavior
{
    private MagicWallManager _manager;
    private DaoService _daoService;

    private DisplayBehaviorConfig _displayBehaviorConfig;
    private bool flag = false;

    //
    //  初始化 （参数：内容类型，row）
    //
    public void Init(DisplayBehaviorConfig displayBehaviorConfig)
    {
        _displayBehaviorConfig = displayBehaviorConfig;
        _manager = _displayBehaviorConfig.Manager;
        _daoService = DaoService.Instance;

        flag = false;
    }

    public void Run()
	{
	    // 面板向左移动
        Vector3 to = new Vector3(0 - Time.deltaTime * _manager.MovePanelFactor, 0, 0);
        _manager.mainPanel.transform.Translate(to);

        // 调整panel的差值
        _manager.updateOffsetOfCanvas();

        UpdateAgents();
    }

    /// <summary>
    ///     更新移动状态
    /// </summary>
    private void UpdateAgents()
    {
        if (_displayBehaviorConfig.SceneContentType == SceneContentType.activity)
        {
            UpdateAgentsOfActivity();
        }
        else if (_displayBehaviorConfig.SceneContentType == SceneContentType.product)
        {
            UpdateAgentsOfProduct();
        }
        else
        {
            UpdateAgentsOfEnv();
        }
    }

    private void UpdateAgentsOfEnv()
    {
        int itemHeight = _displayBehaviorConfig.sceneUtils.GetFixedItemHeight();

        // 获取右侧最小的距离
        int column = _displayBehaviorConfig.Column;

        Vector2 position = _displayBehaviorConfig.sceneUtils.GetPositionOfSquareItem(itemHeight, 0, column);

        // 超过屏幕的距离
        float overDistense = (position.x - itemHeight / 2) - Screen.width;

        if ((overDistense - _manager.PanelOffsetX) < 0)
        {
            if (flag == false)
            {
                flag = true;
                //int startColumn = _displayBehaviorConfig.sceneUtils.GetColumnNumberByFixedWidth(itemHeight);
                int extra = 2; // 该值需注意不要与自动清理的功能冲突

                for (int i = 0; i < _displayBehaviorConfig.Row; i++)
                {
                    for (int j = column; j < column + extra; j++)
                    {
                        Vector2 vector2 = _displayBehaviorConfig.sceneUtils.GetPositionOfSquareItem(itemHeight, i, j);
                        float x = vector2.x;
                        float y = vector2.y;

                        //生成 agent
                         _displayBehaviorConfig.ItemsFactory.Generate(x, y, x, y, i, j,
                            itemHeight, itemHeight,_daoService.GetEnterprise(), AgentContainerType.MainPanel);
                    }
                }

                _displayBehaviorConfig.Column = column + extra;
                flag = false;
            }
        }
    }

    private void UpdateAgentsOfActivity()
    {
        float gap = _displayBehaviorConfig.sceneUtils.GetGap();
        float offset = Math.Abs(_manager.PanelOffsetX);
        int extra = (int)(10 / 20f * _displayBehaviorConfig.DisplayTime) + 1;

        // 获取右侧最小的距离
        var rowDic = _displayBehaviorConfig.rowAgentsDic;

        int row = 0;    // 最短行长的行值
        int last_x = 10000000;
        ItemPositionInfoBean bean = new ItemPositionInfoBean();
        foreach (KeyValuePair<int, ItemPositionInfoBean> keyValuePair in rowDic)
        {
            if (keyValuePair.Value.xposition < last_x)
            {
                last_x = keyValuePair.Value.xposition;
                row = keyValuePair.Key;
                bean = keyValuePair.Value;
            }
        }

        // 超过屏幕的距离
        float overDistense = last_x - Screen.width;

        if ((overDistense - _manager.PanelOffsetX) < 0)
        {
            if (flag == false)
            {
                // 该行添加内容
                var activity = _manager.daoService.GetActivity();

                int itemHeight = _displayBehaviorConfig.sceneUtils.GetFixedItemHeight();
                int itemWidth = Mathf.RoundToInt(AppUtils.GetSpriteWidthByHeight(activity.SpriteImage, itemHeight));

                Vector2 position = _displayBehaviorConfig.sceneUtils.GetPositionOfIrregularItemByFixedHeight(bean, itemHeight, itemWidth, row);

                // 生成 agent
                FlockAgent go = _displayBehaviorConfig.ItemsFactory.Generate(position.x, position.y, position.x, position.y
                    , row, bean.column + 1, itemWidth, itemHeight, activity, AgentContainerType.MainPanel);


                int position_x = Mathf.RoundToInt(position.x + itemWidth / 2 + gap / 2);
                rowDic[row].column = bean.column + 1;
                rowDic[row].xposition = position_x;
            }
        }
    }

    private void UpdateAgentsOfProduct()
    {
        float gap = _displayBehaviorConfig.sceneUtils.GetGap();
        float offset = Math.Abs(_manager.PanelOffsetX);
        int extra = (int)(10 / 20f * _displayBehaviorConfig.DisplayTime) + 1;

        // 获取右侧最小的距离
        var rowDic = _displayBehaviorConfig.rowAgentsDic;

        int row = 0;    // 最短行长的行值
        int last_x = 10000000;
        ItemPositionInfoBean bean = new ItemPositionInfoBean();
        foreach (KeyValuePair<int, ItemPositionInfoBean> keyValuePair in rowDic) {
            if (keyValuePair.Value.xposition < last_x) {
                last_x = keyValuePair.Value.xposition;
                row = keyValuePair.Key;
                bean = keyValuePair.Value;
            }
        }

        // 超过屏幕的距离
        float overDistense = last_x - Screen.width;

        if ((overDistense - _manager.PanelOffsetX) < 0) {
            if (flag == false) {
                // 该行添加内容
                Product product = _manager.daoService.GetProduct();

                int itemHeight = _displayBehaviorConfig.sceneUtils.GetFixedItemHeight();
                int itemWidth = Mathf.RoundToInt(AppUtils.GetSpriteWidthByHeight(product.SpriteImage, itemHeight));

                Vector2 position = _displayBehaviorConfig.sceneUtils.GetPositionOfIrregularItemByFixedHeight(bean,itemHeight,itemWidth,row);

                // 生成 agent
                FlockAgent go = _displayBehaviorConfig.ItemsFactory.Generate(position.x, position.y, position.x, position.y
                    , row, bean.column + 1, itemWidth, itemHeight, product, AgentContainerType.MainPanel);


                int position_x = Mathf.RoundToInt(position.x + itemWidth / 2 + gap / 2); 
                rowDic[row].column = bean.column + 1;
                rowDic[row].xposition = position_x;
            }
        }

    }

}
