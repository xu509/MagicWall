using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 场景管理器
public class SceneManager : ScriptableObject
{
    List<IScene> Scenes;
    int index; // 当前的索引

    float start_time;
    float destoryingDuringTime = 2f; // 销毁的时间
    float destory_time; // Destory_Time

    CutEffect cutEffect; // 当前的过场动画	

	private MagicWallManager magicWallManager;


    // 加载场景信息
    public void Init(MagicWallManager magicWall) {
		magicWallManager = magicWall;
        Scenes = new List<IScene>();

        // 装载场景
        //StartScene startScene = new StartScene();
        //Scenes.Add(startScene);
        EnvScene envScene = new EnvScene();
        Scenes.Add(envScene);
        index = 0;

//		cutEffect = CutEffectFactory.GetInstance ().getByRandom ();
    }

    //
    // 开始场景调整
    //
    public void UpdateItems() {
		// 准备状态
        if (Scenes[index].Status == SceneStatus.PREPARING) {
            start_time = Time.time;
			loadCutEffect ();
			Scenes[index].DoInit(magicWallManager, cutEffect);
            Scenes[index].Status = SceneStatus.STARTTING;
			Debug.Log ("Scene is Cutting");
			magicWallManager.Status = WallStatusEnum.Cutting;
            destoryingDuringTime = Scenes[index].DeleteDurTime;
        }

		// 过场动画
        if (Scenes[index].Status == SceneStatus.STARTTING) {
			if ((Time.time - start_time) > cutEffect.DurTime) {
                // 完成开场动画
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

        //Debug.Log("(Time.time - start_time) :" + (Time.time - start_time) + " -> Scene[0].Durtime :" + Scene[0].Durtime);
    }

	//
	//	Load effect
	//
	private void loadCutEffect(){
		cutEffect = CutEffectFactory.GetInstance(magicWallManager).getByRandom();
	}


}