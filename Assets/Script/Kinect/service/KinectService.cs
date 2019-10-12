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
        MagicWallManager _manager;

        public void Init(RectTransform container, KinectAgent agentPrefab,MagicWallManager manager)
        {
            throw new NotImplementedException();
        }

        public void Monitoring()
        {
            throw new NotImplementedException();

            // 生成新实体
            KinectAgent kinectAgent;

            // 添加至移动模块
            _manager.collisionManager.AddCollisionEffectAgent(kinectAgent);

            // 删除
            _manager.collisionManager.RemoveCollisionEffectAgent(kinectAgent);



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
