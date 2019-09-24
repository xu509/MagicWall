using UnityEngine;
using UnityEditor;

namespace MagicWall
{
    public class CutEffectConfigAsset
    {
        [MenuItem("Assets/Create/MagicWall/CutEffect Config")]
        public static void CreateAsset()
        {
            ScriptableObjectUtility.CreateAsset<CutEffectConfig>();
        }

    }
}