using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MEnumClass
{


}

public enum DataType
{
    env, // 企业
    activity, // 活动
    product, // 产品
    none
}


public enum WallStatusEnum
{
    Cutting, // 过场中
    Displaying // 显示中
}



//
//  卡片状态
//
public enum CardStatusEnum
{
    NORMAL, // 正常
    DESTORING_STEP_FIRST, // 销毁动画中(第一次)
    DESTORYING_STEP_SCEOND, // 进行第二次动画中
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


/// <summary>
///     场景相关
/// </summary>

public enum SceneTypeEnum {
    CurveStagger,   //曲线
    FrontBackUnfold,    //前后
    LeftRightAdjust,    // 左右
    MidDisperse,    // 中外
    Stars,  // 星空
    UpDownAdjustCutEffect   // 上下
}


public enum SceneStatus
{
    PREPARING, // 准备中
    STARTTING, // 启动动画中
    DISPLAYING, // 运行中
    DESTORING //  销毁中
}

public enum CustomImageType
{
    LEFT1, LEFT2, RIGHT
}
