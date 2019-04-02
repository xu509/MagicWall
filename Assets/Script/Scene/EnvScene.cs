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

        Durtime = 60;
    }

    public override void DoUpdate(MagicWall magicWall)
    {
        magicWall.mainPanel.transform.position += (Vector3)Vector2.left * Time.deltaTime * magicWall.driveFactor;

        // 添加并删除碰撞体
        foreach (FlockAgent agent in magicWall.Agents)
        {
            if (magicWall.HasItemsInNear(agent))
            {
                //Debug.Log(agent.name + " has items in near !");
                scaleBehavior.DoScale(agent, magicWall);
            }
            else
            {
                int x =  Mathf.RoundToInt(agent.AgentRectTransform.anchoredPosition.x);
                int y = Mathf.RoundToInt(agent.AgentRectTransform.anchoredPosition.y);
                int t_x = Mathf.RoundToInt(agent.TarVector2.x);
                int t_y = Mathf.RoundToInt(agent.TarVector2.y);

                bool isAtCurrentPostion = (x == t_x) && (y == t_y);

                if (agent.AgentStatus == AgentStatus.MOVING)
                {
                    // 回归原位
                    recoverBehavior.CalculateMove(agent, null, magicWall);
                    if (isAtCurrentPostion) {
                        reScaleBehavior.DoScale(agent, magicWall);
                    }
                }
                // 位置不相同时
                else if (agent.AgentStatus != AgentStatus.CHOOSING)
                {
                    if (!isAtCurrentPostion)
                    {
                        //Debug.Log(agent.name + " - anchored postion : [ " + x + " , " + y + " ] ; tar postion : [" + t_x + " , " + t_y + " ]");
                        scaleBehavior.DoScale(agent, magicWall);
                        //Debug.Log("After Scale");
                        
                        //Debug.Log(agent.name + " : x :" + x + " - y: " + y);
                        agent.AgentStatus = AgentStatus.MOVING;
                    } else if (agent.ScaleFactor != 1) {
                        agent.AgentStatus = AgentStatus.MOVING;

                    }


                }



            }
        }


    }

}
