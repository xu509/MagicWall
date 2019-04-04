using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IScene : ScriptableObject
{

    SceneStatus status = SceneStatus.PREPARING; //场景状态
    public SceneStatus Status { get { return status; } set { status = value; } }

    float durtime = 20;
    public float Durtime { set { durtime = value; } get { return durtime; } }

    float startTime = 5;
    public float StartTime { set { startTime = value; } get { return startTime; } }

    public void Awake()
    {

    }


    public abstract void DoInit(MagicWallManager magicWall,CutEffect cutEffect);

    public abstract void DoUpdate(MagicWallManager magicWall);

    public abstract void DoDestory(MagicWallManager magicWall);


}

public enum SceneStatus{
    RUNNING,DESTORING,PREPARING,STARTTING
}