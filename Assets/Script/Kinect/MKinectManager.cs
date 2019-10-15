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


        [SerializeField, Header("Prefab")] KinectAgent _kinectAgentPrefab;
        [SerializeField, Header("UI")] RectTransform _agentContainer;
        [SerializeField, Header("Service")] KinectService _kinect2Service;

        [SerializeField] KinectType _kinectType;


        private List<KinectAgent> _kinectAgents;
        public List<KinectAgent> kinectAgents { get { return _kinectAgents; } }



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
        void Update()
        {
            if (_manager != null) {
                _kinectService.Monitoring();
            }


            //if (Input.GetMouseButton(1)) {

            //    Debug.Log("Click Right Button");

            //}

            
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

            _kinectService.Init(_agentContainer, _kinectAgentPrefab,_manager);



            // 模拟创建实体，实际使用kinect需注释
            //KinectAgent _kinectAgent = Instantiate(_kinectAgentPrefab, _agentContainer);

            //_kinectAgent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

            //var factoryBehavior = _manager.collisionMoveBehaviourFactory.GetMoveBehavior(_manager.collisionBehaviorConfig.behaviourType);

            //_kinectAgent.SetMoveBehavior(factoryBehavior);

            //_manager.collisionManager.AddCollisionEffectAgent(_kinectAgent);

            //_kinectAgents.Add(_kinectAgent);
            // 模拟创建实体，实际使用kinect需注释  结束

            StartMonitoring();

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
                var distance = Vector2.Distance(agent.transform.position, position);

                if (distance < safeDistance) {
                    isAvailable = false;
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

                if (agent != kinectAgent) {
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

            if (item != null)
            {
                return null;
            }
            else {
                // 屏幕坐标转UI坐标
                Vector2 rectPosition = new Vector2();
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_agentContainer, screenPosition, null, out rectPosition);

                KinectAgent kinectAgent = Instantiate(_kinectAgentPrefab, _agentContainer);
                kinectAgent.GetComponent<RectTransform>().anchoredPosition = rectPosition;
                kinectAgent.Init(userId);
                kinectAgent.SetMoveBehavior(_manager.collisionMoveBehaviourFactory.GetMoveBehavior(_manager.collisionBehaviorConfig.behaviourType));

                _manager.collisionManager.AddCollisionEffectAgent(kinectAgent);
                _kinectAgents.Add(kinectAgent);

                print("_kinectAgents add new" + _kinectAgents.Count);


                return kinectAgent;
            }            
        }


        public void RemoveKinectAgents(KinectAgent kinectAgent) {
            _manager.collisionManager.RemoveCollisionEffectAgent(kinectAgent);
            _kinectAgents.Remove(kinectAgent);

            Debug.Log("_kinectAgents size : " + _kinectAgents.Count);

            Destroy(kinectAgent.gameObject);
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
