using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossCardCellData
{
    int _id; // 标题
    int _index;     //  索引
    string _title;  //  标题
    CrossCardCategoryEnum _category;    //  类别
    Texture _imageTexture;
    List<CrossCardCellData> _datas;//二级内容

    public int Id { set { _id = value; } get { return _id; } }

    public string Title { set { _title = value; } get { return _title; } }

    public int Index { set { _index = value; } get { return _index; } }

    public CrossCardCategoryEnum Category { set { _category = value; } get { return _category; } }

    public Texture ImageTexture { set { _imageTexture = value; } get { return _imageTexture; } }

    public List<CrossCardCellData> Datas { set { _datas = value; } get { return _datas; } }



}