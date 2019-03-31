using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Flock/Behavior/ReScale")]
public class ReScaleBehavior : FlockBehavior
{
	// 计算移动
	public override Vector2 CalculateMove(FlockAgent agent, Transform tar, MagicWall magicWall){
		return Vector2.zero;
	}

	public override void DoScale(FlockAgent agent,MagicWall magicWall){

		// 减半
		float theScaleFactor = magicWall.scaleSpeed;

		// 缩小 
		RectTransform rt = agent.AgentRectTransform;
		rt.DOScale(1f, 1);

//		BoxCollider2D collider = GetComponent<BoxCollider2D>();
//		collider.edgeRadius = AgentMagicWall.agent_colider_radius * scaleFactor;
		magicWall.AdjustAgentStatus(agent);
//		agent.AgentStatus = AgentStatus.NORMAL;


	}


}
