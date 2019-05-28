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
    private DisplayBehaviorConfig _displayBehaviorConfig;
    private bool flag = false;

    //
    //  初始化 （参数：内容类型，row）
    //
    public void Init(DisplayBehaviorConfig displayBehaviorConfig)
    {
        _displayBehaviorConfig = displayBehaviorConfig;
        flag = false;

    }

    public void Run()
	{
        _manager = MagicWallManager.Instance;

		// 面板向左移动
		float x = _manager.mainPanel.anchoredPosition.x - Time.deltaTime * _manager.MoveFactor_Panel;
		Vector2 to = new Vector2(x, _manager.mainPanel.anchoredPosition.y);
        _manager.mainPanel.DOAnchorPos(to, Time.deltaTime);

        // 调整panel的差值
        _manager.updateOffsetOfCanvas();

        UpdateAgents();

    }

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
        int h = (int)_manager.mainPanel.rect.height;
        int w = (int)_manager.mainPanel.rect.width;

        int offsetUnit = Mathf.CeilToInt((w * 1.0f) / 4);
        int page = _displayBehaviorConfig.Page;

        if (w + _manager.PanelOffsetX - (offsetUnit * page) > 0)
        {
            // 需要获得当前的 column
            int cols_offsets = Mathf.CeilToInt(_displayBehaviorConfig.Column * 1.0f / 4);
            int y = page * cols_offsets + 1;
            int rows = _displayBehaviorConfig.Row;

            for (int x = 1; x <= rows; x++)
            {
                for (int z = y; z < (y + cols_offsets); z++)
                {

                    FlockAgent agent = CreateItem(_displayBehaviorConfig.ItemsFactory, x, z); // 创建新的
                    _displayBehaviorConfig.AddFlockAgentToAgentsOfPages(page, agent); // 加入list
                }
            }
            _displayBehaviorConfig.Page += 1;

            if ((_displayBehaviorConfig.Page - 5) > 0)
                AgentManager.Instance.ClearAgentsByList(_displayBehaviorConfig.AgentsOfPages[_displayBehaviorConfig.Page - 5]); // 清理最左侧
        }
        else
        {

        }
    }

    private void UpdateAgentsOfActivity()
    {
        float itemWidth = 0;
        float itemHeight = 250;
        float gap = _displayBehaviorConfig.ItemsFactory.GetSceneGap();
        if (Math.Abs(_manager.PanelOffsetX) > 0)
        {
            if (flag == false)
            {
                foreach (KeyValuePair<int, float> pair in _manager.rowAndRights)
                {
                    //Debug.Log(pair.Key + "+++" + pair.Value);
                    float x = pair.Value;
                    for (int i = 0; i < 8; i++)
                    {
                        float ori_x = x;
                        float ori_y = pair.Key * (itemHeight + gap) + itemHeight / 2 + gap;

                        Activity activity = _manager.DaoService.GetActivity();
                        //高固定
                        itemWidth = (float)activity.TextureImage.width / (float)activity.TextureImage.height * itemHeight;
                        ori_x = ori_x + itemWidth / 2 + gap;

                        // 生成 agent
                        FlockAgent go = _displayBehaviorConfig.ItemsFactory.Generate(ori_x, ori_y, ori_x, ori_y, pair.Key, i, itemWidth, itemHeight, activity, _manager.mainPanel);
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
        float itemHeight = 250;
        float gap = _displayBehaviorConfig.ItemsFactory.GetSceneGap();
        float offset = Math.Abs(_manager.PanelOffsetX);
        if (offset > 0)
        {
            if (flag == false)
            {
                foreach (KeyValuePair<int, float> pair in _manager.rowAndRights)
                {
                    //Debug.Log(pair.Key + "+++" + pair.Value);
                    float x = pair.Value;
                    for (int i = 0; i < 8; i++)
                    {
                        float ori_x = x;
                        float ori_y = pair.Key * (itemHeight + gap) + itemHeight / 2 + gap;

                        Product product = _manager.DaoService.GetProduct();
                        //高固定
                        itemWidth = (float)product.TextureImage.width / (float)product.TextureImage.height * itemHeight;
                        ori_x = ori_x + itemWidth / 2 + gap;

                        // 生成 agent
                        FlockAgent go = _displayBehaviorConfig.ItemsFactory.Generate(ori_x, ori_y, ori_x, ori_y, pair.Key, i, itemWidth, itemHeight, product, _manager.mainPanel);
                        x = x + go.Width + gap;
                        //Debug.Log(go.name + " i : " + i + " y : " + y + "gap : " + gap + " go.Height : " + go.Height);
                    }
                    x = pair.Value;
                }
                flag = true;
            }
        }

    }

    private FlockAgent CreateItem(ItemsFactory factory, int row, int column) {
        row = row - 1;
        column = column - 1;

        Vector2 vector2 = factory.GetOriginPosition(row, column);
        float x = vector2.x;
        float y = vector2.y;

        return factory.Generate(x, y, x, y, row, column, factory.GetItemWidth(), factory.GetItemHeight(), DaoService.Instance.GetEnterprise(), _manager.mainPanel);
    }

}
