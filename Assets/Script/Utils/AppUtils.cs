﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AppUtils : MonoBehaviour
{
  
    public static float ConvertToFloat(string str) {
        float r = 0f;

        if (!float.TryParse(str, out r)) {
            r = 0f;
        }
        return r;
    }

    public static Texture LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(100, 100);
            bool t = tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        else {
            Debug.Log("File is not found : " + filePath);
        }
        return tex;
    }

    public static Vector2 ResetTexture(Vector2 size,float displayFactor)
    {
        //图片宽高
        float w = size.x;
        float h = size.y;
        //组件宽高
        float width;
        float height;
        //if (w > 600 || h > 400)
        //{
        //    w *= 0.9f;
        //    h *= 0.9f;
        //    ResetTexture(new Vector2(w, h));
        //}
        if (w >= h)
        {
            //宽固定
            width = Random.Range(300 * displayFactor, 500 * displayFactor);
            height = h / w * width;
        }
        else
        {
            //高固定
            height = Random.Range(200 * displayFactor, 400 * displayFactor);
            width = w / h * height;
        }
        width = (int)width;
        height = (int)height;
        Debug.Log("width:" + width+ "height:" + height);
        return new Vector2(width, height);
    }

}
