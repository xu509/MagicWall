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
        RectTransform _parentRectTransform;
        GameObject _kinectAgentPrefab;
        MagicWallManager _manager;

        KinectManager kinectManager;
        Dictionary<long, GameObject> userAndAgents;
        public void Init(RectTransform container, KinectAgent agentPrefab,MagicWallManager manager)
        {
            _parentRectTransform = container;
            _kinectAgentPrefab = agentPrefab.gameObject;
            _manager = manager;
            kinectManager = KinectManager.Instance;
            userAndAgents = new Dictionary<long, GameObject>();
            //print("KinectService Init");
        }

        public void Monitoring()
        {
            bool isInit = kinectManager.IsInitialized(); //首先要对设备进行实例化和初始化，之后才能进行后续的操作
            if (isInit)
            {
                if (kinectManager.IsUserDetected()) //判断是否检测到人物
                {
                    long userid = kinectManager.GetPrimaryUserID();//获取主要用户的ID
                    int jointType = (int)KinectInterop.JointType.Head;//头节点

                    //当检测到用户时，就获取到用户的位置信息
                    //Vector3 userPos = KinectManager.Instance.GetUserPosition(userid);
                    //print("user.x= " + userPos.x + " user.y= " + userPos.y + " user.z=" + userPos.z);

                    //判断这个用户的指定关节点是否被追踪到
                    if (kinectManager.IsJointTracked(userid, jointType))
                    {
                        print(11111111);
                        /*如果零追踪到，就开始获取主要用户的左手的位置坐标，单位是m
                         * LeftHandKinectPos 的值表示在Kinect坐标系下，其y轴的值是减去了传感器高度：Sensor Height之后的值
                         * LeftHandPos 的值在x,z轴上的和LeftHandKinectPos的相同，不同的是其y轴的值会加上Sensor Height的值,为世界坐标系下的值
                         */
                        //Vector3 LeftHandKinectPos = KinectManager.Instance.GetJointKinectPosition(userid, jointType);
                        //Vector3 LeftHandPos = KinectManager.Instance.GetJointPosition(userid, jointType);
                        ////将左手的坐标打印出来
                        //print("左手坐标为：kx=" + LeftHandKinectPos.x + " ky=" + LeftHandKinectPos.y + " kz=" + LeftHandKinectPos.z);
                        //print("左手坐标为：x=" + LeftHandKinectPos.x + " y=" + LeftHandKinectPos.y + " z=" + LeftHandKinectPos.z);
                        Vector3 jointKinectPos = kinectManager.GetJointKinectPosition(userid, jointType);
                        Vector3 jointScreenPos = Camera.main.WorldToScreenPoint(jointKinectPos);
                        print("节点世界坐标为：kx=" + jointKinectPos.x + " y=" + jointKinectPos.y + " z=" + jointKinectPos.z);
                        print("节点屏幕坐标为：x=" + jointScreenPos.x + " y=" + jointScreenPos.y + " z=" + jointScreenPos.z);

                        // 生成新实体
                        GameObject agent = Instantiate(_kinectAgentPrefab, _parentRectTransform) as GameObject;
                        //agent.GetComponent<RectTransform>().anchoredPosition = jointScreenPos;
                        agent.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

                        userAndAgents.Add(userid, agent);
                    }   else
                    {
            
                    }
                }
            }




            // 添加至移动模块
            //_manager.collisionManager.AddCollisionEffectAgent(kinectAgent);

            // 删除
            //_manager.collisionManager.RemoveCollisionEffectAgent(kinectAgent);



        }

        public void StartMonitoring(Action startSuccessAction, Action<string> startFailedAction)
        {

        }

        public void StopMonitoring()
        {

        }
    }

}
