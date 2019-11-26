using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using UnityEngine.UI;



//
//  联想词代理
//
namespace MagicWall
{
    public class AssociateWordAgent : MonoBehaviour
    {
        [SerializeField] Text text;

        Action<string> _OnClickAssociateWord;

        MagicWallManager _manager;


        public void Init() {
            _manager = GameObject.Find("MagicWall").GetComponent<MagicWallManager>();
            InitUI();
        }



        public void SetOnClickWord(Action<string> action)
        {
            _OnClickAssociateWord = action;
        }



        public void OnClick()
        {
            _OnClickAssociateWord.Invoke(text.text);
        }

        public void SetText(string str)
        {
            text.text = str;
        }

        private void InitUI() {
            text.color = _manager.themeManager.GetService().GetFontColor();
        }


    }
}