using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

//
//	向左移动
//
namespace MagicWall
{
    public class GoLeftDisplayBehavior : CutEffectDisplayBehavior
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
            _manager = _displayBehaviorConfig.Manager;
            _daoService = _manager.daoService;

            flag = false;
        }

        public void Run()
        {
            // 面板向左移动
            Vector3 to = new Vector3(0 - Time.deltaTime * _manager.managerConfig.MainPanelMoveFactor, 0, 0);
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

            FillItem(_displayBehaviorConfig.dataType);
        }

        /// <summary>
        ///     补充内容
        /// </summary>
        /// <param name="dataType"></param>
        private void FillItem(DataTypeEnum dataType)
        {
            float gap = _displayBehaviorConfig.sceneUtils.GetGap();

            // 获取右侧最小的距离
            var rowDic = _displayBehaviorConfig.rowAgentsDic;

            int row = 0;    // 最短行长的行值
            int last_x = int.MaxValue;
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

            float deviationValue = _displayBehaviorConfig.sceneUtils.GetFixedItemHeight() / 2;

            // 超过屏幕的距离
            float overDistense = last_x - deviationValue - Screen.width;

            if ((overDistense - _manager.PanelOffsetX) < 0)
            {
                if (flag == false)
                {
                    // 该行添加内容
                    FlockData data = _manager.daoService.GetFlockData(dataType);
                    Sprite spriteImage = data.GetCoverSprite();

                    int itemHeight = _displayBehaviorConfig.sceneUtils.GetFixedItemHeight();
                    int itemWidth = Mathf.RoundToInt(AppUtils.GetSpriteWidthByHeight(spriteImage, itemHeight));


                    // 拿位置
                    float gen_y = _displayBehaviorConfig.sceneUtils.GetYPositionByFixedHeight(itemHeight, row);
                    float gen_x = last_x + itemWidth / 2 + gap / 2;

                    // 生成 agent
                    //FlockAgent go = _displayBehaviorConfig.ItemsFactory.Generate(gen_x, gen_y, gen_x, gen_y
                    //    , row, bean.column + 1, itemWidth, itemHeight, data, AgentContainerType.MainPanel);

                    FlockAgent go = FlockAgentFactoryInstance.Generate(_manager, new Vector2(gen_x,gen_y), AgentContainerType.MainPanel
                        , gen_x, gen_y, row, bean.column + 1, itemWidth, itemHeight, data);
                    go.flockStatus = FlockStatusEnum.NORMAL;

                    go.NextVector2 = new Vector2(gen_x, gen_y);

                    last_x = Mathf.RoundToInt(last_x + itemWidth + gap / 2);
                    rowDic[row].column = bean.column + 1;
                    rowDic[row].xposition = last_x;
                }
            }
        }


    }
}