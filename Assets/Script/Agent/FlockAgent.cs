using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using EasingUtil;
using System;

namespace MagicWall
{
    public class FlockAgent : MonoBehaviour, CollisionMoveBasicAgent
    {
        public string effectAgentName;

        protected MagicWallManager _manager;

        protected FlockTweenerManager _flockTweenerManager;

        private IFlockAgentMoveBehavior _flockAgentMoveBehavior;

        public FlockTweenerManager flockTweenerManager { get { return _flockTweenerManager; } }

        [SerializeField] protected FlockStatusEnum _flockStatus;

        /// <summary>
        ///  卡片状态
        /// </summary>
        public FlockStatusEnum flockStatus { set { _flockStatus = value; } get { return _flockStatus; } }


        #region Data Parameter 
        private bool _data_iscustom; // 是定制的
        [SerializeField] private string _data_img;    //背景图片
        private int _data_id; // id
        private DataTypeEnum _dataType;
        public DataTypeEnum dataTypeEnum { get { return _dataType; } }

        private AgentContainerType _agentContainerType;


        #endregion

        #region Component Parameter

        private int _sceneIndex;    //  场景的索引

        float _x;
        public float X { get { return _x; } }
        float _y;
        public float Y { get { return _y; } }
        float _z;
        public float Z { get { return _z; } set { _z = value; } }


        private float delayX;

        private float delayY;

        private float delay;

        private float delayTime;

        private float duration;

        // 宽度
        private float _width;

        // 高度
        private float _height;

        /// <summary>
        /// 原位置，anchor position
        /// </summary>
        [SerializeField]  private Vector2 _oriVector2;

        // 生成的位置
        private Vector2 _genVector2;



        // 是否被选中
        [SerializeField] private bool _isChoosing = false;


        // 是否正在恢复
        private bool isRecovering = false;


        /// <summary>
        ///     创建成功
        /// </summary>
        private bool _isCreateSuccess = false;
        public bool isCreateSuccess { set { _isCreateSuccess = value; } get { return _isCreateSuccess; } }


        // 卡片代理
        CardAgent _cardAgent;
        public CardAgent cardAgent { set { _cardAgent = value; } }

        // 能被影响
        private bool _canEffected = true;


        #endregion


        private float _lastEffectTime;
        private CardAgent _lastEffectAgent;    // 上一个影响的 agent
        private bool _effectLastFlag = false;

        private bool _isStarEffect = false;
        public bool isStarEffect { set { _isStarEffect = value; } get { return _isStarEffect; } }


        private float _fallDownStartTime = 0f;
        public float fallDownStartTime { set { _fallDownStartTime = value; } get { return _fallDownStartTime; } }

        private float _fallDelayTime;
        public float fallDelayTime { set { _fallDelayTime = value; } get { return _fallDelayTime; } }

        private float _fallSpeed;
        public float fallSpeed { set { _fallSpeed = value; } get { return _fallSpeed; } }

        /* collision 相关 */

        /// <summary>
        /// 下个移动的位置
        /// https://www.yuque.com/u314548/fc6a5l/yb8hw4#8le6t
        /// </summary>
        [SerializeField] private Vector2 _nextVector2;        
        public Vector2 NextVector2 { set { _nextVector2 = value; } get { return _nextVector2; } }

        private bool _moveFlag = false;
        public bool MoveFlag { set { _moveFlag = value; } get { return _moveFlag; } }


        private bool _hasChangedFlag = false;
        public bool hasChangedFlag { set { _hasChangedFlag = value; } get { return _hasChangedFlag; } }



        private bool _hasMoveOffset = false;

        public bool hasMoveOffset { set { _hasMoveOffset = value; } get { return _hasMoveOffset; } }

        /// <summary>
        ///  变化中的位置
        /// </summary>
        private Vector2 _nextChangedPosition;
        public Vector2 NextChangedPosition { set { _nextChangedPosition = value; } get { return _nextChangedPosition; } }


