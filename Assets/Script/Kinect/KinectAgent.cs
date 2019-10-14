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
        private ICollisionMoveBehavior _collisionMoveBehavior;

        private CardAgent _refCardAgent;
        public CardAgent refCardAgent { set { _refCardAgent = value; } get { return _refCardAgent; } }




        public float GetHeight()
        {
            var height = GetComponent<RectTransform>().rect.height;

            Vector3 scaleVector3 = GetComponent<RectTransform>().localScale;
            return 800f;
        }

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


        public ICollisionMoveBehavior GetMoveBehavior()
        {
            return _collisionMoveBehavior;
        }

        public string GetName()
        {
            return "kinect";
        }

        public Vector3 GetRefPosition()
        {
            var pos = GetComponent<RectTransform>().position;
            var screenPosition = RectTransformUtility.WorldToScreenPoint(null, pos);
            return screenPosition;
        }

        public float GetWidth()
        {
            var width = GetComponent<RectTransform>().rect.width;

            Vector3 scaleVector3 = GetComponent<RectTransform>().localScale;
            return 800f;
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
