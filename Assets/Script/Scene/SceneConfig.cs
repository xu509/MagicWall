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
        private DataTypeEnum _dataType;    //内容类型

        [SerializeField]
        private float _durTime; // 持续时间


        public SceneTypeEnum sceneType { set { _sceneType = value; } get { return _sceneType; } }
        public DataTypeEnum dataType { set { _dataType = value; } get { return _dataType; } }
        public float durtime { set { _durTime = value; } get { return _durTime; } }


        public SceneConfig() { }

        public SceneConfig(SceneTypeEnum sceneTypeEnum, DataTypeEnum dataType, float durTime)
        {
            _sceneType = sceneTypeEnum;
            _dataType = dataType;
            _durTime = durTime;
        }


        public override string ToString()
        {
            string str = "";

            str += " Scene Type : " + _sceneType;

            str += " | Data Type : " + _dataType;

            str += " | _durTime : " + _durTime;

            return str;

        }

    }
}