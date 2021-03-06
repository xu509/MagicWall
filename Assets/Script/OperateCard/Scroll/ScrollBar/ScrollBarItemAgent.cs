﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MagicWall {
    public class ScrollBarItemAgent : MonoBehaviour
    {
        [SerializeField] Text _text;

        MagicWallManager _manager;

        public void Awake() {
            MagicWallManager manager = GameObject.Find("MagicWall").GetComponent<MagicWallManager>();

            _manager = manager;

            InitUI();
        }



        public void Init(CrossCardNavType navType)
        {
            gameObject.name = "scrollbar-" + navType.ToString();

            string text;

            if (navType == CrossCardNavType.Index)
            {
                text = "企业名片";
            }
            else if (navType == CrossCardNavType.Activity)
            {
                text = "活动";
            }
            else if (navType == CrossCardNavType.Product)
            {
                text = "产品";
            }
            else if (navType == CrossCardNavType.CataLog)
            {
                text = "CataLog";
            }
            else if (navType == CrossCardNavType.Video)
            {
                text = "视频";
            }
            else {
                text = "";
            }

            _text.text = text;
        }


        private void InitUI() {
            if (_manager.screenTypeEnum == ScreenTypeEnum.Screen1080P)
            {
                _text.fontSize = 26;
            }
            else
            {
                _text.fontSize = 24;
            }

            _text.color = _manager.themeManager.GetService().GetFontColor();

        }

    }

}
