﻿using System.Collections;
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

        Vector3 to = new Vector3(0 - Time.deltaTime * _manager.MovePanelFactor, 0, 0);
        _manager.mainPanel.transform.Translate(to);

        Vector3 backTo = new Vector3(Time.deltaTime * _manager.MovePanelFactor / 2, 0, 0);
        _manager.backPanel.transform.Translate(backTo);

        // 调整panel的差值
        _manager.updateOffsetOfCanvas();


        //  检测每行的数据，当半数行都小的时候，重新开始创建一定数量
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
        FillItemAgency(DataType.env);
    }

    private void UpdateAgentsOfActivity()
    {
        FillItemAgency(DataType.activity);
    }

    private void UpdateAgentsOfProduct()
    {
        FillItemAgency(DataType.product);
    }

    /// <summary>
    ///    当条件达成时，补一列
    /// </summary>
    /// <param name="dataType"></param>
    private void FillItemAgency(DataType dataType) {

        int generatePositionX  = _displayBehaviorConfig.generatePositionX;
        int generatePositionXInBack = _displayBehaviorConfig.generatePositionXInBack;
        

        // 补充前排
        if ((generatePositionX - _manager.mainPanel.rect.width) < _manager.PanelOffsetX) {
            int itemHeight = _displayBehaviorConfig.sceneUtils.GetFixedItemHeight();
            int gap = _displayBehaviorConfig.sceneUtils.GetGap();
            int generate_x_temp = 0;

            // 创造一列前排
            int column = _displayBehaviorConfig.Column;

            bool isOddColumn = column % 2 == 0;

            for (int i = 0; i < _manager.Row; i++)
            {
                //  获取行数奇数状态
                bool isOddRow = i % 2 == 0;

                if ((isOddColumn && isOddRow) || (!isOddRow && !isOddColumn))
                {
                    //  获取要创建的内容
                    FlockData agent = _daoService.GetFlockData(dataType);
                    Sprite coverSprite = agent.GetCoverSprite();
                    float imageWidth = coverSprite.rect.width;
                    float imageHeight = coverSprite.rect.height;

                    // 得到调整后的长宽
                    Vector2 imageSize = AppUtils.ResetTexture(new Vector2(imageWidth, imageHeight),
                        _manager.displayFactor);

                    FlockAgent go;
                    float ori_y = _displayBehaviorConfig.sceneUtils.GetYPositionByFixedHeight(itemHeight, i);

                    float ori_x = generatePositionX + gap + imageSize.x / 2;

                    if (ori_x + gap + imageSize.x / 2 > generate_x_temp)
                    {
                        generate_x_temp = Mathf.RoundToInt(ori_x + gap + imageSize.x / 2);
                    }

                    // 创建前排
                    go = _displayBehaviorConfig.ItemsFactory.Generate(ori_x, ori_y, ori_x, ori_y, i, column,
                        imageSize.x, imageSize.y, agent, AgentContainerType.MainPanel);
                }
                else {
                    continue;
                }
            }

            // 更新 generate_x 的值
            int generate_x = Mathf.RoundToInt(generate_x_temp);
            _displayBehaviorConfig.generatePositionX = generate_x;
            //_displayBehaviorConfig.generatePositionXInBack = generate_x;
            _displayBehaviorConfig.Column = column + 1;
        }



        // 补充后排
        if ((generatePositionXInBack - _manager.mainPanel.rect.width) < _manager.PanelBackOffsetX)
        {
            int itemHeight = _displayBehaviorConfig.sceneUtils.GetFixedItemHeight();
            int gap = _displayBehaviorConfig.sceneUtils.GetGap();
            int generate_x_temp = 0;

            // 创造一列前排
            int column = _displayBehaviorConfig.ColumnInBack;

            bool isOddColumn = column % 2 == 0;

            for (int i = 0; i < _manager.Row; i++)
            {
                //  获取行数奇数状态
                bool isOddRow = i % 2 == 0;

                if ((isOddColumn && isOddRow) || (!isOddRow && !isOddColumn))
                {
                    continue;
                }
                else
                {
                    //  获取要创建的内容
                    FlockData agent = _daoService.GetFlockData(dataType);
                    Sprite coverSprite = agent.GetCoverSprite();
                    float imageWidth = coverSprite.rect.width;
                    float imageHeight = coverSprite.rect.height;

                    // 得到调整后的长宽
                    Vector2 imageSize = AppUtils.ResetTexture(new Vector2(imageWidth, imageHeight),
                        _manager.displayFactor);

                    FlockAgent go;
                    float ori_y = _displayBehaviorConfig.sceneUtils.GetYPositionByFixedHeight(itemHeight, i);
                    float ori_x = generatePositionXInBack + gap + imageSize.x / 2;

                    if (ori_x + gap + imageSize.x / 2 > generate_x_temp)
                    {
                        generate_x_temp = Mathf.RoundToInt(ori_x + gap + imageSize.x / 2);
                    }

                    // 创建前排
                    go = _displayBehaviorConfig.ItemsFactory.Generate(ori_x, ori_y, ori_x, ori_y, i, column,
                        imageSize.x, imageSize.y, agent, AgentContainerType.BackPanel);
                    go.NextVector2 = new Vector2(ori_x, ori_y);
                    go.UpdateImageAlpha(0.2f);

                }
            }

            // 更新 generate_x 的值
            int generate_x = Mathf.RoundToInt(generate_x_temp);
            _displayBehaviorConfig.generatePositionXInBack = generate_x;
            _displayBehaviorConfig.ColumnInBack = column + 1;
        }


    }


}
