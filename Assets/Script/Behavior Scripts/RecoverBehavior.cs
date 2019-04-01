using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Flock/Behavior/Recover")]
public class RecoverBehavior : FlockBehavior
{
	public override Vector2 CalculateMove(FlockAgent agent, Transform tar, MagicWall magicWall)
    {
        Vector2 move = Vector2.zero;

		if (agent.AgentRectTransform.anchoredPosition.sqrMagnitude != agent.TarVector2.sqrMagnitude) {
			move = agent.TarVector2 - agent.AgentRectTransform.anchoredPosition;
//			Debug.Log(agent.name + " - [Local]" + agent.AgentRectTransform.anchoredPosition.sqrMagnitude 
//				+ "[Tar]" + agent.TarVector2.sqrMagnitude + "To :" + move);
			if (move.sqrMagnitude > Vector2.one.sqrMagnitude) {
				move = move.normalized * magicWall.recoverFactor;
				agent.Move (move);
			} else {
				agent.MoveToPosition (agent.AgentRectTransform.anchoredPosition + move);
			}
		
		} else {
			magicWall.AdjustAgentStatus (agent);
		}

        return move;
    }

	public override void DoScale(FlockAgent agent,MagicWall magicWall){
	}

}
