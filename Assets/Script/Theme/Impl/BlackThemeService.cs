using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall {
    public class BlackThemeService : MonoBehaviour, IThemeService
    {
        [SerializeField, Header("整体背景图")] Sprite _backgroundSprite;
        [SerializeField, Header("卡片遮罩图片")] Sprite _cardBackShadeSprite;

        [SerializeField, Header("Move组件")] Sprite _moveTop;
        [SerializeField] Sprite _moveDown;
        [SerializeField] Sprite _moveLeft;
        [SerializeField] Sprite _moveRight;

        [SerializeField, Header("Scroll Bar")] Sprite _scrollBarSprite;


        public Sprite GetBackSprite()
        {
            return _backgroundSprite;
        }

        public Sprite GetCardBackShade(FlockCardTypeEnum flockCardTypeEnum)
        {
            return _cardBackShadeSprite;
        }

        public Color GetFontColor()
        {
            return Color.white;
        }

        public Sprite GetMoveAgentSprite(MoveAgentTypeEnum moveAgentTypeEnum)
        {
            if (moveAgentTypeEnum == MoveAgentTypeEnum.UP)
            {
                return _moveTop;
            }
            else if (moveAgentTypeEnum == MoveAgentTypeEnum.DOWN) {
                return _moveDown;
            }
            else if (moveAgentTypeEnum == MoveAgentTypeEnum.LEFT)
            {
                return _moveLeft;
            }
            else 
            {
                return _moveRight;
            }

        }

        public Sprite GetScrollBarSprite()
        {
            return _scrollBarSprite;
        }
    }
}