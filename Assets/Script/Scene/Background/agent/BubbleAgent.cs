using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 清晰的背景球 Agent
/// </summary>
public abstract class BubbleAgent : MonoBehaviour
{
    [SerializeField] Image _image;

    private bool hasInit = false;
    private BubbleType _bubbleType;
    protected float screenHeight;

    protected MagicWallManager _manager;
    protected BackgroundManager _backgroundManager;

    protected float _moveFactor;    //  移动速度
    protected float _alpha;  //  透明度
    protected float _width;  //  宽度
    protected Vector3 _genAnchorPosition;  //  出生位置


    public void Init(BackgroundManager backgroundManager, MagicWallManager manager,BubbleType bubbleType)
    {
        
        _manager = manager;
        _backgroundManager = backgroundManager;
        _bubbleType = bubbleType;
        screenHeight = manager.magicWallPanel.rect.height;

        DoSet();

        // 此时宽度 80 - 400 / 80 - 200

        // TODO 分布均匀

        GetComponent<RectTransform>().sizeDelta = new Vector2(_width, _width);

        UpdateAlpha(_alpha);
                
        GetComponent<RectTransform>().anchoredPosition3D = _genAnchorPosition;

        gameObject.SetActive(true);

        hasInit = true;
    }

    protected void DoBaseFixedUpdate()
    {
        //在 _upDuringTime 的时间内，气球从底部走到顶部

        if (IsOverTop())
        {
            if (_bubbleType == BubbleType.Clear)
            {
                _backgroundManager.ClearBubbleAgentPool.ReleaseObj(this as ClearBubbleAgent);
            }
            else {
                _backgroundManager.DimBubbleAgentPool.ReleaseObj(this as DimBubbleAgent);
            }
        }
        else {
            if (_bubbleType == BubbleType.Clear)
            {
                // 大球移动速度超过小球
                // r:0 -> width:80 ; r:1 -> width:400
                // (width - 80 ) / 320 = r;
                float factor = Mathf.Lerp(_backgroundManager.ClearBubbleMoveFactor / 3
                    , _backgroundManager.ClearBubbleMoveFactor
                    , (_width - 80) / 320);
                transform.Translate(0, Time.deltaTime * factor, 0);
            }
            else
            {
                // 80 - 200
                float factor = Mathf.Lerp(_backgroundManager.ClearBubbleMoveFactor / 3
                    , _backgroundManager.ClearBubbleMoveFactor
                    , (_width - 80) / 120);
                transform.Translate(0, Time.deltaTime * factor, 0);
            }
        }
    }

    void DoSet() { 
        _width = SetWidth();
        _alpha = SetAlpha();
        _genAnchorPosition = SetGenPosition();
    }

    /// <summary>
    ///     设置透明度
    /// </summary>
    protected abstract float SetAlpha();

    /// <summary>
    ///     设置长 / 宽
    /// </summary>
    protected abstract float SetWidth();

    /// <summary>
    ///     设置长 / 宽
    /// </summary>
    protected abstract Vector3 SetGenPosition();



    /// <summary>
    ///     修改透明度
    /// </summary>
    /// <param name="alpha"></param>
    void UpdateAlpha(float alpha)
    {
        Color c = GetComponent<Image>().color;
        c.a = alpha;
        GetComponent<Image>().color = c;
    }


    private bool IsOverTop() {

        float y_max = _manager.magicWallPanel.rect.yMax;
        return GetComponent<RectTransform>().anchoredPosition.y > (y_max * 2);
    }

}
