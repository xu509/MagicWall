using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;



/// <summary>
/// Texture 资源设置器，减少Unity的内存消耗
/// </summary>
public class VideoResource : Singleton<VideoResource>
{
    private Dictionary<string, VideoClip> _resources;

    public void Add(string address, VideoClip videoClip) {
        if (_resources == null) {
            _resources = new Dictionary<string, VideoClip>();
        }

        if (_resources.ContainsKey(address))
        {
            _resources.Add(address, videoClip);
        }
        else {
            _resources[address] = videoClip;
        }
    }

    public VideoClip GetTexture(string address) {
        if (_resources == null)
        {
            _resources = new Dictionary<string, VideoClip>();
        }

        if (_resources.ContainsKey(address))
        {
            return _resources[address];
        }
        else
        {
            //Texture texture = AppUtils.LoadPNG(address);
            //Add(address, texture);
            //return texture;
            return null;
        }



    }


}
