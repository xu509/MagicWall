using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// 普通场景
public class CommonScene : IScene
{
	//
	//  Parameter
	//

	//  场景状态
	SceneStatus status = SceneStatus.PREPARING; //场景状态
	public SceneStatus Status { get { return status; } set { status = value; } }

	//  场景开始的时间
	float startTime;
	public float StartTime { set { startTime = value; } get { return startTime; } }

	// 运行时间
	float durtime;
	public float Durtime { set { durtime = value; } get { return durtime; } }

	// 过场动画持续时间
	float cutDurTime;
	public float CutDurTime { set { cutDurTime = value; } get { return cutDurTime; } }

	//  场景显示的时间点
	float displayTime;
	public float DisplayTime { set { displayTime = value; } get { return displayTime; } }

	//  场景开始销毁的时间点
	float destoryTime;
	public float DestoryTime { set { destoryTime = value; } get { return destoryTime; } }

	// 销毁动画持续时间
	float destoryDurTime;
	public float DestoryDurTime { set { destoryDurTime = value; } get { return destoryDurTime; } }

	// 使用的过场效果
	CutEffect theCutEffect;
	public CutEffect TheCutEffect { set { theCutEffect = value; } get { return theCutEffect; } }

	// Dao Service
	DaoService daoService;

	//	过场动画
	private void DoStarting()
	{
		theCutEffect.Starting();
	}

	//  展示动画
	private void DoDisplaying()
	{
		theCutEffect.Displaying();
	}

	//  销毁动画
	private void DoDestorying() { }

	//销毁动画已完成
	private bool DoDestoryCompleted()
	{
		// 清理面板
		return MagicWallManager.Instance.Clear();
	}

	//
	//  初始化
	// -- 初始化 Display时间
	// -- 初始化当前的过场效果
	//
	private void DoCreating()
	{
		theCutEffect.Create();
	}


	//
	//	Export Method
	//

	//	配置
	public void DoConfig()
	{

	}

	//  运行
	public bool Run()
	{
		MagicWallManager magicWallManager = MagicWallManager.Instance;

		// 准备状态
		if (Status == SceneStatus.PREPARING)
		{
			// 进入过场状态
			Debug.Log("Scene is Cutting");
			StartTime = Time.time; //标记开始的时间

			MagicWallManager.Instance.Status = WallStatusEnum.Cutting;   //标记项目进入过场状态

			DoCreating();  //初始化场景

			// 将状态标志设置为开始
			Status = SceneStatus.STARTTING;
		}

		// 过场动画
		if (Status == SceneStatus.STARTTING)
		{
			if ((Time.time - StartTime) > theCutEffect.StartingDurTime)
			{
				// 完成开场动画，场景进入展示状态
				Debug.Log("Scene is Displaying");
				DisplayTime = Time.time;
				Status = SceneStatus.DISPLAYING;
				magicWallManager.Status = WallStatusEnum.Displaying;
			}
			else
			{
				DoStarting();
			}
		}

		// 正常展示
		if (Status == SceneStatus.DISPLAYING)
		{
			// 过场状态具有运行状态 或 已达到运行的时间
			if (!theCutEffect.HasRuning || (Time.time - DisplayTime) > Durtime)
			{
				// 完成展示阶段，进行销毁
				DoDestorying();
				Status = SceneStatus.DESTORING;
				destoryTime = Time.time;
			}
			else
			{
				DoDisplaying();
			}
		}

		// 销毁中
		if (Status == SceneStatus.DESTORING)
		{
			// 达到销毁的时间
			if ((Time.time - destoryTime) > destoryDurTime)
			{
				if (DoDestoryCompleted())
				{
					Status = SceneStatus.PREPARING;
					return false;
				}
				else
				{
					DoDestorying();
				}
			}
			else
			{
				DoDestorying();
			}
		}
		return true;
	}

}
