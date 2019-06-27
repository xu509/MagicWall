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
        FillAgents(DataType.activity);
    }

    private void UpdateAgentsOfProduct()
    {
        FillAgents(DataType.product);
    }

    void UpdateAgentsOfEnv()
    {
        FillAgents(DataType.env);                   
    }

    private void FillAgents(DataType dataType) {

        //Debug.Log("Fill Agents");

        float gap = _displayBehaviorConfig.sceneUtils.GetGap();

        // 获取右侧最小的距离
        var columnAgentsDic = _displayBehaviorConfig.columnAgentsDic;

        // 查看每一列顶部是否已低于阈值
        int column = 0;    // 最短的列下表
        int last_y = 10000000;
        ItemPositionInfoBean bean = new ItemPositionInfoBean();
        foreach (KeyValuePair<int, ItemPositionInfoBean> keyValuePair in columnAgentsDic)
        {
            if (keyValuePair.Value.yposition < last_y)
            {
                last_y = keyValuePair.Value.yposition;
                column = keyValuePair.Key;
                bean = keyValuePair.Value;
            }
        }

        
        // 定义偏差值 (因实际运行时会出现延时)
        float deviationValue = _displayBehaviorConfig.sceneUtils.GetFixedItemWidth() / 2;

        // 超过屏幕的距离
        float overDistense = last_y - Screen.height - deviationValue;

        if ((overDistense - _manager.PanelOffsetY) < 0)
        {
            if (flag == false)
            {
                flag = true;
                int itemwidth = _displayBehaviorConfig.sceneUtils.GetFixedItemWidth();


                //在该列上补充一个
                FlockData data = _daoService.GetFlockData(dataType);
                Sprite coverSprite = data.GetCoverSprite();
                int itemHeight = Mathf.RoundToInt(AppUtils.GetSpriteHeightByWidth(coverSprite, itemwidth));

                float ori_x = _displayBehaviorConfig.sceneUtils.GetXPositionByFixedHeight(itemwidth, column);
                float ori_y = Mathf.RoundToInt(last_y + itemHeight / 2);

                int row = bean.row + 1;

                // 创建agent
                FlockAgent go = _displayBehaviorConfig.ItemsFactory.Generate(ori_x, ori_y, ori_x, ori_y, row, column,
                         itemwidth, itemHeight, data, AgentContainerType.MainPanel);


                // 完成创建更新字典
                last_y = Mathf.RoundToInt(last_y + itemHeight + gap);
                _displayBehaviorConfig.columnAgentsDic[column].yposition = last_y;
                _displayBehaviorConfig.columnAgentsDic[column].row = row;
                flag = false;
            }
        }
    }
}
