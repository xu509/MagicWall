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

    [Range(0f, 10f), Header("影响的参数")]
    public float CommonEffectInfluenceFactor = 0.5f;

    [Header("[Round] 动画效果")]
    public EaseEnum RoundEaseEnum;



}
