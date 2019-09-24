using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     移动工厂
/// </summary>
namespace MagicWall
{
    public class MoveBehaviourFactory : MonoBehaviour
    {
        private IFlockAgentMoveBehavior _commonMoveBehavior;
        private IFlockAgentMoveBehavior _roundMoveBehavior;

        // Start is called before the first frame update
        void Start()
        {
            _commonMoveBehavior = new FlockAgentCommonMoveBehavior();
            _roundMoveBehavior = new FlockAgentRoundMoveBehavior();
        }

        public IFlockAgentMoveBehavior GetMoveBehavior(MoveBehaviourType moveBehaviourType)
        {
            if (moveBehaviourType == MoveBehaviourType.Common)
            {
                return _commonMoveBehavior;
            }
            else if (moveBehaviourType == MoveBehaviourType.Round)
            {
                return _roundMoveBehavior;
            }
            else
            {
                return null;
            }
        }
    }

    public enum MoveBehaviourType
    {
        Common, // 正常的类型
        Round   // 圆形
    }
}