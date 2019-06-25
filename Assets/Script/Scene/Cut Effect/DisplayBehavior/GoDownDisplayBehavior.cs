using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

//
//	向左移动
//
public class GoDownDisplayBehavior : CutEffectDisplayBehavior
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
        _daoService = _manager.daoService;

        flag = false;

    }

    public void Run()
	{

		// 面板向下移动
        Vector3 to = new Vector3(0,0 - Time.deltaTime * _manager.MovePanelFactor, 0);
        _manager.mainPanel.transform.Translate(to);

        // 调整panel的差值
        _manager.updateOffsetOfCanvas();

        UpdateAgents();
    }

    private void UpdateAgents() {
        if (_displayBehaviorConfig.SceneContentType == SceneContentType.activity){
            UpdateAgentsOfActivity();
        }
        else if (_displayBehaviorConfig.SceneContentType == SceneContentType.product) {
            UpdateAgentsOfProduct();
        }
        else
        {
            UpdateAgentsOfEnv();
        }
    }

    private void UpdateAgentsOfActivity() {
        float _itemWidth = 300 * _manager.displayFactor;
        float _itemHeight = 0;
        float gap = _displayBehaviorConfig.ItemsFactory.GetSceneGap();
        int extra = (int)(10 / 20f * _displayBehaviorConfig.DisplayTime) + 1;

        if (Math.Abs(_manager.PanelOffsetY) > 0)
        {
            if (flag == false)
            {
                foreach (KeyValuePair<int, float> pair in _manager.columnAndTops)
                {
                    //Debug.Log(pair.Key + "+++" + pair.Value);
                    float y = pair.Value;
                    for (int i = 0; i < extra; i++)
                    {
                        float ori_x = pair.Key * (_itemWidth + gap) + _itemWidth / 2 + gap;
                        float ori_y = y;

                        Activity activity = _manager.daoService.GetActivity();
                        //宽固定
                        _itemHeight = _itemWidth / activity.TextureImage.width * activity.TextureImage.height;
                        ori_y = ori_y + _itemHeight / 2 + gap;

                        // 生成 agent
                        FlockAgent go = _displayBehaviorConfig.ItemsFactory.Generate(ori_x, ori_y, ori_x, ori_y, pair.Key, i, _itemWidth, _itemHeight, activity, _manager.mainPanel);
                        y = y + go.Height + gap;
                        //Debug.Log(go.name + " i : " + i + " y : " + y + "gap : " + gap + " go.Height : " + go.Height);
                    }
                    y = pair.Value;
                }
                flag = true;
            }
        }
    }

    private void UpdateAgentsOfProduct()
    {
        float _itemWidth = 300 * _manager.displayFactor;
        float _itemHeight = 0;
        float gap = _displayBehaviorConfig.ItemsFactory.GetSceneGap();
        int extra = (int)(10 / 20f * _displayBehaviorConfig.DisplayTime) + 1;

        if (Math.Abs(_manager.PanelOffsetY) > 0)
        {
            if (flag == false)
            {
                foreach (KeyValuePair<int, float> pair in _manager.columnAndTops)
                {
                    //Debug.Log(pair.Key + "+++" + pair.Value);
                    float y = pair.Value;
                    for (int i = 0; i < extra; i++)
                    {
                        float ori_x = pair.Key * (_itemWidth + gap) + _itemWidth / 2 + gap;
                        float ori_y = y;

                        Product product = _manager.daoService.GetProduct();
                        //宽固定
                        _itemHeight = _itemWidth / product.TextureImage.width * product.TextureImage.height;
                        ori_y = ori_y + _itemHeight / 2 + gap;

                        // 生成 agent
                        FlockAgent go = _displayBehaviorConfig.ItemsFactory.Generate(ori_x, ori_y, ori_x, ori_y, pair.Key, i, _itemWidth, _itemHeight, product, _manager.mainPanel);
                        y = y + go.Height + gap;
                        //Debug.Log(go.name + " i : " + i + " y : " + y + "gap : " + gap + " go.Height : " + go.Height);
                    }
                    y = pair.Value;
                }
                flag = true;
            }
        }
    }



    void UpdateAgentsOfEnv()
    {
        /*
        int h = (int)_manager.mainPanel.rect.height;
        int w = (int)_manager.mainPanel.rect.width;

        int offsetUnit = Mathf.CeilToInt((h * 1.0f) / 3);
        int page = _displayBehaviorConfig.Page;

        if (h + _manager.PanelOffsetY - (offsetUnit * page) + 100 > 0)
        {
            float i = h + _manager.PanelOffsetY - (offsetUnit * page);

            // 需要获得当前的 column
            int rows_offsets = Mathf.CeilToInt(_displayBehaviorConfig.Row * 1.0f / 3);
            int x = page * rows_offsets + 1;
            int cols = _displayBehaviorConfig.Column;

            for (int y = 1; y <= cols; y++)
            {
                for (int z = x; z < (x + rows_offsets); z++)
                {
                    FlockAgent agent = CreateItem(_displayBehaviorConfig.ItemsFactory, z, y); // 创建新的
                    _displayBehaviorConfig.AddFlockAgentToAgentsOfPages(page, agent); // 加入list
                }
            }
            _displayBehaviorConfig.Page += 1;

            if ((_displayBehaviorConfig.Page - 5) > 0)
            {
                AgentManager.Instance.ClearAgentsByList(_displayBehaviorConfig.AgentsOfPages[_displayBehaviorConfig.Page - 5]); // 清理最下侧
            }
        }
        else
        {

        }
        */
        int startRow = _manager.Row;
        int _column = _displayBehaviorConfig.ItemsFactory.GetSceneColumn();
        float _itemWidth = _displayBehaviorConfig.ItemsFactory.GetItemWidth();
        float _itemHeight = _displayBehaviorConfig.ItemsFactory.GetItemHeight();

        int extra = (int)(10 / 20f * _displayBehaviorConfig.DisplayTime) + 1;
        if (Math.Abs(_manager.PanelOffsetY) > 0)
        {
            if (flag == false)
            {
                for (int i = startRow; i < startRow + extra; i++)
                {
                    for (int j = 0; j < _column; j++)
                    {
                        Vector2 vector2 = _displayBehaviorConfig.ItemsFactory.GetOriginPosition(i, j);
                        float x = vector2.x;
                        float y = vector2.y;
                        FlockAgent go = _displayBehaviorConfig.ItemsFactory.Generate(x, y, x, y, i, j, _itemWidth, _itemHeight, _daoService.GetEnterprise(), _manager.mainPanel);

                    }
                }
                flag = true;
            }
        }                    
    }

    private FlockAgent CreateItem(ItemsFactory factory, int row, int column)
    {
        row = row - 1;
        column = column - 1;

        Vector2 vector2 = factory.GetOriginPosition(row, column);

        float x = vector2.x;
        float y = vector2.y;

        return factory.Generate(x, y, x, y, row, column, factory.GetItemWidth(), factory.GetItemHeight(), _daoService.GetEnterprise(), _manager.mainPanel);

    }
}
