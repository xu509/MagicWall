using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall {
    public class DestoryBehaviorFactory : MonoBehaviour
    {

        public static CutEffectDestoryBehavior GetBehavior(DestoryBehaviorEnum displayBehaviorEnum) {

            if (displayBehaviorEnum == DestoryBehaviorEnum.Fade)
            {
                return new FadeOutDestoryBehavior();
            }
            else if (displayBehaviorEnum == DestoryBehaviorEnum.FallDown) {
                return new FallDownDestoryBehavior();
            }
            else
            {
                return null;
            }
        }


    }

}
