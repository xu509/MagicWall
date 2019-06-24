using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 手写板功能相关配置
/// </summary>
public class WritePanelConfig : ScriptableObject
{

    [Range(50f, 1000f), Header("文字生成图片后，总尺寸宽度（像素）")]
    public int writePanelTotalRectWidth;

    [Range(50f, 1000f), Header("文字生成图片后，总尺寸宽度（像素）")]
    public int writePanelTotalRectHeight;

    [Range(50f, 1000f), Header("文字生成图片后，文字尺寸宽度（像素）")]
    public int writePanelWordRectWidth;

    [Range(50f, 1000f), Header("文字生成图片后，文字尺寸宽度（像素）")]
    public int writePanelWordRectHeight;

    [Header("是否启用毛刺特效")]
    public bool enableBurrEffect
;
    [Range(1f, 10f), Header("延迟识别的时间")]
    public float recognizeIntervalTime;

}


