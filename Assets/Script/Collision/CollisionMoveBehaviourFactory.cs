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
        private ICollisionMoveBehavior _commonMoveBehavior;
        private ICollisionMoveBehavior _roundMoveBehavior;

        // Start is called before the first frame update
        void Start()
        {
            //_commonMoveBehavior = new FlockAgentCommonMoveBehavior();
            _roundMoveBehavior = new CollisionRoundMoveBehavior();
        }

        public ICollisionMoveBehavior GetMoveBehavior(CollisionMoveBehaviourType moveBehaviourType)
        {
            if (moveBehaviourType == CollisionMoveBehaviourType.Common)
            {
                return _commonMoveBehavior;
            }
            else if (moveBehaviourType == CollisionMoveBehaviourType.Round)
            {
                return _roundMoveBehavior;
            }
            else
            {
                return null;
            }
        }
    }


}