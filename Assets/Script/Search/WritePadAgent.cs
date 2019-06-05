using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WritePadAgent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] RawImage _renderImage;
    [SerializeField] Material mat;     //给定的shader新建材质
    [SerializeField] Color brushColor = Color.black;
    [SerializeField] Texture brushTypeTexture;   //画笔纹理，半透明

    private RenderTexture _texRender;
    private Vector3 _startPosition; // 笔记开始位置
    private Vector3 _endPosition; // 笔记结束位置
    private float brushScale = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        float w = _renderImage.GetComponent<RectTransform>().rect.width;
        float h = _renderImage.GetComponent<RectTransform>().rect.height;

        Debug.Log("Image Width :" + w + " - Height :" + h);


        _texRender = new RenderTexture((int)w, (int)h, 24, RenderTextureFormat.ARGB32);
        Clear(_texRender);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("WritePadAgent :On Begin Drag ");
        _startPosition = eventData.position;


    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("WritePadAgent :On Drag ");
        _endPosition = eventData.position;

        // 画线
        Graphics.SetRenderTarget(_texRender);


        // GL库 渲染入栈
        GL.PushMatrix();
        GL.LoadOrtho();

        mat.SetTexture("_MainTex", brushTypeTexture);
        mat.SetColor("_Color", brushColor);
        mat.SetPass(0);

        //GL.Begin(GL.QUADS);


        //GL.Vertex3(_startPosition.x / Screen.width, _startPosition.y / Screen.height, 0);
        //GL.Vertex3(_endPosition.x / Screen.width, _endPosition.y / Screen.height, 0);

        GL.Begin(GL.QUADS);
        GL.Vertex3(0, 1, 0);
        GL.Vertex3(1, 0, 0);

        //GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(eventData.position.x, eventData.position.y, 0);
        //GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(eventData.position.x, eventData.position.y, 0);
        //GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(eventData.position.x, eventData.position.y, 0);
        //GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(eventData.position.x, eventData.position.y, 0);


        GL.End();
        // 渲染出栈
        GL.PopMatrix();

        _startPosition = _endPosition;


        DrawImage();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("WritePadAgent :On End Drag ");
    }



    // Update is called once per frame
    void Update()
    {
        
    }



    void Clear(RenderTexture destTexture)
    {
        Graphics.SetRenderTarget(destTexture);
        GL.PushMatrix();
        GL.Clear(true, true, Color.white);
        GL.PopMatrix();
    }

    void DrawImage() {
        _renderImage.texture = _texRender;
    }


    // 根据笔记绘画
    //void DrawBrush(RenderTexture destTexture, int x, int y, Texture sourceTexture, Color color, float scale)
    //{
    //    DrawBrush(destTexture, new Rect(x, y, sourceTexture.width, sourceTexture.height), sourceTexture, color, scale);
    //}

    //void DrawBrush(RenderTexture destTexture, Rect destRect, Texture sourceTexture, Color color, float scale)
    //{

    //    //增加鼠标位置根据raw图片位置换算。
    //    float left = (destRect.xMin - rawMousePosition.x) * Screen.width / rawWidth - destRect.width * scale / 2.0f;
    //    float right = (destRect.xMin - rawMousePosition.x) * Screen.width / rawWidth + destRect.width * scale / 2.0f;
    //    float top = (destRect.yMin - rawMousePosition.y) * Screen.height / rawHeight - destRect.height * scale / 2.0f;
    //    float bottom = (destRect.yMin - rawMousePosition.y) * Screen.height / rawHeight + destRect.height * scale / 2.0f;

    //    Graphics.SetRenderTarget(destTexture);

    //    GL.PushMatrix();
    //    GL.LoadOrtho();

    //    mat.SetTexture("_MainTex", brushTypeTexture);
    //    mat.SetColor("_Color", color);
    //    mat.SetPass(0);

    //    GL.Begin(GL.QUADS);

    //    GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(left / Screen.width, top / Screen.height, 0);
    //    GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(right / Screen.width, top / Screen.height, 0);
    //    GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(right / Screen.width, bottom / Screen.height, 0);
    //    GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(left / Screen.width, bottom / Screen.height, 0);

    //    GL.End();
    //    GL.PopMatrix();
    //}

    //三阶贝塞尔曲线，获取连续4个点坐标，通过调整中间2点坐标，画出部分（我使用了num/1.5实现画出部分曲线）来使曲线平滑;通过速度控制曲线宽度。
    //private void ThreeOrderBézierCurse(Vector3 pos, float distance, float targetPosOffset)
    //{
    //    //记录坐标
    //    PositionArray1[b] = pos;
    //    b++;
    //    //记录速度
    //    speedArray[s] = distance;
    //    s++;
    //    if (b == 4)
    //    {
    //        Vector3 temp1 = PositionArray1[1];
    //        Vector3 temp2 = PositionArray1[2];

    //        //修改中间两点坐标
    //        Vector3 middle = (PositionArray1[0] + PositionArray1[2]) / 2;
    //        PositionArray1[1] = (PositionArray1[1] - middle) * 1.5f + middle;
    //        middle = (temp1 + PositionArray1[3]) / 2;
    //        PositionArray1[2] = (PositionArray1[2] - middle) * 2.1f + middle;

    //        for (int index1 = 0; index1 < num / 1.5f; index1++)
    //        {
    //            float t1 = (1.0f / num) * index1;
    //            Vector3 target = Mathf.Pow(1 - t1, 3) * PositionArray1[0] +
    //                             3 * PositionArray1[1] * t1 * Mathf.Pow(1 - t1, 2) +
    //                             3 * PositionArray1[2] * t1 * t1 * (1 - t1) + PositionArray1[3] * Mathf.Pow(t1, 3);
    //            //float deltaspeed = (float)(distance - lastDistance) / num;
    //            //获取速度差值（存在问题，参考）
    //            float deltaspeed = (float)(speedArray[3] - speedArray[0]) / num;
    //            //float randomOffset = Random.Range(-1/(speedArray[0] + (deltaspeed * index1)), 1 / (speedArray[0] + (deltaspeed * index1)));
    //            //模拟毛刺效果
    //            float randomOffset = Random.Range(-targetPosOffset, targetPosOffset);
    //            DrawBrush(_texRender, (int)(target.x + randomOffset), (int)(target.y + randomOffset), brushTypeTexture, brushColor, SetScale(speedArray[0] + (deltaspeed * index1)));
    //        }

    //        PositionArray1[0] = temp1;
    //        PositionArray1[1] = temp2;
    //        PositionArray1[2] = PositionArray1[3];

    //        speedArray[0] = speedArray[1];
    //        speedArray[1] = speedArray[2];
    //        speedArray[2] = speedArray[3];
    //        b = 3;
    //        s = 3;
    //    }
    //    else
    //    {
    //        DrawBrush(_texRender, (int)endPosition.x, (int)endPosition.y, brushTypeTexture,
    //            brushColor, brushScale);
    //    }
    //}





}
