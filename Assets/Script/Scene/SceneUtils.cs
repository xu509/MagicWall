using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     场景工具类
/// </summary>
public class SceneUtils
{
    private float _gap = 58;


    MagicWallManager _manager;

    /// <summary>
    ///     固定的行数
    /// </summary>
    int _row;    

    /// <summary>
    ///     固定的列数
    /// </summary>
    int _column;

    /// <summary>
    ///     屏幕的宽度
    /// </summary>
    float _screen_width;

    /// <summary>
    ///     屏幕的高度
    /// </summary>
    float _screen_height;

    public SceneUtils(MagicWallManager manager) {
        _manager = manager;
        _row = manager.Row;
        _column = manager.managerConfig.Column;
        _screen_width = manager.mainPanel.rect.width;
        _screen_height = manager.mainPanel.rect.height;
    }

    /// <summary>
    ///    当列数固定时，获取固定的宽度
    /// </summary>
    /// <returns></returns>
    public int GetFixedItemWidth() {
        int gap = GetGap();

        Debug.Log("_screen_width : " + _screen_width + "Gap:" + gap);


        float w =  (_screen_width - gap / 2) / _column - gap;
        return Mathf.RoundToInt(w);
    }

    /// <summary>
    ///    当列数固定时，获取固定的宽度
    /// </summary>
    /// <returns></returns>
    public int GetFixedItemHeight()
    {
        int gap = GetGap();
        float h = (_screen_height - gap) / _row - gap;
        return Mathf.RoundToInt(h);
    }

    /// <summary>
    ///     获取间隙
    /// </summary>
    /// <returns></returns>
    public int GetGap()
    {
        float df = _manager.displayFactor;
        float gf = _manager.GapFactor;
        float gap = _gap* gf *df;

        return Mathf.RoundToInt(gap);
    }


    /// <summary>
    ///     获取正方形 item 的坐标
    /// </summary>
    /// <param name="length"></param>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    public Vector2 GetPositionOfSquareItem(int length, int row, int column) {
        int gap = GetGap();

        float x = column * (length + gap) + length / 2 + gap;
        float y = row * (length + gap) + length / 2 + gap;

        return new Vector2(x, y);

    }



}
