using System;


/// <summary>
///     场景相关
/// </summary>

namespace MagicWall
{
    [Serializable]
    public enum SceneTypeEnum
    {
        CurveStagger,   //曲线
        FrontBackUnfold,    //前后
        LeftRightAdjust,    // 左右
        MidDisperse,    // 中外
        Stars,  // 星空
        UpDownAdjustCutEffect,   // 上下
        VideoBetweenImageSixScene   //6屏，视频在中间

    }
}