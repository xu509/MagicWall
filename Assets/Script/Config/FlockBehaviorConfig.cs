using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasingUtil;


/// <summary>
///     Flock 移动功能配置文件
/// </summary>
public class FlockBehaviorConfig : ScriptableObject
{

    [SerializeField] public MoveBehaviourType MoveBehaviourType;

    [Range(0f, 10f), Header(" 影响移动距离系数")]
    public float InfluenceMoveFactor = 0.5f;

    [Header("[Common] 动画效果")]
    public EaseEnum CommonEaseEnum;

    [Header("缩放动画效果")]
    public EaseEnum CommonScaleEaseEnum;

    [Range(0f, 10f), Header("位移影响参数")]
    public float CommonOffsetInfluenceFactor;

    [Header("[Round] 动画效果")]
    public EaseEnum RoundEaseEnum;

    [Header("缩放动画效果")]
    public EaseEnum RoundScaleEaseEnum;

    [Header("位移影响参数")]
    public float RoundOffsetInfluenceFactor;



}
