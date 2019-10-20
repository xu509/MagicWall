using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//
//	Display 配置
//
namespace MagicWall
{
    public class DisplayBehaviorConfig
    {
        private MagicWallManager _manager;
        private int _row;
        private int _column;    // 最右侧的列数
        private int _columnInBack;    // 最右侧的列数
        private float _itemWidth;
        private float _itemHeight;
        private int _generatePositionX;
        private int _generatePositionXInBack;
        private DataTypeEnum _dataType;
        private int _page = 0;
        private float _displayTime;
        private SceneUtils _sceneUtils;

        /// <summary>
        /// 根据行数的数据字典
        /// </summary>
        private Dictionary<int, ItemPositionInfoBean> _rowAgentsDic = new Dictionary<int, ItemPositionInfoBean>();
        /// <summary>
        /// 根据行数的数据字典
        /// </summary>
        public Dictionary<int, ItemPositionInfoBean> rowAgentsDic { set { _rowAgentsDic = value; } get { return _rowAgentsDic; } }

        /// <summary>
        /// 根据列数的数据字典
        /// </summary>
        private Dictionary<int, ItemPositionInfoBean> _columnAgentsDic = new Dictionary<int, ItemPositionInfoBean>();
        /// <summary>
        /// 根据列数的数据字典
        /// </summary>
        public Dictionary<int, ItemPositionInfoBean> columnAgentsDic { set { _columnAgentsDic = value; } get { return _columnAgentsDic; } }

        /// <summary>
        /// 如果 agent 是从左至右生成的，则 generatePositionX 代表右侧的目标坐标
        /// </summary>
        public int generatePositionX { set { _generatePositionX = value; } get { return _generatePositionX; } }

        /// <summary>
        /// 如果 agent 是从左至右生成的，则 generatePositionX 代表右侧的目标坐标, 后层的模式
        /// </summary>
        public int generatePositionXInBack { set { _generatePositionXInBack = value; } get { return _generatePositionXInBack; } }


        public MagicWallManager Manager
        {
            set { _manager = value; }
            get { return _manager; }
        }

        public int Row
        {
            set { _row = value; }
            get { return _row; }
        }

        /// <summary>
        /// 最右侧的列数
        /// </summary>
        public int Column
        {
            set { _column = value; }
            get { return _column; }
        }

        public int ColumnInBack
        {
            set { _columnInBack = value; }
            get { return _columnInBack; }
        }

        public float ItemWidth
        {
            set { _itemWidth = value; }
            get { return _itemWidth; }
        }

        public float ItemHeight
        {
            set { _itemHeight = value; }
            get { return _itemHeight; }
        }

        public DataTypeEnum dataType
        {
            set { _dataType = value; }
            get { return _dataType; }
        }

        public int Page
        {
            set { _page = value; }
            get { return _page; }
        }

        public float DisplayTime
        {
            set { _displayTime = value; }
            get { return _displayTime; }
        }

        public SceneUtils sceneUtils
        {
            set { _sceneUtils = value; }
            get { return _sceneUtils; }
        }




    }
}