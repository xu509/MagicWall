using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using UnityEngine.UI;



//
//  联想词代理
//
public class AssociateWordAgent : MonoBehaviour
{
    [SerializeField] Text text;

    Action<string> _OnClickAssociateWord;

    public void SetOnClickWord(Action<string> action) {
        _OnClickAssociateWord = action;
    }

    public void OnClick() {
        _OnClickAssociateWord.Invoke(text.text);
    }

    public void SetText(string str) {
        text.text = str;
    }



}
