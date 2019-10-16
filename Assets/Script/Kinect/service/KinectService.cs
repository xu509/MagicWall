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
        [SerializeField, Header("屏幕实际尺寸m")]
        private Vector2 physicalSize = new Vector2(4.102f, 1.21f);
        [SerializeField, Header("基准值m")]
        private float basicDistance = 3f;
        [SerializeField, Header("m")]
        private float safeZ = 1f;
        RectTransform _parentRectTransform;
        GameObject _kinectAgentPrefab;
        MagicWallManager _manager;

        KinectManager kinectManager;
        Dictionary<long, KinectAgent> userAndAgents;

        private bool isInit;


        public void Init(RectTransform container, KinectAgent agentPrefab, MagicWallManager manager)
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

            //生成体感卡片
            List<long> ids = kinectManager.GetAllUserIds();
            for (int i = 0; i < ids.Count; i++)
            {
                long userid = ids[i];
                //获取关节
                int jointIndex = (int)KinectInterop.JointType.Head;
                if (!kinectManager.IsJointTracked(userid, jointIndex))
                {
                    continue;
                }
                //当检测到用户时，就获取到用户的位置信息
                //Vector3 userPos = kinectManager.GetUserPosition(userid);
                Vector3 userPos = kinectManager.GetJointKinectPosition(userid, jointIndex);
                //kinect在背后，x正负值颠倒
                userPos = new Vector3(-userPos.x, userPos.y, userPos.z);
                float absMaxX = userPos.z / basicDistance * physicalSize.x / 2;
                float absMaxY = userPos.z / basicDistance * physicalSize.y / 2;
                //屏幕中心点屏幕坐标
                Vector2 origin = new Vector2(Screen.width / 2, Screen.height / 2);
                Vector2 userScreenPos = new Vector2(origin.x + userPos.x / physicalSize.x / 2 * Screen.width, origin.y + userPos.y / physicalSize.y / 2 * Screen.height + 400); // 正式环境删除400
                KinectAgent kinectAgent = _manager.kinectManager.GetAgentById(userid);

                if (!InEffectiveRange(new Vector3(userScreenPos.x, userScreenPos.y, userPos.z), absMaxX))
                {
                    //print("超出边界");
                    if (kinectAgent != null)
                    {
                        kinectAgent.Close();
                    }
                    continue;
                }

                //添加逻辑
                if (kinectAgent == null)
                {
                    if (_manager.kinectManager.CalScreenPositionIsAvailable(userScreenPos))
                    {
                        _manager.kinectManager.AddKinectAgents(userScreenPos, userid);
                    }
                }
                else {
                    //移动体感卡片，不进行上下移动
                    Vector2 rectPosition = new Vector2();
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentRectTransform, userScreenPos, null, out rectPosition);

                    //var to = new Vector2(rectPosition.x, kinectAgent.GetComponent<RectTransform>().anchoredPosition.y);
                    //kinectAgent.UpdatePos(to);
                    kinectAgent.UpdatePos(rectPosition);

                    if (_manager.kinectManager.HasEnterCardRange(kinectAgent))
                    {
                        kinectAgent.Close();
                    }
                    KinectAgent target = _manager.kinectManager.HasEnterKinectCardRange(kinectAgent);
                    if (target)
                    {
                        if (kinectAgent.createTime < target.createTime)
                        {
                            kinectAgent.Close();
                        }
                    }
                }                
            }


            // 那 ids 去进行校对，返回需要删除的遮罩
            var existAgents = _manager.kinectManager.kinectAgents;
            // 比对ids 和 existagents.userid
            foreach (var item in existAgents)
            {
                if (!ids.Contains(item.userId) && item.status != KinectAgentStatusEnum.Destoring && item.status != KinectAgentStatusEnum.Obsolete)
                {
                    print("目标被移除:" + item.userId);
                    item.Close();
                }
            }
            // 得出结果，调用kinectAgent.Close();



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

        /// <summary>
        /// 在有效范围内
        /// </summary>
        bool InEffectiveRange(Vector3 pos, float absMaxX)
        {
            if (pos.z < safeZ)
                return false;
            if (pos.x < 0 || pos.x > Screen.width || pos.y < 0 || pos.y > Screen.height)
            {
                return false;
            }
            return true;
        }
    }

}
