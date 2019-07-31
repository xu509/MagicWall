using UnityEngine;
using UnityEditor;

public class FlockBehaviorConfigAsset
{
    [MenuItem("Assets/Create/MagicWall/FlockBehavior Config")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<FlockBehaviorConfig>();
    }

}
