using UnityEditor;
using UnityEngine;

namespace MagicWall
{
    public class MockSceneConfigAsset
    {
        [MenuItem("Assets/Create/MagicWall/Mock Scene Config")]
        public static void CreateAsset()
        {
            ScriptableObjectUtility.CreateAsset<MockSceneConfig>();
        }
    }
}