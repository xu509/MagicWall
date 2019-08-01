using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasingUtil;


/// <summary>
///     过场动画配置表
/// </summary>
public class CutEffectConfig : ScriptableObject
{

    [SerializeField,Header("Mid Disperse")] public EaseEnum MidDisperseMoveEaseEnum;

    [Range(0f, 10f)]
    public float MidDisperseDelayMax;

    public EaseEnum MidDisperseAlphaEaseEnum;

    [Range(-10f, 10f)]
    public float MidDisperseHeightFactor;


}
