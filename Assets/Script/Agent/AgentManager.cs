using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


/// <summary>
/// 浮动体块
/// </summary>
namespace MagicWall
{
    public class AgentManager : MonoBehaviour
    {

        /// <summary>
        ///     普通浮动块
        /// </summary>
        [SerializeField, Header("Prefab")] FlockAgent _flockAgentPrefab;


        /// <summary>
        /// 普通浮动块容器
        /// </summary>
        [SerializeField, Header("Container")] RectTransform _flockContainer;

        /// <summary>
        ///    后层的浮动块容器
        /// </summary>
        [SerializeField] RectTransform _backContainer;

        /// <summary>
        ///    后层的浮动块容器
        /// </summary>
        [SerializeField] RectTransform _starContainer;

        [SerializeField] FlockAgentFactoryInstance _flockAgentFactoryInstance;
        public FlockAgentFactoryInstance flockAgentFactoryInstance { get { return _flockAgentFactoryInstance; } }


        ///// <summary>
        ///// 操作卡片块容器
        ///// </summary>
        //[SerializeField] RectTransform _cardContainer;





        // 主管理器
        private MagicWallManager _manager;

        /// <summary>
        ///     普通浮动块对象池
        /// </summary>
        private FlockAgentPool<FlockAgent> _flockAgentPool;

        /// <summary>
        ///     后层的对象池
        /// </summary>
        private FlockAgentInBackPool<FlockAgent> _flockAgentInBackPool;

        /// <summary>
        ///     Star的对象池
        /// </summary>
        private FlockAgentInStarPool<FlockAgent> _flockAgentInStarPool;



        #region 业务逻辑相关属性
        //  当前界面的 agents
        List<FlockAgent> _agents;
        public List<FlockAgent> Agents { get { return _agents; } }



        private bool runLock = false;
        #endregion


        //
        //  single pattern
        // 
        void Awake()
        {

        }

        //
        //  Constructor
        //
        protected AgentManager() { }


        #region Public Methods

        public void Init(MagicWallManager manager)
        {
            //effectAgent = new List<FlockAgent>();
            _agents = new List<FlockAgent>();
            _manager = manager;

            PrepareAgentPool();
        }

        /// <summary>
        ///     准备对象池
        /// </summary>
        private void PrepareAgentPool()
        {
            _flockAgentPool = FlockAgentPool<FlockAgent>.GetInstance(_manager.managerConfig.FlockPoolSize);
            _flockAgentPool.Init(_flockAgentPrefab, _flockContainer);

            _flockAgentInBackPool = FlockAgentInBackPool<FlockAgent>.GetInstance(_manager.managerConfig.FlockPoolSize / 2);
            _flockAgentInBackPool.Init(_flockAgentPrefab, _backContainer);

            _flockAgentInStarPool = FlockAgentInStarPool<FlockAgent>.GetInstance(_manager.cutEffectConfig.StarEffectAgentsCount);
            _flockAgentInStarPool.Init(_flockAgentPrefab, _starContainer);

        }


        //
        //  清理所有的agents
        //
        public void RecycleAgent(FlockAgent agent)
        {
            //Debug.Log("Recycle Agent : " + agent.gameObject + "[" + agent.SceneIndex + "]") ;

            agent.Reset();
            _agents.Remove(agent);


           

            DestoryAgent(agent);
        }

        //
        //  清理所有的agents
        //
        public void ClearAll()
        {
            for (int i = 0; i < _agents.Count; i++) {

                _agents[i].flockStatus = FlockStatusEnum.OBSOLETE;


                //if (!(_agents[i].flockStatus == FlockStatusEnum.HIDE
                //    || _agents[i].flockStatus == FlockStatusEnum.TOHIDE)) {
                //    _agents[i].flockStatus = FlockStatusEnum.OBSOLETE;
                //}
            }            
        }


        //
        //  移除效果列表中的项
        //
        public void AddItem(FlockAgent agent)
        {
            _agents.Add(agent);
        }


        // 初始化
        public void Reset()
        {
            //  清理 Agents
            for (int i = 0; i < Agents.Count; i++)
            {
                FlockAgent agent = Agents[i];
                _flockAgentPool.ReleaseObj(agent);
            }
            _agents = new List<FlockAgent>();

            _flockAgentPool.Reset();
            _flockAgentInBackPool.Reset();

        }



        // 持续更新
        public void Run()
        {
            if (!runLock)
            {
                runLock = true;
                if (Agents.Count > 0)
                {
                    List<FlockAgent> recycleAgents = new List<FlockAgent>();

                    for (int i = 0; i < Agents.Count; i++) {
                        var agent = Agents[i];

                        if (agent.flockStatus == FlockStatusEnum.OBSOLETE)
                        {
                            recycleAgents.Add(agent);
                        }
                        else {
                            agent.updatePosition();
                            agent.CheckIsNeedRecycle();
                        }
                    }

                    for (int i = 0; i < recycleAgents.Count; i++) {
                        RecycleAgent(recycleAgents[i]);
                    }

                }

                //// 检测打开的个数大于8个时，关闭早的
                //if (EffectAgent.Count > _manager.managerConfig.SelectedItemMaxCount)
                //{
                //    // 此时得到的是CardAgent
                //    CardAgent effectAgent = EffectAgent[0] as CardAgent;
                //    if (effectAgent.CardStatus != CardStatusEnum.DESTORY
                //        && effectAgent.CardStatus != CardStatusEnum.OBSOLETE)
                //    {
                //        effectAgent.DoCloseDirect();
                //    }
                //    //EffectAgent[0].GetCardAgent.DoCloseDirect();
                //}
                runLock = false;
            }
        }

        /// <summary>
        /// 工具型方法，请勿在未判断业务逻辑时直接使用
        /// </summary>
        /// <param name="agent"></param>
        public void DestoryAgent(FlockAgent agent)
        {

            if (agent.agentContainerType == AgentContainerType.MainPanel)
            {
                //Debug.Log("ReleaseObj : " + agent.name);
                Destroy(agent.gameObject);
                //_flockAgentPool.ReleaseObj(agent);
            }
            else if (agent.agentContainerType == AgentContainerType.BackPanel)
            {
                Destroy(agent.gameObject);

                //_flockAgentInBackPool.ReleaseObj(agent);
            }
            else
            {
                _flockAgentInStarPool.ReleaseObj(agent);
            }
            
        }



        #endregion


        public FlockAgent GetFlockAgent(AgentContainerType type)
        {
            if (type == AgentContainerType.MainPanel)
            {
                return _flockAgentPool.GetObj();
            }
            else if (type == AgentContainerType.BackPanel)
            {
                return _flockAgentInBackPool.GetObj();
            }
            else
            {
                return _flockAgentInStarPool.GetObj();
            }
        }



    }
}