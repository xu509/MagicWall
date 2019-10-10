using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 浮动快(背后的)对象池
/// </summary>
namespace MagicWall
{
    public class FlockAgentInStarPool<T> where T : FlockAgent
    {
        #region 单例
        private static FlockAgentInStarPool<T> instance;

        private FlockAgentInStarPool(int initTotal)
        {
            _pool = new Queue<T>();
            _initTotal = initTotal;
        }
        public static FlockAgentInStarPool<T> GetInstance(int total)
        {
            if (instance == null)
            {
                instance = new FlockAgentInStarPool<T>(total);
            }
            return instance;
        }

        #endregion

        /// <summary>
        /// 对象池
        /// </summary>
        private Queue<T> _pool;


        /// <summary>
        /// 对象池总数
        /// </summary>
        private int _initTotal;


        private T _t;
        private RectTransform _container;


        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(T t, RectTransform container)
        {
            _t = t;
            _container = container;

            for (int i = 0; i < _initTotal; i++)
            {
                Add();
            }
        }

        /// <summary>
        /// 从对象池中获取对象
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        public T GetObj()
        {
            // 如果对象池内无剩余可用对象，则再生成一个
            if (_pool.Count == 0)
            {
                Add();
                var result = _pool.Dequeue();
                result.gameObject.SetActive(true);
                return result;
            }
            else
            {
                var result = _pool.Dequeue();
                result.gameObject.SetActive(true);
                return result;
            }
        }


        /// <summary>
        /// 回收 / 增加 对象
        /// </summary>
        /// <param name="obj"></param>
        public void ReleaseObj(T obj)
        {
            obj.Reset();
            obj.gameObject.name = "flock(prepared)";
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }


        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        private void Add()
        {
            T flockAgent = FlockAgentInvoker<T>.CreateAgent(_t, _container);
            flockAgent.gameObject.SetActive(false);
            flockAgent.gameObject.name = "flock(prepared)";
            _pool.Enqueue(flockAgent);
        }

        public void Reset()
        {
            _pool = new Queue<T>();
            _initTotal = 0;
        }

    }
}