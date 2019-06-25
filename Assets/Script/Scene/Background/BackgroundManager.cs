using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class BackgroundManager : MonoBehaviour
{

    // 气泡预制体
    [SerializeField] ClearBubbleAgent _clearBubbleAgentPrefab;
    // 气泡预制体2 
    [SerializeField] DimBubbleAgent _dimBubbleAgentPrefab;

    /// <summary>
    ///     清晰的球移动速度
    /// </summary>
    [SerializeField, Range(0.1f, 50f)] float _ClearBubbleMoveFactor;
    public float ClearBubbleMoveFactor { get { return _ClearBubbleMoveFactor; } }


    /// <summary>
    ///     模糊的球移动速度
    /// </summary>
    [SerializeField, Range(0.1f, 50f)] float _DimBubbleMoveFactor;
    public float DimBubbleMoveFactor { get { return _DimBubbleMoveFactor; } }


    /// <summary>
    /// 对象池 : 清晰的球
    /// </summary>
    BubblePool<ClearBubbleAgent> _clearBubbleAgentPool;
    public BubblePool<ClearBubbleAgent> ClearBubbleAgentPool { get { return _clearBubbleAgentPool; } }


    /// <summary>
    /// 对象池 : 模糊的球
    /// </summary>
    BubblePool<DimBubbleAgent> _dimBubbleAgentPool;
    public BubblePool<DimBubbleAgent> DimBubbleAgentPool { get { return _dimBubbleAgentPool; } }


    MagicWallManager _manager;
      

    [SerializeField] private RectTransform _bubblesBackground;

    private float last_create_clear_time = 0f;

    private float last_create_dim_time = 0f;


    private bool hasInit = false;

    //
    // Awake instead of Constructor
    //
    private void Awake()
    {
        
    }

    public void Init(MagicWallManager manager) {
        _manager = manager;

        _clearBubbleAgentPool = BubblePool<ClearBubbleAgent>.GetInstance(_manager.managerConfig.ClearBubblePoolSize);

        _dimBubbleAgentPool = BubblePool<DimBubbleAgent>.GetInstance(_manager.managerConfig.DimBubblePoolSize);

        last_create_clear_time = 0.0f;
        last_create_dim_time = 0.0f;

        //  初始化对象池
        PrepareData();

        hasInit = true;
    }

    /// <summary>
    /// 气球池初始化
    /// </summary>
    public void PrepareData() {
        _clearBubbleAgentPool.Init(_clearBubbleAgentPrefab, _bubblesBackground);
        _dimBubbleAgentPool.Init(_dimBubbleAgentPrefab, _bubblesBackground);
    }



    //
    //  Constructor
    //
    protected BackgroundManager(){}


    public void run() {

        if (!hasInit)
            return;

        if ((Time.time - last_create_clear_time) > _manager.managerConfig.ClearBubbbleCreateIntervalTime) {
            CreateClearBubble();
            last_create_clear_time = Time.time;
        }

        if ((Time.time - last_create_dim_time) > _manager.managerConfig.DimBubbbleCreateIntervalTime)
        {
            CreateDimBubble();
            last_create_dim_time = Time.time;
        }

    }

    ////创建气泡
    //void CreateBubble()
    //{
    //    BubbleAgent bubble;
    //    float random = Random.Range(0, 2);
    //    int w = 0;
    //    float alpha = 1;
    //    float duration = _backgroundUpDuration;
    //    BubbleType bubbleType;
    //    if (random == 0)
    //    {
    //        //实球
    //        bubble = _clearBubbleAgentPool.GetObj();
    //        w = Random.Range(400, 80);
    //        alpha = w / (400f+100f);
    //        //print(w);
    //        duration -= alpha * 5;
    //        bubbleType = BubbleType.Clear;
    //    }
    //    else
    //    {
    //        //虚球
    //        bubble = _dimBubbleAgentPool.GetObj();
    //        int r = Random.Range(0, 2);
    //        w = (r == 0) ? 200 : 80;
    //        duration += 10f;
    //        alpha = r == 0 ? 0.8f : 0.4f;
    //        bubbleType = BubbleType.Dim;
    //    }
    //    RectTransform rectTransform = bubble.GetComponent<RectTransform>();
    //    rectTransform.sizeDelta = new Vector2(w, w);

    //    float x = Random.Range(-100, (int)_manager.mainPanel.rect.width);

    //    bubble.GetComponent<RawImage>().DOFade(alpha, 0);
    //    rectTransform.anchoredPosition3D = new Vector3(x, -w, 0);
    //    rectTransform.DOLocalMoveY(2000, duration)
    //        .OnComplete(() => BubbleMoveComplete(bubble, bubbleType));

    //    //bubbles.Add(bubble);
    //}

    void CreateClearBubble() {
        BubbleAgent bubble = _clearBubbleAgentPool.GetObj();
        bubble.Init(this, _manager, BubbleType.Clear);
    }

    void CreateDimBubble()
    {
        BubbleAgent bubble = _dimBubbleAgentPool.GetObj();
        bubble.Init(this, _manager, BubbleType.Dim);
    }


    //气泡上升结束后销毁
    void BubbleMoveComplete(BubbleAgent bubble,BubbleType type)
    {
        if (type == BubbleType.Clear)
        {
            _clearBubbleAgentPool.ReleaseObj(bubble as ClearBubbleAgent);
        }
        else {
            _dimBubbleAgentPool.ReleaseObj(bubble as DimBubbleAgent);
        }
    }


    public void Reset() {
        //hasInit = false;
        //for (int i = 0; i < bubbles.Count; i++) {
        //    Destroy(bubbles[i]);
        //}
        //Init(_manager);
    }


}
