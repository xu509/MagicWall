using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerConfig : ScriptableObject
{
    [Header("是否是定制屏")]
    public bool IsCustom = false;

    [Range(3, 8), Header("点开的最大数量")]
    public int SelectedItemMaxCount;

    /// <summary>
    /// 8屏时，固定为28列; 5 屏 18
    /// </summary>
    [Range(5, 30), Header("固定的列数")]
    public int Column;

    [Range(10, 500), Header("浮动块对象池大小")]
    public int FlockPoolSize;

    [Range(1, 30), Header("操作块对象池大小")]
    public int CardPoolSize;


    [Range(0f, 500f), Header("[星空效果] 卡片数量")]
    public int StarEffectAgentsCount;

    [Range(0f, 200f), Header("[星空效果] 动画移动速度")]
    public float StarEffectMoveFactor;

    [Range(1f, 10000f), Header("[星空效果] 动画起点")]
    public float StarEffectOriginPoint;

    [Range(-5000f, 5000f), Header("[星空效果] 动画终点")]
    public float StarEffectEndPoint;

    [Range(10,200),Header("[背景] 清晰的背景气球池")]
    public int BackgroundClearBubblePoolSize;

    [Range(0, 10f), Header("[背景] 清晰的气球最小移动速度")]
    public float BackgroundClearMoveMinFactor;

    [Range(0, 10f), Header("[背景] 清晰的气球最大移动速度")]
    public float BackgroundClearMoveMaxFactor;

    [Range(10, 200), Header("[背景] 模糊的背景气球池")]
    public int BackgroundDimBubblePoolSize;

    [Range(0, 10f), Header("[背景] 模糊的气球最小移动速度")]
    public float BackgroundDimMoveMinFactor;

    [Range(0, 10f), Header("[背景] 模糊的气球最大移动速度")]
    public float BackgroundDimMoveMaxFactor;

    [Range(0f, 5f), Header("[卡片] 最小的宽度系数（对应屏幕宽度）")]
    public float ItemSizeMinWidthFactor;

    [Range(0f, 5f), Header("[卡片] 最大的宽度系数（对应屏幕宽度）")]
    public float ItemSizeMaxWidthFactor;

    [Range(0f, 5f), Header("[卡片] 最小的高度系数（对应屏幕高度）")]
    public float ItemSizeMinHeightFactor;

    [Range(0f, 5f), Header("[卡片] 最大的高度系数（对应屏幕高度）")]
    public float ItemSizeMaxHeightFactor;


}
