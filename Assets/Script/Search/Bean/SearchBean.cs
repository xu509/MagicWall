using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 搜索结果bean
public class SearchBean : Generator<SearchBean>
{
    private int _id; // id
    private MWTypeEnum _type;  // 类型 
    private string _cover; // 封面

    public int id { set { _id = value; } get { return _id; } }

    public MWTypeEnum type { set { _type = value; } get { return _type; } }

    public string cover { set { _cover = value; } get { return _cover; } }

    public SearchBean Generator()
    {
        SearchBean bean = new SearchBean();
        bean.id = UnityEngine.Random.Range(1, 10);

        MWTypeEnum[] typeEnums = Enum.GetValues(typeof(MWTypeEnum)) as MWTypeEnum[];
        bean.type = typeEnums[UnityEngine.Random.Range(0, typeEnums.Length)];

        string[] covers = { "activity\\1.jpg", "activity\\2.jpg", "activity\\3.jpg"
                ,"env\\business-card-1.jpg","env\\business-card-2.jpg","env\\business-card-3.jpg"
                ,"product\\1.jpg","product\\2.jpg","product\\3.jpg"};

        bean.cover = covers[UnityEngine.Random.Range(0, covers.Length)];

        return bean;
    }
}
