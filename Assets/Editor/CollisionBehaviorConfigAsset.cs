using UnityEngine;
using UnityEditor;

namespace MagicWall
{
    public class CollisionBehaviorConfigAsset
    {
        [MenuItem("Assets/Create/MagicWall/Collision Config")]
        public static void CreateAsset()
        {
            ScriptableObjectUtility.CreateAsset<CollisionBehaviorConfig>();
        }

    }
}