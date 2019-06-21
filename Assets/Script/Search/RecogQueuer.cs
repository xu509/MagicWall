using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.IO;
using UnityEngine;
using Baidu.Aip.Ocr;
using Newtonsoft.Json.Linq;

/// <summary>
/// 识别队列
/// </summary>
public class RecogQueuer : MonoBehaviour
{
    private Queue<ErrorActionCallBackBean> _errorCallBackQueue;
    private Queue<SuccessActionCallBackBean> _successCallBackQueue;
    private Queue<Action> _finishCallBackQueue;


    private Ocr client;
    private SVClient sVClient;


    // Start is called before the first frame update
    void Start()
    {
        //  百度API
        //var APP_ID = "16425018";
        //var API_KEY = "cZwexXD7l60l3OcZ4IT8yWPm";
        //var SECRET_KEY = "4hrFgtVchql08SgZ3CQ9iE4oMhg42F5s";

        //client = new Ocr(API_KEY, SECRET_KEY);
        //client.Timeout = 60000;  // 修改超时时间

        var appKey = "195d5435";
        var devKey = "3a6d22a54d7d453d0689551661ea3f8e";
        var timeOut = 20000;

        sVClient = new SVClient(appKey,devKey,timeOut);


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
            //JObject result = client.GeneralBasic(image, options);   //  这部耗时 
            //JObject result = client.AccurateBasic(image, options);   //  这部耗时

            short[] datas = {   103 ,283 ,105 ,283 ,107 ,283 ,113 ,283 ,120 ,283
    ,129 ,283 ,138 ,283 ,146 ,283 ,156 ,283 ,162 ,283
    ,165 ,283 ,166 ,283 ,-1 ,0 ,282 ,245 ,277 ,247
    ,270 ,251 ,266 ,255 ,263 ,257 ,259 ,261 ,254 ,266
    ,250 ,273 ,246 ,281 ,243 ,286 ,240 ,292 ,240 ,294
    ,239 ,296 ,238 ,297 ,238 ,298 ,-1 ,0 ,262 ,271
    ,264 ,272 ,266 ,272 ,268 ,272 ,270 ,273 ,272 ,274
    ,275 ,274 ,278 ,276 ,280 ,278 ,283 ,279 ,286 ,281
    ,289 ,282 ,289 ,283 ,291 ,284 ,292 ,285 ,292 ,286
    ,-1 ,0 ,268 ,281 ,268 ,282 ,268 ,284 ,270 ,287
    ,270 ,290 ,270 ,294 ,270 ,297 ,270 ,299 ,270 ,301
    ,270 ,303 ,270 ,304 ,270 ,306 ,270 ,308 ,269 ,309
    ,269 ,310 ,269 ,311 ,269 ,312 ,269 ,314 ,269 ,316
    ,269 ,318 ,269 ,319 ,269 ,321 ,269 ,322 ,269 ,323
    ,269 ,324 ,268 ,324 ,-1 ,0 ,382 ,255 ,382 ,256
    ,382 ,260 ,382 ,263 ,381 ,267 ,378 ,274 ,375 ,278
    ,373 ,282 ,372 ,287 ,371 ,291 ,369 ,294 ,368 ,297
    ,367 ,300 ,367 ,301 ,366 ,302 ,365 ,304 ,364 ,305
    ,364 ,306 ,363 ,308 ,362 ,308 ,362 ,309 ,361 ,310
    ,361 ,311 ,360 ,311 ,-1 ,0 ,376 ,289 ,377 ,290
    ,378 ,290 ,380 ,291 ,381 ,292 ,382 ,293 ,384 ,294
    ,385 ,294 ,387 ,297 ,388 ,298 ,390 ,299 ,393 ,300
    ,394 ,301 ,396 ,302 ,398 ,303 ,400 ,305 ,401 ,306
    ,403 ,307 ,404 ,309 ,405 ,309 ,407 ,311 ,408 ,312
    ,409 ,314 ,410 ,314 ,411 ,314 ,-1 ,0 ,-1 ,-1,};

            Debug.Log("SV CLIENT RECOGNIZE");

            var r = sVClient.Recognize(datas);

            Debug.Log(r);


            //int r = (int)result["words_result_num"];

            //if (r == 0)
            //{
            //    _errorCallBackQueue.Enqueue(new ErrorActionCallBackBean("未能识别", recognizeImageParams.errorCallBack));
            //}
            //else {
            //    JArray results = (JArray)result["words_result"];

            //    string[] strs = new string[r];

            //    for (int i = 0; i < results.Count; i++) {
            //        string word = (string) results[i]["words"];
            //        strs[i] = word;
            //    }

            //    _successCallBackQueue.Enqueue(new SuccessActionCallBackBean(strs, recognizeImageParams.successCallBack));
            //}

            //  模拟返回结构 
            string[] strs = { "徐", "我", "汉" };
            _successCallBackQueue.Enqueue(new SuccessActionCallBackBean(strs, recognizeImageParams.successCallBack));

        }
        catch (Exception ex)
        {
            //  识别错误服务
            Debug.Log("Exception : " + ex.Message);
            Debug.Log(ex.StackTrace);
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
