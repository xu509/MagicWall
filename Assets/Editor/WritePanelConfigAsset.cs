using UnityEngine;
using UnityEditor;
namespace MagicWall
{
    public class WritePanelConfigAsset
    {
        [MenuItem("Assets/Create/MagicWall/WritePanel Config")]
        public static void CreateAsset()
        {
            ScriptableObjectUtility.CreateAsset<WritePanelConfig>();
        }

    }
}