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

        // 向右下角缩小
        Collider2D[] contextColliders = new Collider2D[10];
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        int isOverlap = agent.GetComponent<Collider2D>().OverlapCollider(contactFilter2D, contextColliders);
        if (isOverlap > 0)
        {
            RectTransform rc = agent.GetComponent<RectTransform>();
            //Vector2 v2 = rc.sizeDelta;
            //v2.x--;
            //v2.y--;

            //rc.sizeDelta = v2;




            Debug.Log(agent.gameObject.name + " is over lap!");
        }
        else {
        }

        return move;

    }
}
