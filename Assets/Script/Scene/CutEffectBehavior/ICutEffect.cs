using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// 过场效果
namespace MagicWall
{
    public interface ICutEffect { 
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="sceneConfig"></param>
        /// <param name="OnCreateAgentCompleted">Agent 创建回调</param>
        /// <param name="OnEffectCompleted">效果完成回调</param>
        /// <param name="OnDisplayStart">提示运行动画回调</param>
        void Init(MagicWallManager manager, SceneConfig sceneConfig,
            Action<DisplayBehaviorConfig> OnCreateAgentCompleted,
            Action OnEffectCompleted, Action OnDisplayStart);

        void Run();

        SceneTypeEnum GetSceneType();
       

    }

}