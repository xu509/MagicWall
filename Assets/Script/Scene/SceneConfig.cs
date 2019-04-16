using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  场景配置
//
public class SceneConfig
{
    private CutEffect _cutEffect;   //  过场效果
    private SceneContentType _sceneContentType;     //  场景内容

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

    public CutEffect CutEffect
    {
        get
        {
            return _cutEffect;
        }

        set
        {
            _cutEffect = value;
        }
    }

}
