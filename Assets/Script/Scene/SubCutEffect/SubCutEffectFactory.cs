using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall {
    public class SubCutEffectFactory : MonoBehaviour
    {
        [SerializeField] SBothImageADEffect _sBothImageADEffect;
        [SerializeField] SNoneEffect _sNoneEffect;


        public ISubCutEffect GetCutEffect(SubCutEffectTypeEnum sceneType) {

            ISubCutEffect cutEffect = null;

            if (sceneType == SubCutEffectTypeEnum.None)
            {
                cutEffect = _sNoneEffect;
            }
            else if (sceneType == SubCutEffectTypeEnum.BothImageAD5p)
            {
                cutEffect = _sBothImageADEffect;
            }

            return cutEffect;
        }


    }

}
