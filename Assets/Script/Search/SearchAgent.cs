using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;


//
//  搜索代理
//
public class SearchAgent : MonoBehaviour
{
    //灵云SDK头文件：
    //hci_hwr.h
    //hci_sys.h
    //灵云SDK库文件：

    //hci_hwr.lib
    //hci_sys.lib
    //运行时所需DLL文件

    //必选模块
    //libhci_curl.dll
    //hci_sys.dll
    //hci_hwr.dll
    //云端识别
    //hci_hwr_cloud_recog.dll
    //本地识别
    //hci_hwr_local_recog.dll
    //联想功能
    //hci_hwr_associate.dll
    //拼音功能
    //hci_hwr_pinyin.dll
    //笔形功能
    //hci_hwr_penscript.dll



    #region  DLL 调用映射
    [DllImport("hci_hwr")]
    public static extern int hci_hwr_session_start(string str);

    [DllImport("hci_sys")]
    public static extern int hci_init(string pszConfig);

    /// <summary>
    /// 手动访问云授权
    //  当正常返回时，可以通过 hci_get_auth_expire_time()得到新授权的过期时间， 通过 hci_get_capability() 得到新授权的可使用的HCI能力。
    /// </summary>
    /// <returns></returns>
    [DllImport("hci_sys")]
    public static extern int hci_check_auth();

    /// <summary>
    /// 获得授权过期时间
    ///得到的时间和 time() 返回概念一致，指 1970-01-01 00:00:00 UTC 之后的秒数。
    /// </summary>
    [DllImport("hci_sys")]
    public static extern int hci_get_auth_expire_time(ref long expireTime);

    /// <summary>
    /// 设置当前用户（暂不支持本地能力）
    ///开发者指定当前用户。此处可以进行用户关联操作，即多个设备可以通过该接口建立请求数据映射。
    /// </summary>
    /// <param name="userid">指定当前用户，如果不存在则创建,字符串，最多64个字符</param>
    /// <returns></returns>
    [DllImport("hci_sys")]
    public static extern int hci_set_current_userid(string userid);


    /// <summary>
    /// 设置当前用户（暂不支持本地能力）
    ///开发者指定当前用户。此处可以进行用户关联操作，即多个设备可以通过该接口建立请求数据映射。
    /// </summary>
    /// <param name="userid">指定当前用户，如果不存在则创建,字符串，最多64个字符</param>
    /// <returns></returns>
    [DllImport("hci_sys")]
    public static extern int hci_hwr_init(string pszConfig);


    #endregion


    #region 灵云参数配置
    private static string devKey = "3a6d22a54d7d453d0689551661ea3f8e";
    private static string appKey = "195d5435";
    private static string cloudUrl = "http://api.hcicloud.com:8888";
    private static string authpath = Application.streamingAssetsPath + "/document/";
    private static string logFilePath = Application.streamingAssetsPath + "/document/log/";
    #endregion
    // 初始化手写服务



    void Start()
    {
        //string session_config = "capkey=hwr.local.pinyin";
        //int sessionId = hci_hwr_session_start(session_config);
        //Debug.Log("hci_hwr" + sessionId);
        InitLingYun();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //  灵云识别流程： 
    //      初始化灵云系统 
    //      授权检查
    //      初始化 HWR 能力
    //      开启 HWR 识别会话
    //      单字、多字识别；获取联想词；笔势识别；模拟笔形；拼音识别
    //      是否继续监测
    //      关闭 HWR 识别会话
    //      终止 HWR 能力
    //      终止灵云系统
    private bool InitLingYun() {

        try
        {
            InitLingYunSystem();

            CheckLingYunAuth();
        }
        catch (LinyunException ex) {
            Debug.Log(ex.GetError());
        }
        
        
        return false;
    }




    //  初始化灵云系统
    private void InitLingYunSystem() {
        string pszConfig = "developerKey=" + devKey + ","
            + "appKey=" + appKey + ","
            + "cloudUrl=" + cloudUrl + ","
            + "authPath=" + authpath + ","
            + "logFileSize=500,logLevel=5,"
            + "logFilePath=" + logFilePath + ","
            + "logFileCount=10";

        //  从云端拿到的授权文件会缓存在 hci_init() 时所提供的 authPath 路径下。
        //  以后使用会直接使用此文件， 不需要再到云端下载。
        //  但授权文件都有一个过期时间，一旦过期了，在一周的宽限期内，仍可以继续使用相应能力， 但超出宽限期，将无法再使用相应的能力。
        //  因此在过期时间到了之后必须及时到云端更新授权文件。
        int apiResult = hci_init(pszConfig);

        if (apiResult != 0) {
            throw new LinyunApiException("hci_init", apiResult);
        }
         
    }

    //  授权检查
    private void CheckLingYunAuth()
    {
        // 更新授权
        int apiResult = hci_check_auth();
        if (apiResult != 0)
        {
            throw new LinyunApiException("hci_check_auth", apiResult);
        }

        long expiredTime = 1;
        // 获取接口过期时间
        apiResult = hci_get_auth_expire_time(ref expiredTime);
        if (apiResult != 0)
        {
            throw new LinyunApiException("hci_get_auth_expire_time", apiResult);
        }

        // 获取当前的时间戳
        long currentTimestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;

        //  判断是否已过期
        if (expiredTime < currentTimestamp) {
            // 已过期
            throw new LinyunLogicException("Lingyun Auth Is Expired");
        }

        apiResult = hci_set_current_userid("admin");

        // 设置当前用户
        if (apiResult != 0)
        {
            throw new LinyunApiException("hci_set_current_userid", apiResult);
        }

    }



    //  灵云调用失败
    private void LingYunIsBreakCallback(string apiname,int result) {
        Debug.Log("灵云初始化失败");
    }

    //  

    abstract class LinyunException : ApplicationException
    {

        public LinyunException() {

        }

        public abstract string GetError();

    }

    class LinyunApiException : LinyunException {
        string api;
        int result;

        public LinyunApiException(string api, int result) {
            this.api = api;
            this.result = result;
        }

        public override string GetError()
        {
            string errorMessage = "API : " + api + " / result :" + result;
            return errorMessage;
        }
    }

    class LinyunLogicException : LinyunException
    {
        string message;

        public LinyunLogicException(string message)
        {
            this.message = message;
        }

        public override string GetError()
        {
            string errorMessage = "Logic Message : " + message;
            return errorMessage;
        }
    }


}
