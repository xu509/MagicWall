using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IScene : ScriptableObject
{
    public FlockBehavior moveBehavior;
    public FlockBehavior scaleBehavior;
    public FlockBehavior reScaleBehavior;
    public FlockBehavior recoverBehavior;

    SceneStatus status = SceneStatus.PREPARING; //场景状态
    public SceneStatus Status { get { return status; } set { status = value; } }

    float durtime = 20;
    public float Durtime { set { durtime = value; } get { return durtime; } }

    float startTime = 5;
    public float StartTime { set { startTime = value; } get { return startTime; } }

    public void Awake()
    {
        moveBehavior = new MoveBehavior();
        scaleBehavior = new ScaleBehavior();
        reScaleBehavior = new ReScaleBehavior();
        recoverBehavior = new RecoverBehavior();
    }


    public abstract void DoInit(MagicWall magicWall);

    public abstract void DoUpdate(MagicWall magicWall);

    public abstract void DoDestory(MagicWall magicWall);


}

public enum SceneStatus{
    RUNNING,DESTORING,PREPARING,STARTTING
}