using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  场景管理器
//  ---
//  1. 场景问题
//  2. 背景问题
//  3. 过场动画问题
//
public class SceneManager : ScriptableObject
{
    List<IScene> Scenes;
    int index; // 当前的索引

    float start_time;
    float destoryingDuringTime = 2f; // 销毁的时间
    float destory_time; // Destory_Time

    CutEffect cutEffect; // 当前的过场动画	

    // 管理器
    private BackgroundManager backgroundManager; // 背景管理器
    private MagicWallManager magicWallManager;
    

    //
    // 加载场景信息
    //
    public void Init(MagicWallManager magicWall) {
		magicWallManager = magicWall;
        Scenes = new List<IScene>();

        // 装载场景
        //StartScene startScene = new StartScene();
        //Scenes.Add(startScene);
        EnvScene envScene = new EnvScene();
        Scenes.Add(envScene);
        index = 0;

        // 初始化背景管理器
        backgroundManager = new BackgroundManager();
        backgroundManager.init(magicWall);

        //		cutEffect = CutEffectFactory.GetInstance ().getByRandom ();
    }

    //
    // 开始场景调整
    //
    public void UpdateItems() {

        // 背景始终运行
        backgroundManager.run();

        // 准备状态
        if (Scenes[index].Status == SceneStatus.PREPARING) {
            // 进入过场状态
            Debug.Log("Scene is Cutting");
            start_time = Time.time; //标记开始的时间
			loadCutEffect (); //加载过场效果
            magicWallManager.Status = WallStatusEnum.Cutting;   //标记项目进入过场状态
            Scenes[index].DoInit(magicWallManager, cutEffect);  //初始化场景状态
            Scenes[index].Status = SceneStatus.STARTTING;
            destoryingDuringTime = Scenes[index].DeleteDurTime;
        }

		// 过场动画
        if (Scenes[index].Status == SceneStatus.STARTTING) {
			if ((Time.time - start_time) > cutEffect.DurTime) {
                // 完成开场动画，场景进入展示状态
                Debug.Log ("Scene is Displaying");
				Scenes [index].Status = SceneStatus.RUNNING;
				magicWallManager.Status = WallStatusEnum.Displaying;
			} else {
				Scenes [index].DoStarting ();
			}
        }
               
		// 正常展示
        if (Scenes[index].Status == SceneStatus.RUNNING)
        {
            // 过场状态具有运行状态 或 已达到运行的时间
            if (!cutEffect.HasRuning || (Time.time - start_time) > Scenes[index].Durtime)
            {
                Scenes[index].DoDestory();
                Scenes[index].Status = SceneStatus.DESTORING;
                destory_time = Time.time;
            }
            else
            {
                Scenes[index].DoUpdate();
            }
        }

		// 销毁中
        if (Scenes[index].Status == SceneStatus.DESTORING)
        {
            // 达到销毁的时间
            if ((Time.time - destory_time) > destoryingDuringTime) {
                Scenes[index].Status = SceneStatus.PREPARING;
                if (index == Scenes.Count - 1)
                {
                    index = 0;
                }
                else {
                    index++;
                }
            }
        }

    }

	//
	//	Load effect
	//
	private void loadCutEffect(){
		cutEffect = CutEffectFactory.GetInstance(magicWallManager).getByRandom();
	}


}