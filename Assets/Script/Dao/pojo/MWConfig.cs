using LitJson;
using System.Collections;
using System.Collections.Generic;

/// <summary>
///     魔墙配置文件
/// </summary>
public class MWConfig
{
    /// <summary>
    /// 显示类型是1(通用型)还是2(定制型)，1通用型即显示logo墙，2定制型即只显示企业单页
    /// </summary>
    int _showType;

    public static int ShowType_Common = 1;
    public static int ShowType_Custom = 2;

    /// <summary>
    /// 自定义背景图片，可选，如无数据则显示软件内置图片，需后台维护
    /// </summary>
    int _imageBackground;

    /// <summary>
    /// 0 显示动画 /1 不显示动画，需后台维护
    /// </summary>
    int _showAnimation;

    /// <summary>
    /// 曲线穿插动画 显示时间
    /// </summary>
    int _cutEffectDuringCurvestagger;

    /// <summary>
    /// 左右校准动画 显示时间
    /// </summary>
    int _cutEffectDuringLeftRightAdjust;

    /// <summary>
    /// 中间散开动画 显示时间
    /// </summary>
    int _cutEffectDuringMidDisperse;

    /// <summary>
    /// 星空动画 显示时间
    /// </summary>
    int _cutEffectDuringStars;

    /// <summary>
    /// 上下校准动画 显示时间
    /// </summary>
    int _cutEffectDuringUpDownAdjust;

    /// <summary>
    /// 分层右侧拉开动画 显示时间
    /// </summary>
    int _cutEffectDuringFrontBackRightPullOpen;

    /// <summary>
    /// 显示顺序的json配置数组，详细如下（需按show_type进行区分）
    /// </summary>
    string _showConfig;

    /// <summary>
    /// 当前启用主题id为1
    /// </summary>
    int _themeId;

    public int ShowType { get => _showType; set => _showType = value; }
    public int ImageBackground { get => _imageBackground; set => _imageBackground = value; }
    public int ShowAnimation { get => _showAnimation; set => _showAnimation = value; }
    public int CutEffectDuringCurvestagger { get => _cutEffectDuringCurvestagger; set => _cutEffectDuringCurvestagger = value; }
    public int CutEffectDuringLeftRightAdjust { get => _cutEffectDuringLeftRightAdjust; set => _cutEffectDuringLeftRightAdjust = value; }
    public int CutEffectDuringMidDisperse { get => _cutEffectDuringMidDisperse; set => _cutEffectDuringMidDisperse = value; }
    public int CutEffectDuringStars { get => _cutEffectDuringStars; set => _cutEffectDuringStars = value; }
    public int CutEffectDuringUpDownAdjust { get => _cutEffectDuringUpDownAdjust; set => _cutEffectDuringUpDownAdjust = value; }
    public int CutEffectDuringFrontBackRightPullOpen { get => _cutEffectDuringFrontBackRightPullOpen; set => _cutEffectDuringFrontBackRightPullOpen = value; }
    public string ShowConfig { get => _showConfig; set => _showConfig = value; }
    public int ThemeId { get => _themeId; set => _themeId = value; }



    public override string ToString()
    {
        string str;

        str = JsonMapper.ToJson(this);

        return str;
    }

}
