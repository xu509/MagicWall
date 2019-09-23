using System.Collections.Generic;

namespace MagicWall {

    /// <summary>
    /// 滑动卡片
    /// </summary>
    public class OperateCardDataSlide : OperateCardData
    {
        private List<ScrollData> scrollData;

        public List<ScrollData> ScrollData { set { scrollData = value; } get { return scrollData; } }

        private List<ExtraCardData> extraCardData;

        public List<ExtraCardData> ExtraCardData { set { extraCardData = value; } get { return extraCardData; } }


    }

}
