using System.Collections.Generic;


namespace MagicWall {

    /// <summary>
    /// https://www.yuque.com/docs/share/3b959a84-828e-4401-b735-f80532d2376f
    /// 十字卡片
    /// </summary>
    public class OperateCardDataCross : OperateCardData
    {
        Dictionary<CrossCardNavType, List<ScrollData>> scrollDic;

        public Dictionary<CrossCardNavType, List<ScrollData>> ScrollDic { set { scrollDic = value; } get { return scrollDic; } }

    }

}
