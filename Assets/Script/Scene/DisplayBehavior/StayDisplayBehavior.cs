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
    public class StayDisplayBehavior : CutEffectDisplayBehavior
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
            _daoService = _manager.daoService;

            flag = false;
        }

        public void Run()
        {
            // 保持静止
            
        }

    }
}