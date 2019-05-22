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

    [SerializeField] RectTransform _btn_play;
    [SerializeField] RectTransform _btn_pause;


    private CrossCardAgent crossCardAgent;
    private string _address;
    private bool _isPlaying = false;

    private Vector2 _progress_init = new Vector2(-487, 0);
    private Vector2 _progress_finish = new Vector2(-15, 0);

    public void SetAddress(string address) {
        _address = address;
    }

    public void Init() {
        _videoPlayer.source = VideoSource.Url;
        _videoPlayer.url = "file://E:/workspace/MagicWall/Assets/Files/env/video/2.mp4";

        // 设置进度条
        _progress.anchoredPosition = _progress_init;
        StartCoroutine(PlayVideo());
    }

    void Update() {
        //Debug.Log("Video time : " + _videoPlayer.time);
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
    }


    IEnumerator PlayVideo()
    {
        Debug.Log("Play Video !");

        _videoPlayer.Prepare();

        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while (!_videoPlayer.isPrepared)
        {
            Debug.Log("Wait prepared");
            yield return waitForSeconds;
            break;
        }

        Debug.Log("videoPlayer.isPrepared !");

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
            //Debug.Log("Video timeReference : " + _videoPlayer.frameCoun);
            //Debug.Log("Video timeSource : " + _videoPlayer.timeSource);

            //_screen.texture.width

        }

        _videoPlayer.Play();
        _isPlaying = true;
        //audioSource.Play();
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
        float total = _videoPlayer.frameCount / _videoPlayer.frameRate; // 此时是秒
        float now = (float)_videoPlayer.time;

        float r = now / total;

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

    public void DoClose()
    {

  
    }

}
