using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MEnumClass
{


}

public enum AgentType
{
    env, // 企业
    activity, // 活动
    product // 产品
}

public enum AgentStatus
{
    NORMAL, // 正常
    MOVING, // 移动中
    CHOOSING    //已选择
}


public enum WallStatusEnum
{
    Cutting, // 过场中
    Displaying // 显示中
}

public enum SceneContentType
{
    env, // 企业
    activity, // 活动
    product, // 产品
    none
}

public enum SceneStatus
{
    PREPARING, // 准备中
    STARTTING, // 启动动画中
    DISPLAYING, // 运行中
    DESTORING //  销毁中
}

//
//  卡片状态
//
public enum CardStatusEnum
{
    NORMAL, // 正常
    DESTORING, // 销毁动画中
    DESTORYED, // 已销毁
}

//
//  十字卡片类型
//
public enum CrossCardCategoryEnum {
    INDEX, //       公司卡片
    PRODUCT, //     产品
    ACTIVITY, //    活动
    VIDEO,  //      视频
    CATALOG //      CATALOG
}


