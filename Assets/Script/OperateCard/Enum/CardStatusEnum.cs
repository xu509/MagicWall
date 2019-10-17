

namespace MagicWall {


    /// <summary>
    ///  卡片状态 
    ///  ref ： https://www.yuque.com/docs/share/0b83cf36-3400-4544-8ffe-f3b8f9f78811
    /// </summary>

    public enum CardStatusEnum
    {   
        GENERATE, // 生成中
        NORMAL, // 正常
        DESTORYINGFIRST, // 销毁动画中(第一次)
        DestoryFirstCompleted, // 第一次销毁完成
        RECOVER,    // 恢复动画中
        DESTORYINGSECOND, // 进行第二次动画中
        OBSOLETE, // 废弃的
        HIDE, // 隐藏中
        MOVE    // 移动中
    }

}
