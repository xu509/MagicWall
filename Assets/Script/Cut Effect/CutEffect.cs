using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 过场效果 1 
public abstract class CutEffect : MonoBehaviour
{
    public abstract void run(FlockAgent prefab, MagicWallManager magicWallManager);

}
