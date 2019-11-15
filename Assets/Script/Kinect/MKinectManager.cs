using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall {

    /// <summary>
    /// ref : https://www.yuque.com/books/share/4f5397bb-9ecf-4622-bf62-f812a38d2057
    /// </summary>
    public class MKinectManager : MonoBehaviour
    {
        [SerializeField] float safeDistance = 500f;
        [SerializeField] bool isMock = false;

        [SerializeField, Header("Prefab")] KinectAgent _kinectAgentPrefab;
        [SerializeField, Header("UI")] RectTransform _agentContainer;
        [SerializeField, Header("Service")] KinectService _kinect2Service;
        [SerializeField, Tooltip("体感块移动延迟时间")]
        public float agentMoveDelayTime = 0.5f;
        [SerializeField, Tooltip("体感块移动忽略值(减少灵敏度)，目前无用")]
        public float ignoreValue = 10f;
        [SerializeField] KinectType _kinectType;


        private List<KinectAgent> _kinectAgents;
        public List<KinectAgent> kinectAgents { get { return _kinectAgents; } }


        private bool _isInit = false;

        private bool isMonitoring = false;

        private IKinectService _kinectService;


        private Action _startSuccessAction;
        private Action<string> _startFailedAction;

        private MagicWallManager _manager;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        public void Run()
        {
            if (!_isInit)
            {
                _isInit = true;
                StartMonitoring();
            }
            else {
                if (_manager != null && isMonitoring)
                {
                    //Debug.Log("@@@ Kinecet 正在检测");

                    _kinectService.Monitoring();
                    //_kinectCardObserver.Observering();
                }

                if (_kinectAgents != null)
                {
                    List<KinectAgent> needDestoryAgents = new List<KinectAgent>();

                    for (int i = 0; i < _kinectAgents.Count; i++)
                    {
                        _kinectAgents[i].UpdateBehaviour();

                        if (_kinectAgents[i].status == KinectAgentStatusEnum.Obsolete || _manager.useKinect == false)
                        {
                            needDestoryAgents.Add(_kinectAgents[i]);
                        }
                    }

                    for (int i = 0; i < needDestoryAgents.Count; i++)
                    {
                        var agent = needDestoryAgents[i];
                        _manager.collisionManager.RemoveCollisionEffectAgent(agent);
                        _kinectAgents.Remove(agent);
                        Destroy(agent.gameObject);
                    }

                    needDestoryAgents.Clear();
                }

                if (_manager.useKinect && isMock && _manager.openKinect) {
                    if (_kinectAgents.Count == 0)
                    {
                        // 模拟创建实体，实际使用kinect需注释
                        var screenPosition = new Vector2(2000, 960);
                        AddKinectAgents(screenPosition, 111);
                    }
                }

            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(MagicWallManager magicWallManager) {
            _manager = magicWallManager;

            if (_kinectType == KinectType.Kinect2)
            {
                _kinectService = _kinect2Service;
            }
            else if (_kinectType == KinectType.AzureKinect){                
                // 缺失 kinect 3
            }

            _kinectAgents = new List<KinectAgent>();

            _startSuccessAction = StartKinectSuccess;
            _startFailedAction = StartKinectFailed;


            if (isMock)
            {
                Debug.Log("开启 kinect 模拟模式。");

                // 模拟创建实体，实际使用kinect需注释  结束
            }
            else {
                _kinectService.Init(_agentContainer, _kinectAgentPrefab, _manager);
                //StartMonitoring();
            }

            Debug.Log("@@@ Kinecet 初始化成功");


        }


        /// <summary>
        /// 开始监控
        /// </summary>
        public void StartMonitoring() {            
            _kinectService.StartMonitoring(_startSuccessAction, _startFailedAction);
            // 开启成功/失败后调用回调，修改isMonitoring
        }

        /// <summary>
        /// 关闭监控
        /// </summary>
        public void StopMonitoring()
        {
            // 关闭逻辑
            _kinectService.StopMonitoring();

            isMonitoring = false;
        }




        void StartKinectSuccess() {
            Debug.Log("启动 kinect 成功");
            isMonitoring = true;
        }

        void StartKinectFailed(string msg)
        {
            Debug.Log("启动 kinect 失败");
            isMonitoring = false;
        }


        /// <summary>
        ///     ref ： https://www.yuque.com/u314548/fc6a5l/zqi1iq
        ///     对应生成体感卡片
        /// </summary>
        /// <param name="screenPosition">屏幕的位置</param>
        /// <returns></returns>
        public bool CalScreenPositionIsAvailable(Vector2 screenPosition) {
            var position = Camera.main.ScreenToWorldPoint(screenPosition);

            bool isAvailable = true;

            for (int i = 0; i < _kinectAgents.Count; i++) {
                var agent = _kinectAgents[i];

                if (agent.status == KinectAgentStatusEnum.Normal) {

                    var distance = Vector2.Distance(agent.transform.position, position);

                    if (distance < safeDistance)
                    {
                        isAvailable = false;
                    }

                }

            }

            for (int i = 0; i < _manager.operateCardManager.EffectAgents.Count; i++)
            {
                var agent = _manager.operateCardManager.EffectAgents[i];
                var distance = Vector2.Distance(agent.transform.position, position);

                if (distance < safeDistance)
                {
                    isAvailable = false;
                }
            }

            return isAvailable;
        }

        /// <summary>
        ///     进入了点开卡片的影响范围
        /// </summary>
        /// <param name="kinectAgent"></param>
        /// <returns></returns>
        public bool HasEnterCardRange(KinectAgent kinectAgent)
        {
            var screenPosition = kinectAgent.GetRefPosition();
            var position = Camera.main.ScreenToWorldPoint(screenPosition);


            bool hasEnter = false;

            for (int i = 0; i < _manager.operateCardManager.EffectAgents.Count; i++)
            {
                var agent = _manager.operateCardManager.EffectAgents[i];
                var distance = Vector2.Distance(agent.transform.position, position);
                if (distance < safeDistance)
                {
                    hasEnter = true;
                }
            }

            return hasEnter;
        }


        /// <summary>
        ///     进入了体感卡片的影响范围
        /// </summary>
        /// <param name="kinectAgent"></param>
        /// <returns></returns>
        public KinectAgent HasEnterKinectCardRange(KinectAgent kinectAgent)
        {
            var screenPosition = kinectAgent.GetRefPosition();
            var position = Camera.main.ScreenToWorldPoint(screenPosition);

            KinectAgent targetKinectAgent = null;

            for (int i = 0; i < _kinectAgents.Count; i++)
            {
                var agent = _kinectAgents[i];

                if ((agent.userId != kinectAgent.userId) && agent.status == KinectAgentStatusEnum.Normal) {
                    var distance = Vector2.Distance(agent.transform.position, position);
                    if (distance < safeDistance)
                    {
                        targetKinectAgent = agent;
                    }
                }
            }

            return targetKinectAgent;
        }



        /// <summary>
        /// 增加一个新的体感卡片
        /// </summary>
        /// <param name="kinectAgent"></param>
        public KinectAgent AddKinectAgents(Vector2 screenPosition,long userId) {

            var item = GetAgentById(userId);

            //最多出现体感球
            if (item != null || kinectAgents.Count > 1)
            {
                return null;
            }
            else {
                // 屏幕坐标转UI坐标
                Vector2 rectPosition = new Vector2();
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_agentContainer, screenPosition, null, out rectPosition);

                KinectAgent kinectAgent = Instantiate(_kinectAgentPrefab, _agentContainer);
                kinectAgent.GetComponent<RectTransform>().anchoredPosition = rectPosition;
                kinectAgent.Init(userId,_manager);
                _manager.collisionManager.AddCollisionEffectAgent(kinectAgent);
                _kinectAgents.Add(kinectAgent);
                return kinectAgent;
            }            
        }





        public KinectAgent GetAgentById(long userId) {
            for (int i = 0; i < _kinectAgents.Count; i++) {
                if (_kinectAgents[i].userId == userId) {
                    return _kinectAgents[i];
                }
            }
            return null;
        }




    }

}
