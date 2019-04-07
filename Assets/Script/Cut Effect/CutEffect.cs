using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 过场效果 1 
public abstract class CutEffect : MonoBehaviour
{
	public abstract void init(FlockAgent prefab, MagicWallManager magicWallManager,float durtime);


    public abstract void run();

}
