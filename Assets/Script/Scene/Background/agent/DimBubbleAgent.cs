using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 模糊的背景球 Agent
/// </summary>
public class DimBubbleAgent : BubbleAgent
{
    void Awake()
    {
        int r = Random.Range(0, 2);
        _width = (r == 0) ? 200 : 80;
        //_upDuringTime += 10f;

        _alpha = r == 0 ? 0.8f : 0.4f;
    }


    //public void Init(float upDuringTime, MagicWallManager manager)
    //{
    //    _upDuringTime = upDuringTime;
    //    _manager = manager;

    //    int r = Random.Range(0, 2);
    //    int w = (r == 0) ? 200 : 80;
    //    _upDuringTime += 10f;

    //    float alpha = r == 0 ? 0.8f : 0.4f;

    //    GetComponent<RectTransform>().sizeDelta = new Vector2(w, w);

    //    SetAlpha(alpha);


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
        return _alpha;
    }


    protected override float SetWidth()
    {
        return _width;
    }

    protected override Vector3 SetGenPosition()
    {
        float x = Random.Range(-100, (int)_manager.mainPanel.rect.width);
        Vector3 position = new Vector3(x, -_width, 0);
        return position;
    }
}
