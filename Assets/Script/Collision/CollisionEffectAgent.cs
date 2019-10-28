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

        float GetEffectDistance();

        bool IsEffective();

        float GetWidth();

        float GetHeight();

        string GetName();

        void SetDisableEffect(bool disableEffect);

    }
}