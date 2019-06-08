using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;

public class VideoAgent : MonoBehaviour
{
    [SerializeField] VideoPlayer _videoPlayer;
    [SerializeField] RawImage _screen;
    [SerializeField] Text _time;
    [SerializeField] RectTransform _progress;

    [SerializeField] Text _text_description;
    [SerializeField] RectTransform _btn_play;
    [SerializeField] RectTransform _btn_pause;
    [SerializeField] RectTransform _btn_music_enable;
    [SerializeField] RectTransform _btn_muisc_disable;


    private CardAgent _cardAgent;

    private string _address;
    private string _description;
    private bool _isPlaying = false;
    private bool _isMusicing = true;


    private Vector2 _progress_init = new Vector2(-487, 0);
    private Vector2 _progress_finish = new Vector2(-15, 0);

    public void SetAddress(string address) {
        _address = address;
    }

    public void SetDescription(string description)
    {
        _description = description;
    }

    public void SetCardAgent(CardAgent cardAgent)
    {
        _cardAgent = cardAgent;
    }


    public void SetData(string address,string description,CardAgent cardAgent) {
        SetAddress(address);
        SetCardAgent(cardAgent);
        SetDescription(description);
    }





    public void Init() {
        _videoPlayer.source = VideoSource.Url;
        _videoPlayer.url = MagicWallManager.FileDir + "video\\" + _address;

        // 设置进度条
        _progress.anchoredPosition = _progress_init;

        //  设置播放错误回调
        _videoPlayer.errorReceived += ErrorReceivedCallBack;

        //  设置播放完成回调
        _videoPlayer.loopPointReached += LoopPointReachedCallBack;

        //  设置描述
        _text_description.text = _description;

        //  播放视频
        StartCoroutine(PlayVideo());
    }

    void Update() {

        // 播放控制按钮监控
        if (!_isPlaying)
        {
            if (!_btn_play.gameObject.activeSelf)
            {
                _btn_play.gameObject.SetActive(true);
            }
            _btn_pause.gameObject.SetActive(false);
        }
        else {
            if (!_btn_pause.gameObject.activeSelf)
            {
                _btn_pause.gameObject.SetActive(true);
            }
            _btn_play.gameObject.SetActive(false);


            UpdateTime();
            Progress(CalculateRate());
        }

        // 音乐控制按钮监控
        if (_isMusicing)
        {
            if (!_btn_muisc_disable.gameObject.activeSelf)
            {
                _btn_muisc_disable.gameObject.SetActive(true);
            }
            _btn_music_enable.gameObject.SetActive(false);
        }
        else {
            if (!_btn_music_enable.gameObject.activeSelf)
            {
                _btn_music_enable.gameObject.SetActive(true);
            }
            _btn_muisc_disable.gameObject.SetActive(false);
        }

    }


    IEnumerator PlayVideo()
    {
        _videoPlayer.Prepare();
        DoDisableMusic();


        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while (!_videoPlayer.isPrepared)
        {
            yield return waitForSeconds;
            break;
        }

        if (_videoPlayer.isPrepared)
        {
            _screen.texture = _videoPlayer.texture;

            float videow = _videoPlayer.texture.width;
            float videoh = _videoPlayer.texture.height;
            float screenw = _screen.texture.width;
            float screenh = _screen.texture.height;

            //Debug.Log("videow : " + videow + " videoh :" + videoh + " screenw:" + screenw + " screenh:" + screenh);

            RectTransform r = _screen.gameObject.GetComponent<RectTransform>();
            r.sizeDelta = new Vector2(videow, videoh);

            // 得出总时长 - 秒
            SetTotalTime();

        }

        DoPlay();
    
    }

    private void UpdateTime()
    {
        double d = _videoPlayer.time;
        string timeStr = ConverSecondsToTimeStr((float)d);
        _time.text = timeStr;
    }


    // 设置总体时间
    private void SetTotalTime() {
        float total = _videoPlayer.frameCount / _videoPlayer.frameRate; // 此时是秒
        string timeStr = ConverSecondsToTimeStr(total);
        _time.text = timeStr;
    }

    private string ConverSecondsToTimeStr(float seconds) {

        // 获取分钟数
        int minutes = Mathf.FloorToInt(seconds / 60);

        // 获取秒
        int sec = Mathf.CeilToInt(seconds % 60);

        string str_minute;
        if (minutes == 0)
        {
            str_minute = "00";
        }
        else if (minutes > 0 && minutes < 10)
        {
            str_minute = "0" + minutes.ToString();
        }
        else {
            str_minute = minutes.ToString();
        }

        string str_seconds;
        if (sec < 10)
        {
            str_seconds = "0" + sec.ToString();
        }
        else {
            str_seconds = sec.ToString();
        }

        return str_minute + ":" + str_seconds;
    }

    private float CalculateRate() {
        // 获取百分百
        if (_videoPlayer.frameRate == 0) {
            return 0;
        }

        float total = _videoPlayer.frameCount / _videoPlayer.frameRate; // 此时是秒
        float now = (float)_videoPlayer.time;
        float r;

        if (total == 0)
        {
            r = 0;
        }
        else {
            r = now / total;
        }

        return r;
    }

    private void Progress(float rate) {
        Vector2 to = Vector2.Lerp(_progress_init, _progress_finish, rate);
        _progress.DOAnchorPos(to, Time.deltaTime);
    }

    public void DoPlay() {
        _videoPlayer.Play();
        _isPlaying = true;
    }

    public void DoPause()
    {
        _videoPlayer.Pause();
        _isPlaying = false;
    }

    public void DoEnableMusic() {
        //_videoPlayer.SetDirectAudioVolume();
        _videoPlayer.SetDirectAudioMute(0, false);
        _isMusicing = true;
    }

    public void DoDisableMusic()
    {
        _videoPlayer.SetDirectAudioMute(0, true);
        _isMusicing = false;
    }

    public void DoClose()
    {
        _cardAgent?.DoCloseVideoContainer();
    }



    public void DoDestory()
    {
        // 销毁

    }


    private void ErrorReceivedCallBack(VideoPlayer source, string message) {

    }

    private void LoopPointReachedCallBack(VideoPlayer source) {
        _isPlaying = false;
        _progress.DOAnchorPos(_progress_init, Time.deltaTime);
    }

}
