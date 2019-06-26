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

    #region 当列数固定时，获取固定的宽度
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
    #endregion

    #region 当列数固定时，获取固定的宽度
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
    #endregion

    #region 获取间隙

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
    #endregion

    #region 获取正方形 item 的坐标

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
    #endregion

    /// <summary>
    /// 根据宽度获取列数
    /// </summary>
    /// <param name="ItemWidth"></param>
    /// <returns></returns>
    public int GetColumnNumberByFixedWidth(int ItemWidth) {
        float columnf = _screen_width / (ItemWidth + GetGap());
        return Mathf.RoundToInt(columnf);
    }

    /// <summary>
    ///     获得固定高度状态下，不规律的item的位置
    /// </summary>
    /// <param name="previousItem">上一个item的信息</param>
    /// <param name="fixedHeight">固定的高度</param>
    /// <param name="itemWidth">当前item的宽度</param>
    /// <returns></returns>
    public Vector2 GetPositionOfIrregularItemByFixedHeight(ItemPositionInfoBean previousItem,int fixedHeight,int itemWidth,int row)
    {
        int gap = GetGap();
        int x;
        if (previousItem == null)
        {
            // 首个
            x = 0 + gap / 2 + itemWidth / 2;
        }
        else {
            x = previousItem.xposition + gap / 2 + itemWidth / 2;
        }

        int y = row * (fixedHeight + gap) + fixedHeight / 2 + gap;
        return new Vector2(x, y);

    }

}
