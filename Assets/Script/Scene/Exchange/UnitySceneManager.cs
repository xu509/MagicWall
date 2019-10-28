using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MagicWall {
    public class UnitySceneManager : MonoBehaviour
    {


        public void DoChange() {

            var activeScene = SceneManager.GetActiveScene();
            Debug.Log(activeScene.name);
            Debug.Log(activeScene.buildIndex);

            var number = SceneManager.sceneCount;
            Debug.Log("number : " + number);

            Debug.Log("last scene : " + SceneManager.GetSceneAt(number-1).name);

            var lastScene = SceneManager.GetSceneAt(number - 1);


            var sceneCount = SceneManager.sceneCountInBuildSettings;

            int bulidIndex = activeScene.buildIndex;

            //int toIndex = bulidIndex + 1;
            //if (toIndex == sceneCount) {
            //    toIndex = 0;
            //}


            //if (toIndex == 0) {
            //    toIndex = 1;
            //}

            var unloadIndex = lastScene.buildIndex;


            SceneManager.UnloadSceneAsync(lastScene);



            var toIndex = unloadIndex + 1;
            var total = SceneManager.sceneCountInBuildSettings;
            if (toIndex == total)
            {
                toIndex = 1;
            }

            SceneManager.LoadSceneAsync(toIndex, LoadSceneMode.Additive);


            //Debug.Log("Load index : " + toIndex);

            //SceneManager.UnloadSceneAsync(activeScene);

        }


    }


}
