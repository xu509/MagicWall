using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     移动工厂
/// </summary>
namespace MagicWall
{
    public class CollisionMoveBehaviourFactory : MonoBehaviour
    {
        private bool _hasInit = false;

        private ICollisionMoveBehavior _commonMoveBehavior;
        private ICollisionMoveBehavior _roundMoveBehavior;
        private ICollisionMoveBehavior _kinectRoundMoveBehavior;

        private MagicWallManager _manager;

        // Start is called before the first frame update
        void Start()
        {
            if (!_hasInit) {
                Init();
            }

        }

        void Init() {
            _roundMoveBehavior = new CollisionRoundMoveBehavior();
            _commonMoveBehavior = new CollisionRoundMoveBehavior();
            _kinectRoundMoveBehavior = new CollisionKinectRoundMoveBehavior();
            _hasInit = true;

            _manager = GameObject.Find("MagicWall").GetComponent<MagicWallManager>();


        }


        /// <summary>
        /// 获取影响范围
        /// </summary>
        /// <returns></returns>
        public float GetMoveEffectDistance() {
            if (_manager.useKinect)
            {
                return _manager.collisionBehaviorConfig.kinectCardInfluenceMoveFactor;
            }
            else {
                return _manager.collisionBehaviorConfig.InfluenceMoveFactor;
            }
        }

        /// <summary>
        /// 获取动画效果
        /// </summary>
        /// <returns></returns>
        public float GetOffsetEffectDistance()
        {
            if (_manager.useKinect)
            {
                return _manager.collisionBehaviorConfig.KinectRoundOffsetInfluenceFactor;
            }
            else
            {
                return _manager.collisionBehaviorConfig.RoundOffsetInfluenceFactor;
            }
        }





        public ICollisionMoveBehavior GetMoveBehavior(CollisionMoveBehaviourType moveBehaviourType)
        {
            if (!_hasInit) {
                Init();
            }



            if (_manager.useKinect)
            {
                return _kinectRoundMoveBehavior;
            }
            else {

                if (moveBehaviourType == CollisionMoveBehaviourType.Common)
                {
                    return _commonMoveBehavior;
                }
                else if (moveBehaviourType == CollisionMoveBehaviourType.Round)
                {
                    return _roundMoveBehavior;
                }
                else {
                    return null;
                }

            }

        }
    }


}