        /* collision 相关结束 */




        #region 引用
        public string DataImg { set { _data_img = value; } get { return _data_img; } }
        public int DataId { set { _data_id = value; } get { return _data_id; } }
        public bool DataIsCustom { set { _data_iscustom = value; } get { return _data_iscustom; } }
        public AgentContainerType agentContainerType { set { _agentContainerType = value; } get { return _agentContainerType; } }
        public bool CanEffected { set { _canEffected = value; } get { return _canEffected; } }
        public float DelayX { set { delayX = value; } get { return delayX; } }
        public float DelayY { set { delayY = value; } get { return delayY; } }
        public float Delay { set { delay = value; } get { return delay; } }
        public float DelayTime { set { delayTime = value; } get { return delayTime; } }
        public float Duration { set { duration = value; } get { return duration; } }
        public float Width { set { _width = value; } get { return _width; } }
        public float Height { set { _height = value; } get { return _height; } }
        public Vector2 OriVector2 { set { _oriVector2 = value; } get { return _oriVector2; } }
        public Vector2 GenVector2 { set { _genVector2 = value; } get { return _genVector2; } }
        public CardAgent GetCardAgent { get { return _cardAgent; } }
        public int SceneIndex { set { _sceneIndex = value; } get { return _sceneIndex; } }
        #endregion

        // Start is called before the first frame update

        void Start()
        {
            // _flockAgentMoveBehavior = new FlockAgentCommonMoveBehavior();
        }


        /// <summary>
        ///     初始化基础数据
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="dataId"></param>
        /// <param name="type"></param>
        /// <param name="isCard"></param>
        protected void InitBase(MagicWallManager manager, int dataId, DataTypeEnum dataType)
        {
            //Debug.Log("Init Base : " + dataId);

            _manager = manager;
            _data_id = dataId;
            _dataType = dataType;

            _flockTweenerManager = new FlockTweenerManager();
            _flockStatus = FlockStatusEnum.NORMAL;
        }


        /// <summary>
        ///         初始化 Agent 信息,在生成浮动块时调用
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="originVector">在屏幕上显示的位置</param>
        /// <param name="genVector">出生的位置</param>
        /// <param name="row">x</param>
        /// <param name="column">y</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="dataId"></param>
        /// <param name="dataImg"></param>
        /// <param name="dataIsCustom"></param>
        /// <param name="dataType"></param>
        public virtual void Initialize(MagicWallManager manager, Vector2 originVector, Vector2 genVector, int row,
            int column, float width, float height, int dataId, string dataImg, bool dataIsCustom, DataTypeEnum dataTypeEnum, AgentContainerType agentContainerType)
        {
            InitBase(manager, dataId, dataTypeEnum);
            _manager = manager;
            OriVector2 = originVector;

            //  出生位置
            GenVector2 = genVector;
            GetComponent<RectTransform>().anchoredPosition = genVector;

            _nextVector2 = genVector;

            _x = row;
            _y = column;
            _width = width;
            _height = height;

            // 设置组件长宽
            GetComponent<RectTransform>().sizeDelta = new Vector2(_width, _height);

            _data_img = dataImg;
            _data_iscustom = dataIsCustom;

            // 设置显示图片
            //GetComponent<RawImage>().texture = TextureResource.Instance.GetTexture(AppUtils.GetFullFileAddressOfImage(DataImg));
            GetComponent<Image>().sprite = SpriteResource.Instance.GetData(AppUtils.GetFullFileAddressOfImage(DataImg));

            // 定义 agent 的名字
            int si = _manager.SceneIndex;
            _sceneIndex = si + 0;

            _agentContainerType = agentContainerType;

        }


        #region 点击选择

        public void DoChoose()
        {

            _manager.agentManager.agentChooseBehavior.DoChoose(this);

        }

        #endregion

