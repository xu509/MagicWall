using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall
{
    public class ThemeManager : MonoBehaviour
    {
        [SerializeField,Header("当前主题")] ThemeEnum _theme;

        [SerializeField,Header("Component")] ThemeFactory _themeFactory;

        public void Init() { 
            
        }


        public IThemeService GetService() {
            return _themeFactory.GetService(_theme);            
        }


    }
}