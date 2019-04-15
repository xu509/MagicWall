using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 过场效果
public abstract class CutEffect : MonoBehaviour
{
    //
    //  Parameter
    //
    CutEffectDisplayBehavior displayBehavior; //表现

    // 运行状态标志
    bool hasRuning = true;
    public bool HasRuning { set { hasRuning = value; } get { return hasRuning; } }

    // 切换动画时长
    float startingDurTime;
    public float StartingDurTime { set { startingDurTime = value; } get { return startingDurTime; } }

    // 显示动画的时长
    float displayDurTime;
    public float DisplayDurTime { set { displayDurTime = value; } get { return displayDurTime; } }

    internal CutEffectDisplayBehavior DisplayBehavior { get { return displayBehavior; } set { displayBehavior = value; } }

    //
    //  Method
    //
    public abstract void Create();

    public abstract void Starting();

    //	显示中
    public void Displaying() {
        DisplayBehavior.Run();
    }

	
	//	销毁中
	public abstract void Destorying();

    public abstract void OnStartingCompleted();

}

