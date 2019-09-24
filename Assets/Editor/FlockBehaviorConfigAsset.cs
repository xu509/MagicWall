using UnityEngine;
using UnityEditor;

namespace MagicWall
{
    public class FlockBehaviorConfigAsset
    {
        [MenuItem("Assets/Create/MagicWall/FlockBehavior Config")]
        public static void CreateAsset()
        {
            ScriptableObjectUtility.CreateAsset<FlockBehaviorConfig>();
        }

    }
}