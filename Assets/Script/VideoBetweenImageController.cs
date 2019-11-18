using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using MagicWall;
using DG.Tweening;

public class VideoBetweenImageController : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    public GameObject videoPlayerGo;
    [Header("left从左向右，right从右向左")]
    public RectTransform leftPanel1;
    public RectTransform leftPanel2;
    public RectTransform rightPannel1;
    public RectTransform rightPannel2;
    public RawImage rawImagePrefab;
    public RawImage videoPlayerHolder;

    private List<string> leftImages1;
    private List<string> leftImages2;
    private List<string> rightImages1;
    private List<string> rightImages2;
    private List<string> videos;

    private DaoTypeEnum _daoTypeEnum;

    // 所有图片
    List<RawImage> images;

    [SerializeField, Range(1f, 20f)] public float _leftChangeTime1 = 10f;
    [SerializeField, Range(1f, 20f)] public float _leftChangeTime2 = 10f;
    [SerializeField, Range(1f, 20f)] public float _rightChangeTime1 = 5f;
    [SerializeField, Range(1f, 20f)] public float _rightChangeTime2 = 5f;
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

        //获取视频
        videos = daoService.GetVideosForVBI6S();

        videoPlayerHolder.texture = null;
        videoPlayerGo.AddComponent<VideoPlayer>();
        videoPlayer = videoPlayerGo.GetComponent<VideoPlayer>();
        videoPlayer.source = VideoSource.Url;
        videoPlayer.playOnAwake = false;
        videoPlayer.loopPointReached += LoopPointReached;
        videoPlayer.url = MagicWallManager.FileDir + videos[0];
        videoPlayer.Prepare();

        StartCoroutine(PlayVideo());

        images = new List<RawImage>();

        // 初始化最左侧图片
        leftImages1 = daoService.GetImagesForVideoPanel8Screen(VideoPanel8Type.Left1);
        if (leftImages1.Count > 0)
        {
            SetLeftImages1();
        }
        // 初始化最左侧1图片
        leftImages2 = daoService.GetImagesForVideoPanel8Screen(VideoPanel8Type.Left2);
        if (leftImages2.Count > 0)
        {
            SetLeftImages2();
        }

        // 初始化右侧图片
        rightImages1 = daoService.GetImagesForVideoPanel8Screen(VideoPanel8Type.Right1);
        if (rightImages1.Count > 0)
        {
            SetRightImages1();
        }
        // 初始化右侧图片1
        rightImages2 = daoService.GetImagesForVideoPanel8Screen(VideoPanel8Type.Right2);
        if (rightImages2.Count > 0)
        {
            SetRightImages2();
        }

        if (leftImages1.Count > 1)
        {
            InvokeRepeating("ChangeLeftImage1", _leftChangeTime1, _leftChangeTime1 + _fadeoutDuration);
        }
        if (leftImages2.Count > 1)
        {
            InvokeRepeating("ChangeLeftImage2", _leftChangeTime2 + 2, _leftChangeTime2 + _fadeoutDuration);
        }
        if (rightImages1.Count > 1)
        {
            InvokeRepeating("ChangeRightImage1", _rightChangeTime1, _rightChangeTime1 + _fadeoutDuration);
        }
        if (rightImages2.Count > 1)
        {
            InvokeRepeating("ChangeRightImage2", _rightChangeTime2 + 2, _rightChangeTime2 + _fadeoutDuration);
        }
    }

    public void StopPlay()
    {
        videoPlayer.Stop();
        Destroy(GetComponentInChildren<VideoPlayer>());
        gameObject.SetActive(false);

        CancelInvoke("ChangeLeftImage1");
        CancelInvoke("ChangeLeftImage2");
        CancelInvoke("ChangeRightImage1");
        CancelInvoke("ChangeRightImage2");


        //GetComponent<CanvasGroup>().alpha = 0;
        //CancelInvoke();

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

    void ChangeLeftImage2()
    {
        RawImage[] rawImages = leftPanel2.GetComponentsInChildren<RawImage>();
        RawImage rawImage = rawImages[rawImages.Length - 1];

        rawImage.DOFade(0, _fadeoutDuration).OnComplete(() =>
        {
            GameObject.Destroy(rawImage.gameObject);
            this.images.Remove(rawImage);

            RawImage[] images = leftPanel2.GetComponentsInChildren<RawImage>();
            if (images.Length == 2)
            {
                SetLeftImages2();
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

    void ChangeRightImage2()
    {
        RawImage[] rawImages = rightPannel2.GetComponentsInChildren<RawImage>();
        RawImage rawImage = rawImages[rawImages.Length - 1];

        rawImage.DOFade(0, _fadeoutDuration).OnComplete(() =>
        {
            GameObject.Destroy(rawImage.gameObject);
            this.images.Remove(rawImage);

            RawImage[] images = rightPannel2.GetComponentsInChildren<RawImage>();
            if (images.Length == 2)
            {
                SetRightImages2();
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

    void SetLeftImages2()
    {
        for (int i = 0; i < leftImages2.Count; i++)
        {
            RawImage rawImage = GameObject.Instantiate(rawImagePrefab, leftPanel2) as RawImage;
            RectTransform rtf = rawImage.GetComponent<RectTransform>();
            rtf.anchoredPosition = Vector2.zero;
            rtf.localScale = new Vector3(1, 1, 1);
            rtf.SetAsFirstSibling();
            rawImage.texture = TextureResource.Instance.GetTexture(MagicWallManager.FileDir + leftImages2[i]);
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

    void SetRightImages2()
    {
        for (int i = 0; i < rightImages2.Count; i++)
        {
            RawImage rawImage = GameObject.Instantiate(rawImagePrefab, rightPannel2) as RawImage;
            RectTransform rtf = rawImage.GetComponent<RectTransform>();
            rtf.anchoredPosition = Vector2.zero;
            rtf.localScale = new Vector3(1, 1, 1);
            rtf.SetAsFirstSibling();
            rawImage.texture = TextureResource.Instance.GetTexture(MagicWallManager.FileDir + rightImages2[i]);
            images.Add(rawImage);
        }
    }

    IEnumerator PlayVideo()
    {
        while (!videoPlayer.isPrepared)
        {
            yield return new WaitForSeconds(1);
            break;
        }

        Debug.Log("视频准备完毕");

        // 将texture 赋值 (必须等准备好才能赋值)
        videoPlayerHolder.texture = videoPlayer.texture;

        videoPlayer.Play();
        videoPlayer.SetDirectAudioMute(0, false);

        /*
        float screenW = Screen.width;
        float screenH = Screen.height;
        float w = videoPlayer.texture.width;
        float h = videoPlayer.texture.height;
        print("screenW:" + screenW + "screenH:" + screenH);
        print("w:" + w + "h:" + h);
        if (w / screenW >= h / screenH)
        {
            //宽铺满
            h = h / w * screenW;
            w = screenW;
        }
        else
        {
            //高铺满
            w = w / h * screenH;
            h = screenH;
        }
        print("w:" + w + "h:" + h);
        videoPlayerHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(w, h);
        */
    }

    //视频播放完成
    private void LoopPointReached(VideoPlayer source)
    {
        print("视频播放完成");
    }

}
