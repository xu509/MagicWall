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
    private DisplayBehaviorConfig _displayBehaviorConfig;

    //
    //  初始化 （参数：内容类型，row）
    //
    public void Init(DisplayBehaviorConfig displayBehaviorConfig)
    {
        _displayBehaviorConfig = displayBehaviorConfig;
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

    private void UpdateAgentsOfEnv ()
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

        float z = (row + column) % 2 == 0 ? 0 : 300;
        float a = (z == 0) ? 1 : 0.2f;

        FlockAgent agent = factory.Generate(x, y, x, y, row, column, factory.GetItemWidth(), factory.GetItemHeight(), DaoService.Instance.GetEnterprise());
        agent.GetComponent<RawImage>().DOFade(a, 0);
        // 将agent的z轴定义在后方
        RectTransform rect = agent.GetComponent<RectTransform>();
        if (z != 0)
        {
            rect.SetAsFirstSibling();
        }
        Vector3 position = rect.anchoredPosition3D;
        rect.anchoredPosition3D = rect.anchoredPosition3D + new Vector3(0, 0, z);

        return agent;
    }

}
