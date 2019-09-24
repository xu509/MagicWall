

namespace MagicWall{

    /// <summary>
    ///     操作卡片数据基类
    ///     ref: https://www.yuque.com/u314548/fc6a5l/vcvbw9#VtDhb
    /// </summary>
    public class OperateCardData
    {
        int id;
        DataTypeEnum dataType;
        string title;
        string description;
        string cover;

        public int Id { set { id = value; } get { return id; } }
        public DataTypeEnum DataType { set { dataType = value; } get { return dataType; } }
        public string Title { set { title = value; } get { return title; } }
        public string Description { set { description = value; } get { return description; } }
        public string Cover { set { cover = value; } get { return cover; } }

        public override string ToString()
        {
            string str = "id : " + id + " ,"
                + "dataType: " + dataType.ToString() + ","
                + "title: " + title + ","
                + "description: " + description + ","
                + "cover: " + cover;
            return str;
        }


    }
}


