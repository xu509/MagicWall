using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IScene : ScriptableObject
{

    SceneStatus status = SceneStatus.PREPARING; //场景状态
    public SceneStatus Status { get { return status; } set { status = value; } }

    float durtime;
    public float Durtime { set { durtime = value; } get { return durtime; } }

    float cutDurTime;
    public float CutDurTime { set { cutDurTime = value; } get { return cutDurTime; } }

    float deleteDurTime;
    public float DeleteDurTime { set { deleteDurTime = value; } get { return deleteDurTime; } }

    public void Awake()
    {

    }

	public abstract void DoInit(MagicWallManager magicWall,CutEffect cutEffect);

	//
	//	过场动画
	//
	public abstract void DoStarting ();

    public abstract void OnStartComplete();

    public abstract void DoUpdate();

    public abstract void DoDestory();


}

public enum SceneStatus{
    RUNNING,DESTORING,PREPARING,STARTTING
}