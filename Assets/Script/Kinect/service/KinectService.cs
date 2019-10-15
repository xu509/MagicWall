using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace MagicWall
{

    /// <summary>
    ///   kinect 服务类
    /// </summary>
    public class KinectService : MonoBehaviour, IKinectService
    {
        [SerializeField, Header("设备到人距离m")]
        private float distance = 3f;
        [SerializeField, Header("屏幕实际尺寸m")]
        private Vector2 physicalSize = new Vector2(4.102f, 1.21f);
        [SerializeField, Header("基准值m")]
        private float basicDistance = 3f;
        RectTransform _parentRectTransform;
        GameObject _kinectAgentPrefab;
        MagicWallManager _manager;

        KinectManager kinectManager;
        Dictionary<long, KinectAgent> userAndAgents;

        private bool isInit;

        //最大宽度的一半
        private float absMaxX;
        //最大高度的一半
        private float absMaxY;

        public void Init(RectTransform container, KinectAgent agentPrefab, MagicWallManager manager)
        {
            _parentRectTransform = container;
            _kinectAgentPrefab = agentPrefab.gameObject;
            _manager = manager;
            kinectManager = KinectManager.Instance;
            userAndAgents = new Dictionary<long, KinectAgent>();
            //print("KinectService Init");
            absMaxX = distance / basicDistance * physicalSize.x / 2;
            absMaxY = distance / basicDistance * physicalSize.y / 2;
            kinectManager.SetAddUserAction(AddUserAction);
            kinectManager.SetRemoveUserAction(RemoveUserAction);
        }

        private void AddUserAction(long userId)
        {
            print("新增用户：" + userId);
        }

        private void RemoveUserAction(long userId)
        {
            print("删除用户：" + userId);
        }




        public void Monitoring()
        {
            return;
            if (!isInit)
                return;

            //bool isInit = kinectManager.IsInitialized(); //首先要对设备进行实例化和初始化，之后才能进行后续的操作

            List<long> ids = kinectManager.GetAllUserIds();
            for (int i = 0; i < ids.Count; i++)
            {
                long userid = ids[i];
                //当检测到用户时，就获取到用户的位置信息
                Vector3 userPos = kinectManager.GetUserPosition(userid);
                //kinect在背后，x正负值颠倒
                userPos = new Vector3(-userPos.x, userPos.y, userPos.z);
                //屏幕中心点屏幕坐标
                Vector2 origin = new Vector2(Screen.width / 2, Screen.height / 2);
                Vector2 userScreenPos = new Vector2(origin.x + userPos.x / absMaxX * Screen.width, origin.y + userPos.y / absMaxY * Screen.height);
                Vector2 anchPos = new Vector2(userScreenPos.x - origin.x, userScreenPos.y - origin.y);

                if (!userAndAgents.ContainsKey(userid) && userPos.z > distance && userAndAgents.Count < 3)
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
                }
                else
                {
                    if (userAndAgents.ContainsKey(userid))
                    {
                        //print("Y" + userPos.y);
                        userAndAgents[userid].GetComponent<RectTransform>().anchoredPosition = anchPos;
                    }
                }

            }
            Dictionary<long, KinectAgent> keyValuePairs = userAndAgents;
            foreach (var userid in keyValuePairs.Keys)
            {
                if (!ids.Exists(t => t == userid))
                {
                    KinectAgent agent = userAndAgents[userid];
                    agent.GetComponent<RectTransform>().DOScale(0, 0.5f).OnComplete(() => {
                        _manager.collisionManager.RemoveCollisionEffectAgent(agent);
                        Destroy(userAndAgents[userid].gameObject);
                        userAndAgents.Remove(userid);
                    });
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
            else
            {
                startFailedAction.Invoke("error");
            }

        }

        public void StopMonitoring()
        {

        }
    }

}
