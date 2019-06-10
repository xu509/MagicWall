using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Texture 资源设置器，减少Unity的内存消耗
/// </summary>
public class TextureResource : Singleton<TextureResource>
{
    private Dictionary<string, Texture> _resources;

    public void Add(string address, Texture texture) {
        if (_resources == null) {
            _resources = new Dictionary<string, Texture>();
        }

        if (_resources.ContainsKey(address))
        {
            _resources.Add(address, texture);
        }
        else {
            _resources[address] = texture;
        }
    }

    public Texture GetTexture(string address) {
        if (_resources == null)
        {
            _resources = new Dictionary<string, Texture>();
        }

        if (_resources.ContainsKey(address))
        {
            return _resources[address];
        }
        else
        {
            Debug.Log("Address : " + address);


            Texture texture = AppUtils.LoadPNG(address);
            Add(address, texture);
            return texture;
        }



    }


}
