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
namespace MagicWall
{
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
            var timeOut = 2000;

            sVClient = new SVClient(appKey, devKey, timeOut);


            _successCallBackQueue = new Queue<SuccessActionCallBackBean>();
            _errorCallBackQueue = new Queue<ErrorActionCallBackBean>();
            _finishCallBackQueue = new Queue<Action>();

        }

        // Update is called once per frame
        void Update()
        {
            if (_successCallBackQueue.Count > 0)
            {
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
        public void AddRecogTask(string path, Action<string> errorCallBack, Action<string[]> successCallBack, Action finishCallBack)
        {

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

        public void AddRecogTask(short[] shorts, Action<string> errorCallBack, Action<string[]> successCallBack, Action finishCallBack)
        {

            ParameterizedThreadStart start = RecognizeImage;

            Thread thread = new Thread(start);
            thread.Start(new RecognizeImageParams(shorts, errorCallBack, successCallBack, finishCallBack));

        }



        /// <summary>
        /// 识别图片
        /// </summary>
        /// <param name="path">全路径</param>
        private void RecognizeImage(object p)
        {
            RecognizeImageParams recognizeImageParams = p as RecognizeImageParams;

            //byte[] image = null;
            //if (recognizeImageParams.bytes != null)
            //{
            //    image = recognizeImageParams.bytes;
            //}
            //else {
            //    image = File.ReadAllBytes(recognizeImageParams.path);
            //}

            // 获取识别用数组
            short[] datas = recognizeImageParams.shorts;

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

                //short[] datas = { 526, 159, 532, 159, 541, 159, 550, 159, 560, 159, 570,
                //    159, 581, 159, 593, 158, 603, 157, 614, 156, 624, 155, 634, 153, 642,
                //    152, 649, 151, 655, 150, 659, 149, 662, 148, 663, 148, -1, 0, 578,
                //    141, 573, 154, 567, 169, 561, 185, 555, 201, 549, 217, 543, 232, 539,
                //    246, 535, 258, 531, 268, 529, 275, 527, 281, 527, 285, 527, 287, 528,
                //    288, 530, 288, 534, 287, 539, 285, 545, 283, 552, 280, 560, 277, 569,
                //    273, 578, 270, 588, 267, 598, 265, 607, 263, 616, 263, 625, 262, 633,
                //    262, 640, 262, 646, 262, 651, 263, 655, 263, 658, 264, 659, 265, 659,
                //    266, -1, 0, 523, 396, 527, 393, 532, 391, 539, 388, 546, 386, 554,
                //    383, 563, 382, 573, 381, 582, 380, 592, 379, 602, 379, 612, 378, 621,
                //    377, 629, 377, 637, 376, 645, 375, 652, 374, 658, 373, 663, 372, 667,
                //    372, 670, 371, 671, 369, 671, 367, -1, 0, 604, 238, 604, 249, 603, 262,
                //    602, 278, 601, 295, 600, 312, 598, 330, 596, 348, 594, 366, 592, 383, 591,
                //    399, 589, 414, 587, 426, 586, 437, 586, 445, 585, 453, 585, 459, 585, 463,
                //    585, 467, 585, 469, -1, 0, 440, 131, 441, 136, 444, 143, 446, 150, 449, 158,
                //    452, 167, 455, 176, -1, 0, 450, 234, 455, 234, 459, 235, 463, 236, 468, 237, 471,
                //    240, 475, 242, 477, 246, 479, 250, 480, 255, 480, 260, 480, 266, 479, 272, 477, 278,
                //    474, 284, 472, 289, 470, 294, 469, 298, 467, 302, 466, 305, 466, 308, 466, 310, 466,
                //    312, 467, 313, 469, 315, 471, 316, 474, 317, 478, 318, 481, 320, 485, 322, 488, 324,
                //    491, 326, 494, 330, 496, 334, 498, 339, 500, 345, 500, 352, 501, 359, 501, 368, 500,
                //    377, 498, 386, 496, 397, 493, 407, 489, 418, 486, 430, 481, 440, 477, 451, 473, 461,
                //    469, 470, 465, 477, 462, 484, 459, 489, 457, 492, 455, 495, 454, 496, 454, 497, 456,
                //    497, 458, 497, 461, 496, 465, 495, 469, 493, 474, 491, 480, 489, 486, 487, 492, 485, 498,
                //    484, 505, 482, 511, 481, 517, 480, 524, 478, 531, 478, 537, 477, 544, 477, 552, 476, 560,
                //    476, 568, 476, 576, 476, 585, 476, 594, 475, 603, 474, 612, 473, 621, 472, 631, 470, 640, 467,
                //    650, 465, 659, 463, 669, 460, 679, 457, 689, 454, 698, 452, 708, 450, 717, 447, 726, 445, 734,
                //    444, 741, 442, 748, 441, 754, 440, 759, 439, 764, 438, 768, 437, 772, 437, 775, 437, 778, 436, 780,
                //    436, 781, 436, 782, 436, 783, 436, 781, 436, -1, 0, -1, -1 };

                var r = sVClient.Recognize(datas);

                if ((string)r["result"] == "success")
                {
                    int count = (int)r["count"];

                    if (count == 0)
                    {
                        _errorCallBackQueue.Enqueue(new ErrorActionCallBackBean("未能识别", recognizeImageParams.errorCallBack));
                    }
                    else
                    {
                        JArray results = (JArray)r["data"];
                        string[] words = new string[results.Count];

                        for (int i = 0; i < results.Count; i++)
                        {
                            string word = (string)results[i];
                            words[i] = word;
                        }

                        _successCallBackQueue.Enqueue(new SuccessActionCallBackBean(words, recognizeImageParams.successCallBack));
                    }

                }
                else
                {

                    Debug.Log("识别错误");

                    _errorCallBackQueue.Enqueue(new ErrorActionCallBackBean("识别服务错误", recognizeImageParams.errorCallBack));
                }


                //  模拟返回结构 
                //string[] strs = { "徐", "我", "汉" };
                //_successCallBackQueue.Enqueue(new SuccessActionCallBackBean(strs, recognizeImageParams.successCallBack));

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


        class RecognizeImageParams
        {
            byte[] _bytes;
            short[] _shorts;
            string _path;   //  完整路径
            Action<string> _errorCallBack;
            Action<string[]> _successCallBack;
            Action _finishCallBack;

            public RecognizeImageParams(string path, Action<string> errorCallBack, Action<string[]> successCallBack, Action finishCallBack)
            {
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

            public RecognizeImageParams(short[] shorts, Action<string> errorCallBack, Action<string[]> successCallBack, Action finishCallBack)
            {
                _shorts = shorts;
                _errorCallBack = errorCallBack;
                _successCallBack = successCallBack;
                _finishCallBack = finishCallBack;
            }

            public byte[] bytes { set { _bytes = value; } get { return _bytes; } }

            public short[] shorts { set { _shorts = value; } get { return _shorts; } }

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
}