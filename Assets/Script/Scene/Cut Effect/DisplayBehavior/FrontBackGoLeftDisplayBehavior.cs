using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

//
//	向左移动
//
public class FrontBackGoLeftDisplayBehavior : CutEffectDisplayBehavior
{
    private MagicWallManager _manager;
    private DaoService _daoService;
    private DisplayBehaviorConfig _displayBehaviorConfig;

    //
    //  初始化 （参数：内容类型，row）
    //
    public void Init(DisplayBehaviorConfig displayBehaviorConfig)
    {
        _displayBehaviorConfig = displayBehaviorConfig;
        _manager = displayBehaviorConfig.Manager;
        _daoService = _manager.daoService;
    }

    public void Run()
    {

        // 面板向左移动
        //float x = _manager.mainPanel.anchoredPosition.x - Time.deltaTime * _manager.MovePanelFactor;
        //Vector2 to = new Vector2(x, _manager.mainPanel.anchoredPosition.y);
        //_manager.mainPanel.DOAnchorPos(to, Time.deltaTime);
        _manager.mainPanel.transform.Translate(Vector3.left * Time.deltaTime);

        //float backX = _manager.backPanel.anchoredPosition.x + Time.deltaTime * _manager.MovePanelFactor / 2;
        //Vector2 backTo = new Vector2(backX, _manager.backPanel.anchoredPosition.y);
        //_manager.backPanel.DOAnchorPos(backTo, Time.deltaTime);
        _manager.mainPanel.transform.Translate(Vector3.left * Time.deltaTime * 0.5f);

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

    private void UpdateAgentsOfEnv ()
    {
        /*
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
        */
    }

    private void UpdateAgentsOfActivity()
    {

    }

    private void UpdateAgentsOfProduct()
    {

        
    }

    private FlockAgent CreateItem(ItemsFactory factory, int row, int column)
    {
        row = row - 1;
        column = column - 1;

        Vector2 vector2 = factory.GetOriginPosition(row, column);
        float x = vector2.x;
        float y = vector2.y;

        bool front = (row + column) % 2 == 0 ? true : false;
        //float offsetX = (z == 0) ? 0 : -500;
        //x = x + offsetX;
        //生成 agent
        FlockAgent go;
        if (front)
        {
            go = factory.Generate(x, y, x, y, row, column, factory.GetItemWidth(), factory.GetItemHeight(), _daoService.GetEnterprise(), _manager.mainPanel);

        }
        else
        {
            go = factory.Generate(x, y, x, y, row, column, factory.GetItemWidth(), factory.GetItemHeight(), _daoService.GetEnterprise(), _manager.backPanel);
            go.GetComponent<RawImage>()?.DOFade(0.2f, 0);
        }

        return go;
    }

}
