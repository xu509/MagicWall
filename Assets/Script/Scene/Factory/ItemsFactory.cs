using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  浮块的设计
//
public interface ItemsFactory
{
    void Init(MagicWallManager manager);

    FlockAgent Generate(float gen_x, float gen_y, float ori_x, float ori_y, int row, int column, float width, float height, BaseData data, Transform transform);

    CardAgent GenerateCardAgent(Vector3 genPos, FlockAgent flockAgent,int dataId,bool isActive);

    Vector2 GetOriginPosition(int row,int column);
    //上下交叉效果结束向上移动，从上向下创建
    Vector2 GoUpGetOriginPosition(int row, int column);

    float GetItemWidth();

    float GetItemHeight();

    int GetSceneColumn();

    float GetSceneGap();

}
