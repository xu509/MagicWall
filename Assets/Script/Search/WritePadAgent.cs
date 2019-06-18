using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Baidu.Aip.Ocr;
using System.Threading;
using System;

public class WritePadAgent : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{

    private RenderTexture texRender;   //画布
    public Material mat;     //给定的shader新建材质
    public Texture brushTypeTexture;   //画笔纹理，半透明
    private Camera mainCamera;
    private float brushScale = 0.5f;
    public Color brushColor = Color.black;
    public RawImage raw;                   //使用UGUI的RawImage显示，方便进行添加UI,将pivot设为(0.5,0.5)
    private float lastDistance;
    private Vector3[] PositionArray = new Vector3[3];
    private int a = 0;
    private Vector3[] PositionArray1 = new Vector3[4];
    private int b = 0;
    private float[] speedArray = new float[4];
    private int s = 0;
    public int num = 50;

    Vector2 rawMousePosition;            //raw图片的左下角对应鼠标位置 
    float rawWidth;                               //raw图片宽度
    float rawHeight;                              //raw图片长度


    private Ocr client;

    private WriteStatus _writeStatus = WriteStatus.Init;   //  书写状态
    private float _lastWriteTime = 0f;  //  最近的书写时间点
    [SerializeField] private float _recognizeIntervalTime = 2f; // 识别周期

    Action<string[]> _OnRecognizeSuccess;    //识别成功回调
    Action<string> _OnRecognizeError;    //识别失败回调


    [SerializeField] private RecogQueuer _recogQueuer;  //识别队列


    // 书写状态
    private enum WriteStatus
    {
        Init,   //  复位
        Writing,    // 手写中
        WriteFinished,  // 手写结束
        RecognizeStart,    // 开始识别
        Recognizing,    // 识别中
        RecognizeFinished    // 识别结束

    }



    void Start()
    {

        //// 设置APPID/AK/SK
        var APP_ID = "16425018";
        var API_KEY = "cZwexXD7l60l3OcZ4IT8yWPm";
        var SECRET_KEY = "4hrFgtVchql08SgZ3CQ9iE4oMhg42F5s";

        client = new Ocr(API_KEY, SECRET_KEY);
        client.Timeout = 60000;  // 修改超时时间


        //raw图片鼠标位置，宽度计算
        rawWidth = raw.rectTransform.sizeDelta.x;
        rawHeight = raw.rectTransform.sizeDelta.y;

        UpdateRawMousePosition();

        //texRender = new RenderTexture(1000, 1000, 24, RenderTextureFormat.ARGB32);
        //RenderTexture.GetTemporary

        texRender = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        Clear(texRender);

        DrawImage();
    }

    Vector3 startPosition = Vector3.zero;
    Vector3 endPosition = Vector3.zero;
    void Update()
    {
        float now = Time.time;

        if (_writeStatus == WriteStatus.WriteFinished && ((now - _lastWriteTime) > _recognizeIntervalTime))
        {

            // 开始确认
            Debug.Log("Begin Recognize");
            _writeStatus = WriteStatus.RecognizeStart;
        }

        if (_writeStatus == WriteStatus.RecognizeStart)
        {
            Recognizing();
        }

        if (_writeStatus == WriteStatus.Recognizing)
        {
            // Do Recognizing
            Debug.Log(" Do Recognizing");

        }

        if (_writeStatus == WriteStatus.RecognizeFinished)
        {
            // DO Recognize finished
            RecognizeComplete();
            Debug.Log(" Do Recognizing finished");
            _writeStatus = WriteStatus.Init;
        }

    }

    #region 手写功能相关
    void OnMouseUp()
    {
        startPosition = Vector3.zero;
        //brushScale = 0.5f;
        a = 0;
        b = 0;
        s = 0;

        UpdateRawMousePosition();
    }
    //设置画笔宽度
    float SetScale(float distance)
    {
        float Scale = 0;
        if (distance < 100)
        {
            Scale = 0.8f - 0.005f * distance;
        }
        else
        {
            Scale = 0.425f - 0.00125f * distance;
        }
        if (Scale <= 0.05f)
        {
            Scale = 0.05f;
        }
        return Scale;
    }

