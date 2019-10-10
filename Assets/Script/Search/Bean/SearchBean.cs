using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 搜索结果bean
namespace MagicWall
{
    public class SearchBean : Generator<SearchBean>
    {
        private int _id; // id
        private DataTypeEnum _type;  // 类型 
        private string _cover; // 封面

        public int id { set { _id = value; } get { return _id; } }

        public DataTypeEnum type { set { _type = value; } get { return _type; } }

        public string cover { set { _cover = value; } get { return _cover; } }

        public SearchBean Generator()
        {
            SearchBean bean = new SearchBean();
            bean.id = UnityEngine.Random.Range(1, 10);

            DataTypeEnum[] typeEnums = Enum.GetValues(typeof(DataTypeEnum)) as DataTypeEnum[];
            bean.type = typeEnums[UnityEngine.Random.Range(0, typeEnums.Length)];

            string[] covers = { "activity\\1.png", "activity\\2.png", "activity\\3.png"
                ,"env\\business-card-1.png","env\\business-card-2.png","env\\business-card-3.png"
                ,"product\\1.png","product\\2.png","product\\3.png"};

            bean.cover = covers[UnityEngine.Random.Range(0, covers.Length)];

            return bean;
        }
    }
}