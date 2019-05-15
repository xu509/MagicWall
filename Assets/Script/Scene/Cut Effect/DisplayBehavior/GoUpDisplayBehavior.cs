using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

//
//	向左移动
//
public class GoUpDisplayBehavior : CutEffectDisplayBehavior
{
    private MagicWallManager _manager;
    private DisplayBehaviorConfig _displayBehaviorConfig;

    private int _initPage;
    private bool flag = false;
    //
    //  初始化 （参数：内容类型，row）
    //
    public void Init(DisplayBehaviorConfig displayBehaviorConfig)
    {
        _displayBehaviorConfig = displayBehaviorConfig;
        _initPage = _displayBehaviorConfig.Page;
        flag = false;

    }

    public void Run()
    {
        _manager = MagicWallManager.Instance;

        // 面板向上移动
        float y = _manager.mainPanel.anchoredPosition.y + Time.deltaTime * _manager.MoveFactor_Panel;
        Vector2 to = new Vector2(_manager.mainPanel.anchoredPosition.x, y);
        _manager.mainPanel.DOAnchorPos(to, Time.deltaTime);

        // 调整panel的差值
        _manager.updateOffsetOfCanvas();

        // 调整所有agent
        _manager.UpdateAgents();

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

    private void UpdateAgentsOfActivity()
    {

        int h = (int)_manager.mainPanel.rect.height;
        float _itemWidth = 300;
        float _itemHeight = 0;
        float gap = _displayBehaviorConfig.ItemsFactory.GetSceneGap();
        int offsetUnit = Mathf.CeilToInt((h * 1.0f) / 3);
        int page = _displayBehaviorConfig.Page;
        if (Math.Abs(_manager.PanelOffsetY) > 0)
        {
            if (flag == false)
            {
                foreach (KeyValuePair<int, float> pair in _manager.columnAndBottoms)
                {
                    //Debug.Log(pair.Key + "+++" + pair.Value);
                    float y = pair.Value;
                    for (int i = 0; i < 7; i++)
                    {
                        float ori_x = pair.Key * (_itemWidth + gap) + _itemWidth / 2 + gap;
                        float ori_y = y;

                        Activity activity = _manager.daoService.GetActivity();
                        //宽固定
                        _itemHeight = _itemWidth / activity.TextureImage.width * activity.TextureImage.height;
                        ori_y = ori_y - _itemHeight / 2 - gap;
                        // 获取出生位置
                        float gen_x, gen_y;

                        // 计算移动的目标位置
                        if (pair.Key % 2 == 0)
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
                        FlockAgent go = _displayBehaviorConfig.ItemsFactory.Generate(ori_x, ori_y, ori_x, ori_y, pair.Key, i, _itemWidth, _itemHeight, activity);
                        y = y - go.Height - gap;
                        //Debug.Log(go.name + " i : " + i + " y : " + y + "gap : " + gap + " go.Height : " + go.Height);
                    }
                    y = pair.Value;
                }
                //_manager.columnAndBottoms = newRowAndHeights;
                _displayBehaviorConfig.Page += 1;
                flag = true;

            }
        }
    }

    private void UpdateAgentsOfEnv()
    {
        int h = (int)_manager.mainPanel.rect.height;
        int w = (int)_manager.mainPanel.rect.width;

        int offsetUnit = Mathf.CeilToInt((h * 1.0f) / 3);
        int page = _displayBehaviorConfig.Page;
        if (h - _manager.PanelOffsetY - (offsetUnit * page) + 100 > 0)
        {
            //float i = h + _manager.PanelOffsetY - (offsetUnit * page);
            Debug.Log(111);
            // 需要获得当前的 row
            int rows_offsets = Mathf.CeilToInt(_displayBehaviorConfig.Row * 1.0f / 3);//2
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
    }


    private FlockAgent CreateItem(ItemsFactory factory, int row, int column)
    {
        row = row - 1;
        column = column - 1;

        //row = -row + _displayBehaviorConfig.Row - 1;

        Vector2 vector2 = factory.GoUpGetOriginPosition(row, column);

        float x = vector2.x;
        float y = vector2.y;

        return factory.Generate(x, y, x, y, row, column, factory.GetItemWidth(), factory.GetItemHeight(), DaoService.Instance.GetEnterprise());

    }

    void UpdateAgentsOfProduct()
    {

        int h = (int)_manager.mainPanel.rect.height;
        float _itemWidth = 300;
        float _itemHeight = 0;
        float gap = _displayBehaviorConfig.ItemsFactory.GetSceneGap();
        int offsetUnit = Mathf.CeilToInt((h * 1.0f) / 3);
        int page = _displayBehaviorConfig.Page;
        if (Math.Abs(_manager.PanelOffsetY) > 0)
        {
            if (flag == false)
            {
                foreach (KeyValuePair<int, float> pair in _manager.columnAndBottoms)
                {
                    //Debug.Log(pair.Key + "+++" + pair.Value);
                    float y = pair.Value;
                    for (int i = 0; i < 7; i++)
                    {
                        float ori_x = pair.Key * (_itemWidth + gap) + _itemWidth / 2 + gap;
                        float ori_y = y;

                        Product product = _manager.daoService.GetProduct();
                        //宽固定
                        _itemHeight = _itemWidth / product.TextureImage.width * product.TextureImage.height;
                        ori_y = ori_y - _itemHeight / 2 - gap;
                        // 获取出生位置
                        float gen_x, gen_y;

                        // 计算移动的目标位置
                        if (pair.Key % 2 == 0)
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
                        FlockAgent go = _displayBehaviorConfig.ItemsFactory.Generate(ori_x, ori_y, ori_x, ori_y, pair.Key, i, _itemWidth, _itemHeight, product);
                        y = y - go.Height - gap;
                        //Debug.Log(go.name + " i : " + i + " y : " + y + "gap : " + gap + " go.Height : " + go.Height);
                    }
                    y = pair.Value;
                }
                //_manager.columnAndBottoms = newRowAndHeights;
                _displayBehaviorConfig.Page += 1;
                flag = true;

            }
        }
    }

}
