using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BusinessCardData 
{

    private Texture _image;
    private int _index;
    private string _address;

    public Texture Image { set { _image = value; } get { return _image; } }

    public int Index { set { _index = value; } get { return _index; } }

    public string address { set { _address = value; } get { return _address; } }


}


