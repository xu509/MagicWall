using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  配置表映射
//
public class AppConfig
{
    public static string KEY_THEME_ID = "theme_id";

    // 曲线交错持续时间
    public static string KEY_CutEffectDuring_CurveStagger = "cuteffectduring_curvestagger";

    // 左右校准持续时间
    public static string KEY_CutEffectDuring_LeftRightAdjust = "cuteffectduring_leftrightadjust";

    // 中间散开持续时间
    public static string KEY_CutEffectDuring_MidDisperseAdjust = "cuteffectduring_middisperse";

    // 星空效果持续时间
    public static string KEY_CutEffectDuring_Stars = "cuteffectduring_stars";

    // 上下校准持续时间
    public static string KEY_CutEffectDuring_UpDownAdjust = "cuteffectduring_updownadjust";

    // 右侧前后手风琴持续时间
    public static string KEY_CutEffectDuring_FrontBackRightPullOpen = "cuteffectduring_frontbackrightpullopen";


    // 企业 ID
    private string key;
    public string Key { set { key = value; } get { return key; } }

    // 企业的logo
    private string value;
    public string Value { set { this.value = value; } get { return value; } }
    
}





