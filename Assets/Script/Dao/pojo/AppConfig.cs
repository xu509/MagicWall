using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  配置表映射
//
namespace MagicWall
{
    public class AppConfig
    {
        public static string KEY_THEME_ID = "theme_id";
        public static string KEY_SHOW_CONFIG = "show_config";

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

        //  前后分层效果持续时间
        public static string KEY_CutEffectDuring_FrontBackUnfold = "cuteffectduring_frontbackrightpullopen";



        // 企业 ID
        private string key;
        public string Key { set { key = value; } get { return key; } }

        // 企业的logo
        private string value;
        public string Value { set { this.value = value; } get { return value; } }

        public AppConfig GetConfigByMWConfig(MWConfig mwConfig, string key)
        {
            AppConfig appConfig = new AppConfig();

            if (key == KEY_THEME_ID)
            {
                appConfig.key = key;
                appConfig.value = mwConfig.ThemeId.ToString();
            }

            if (key == KEY_SHOW_CONFIG)
            {
                appConfig.key = key;
                appConfig.value = mwConfig.ShowConfig.ToString();
            }

            if (key == KEY_CutEffectDuring_CurveStagger)
            {
                appConfig.key = key;
                appConfig.value = mwConfig.CutEffectDuringCurvestagger.ToString();
            }

            if (key == KEY_CutEffectDuring_LeftRightAdjust)
            {
                appConfig.key = key;
                appConfig.value = mwConfig.CutEffectDuringLeftRightAdjust.ToString();
            }

            if (key == KEY_CutEffectDuring_MidDisperseAdjust)
            {
                appConfig.key = key;
                appConfig.value = mwConfig.CutEffectDuringMidDisperse.ToString();
            }

            if (key == KEY_CutEffectDuring_Stars)
            {
                appConfig.key = key;
                appConfig.value = mwConfig.CutEffectDuringStars.ToString();
            }

            if (key == KEY_CutEffectDuring_UpDownAdjust)
            {
                appConfig.key = key;
                appConfig.value = mwConfig.CutEffectDuringUpDownAdjust.ToString();
            }

            if (key == KEY_CutEffectDuring_FrontBackUnfold)
            {
                appConfig.key = key;
                appConfig.value = mwConfig.CutEffectDuringFrontBackRightPullOpen.ToString();
            }

            return appConfig;
        }


    }





}