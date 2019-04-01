using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Environment 
public class EnvScene : IScene
{
    public override void DoDestory(MagicWall magicWall)
    {
        magicWall.DoDestory();
    }

    public override void DoInit(MagicWall magicWall)
    {
        for (int i = 0; i < magicWall.row * magicWall.column; i++)
        {
            magicWall.CreateNewAgent(i);
        }

        Durtime = 20;
    }

    public override void DoUpdate(MagicWall magicWall)
    {
        magicWall.mainPanel.transform.position += (Vector3)Vector2.left * Time.deltaTime * magicWall.driveFactor;

        // 添加并删除碰撞体
        foreach (FlockAgent agent in magicWall.Agents)
        {
            if (magicWall.HasItemsInNear(agent))
            {
                scaleBehavior.DoScale(agent, magicWall);
            }
            else
            {
                if (agent.AgentStatus == AgentStatus.MOVING)
                {
                    // 回归原位
                    Vector2 v2 = recoverBehavior.CalculateMove(agent, null, magicWall);

                    reScaleBehavior.DoScale(agent, magicWall);
                }
            }
        }


    }

}
