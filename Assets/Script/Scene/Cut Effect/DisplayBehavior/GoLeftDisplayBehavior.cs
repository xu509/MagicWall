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
        float itemWidth = 0;
        float itemHeight = 250 * _manager.displayFactor;
        float gap = _displayBehaviorConfig.ItemsFactory.GetSceneGap();
        int extra = (int)(10 / 20f * _displayBehaviorConfig.DisplayTime) + 1;
        if (Math.Abs(_manager.PanelOffsetX) > 0)
        {
            if (flag == false)
            {
                foreach (KeyValuePair<int, float> pair in _manager.rowAndRights)
                {
                    //Debug.Log(pair.Key + "+++" + pair.Value);
                    float x = pair.Value;
                    for (int i = 0; i < extra; i++)
                    {
                        float ori_x = x;
                        float ori_y = pair.Key * (itemHeight + gap) + itemHeight / 2 + gap;

                        Activity activity = _manager.daoService.GetActivity();
                        //高固定
                        itemWidth = (float)activity.TextureImage.width / (float)activity.TextureImage.height * itemHeight;
                        ori_x = ori_x + itemWidth / 2 + gap;

                        // 生成 agent
                        FlockAgent go = _displayBehaviorConfig.ItemsFactory.Generate(ori_x, ori_y, ori_x, ori_y, 
                            pair.Key, i, itemWidth, itemHeight, activity, AgentContainerType.MainPanel);
                        x = x + go.Width + gap;
                        //Debug.Log(go.name + " i : " + i + " y : " + y + "gap : " + gap + " go.Height : " + go.Height);
                    }
                    x = pair.Value;
                }
                flag = true;
            }
        }
    }

    private void UpdateAgentsOfProduct()
    {
        float itemWidth = 0;
        float itemHeight = 250 * _manager.displayFactor;
        float gap = _displayBehaviorConfig.ItemsFactory.GetSceneGap();
        float offset = Math.Abs(_manager.PanelOffsetX);
        int extra = (int)(10 / 20f * _displayBehaviorConfig.DisplayTime) + 1;

        if (offset > 0)
        {
            if (flag == false)
            {
                foreach (KeyValuePair<int, float> pair in _manager.rowAndRights)
                {
                    //Debug.Log(pair.Key + "+++" + pair.Value);
                    float x = pair.Value;
                    for (int i = 0; i < extra; i++)
                    {
                        float ori_x = x;
                        float ori_y = pair.Key * (itemHeight + gap) + itemHeight / 2 + gap;

                        Product product = _manager.daoService.GetProduct();
                        //高固定
                        itemWidth = (float)product.TextureImage.width / (float)product.TextureImage.height * itemHeight;
                        ori_x = ori_x + itemWidth / 2 + gap;

                        // 生成 agent
                        FlockAgent go = _displayBehaviorConfig.ItemsFactory.Generate(ori_x, ori_y, ori_x, ori_y, pair.Key,
                            i, itemWidth, itemHeight, product, AgentContainerType.MainPanel);
                        x = x + go.Width + gap;
                        //Debug.Log(go.name + " i : " + i + " y : " + y + "gap : " + gap + " go.Height : " + go.Height);
                    }
                    x = pair.Value;
                }
                flag = true;
            }
        }

    }

}
