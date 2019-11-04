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
    public RectTransform leftPanel;
    public RectTransform rightPannel;
    public RawImage rawImagePrefab;
    public RawImage videoPlayerHolder;

    private List<string> leftImages;
    private List<string> rightImages;
    private List<string> videos;

    private DaoTypeEnum _daoTypeEnum;

    // 所有图片
    List<RawImage> images;

    [SerializeField, Range(1f, 20f)] public float _leftChangeTime = 10f;
    [SerializeField, Range(1f, 20f)] public float _rightChangeTime = 10f;
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
        leftImages = daoService.GetLeftImagesForVBI6S();
        SetLeftImages();

        // 初始化右侧图片
        rightImages = daoService.GetRigetImagesForVBI6S();
        SetRightImages();

        if (leftImages.Count > 1)
        {
            InvokeRepeating("ChangeLeftImage", _leftChangeTime, _leftChangeTime + _fadeoutDuration);
        }
        if (rightImages.Count > 1)
        {
            InvokeRepeating("ChangeRightImage", _rightChangeTime, _rightChangeTime + _fadeoutDuration);
        }
    }

    public void StopPlay()
    {
        videoPlayer.Stop();
        Destroy(GetComponentInChildren<VideoPlayer>());
        gameObject.SetActive(false);

        CancelInvoke("ChangeLeftImage");
        CancelInvoke("ChangeRightImage");


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

    void ChangeLeftImage()
    {
        RawImage[] rawImages = leftPanel.GetComponentsInChildren<RawImage>();
        RawImage rawImage = rawImages[rawImages.Length - 1];

        rawImage.DOFade(0, _fadeoutDuration).OnComplete(() =>
        {
            GameObject.Destroy(rawImage.gameObject);
            this.images.Remove(rawImage);

            RawImage[] images = leftPanel.GetComponentsInChildren<RawImage>();
            if (images.Length == 2)
            {
                SetLeftImages();
            }
        });
    }

    void ChangeRightImage()
    {
        RawImage[] rawImages = rightPannel.GetComponentsInChildren<RawImage>();
        RawImage rawImage = rawImages[rawImages.Length - 1];

        rawImage.DOFade(0, _fadeoutDuration).OnComplete(() =>
        {
            GameObject.Destroy(rawImage.gameObject);
            this.images.Remove(rawImage);

            RawImage[] images = rightPannel.GetComponentsInChildren<RawImage>();
            if (images.Length == 2)
            {
                SetRightImages();
            }
        });
    }

    void SetLeftImages()
    {
        for (int i = 0; i < leftImages.Count; i++)
        {
            RawImage rawImage = GameObject.Instantiate(rawImagePrefab, leftPanel) as RawImage;
            RectTransform rtf = rawImage.GetComponent<RectTransform>();
            rtf.anchoredPosition = Vector2.zero;
            rtf.localScale = new Vector3(1, 1, 1);
            rtf.SetAsFirstSibling();
            rawImage.texture = TextureResource.Instance.GetTexture(MagicWallManager.FileDir + leftImages[i]);
            images.Add(rawImage);
        }
    }

    void SetRightImages()
    {
        for (int i = 0; i < rightImages.Count; i++)
        {
            RawImage rawImage = GameObject.Instantiate(rawImagePrefab, rightPannel) as RawImage;
            RectTransform rtf = rawImage.GetComponent<RectTransform>();
            rtf.anchoredPosition = Vector2.zero;
            rtf.localScale = new Vector3(1, 1, 1);
            rtf.SetAsFirstSibling();
            rawImage.texture = TextureResource.Instance.GetTexture(MagicWallManager.FileDir + rightImages[i]);
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
