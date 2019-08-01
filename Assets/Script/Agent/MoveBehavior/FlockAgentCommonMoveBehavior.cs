using EasingUtil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockAgentCommonMoveBehavior : IFlockAgentMoveBehavior
{
    public Vector2 CalculatePosition(Vector2 position, Vector2 positionWithOffset, Vector2 targetPosition, float distance, float effectDistance,
        float width, float height, MagicWallManager manager)
    {
        if (distance > effectDistance)
        {
            return Vector2.zero;
        }
        else {
            // 获取offset_x;offset_y
            float offset_x = Mathf.Abs(positionWithOffset.x - targetPosition.x);
            float offset_y = Mathf.Abs(positionWithOffset.y - targetPosition.y);

            //
            //  上下移动的偏差值
            //
            float move_offset_y = offset_y * ((height / 2) / effectDistance);
            move_offset_y += height / 10 * manager.flockBehaviorConfig.CommonOffsetInfluenceFactor;

            float move_offset_x = offset_x * ((width / 2) / effectDistance);
            move_offset_x += width / 10 * manager.flockBehaviorConfig.CommonOffsetInfluenceFactor;

            float to_y, to_x;
            if (positionWithOffset.y > targetPosition.y)
            {
                to_y = position.y + move_offset_y;
            }
            else if (positionWithOffset.y < targetPosition.y)
            {
                to_y = position.y - move_offset_y;
            }
            else
            {
                to_y = position.y;
            }

            if (positionWithOffset.x > targetPosition.x)
            {
                to_x = position.x + move_offset_x;
            }
            else if (positionWithOffset.x < targetPosition.x)
            {
                to_x = position.x - move_offset_x;
            }
            else
            {
                to_x = position.x;
            }

            Vector2 to = new Vector2(to_x, to_y); //目标位置

            Func<float, float> defaultEasingFunction = EasingFunction.Get(manager.flockBehaviorConfig.CommonEaseEnum);
            float k = defaultEasingFunction((effectDistance - distance) / effectDistance);

            Vector2 r = Vector2.Lerp(position, to, k);

            return r;
        }
    }

    public float CalculateScale(Vector2 position, Vector2 positionWithOffset, Vector2 targetPosition, float distance, float effectDistance, float width, float height, MagicWallManager manager)
    {
        throw new NotImplementedException();
    }
}
