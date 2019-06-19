using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Net;
using System;





/// <summary>
/// 灵云客户端
/// </summary>
public class SVClient
{
    private static string api_address = "api.hcicloud.com:8888";

    //  devKey : 3a6d22a54d7d453d0689551661ea3f8e
    //  appKey : 195d5435
    private static string api_hwr_recognize = api_address + "/hwr/Recognise";


    /// <summary>
    /// 
    /// </summary>
    /// <param name="appKey"></param>
    /// <param name=""></param>
    public SVClient(string appKey) {



    }


    /// <summary>
    /// 识别函数
    /// </summary>
    /// <returns></returns>

    public JObject Recognize(short[] datas) {

        Uri uri = new Uri(api_hwr_recognize);


        return null;
    }


}
