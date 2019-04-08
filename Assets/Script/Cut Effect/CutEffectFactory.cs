using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutEffectFactory{

	private static CutEffectFactory instance;
	private List<CutEffect> cutEffects;	// 过场效果数组
	public List<CutEffect> CutEffects{get{return cutEffects;}}


	private MagicWallManager magicWallManager;	//	manager

	//
	//	Single Pattern
	//
	public static CutEffectFactory GetInstance(MagicWallManager magicWallManager){
		if (instance == null) {
			instance = new CutEffectFactory (magicWallManager);
		}
		return instance;
	}

	//
	// Construct
	//
	private CutEffectFactory(MagicWallManager magicWallManager){
		this.magicWallManager = magicWallManager;
		cutEffects = new List<CutEffect> ();

		CutEffect cutEffect1 = new CutEffect1 ();
//		cutEffect1.init (magicWallManager);
		cutEffects.Add (cutEffect1);

		CutEffect cutEffect2 = new CutEffect2 ();
//		cutEffect2.init (magicWallManager);
		cutEffects.Add (cutEffect2);

		CutEffect cutEffect3 = new CutEffect3 ();
//		cutEffect3.init (magicWallManager);
		cutEffects.Add (cutEffect3);
	}

	//
	//	随机获取过场
	//
	public CutEffect getByRandom(){
		int count = cutEffects.Count;
		if (count == 0) {
			return null;
		}
		int index = Random.Range (0,count);
//		Debug.Log ("Random : " + index);
		return cutEffects [index];
	}

}