    void OnMouseMove(Vector3 pos)
    {
        if (startPosition == Vector3.zero)
        {
            startPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        }

        endPosition = pos;
        float distance = Vector3.Distance(startPosition, endPosition);

        brushScale = SetScale(distance);

        ThreeOrderBézierCurse(pos, distance, 4.5f);

        startPosition = endPosition;
        lastDistance = distance;
    }

    void Clear(RenderTexture destTexture)
    {
        Graphics.SetRenderTarget(destTexture);
        GL.PushMatrix();
        GL.Clear(true, true, Color.clear);
        GL.PopMatrix();
    }

    void DrawBrush(RenderTexture destTexture, int x, int y, Texture sourceTexture, Color color, float scale)
    {
        DrawBrush(destTexture, new Rect(x, y, sourceTexture.width, sourceTexture.height), sourceTexture, color, scale);
    }
    void DrawBrush(RenderTexture destTexture, Rect destRect, Texture sourceTexture, Color color, float scale)
    {

        //增加鼠标位置根据raw图片位置换算。
        float left = (destRect.xMin - rawMousePosition.x) * Screen.width / rawWidth - destRect.width * scale / 2.0f;
        float right = (destRect.xMin - rawMousePosition.x) * Screen.width / rawWidth + destRect.width * scale / 2.0f;
        float top = (destRect.yMin - rawMousePosition.y) * Screen.height / rawHeight - destRect.height * scale / 2.0f;
        float bottom = (destRect.yMin - rawMousePosition.y) * Screen.height / rawHeight + destRect.height * scale / 2.0f;

        // 位置偏移
        //top = top + 250;
        //bottom = bottom + 250;

        Graphics.SetRenderTarget(destTexture);

        GL.PushMatrix();
        GL.LoadOrtho();

        mat.SetTexture("_MainTex", brushTypeTexture);
        mat.SetColor("_Color", color);
        mat.SetPass(0);

        GL.Begin(GL.QUADS);

        //Debug.Log("left / Screen.width : " + left / Screen.width + " top / Screen.height : " + top / Screen.height);
        //Debug.Log("right / Screen.width : " + right / Screen.width + " top / Screen.height : " + top / Screen.height);
        //Debug.Log("right / Screen.width : " + right / Screen.width + " bottom / Screen.height : " + top / Screen.height);
        //Debug.Log("left / Screen.width : " + right / Screen.width + " bottom / Screen.height : " + top / Screen.height);



        GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(left / Screen.width, top / Screen.height, 0);
        GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(right / Screen.width, top / Screen.height, 0);
        GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(right / Screen.width, bottom / Screen.height, 0);
        GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(left / Screen.width, bottom / Screen.height, 0);
        //GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(left / 1000, top / 1000, 0);
        //GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(right / 1000, top / 1000, 0);
        //GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(right / 1000, bottom / 1000, 0);
        //GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(left / 1000, bottom / 1000, 0);

        GL.End();
        GL.PopMatrix();
    }
    bool bshow = true;
    void DrawImage()
    {
        raw.texture = texRender;
    }

