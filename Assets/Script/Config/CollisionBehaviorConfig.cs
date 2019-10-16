using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasingUtil;


/// <summary>
///     Flock 移动功能配置文件
/// </summary>
namespace MagicWall
{
    public class CollisionBehaviorConfig : ScriptableObject
    {

        [SerializeField] public CollisionMoveBehaviourType behaviourType;

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

        [Range(0f, 2f), Header("位移影响参数")]
        public float RoundOffsetInfluenceFactor;

        [Range(0f, 1000f), Header("位移变量")]
        public float RoundOffsetInfluenceOffset;

        [Range(0f, 10f), Header("影响疲劳时间")]
        public float EffectTiredTime;

        [Header("[KinectRound] 动画效果")]
        public EaseEnum KinectRoundEaseEnum;

        [Header("缩放动画效果")]
        public EaseEnum KinectRoundScaleEaseEnum;

        [Range(0f, 2f), Header("位移影响参数")]
        public float KinectRoundOffsetInfluenceFactor;





    }
}