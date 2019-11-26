using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall
{
    public interface IThemeService 
    {
        /// <summary>
        ///    字体颜色
        /// </summary>
        /// <returns></returns>
        Color GetFontColor();

        /// <summary>
        ///     获取背景
        /// </summary>
        /// <returns></returns>
        Sprite GetBackSprite();
        
    }
}