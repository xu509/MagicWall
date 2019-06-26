using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 位置信息
/// </summary>
public class ItemPositionInfoBean
{
    /// <summary>
    /// 所处的行
    /// </summary>
    int _row;

    /// <summary>
    /// 行坐标
    /// </summary>
    int _xposition;

    /// <summary>
    /// 所处的列
    /// </summary>
    int _column;

    /// <summary>
    /// 列坐标
    /// </summary>
    int _yposition;

    int _height;

    int _width;

    public int row {
        set {
            _row = value;
        }
        get {
            return _row;
        }
    }

    public int xposition
    {
        set
        {
            _xposition = value;
        }
        get
        {
            return _xposition;
        }
    }

    public int column
    {
        set
        {
            _column = value;
        }
        get
        {
            return _column;
        }
    }
    public int yposition
    {
        set
        {
            _yposition = value;
        }
        get
        {
            return _yposition;
        }
    }

    public int height
    {
        set
        {
            _height = value;
        }
        get
        {
            return _height;
        }
    }
    public int width
    {
        set
        {
            _width = value;
        }
        get
        {
            return _width;
        }
    }

}
