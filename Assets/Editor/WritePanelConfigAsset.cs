using UnityEngine;
using UnityEditor;

public class WritePanelConfigAsset
{
    [MenuItem("Assets/Create/MagicWall/WritePanel Config")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<WritePanelConfig>();
    }

}
