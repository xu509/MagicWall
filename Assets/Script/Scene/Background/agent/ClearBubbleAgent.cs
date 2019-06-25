using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 清晰的背景球 Agent
/// </summary>
public class ClearBubbleAgent : BubbleAgent
{

    void Awake()
    {
        _width = Random.Range(400, 80);
        _alpha = _width / (400f + 100f);
    }

    ///// <summary>
    ///// 初始化
    ///// </summary>
    //public void Init(float upDuringTime,MagicWallManager manager) {
    //    _upDuringTime = upDuringTime;
    //    _manager = manager;

        

    //    int w = Random.Range(400, 80);
    //    float alpha = w / (400f + 100f);

    //    _upDuringTime -= alpha * 5;

    //    GetComponent<RectTransform>().sizeDelta = new Vector2(w, w);
    //    float x = Random.Range(-100, (int)_manager.mainPanel.rect.width);

    //    SetAlpha(alpha);

    //    GetComponent<RectTransform>().anchoredPosition3D = new Vector3(x, -w, 0);

    //    hasInit = true;
    //}


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        DoBaseFixedUpdate();
    }

    protected override float SetAlpha()
    {
        float alpha = _width / (400f + 100f);
        return alpha;
    }

    protected override float SetWidth()
    {
        return _width;
    }

    protected override Vector3 SetGenPosition()
    {
        float x = Random.Range(-100, (int)_manager.mainPanel.rect.width);
        Vector3 position =  new Vector3(x, -_width, 0);
        return position;
    }
}
