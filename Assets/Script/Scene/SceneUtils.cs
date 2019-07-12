using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     场景工具类
/// </summary>
public class SceneUtils
{
    private float _gap = 58;

    private MagicWallManager _manager;

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

        float w =  (_screen_width - gap / 2) / _column - gap;
        return Mathf.RoundToInt(w);
    }
    #endregion

    #region 当行数固定时，获取固定的高度
    /// <summary>
    ///    当行数固定时，获取固定的高度
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
        //float df = _manager.displayFactor;
        float gf = _manager.managerConfig.gapFactor;
        float gap = _gap* gf;

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

    #region 根据宽度获取列数
    /// <summary>
    /// 根据宽度获取列数
    /// </summary>
    /// <param name="ItemWidth"></param>
    /// <returns></returns>
    public int GetColumnNumberByFixedWidth(int ItemWidth) {
        float columnf = _screen_width / (ItemWidth + GetGap());
        return Mathf.RoundToInt(columnf);
    }
    #endregion

    #region 根据宽度获取列数
    /// <summary>
    ///     获得固定高度状态下，不规律的item的位置
    /// </summary>
    /// <param name="previousItem">上一个item的信息</param>
    /// <param name="fixedHeight">固定的高度</param>
    /// <param name="itemWidth">当前item的宽度</param>
    /// <returns></returns>
    public Vector2 GetPositionOfIrregularItemByFixedHeight(ItemPositionInfoBean currentItem,int fixedHeight,int itemWidth,int row)
    {
        int gap = GetGap();
        int x;
 
        x = currentItem.xposition + gap / 2 + itemWidth / 2;
        
        int y = row * (fixedHeight + gap) + fixedHeight / 2 + gap;

        return new Vector2(x, y);

    }
    #endregion

    #region 根据固定的 item 高度,行数索引，获取纵坐标
    /// <summary>
    ///     根据固定的 item 高度,行数索引，获取纵坐标
    /// </summary>
    /// <param name="itemHeight"></param>
    /// <param name="row"></param>
    /// <returns></returns>
    public float GetYPositionByFixedHeight(int itemHeight,int row) {
        int gap = GetGap();
        float y = row * (itemHeight + gap) + itemHeight / 2 + gap;
        return y;
    }
    #endregion

    #region 根据固定的 item 宽度,列数索引，获取横坐标
    /// <summary>
    ///     根据固定的 item 宽度,列数索引，获取横坐标
    /// </summary>
    /// <param name="itemHeight"></param>
    /// <param name="row"></param>
    /// <returns></returns>
    public float GetXPositionByFixedWidth(int itemWidth, int column)
    {
        int gap = GetGap();
        float x = column * (itemWidth + gap) + itemWidth / 2 + gap;
        return x;
    }
    #endregion


    /// <summary>
    ///     随机设置图片大小，用于前后分层效果与星空效果
    /// </summary>
    /// <param name="size"></param>
    /// <param name="displayFactor"></param>
    /// <returns></returns>
    public Vector2 ResetTexture(Vector2 originVector2)
    {
        //图片宽高
        float w = originVector2.x;
        float h = originVector2.y;
        //组件宽高
        float width;
        float height;

        if (w >= h)
        {
            float minW = _manager.managerConfig.ItemSizeMinWidthFactor * _manager.GetScreenRect().x;
            float maxW = _manager.managerConfig.ItemSizeMaxWidthFactor * _manager.GetScreenRect().x;

            //宽固定
            width = Random.Range(minW, maxW);
            height = h / w * width;
        }
        else
        {
            float minH = _manager.managerConfig.ItemSizeMinHeightFactor * _manager.GetScreenRect().y;
            float maxH = _manager.managerConfig.ItemSizeMaxHeightFactor * _manager.GetScreenRect().y;

            //高固定
            height = Random.Range(minH, maxH);
            width = w / h * height;
        }
        width = (int)width;
        height = (int)height;
        return new Vector2(width, height);
    }
}
