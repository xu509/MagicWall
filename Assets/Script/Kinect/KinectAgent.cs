using UnityEngine;
using DG.Tweening;

namespace MagicWall
{

    /// <summary>
    ///   体感实体
    /// </summary>
    public class KinectAgent : MonoBehaviour, CollisionEffectAgent
    {
        private long _userId;
        public long userId { get { return _userId; } }



        private float _createTime;
        public float createTime { get { return _createTime; } }

        private KinectAgentStatusEnum _status;
        public KinectAgentStatusEnum status { get { return _status; } }


        private ICollisionMoveBehavior _collisionMoveBehavior;
        private CardAgent _refCardAgent;
        public CardAgent refCardAgent { set { _refCardAgent = value; } get { return _refCardAgent; } }



        void Awake() {
            _createTime = Time.time;

            // 生成动画

        }


        public float GetHeight()
        {
            var height = GetComponent<RectTransform>().rect.height;

            Vector3 scaleVector3 = GetComponent<RectTransform>().localScale;
            return 800f;
        }

        private float _width;
        public float width
        {
            get
            {
                return GetComponent<RectTransform>().rect.width;
            }
        }
        private float _height;
        public float height
        {
            get
            {
                return GetComponent<RectTransform>().rect.height;
            }
        }


        public ICollisionMoveBehavior GetMoveBehavior()
        {
            return _collisionMoveBehavior;
        }

        public string GetName()
        {
            return "kinectobj";
        }

        public Vector3 GetRefPosition()
        {
            var pos = GetComponent<RectTransform>().position;
            var screenPosition = RectTransformUtility.WorldToScreenPoint(null, pos);
            return screenPosition;
        }

        public float GetWidth()
        {
            var width = GetComponent<RectTransform>().rect.width;

            Vector3 scaleVector3 = GetComponent<RectTransform>().localScale;
            return 800f;
        }

        public bool IsEffective()
        {
            return true;
        }

        public void SetMoveBehavior(ICollisionMoveBehavior moveBehavior)
        {
            _collisionMoveBehavior = moveBehavior;
        }


        /// <summary>
        /// 生成动画
        /// </summary>
        public void Init(long userId) {
            _userId = userId;
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close() {

            Debug.Log(gameObject.name + "delete!");


            // 关闭动画

            // 关闭

            // 动画完成后           
            var MKinectManager = GameObject.Find("kinect").GetComponent<MKinectManager>();
            MKinectManager.RemoveKinectAgents(this);

        }

        public void UpdatePos(Vector2 anchPos)
        {
            //GetComponent<RectTransform>().DOMoveX(anchPos.x, Time.deltaTime);
            GetComponent<RectTransform>().anchoredPosition = anchPos;
        }

    }

}
