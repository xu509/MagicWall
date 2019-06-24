using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerConfig : ScriptableObject
{
    [Header("是否是定制屏")]
    public bool IsCustom = false;

    [Range(100f, 10000f), Header("星空效果，最远距离")]
    public float StarEffectDistance;

    [Range(2f, 10f), Header("星空效果，动画时间")]
    public float StarEffectDistanceTime;

    [Range(3, 8), Header("点开的最大数量")]
    public int SelectedItemMaxCount;


}
