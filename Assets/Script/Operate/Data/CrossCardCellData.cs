using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossCardCellData
{
    CrossCardAgent _crossCardAgent;

    int _envid; // envid
    int _id; // id
    int _index;     //  索引
    int _likes; //喜欢数
    string _title;  //  标题
    CrossCardCategoryEnum _category;    //  类别
    Texture _imageTexture;
    List<CrossCardCellData> _datas;//二级内容

    public CrossCardAgent crossCardAgent { set { _crossCardAgent = value; } get { return _crossCardAgent; } }

    public int Id { set { _id = value; } get { return _id; } }

    public int Likes { set { _likes = value; } get { return _likes; } }


    public int EnvId { set { _envid = value; } get { return _envid; } }

    public string Title { set { _title = value; } get { return _title; } }

    public int Index { set { _index = value; } get { return _index; } }

    public CrossCardCategoryEnum Category { set { _category = value; } get { return _category; } }

    public Texture ImageTexture { set { _imageTexture = value; } get { return _imageTexture; } }

    public List<CrossCardCellData> Datas { set { _datas = value; } get { return _datas; } }

    public override string ToString() {
        string str = "";

        str += "Id : " + _id;
        str += "Env Id : " + _envid;
        str += "Index : " + _index;
        str += "title : " + _title;
        str += "category : " + _category;
        str += "Image Texture is Null : " + _imageTexture == null;



        return str;
    }


}