        #region 恢复

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position">world position</param>
        public void DoRecoverAfterChoose(Vector3 position)
        {
            Debug.Log("DoRecoverAfterChoose");


            if (_flockStatus != FlockStatusEnum.HIDE) {
                return;
            }

            _flockStatus = FlockStatusEnum.RECOVER;

            //  将原组件启用
            gameObject.SetActive(true);
            
            transform.SetParent(GetParentContainer());


            // 调整位置
            RectTransform rect = GetComponent<RectTransform>();

            rect.position = position;


            // 恢复原位
            RecoverToOriginPosition();

        }

        /// <summary>
        /// 恢复原位
        /// </summary>
        public void RecoverToOriginPosition() {
            if (_manager.SceneIndex == _sceneIndex) {
                Vector3 to = new Vector3(OriVector2.x, OriVector2.y, 0);
                Tweener t2 = GetComponent<RectTransform>().DOAnchorPos3D(to, 0.3f);
                _flockTweenerManager.Add(FlockTweenerManager.FlockAgent_DoRecoverAfterChoose_DOAnchorPos3D, t2);

                // 放大至原大小
                Vector3 scaleVector3 = Vector3.one;

                // 在放大动画开始前，标记该组件为不被选择的

                Tweener t = GetComponent<RectTransform>().DOScale(scaleVector3, 1f)
                   .OnUpdate(() =>
                   {
                       Width = GetComponent<RectTransform>().sizeDelta.x;
                       Height = GetComponent<RectTransform>().sizeDelta.y;
                   }).OnComplete(() =>
                   {
                       flockStatus = FlockStatusEnum.NORMAL;

                       Debug.Log("放大动画 completed");

                   }).OnKill(() =>
                   {

               });

                _flockTweenerManager.Add(FlockTweenerManager.FlockAgent_DoRecoverAfterChoose_DOScale, t);
            }
        }




        #endregion






        //  删除代理
        public void DestoryAgency()
        {
            // 清除 Dotween 代理
            _flockStatus = FlockStatusEnum.OBSOLETE;
            // 删除 Gameobject
        }


        public void UpdateImageAlpha(float alpha)
        {
            Color color = GetComponent<Image>().color;
            color.a = alpha;
            GetComponent<Image>().color = color;

        }


        /// <summary>
        ///     重置 Agent
        /// </summary>
        public void Reset()
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            _flockTweenerManager.Reset();
            _flockTweenerManager = new FlockTweenerManager();



            // 透明度调整
            if (GetComponent<Image>().color != new Color(255, 255, 255, 255))
            {
                GetComponent<Image>().color = new Color(255, 255, 255, 255);
            }

            // 设置 scale
            Vector3 scale = new Vector3(1, 1, 1);
            if (GetComponent<RectTransform>().localScale != scale)
            {
                GetComponent<RectTransform>().localScale = scale;
            }

            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);

            GetComponent<Image>().sprite = null;
            isCreateSuccess = false;
            CanEffected = true;
            _oriVector2 = Vector2.zero;
            _nextVector2 = Vector2.zero;

