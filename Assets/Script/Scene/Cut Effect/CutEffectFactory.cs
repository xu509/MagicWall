using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall {
    public class CutEffectFactory : MonoBehaviour
    {

        public static ICutEffect GetCutEffect(SceneTypeEnum sceneType) {

            ICutEffect cutEffect = null;

            if (sceneType == SceneTypeEnum.CurveStagger)
            {
                cutEffect = new CurveStaggerCutEffect();
            }
            //else if (sceneType == SceneTypeEnum.FrontBackUnfold)
            //{
            //    cutEffect = new FrontBackUnfoldCutEffect();
            //}
            //else if (sceneType == SceneTypeEnum.LeftRightAdjust)
            //{
            //    cutEffect = new LeftRightAdjustCutEffect();
            //}
            //else if (sceneType == SceneTypeEnum.MidDisperse)
            //{
            //    cutEffect = new MidDisperseCutEffect();
            //}
            //else if (sceneType == SceneTypeEnum.UpDownAdjustCutEffect)
            //{
            //    cutEffect = new UpDownAdjustCutEffect();
            //}

            return cutEffect;

        }


    }

}
