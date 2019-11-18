using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall {
    public class WholeEffectFactory : MonoBehaviour
    {
        [SerializeField] WBothImageADEffect _sBothImageADEffect;
        [SerializeField] WNoneEffect _sNoneEffect;


        public IWholeEffect GetWholeEffect(WholeEffectEffectTypeEnum sceneType) {

            IWholeEffect cutEffect = null;

            if (sceneType == WholeEffectEffectTypeEnum.None)
            {
                cutEffect = _sNoneEffect;
            }
            else if (sceneType == WholeEffectEffectTypeEnum.BothImageAD5p)
            {
                cutEffect = _sBothImageADEffect;
            }

            return cutEffect;
        }


    }

}