    //二阶贝塞尔曲线
    public void TwoOrderBézierCurse(Vector3 pos, float distance)
    {
        PositionArray[a] = pos;
        a++;
        if (a == 3)
        {
            for (int index = 0; index < num; index++)
            {
                Vector3 middle = (PositionArray[0] + PositionArray[2]) / 2;
                PositionArray[1] = (PositionArray[1] - middle) / 2 + middle;

                float t = (1.0f / num) * index / 2;
                Vector3 target = Mathf.Pow(1 - t, 2) * PositionArray[0] + 2 * (1 - t) * t * PositionArray[1] +
                                 Mathf.Pow(t, 2) * PositionArray[2];
                float deltaSpeed = (float)(distance - lastDistance) / num;
                DrawBrush(texRender, (int)target.x, (int)target.y, brushTypeTexture, brushColor, SetScale(lastDistance + (deltaSpeed * index)));
            }
            PositionArray[0] = PositionArray[1];
            PositionArray[1] = PositionArray[2];
            a = 2;
        }
        else
        {
            DrawBrush(texRender, (int)endPosition.x, (int)endPosition.y, brushTypeTexture,
                brushColor, brushScale);
        }
    }
    //三阶贝塞尔曲线，获取连续4个点坐标，通过调整中间2点坐标，画出部分（我使用了num/1.5实现画出部分曲线）来使曲线平滑;通过速度控制曲线宽度。
    private void ThreeOrderBézierCurse(Vector3 pos, float distance, float targetPosOffset)
    {
        //记录坐标
        PositionArray1[b] = pos;
        b++;
        //记录速度
        speedArray[s] = distance;
        s++;
        if (b == 4)
        {
            Vector3 temp1 = PositionArray1[1];
            Vector3 temp2 = PositionArray1[2];

            //修改中间两点坐标
            Vector3 middle = (PositionArray1[0] + PositionArray1[2]) / 2;
            PositionArray1[1] = (PositionArray1[1] - middle) * 1.5f + middle;
            middle = (temp1 + PositionArray1[3]) / 2;
            PositionArray1[2] = (PositionArray1[2] - middle) * 2.1f + middle;

            for (int index1 = 0; index1 < num / 1.5f; index1++)
            {
                float t1 = (1.0f / num) * index1;
                Vector3 target = Mathf.Pow(1 - t1, 3) * PositionArray1[0] +
                                 3 * PositionArray1[1] * t1 * Mathf.Pow(1 - t1, 2) +
                                 3 * PositionArray1[2] * t1 * t1 * (1 - t1) + PositionArray1[3] * Mathf.Pow(t1, 3);
                //float deltaspeed = (float)(distance - lastDistance) / num;
                //获取速度差值（存在问题，参考）
                float deltaspeed = (float)(speedArray[3] - speedArray[0]) / num;

                //模拟毛刺效果
                float randomOffset = UnityEngine.Random.Range(-targetPosOffset, targetPosOffset);
                //float randomOffset = 0;

                DrawBrush(texRender, (int)(target.x + randomOffset), (int)(target.y + randomOffset), brushTypeTexture, brushColor, SetScale(speedArray[0] + (deltaspeed * index1)));
            }

            PositionArray1[0] = temp1;
            PositionArray1[1] = temp2;
            PositionArray1[2] = PositionArray1[3];

            speedArray[0] = speedArray[1];
            speedArray[1] = speedArray[2];
            speedArray[2] = speedArray[3];
            b = 3;
            s = 3;
        }
        else
        {
            DrawBrush(texRender, (int)endPosition.x, (int)endPosition.y, brushTypeTexture,
                brushColor, brushScale);
        }

    }

    //    if (Input.GetMouseButton(0))
    //{
    //    OnMouseMove(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
    //}
    //if (Input.GetMouseButtonUp(0))
    //{
    //    OnMouseUp();
    //}
    //DrawImage();


    public void OnBeginDrag(PointerEventData eventData)
    {
        OnMouseUp();
        OnMouseMove(new Vector3(eventData.position.x, eventData.position.y, 0));

        _writeStatus = WriteStatus.Init;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnMouseMove(new Vector3(eventData.position.x, eventData.position.y, 0));
        OnMouseUp();
        _lastWriteTime = Time.time;
        _writeStatus = WriteStatus.WriteFinished;

    }

    public void OnDrag(PointerEventData eventData)
    {
        OnMouseMove(new Vector3(eventData.position.x, eventData.position.y, 0));
        DrawImage();
        _writeStatus = WriteStatus.Writing;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }
    #endregion




    // 进行识别
    private void Recognizing()
    {
        Debug.Log("Recognizing");

        _writeStatus = WriteStatus.Recognizing;

        RecognizingFun();
    }

    //byte[] bytes = null;
    //private IEnumerator coroutine;

