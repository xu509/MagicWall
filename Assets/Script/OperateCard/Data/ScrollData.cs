

namespace MagicWall{


    /// <summary>
    /// https://www.yuque.com/docs/share/3b959a84-828e-4401-b735-f80532d2376f
    /// </summary>
    public class ScrollData
    {
        int type; // 0 : 图片； 1： 视频
        string cover;
        string src;
        string description;


        public int Type { set { type = value; } get { return type; } } 
        public string Cover { set { cover = value; } get { return cover; } } 
        public string Src { set { src = value; } get { return src; } } 
        public string Description { set { description = value; } get { return description; } } 


    }
}


