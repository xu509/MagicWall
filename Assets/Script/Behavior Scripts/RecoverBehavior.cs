using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



[CreateAssetMenu(menuName = "Flock/Behavior/Recover")]
public class RecoverBehavior : FlockBehavior
{
	public override Vector2 CalculateMove(FlockAgent agent, Transform tar, MagicWallManager magicWall)
    {
        Vector2 move = Vector2.zero;

        //if (agent.isRunning)
        //    return move;



        //else {
        //if (agent.AgentRectTransform.anchoredPosition.sqrMagnitude != agent.TarVector2.sqrMagnitude)
        //{
        //    move = agent.TarVector2 - agent.AgentRectTransform.anchoredPosition;
        //    if (move.sqrMagnitude > Vector2.one.sqrMagnitude)
        //    {

        //        float distance = Vector2.Distance(agent.TarVector2, agent.AgentRectTransform.anchoredPosition);
        //        // 判断距离，如果过远，则增加移动速度

        //        move = move.normalized * magicWall.recoverFactor;
        //        if (distance > 10 && distance <= 20)
        //        {
        //            move *= 2f;
        //        }
        //        else if (distance > 20)
        //        {
        //            move *= 3f;
        //        }

        //        //if (Vector2.Distance(agent.TarVector2, agent.AgentRectTransform.anchoredPosition) > 200)
        //        //    move = move * 1.5f;
        //        //Debug.Log(agent.name + " distance : " + distance);
        //        agent.signTextComponent.text = distance.ToString();
        //        agent.transform.position += (Vector3)move * Time.deltaTime;
        //        //agent.Move(move);
        //    }
        //    else
        //    {
        //        agent.AgentRectTransform.DOAnchorPos(agent.AgentRectTransform.anchoredPosition + move, 0.5f);
        //    }
        //}
        //else
        //{
        //    magicWall.AdjustAgentStatus(agent);
        //}

        return move;
    }

	public override void DoScale(FlockAgent agent,MagicWallManager magicWall){
	}

}
