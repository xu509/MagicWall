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


    // 加载场景信息
    public void Init(MagicWall magicWall) {
        Scenes = new List<IScene>();

        EnvScene envScene = new EnvScene();
        Scenes.Add(envScene);
        EnvScene envScene1 = new EnvScene();
        Scenes.Add(envScene1);

        index = 0;
        //currentScene = envScene;
        //currentScene.DoInit(magicWall);
    }



    // 开始场景调整
    public void UpdateItems(MagicWall magicWall) {
        if (Scenes[index].Status == SceneStatus.PREPARING) {
            start_time = Time.time;
            Scenes[index].DoInit(magicWall);
            //Scenes[index].Status = SceneStatus.RUNNING;
            Scenes[index].Status = SceneStatus.STARTTING;
        }

        if (Scenes[index].Status == SceneStatus.STARTTING) {
            //Debug.Log("IS START");

            if ((Time.time - start_time) > Scenes[index].StartTime) {
                Scenes[index].Status = SceneStatus.RUNNING;
            }
        }



        if (Scenes[index].Status == SceneStatus.RUNNING)
        {
            if ((Time.time - start_time) > Scenes[index].Durtime)
            {
                Scenes[index].DoDestory(magicWall);
                Scenes[index].Status = SceneStatus.DESTORING;
            }
            else
            {
                Scenes[index].DoUpdate(magicWall);
            }
        }

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