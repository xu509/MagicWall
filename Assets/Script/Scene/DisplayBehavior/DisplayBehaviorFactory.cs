using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall {
    public class DisplayBehaviorFactory : MonoBehaviour
    {

        public static CutEffectDisplayBehavior GetBehavior(DisplayBehaviorEnum displayBehaviorEnum) {
            if (displayBehaviorEnum == DisplayBehaviorEnum.FrontBackGoLeft)
            {
                return new FrontBackGoLeftDisplayBehavior();
            }
            else if (displayBehaviorEnum == DisplayBehaviorEnum.GoDown)
            {
                return new GoDownDisplayBehavior();
            }
            else if (displayBehaviorEnum == DisplayBehaviorEnum.GoLeft)
            {
                return new GoLeftDisplayBehavior();
            }
            else if (displayBehaviorEnum == DisplayBehaviorEnum.GoUp)
            {
                return new GoUpDisplayBehavior();
            }
            else if (displayBehaviorEnum == DisplayBehaviorEnum.Stay) {
                return new StayDisplayBehavior();
            }
            else
            {
                return null;
            }
        }


    }

}
