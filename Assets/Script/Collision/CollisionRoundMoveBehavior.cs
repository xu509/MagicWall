using EasingUtil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall
{
    /// <summary>
    ///  
    /// </summary>
    public class CollisionRoundMoveBehavior : ICollisionMoveBehavior
    {
        /// <summary>
        ///  将影响范围内的点直接衍生至已影响范围的半径
        /// </summary>
        /// <param name="position">屏幕坐标</param>
        /// <param name="targetPosition">屏幕坐标</param>
        /// <param name="distance"></param>
        /// <param name="effectDistance"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="manager"></param>
        /// <param name="InfluenceEaseEnum"></param>
        /// <returns>屏幕 坐标</returns>
        public Vector2 CalculatePosition(Vector2 position, Vector2 targetPosition, float distance,
            float effectDistance, float width, float height, MagicWallManager manager)
        {
            Func<float, float> easeFun = EasingFunction.Get(manager.flockBehaviorConfig.RoundEaseEnum);
            //Vector2 panelOffset = positionWithOffset - position;

            if (distance > effectDistance)
            {
                return position;
            }
            else
            {
                // positionWithOffset 原位置
                // targetPosition 目标位置
                float k = manager.flockBehaviorConfig.RoundOffsetInfluenceFactor;
                k = easeFun(k);

                float e = (effectDistance - distance) * k + distance;

                //Vector2 to = targetPosition + (positionWithOffset - targetPosition).normalized * effectDistance;
                Vector2 to = targetPosition + (position - targetPosition).normalized * e;

                //to = to - panelOffset;
                
                return to;
            }
        }

        public float CalculateScale(Vector2 position, Vector2 targetPosition, float distance, float effectDistance, float width, float height, MagicWallManager manager)
        {
            Func<float, float> easeFun = EasingFunction.Get(manager.flockBehaviorConfig.RoundEaseEnum);

            //Vector2 panelOffset = positionWithOffset - position;

            float offset = effectDistance - distance;

            if (distance > effectDistance)
            {
                return 1f;
            }
            else
            {
                float maxScale = 1f;
                float minScale = 0.1f;

                float k = easeFun(offset / effectDistance);

                float s = Mathf.Lerp(maxScale, minScale, k);
                return s;
            }
        }
    }
}