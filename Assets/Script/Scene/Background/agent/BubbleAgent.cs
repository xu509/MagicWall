using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

/// <summary>
/// 清晰的背景球 Agent
/// </summary>
public abstract class BubbleAgent : MonoBehaviour
{
    [SerializeField] Image _image;

    protected float _scaleFactorMin = 0.6f;
    protected float _scaleFactorMax = 1f;


    protected float _scaleFactor;
    public float scaleFactor { set { _scaleFactor = value;  } get { return _scaleFactor; } }

    private bool hasInit = false;
    private BubbleType _bubbleType; //  气球类型
    public BubbleType bubbleType{ get { return _bubbleType; } }
    protected float screenHeight;

    protected MagicWallManager _manager;
    protected BackgroundManager _backgroundManager;


    public void Init(BackgroundManager backgroundManager, MagicWallManager manager,BubbleType bubbleType,Vector3 position)
    {
        
        _manager = manager;
        _backgroundManager = backgroundManager;
        _bubbleType = bubbleType;
        screenHeight = manager.magicWallPanel.rect.height;

        // 设置位置
        GetComponent<RectTransform>().anchoredPosition3D = position;

        // 设置图片
        SpriteAtlas spriteAtlas = Resources.Load<SpriteAtlas>("SpriteAtlas");

        if (bubbleType == BubbleType.Clear)
        {
            _image.sprite = spriteAtlas.GetSprite("background-bubble-clear");
        }
        else {
            _image.sprite = spriteAtlas.GetSprite("background-bubble-dim");

        }



        //  生成随机大小 0.6 - 1
        float s = Mathf.Lerp(_scaleFactorMin, _scaleFactorMax, Random.Range(0f, 1f));
        _scaleFactor = s;
        Vector3 scaleFactor = new Vector3(s, s, s);
        GetComponent<RectTransform>().localScale = scaleFactor;


        float a = Mathf.Lerp(0.4f, 1f, Random.Range(0f, 1f));
        UpdateAlpha(a);


        gameObject.SetActive(true);

        hasInit = true;
    }


    /// <summary>
    ///     修改透明度
    /// </summary>
    /// <param name="alpha"></param>
    void UpdateAlpha(float alpha)
    {
        //Color c = GetComponent<RawImage>().color;
        Color c = GetComponent<Image>().color;
        c.a = alpha;
        GetComponent<Image>().color = c;   
        //GetComponent<RawImage>().color = c;
    }


    public bool IsOverTop() {
        float y_max = _manager.magicWallPanel.rect.yMax;
        return GetComponent<RectTransform>().anchoredPosition.y > (y_max * 2);
    }

    /// <summary>
    /// 上升
    /// </summary>
    /// <param name="moveFactor"></param>
    public abstract void Raise(float moveFactor);


    /// <summary>
    /// 获取移动因素
    /// </summary>
    /// <returns></returns>
    public abstract float GetMoveFactor(float minFactor,float maxFactor);


}
