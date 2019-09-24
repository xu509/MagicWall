using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace MagicWall
{
    public class BackgroundManager : MonoBehaviour
    {

        // 气泡预制体
        [SerializeField] ClearBubbleAgent _clearBubbleAgentPrefab;
        // 气泡预制体2 
        [SerializeField] DimBubbleAgent _dimBubbleAgentPrefab;

        /// <summary>
        /// 对象池 : 清晰的球
        /// </summary>
        BubblePool<ClearBubbleAgent> _clearBubbleAgentPool;
        public BubblePool<ClearBubbleAgent> ClearBubbleAgentPool { get { return _clearBubbleAgentPool; } }


        /// <summary>
        /// 对象池 : 模糊的球
        /// </summary>
        BubblePool<DimBubbleAgent> _dimBubbleAgentPool;
        public BubblePool<DimBubbleAgent> DimBubbleAgentPool { get { return _dimBubbleAgentPool; } }

        [SerializeField] private RectTransform _bubblesBackground;

        MagicWallManager _manager;
        private List<BubbleAgent> _bubbleAgents;

        private bool hasInit = false;
        private bool _doBeforeRun = false;



        //
        // Awake instead of Constructor
        //
        private void Awake()
        {

        }

        public void Init(MagicWallManager manager)
        {
            _manager = manager;

            _clearBubbleAgentPool = BubblePool<ClearBubbleAgent>.GetInstance(_manager.managerConfig.BackgroundClearBubblePoolSize);
            _dimBubbleAgentPool = BubblePool<DimBubbleAgent>.GetInstance(_manager.managerConfig.BackgroundDimBubblePoolSize);

            //  初始化对象池
            PrepareData();

            hasInit = true;
            _doBeforeRun = false;
            _bubbleAgents = new List<BubbleAgent>();
        }

        /// <summary>
        /// 气球池初始化
        /// </summary>
        private void PrepareData()
        {
            _clearBubbleAgentPool.Init(_clearBubbleAgentPrefab, _bubblesBackground);
            _dimBubbleAgentPool.Init(_dimBubbleAgentPrefab, _bubblesBackground);
        }



        //
        //  Constructor
        //
        protected BackgroundManager() { }


        public void run()
        {

            if (!hasInit)
                return;

            DoBeforeRun();

            if (!_doBeforeRun)
                return;

            for (int i = 0; i < _bubbleAgents.Count; i++)
            {

                BubbleAgent bubbleAgent = _bubbleAgents[i];

                //  如果气球已需要销毁
                if (_bubbleAgents[i].IsOverTop())
                {
                    float k = Random.Range(0f, 1f);
                    float position_x = Mathf.Lerp(0, Screen.width, k);

                    // 清理
                    if (_bubbleAgents[i].bubbleType == BubbleType.Clear)
                    {
                        _clearBubbleAgentPool.ReleaseObj(_bubbleAgents[i] as ClearBubbleAgent);
                        _bubbleAgents.Remove(_bubbleAgents[i]);
                        CreateClearBubble(new Vector3(position_x, 0));
                    }
                    else
                    {
                        _dimBubbleAgentPool.ReleaseObj(_bubbleAgents[i] as DimBubbleAgent);
                        _bubbleAgents.Remove(_bubbleAgents[i]);
                        CreateDimBubble(new Vector3(position_x, 0));
                    }
                }
                else
                {
                    float minf, maxf;
                    if (bubbleAgent.bubbleType == BubbleType.Clear)
                    {
                        minf = _manager.managerConfig.BackgroundClearMoveMinFactor;
                        maxf = _manager.managerConfig.BackgroundClearMoveMaxFactor;
                    }
                    else
                    {
                        minf = _manager.managerConfig.BackgroundDimMoveMinFactor;
                        maxf = _manager.managerConfig.BackgroundDimMoveMaxFactor;
                    }

                    float moveFactor = bubbleAgent.GetMoveFactor(minf, maxf);
                    _bubbleAgents[i].Raise(moveFactor);
                }

            }




        }


        private void DoBeforeRun()
        {
            if (!_doBeforeRun)
            {

                for (int i = 0; i < _manager.managerConfig.BackgroundClearBubblePoolSize; i++)
                {
                    Vector3 position = Random.insideUnitSphere;

                    float x = (position.x + 1f) / 2f;
                    float y = (position.y + 1f) / 2f;
                    float z = (position.z + 1f) / 2f;

                    x = Mathf.Lerp(0, Screen.width, x);
                    y = Mathf.Lerp(0, Screen.height, y);
                    //y = Mathf.Lerp(0, Screen.height, y);

                    y = y - Screen.height;

                    CreateClearBubble(new Vector3(x, y, z));
                }


                for (int i = 0; i < _manager.managerConfig.BackgroundDimBubblePoolSize; i++)
                {
                    Vector3 position = Random.insideUnitSphere;

                    float x = (position.x + 1f) / 2f;
                    float y = (position.y + 1f) / 2f;
                    float z = (position.z + 1f) / 2f;

                    x = Mathf.Lerp(0, Screen.width, x);
                    y = Mathf.Lerp(0, Screen.height, y);

                    y = y - Screen.height;

                    CreateDimBubble(new Vector3(x, y, z));
                }

                _doBeforeRun = true;
            }
        }


        void CreateClearBubble(Vector3 position)
        {
            BubbleAgent bubble = _clearBubbleAgentPool.GetObj();

            float height = bubble.GetComponent<RectTransform>().rect.height;
            position = position - new Vector3(0, height, 0);

            bubble.Init(this, _manager, BubbleType.Clear, position);


            _bubbleAgents.Add(bubble);
        }

        void CreateDimBubble(Vector3 position)
        {
            BubbleAgent bubble = _dimBubbleAgentPool.GetObj();

            float height = bubble.GetComponent<RectTransform>().rect.height;
            position = position - new Vector3(0, height, 0);

            bubble.Init(this, _manager, BubbleType.Dim, position);
            bubble.GetComponent<RectTransform>().SetAsFirstSibling();
            _bubbleAgents.Add(bubble);
        }



        public void Reset()
        {
            hasInit = false;
            _clearBubbleAgentPool.Reset();
            _dimBubbleAgentPool.Reset();
        }


    }
}