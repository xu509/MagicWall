using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace MagicWall
{

    /// <summary>
    ///   体感实体
    /// </summary>
    public class KinectAgent : MonoBehaviour, CollisionEffectAgent
    {
        [SerializeField] RectTransform _rectBg;
        [SerializeField] RectTransform _rectRemind;


        /// <summary>
        /// 上一次移动的时间点
        /// </summary>
        private float _lastMoveTime = -1f;


        private FlockTweenerManager _flockTweenerManager;
        public FlockTweenerManager flockTweenerManager { get { return _flockTweenerManager; } }



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
        public bool disableEffect { get { return _disableEffect; } }

        private MagicWallManager _manager;
        private float _ignoreValue;
        private float _moveDelayTime;

        private float _destoryStartTime = -1f;
        private float _destoryDelayTime = 3f;


        void Awake() {
            _flockTweenerManager = new FlockTweenerManager();

            _createTime = Time.time;
            GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);

            // 生成动画
            GetComponent<RectTransform>().DOScale(1f, 1f).OnComplete(()=> {
                _status = KinectAgentStatusEnum.Normal;

                //Debug.Log("Width : " + GetWidth());
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
                return true;

            if (_status == KinectAgentStatusEnum.Hide)
                return true;

            if (_status == KinectAgentStatusEnum.Destoring)
                return true;

            if (_status == KinectAgentStatusEnum.Obsolete)
                return false;

            if (_status == KinectAgentStatusEnum.Small)
                return true;

            if (_status == KinectAgentStatusEnum.WaitingHiding)
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
            _ignoreValue = FindObjectOfType<MKinectManager>().ignoreValue;
            _moveDelayTime = FindObjectOfType<MKinectManager>().agentMoveDelayTime;
            SetMoveBehavior(_manager.collisionMoveBehaviourFactory.GetMoveBehavior(CollisionMoveBehaviourType.KinectRound,1));
            
            _status = KinectAgentStatusEnum.Creating;

            InitUI();
        }

        /// <summary>
        ///  隐藏动画
        /// </summary>
        public void Hide() {
            _status = KinectAgentStatusEnum.Hiding;

            _status = KinectAgentStatusEnum.Hide;

            //// 隐藏动画
            //_rectRemind.GetComponent<Image>().DOFade(0, 1f)
            //    .OnComplete(()=> {
            //        _status = KinectAgentStatusEnum.Hide;
            //    });
        }


        /// <summary>
        /// 关闭
        /// </summary>
        public void Close() {
            if (_status != KinectAgentStatusEnum.Destoring) {
                _status = KinectAgentStatusEnum.Destoring;

                //_destoryStartTime


                var closeAnimi = GetComponent<RectTransform>().DOScale(0.1f, 0.5f)
                    .OnComplete(() =>
                    {
                        _status = KinectAgentStatusEnum.Obsolete;
                    });

                _flockTweenerManager.Add(FlockTweenerManager.Kinnect_Close, closeAnimi);
            }
        }

        public void CancelClose() {
            _status = KinectAgentStatusEnum.Recovering;

            _flockTweenerManager.Get(FlockTweenerManager.Kinnect_Close).Kill();

            var canelClose = GetComponent<RectTransform>().DOScale(1f, 0.5f)
                .OnComplete(() =>
                {
                    _status = KinectAgentStatusEnum.Normal;

                    Debug.Log("取消关闭成功");

                });
            _flockTweenerManager.Add(FlockTweenerManager.Kinnect_Close_Cancel, canelClose);

        }


        public void UpdatePos(Vector2 anchPos)
        {
            Debug.Log("@@@ Update Position : " + _status);

            if (_status == KinectAgentStatusEnum.Normal || _status == KinectAgentStatusEnum.Creating) {
                float currentX = GetComponent<RectTransform>().anchoredPosition.x;
                //if (Mathf.Abs(currentX-anchPos.x) < _ignoreValue)
                //{
                //    return;
                //}
                //GetComponent<RectTransform>().anchoredPosition = anchPos;
                if (Time.time - _lastMoveTime > _moveDelayTime)
                {
                    _lastMoveTime = Time.time;

                    Debug.Log("@@@ Update Position :  开始移动 " );


                    GetComponent<RectTransform>().DOAnchorPosX(anchPos.x, _moveDelayTime).OnComplete(() =>
                    {
                    });
                    //GetComponent<RectTransform>().anchoredPosition = anchPos;
                }
                //Debug.Log("update pos : " + _status);

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
            if (disableEffect) {
                print(gameObject.name + " Set Disabled false - " + status);
            }
            
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



        /// <summary>
        /// 实现功能：
        /// 跟随依附卡片变化大小
        /// 跟随依附卡片移动
        /// 当卡片消失时，跟随小时
        /// 
        /// </summary>
        public void UpdateBehaviour() {

            if (refFlockAgent == null) {
                if (disableEffect) {
                    Close();
                }                
            }

            // 存在点开的卡片
            if ((refFlockAgent != null) && (refFlockAgent.GetCardAgent != null))
            {
                var cardAgent = refFlockAgent.GetCardAgent;
                //Debug.Log(cardAgent.name + " status :" + cardAgent._cardStatus);

                if (cardAgent._cardStatus == CardStatusEnum.DESTORYINGFIRST)
                {
                    var cardScale = cardAgent.GetComponent<RectTransform>().localScale;
                    GetComponent<RectTransform>().localScale = cardScale;
                    status = KinectAgentStatusEnum.Small;
                }

                if (cardAgent._cardStatus == CardStatusEnum.RECOVER)
                {
                    var cardScale = cardAgent.GetComponent<RectTransform>().localScale;
                    GetComponent<RectTransform>().localScale = cardScale;
                    status = KinectAgentStatusEnum.Recovering;
                }

                if (cardAgent._cardStatus == CardStatusEnum.DESTORYINGSECOND)
                {
                    Close();
                }

                if (cardAgent._cardStatus == CardStatusEnum.MOVE)
                {
                    transform.position = cardAgent.transform.position;
                }
            }



        }



    }

}
