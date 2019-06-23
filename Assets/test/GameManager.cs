using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    private RenderTexture texRender;

    public Material mat;
    public Texture brushTypeTexture;   //画笔纹理，半透明

    public Material lineMaterial;
    public float lineWidth;
    public float depth = 5;
    public RawImage raw;

    public RawImage testImage;

    // pri
    private Camera mainCamera;
    private float brushScale = 0.5f;
    public Color brushColor = Color.black;

    // 
    private float lastDistance;
    private Vector3[] PositionArray = new Vector3[3];
    private int a = 0;
    private Vector3[] PositionArray1 = new Vector3[4];
    private int b = 0;
    private float[] speedArray = new float[4];
    private int s = 0;
    public int num = 50;


    Vector2 rawMousePosition;       //raw图片的左下角对应鼠标位置
    float rawWidth;                               //raw图片宽度
    float rawHeight;                              //raw图片长度



    private Vector3? lineStartPoint = null;

    // Start is called before the first frame update
    void Start()
    {

        //raw图片鼠标位置，宽度计算
        rawWidth = raw.rectTransform.sizeDelta.x;
        rawHeight = raw.rectTransform.sizeDelta.y;
        Vector2 rawanchorPositon = new Vector2(raw.rectTransform.anchoredPosition.x - raw.rectTransform.sizeDelta.x / 2.0f, raw.rectTransform.anchoredPosition.y - raw.rectTransform.sizeDelta.y / 2.0f);
        rawMousePosition = rawanchorPositon + new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);

        Debug.Log("rawMousePosition : " + rawMousePosition);


        //texRender = new RenderTexture(1000, 1000, 24, RenderTextureFormat.ARGB32);
        texRender = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        Clear(texRender);

    }


    Vector3 startPosition = Vector3.zero;
    Vector3 endPosition = Vector3.zero;
    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            OnMouseMove(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnMouseUp();
        }
        DrawImage();


        //texRender.Create();

    }




    void OnMouseUp()
    {
        startPosition = Vector3.zero;
        //brushScale = 0.5f;
        a = 0;
        b = 0;
        s = 0;
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
        GL.Clear(true, true, Color.white);
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

        Graphics.SetRenderTarget(destTexture);

        GL.PushMatrix();
        GL.LoadOrtho();

        mat.SetTexture("_MainTex", brushTypeTexture);
        mat.SetColor("_Color", color);
        mat.SetPass(0);

        GL.Begin(GL.QUADS);

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
    public void OnClickClear()
    {
        Clear(texRender);
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
                //float randomOffset = Random.Range(-1/(speedArray[0] + (deltaspeed * index1)), 1 / (speedArray[0] + (deltaspeed * index1)));
                //模拟毛刺效果
                float randomOffset = Random.Range(-targetPosOffset, targetPosOffset);
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

    public void test() {

        //byte[] bytes = ImageConversion.EncodeToPNG((Texture2D)raw.texture);
        //GeneralBasicDemo(bytes);

        //testImage.texture

        //Texture2D myTexture = toTexture2D(texRender);

        FileStream stream = null ;
        byte[] pReadByte = new byte[0];

        try
        {
            stream = new FileStream(Application.dataPath + "/test/test.png", FileMode.Open);
            BinaryReader r = new BinaryReader(stream);
            r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
            pReadByte = r.ReadBytes((int)r.BaseStream.Length);
        }
        catch { }
        finally
        {
            if (stream != null)
                stream.Close();
        }


        GeneralBasicDemo(pReadByte);
        //Debug.Log(raw.texture);
    }


    public void GeneralBasicDemo(byte[] bytes)
    {
        Debug.Log(bytes.Length);
        Debug.Log(Application.dataPath + "/test/SavedScreen.png");

        File.WriteAllBytes(Application.dataPath + "/test/SavedScreen.png", bytes);



        var APP_ID = "15887702";
        var API_KEY = "H077BtXG2KUbp2AgMjIVt0UV";
        var SECRET_KEY = "bolOO638ObtBd1ovTHdGj6d5LkhG44b9";

        var client = new Baidu.Aip.Ocr.Ocr(API_KEY, SECRET_KEY);
        client.Timeout = 60000;  // 修改超时时间


        //var image = File.ReadAllBytes("图片文件路径");
        // 调用通用文字识别, 图片参数为本地图片，可能会抛出网络等异常，请使用try/catch捕获
        //var result = client.GeneralBasic(image);
        var result = client.GeneralBasic(bytes);
        var options = new Dictionary<string, object>{
        {"language_type", "CHN_ENG"},
        {"detect_direction", "true"},
        {"detect_language", "true"},
        {"probability", "true"}
        };
        Debug.Log(result);
        // 如果有可选参数
        //var options = new Dictionary<string, object>{
        //{"language_type", "CHN_ENG"},
        //{"detect_direction", "true"},
        //{"detect_language", "true"},
        //{"probability", "true"}
        //};
        //// 带参数调用通用文字识别, 图片参数为本地图片
        //result = client.GeneralBasic(image, options);
        //Console.WriteLine(result);
    }


    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }


}
