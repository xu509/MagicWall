using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MockSceneConfig : ScriptableObject
{
    [SerializeField]
    public List<SceneConfig> sceneConfigs;

    public float data;

    public void OnEnable() {
        hideFlags = HideFlags.None;
        //hideFlags = HideFlags.HideInHierarchy;
    }

}
