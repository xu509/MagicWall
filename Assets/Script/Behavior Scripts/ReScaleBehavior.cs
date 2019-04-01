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

        if (!agent.IsScale)
        {
            // 缩小 
            RectTransform rt = agent.AgentRectTransform;
            rt.DOScale(1f, 1).OnComplete(() => MyCallback(agent)); ;

            agent.IsScale = true;
        }
        else
        {
            RectTransform rt = agent.AgentRectTransform;

            if (rt.localScale == Vector3.one)
            {
                agent.scaleFactor = 1f;
                agent.IsScale = false;

                magicWall.AdjustAgentStatus(agent);
            }
        }

        //		BoxCollider2D collider = GetComponent<BoxCollider2D>();
        //		collider.edgeRadius = AgentMagicWall.agent_colider_radius * scaleFactor;
        //		agent.AgentStatus = AgentStatus.NORMAL;


    }

    void MyCallback(FlockAgent agent)
    {
        agent.IsScale = false;
        agent.ScaleFactor = 1f;

    }


}
