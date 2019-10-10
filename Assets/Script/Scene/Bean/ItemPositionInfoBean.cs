using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 位置信息
/// </summary>
namespace MagicWall
{
    public class ItemPositionInfoBean
    {
        /// <summary>
        /// 所处的行
        /// </summary>
        int _row = 0;

        /// <summary>
        /// 行坐标（最大的）
        /// </summary>
        int _xposition = 0;
        public int xposition { set { _xposition = value; } get { return _xposition; } }

        int _xpositionMin = 0;
        public int xPositionMin { set { _xpositionMin = value; } get { return _xpositionMin; } }

        /// <summary>
        /// 所处的列
        /// </summary>
        int _column = 0;

        /// <summary>
        /// 列坐标(最大的)
        /// </summary>
        int _yposition = 0;

        int _ypositionMin = 0;
        public int yPositionMin { set { _ypositionMin = value; } get { return _ypositionMin; } }


        int _height = 0;

        int _width = 0;

        public int row
        {
            set
            {
                _row = value;
            }
            get
            {
                return _row;
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
}