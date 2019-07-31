using EasingUtil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockAgentMoveBehavior2 : IFlockAgentMoveBehavior
{
    public Vector2 CalculatePosition(Vector2 position, Vector2 positionWithOffset, Vector2 targetPosition, float distance, 
        float effectDistance, float width, float height, MagicWallManager manager, EaseEnum InfluenceEaseEnum)
    {
        Func<float, float> easeFun = EasingFunction.Get(InfluenceEaseEnum);


        if (distance > effectDistance)
        {
            return Vector2.zero;
        }
        else {
            // 获取offset_x;offset_y
            float offset_x = Mathf.Abs(positionWithOffset.x - targetPosition.x);
            float offset_y = Mathf.Abs(positionWithOffset.y - targetPosition.y);

            float max_x_moveoffset = width / 4;

            float mid_x = effectDistance / 2;
            float x;

            if (offset_x > mid_x)
            {
                x = Mathf.Lerp(0, max_x_moveoffset, offset_x / mid_x);
            }
            else if (offset_x < mid_x)
            {
                x = Mathf.Lerp(max_x_moveoffset ,0 , (mid_x - offset_x) / mid_x);
            }
            else {
                x = max_x_moveoffset;
            }

            float max_y_moveoffset = height / 4;
            float mid_y = effectDistance / 2;
            float y;

            if (offset_y > mid_y)
            {
                y = Mathf.Lerp(0, max_y_moveoffset, offset_y / mid_y);
            }
            else if (offset_y < mid_y)
            {
                y = Mathf.Lerp(max_y_moveoffset, 0, (mid_y - offset_y) / mid_y);
            }
            else
            {
                y = max_y_moveoffset;
            }


            float to_y, to_x;
            if (positionWithOffset.y > targetPosition.y)
            {
                to_y = position.y + y;
            }
            else if (positionWithOffset.y < targetPosition.y)
            {
                to_y = position.y - y;
            }
            else
            {
                to_y = position.y;
            }

            if (positionWithOffset.x > targetPosition.x)
            {
                to_x = position.x + x;
            }
            else if (positionWithOffset.x < targetPosition.x)
            {
                to_x = position.x - x;
            }
            else
            {
                to_x = position.x;
            }


            Vector2 to = new Vector2(to_x,to_y);

            return to;
        }
    }
}
