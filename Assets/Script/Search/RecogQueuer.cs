using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.IO;
using UnityEngine;
using Baidu.Aip.Ocr;


/// <summary>
/// 识别队列
/// </summary>
public class RecogQueuer : MonoBehaviour
{
    private Queue<ErrorActionCallBackBean> _errorCallBackQueue;
    private Queue<SuccessActionCallBackBean> _successCallBackQueue;
    private Queue<Action> _finishCallBackQueue;


    private Ocr client;


    // Start is called before the first frame update
    void Start()
    {
        var APP_ID = "16425018";
        var API_KEY = "cZwexXD7l60l3OcZ4IT8yWPm";
        var SECRET_KEY = "4hrFgtVchql08SgZ3CQ9iE4oMhg42F5s";

        client = new Ocr(API_KEY, SECRET_KEY);
        client.Timeout = 60000;  // 修改超时时间


        _successCallBackQueue = new Queue<SuccessActionCallBackBean>();
        _errorCallBackQueue = new Queue<ErrorActionCallBackBean>();
        _finishCallBackQueue = new Queue<Action>();

    }

    // Update is called once per frame
    void Update()
    {
        if (_successCallBackQueue.Count > 0) {
            SuccessActionCallBackBean successActionCallBackBean = _successCallBackQueue.Dequeue();
            successActionCallBackBean.successCallBack.Invoke(successActionCallBackBean.param);
        }

        if (_errorCallBackQueue.Count > 0)
        {
            ErrorActionCallBackBean errorActionCallBackBean = _errorCallBackQueue.Dequeue();
            errorActionCallBackBean.errorCallBack.Invoke(errorActionCallBackBean.param);
        }

        if (_finishCallBackQueue.Count > 0)
        {
            _finishCallBackQueue.Dequeue().Invoke();
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path">图片文件全路径</param>
    public void AddRecogTask(string path,Action<string> errorCallBack,Action<string[]> successCallBack, Action finishCallBack) {

        ParameterizedThreadStart start = RecognizeImage;

        Thread thread = new Thread(start);
        thread.Start(new RecognizeImageParams(path, errorCallBack, successCallBack, finishCallBack));
        
    }

    public void AddRecogTask(byte[] bytes, Action<string> errorCallBack, Action<string[]> successCallBack, Action finishCallBack)
    {

        ParameterizedThreadStart start = RecognizeImage;

        Thread thread = new Thread(start);
        thread.Start(new RecognizeImageParams(bytes, errorCallBack, successCallBack, finishCallBack));

    }



    /// <summary>
    /// 识别图片
    /// </summary>
    /// <param name="path">全路径</param>
    private void RecognizeImage(object p)
    {
        RecognizeImageParams recognizeImageParams = p as RecognizeImageParams;

        byte[] image = null;
        if (recognizeImageParams.bytes != null)
        {
            image = recognizeImageParams.bytes;
        }
        else {
            image = File.ReadAllBytes(recognizeImageParams.path);
        }



        // 如果有可选参数
        var options = new Dictionary<string, object>{
        {"language_type", "CHN_ENG"},
        {"detect_direction", "false"},
        {"detect_language", "false"},
        {"probability", "false"}
        };
        // 带参数调用通用文字识别, 图片参数为本地图片
        try
        {
            var result = client.GeneralBasic(image, options);   //  这部耗时

            Debug.Log("111：" + result);

            //  模拟返回结构 
            string[] strs = { "徐", "我", "汉" };

            _successCallBackQueue.Enqueue(new SuccessActionCallBackBean(strs, recognizeImageParams.successCallBack));

        }
        catch (Exception ex)
        {
            //  识别错误服务
            Debug.Log("Exception : " + ex.Message);
            _errorCallBackQueue.Enqueue(new ErrorActionCallBackBean(ex.Message, recognizeImageParams.errorCallBack));

        }
        finally
        {
            //_writeStatus = WriteStatus.RecognizeFinished;
            _finishCallBackQueue.Enqueue(recognizeImageParams.finishCallBack);
        }
    }


    class RecognizeImageParams {
        byte[] _bytes;
        string _path;   //  完整路径
        Action<string> _errorCallBack;
        Action<string[]> _successCallBack;
        Action _finishCallBack;

        public RecognizeImageParams(string path, Action<string> errorCallBack, Action<string[]> successCallBack, Action finishCallBack) {
            _path = path;
            _errorCallBack = errorCallBack;
            _successCallBack = successCallBack;
            _finishCallBack = finishCallBack;
        }

        public RecognizeImageParams(byte[] bytes, Action<string> errorCallBack, Action<string[]> successCallBack, Action finishCallBack)
        {
            _bytes = bytes;
            _errorCallBack = errorCallBack;
            _successCallBack = successCallBack;
            _finishCallBack = finishCallBack;
        }

        public byte[] bytes { set { _bytes = value; } get { return _bytes; } }
        public string path { set { _path = value; } get { return _path; } }
        public Action<string> errorCallBack { set { _errorCallBack = value; } get { return _errorCallBack; } }
        public Action<string[]> successCallBack { set { _successCallBack = value; } get { return _successCallBack; } }
        public Action finishCallBack { set { _finishCallBack = value; } get { return _finishCallBack; } }
    }


    class SuccessActionCallBackBean
    {
        string[] _params;
        Action<string[]> _successCallBack;

        public SuccessActionCallBackBean(string[] param, Action<string[]> successCallBack)
        {
            _params = param;
            _successCallBack = successCallBack;
        }

        public string[] param { set { _params = value; } get { return _params; } }
        public Action<string[]> successCallBack { set { _successCallBack = value; } get { return _successCallBack; } }
    }

    class ErrorActionCallBackBean
    {
        string _params;
        Action<string> _errorCallBack;

        public ErrorActionCallBackBean(string param, Action<string> successCallBack)
        {
            _params = param;
            _errorCallBack = successCallBack;
        }

        public string param { set { _params = value; } get { return _params; } }
        public Action<string> errorCallBack { set { _errorCallBack = value; } get { return _errorCallBack; } }
    }


}
