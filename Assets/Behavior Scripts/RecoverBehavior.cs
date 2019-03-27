using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//纵坐标校准
[CreateAssetMenu(menuName = "Flock/Behavior/Recover")]
public class RecoverBehavior : FlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, MagicWall magicWall)
    {
        Vector2 move = Vector2.zero;


        // ##上下位置校准
        //RectTransform transform = agent.GetComponent<RectTransform>();

        RectTransform transform = agent.AgentRectTransform;
        float current_y = transform.anchoredPosition.y;
        // 通过 current_y 找到需要校准的y轴
        float target_y = magicWall.flock_width / 2;

        int n = (int)current_y / magicWall.flock_width;
        if (n > 0) { 
            target_y += n * magicWall.flock_width;
        }

        if (current_y < target_y)
            move = Vector2.up;
        else if (current_y > target_y)
        {
            move = Vector2.down;
        }
        else
        {
            move = Vector2.zero;
        }

        // ## 左右位置校准
        //Vector2 move2 = Vector2.zero;

        //float current_x = transform.anchoredPosition.x;
        //float ref_x = magicWall.Ref_Agent.anchoredPosition.x;
        ////Debug.Log("ref_x : " + ref_x);


        //float real_x = current_x - ref_x;
        //float target_x = magicWall.flock_width / 2;

        //int m = (int)real_x / magicWall.flock_width;

        //if (m > 0)
        //{
        //    target_x += m * magicWall.flock_width + ref_x;
        //}

        ////if (current_x < target_x)
        ////    move2 = Vector2.right * 2;
        ////else if (current_x > target_x)
        ////{
        ////    move2 = Vector2.left * 2;
        ////}
        ////else
        ////{
        ////    move2 = Vector2.zero * 2;
        ////}

        ////move2 = new V

        //move.x = target_x;
        //move.y = target_y;

        //Debug.Log(move);





        //Debug.Log(agent.gameObject.name + " : c - " + current_y + " ; t - " + target_y  + " [[" + n + "]]");



        return move;
    }
}
