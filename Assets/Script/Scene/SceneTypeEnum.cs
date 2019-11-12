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
        VideoBetweenImageSixScene,   //6屏，视频在中间

        /// <summary>
        ///     8屏定制效果 Start
        /// </summary>
        CustomCurveStagger8p,
        CustomFrontBackUnfold8p,
        CustomLeftRightAdjust8p,
        CustomMidDisperse8p,
        Stars8p,
        UpDownAdjust8p,

    }
}