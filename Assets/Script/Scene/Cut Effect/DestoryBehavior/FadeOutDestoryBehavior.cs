using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutDestoryBehavior : CutEffectDestoryBehavior
{
    bool hasInit = false;
    float startTime;
    float totalTime;
    
    public void Init(float destoryDurTime)
    {
        totalTime = destoryDurTime;
    }

    public void Run()
    {
        if (!hasInit) {
            startTime = Time.time;
            hasInit = true;    
        }
        // TODO 淡出
        float time = Time.time - startTime;  // 当前已运行的时间;
        float a = Mathf.Lerp(1, 0, time / totalTime);
        MagicWallManager.Instance.mainPanel.GetComponent<CanvasGroup>().alpha = a;
        MagicWallManager.Instance.mainPanel.GetComponentInChildren<CanvasGroup>().alpha = a;
    }
}
