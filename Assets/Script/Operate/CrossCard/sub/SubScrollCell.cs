﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;

//
//  分类选项卡
//
public class SubScrollCell : SubScrollBaseCell<CrossCardCellData, CrossCardScrollViewContext>
{
    bool _hasClickedLiked = false;   // 标识符，是否已点击喜欢

    CrossCardCellData _cellData;
    string _title;  // 标题
    int _index; //  索引

    int _likes;

    public int Index { set { _index = value; } get { return _index; } }

    [SerializeField] Animator _animator;
    [SerializeField] float _position;
    [SerializeField] RectTransform scale_tool; // 缩小icon

    [SerializeField] Button btn_like;
    [SerializeField] Button btn_like_withnumber;

    //
    //  Component Paramater 
    //

    [SerializeField] TestScript videoPrefab;
    [SerializeField] RectTransform videoContainer;

    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] RawImage videoContent;

    [SerializeField] RawImage _image;

    bool _hasLikeNumber = false;



    static class AnimatorHash
    {
        public static readonly int Scroll = Animator.StringToHash("scroll");
    }


    // Start is called before the first frame update
    void Start()
    {


        //button.onClick.AddListener(() => Context.OnCellClicked?.Invoke(Index));

    }

    public override void UpdateContent(CrossCardCellData cellData)
    {
        _cellData = cellData;
        _index = cellData.Index;
        _title = cellData.Title;

        gameObject.name = "CrossSubScroll" + cellData.Index;
            
        // TODO 更新横向的选项卡
        //IList<CrossCardCellData> datas = CardItemFactoryInstance.Instance.Generate(cellData.Id, cellData.Category);
        //crossCardScrollViewCellItem.UpdateData(datas);

        if (cellData.Category == CrossCardCategoryEnum.VIDEO)
        {
            Debug.Log("NOW:" + gameObject.activeSelf);

            videoContainer.gameObject.SetActive(true);
            _image.gameObject.SetActive(false);

            //videoPlayer.errorReceived += VideoErrorDid;
            //videoPlayer.prepareCompleted += VideoPreparedDid;

            //videoPlayer.source = VideoSource.Url;
            //videoPlayer.url = "file://E:/workspace/MagicWall/Assets/Files/env/video/1.mp4";

            //// 播放视频
            //StartCoroutine(PlayVideo());

        }
        else {
            //  设置 Image
            _image.gameObject.SetActive(true);
            videoContainer.gameObject.SetActive(false);
            _image.texture = cellData.ImageTexture;
        }

    }

    public void DoPlayVideo() {

        string url = "E:/workspace/MagicWall/Assets/Files/env/video/1.mp4";
        Context.OnPlayVideo?.Invoke(url);

    }


    public override void UpdatePosition(float position)
    {
        currentPosition = position;
        _animator.Play(AnimatorHash.Scroll, -1, position);
        _animator.speed = 0;

        _position = position;
    }

    float currentPosition = 0;

    void OnEnable() => UpdatePosition(currentPosition);

    public override void InitData()
    {

    }

    public void DoScale() {
        // 获取图片
        Context.OnScaleClicked?.Invoke(_image.texture);

    }

    public override void DoUpdateDescription()
    {
        // 获取图片
        Context.OnDescriptionChanged?.Invoke(_cellData.Description);

    }

    public override void UpdateComponentStatus()
    {
        // 调整 Scale 按钮

        if (!_cellData.IsImage)
        {
            scale_tool.gameObject.SetActive(false);
        }
        else
        {
            scale_tool.gameObject.SetActive(true);
        }

        // 调整 Like 按钮
        _likes = DaoService.Instance.GetLikes(_cellData.Id, _cellData.Category);
        string likesStr;
        bool hasLikesNumber = false;


        if (_likes > 0)
        {
            //_btnLikeAnimator.Play(AnimatorHash.CardButtonLikeNormalWithNumber);
            hasLikesNumber = true;
            _hasLikeNumber = true;
        }
        else
        {
            _hasLikeNumber = false;
        }

        if (_likes > 99)
        {
            likesStr = "99+";
        }
        else
        {
            likesStr = _likes.ToString();
        }

        if (hasLikesNumber)
        {
            // 已存在喜欢数时，开启相关的按钮，并赋值
            btn_like.gameObject.SetActive(false);
            btn_like_withnumber.gameObject.SetActive(true);
            btn_like_withnumber.GetComponentInChildren<Text>().text = likesStr;
        }
        else
        {
            btn_like_withnumber.gameObject.SetActive(false);
            btn_like.gameObject.SetActive(true);
        }


        // 将 Card 放在最前端
        GetComponent<RectTransform>().SetAsLastSibling();

    }

    public override void ClearComponentStatus()
    {
        // 清除 缩放 按钮
        if (scale_tool.gameObject.activeSelf) {
            scale_tool.gameObject.SetActive(false);
        }


        // 清除喜欢按钮
        if (btn_like.gameObject.activeSelf) {
            btn_like.gameObject.SetActive(false);
        }
        if (btn_like_withnumber.gameObject.activeSelf)
        {
            btn_like_withnumber.gameObject.SetActive(false);
        }

    }

    public void DoLike()
    {
        if (_hasClickedLiked)
        {
            return;
        }

        _hasClickedLiked = true;

        //  显示数增加
        if (!_hasLikeNumber)
        {
            btn_like.GetComponentInChildren<Text>().text = (_likes + 1).ToString();
            // 需从不显示改为显示
            btn_like.GetComponentInChildren<Text>().DOFade(1, Time.deltaTime);
        }
        else
        {
            int newLikes = _likes + 1;
            string newLikeStr = newLikes > 99 ? "99+" : newLikes.ToString();
            btn_like_withnumber.GetComponentInChildren<Text>().DOText(newLikeStr, Time.deltaTime);
        }

        // TODO 数据逻辑上进行添加数值
        _likes = _likes + 1;
        _cellData.crossCardAgent.DoUpdate();
    }

    public override CrossCardCellData GetData()
    {
        return _cellData;
    }

    IEnumerator PlayVideo()
    {
        Debug.Log("Play Video !");

        videoPlayer.Prepare();

        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while (!videoPlayer.isPrepared)
        {
            Debug.Log("Wait prepared");
            yield return waitForSeconds;
            break;
        }

        Debug.Log("videoPlayer.isPrepared !");

        if (videoPlayer.isPrepared) {
            videoContent.texture = videoPlayer.texture;
        }

        //videoPlayer.Play();
        //audioSource.Play();
    }

    private void VideoErrorDid(VideoPlayer source, string message) {
        Debug.Log("Video Error Did");
        Debug.Log("Video Error Did : " + message);
    }

    void VideoPreparedDid(VideoPlayer source)
    {
        Debug.Log("Video Prepared Did : ");

    }
}
