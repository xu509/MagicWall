using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 过场效果
public abstract class CutEffect : MonoBehaviour
{
    //
    //  Parameter
    //
    CutEffectDisplayBehavior displayBehavior; //表现
    CutEffectDestoryBehavior destoryBehavior; //销毁时间
    ItemsFactory _itemsFactory; // 工厂

    private SceneContentType _sceneContentType;
    public SceneContentType sceneContentType { set { _sceneContentType = value; } get { return _sceneContentType; } }

    private float _startTime;
    protected float StartTime { get { return _startTime; } }

    // 运行状态标志
    private bool hasDisplaying = true;
    public bool HasDisplaying { set { hasDisplaying = value; } get { return hasDisplaying; } }

    // 切换动画时长
    float startingDurTime;
    public float StartingDurTime { set { startingDurTime = value; } get { return startingDurTime; } }

    // 显示动画的时长
    float displayDurTime;
    public float DisplayDurTime { set { displayDurTime = value; } get { return displayDurTime; } }

    // 销毁动画的时长
    float destoryDurTime;
    public float DestoryDurTime { set { destoryDurTime = value; } get { return destoryDurTime; } }

    internal CutEffectDisplayBehavior DisplayBehavior { get { return displayBehavior; } set { displayBehavior = value; } }

    internal CutEffectDestoryBehavior DestoryBehavior { get { return destoryBehavior; } set { destoryBehavior = value; } }

    internal ItemsFactory ItemsFactory { get { return _itemsFactory; }}


    protected abstract void Init();

    protected abstract void CreateActivity();

    protected abstract void CreateProduct();

    protected abstract void CreateLogo();


    //
    //  Method
    //
    public void Create(SceneContentType st) {
        Init();

        sceneContentType = st;

        if (sceneContentType == SceneContentType.activity)
        {
            // TODO 修改工厂实现
            _itemsFactory = ActivityFactory.Instance;
            CreateActivity();
        }
        else if (sceneContentType == SceneContentType.env)
        {
            _itemsFactory = EnvFactory.Instance;
            CreateLogo();
        }
        else {
            // TODO 修改工厂实现
            _itemsFactory = ProductFactory.Instance;
            CreateProduct();
        }

        // 初始化完成后更新时间
        _startTime = Time.time;
    }

    public abstract void Starting();

    //	显示中
    public void Displaying() {
        if (hasDisplaying) {
            DisplayBehavior.Run();
        }      
    }

	//	销毁中
	public void Destorying() {
        destoryBehavior.Run();
    }

    public abstract void OnStartingCompleted();

}

