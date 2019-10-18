using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall
{
    public class FadeOutDestoryBehavior : CutEffectDestoryBehavior
    {
        private MagicWallManager _manager;

        bool hasInit = false;
        float startTime;
        float totalTime = 1f;

        Action _onDestoryCompleted;

        //public void Init(MagicWallManager manager, float destoryDurTime)
        //{
        //    totalTime = destoryDurTime;
        //    _manager = manager;
        //}

        public void Init(MagicWallManager manager, Action onDestoryCompleted)
        {
            _manager = manager;
            _onDestoryCompleted = onDestoryCompleted;
        }

        public void Run()
        {
            if (!hasInit)
            {
                startTime = Time.time;
                hasInit = true;
            }
            float time = Time.time - startTime;  // 当前已运行的时间;
            float a = Mathf.Lerp(1, 0, time / totalTime);
            _manager.mainPanel.GetComponent<CanvasGroup>().alpha = a;
            //_manager.mainPanel.GetComponentInChildren<CanvasGroup>().alpha = a;

            if (time >= totalTime) {
                _onDestoryCompleted.Invoke();
            }

        }
    }
}