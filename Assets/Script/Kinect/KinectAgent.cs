using UnityEngine;
using DG.Tweening;

namespace MagicWall
{

    /// <summary>
    ///   体感实体
    /// </summary>
    public class KinectAgent : MonoBehaviour, CollisionEffectAgent
    {
        [SerializeField] RectTransform _rectBg;
        [SerializeField] RectTransform _rectRemind;       


        private long _userId;
        public long userId { get { return _userId; } }

        private float _createTime;
        public float createTime { get { return _createTime; } }

        private KinectAgentStatusEnum _status;
        public KinectAgentStatusEnum status { get { return _status; } set { _status = value; } }


        private ICollisionMoveBehavior _collisionMoveBehavior;
        private CardAgent _refCardAgent;
        public CardAgent refCardAgent { set { _refCardAgent = value; } get { return _refCardAgent; } }

        private FlockAgent _refFlockAgent;
        public FlockAgent refFlockAgent { set { _refFlockAgent = value; } get { return _refFlockAgent; } }

        private bool _disableEffect = false;
        private MagicWallManager _manager;

        void Awake() {
            _createTime = Time.time;
            GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);

            // 生成动画
            GetComponent<RectTransform>().DOScale(1f, 1f).OnComplete(()=> {
                _status = KinectAgentStatusEnum.Normal;

                Debug.Log("Width : " + GetWidth());
            });

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

            //Debug.Log("Kinect Agent width : " + width + " / scale : " + scaleVector3);

            return width * scaleVector3.x;
        }

        public bool IsEffective()
        {
            if (_disableEffect)
                return false;

            if (_status == KinectAgentStatusEnum.Normal)
                return true;

            if (_status == KinectAgentStatusEnum.Creating)
                return true;

            if (_status == KinectAgentStatusEnum.Hiding)
                return false;

            if (_status == KinectAgentStatusEnum.Hide)
                return false;

            if (_status == KinectAgentStatusEnum.Destoring)
                return true;

            if (_status == KinectAgentStatusEnum.Obsolete)
                return false;

            if (_status == KinectAgentStatusEnum.Small)
                return true;

            return false;
        }

        public void SetMoveBehavior(ICollisionMoveBehavior moveBehavior)
        {
            _collisionMoveBehavior = moveBehavior;
        }


        /// <summary>
        /// 生成动画
        /// </summary>
        public void Init(long userId,MagicWallManager magicWallManager) {
            _userId = userId;
            _manager = magicWallManager;

            SetMoveBehavior(_manager.collisionMoveBehaviourFactory.GetMoveBehavior(CollisionMoveBehaviourType.KinectRound));
            
            _status = KinectAgentStatusEnum.Creating;


            InitUI();
        }

        /// <summary>
        ///  隐藏动画
        /// </summary>
        public void Hide() {

        }



        /// <summary>
        /// 关闭
        /// </summary>
        public void Close() {
            _status = KinectAgentStatusEnum.Destoring;

            GetComponent<RectTransform>().DOScale(0.1f, 0.5f)
                .OnComplete(() =>
                {
                    _status = KinectAgentStatusEnum.Obsolete;
                });
            // 关闭动画

            //// 关闭

            //// 动画完成后           
            //var MKinectManager = GameObject.Find("kinect").GetComponent<MKinectManager>();
            //MKinectManager.RemoveKinectAgents(this);

        }

        public void UpdatePos(Vector2 anchPos)
        {
            if (_status == KinectAgentStatusEnum.Normal || _status == KinectAgentStatusEnum.Creating) {
                GetComponent<RectTransform>().anchoredPosition = anchPos;
            }

        }

        public float GetEffectDistance()
        {

            CollisionMoveBehaviourFactory collisionMoveBehaviourFactory = GameObject.Find("Collision").GetComponent<CollisionMoveBehaviourFactory>();
            var influenceMoveFactor = collisionMoveBehaviourFactory.GetMoveEffectDistance();
            var effectDistance = GetWidth() * influenceMoveFactor;

            return effectDistance;
        }

        public void SetDisableEffect(bool disableEffect)
        {
            _disableEffect = disableEffect;            
        }



        /// <summary>
        ///     初始化 UI
        /// </summary>
        private void InitUI() {
            var rect = GetComponent<RectTransform>();

            if (_manager.screenTypeEnum == ScreenTypeEnum.Screen1080P)
            {
                rect.sizeDelta = new Vector2(800, 800);
                _rectBg.sizeDelta = new Vector2(1400,1400);
                _rectRemind.sizeDelta = new Vector2(251,135);

            }
            else {
                rect.sizeDelta = new Vector2(800, 800);
                _rectBg.sizeDelta = new Vector2(1400, 1400);
                _rectRemind.sizeDelta = new Vector2(251, 135);
            }
           
        }


        public void RecoverColliderEffect() {
            _disableEffect = false;
            if (GetComponent<RectTransform>().localScale != Vector3.one) {
                GetComponent<RectTransform>().DOScale(1, 0.5f);
            }
        }




    }

}
