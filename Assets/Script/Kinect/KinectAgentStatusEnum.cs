using UnityEngine;

namespace MagicWall
{

    /// <summary>
    ///   体感实体状态
    ///   ref : https://www.yuque.com/u314548/fc6a5l/mnxg3w
    /// </summary>
    public enum KinectAgentStatusEnum
    {       
        Creating,Normal,
        WaitingHiding,
        Hiding,Hide,Destoring,Obsolete,Small,Recovering
    }

}
