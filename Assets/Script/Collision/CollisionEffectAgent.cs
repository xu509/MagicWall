using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall
{

    public interface CollisionEffectAgent
    {
        Vector3 GetRefPosition();

        void SetMoveBehavior(ICollisionMoveBehavior moveBehavior);

        ICollisionMoveBehavior GetMoveBehavior();

        bool IsEffective();

        float GetWidth();

        float GetHeight();

    }
}