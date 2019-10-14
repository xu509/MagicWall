using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        Dictionary<long, KinectAgent> userAndAgents;

        private bool isInit;


        public void Init(RectTransform container, KinectAgent agentPrefab,MagicWallManager manager)
        {
            _parentRectTransform = container;
            _kinectAgentPrefab = agentPrefab.gameObject;
            _manager = manager;
            kinectManager = KinectManager.Instance;
            userAndAgents = new Dictionary<long, KinectAgent>();
            //print("KinectService Init");
        }

        public void Monitoring()
        {
            if (!isInit)
                return;

            if (userAndAgents.Count > 2)
            {
                return;
            }

            //bool isInit = kinectManager.IsInitialized(); //首先要对设备进行实例化和初始化，之后才能进行后续的操作

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
                    Vector3 jointKinectPos = kinectManager.GetJointKinectPosition(userid, jointType);
                    Vector3 jointScreenPos = Camera.main.WorldToScreenPoint(jointKinectPos);
                    //print("节点世界坐标为：kx=" + jointKinectPos.x + " y=" + jointKinectPos.y + " z=" + jointKinectPos.z);
                    //print("节点屏幕坐标为：x=" + jointScreenPos.x + " y=" + jointScreenPos.y + " z=" + jointScreenPos.z);
                    Vector2 origin = new Vector2(Screen.width / 2, Screen.height / 2);
                    Vector2 anchPos = new Vector2(jointScreenPos.x - origin.x, jointScreenPos.y - origin.y);
                    if (!userAndAgents.ContainsKey(userid))
                    {
                        // 生成新实体
                        GameObject agent = Instantiate(_kinectAgentPrefab, _parentRectTransform) as GameObject;
                        RectTransform rtf = agent.GetComponent<RectTransform>();
                        rtf.anchoredPosition = anchPos;
                        rtf.localScale = Vector3.zero;
                        rtf.DOScale(1, 0.5f);
                        //添加至移动模块
                        _manager.collisionManager.AddCollisionEffectAgent(agent.GetComponent<KinectAgent>());
                        agent.GetComponent<KinectAgent>().SetMoveBehavior(_manager.collisionMoveBehaviourFactory.GetMoveBehavior(_manager.collisionBehaviorConfig.behaviourType));
                        userAndAgents.Add(userid, agent.GetComponent<KinectAgent>());
                    }   else
                    {
                        userAndAgents[userid].GetComponent<RectTransform>().anchoredPosition = anchPos;
                    }
                }   else
                {
                    if (userAndAgents.ContainsKey(userid))
                    {
                        KinectAgent agent = userAndAgents[userid];
                        agent.GetComponent<RectTransform>().DOScale(0, 0.5f).OnComplete(() => {
                            _manager.collisionManager.RemoveCollisionEffectAgent(agent);
                            Destroy(userAndAgents[userid].gameObject);
                        });
                        userAndAgents.Remove(userid);
                    }
                }
            }
        }

        public void StartMonitoring(Action startSuccessAction, Action<string> startFailedAction)
        {
            isInit = kinectManager.IsInitialized(); //首先要对设备进行实例化和初始化，之后才能进行后续的操作
            if (isInit)
            {
                startSuccessAction.Invoke();
            }
            else {
                startFailedAction.Invoke("error");
            }

        }

        public void StopMonitoring()
        {

        }
    }

}
