using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// 过场效果
namespace MagicWall
{
    public interface ICutEffect
    {
        void Init(MagicWallManager manager, SceneConfig sceneConfig,
            Action OnEffectEnd, Action OnDisplayStart, Action OnStartCompleted,
            Action OnDestoryStart, Action OnDestoryCompleted);

        void RunEntrance();

        void RunDisplaying();

        void RunDestoring();

        void Run();

        SceneTypeEnum GetSceneType();
       

    }

}