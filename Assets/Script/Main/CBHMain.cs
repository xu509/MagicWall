using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MagicWall{
    public class CBHMain : MonoBehaviour
    {
        [SerializeField] UdpManager _udpManager;

        private int _currentIndex = 0;
        string[] scenesNames;

        private bool flag = false;

        // Start is called before the first frame update
        void Start()
        {
            var s = SceneManager.sceneCount;
            var buildSettings = SceneManager.sceneCountInBuildSettings;
            Debug.Log(buildSettings);

            scenesNames = new string[] {
                "ZBHFeiYueWithKinectSix",
                "ZBHfengxian",
                "ZBHTubuVideoScene"

            };
            Debug.Log("Do START");
            Debug.Log("Do START flag : " + flag);

            _currentIndex = 0;

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;


            SceneManager.LoadScene(scenesNames[_currentIndex],LoadSceneMode.Additive);

            flag = true;

        }

        // Update is called once per frame
        void Update()
        {
            
        
        }


        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
           

            //SceneManager.LoadScene(scenesNames[toIndex]);
        }

        private void OnSceneUnloaded(Scene current)
        {
            //if (_udpManager.receMsg)
            //{
            //    Debug.Log("OnSceneUnloaded");

            //}
            //else {
            //    Debug.Log("OnSceneUnloaded: " + current.name);
            //    var unloadIndex = current.buildIndex;

            //    var toIndex = unloadIndex + 1;
            //    var total = SceneManager.sceneCountInBuildSettings;
            //    if (toIndex == total)
            //    {
            //        toIndex = 1;
            //    }

            //    SceneManager.LoadSceneAsync(toIndex, LoadSceneMode.Additive);
            //}







        }


    }

}