            _flockStatus = FlockStatusEnum.PREPARED;

            

        }


        /// <summary>
        /// 检查是否需要回收
        /// </summary>
        public bool CheckIsNeedRecycle()
        {
            var result = false;

            // 如果在运行中的 flock，已经远远离开屏幕，则进行销毁
            if (_flockStatus == FlockStatusEnum.NORMAL && (_manager.SceneStatus != WallStatusEnum.Cutting)                )
            {

                //Vector3 position = Camera.main.WorldToScreenPoint(GetComponent<RectTransform>().transform.position);
                Vector3 position = GetComponent<RectTransform>().transform.position;

                //Debug.Log("Position: " + GetComponent<RectTransform>().transform.position);

                if (position.x + (Width * 3) < 0)
                {
                    result = true;
                }
                else if (position.x - (Width * 3) > Screen.width)
                {
                    result = true;

                }
                else if (position.y + (Height * 3) < 0)
                {
                    result = true;

                }
                else if (position.y - (Height * 3) > Screen.height)
                {
                    result = true;
                }
            }

            if (result) {
                //Debug.Log(gameObject.name + " 废弃 - " + _flockStatus);
                _flockStatus = FlockStatusEnum.OBSOLETE;
            }

            return result;
        }



        /* CollisionMoveBasicAgent Impl 相关 */

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="effectAgents"></param>
        /// <returns></returns>
        public void CalculateEffectedDestination(List<CollisionEffectAgent> effectAgents)
        {
            bool needReturnPositionFlag = false;

            if (effectAgents == null || effectAgents.Count == 0 || (!_canEffected) || (_flockStatus == FlockStatusEnum.STAR)){
                needReturnPositionFlag = true;
            }
            else { 
                // basic position
                Vector2 refPosition = GetCollisionRefPosition();    

                CollisionEffectAgent targetAgent = null;

                // target position
                Vector2 targetPosition = new Vector2();

                float distance = 1000f;

                for (int i = 0; i < effectAgents.Count; i++)
                {
                    var item = effectAgents[i];

                    if (!item.IsEffective())
                    {
                        continue;
                    }
                    else {
                        Vector2 itemPosition = item.GetRefPosition();

                        float newDistance = Vector2.Distance(refPosition, itemPosition);

                        if (newDistance < distance)
                        {
                            distance = newDistance;
                            targetAgent = item;
                            targetPosition = itemPosition;
                        }
                    }                    
                }

                float effectDistance;
                float w, h;
                if (targetAgent != null)
                {
                    w = targetAgent.GetWidth();
                    h = targetAgent.GetHeight();
                    effectDistance = targetAgent.GetEffectDistance();
                }
                else
                {
                    w = 0;
                    h = 0;
                    effectDistance = 0;
                }
                                
                // 获取差值，差值越大，则表明两个物体距离越近，MAX（offsest） = effectDistance
                float offset = effectDistance - distance;

                // 进入影响范围
                if (offset >= 0)
                {
                    effectAgentName = targetAgent.GetName();
                    

                    TurnOnHasMovedOffsetFlag();

                    var moveBehavior = targetAgent.GetMoveBehavior();
                    //targetPosition = targetAgent.GetRefPosition();

                    /// 受影响浮块具体实现
                    /// 
                    if (moveBehavior == null) {
                        Debug.Log("target agent name : " + targetAgent.GetName());
                    }

                    Vector2 to = moveBehavior.CalculatePosition(refPosition,
                        targetPosition, distance,
                        effectDistance, w, h, _manager);


                    var toLocalPosition = TransformScreenPositionToRectPosition(to);
;
                    SetNextPosition(toLocalPosition);

                    float sc = moveBehavior.CalculateScale(refPosition,
                                targetPosition, distance,
                                effectDistance, w, h, _manager);

                    // 获取缓动方法
                    Func<float, float> defaultEasingFunction = EasingFunction.Get(_manager.flockBehaviorConfig.CommonEaseEnum);
                    float k = defaultEasingFunction(offset / effectDistance);

                    GetComponent<RectTransform>()?.DOScale(sc, Time.deltaTime);
                }
                else {
                    needReturnPositionFlag = true;
                }

            }


            if (needReturnPositionFlag) {
                if (_hasMoveOffset)
                {
                    TurnOffHasMovedOffsetFlag();
                    RecoverPosition();
                    GetComponent<RectTransform>()?.DOScale(1, Time.deltaTime);
                }
                else {
                    if (_hasChangedFlag) {
                        _hasChangedFlag = false;
                        SetNextPosition(_nextChangedPosition);                        
                    }

                }
            }

        }


        /// <summary>
        /// 获取碰撞参考位置,此时的参考位置应该是默认存在的位置，而不是被偏移过的位置
        /// </summary>
        /// <returns>屏幕坐标</returns>
        public Vector3 GetCollisionRefPosition()
        {
            // _oriVector2
            var refVector2 = new Vector2();

            if (_hasChangedFlag)
            {
                refVector2 = _nextChangedPosition;

            }
            else if (isCreateSuccess)
            {
                //当前场景为正常展示时，参考位置为固定位置
                refVector2 = _oriVector2;
            }
            
            else {
                // 当前场景正在切换时，参考位置为目标的下个移动位置
                refVector2 = _nextChangedPosition;
            }


            Vector2 refVector2WithOffset;


            if (_agentContainerType == AgentContainerType.MainPanel)
            {
                refVector2WithOffset = refVector2 - new Vector2(_manager.PanelOffsetX, _manager.PanelOffsetY); //获取带偏移量的参考位置
            }
            else if (_agentContainerType == AgentContainerType.BackPanel) {
                refVector2WithOffset = refVector2 - new Vector2(_manager.PanelBackOffsetX, _manager.PanelOffsetY); //获取带偏移量的参考位置
            }
            else
            {
                refVector2WithOffset = refVector2 - new Vector2(_manager.PanelOffsetX, _manager.PanelOffsetY); //获取带偏移量的参考位置
            }

            // refVector2 此时该数据需要进行修改偏移量
            var screenPosition = RectTransformUtility.WorldToScreenPoint(null, refVector2WithOffset);
            return screenPosition;
        }


        //public Vector3 GetNextMovePosition




        public void UpdatePosition(List<CollisionEffectAgent> effectAgents)
        {
            //Debug.Log(gameObject.name + " Start ");

            // 隐藏中的agent不需要修改位置
            if (_flockStatus == FlockStatusEnum.HIDE) {
                return;
            }

            // 判断碰撞位置
            CalculateEffectedDestination(effectAgents);

            if (MoveFlag)
            {
                var ap = GetComponent<RectTransform>().anchoredPosition;

                // 移动到下个位置
                GetComponent<RectTransform>().anchoredPosition = NextVector2;
                MoveFlag = false;

            }
            else {

            }

            //Debug.Log(gameObject.name + " End ");
        }

        public void TurnOnHasMovedOffsetFlag()
        {
            _hasMoveOffset = true;
        }

        public void TurnOffHasMovedOffsetFlag()
        {
            _hasMoveOffset = false;
        }

        public void SetChangedPosition(Vector3 vector)
        {
            _nextChangedPosition = vector;
            _hasChangedFlag = true;
        }

        public void SetNextPosition(Vector3 vector)
        {
            NextVector2 = vector;
            MoveFlag = true;
        }

        void RecoverPosition() {
            if (_hasChangedFlag)
            {
                SetNextPosition(_nextChangedPosition);
            }
            else {
                SetNextPosition(_oriVector2);
            }

        }

        Vector2 TransformScreenPositionToRectPosition(Vector2 screenPosition) {
            RectTransform container;

            if (_agentContainerType == AgentContainerType.MainPanel)
            {
                container = _manager.mainPanel;
            }
            else if (_agentContainerType == AgentContainerType.BackPanel)
            {
                container = _manager.backPanel;
            }
            else {
                container = _manager.mainPanel;
            }

            // 将屏幕坐标转换为rect 坐标
            var localPosition = new Vector2();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(container, screenPosition, null, out localPosition);
          
            var panelAnchorPosition = new Vector2(_manager.mainPanel.GetComponent<RectTransform>().rect.width / 2,
                _manager.mainPanel.GetComponent<RectTransform>().rect.height / 2);
            localPosition += panelAnchorPosition;

            return localPosition;
        }



        /* CollisionMoveBasicAgent 相关 结束 */

        private Transform GetParentContainer() {
            if (_agentContainerType == AgentContainerType.MainPanel)
            {
                return _manager.mainPanel;
            }
            else if (_agentContainerType == AgentContainerType.BackPanel)
            {
                return _manager.backPanel;
            }
            else {
                return _manager.starEffectContainer;
            }


        }


    }

}