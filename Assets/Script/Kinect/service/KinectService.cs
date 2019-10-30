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
        [SerializeField, Header("基准值m，无用"), Tooltip("调整Kinect位置到刚好显示大屏两端")]
        private float basicDistance = 3f;
        [SerializeField, Header("Kinect识别最大距离m")]
        private float safeZ = 1f;
        RectTransform _parentRectTransform;
        GameObject _kinectAgentPrefab;
        MagicWallManager _manager;

        KinectManager kinectManager;

        private bool isInit;

        //float minX;
        //float maxX;

        public void Init(RectTransform container, KinectAgent agentPrefab, MagicWallManager manager)
        {
            _parentRectTransform = container;
            _kinectAgentPrefab = agentPrefab.gameObject;
            _manager = manager;
            kinectManager = KinectManager.Instance;
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
                //if (!kinectManager.IsJointTracked(userid, jointIndex))
                //{
                //    continue;
                //}
                //当检测到用户时，就获取到用户的位置信息
                Vector3 userPos = kinectManager.GetUserPosition(userid);
                //Vector3 userPos = kinectManager.GetJointKinectPosition(userid, jointIndex);
                //print(userid + "===" + userPos);
                //kinect在背后，x正负值颠倒y
                //userPos = new Vector3(-userPos.x, userPos.y, userPos.z);
                //保留?位小数
                //float x = float.Parse(string.Format("{0:f3}", userPos.x));
                //float y = float.Parse(string.Format("{0:f3}", userPos.y));

                //if (x<minX)
                //{
                //    minX = x;
                //    print("minX"+minX + "---" + userPos.z);
                //}
                //if (x>maxX)
                //{
                //    maxX = x;
                //    print("maxX" + maxX + "---" + userPos.z);
                //}

                //屏幕中心点屏幕坐标
                Vector2 origin = new Vector2(Screen.width / 2, Screen.height / 2);
                //Vector2 userScreenPos = new Vector2(origin.x + basicDistance / 2 / userPos.z * x * Screen.width / 2, origin.y + basicDistance / userPos.z * y * Screen.height / 2); // 正式环境删除400
                Vector2 userScreenPos = new Vector2(origin.x + userPos.x / 4f * Screen.width, userPos.y * Screen.height/2);


                //Vector2 userScreenPos = new Vector2(origin.x + userPos.x;
                KinectAgent kinectAgent = _manager.kinectManager.GetAgentById(userid);

                if (!InEffectiveRange(new Vector3(userScreenPos.x, userScreenPos.y, userPos.z)))
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
                    float currentY = kinectAgent.GetComponent<RectTransform>().anchoredPosition.y;
                    var to = new Vector2(rectPosition.x, currentY);

                    Debug.Log("@@@ kinectAgent 进行移动");

                    kinectAgent.UpdatePos(to);
                    //kinectAgent.UpdatePos(rectPosition);

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
                    //print("目标被移除:" + item.userId);
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
        bool InEffectiveRange(Vector3 pos)
        {
            float kinectAgentMaskWidth = 0;

            //if (_manager.screenTypeEnum == ScreenTypeEnum.Screen1080P)
            //{
            //    kinectAgentMaskWidth = 1400;
            //}
            //else {
            //    kinectAgentMaskWidth = 933;
            //}

            if (pos.z > safeZ)
                return false;
            if (pos.x < kinectAgentMaskWidth || pos.x > Screen.width - kinectAgentMaskWidth || pos.y < kinectAgentMaskWidth || pos.y > Screen.height - kinectAgentMaskWidth)
            {
                return false;
            }
            return true;
        }
    }

}
