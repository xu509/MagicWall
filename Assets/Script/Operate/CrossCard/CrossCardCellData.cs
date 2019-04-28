using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossCardCellData
{
    int _index;
    string _title;
    float _postion;

    public string Title { set { _title = value; } get { return _title; } }
    public int Index { set { _index = value; } get { return _index; } }

    public float Postion { set { _postion = value; } get { return _postion; } }


    public CrossCardCellData(int index, string title)
    {
        _index = index;
        _title = title;
    }

}