using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Move")]
public class MoveBehavior : FlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, MagicWall magicWall)
    {
        // 向左移动
        Vector2 move = Vector2.left;



        return move;

    }
}
