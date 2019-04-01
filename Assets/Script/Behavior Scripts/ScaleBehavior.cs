using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Flock/Behavior/Scale")]
public class ScaleBehavior : FlockBehavior
{
	// 计算移动
	public override Vector2 CalculateMove(FlockAgent agent, Transform tar, MagicWall magicWall){
		return Vector2.zero;
	}

	public override void DoScale(FlockAgent agent,MagicWall magicWall){

        if (!agent.IsScale)
        {
            // 减半
            float theScaleFactor = magicWall.scaleSpeed;

            // 缩小 
            RectTransform rt = agent.AgentRectTransform;
            rt.DOScale(0.6f, 1).OnComplete(() => MyCallback(agent)); 

            agent.AgentStatus = AgentStatus.MOVING;

            agent.IsScale = true;

        }


        //		BoxCollider2D collider = GetComponent<BoxCollider2D>();
        //		collider.edgeRadius = AgentMagicWall.agent_colider_radius * scaleFactor;

    }

    void MyCallback(FlockAgent agent) {
        agent.IsScale = false;
        agent.ScaleFactor = 0.6f;

    }


}