    void RecognizingFun() {

        //  识别功能
        //  将 texture 保存为图片
        string pathDir = Application.streamingAssetsPath + "/temp/ss";

        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = texRender;   //设置当前的 render

        Texture2D newPng = ScaleTexture(raw.texture, 200, 200); // 获取缩放的图片
        SaveBytesToFile(newPng.EncodeToPNG(), pathDir, "new");


        // 加载内容
        Texture2D png = TextureResource.Instance.GetTexture(TextureResource.Screen_Texture) as Texture2D;


        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        sw.Start();
        png.ReadPixels(new Rect(0, 0, texRender.width, texRender.height), 0, 0);
        sw.Stop();
        Debug.Log("Time : " + sw.ElapsedMilliseconds / 1000f);

        sw.Start();
        byte[] bytes = png.EncodeToPNG();  //此步耗费大量时间
        sw.Stop();
        Debug.Log("Time2 : " + sw.ElapsedMilliseconds / 1000f);

        sw.Start();
        string filename = UnityEngine.Random.Range(0, 999).ToString();
        SaveBytesToFile(bytes, pathDir, filename);
        sw.Stop();
        Debug.Log("Time3 : " + sw.ElapsedMilliseconds / 1000f);


        // 进行网络请求
        //string path = pathDir + "/" + filename + ".png";

        //_recogQueuer.AddRecogTask(path, _OnRecognizeError, _OnRecognizeSuccess, () => {
        //    _writeStatus = WriteStatus.RecognizeFinished;
        //});

        _recogQueuer.AddRecogTask(png.EncodeToPNG(), _OnRecognizeError, _OnRecognizeSuccess, () => {
            _writeStatus = WriteStatus.RecognizeFinished;
        });


        // 清理
        //Destroy(png);
        //png = null;


        //Texture2D.DestroyImmediate(png);
        RenderTexture.active = prev;
    }

    private Texture2D ScaleTexture(Texture target, int width, int height)
    {
        RenderTexture rt = new RenderTexture(width, height, 24);
        Graphics.Blit(target, rt);
        var result = TextureResource.Instance.GetTexture(TextureResource.Write_Pad_Texture)as Texture2D;
            
        result.name = "ScaleTextureResult";
        RenderTexture.active = rt;
        result.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        result.Apply();
        RenderTexture.active = null;
        GameObject.Destroy(rt);
        rt = null;
        return result;
    }


    /// <summary>
    ///    将字节保存进文件
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="dir"></param>
    /// <param name="filename"></param>
    private void SaveBytesToFile(byte[] bytes, string dir, string filename) {
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir); // 这步耗时
        string path = dir + "/" + filename + ".png";
        FileStream file = File.Open(path, FileMode.Create);
        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(bytes);
        file.Close();
    }

    /// <summary>
    /// 识别图片
    /// </summary>
    /// <param name="path">全路径</param>
    private void RecognizeImage(string path) {
        var image = File.ReadAllBytes(path);

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

            //string[] strs = {};
            _OnRecognizeSuccess.Invoke(strs);
        }
        catch (Exception ex)
        {
            //  识别错误服务
            Debug.Log("Exception : " + ex.Message);

            _OnRecognizeError(ex.Message);
        }
        finally {
            _writeStatus = WriteStatus.RecognizeFinished;
        }
    }






    // 识别完成
    private void RecognizeComplete()
    {
        _writeStatus = WriteStatus.RecognizeFinished;

        // 清理画布
        Clear(texRender);
        DrawImage();

        _writeStatus = WriteStatus.Init;
    }


    // 装载识别回调
    public void SetOnRecognizedSuccess(Action<string[]> action)
    {
        this._OnRecognizeSuccess = action;
    }

    public void SetOnRecognizedError(Action<string> action)
    {
        this._OnRecognizeError = action;
    }

    //  获取的左下角的屏幕坐标
    private void UpdateRawMousePosition() {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(raw.GetComponent<RectTransform>().position);
        Vector2 rawanchorPositon = new Vector2(screenPos.x - raw.rectTransform.sizeDelta.x / 2.0f
        , screenPos.y - raw.rectTransform.sizeDelta.y / 2.0f);
        rawMousePosition = rawanchorPositon;
    }




    IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        print("WaitAndPrint " + Time.time);
    }

}
