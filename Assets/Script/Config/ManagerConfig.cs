using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerConfig : ScriptableObject
{
    [Header("是否是定制屏")]
    public bool IsCustom = false;

    [Range(3, 8), Header("点开的最大数量")]
    public int SelectedItemMaxCount;

    [Range(10, 500), Header("浮动块对象池大小")]
    public int FlockPoolSize;

    [Range(1, 30), Header("操作块对象池大小")]
    public int CardPoolSize;

    [Range(100f, 10000f), Header("[星空效果] 最远距离")]
    public float StarEffectDistance;

    [Range(2f, 10f), Header("[星空效果] 动画时间")]
    public float StarEffectDistanceTime;

    [Range(10,200),Header("[背景] 清晰的背景气球池")]
    public int ClearBubblePoolSize;
    
    [Range(10, 200), Header("[背景] 模糊的背景气球池")]
    public int DimBubblePoolSize;

    [Range(0.1f,5f),Header("[背景] 气球的生成间隔(清晰)")]
    public float ClearBubbbleCreateIntervalTime;

    [Range(0.1f, 5f), Header("[背景] 气球的生成间隔(模糊)")]
    public float DimBubbbleCreateIntervalTime;

}
