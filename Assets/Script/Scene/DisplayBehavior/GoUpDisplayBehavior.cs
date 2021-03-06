﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

//
//	向左移动
//
namespace MagicWall
{
    public class GoUpDisplayBehavior : CutEffectDisplayBehavior
    {
        private MagicWallManager _manager;
        private IDaoService _daoService;
        private DisplayBehaviorConfig _displayBehaviorConfig;

        private bool flag = false;
        //
        //  初始化 （参数：内容类型，row）
        //
        public void Init(DisplayBehaviorConfig displayBehaviorConfig)
        {
            _displayBehaviorConfig = displayBehaviorConfig;

            _manager = displayBehaviorConfig.Manager;
            _daoService = _manager.daoServiceFactory.GetDaoService(displayBehaviorConfig.sceneConfig.daoTypeEnum);

            flag = false;
        }

        public void Run()
        {

            // 面板向上移动
            Vector3 to = new Vector3(0, Time.deltaTime * _manager.managerConfig.MainPanelMoveFactor, 0);
            _manager.mainPanel.transform.Translate(to);

            // 调整panel的差值
            _manager.updateOffsetOfCanvasDirect();

            FillAgents(_displayBehaviorConfig.dataType);
        }


        private void FillAgents(DataTypeEnum dataType)
        {
            float gap = _displayBehaviorConfig.sceneUtils.GetGap();

            // 获取右侧最小的距离
            var columnAgentsDic = _displayBehaviorConfig.columnAgentsDic;

            // 查看每一列顶部是否已低于阈值
            int column = 0;    // 最短的列下表
            int last_y = int.MinValue;

            ItemPositionInfoBean bean = new ItemPositionInfoBean();
            foreach (KeyValuePair<int, ItemPositionInfoBean> keyValuePair in columnAgentsDic)
            {
                if (keyValuePair.Value.yPositionMin > last_y)
                {
                    last_y = keyValuePair.Value.yPositionMin;
                    column = keyValuePair.Key;
                    bean = keyValuePair.Value;
                }
            }

            // 定义偏差值 (因实际运行时会出现延时)
            float deviationValue = _displayBehaviorConfig.sceneUtils.GetFixedItemWidth() / 2;

            // 超过屏幕的距离
            float overDistense = last_y + deviationValue;

            if ((overDistense - _manager.PanelOffsetY) > 0)
            {
                if (flag == false)
                {
                    flag = true;
                    int itemwidth = _displayBehaviorConfig.sceneUtils.GetFixedItemWidth();

                    //在该列上补充一个
                    //FlockData data = _daoService.GetFlockData(dataType);
                    FlockData data = _daoService.GetFlockDataByScene(dataType,_manager.SceneIndex);
                    Sprite coverSprite = data.GetCoverSprite();
                    int itemHeight = Mathf.RoundToInt(AppUtils.GetSpriteHeightByWidth(coverSprite, itemwidth));

                    float ori_x = _displayBehaviorConfig.sceneUtils.GetXPositionByFixedWidth(itemwidth, column);
                    float ori_y = Mathf.RoundToInt(last_y - itemHeight / 2);

                    int row = bean.row + 1;

                    // 创建agent
                    //FlockAgent go = _displayBehaviorConfig.ItemsFactory.Generate(ori_x, ori_y, ori_x, ori_y, row, column,
                    //         itemwidth, itemHeight, data, AgentContainerType.MainPanel);

                    FlockAgent go = FlockAgentFactoryInstance.Generate(_manager, new Vector2(ori_x, ori_y), AgentContainerType.MainPanel
, ori_x, ori_y, row, column, itemwidth, itemHeight, data,DaoTypeEnum.CBHAiqigu);

                    go.flockStatus = FlockStatusEnum.NORMAL;

                    go.NextVector2 = new Vector2(ori_x, ori_y);



                    // 完成创建更新字典
                    last_y = Mathf.RoundToInt(last_y - itemHeight - gap);
                    _displayBehaviorConfig.columnAgentsDic[column].yPositionMin = last_y;
                    _displayBehaviorConfig.columnAgentsDic[column].row = row;
                    flag = false;
                }
            }

        }


    }
}