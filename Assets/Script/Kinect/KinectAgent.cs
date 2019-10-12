using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall {

    /// <summary>
    ///   体感实体
    /// </summary>
    public class KinectAgent : MonoBehaviour, CollisionEffectAgent
    {
        public float GetHeight()
        {
            throw new NotImplementedException();
        }

        public ICollisionMoveBehavior GetMoveBehavior()
        {
            throw new NotImplementedException();
        }

        public Vector3 GetRefPosition()
        {
            throw new NotImplementedException();
        }

        public float GetWidth()
        {
            throw new NotImplementedException();
        }

        public bool IsEffective()
        {
            throw new NotImplementedException();
        }

        public void SetMoveBehavior(ICollisionMoveBehavior moveBehavior)
        {
            throw new NotImplementedException();
        }
    }

}
