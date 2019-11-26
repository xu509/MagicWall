using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall {
    public class BlackThemeService : MonoBehaviour, IThemeService
    {
        [SerializeField] Sprite _backgroundSprite;


        public Sprite GetBackSprite()
        {
            return _backgroundSprite;
        }

        public Color GetFontColor()
        {
            throw new System.NotImplementedException();
        }
    }
}