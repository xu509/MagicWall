using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using MagicWall;
using DG.Tweening;

public class ImageBothSideController : MonoBehaviour
{
    [Header("left从左向右，right从右向左")]
    public RectTransform leftPanel1;
    public RectTransform rightPannel1;
    public RawImage rawImagePrefab;

    private List<string> leftImages1;
    private List<string> rightImages1;

    private DaoTypeEnum _daoTypeEnum;

    // 所有图片
    List<RawImage> images;

    [SerializeField, Range(1f, 20f)] public float _leftChangeTime1 = 10f;
    [SerializeField, Range(1f, 20f)] public float _rightChangeTime1 = 5f;
    [SerializeField, Range(0f, 3f), Header("图片淡出时间")] public float _fadeoutDuration = 0.5f;

    private MagicWallManager _manager;

    public void Init(MagicWallManager manager,DaoTypeEnum daoTypeEnum)
    {
        _manager = manager;
        _daoTypeEnum = daoTypeEnum;
    }
    public void StartPlay()
    {
        gameObject.SetActive(true);

        var daoService = _manager.daoServiceFactory.GetDaoService(_daoTypeEnum);


        images = new List<RawImage>();

        // 初始化最左侧图片
        leftImages1 = daoService.GetImageForImageBothSide(VideoPanel8Type.Left1);
        if (leftImages1.Count > 0)
        {
            SetLeftImages1();
        }

        // 初始化右侧图片
        rightImages1 = daoService.GetImageForImageBothSide(VideoPanel8Type.Right1);
        if (rightImages1.Count > 0)
        {
            SetRightImages1();
        }

        if (leftImages1.Count > 1)
        {
            InvokeRepeating("ChangeLeftImage1", _leftChangeTime1, _leftChangeTime1 + _fadeoutDuration);
        }
 
        if (rightImages1.Count > 1)
        {
            InvokeRepeating("ChangeRightImage1", _rightChangeTime1, _rightChangeTime1 + _fadeoutDuration);
        }
    }

    public void StopPlay()
    {
        gameObject.SetActive(false);

        CancelInvoke("ChangeLeftImage1");
        CancelInvoke("ChangeRightImage1");

        for (int i = 0; i < images.Count; i++)
        {
            if (images[i].gameObject != null || images[i].gameObject.activeSelf)
            {
                Destroy(images[i].gameObject);
            }
        }

        images = new List<RawImage>();
    }

    void ChangeLeftImage1()
    {
        RawImage[] rawImages = leftPanel1.GetComponentsInChildren<RawImage>();
        RawImage rawImage = rawImages[rawImages.Length - 1];

        rawImage.DOFade(0, _fadeoutDuration).OnComplete(() =>
        {
            GameObject.Destroy(rawImage.gameObject);
            this.images.Remove(rawImage);

            RawImage[] images = leftPanel1.GetComponentsInChildren<RawImage>();
            if (images.Length == 2)
            {
                SetLeftImages1();
            }
        });
    }


    void ChangeRightImage1()
    {
        RawImage[] rawImages = rightPannel1.GetComponentsInChildren<RawImage>();
        RawImage rawImage = rawImages[rawImages.Length - 1];

        rawImage.DOFade(0, _fadeoutDuration).OnComplete(() =>
        {
            GameObject.Destroy(rawImage.gameObject);
            this.images.Remove(rawImage);

            RawImage[] images = rightPannel1.GetComponentsInChildren<RawImage>();
            if (images.Length == 2)
            {
                SetRightImages1();
            }
        });
    }


    void SetLeftImages1()
    {
        for (int i = 0; i < leftImages1.Count; i++)
        {
            RawImage rawImage = GameObject.Instantiate(rawImagePrefab, leftPanel1) as RawImage;
            RectTransform rtf = rawImage.GetComponent<RectTransform>();
            rtf.anchoredPosition = Vector2.zero;
            rtf.localScale = new Vector3(1, 1, 1);
            rtf.SetAsFirstSibling();
            rawImage.texture = TextureResource.Instance.GetTexture(MagicWallManager.FileDir + leftImages1[i]);
            images.Add(rawImage);
        }
    }


    void SetRightImages1()
    {
        for (int i = 0; i < rightImages1.Count; i++)
        {
            RawImage rawImage = GameObject.Instantiate(rawImagePrefab, rightPannel1) as RawImage;
            RectTransform rtf = rawImage.GetComponent<RectTransform>();
            rtf.anchoredPosition = Vector2.zero;
            rtf.localScale = new Vector3(1, 1, 1);
            rtf.SetAsFirstSibling();
            rawImage.texture = TextureResource.Instance.GetTexture(MagicWallManager.FileDir + rightImages1[i]);
            images.Add(rawImage);
        }
    }


    //视频播放完成
    private void LoopPointReached(VideoPlayer source)
    {
        print("视频播放完成");
    }

}
