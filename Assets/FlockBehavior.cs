using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlockBehavior : ScriptableObject
{
    // 计算移动
	public abstract Vector2 CalculateMove(FlockAgent agent, Transform tar, MagicWall magicWall);

	// Do Scale
	public abstract void DoScale(FlockAgent agent,MagicWall magicWall);
}
