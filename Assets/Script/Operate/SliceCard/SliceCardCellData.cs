using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceCardCellData
{
    SliceCardAgent _sliceCardAgent;

    int _index;

    int _type; // 类型，0：产品；1活动；

    int _id; // id
    int _likes; //喜欢数
    string _image;  //  图片地址
    string _description; // 描述
    bool _isImage; // 是图片

    public SliceCardAgent sliceCardAgent { set { _sliceCardAgent = value; } get { return _sliceCardAgent; } }

    public int Id { set { _id = value; } get { return _id; } }

    public int Type { set { _type = value; } get { return _type; } }


    public int Index { set { _index = value; } get { return _index; } }


    public int Likes { set { _likes = value; } get { return _likes; } }

    public bool IsImage { set { _isImage = value; } get { return _isImage; } }

    public string Description { set { _description = value; } get { return _description; } }

    public string Image { set { _image = value; } get { return _image; } }

    public bool IsProduct (){
        return _type == 0;
    }

    public bool IsActivity()
    {
        return _type == 1;
    }


    public void LoadProductDetail(ProductDetail productDetail) {
        _id = productDetail.Id;
        _isImage = true;
        _description = productDetail.Description;
        _likes = DaoService.Instance.GetLikesByProductDetail(_id);
        _image = productDetail.Image;
        _type = 0;
    }

    public void LoadActivityDetail(ActivityDetail activityDetail)
    {
        _id = activityDetail.Id;
        _isImage = true;
        _description = activityDetail.Description;
        _likes = DaoService.Instance.GetLikesByActivityDetail(_id);
        _image = activityDetail.Image;
        _type = 1;
    }


    public override string ToString() {
        string str = "";

               
        return str;
    }


}