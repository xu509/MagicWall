using System.Collections;
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
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.

        }
        else {
            Debug.Log("File is not found : " + filePath);
        }
        return tex;
    }

}
