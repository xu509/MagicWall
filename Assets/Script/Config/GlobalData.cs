using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  全局变量
/// </summary>
public class GlobalData : MonoBehaviour
{
    private MagicWallManager _manager;

    private MWConfig _mwConfig;

    public void Init(MagicWallManager manager) {
        _manager = manager;
        var config = _manager.daoService.GetConfig();
        SetMWConfig(config);

    }

    /// <summary>
    ///  设置配置文件
    /// </summary>
    /// <param name="mwConfig"></param>
    public void SetMWConfig(MWConfig mwConfig) {
        _mwConfig = mwConfig;
    }

    /// <summary>
    ///     获取配置文件
    /// </summary>
    /// <returns></returns>
    public MWConfig GetConfig() {
        return _mwConfig;
    }


}
