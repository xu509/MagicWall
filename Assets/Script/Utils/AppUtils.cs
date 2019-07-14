using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AppUtils
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
            tex = new Texture2D(600, 600);
            bool t = tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        else {
            Debug.Log("File is not found : " + filePath);
        }
        return tex;
    }

    public static Texture2D LoadPNGToTexture2D(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(400, 400);

            bool t = tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        else
        {
            Debug.Log("File is not found : " + filePath);
        }
        return tex;
    }



    /// <summary>
    /// 获取完整的图片路径
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
    public static string GetFullFileAddressOfImage(string filepath) {
        return MagicWallManager.FileDir + filepath;
    }

    /// <summary>
    ///     根据固定高度，获取 sprite 的宽
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static float GetSpriteWidthByHeight(Sprite sprite ,float height) {
        float imageWidth = sprite.rect.width;
        float imageHeight = sprite.rect.height;

        return imageWidth / (float)imageHeight * height;
    }

    /// <summary>
    ///     根据固定宽度，获取 sprite 的高
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    public static float GetSpriteHeightByWidth(Sprite sprite, float width)
    {
        float imageWidth = sprite.rect.width;
        float imageHeight = sprite.rect.height;
        return width / imageWidth * imageHeight;
    }

    /// <summary>
    ///     检查向量是否重合
    /// </summary>
    /// <returns></returns>
    public static bool CheckVectorIsEqual(Vector2 position , Vector2 to) {
        // 误差度设置为2个小数点

        int position_x = Mathf.RoundToInt(position.x * 100);
        int position_y = Mathf.RoundToInt(position.y * 100);

        int to_x = Mathf.RoundToInt(to.x * 100);
        int to_y = Mathf.RoundToInt(to.y * 100);

        if ((position_x == to_x) && (position_y == to_y))
        {
            return true;
        }
        else {
            return false;
        }
    }



}
