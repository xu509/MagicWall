using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SearchResultScrollBarItemAgent : MonoBehaviour
{

    private int _index;
    private float _width;   // 图片宽度
    private float _default_width;   // 默认宽度


    [SerializeField] private Image _image;  //  图片

    #region 引用
    public int Index { get { return _index; } set { _index = value; } }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(int index,float _initItemWidth) {
        _index = index;
        //_total = total;
        _default_width = _initItemWidth;
        SetImageWidth(0);
    }


    public void Refresh(float widthOffset) {

        if ((widthOffset + _default_width) != _width) {
            SetImageWidth(widthOffset);
        }

    }

    private void SetImageWidth(float widthOffset) {

        // 只有一半进行变化
        //_image.GetComponent<RectTransform>().sizeDelta = new Vector2(width, _image.GetComponent<RectTransform>().sizeDelta.y);
        _width = widthOffset + _default_width;

        _image.GetComponent<RectTransform>()
            .DOSizeDelta(new Vector2(_width , _image.GetComponent<RectTransform>().sizeDelta.y), 0.2f)
            .OnUpdate(() => {
                // 根据宽度进行调整
                float anchorx;
                anchorx = (_image.GetComponent<RectTransform>().sizeDelta.x - _default_width) / 2;
                anchorx = 0 - anchorx;
                _image.GetComponent<RectTransform>().anchoredPosition = new Vector2(anchorx, _image.GetComponent<RectTransform>().anchoredPosition.y);
            });

    }

}
