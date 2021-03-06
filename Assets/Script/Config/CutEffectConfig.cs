﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasingUtil;


/// <summary>
///     过场动画配置表
/// </summary>
namespace MagicWall
{
    public class CutEffectConfig : ScriptableObject
    {

        [SerializeField, Header("Mid Disperse")] public EaseEnum MidDisperseMoveEaseEnum;

        [Range(0f, 10f)]
        public float MidDisperseDelayMax;

        [Range(0f, 10f)]
        public float MidDisperseDisplayTime;    // 显示时间

        /// <summary>
        /// 以高度为基数，显示透明度开始变化的位置点。 
        /// 比如 value = 1 时，则当实体距离目标位置等于高度时，开始变换透明度。
        /// </summary>
        [Range(1f, 3f)]
        public float MidDisperseAlphaMinDistanceFactor;


        public EaseEnum MidDisperseAlphaEaseEnum;

        [Range(-10f, 10f)]
        public float MidDisperseHeightFactor;



        [SerializeField, Header("LeftRightAdjust")] public EaseEnum LeftRightDisplayEaseEnum;
        [Range(0f, 10f)] public float LeftRightDisplayDurTime;
        [Range(0f, 10f),Tooltip("间隔时间")] public float LeftRightGapTime = 0.3f;
        [SerializeField] public EaseEnum LeftRightGapEaseEnum;


        [SerializeField, Header("UpDownAdjustCutEffect")] public EaseEnum UpDownDisplayEaseEnum;
        [Range(0f, 10f)] public float UpDownDisplayDurTime;

        [SerializeField, Header("前后分层")] public EaseEnum FrontBackDisplayEaseEnum;
        [Range(0f, 10f)] public float FrontBackDisplayDurTime;

        [SerializeField, Header("曲线")] public EaseEnum CurveStaggerDisplayEaseEnum;
        [Range(0f, 10f)] public float CurveStaggerDisplayDurTime;


        [SerializeField, Range(0f, 500f), Header("[星空效果] 卡片数量")]
        public int StarEffectAgentsCount;

        [SerializeField, Range(0f, 200f), Header("[星空效果] 动画移动速度")]
        public float StarEffectMoveFactor;

        [SerializeField, Range(1f, 10000f), Header("[星空效果] 动画起点")]
        public float StarEffectOriginPoint;

        [SerializeField, Range(-5000f, 5000f), Header("[星空效果] 动画终点")]
        public float StarEffectEndPoint;

        [SerializeField, Range(0f, 500f), Header("下坠速度")]
        public float FallDownSpeed;

        [SerializeField, Range(0f, 10f), Header("下坠间隔时间")]
        public float FallDownGapTime;

        [SerializeField, Range(0f, 10f), Header("下坠噪音")]
        public float FallDownNoise;

        [SerializeField,Range(0f,50f),Header("下坠总时间")]
        public float FallDownTotalTime;

        [SerializeField, Range(0f, 5f), Header("下坠LOGO渐隐时间")]
        public float FallDownLogoFadeTime;

        [SerializeField, Range(0, 12), Header("[前后分层效果] 行数")]
        public int FrontBackRow;

    }
}