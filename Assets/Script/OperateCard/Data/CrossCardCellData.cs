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
    string _description; // 描述
    string _image; //图片地址
    bool _isImage; // 是图片
    string _videoUrl; // Video Url
    CrossCardCategoryEnum _category;    //  类别
    Texture _imageTexture;
    List<CrossCardCellData> _datas;//二级内容
    MagicWallManager _manager;

    public CrossCardAgent crossCardAgent { set { _crossCardAgent = value; } get { return _crossCardAgent; } }

    public int Id { set { _id = value; } get { return _id; } }

    public int Likes { set { _likes = value; } get { return _likes; } }

    public bool IsImage { set { _isImage = value; } get { return _isImage; } }

    public int EnvId { set { _envid = value; } get { return _envid; } }

    public string Title { set { _title = value; } get { return _title; } }

    public string VideoUrl { set { _videoUrl = value; } get { return _videoUrl; } }

    public string Image { set { _image = value; } get { return _image; } }


    public string Description { set { _description = value; } get { return _description; } }


    public int Index { set { _index = value; } get { return _index; } }

    public CrossCardCategoryEnum Category { set { _category = value; } get { return _category; } }

    public Texture ImageTexture { set { _imageTexture = value; } get { return _imageTexture; } }

    public List<CrossCardCellData> Datas { set { _datas = value; } get { return _datas; } }

    public MagicWallManager magicWallManager { set { _manager = value; } get { return _manager; } }

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