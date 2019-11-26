using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall
{
    public class ThemeFactory : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] WhiteThemeService _whiteThemeService;
        [SerializeField] BlackThemeService _blackThemeService;

        public IThemeService GetService(ThemeEnum themeEnum) {
            if (themeEnum == ThemeEnum.Black)
            {
                return _blackThemeService;
            }
            else if (themeEnum == ThemeEnum.White)
            {
                return _whiteThemeService;
            }
            else {
                return null;
            }
        }


    }
}