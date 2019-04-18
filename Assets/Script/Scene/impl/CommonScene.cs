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

    //
    private bool isDoDestoryCompleting = false;

    //  使用的过场效果
    private CutEffect _theCutEffect;

    //  使用的类型
    private SceneContentType _sceneContentType;

    //  场景开始的时间
    private float _startTime;

    //  场景显示的时间点
    private float _displayTime;

    //  场景开始销毁的时间点
    private float _destoryTime;

    //  场景状态
    SceneStatus status = SceneStatus.PREPARING; //场景状态
    public SceneStatus Status { get { return status; } set { status = value; } }

    // Dao Service
    DaoService daoService;

    //
    //  Private Methods
    //

	//	过场动画
	private void DoStarting()
	{
        _theCutEffect.Starting();
	}

	//  展示动画
	private void DoDisplaying()
	{
        _theCutEffect.Displaying();

        
    }

    //  销毁动画
    private void DoDestorying() {
        _theCutEffect.Destorying();
    }

	//销毁动画已完成
	private bool DoDestoryCompleted()
	{

        if (!isDoDestoryCompleting)
        {
            isDoDestoryCompleting = true;
            // 清理面板
            return MagicWallManager.Instance.Clear();
        }
        else {
 
            return false;
        }

	}

	//
	//  初始化
	// -- 初始化 Display时间
	// -- 初始化当前的过场效果
	//
	private void DoCreating()
	{
        _theCutEffect.Create(_sceneContentType);

        isDoDestoryCompleting = false;
    }


	//
	//	Export Methods
	//

	//	配置
	public void DoConfig(SceneConfig sceneConfig)
	{
        _theCutEffect = sceneConfig.CutEffect; // 设置过场效果
        _sceneContentType = sceneConfig.SceneContentType; // 设置类型

    }

	//  运行
	public bool Run()
	{
		MagicWallManager magicWallManager = MagicWallManager.Instance;

		// 准备状态
		if (Status == SceneStatus.PREPARING)
		{
			// 进入过场状态
			//Debug.Log("Scene is Cutting");
            _startTime = Time.time; //标记开始的时间

			MagicWallManager.Instance.Status = WallStatusEnum.Cutting;   //标记项目进入过场状态

			DoCreating();  //初始化场景

            Debug.Log("当前场景 ： " + GetContentType());

			// 将状态标志设置为开始
			Status = SceneStatus.STARTTING;
		}

		// 过场动画
		if (Status == SceneStatus.STARTTING)
		{
			if ((Time.time - _startTime) > _theCutEffect.StartingDurTime)
			{
                // 完成开场动画，场景进入展示状态
                
                // 调用效果完成回调
                _theCutEffect.OnStartingCompleted();

                _displayTime = Time.time;
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
			if (!_theCutEffect.HasDisplaying || (Time.time - _displayTime) > _theCutEffect.DisplayDurTime)
			{
				// 完成展示阶段，进行销毁
				DoDestorying();
				Status = SceneStatus.DESTORING;
                _destoryTime = Time.time;
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
            if ((Time.time - _destoryTime) > _theCutEffect.DestoryDurTime)
			{
                if (DoDestoryCompleted())
				{
                    Status = SceneStatus.PREPARING;
					return false;
				}
				else
				{

                    Debug.Log("DoDestorying 4");
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

    public SceneContentType GetContentType()
    {
        return _sceneContentType;
    }
}
