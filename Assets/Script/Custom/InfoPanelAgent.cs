using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InfoPanelAgent : MonoBehaviour
{
    [SerializeField] RectTransform leftPanel;
    [SerializeField] RectTransform middlePanel;
    [SerializeField] RectTransform rightPanel;
    public RawImage rawImagePrefab;


    [SerializeField, Range(1f, 20f)] float _leftChangeTime = 10f;
    [SerializeField, Range(1f, 20f)] float _middleChangeTime = 5f;
    [SerializeField, Range(1f, 20f)] float _rightChangeTime = 5f;

    [SerializeField, Range(0f, 3f)] float _fadeoutDuration = 0.5f;//淡出时间


    List<string> _leftImages,_middleImages,_rightImages;

    private float _panelWidth;
    private float _panelHeight;



    // Start is called before the first frame update
    void Start()
    {
        // 初始化最左侧图片
        _leftImages = DaoService.Instance.GetCustomImage(DaoService.CustomImageType.LEFT1);

        //AdjustLayout();

        SetLeftImages();

        // 初始化中间图片
        _middleImages = DaoService.Instance.GetCustomImage(DaoService.CustomImageType.LEFT2);
        SetMiddleImages();

        // 初始化右侧图片
        _rightImages = DaoService.Instance.GetCustomImage(DaoService.CustomImageType.RIGHT);
        if(_rightImages.Count > 0)
            SetRightImages();

        if (_leftImages.Count > 1)
        {
            InvokeRepeating("ChangeLeftImage", _leftChangeTime, _leftChangeTime + _fadeoutDuration);
        }
        if (_middleImages.Count > 1)
        {
            InvokeRepeating("ChangeMiddleImage", _middleChangeTime, _middleChangeTime + _fadeoutDuration);
        }
        if (_rightImages.Count > 1)
        {
            InvokeRepeating("ChangeRightImage", _rightChangeTime, _rightChangeTime + _fadeoutDuration);
        }
    }

    void ChangeLeftImage()
    {
        RawImage[] rawImages = leftPanel.GetComponentsInChildren<RawImage>();
        RawImage rawImage = rawImages[rawImages.Length - 1];

        rawImage.DOFade(0, _fadeoutDuration).OnComplete(() => {
            Destroy(rawImage.gameObject);
            RawImage[] images = leftPanel.GetComponentsInChildren<RawImage>();
            if (images.Length == 2)
            {
                SetLeftImages();
            }
        });    
    }

    void ChangeMiddleImage()
    {
        RawImage[] rawImages = middlePanel.GetComponentsInChildren<RawImage>();
        RawImage rawImage = rawImages[rawImages.Length - 1];

        rawImage.DOFade(0, _fadeoutDuration).OnComplete(() => {
            Destroy(rawImage.gameObject);
            RawImage[] images = middlePanel.GetComponentsInChildren<RawImage>();
            if (images.Length == 2)
            {
                SetMiddleImages();
            }
        });
    }

    void ChangeRightImage()
    {
        RawImage[] rawImages = rightPanel.GetComponentsInChildren<RawImage>();
        RawImage rawImage = rawImages[rawImages.Length - 1];

        rawImage.DOFade(0, _fadeoutDuration).OnComplete(() => {
            Destroy(rawImage.gameObject);
            RawImage[] images = rightPanel.GetComponentsInChildren<RawImage>();
            Debug.Log(images.Length);
            if (images.Length == 2)
            {
                SetRightImages();
            }
        });
    }

    void SetLeftImages()
    {
        for (int i = 0; i < _leftImages.Count; i++)
        {
            RawImage rawImage = Instantiate(rawImagePrefab, leftPanel) as RawImage;
            RectTransform rtf = rawImage.GetComponent<RectTransform>();
            rtf.anchoredPosition = Vector2.zero;
            rtf.localScale = new Vector3(1, 1, 1);
            rtf.SetAsFirstSibling();
            rawImage.texture = TextureResource.Instance.GetTexture(MagicWallManager.FileDir  + _leftImages[i]);
        }
    }

    void SetMiddleImages()
    {
        for (int i = 0; i < _middleImages.Count; i++)
        {
            RawImage rawImage = Instantiate(rawImagePrefab, middlePanel) as RawImage;
            RectTransform rtf = rawImage.GetComponent<RectTransform>();
            rtf.anchoredPosition = Vector2.zero;
            rtf.localScale = new Vector3(1, 1, 1);
            rtf.SetAsFirstSibling();
            rawImage.texture = TextureResource.Instance.GetTexture(MagicWallManager.FileDir  + _middleImages[i]);
        }
    }

    void SetRightImages()
    {
        for (int i = 0; i < _rightImages.Count; i++)
        {
            RawImage rawImage = Instantiate(rawImagePrefab, rightPanel) as RawImage;
            RectTransform rtf = rawImage.GetComponent<RectTransform>();
            rtf.anchoredPosition = Vector2.zero;
            rtf.localScale = new Vector3(1, 1, 1);
            rtf.SetAsFirstSibling();
            rawImage.texture = TextureResource.Instance.GetTexture(MagicWallManager.FileDir + _rightImages[i]);
        }
    }

    public void Run() {
        //TODO 左侧的动画更换

    }

    void AdjustLayout() {
        Texture texture = TextureResource.Instance.GetTexture(MagicWallManager.FileDir  + _leftImages[0]);

        float h = texture.height;
        float w = texture.width;

        //調整最左側的礦體
        _panelHeight = leftPanel.rect.height;

        float radio = w / h;
        _panelWidth = radio * _panelHeight;

        leftPanel.sizeDelta = new Vector2(_panelWidth, _panelHeight);
        middlePanel.sizeDelta = new Vector2(_panelWidth, _panelHeight);
        rightPanel.sizeDelta = new Vector2(_panelWidth, _panelHeight);

        Debug.Log("_panelWidth : " + _panelWidth + " |_panelHeight " + _panelHeight);



    }


}
