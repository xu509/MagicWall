using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using UnityEngine.UI;



//
//  搜索代理
//
public class SearchAgent : MonoBehaviour
{
    Action _onClickReturn;

    [SerializeField] WritePadAgent _writePadAgent;  // 手写板agent
    [SerializeField] RectTransform _associateWordArea; // 联想内容区域
    [SerializeField] AssociateWordAgent _associateWordPrefab; //联想字的prefab
    [SerializeField] RectTransform _associateWordMessagePrefab; //联想字提示的prefab
    [SerializeField] RectTransform _backspaceRect; //退格控件
    [SerializeField] Text _searchText; //搜索词的文本控件

    private string _searchWord;


    private int sessionId; //该会话

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
    /// 灵云HWR能力 初始化
    /// </summary>
    /// <param name="pszConfig">初始化配置串,ASCII字符串，以'\0'结束</param>
    /// <returns></returns>
    [DllImport("hci_hwr")]
    public static extern int hci_hwr_init(string pszConfig);

    /// <summary>
    ///     开始会话
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    [DllImport("hci_hwr")]
    public static extern int hci_hwr_session_start(string pszConfig,ref int sessionId);

    #endregion


    #region 灵云参数配置
    private static string devKey = "3a6d22a54d7d453d0689551661ea3f8e";
    private static string appKey = "195d5435";
    private static string cloudUrl = "http://api.hcicloud.com:8888";
    private static string authpath = Application.streamingAssetsPath + "/lingyun/document/";
    private static string logFilePath = Application.streamingAssetsPath + "/lingyun/document/log/";

    private static string hwrDataPath = Application.streamingAssetsPath + "/lingyun/data/hwr";
    private static string hwrCapKey_Letter = "hwr.local.letter";

    #endregion
    // 初始化手写服务



    [DllImport("hci_hwr_pinyin")]
    public static extern int hci_hwr_pinyin(string str);

    

    void Start()
    {
        //string session_config = "capkey=hwr.local.pinyin";
        //int sessionId = hci_hwr_session_start(session_config);
        //Debug.Log("hci_hwr" + sessionId);
        InitLingYun();

        // 初始化
        Init();

        // 初始化手写板相关
        _writePadAgent.SetOnRecognized(OnRecognized);

    }

    void Init() {
        _searchWord = "";

        InitBackspaceStatus();
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
            //InitLingYunSystem();
            //CheckLingYunAuth();
            //InitLingYunHWR();
            //InitLingYunHwrSessionStart();
            //Debug.Log("Session ID : " + sessionId);
        }
        catch (LingyunException ex) {
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
            throw new LingyunApiException("hci_init", apiResult);
        }
         
    }

    //  授权检查
    private void CheckLingYunAuth()
    {
        // 更新授权
        int apiResult = hci_check_auth();
        if (apiResult != 0)
        {
            throw new LingyunApiException("hci_check_auth", apiResult);
        }

        long expiredTime = 1;
        // 获取接口过期时间
        apiResult = hci_get_auth_expire_time(ref expiredTime);
        if (apiResult != 0)
        {
            throw new LingyunApiException("hci_get_auth_expire_time", apiResult);
        }

        // 获取当前的时间戳
        long currentTimestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;

        //  判断是否已过期
        if (expiredTime < currentTimestamp) {
            // 已过期
            throw new LingyunLogicException("Lingyun Auth Is Expired");
        }

        apiResult = hci_set_current_userid("admin");

        // 设置当前用户
        if (apiResult != 0)
        {
            throw new LingyunApiException("hci_set_current_userid", apiResult);
        }

    }

    // 初始化 HWR 能力
    private void InitLingYunHWR() {
        //string pszConfig = "dataPath=" + hwrDataPath 
        //    + ",initCapKeys=" + hwrCapKey_Letter;
        string pszConfig = "";

        int apiResult = hci_hwr_init(pszConfig);
        if (apiResult == 301)
        {
            Debug.Log("HWR 已经初始化");
        }
        else if (apiResult != 0)
        {
            throw new LingyunApiException("hci_hwr_init", apiResult);
        }

    }

    // 开启 HWR 识别会话
    private void InitLingYunHwrSessionStart() {
        
        string pszConfig = "capKey=hwr.cloud.letter";
        int apiResult = hci_hwr_session_start(pszConfig,ref sessionId);
         if (apiResult != 0)
        {
            throw new LingyunApiException("InitLingYunHwrSessionStart", apiResult);
        }
    }

    // 开启 HWR 识别会话

    //  灵云调用失败
    private void LingYunIsBreakCallback(string apiname,int result) {
        Debug.Log("灵云初始化失败");
    }

    #region 其他功能
    public void OnClickReturn(Action action) {
        _onClickReturn = action;
    }

    // 点击回退
    public void ClickReturn() {
        _onClickReturn.Invoke();
    }


    #endregion

    #region 灵云错误类型 

    abstract class LingyunException : ApplicationException
    {

        public LingyunException() {

        }

        public abstract string GetError();

    }

    class LingyunApiException : LingyunException {
        string api;
        int result;

        public LingyunApiException(string api, int result) {
            this.api = api;
            this.result = result;
        }

        public override string GetError()
        {
            string errorMessage = "API : " + api + " / result :" + result;
            return errorMessage;
        }
    }

    class LingyunLogicException : LingyunException
    {
        string message;

        public LingyunLogicException(string message)
        {
            this.message = message;
        }

        public override string GetError()
        {
            string errorMessage = "Logic Message : " + message;
            return errorMessage;
        }
    }
    #endregion

    //  手写板识别内容后的回调
    private void OnRecognized(string[] strs) {
        // 清理联想板块
        ClearAssociateWordArea();

        // 增加联想的内容
        if (strs.Length == 0)
        {
            RectTransform item = Instantiate(_associateWordMessagePrefab, _associateWordArea);
            item.GetComponent<Text>().text = "未能识别您的笔迹。";
        }
        else {
            int length = strs.Length;
            if (length > 6)
                length = 6;

            for (int i = 0; i < length; i++) {
                AssociateWordAgent associateWordAgent = Instantiate(_associateWordPrefab, _associateWordArea);
                associateWordAgent.SetText(strs[i]);
                // 装载点击事件
                associateWordAgent.SetOnClickWord(OnClickAssociateWord);

            }
        }
    }



    // 清理联想板块
    private void ClearAssociateWordArea() {
        foreach (Transform child in _associateWordArea)
        {
            Destroy(child.gameObject);
        }
    }

    // 联想词点击事件
    private void OnClickAssociateWord(string str) {
        // 将被点击的字添加至搜索框内

        _searchWord += str;

        UpdateSearchWord();

        // 清理联想面板
        ClearAssociateWordArea();

        // 更新退格状态
        InitBackspaceStatus();
    }

    //  初始化退格状态
    private void InitBackspaceStatus() {
        int count = _searchWord.Length;
        if (count == 0)
        {
            _backspaceRect.gameObject.SetActive(false);
        }
        else {
            _backspaceRect.gameObject.SetActive(true);
        }
    }


    // 退格功能
    public void DoBackspace() {
        _searchWord = _searchWord.Substring(0, _searchWord.Length - 1);

        UpdateSearchWord();

        InitBackspaceStatus();
    }

    // 搜索功能
    public void DoSearch()
    {
        // 进行搜索
         

        // 获取搜索结果

    }




    private void UpdateSearchWord() {
        _searchText.text = _searchWord;
    }

}
