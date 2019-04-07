using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 场景管理器
public class SceneManager : ScriptableObject
{
    List<IScene> Scenes;
    int index; // 当前的索引

    float start_time;
    float destoryTime = 2f;

    CutEffect cutEffect;

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

        cutEffect = new CutEffect1();
        //currentScene = envScene;
        //currentScene.DoInit(magicWall);
    }



    // 开始场景调整
    public void UpdateItems() {
		// 准备状态
        if (Scenes[index].Status == SceneStatus.PREPARING) {
            start_time = Time.time;
			Scenes[index].DoInit(magicWallManager, cutEffect);
            //Scenes[index].Status = SceneStatus.RUNNING;
            Scenes[index].Status = SceneStatus.STARTTING;
			Debug.Log ("Scene is Cutting");
			magicWallManager.Status = WallStatusEnum.Cutting;

        }

		// 过场动画
        if (Scenes[index].Status == SceneStatus.STARTTING) {
            //Debug.Log("IS START");

			if ((Time.time - start_time) > Scenes [index].StartTime) {
				Scenes [index].Status = SceneStatus.RUNNING;
				Debug.Log ("Scene is Displaying");
				magicWallManager.Status = WallStatusEnum.Displaying;

			} else {
				Scenes [index].DoStarting ();
			}

        }
               
		// 正常展示
        if (Scenes[index].Status == SceneStatus.RUNNING)
        {
            if ((Time.time - start_time) > Scenes[index].Durtime)
            {
				Scenes[index].DoDestory(magicWallManager);
                Scenes[index].Status = SceneStatus.DESTORING;
            }
            else
            {
				Scenes[index].DoUpdate(magicWallManager);
            }

        }

		// 销毁中
        if (Scenes[index].Status == SceneStatus.DESTORING)
        {
            if ((Time.time + destoryTime - start_time) > Scenes[index].Durtime) {
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



}