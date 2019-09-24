using UnityEngine;
using UnityEditor;

namespace MagicWall
{
    public class ManagerConfigAsset
    {
        [MenuItem("Assets/Create/MagicWall/Manager Config")]
        public static void CreateAsset()
        {
            ScriptableObjectUtility.CreateAsset<ManagerConfig>();
        }

    }
}