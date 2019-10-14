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
            _hasInit = true;

        }



        public ICollisionMoveBehavior GetMoveBehavior(CollisionMoveBehaviourType moveBehaviourType)
        {
            if (!_hasInit) {
                Init();
            }



            if (moveBehaviourType == CollisionMoveBehaviourType.Common)
            {
                Debug.Log(11);

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