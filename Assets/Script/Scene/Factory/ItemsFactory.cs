using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  浮块的设计
//
public interface ItemsFactory
{
    void Init(MagicWallManager manager);

    FlockAgent Generate(float gen_x, float gen_y, float ori_x, float ori_y, int row, int column,
        float width, float height, BaseData data, AgentContainerType agentContainerType);

    CardAgent GenerateCardAgent(Vector3 genPos, FlockAgent flockAgent,int dataId,bool isActive);

}
