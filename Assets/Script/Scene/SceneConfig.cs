using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
/// <summary>
///     场景配置信息
/// </summary>
/// 


namespace MagicWall
{
    [Serializable]
    public class SceneConfig
    {
        [SerializeField]
        private SceneTypeEnum _sceneType;    //过场名

        [SerializeField]
        private SubCutEffectTypeEnum _subEffect; // 次效果

        [SerializeField]
        private DataTypeEnum _dataType;    //内容类型

        [SerializeField]
        private float _durTime; // 持续时间

        [SerializeField]
        private DisplayBehaviorEnum _displayBehavior;   //移动

        [SerializeField]
        private DestoryBehaviorEnum _destoryBehavior;   // 销毁

        [SerializeField]
        private DaoTypeEnum _daoTypeEnum;   // dao

        [SerializeField]
        private int _isKinect;


        public SceneTypeEnum sceneType { set { _sceneType = value; } get { return _sceneType; } }
        public SubCutEffectTypeEnum subEffect { set { _subEffect = value; } get { return _subEffect; } }
        public DataTypeEnum dataType { set { _dataType = value; } get { return _dataType; } }
        public float durtime { set { _durTime = value; } get { return _durTime; } }
        public DisplayBehaviorEnum displayBehavior { set { _displayBehavior = value; } get { return _displayBehavior; } }
        public DestoryBehaviorEnum destoryBehavior { set { _destoryBehavior = value; } get { return _destoryBehavior; } }
        public DaoTypeEnum daoTypeEnum { set { _daoTypeEnum = value; } get { return _daoTypeEnum; } }
        public int isKinect { set { _isKinect = value; } get { return _isKinect; } }

        public SceneConfig() { }

        public SceneConfig(SceneTypeEnum sceneTypeEnum, DataTypeEnum dataType, 
            DisplayBehaviorEnum displayBehavior, DestoryBehaviorEnum destoryBehavior,
            SubCutEffectTypeEnum subEffect,
            DaoTypeEnum daoTypeEnum,float durTime)
        {
            _sceneType = sceneTypeEnum;
            _subEffect = subEffect;
            _dataType = dataType;
            _durTime = durTime;
            _displayBehavior = displayBehavior;
            _destoryBehavior = destoryBehavior;
            _daoTypeEnum = daoTypeEnum;
            isKinect = 0;
        }


        public override string ToString()
        {
            string str = "";

            str += " Scene Type : " + _sceneType;

            str += " | Data Type : " + _dataType;

            str += " | _durTime : " + _durTime;

            str += " | display Behavior : " + displayBehavior;

            str += " | destory Behavior : " + _destoryBehavior;

            return str;

        }

    }
}