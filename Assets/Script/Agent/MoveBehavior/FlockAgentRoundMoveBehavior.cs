using EasingUtil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  
/// </summary>
public class FlockAgentRoundMoveBehavior : IFlockAgentMoveBehavior
{
    /// <summary>
    ///  将影响范围内的点直接衍生至已影响范围的半径
    /// </summary>
    /// <param name="position"></param>
    /// <param name="positionWithOffset"></param>
    /// <param name="targetPosition"></param>
    /// <param name="distance"></param>
    /// <param name="effectDistance"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="manager"></param>
    /// <param name="InfluenceEaseEnum"></param>
    /// <returns></returns>
    public Vector2 CalculatePosition(Vector2 position, Vector2 positionWithOffset, Vector2 targetPosition, float distance, 
        float effectDistance, float width, float height, MagicWallManager manager)
    {
        Func<float, float> easeFun = EasingFunction.Get(manager.flockBehaviorConfig.RoundEaseEnum);

        Vector2 panelOffset = positionWithOffset - position;

        if (distance > effectDistance)
        {
            return position;
        }
        else {
            // positionWithOffset 原位置

            // targetPosition 目标位置

            Vector2 to = targetPosition + (positionWithOffset - targetPosition).normalized * effectDistance;

            to = to - panelOffset;

            return to;
        }
    }
}
