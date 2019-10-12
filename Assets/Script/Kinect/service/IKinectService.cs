using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall {

    /// <summary>
    /// ref : https://www.yuque.com/books/share/4f5397bb-9ecf-4622-bf62-f812a38d2057
    /// </summary>
    public interface IKinectService
    {
        void Init(RectTransform container,KinectAgent agentPrefab, MagicWallManager manager);

        void StartMonitoring(Action startSuccessAction, Action<string> startFailedAction);

        void StopMonitoring();

        void Monitoring();
    }

}
