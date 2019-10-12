using EasingUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall
{
    public interface ICollisionMoveBehavior
    {

        Vector2 CalculatePosition(Vector2 position, Vector2 targetPosition,
            float distance, float effectDistance, float width, float height,
            MagicWallManager manager);


        float CalculateScale(Vector2 position, Vector2 targetPosition,
            float distance, float effectDistance, float width, float height,
            MagicWallManager manager);

    }
}