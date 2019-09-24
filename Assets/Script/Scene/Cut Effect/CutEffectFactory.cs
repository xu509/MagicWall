using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  过场效果工厂类 （单例）
//
namespace MagicWall
{
    public class CutEffectFactory : Singleton<CutEffectFactory>
    {

        private List<CutEffect> cutEffects; // 过场效果数组
        public List<CutEffect> CutEffects { get { return cutEffects; } }

        private MagicWallManager magicWallManager;  //	manager

        //
        //	Single Pattern
        //

        void Awake()
        {
            cutEffects = new List<CutEffect>();
            CutEffect curveStaggerCutEffect = new CurveStaggerCutEffect();
            cutEffects.Add(curveStaggerCutEffect);

            CutEffect midDisperseCutEffect = new MidDisperseCutEffect();
            cutEffects.Add(midDisperseCutEffect);

            //CutEffect cutEffect3 = new CutEffect3();
            //cutEffects.Add(cutEffect3);

            CutEffect leftRightAdjustCutEffect = new LeftRightAdjustCutEffect();
            cutEffects.Add(leftRightAdjustCutEffect);

            CutEffect starsCutEffect = new StarsCutEffect();
            cutEffects.Add(starsCutEffect);
        }

        //
        //  Constructor
        //
        protected CutEffectFactory() { }


        //
        //	随机获取过场
        //
        public CutEffect GetByRandom()
        {
            int count = cutEffects.Count;
            if (count == 0)
            {
                return null;
            }
            int index = Random.Range(0, count);

            //int index = 4;

            return cutEffects[index];
        }

        //
        //	根据场景类型获取过场
        //
        public CutEffect GetByScenes(DataType type)
        {
            int count = cutEffects.Count;
            if (count == 0)
            {
                return null;
            }
            int index = Random.Range(0, count);

            //int index = 0;

            return cutEffects[index];
        }

    }
}