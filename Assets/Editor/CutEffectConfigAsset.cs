using UnityEngine;
using UnityEditor;

public class CutEffectConfigAsset
{
    [MenuItem("Assets/Create/MagicWall/CutEffect Config")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<CutEffectConfig>();
    }

}
