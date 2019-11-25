using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Net;
using System;
using System.Security.Cryptography;
using System.IO;
using System.Globalization;
using System.Text;
using System.Xml;

/// <summary>
/// 灵云客户端
/// </summary>
namespace MagicWall
{
    public class SVClient
    {
        private static string api_address = "http://api.hcicloud.com:8880";

        //  devKey : 3a6d22a54d7d453d0689551661ea3f8e
        //  appKey : 195d5435
        private static string api_hwr_recognize = api_address + "/hwr/Recognise";

        /// <summary>
        /// 应用标识
        /// </summary>
        private static string x_app_key = "x-app-key";

        /// <summary>
        /// sdk版本号
        /// </summary>
        private static string x_sdk_version = "x-sdk-version";

        /// <summary>
        ///     请求时间 : 2016-6-18 10:10:11
        /// </summary>
        private static string x_request_date = "x-request-date";

        /// <summary>
        /// 任务参数信息: capkey=hwr.cloud.freewrite, candNum=10  [必选，为name=value形式，多个参数以逗号隔开]
        /// </summary>
        private static string x_task_config = "x-task-config";

        /// <summary>
        /// 请求数据签名: 必选 x-session-key生成算法说明： x-session-key = md5(x-request-date + devkey)
        /// </summary
        private static string x_session_key = "x-session-key";

        /// <summary>
        /// 可选，如使用设备取设备标识号，如不使用设备设置为例子中的默认值
        /// </summary>
        private static string x_udid = "x-udid";

        /// <summary>
        /// APP KEY
        /// </summary>
        private string _appKey;

        /// <summary>
        /// DEV KEY
        /// </summary>
        private string _devKey;

        /// <summary>
        /// 请求日期
        /// </summary>
        private string _requestDate;

        /// <summary>
        /// 超时时间
        /// </summary>
        private int _timeOut;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name=""></param>
        public SVClient(string appKey, string devKey, int timeOut)
        {
            _appKey = appKey;
            _devKey = devKey;
            _timeOut = timeOut;

        }


        /// <summary>
        /// 识别函数
        /// </summary>
        /// <returns></returns>

        public JObject Recognize(short[] datas)
        {
            //string test_url = "http://systemapi.shsportshistory.com/api/company/companies";
            // api_hwr_recognize


            Uri uri = new Uri(api_hwr_recognize);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = "POST";    //  设置请求模式
            request.ContentType = "";

            // 设置 header
            request.Headers.Add(x_app_key, _appKey);
            request.Headers.Add(x_sdk_version, "8.1");
            request.Headers.Add(x_request_date, GetRequestDateStr());
            request.Headers.Add(x_task_config, "capkey=hwr.cloud.letter,candNum=10");
            request.Headers.Add(x_session_key, GetSessionKey());
            request.Headers.Add("x-udid", "101:1234567890");


            // 设置过期时间
            request.Timeout = _timeOut;


            // 设置包体数据

            var reqStream = request.GetRequestStream();
            byte[] b = shortToBytes(datas);  // 类型转换

            reqStream.Write(b, 0, b.Length);
            reqStream.Close();


            HttpWebResponse httpWebResponse = null;
            StreamReader streamReader = null;
            JObject result = null;
            // 获得 response
            try
            {
                httpWebResponse = (HttpWebResponse)request.GetResponse();
                streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                string responseContent = streamReader.ReadToEnd();

                result = ReadXmlResult(responseContent);

            }
            catch (Exception ex)
            {
                Debug.Log("请求出错： " + ex.Message);
                result["result"] = "error";
            }
            finally {

                httpWebResponse?.Close();
                streamReader?.Close();
                request.Abort();
                httpWebResponse.Close();
            }

            return result;

        }

        private JObject ReadXmlResult(string xmlresult)
        {
            JObject result = new JObject();
            // 读取XML格式的文件
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlresult);

            XmlNode responseInfo = doc.SelectSingleNode("ResponseInfo");
            XmlNodeList responseInfoList = responseInfo.ChildNodes;

            string ResCode = responseInfoList.Item(0).InnerText;

            if (ResCode.Equals("Success"))
            {
                // 识别成功
                XmlNode resultCountNode = doc.SelectSingleNode("ResponseInfo/ResultCount");

                //  获取识别数目
                int resultCount = 0;
                int.TryParse(resultCountNode.InnerText, out resultCount);
                result.Add("result", "success");
                result.Add("count", resultCount);

                if (resultCount > 0)
                {
                    XmlNodeList results = doc.SelectNodes("ResponseInfo/Result");

                    JArray datas = new JArray();
                    foreach (XmlNode re in results)
                    {
                        datas.Add(re["Text"].InnerText);
                    }

                    result.Add("data", datas);
                }
            }
            else
            {
                result.Add("result", "error");
                result.Add("msg", "识别程序发生了错误");
            }

            return result;
        }



        /// <summary>
        ///  日期格式：2016-6-18 10:10:11
        /// </summary>
        /// <returns></returns>
        private string GetRequestDateStr()
        {
            DateTime date = DateTime.Now;
            string str = date.ToString("yyyy-MM-dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo);

            _requestDate = str;
            return str;
        }


        /// <summary>
        /// x-session-key生成算法说明： x-session-key = md5(x-request-date + devkey)
        /// </summary>
        /// <returns></returns>
        private string GetSessionKey()
        {
            string content = _requestDate + _devKey;

            // MD5 加密过程
            var md5 = new MD5CryptoServiceProvider();

            string result = byteToHex(md5.ComputeHash(System.Text.Encoding.Default.GetBytes(content)));

            return result;
        }

        // 16进制
        private string byteToHex(byte[] bytes)
        {
            StringBuilder hexStr = new StringBuilder();
            int num;
            for (int count = 0; count < bytes.Length; count++)
            {
                num = bytes[count];
                if (num < 0)
                {
                    num += 256;
                }
                if (num < 16)
                {
                    hexStr.Append("0");
                    //hexStr.append("0");
                }

                hexStr.Append(num.ToString("X"));

            }
            return hexStr.ToString().ToUpper();
        }

        //手写Short数组转化为Byte数组
        private byte[] shortToBytes(short[] shortData)
        {
            // TODO Auto-generated method stub
            int count = shortData.Length;
            byte[] dest = new byte[count << 1];

            for (int i = 0; i < count; i++)
            {
                dest[i * 2] = (byte)(shortData[i]);
                dest[i * 2 + 1] = (byte)(shortData[i] >> 8);
            }
            return dest;
        }

    }
}