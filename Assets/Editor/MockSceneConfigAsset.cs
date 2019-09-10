using UnityEditor;
using UnityEngine;

public class MockSceneConfigAsset 
{
    [MenuItem("Assets/Create/MagicWall/Mock Scene Config")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<MockSceneConfig>();
    }
}
