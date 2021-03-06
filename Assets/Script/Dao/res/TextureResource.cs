﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Texture 资源设置器，减少Unity的内存消耗
/// </summary>
namespace MagicWall
{
    public class TextureResource : Singleton<TextureResource>
    {
        public static string Screen_Texture = "ScreenTexture";   // 屏幕大小的Texture
        public static string Write_Pad_Texture = "WritePadTexture";   // 手写板的图片
        public static string Write_Pad_Texture_Big = "WritePadTextureBig";   // 手写板的图片大型

        private Dictionary<string, Texture> _resources;

        public void Add(string address, Texture texture)
        {
            if (_resources == null)
            {
                _resources = new Dictionary<string, Texture>();
            }

            if (_resources.ContainsKey(address))
            {
                _resources.Remove(address);
                _resources.Add(address, texture);
            }
            else
            {
                _resources[address] = texture;
            }
        }

        public Texture GetTexture(string address)
        {
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
                Texture texture = AppUtils.LoadPNGToTexture2D(address);
                Add(address, texture);
                return texture;
            }
        }

    }
}