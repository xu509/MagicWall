using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  过场效果工厂类 （单例）
//
public class CutEffectFactory : Singleton<CutEffectFactory>
{

	private List<CutEffect> cutEffects;	// 过场效果数组
	public List<CutEffect> CutEffects{get{return cutEffects;}}

	private MagicWallManager magicWallManager;  //	manager

    //
    //	Single Pattern
    //

    void Awake()
    {
        cutEffects = new List<CutEffect>();
        CutEffect cutEffect1 = new CutEffect1();
        //		cutEffect1.init (magicWallManager);
        cutEffects.Add(cutEffect1);

        CutEffect cutEffect2 = new CutEffect2();
        cutEffects.Add(cutEffect2);

        //CutEffect cutEffect3 = new CutEffect3();
        //cutEffects.Add(cutEffect3);

        CutEffect cutEffect4 = new CutEffect4();
        cutEffects.Add(cutEffect4);

        CutEffect cutEffect5 = new CutEffect5();
        cutEffects.Add(cutEffect5);
    }

    //
    //  Constructor
    //
    protected CutEffectFactory() { }


	//
	//	随机获取过场
	//
	public CutEffect getByRandom(){
        int count = cutEffects.Count;
        if (count == 0)
        {
            return null;
        }
        int index = Random.Range(0, count);

        //int index = 4;

        return cutEffects [index];
	}

    //
    //	根据场景类型获取过场
    //
    public CutEffect GetByScenes(SceneType type)
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
