using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//横坐标
[CreateAssetMenu(menuName = "Flock/Behavior/Recover1")]
public class RecoverBehavior1 : FlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, MagicWall magicWall)
    {
        //Vector2 move = Vector2.zero;


        //// ##上下位置校准
        ////RectTransform transform = agent.GetComponent<RectTransform>();

        RectTransform transform = agent.AgentRectTransform;
        //float current_y = transform.anchoredPosition.y;
        //// 通过 current_y 找到需要校准的y轴
        //float target_y = magicWall.flock_width / 2;

        //int n = (int)current_y / magicWall.flock_width;
        //if (n > 0) { 
        //    target_y += n * magicWall.flock_width;
        //}

        //if (current_y < target_y)
        //    move =  Vector2.up;
        //else if (current_y > target_y)
        //{
        //    move = Vector2.down;
        //}
        //else
        //{
        //    move = Vector2.zero;
        //}

        // ## 左右位置校准
        Vector2 move2 = Vector2.zero;

        int current_x = (int)transform.anchoredPosition.x;
        int ref_x = (int)magicWall.Ref_Agent.anchoredPosition.x;
        //Debug.Log("ref_x : " + ref_x);


        int real_x = current_x - ref_x;
        int target_x = magicWall.flock_width / 2;

        int m = (int)real_x / magicWall.flock_width;

        if (m > 0) {
            target_x = m * magicWall.flock_width + ref_x;
        }

        if (current_x < target_x) { 
            move2 = Vector2.right;
            agent.gameObject.GetComponentInChildren<Image>().color = Color.red;
        }
        else if (current_x > target_x)
        {
            // 此处会出现误判， 当左侧已有 agent 时，还会不断的向左移动。
            move2 = Vector2.left;
            agent.gameObject.GetComponentInChildren<Image>().color = Color.blue;
        }
        else
        {
            move2 = Vector2.zero;
        }






        //Debug.Log(agent.gameObject.name + " : c - " + current_x + " ; t - " + target_x + " [[" + m + "]]");



        return Vector2.zero;
    }
}
