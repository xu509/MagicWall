using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Texture 资源设置器，减少Unity的内存消耗
/// </summary>
public class SpriteResource : Singleton<SpriteResource>
{
    public static string Screen_Texture = "ScreenTexture";   // 屏幕大小的Texture
    public static string Write_Pad_Texture = "WritePadTexture";   // 手写板的图片
    public static string Write_Pad_Texture_Big = "WritePadTextureBig";   // 手写板的图片大型


    private Dictionary<string, Sprite> _resources;

    public void Add(string address, Sprite sprite) {
        if (_resources == null) {
            _resources = new Dictionary<string, Sprite>();
        }

        if (_resources.ContainsKey(address))
        {
            _resources.Remove(address);
            _resources.Add(address, sprite);
        }
        else {
            _resources[address] = sprite;
        }
    }

    public Sprite GetData(string address) {
        if (_resources == null)
        {
            _resources = new Dictionary<string, Sprite>();
        }

        if (_resources.ContainsKey(address))
        {
            return _resources[address];
        }
        else
        {
            Texture2D texture = AppUtils.LoadPNGToTexture2D(address);
            texture.filterMode = FilterMode.Bilinear;
            Sprite sprite = Sprite.Create(texture,new Rect(0,0,texture.width,texture.height), Vector2.zero);
            Add(address, sprite);
            return sprite;
        }
    }

}
