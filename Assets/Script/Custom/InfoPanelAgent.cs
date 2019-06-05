using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InfoPanelAgent : MonoBehaviour
{
    [SerializeField] RawImage _leftPanelImage;
    [SerializeField] RawImage _middlePanelImage;
    [SerializeField] RawImage _rightPanelImage;

    [SerializeField, Range(1f, 20f)] float _leftChangeTime = 10f;
    [SerializeField, Range(1f, 20f)] float _middleChangeTime = 5f;
    [SerializeField, Range(1f, 20f)] float _rightChangeTime = 5f;

    [SerializeField, Range(1f, 20f)] float _fadeoutDuration = 0.5f;//淡出时间
    [SerializeField, Range(1f, 20f)] float _fadeinDuration = 0.5f;//淡入时间


    List<string> _leftImages,_middleImages,_rightImages;
    int _left_index = 0;
    int _middle_index = 0;
    int _right_index = 0;


    // Start is called before the first frame update
    void Start()
    {
        // 初始化最左侧图片
        _leftImages = DaoService.Instance.GetCustomImage(DaoService.CustomImageType.LEFT1);
        SetLeftImage();

        // 初始化中间图片
        _middleImages = DaoService.Instance.GetCustomImage(DaoService.CustomImageType.LEFT2);
        SetMiddleImage();

        // 初始化右侧图片
        _rightImages = DaoService.Instance.GetCustomImage(DaoService.CustomImageType.RIGHT);
        SetRightImage();

        if (_leftImages.Count > 1)
        {
            InvokeRepeating("ChangeLeftImage", _leftChangeTime, _leftChangeTime + _fadeoutDuration + _fadeinDuration);
        }
        if (_middleImages.Count > 1)
        {
            InvokeRepeating("ChangeMiddleImage", _middleChangeTime, _middleChangeTime + _fadeoutDuration + _fadeinDuration);
        }
        if (_rightImages.Count > 1)
        {
            InvokeRepeating("ChangeRightImage", _rightChangeTime, _rightChangeTime + _fadeoutDuration + _fadeinDuration);
        }
    }

    void ChangeLeftImage()
    {
        _leftPanelImage.DOFade(0.2f, _fadeoutDuration).OnComplete(() => {
            _left_index++;
            _left_index = _left_index % _leftImages.Count;
            SetLeftImage();
            _leftPanelImage.DOFade(1, _fadeinDuration);
        });

    }

    void ChangeMiddleImage()
    {
        _middlePanelImage.DOFade(0.2f, _fadeoutDuration).OnComplete(() => {
            _middle_index++;
            _middle_index = _middle_index % _middleImages.Count;
            SetMiddleImage();
            _middlePanelImage.DOFade(1, _fadeinDuration);
        });
    }

    void ChangeRightImage()
    {
        _rightPanelImage.DOFade(0.2f, _fadeoutDuration).OnComplete(() => {
            _right_index++;
            _right_index = _right_index % _rightImages.Count;
            SetRightImage();
            _rightPanelImage.DOFade(1, _fadeinDuration);
        });
    }

    void SetLeftImage()
    {
        _leftPanelImage.texture = TextureResource.Instance.GetTexture(MagicWallManager.URL_ASSET + "custom\\" + _leftImages[_left_index]);
    }

    void SetMiddleImage()
    {
        _middlePanelImage.texture = TextureResource.Instance.GetTexture(MagicWallManager.URL_ASSET + "custom\\" + _middleImages[_middle_index]);
    }

    void SetRightImage()
    {
        _rightPanelImage.texture = TextureResource.Instance.GetTexture(MagicWallManager.URL_ASSET + "custom\\" + _rightImages[_right_index]);
    }

    public void Run() {
        //TODO 左侧的动画更换

    }

}
