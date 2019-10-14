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

        private float _width;
        public float width
        {
            get
            {
                return GetComponent<RectTransform>().rect.width;
            }
        }
        private float _height;
        public float height
        {
            get
            {
                return GetComponent<RectTransform>().rect.height;
            }
        }

        /// <summary>
        ///     碰撞体移动表现器
        /// </summary>
        private ICollisionMoveBehavior _collisionMoveBehavior;

        public float GetHeight()
        {
            Vector3 scaleVector3 = GetComponent<RectTransform>().localScale;
            return height * scaleVector3.y * 2f;
        }

        public ICollisionMoveBehavior GetMoveBehavior()
        {
            return _collisionMoveBehavior;
        }

        public string GetName()
        {
            return gameObject.name;
        }

        public Vector3 GetRefPosition()
        {
            var pos = GetComponent<RectTransform>().position;
            var screenPosition = RectTransformUtility.WorldToScreenPoint(null, pos);

            //var sposition = _manager.mainCamera.WorldToScreenPoint(pos);

            //Debug.Log(gameObject.name + " | position : " + pos + " - get ref position : " + screenPosition);

            return screenPosition;
        }

        public float GetWidth()
        {
            Vector3 scaleVector3 = GetComponent<RectTransform>().localScale;
            return width * scaleVector3.x * 2f;
        }

        public bool IsEffective()
        {
            return true;
        }

        public void SetMoveBehavior(ICollisionMoveBehavior moveBehavior)
        {
            _collisionMoveBehavior = moveBehavior;
        }
    }

}
