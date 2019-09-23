using MagicWall;
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
    string _videoUrl; // 视频地址
    bool _isImage; // 是图片
    MagicWallManager _manager;

    public SliceCardAgent sliceCardAgent { set { _sliceCardAgent = value; } get { return _sliceCardAgent; } }

    public int Id { set { _id = value; } get { return _id; } }

    public int Type { set { _type = value; } get { return _type; } }

    public MagicWallManager magicWallManager { set { _manager = value; } get { return _manager; } }


    public int Index { set { _index = value; } get { return _index; } }


    public int Likes { set { _likes = value; } get { return _likes; } }

    public bool IsImage { set { _isImage = value; } get { return _isImage; } }

    public string Description { set { _description = value; } get { return _description; } }

    public string Image { set { _image = value; } get { return _image; } }

    public string VideoUrl { set { _videoUrl = value; } get { return _videoUrl; } }


    public bool IsProduct (){
        return _type == 0;
    }

    public bool IsActivity()
    {
        return _type == 1;
    }

    public void LoadDetail(ScrollData scrollData)
    {

        _id = scrollData.Type;
        _description = scrollData.Description;
        _likes = _manager.daoService.GetLikes(scrollData.Cover);
        _image = scrollData.Cover;
        _type = 0;
        _isImage = scrollData.Type == 0;

        if (!_isImage)
        {
            _videoUrl = scrollData.Src;
        }

    }


    public void LoadProductDetail(ProductDetail productDetail) {

        _id = productDetail.Id;
        _description = productDetail.Description;
        _likes = _manager.daoService.GetLikes(productDetail.Image);
        _image = productDetail.Image;
        _type = 0;
        _isImage = productDetail.IsImage();

        if (!_isImage) {
            _videoUrl = productDetail.VideoUrl;
        }

    }

    public void LoadActivityDetail(ActivityDetail activityDetail)
    {
        _id = activityDetail.Id;
        _isImage = activityDetail.IsImage();
        _description = activityDetail.Description;
        _likes = _manager.daoService.GetLikes(activityDetail.Image);
        _image = activityDetail.Image;
        _type = 1;

        if (!_isImage)
        {
            _videoUrl = activityDetail.VideoUrl;
        }
    }


    public override string ToString() {
        string str = "";

               
        return str;
    }


}