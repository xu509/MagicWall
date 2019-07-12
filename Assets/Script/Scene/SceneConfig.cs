using System.Collections;
using System.Collections.Generic;
/// <summary>
///     场景配置信息
/// </summary>
public class SceneConfig
{
    private SceneTypeEnum _sceneType;    //过场名
    private DataType _dataType;    //内容类型
    private float _durtdime; // 持续时间


    public SceneTypeEnum sceneType { set { _sceneType = value;  } get { return _sceneType;  } }
    public DataType dataType { set { _dataType = value;  } get { return _dataType;  } }
    public float durtime { set { _durtdime = value;  } get { return _durtdime;  } }
}
