using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall {

    /// <summary>
    ///   kinect 服务类
    /// </summary>
    public class KinectService : MonoBehaviour, IKinectService
    {
        public void Init(RectTransform container, KinectAgent agentPrefab)
        {
            throw new NotImplementedException();
        }

        public void Monitoring()
        {
            throw new NotImplementedException();
        }

        public void StartMonitoring(Action startSuccessAction, Action<string> startFailedAction)
        {
            throw new NotImplementedException();
        }

        public void StopMonitoring()
        {
            throw new NotImplementedException();
        }
    }

}
