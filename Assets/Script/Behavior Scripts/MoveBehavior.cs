using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Move")]
public class MoveBehavior : FlockBehavior
{
	public override Vector2 CalculateMove(FlockAgent agent, Transform tar, MagicWallManager magicWall)
    {
		float neighbor_radius = 439f;


		Vector2 move = Vector2.zero;

		bool isUpper = false; // 在目标物的上侧
		bool isEqual = false; // 在目标物的下侧

		// 获取两个点之间点距离
		float distance = (tar.position - agent.AgentRectTransform.transform.position).sqrMagnitude;
		float rdistance = (tar.position - magicWall.RefObj.position).sqrMagnitude;
		Debug.Log ("[" + rdistance + "] -> " + distance + " - distance");



		if (agent.AgentRectTransform.transform.position.y > tar.position.y) {
			isUpper = true;
		} else if (agent.AgentRectTransform.transform.position.y < tar.position.y) {
			isUpper = false;
		} else {
			isEqual = true;
		}



		if (isEqual) {
				
		} else {
			if (distance != neighbor_radius) {
				if (isUpper) {
					// 向下移
					move = Vector2.down;
					Debug.Log (agent.name + " - Do Down");


				}
				if (!isUpper) {
					// 向上移
					move = Vector2.up;
					Debug.Log (agent.name + " - Do Up");

				}
			
			}

		}



        return move;

    }

	public override void DoScale(FlockAgent agent,MagicWallManager magicWall){
	}

		
}
