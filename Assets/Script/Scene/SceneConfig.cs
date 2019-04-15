using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  场景配置
//
public class SceneConfig
{
    private string _durtime;    //持续时间
    private CutEffectDisplayBehavior _cutEffectDisplayBehavior;     //表现形式
    private SceneContentType _sceneContentType;     // 场景内容

    public string Durtime
    {
        get
        {
            return _durtime;
        }

        set
        {
            _durtime = value;
        }
    }

    public SceneContentType SceneContentType
    {
        get
        {
            return _sceneContentType;
        }

        set
        {
            _sceneContentType = value;
        }
    }

    internal CutEffectDisplayBehavior CutEffectDisplayBehavior
    {
        get
        {
            return _cutEffectDisplayBehavior;
        }

        set
        {
            _cutEffectDisplayBehavior = value;
        }
    }
}
