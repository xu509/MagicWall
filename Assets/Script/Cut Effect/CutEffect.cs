using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 过场效果 1 
public abstract class CutEffect : Object
{
    //
    //  Parameter
    //
    public MagicWallManager manager;

    // 运行状态标志
    bool hasRuning = true;
    public bool HasRuning { set { hasRuning = value; } get { return hasRuning; } }

    // 持续的运行时间
    float durTime;
    public float DurTime { set { durTime = value; } get { return durTime; } }

    public abstract void init();

    public abstract void run();

    public abstract void OnCompleted();


}
