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
        private string _data_img;    //背景图片
        private int _data_id; // id
        private DataTypeEnum _dataType;

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

        // 原位
        private Vector2 _oriVector2;

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


        /// <summary>
        ///     flock 移动状态
        /// </summary>
        private AgentMoveStatus _agentMoveStatus;
        public AgentMoveStatus agentMoveStatus { set { _agentMoveStatus = value; } get { return _agentMoveStatus; } }

        // 卡片代理
        CardAgent _cardAgent;

        // 能被影响
        private bool _canEffected = true;


        #endregion


        private float _lastEffectTime;
        private CardAgent _lastEffectAgent;    // 上一个影响的 agent
        private bool _effectLastFlag = false;

        private bool _isStarEffect = false;
        public bool isStarEffect { set { _isStarEffect = value; } get { return _isStarEffect; } }

        /* collision 相关 */

        /// <summary>
        /// 下个移动的位置
        /// https://www.yuque.com/u314548/fc6a5l/yb8hw4#8le6t
        /// </summary>
        private Vector2 _nextVector2;        
        public Vector2 NextVector2 { set { _nextVector2 = value; } get { return _nextVector2; } }

        private bool _moveFlag = false;
        public bool MoveFlag { set { _moveFlag = value; } get { return _moveFlag; } }

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

        /// <summary>
        ///     位置更新状态依据： https://www.yuque.com/docs/share/244127ea-46a4-4fe8-baca-55e4d333ffc1
        /// </summary>
        #region 更新位置
        public void updatePosition()
        {
            // 如果是被选中，并打开的
            if (_flockStatus == FlockStatusEnum.RUNIN || _flockStatus == FlockStatusEnum.NORMAL)
            {
                // 当需要判断位置时
                if (NeedAdjustPostion())
                {
                    UpdatePositionEffect();
                }
                else {
                    //Debug.Log("No Need To Adjust Position");

                }
            }
        }

        private void UpdatePositionEffect()
        {

            Vector2 refVector2; // 参照的目标位置
            if (_manager.SceneStatus == WallStatusEnum.Cutting)
            {
                // 当前场景正在切换时，参考位置为目标的下个移动位置
                refVector2 = NextVector2;
            }
            else
            {
                //当前场景为正常展示时，参考位置为固定位置
                refVector2 = _oriVector2;
            }
            Vector2 refVector2WithOffset = refVector2 - new Vector2(_manager.PanelOffsetX, _manager.PanelOffsetY); //获取带偏移量的参考位置

            // 此时的坐标位置可能已处于偏移状态
            RectTransform m_transform = GetComponent<RectTransform>();

            if (m_transform == null)
            {
                return;
            }


            // 获取施加影响的目标物
            //  判断是否有多个影响体，如有多个，取距离最近的那个
            List<CardAgent> transforms = _manager.operateCardManager.EffectAgents;

            CardAgent targetAgent = null;
            Vector2 targetVector2; // 目标物位置
            float distance = 1000f;

            foreach (CardAgent item in transforms)
            {
                // 判断大小，如果item还过小则不认为是影响的
                if (!IsEffectiveTarget(item))
                {
                    continue;
                }

                Vector2 effectPosition = item.GetComponent<RectTransform>().anchoredPosition;

                float newDistance = Vector2.Distance(refVector2WithOffset, effectPosition);
                if (newDistance < distance)
                {
                    distance = newDistance;
                    targetAgent = item;
                }
            }
            float w, h;
            if (targetAgent != null)
            {
                Vector3 scaleVector3 = targetAgent.GetComponent<RectTransform>().localScale;
                w = targetAgent.width * scaleVector3.x;
                h = targetAgent.height * scaleVector3.y;

            }
            else
            {
                w = 0;
                h = 0;
            }
            // 判断结束

            // 获取有效影响范围，是宽度一半以上
            float effectDistance = (w / 2) + (w / 2) * _manager.flockBehaviorConfig.InfluenceMoveFactor;
            // 获取差值，差值越大，则表明两个物体距离越近，MAX（offsest） = effectDistance
            float offset = effectDistance - distance;

            // 进入影响范围
            if (offset >= 0)
            {
                targetVector2 = targetAgent.GetComponent<RectTransform>().anchoredPosition;

                _flockAgentMoveBehavior = _manager.moveBehaviourFactory
                    .GetMoveBehavior(_manager.flockBehaviorConfig.MoveBehaviourType);

                //_flockAgentMoveBehavior = _manager.moveBehaviourType


                /// 受影响浮块具体实现
                Vector2 to = _flockAgentMoveBehavior.CalculatePosition(refVector2, refVector2WithOffset,
                    targetVector2, distance,
                    effectDistance, w, h, _manager);

                float sc = _flockAgentMoveBehavior.CalculateScale(refVector2, refVector2WithOffset,
                            targetVector2, distance,
                            effectDistance, w, h, _manager);


                // 获取缓动方法
                Func<float, float> defaultEasingFunction = EasingFunction.Get(_manager.flockBehaviorConfig.CommonEaseEnum);
                float k = defaultEasingFunction(offset / effectDistance);

                //m_transform?.DOAnchorPos(Vector2.Lerp(refVector2, to, k), Time.deltaTime);
                m_transform?.DOAnchorPos(to, Time.deltaTime);
                //m_transform?.DOScale(Mathf.Lerp(1f, 0.1f, k), Time.deltaTime);
                m_transform?.DOScale(sc, Time.deltaTime);

                // 记录影响的数据
                if (!_effectLastFlag)
                {
                    _lastEffectTime = Time.time;
                    _lastEffectAgent = targetAgent;
                }

            }
            else
            // 未进入影响范围
            {
                var ap = GetComponent<RectTransform>().anchoredPosition;
                //Debug.Log(Vector2.Distance(ap, refVector2));

                if (Vector2.Distance(ap, refVector2) > 0.1f) {
                    Vector2 toy = new Vector2(refVector2.x, refVector2.y);
                    //m_transform?.DOAnchorPos(toy, Time.deltaTime);

                    GetComponent<RectTransform>().anchoredPosition = toy;

                }


                if (m_transform.localScale != Vector3.one)
                {
                    m_transform?.DOScale(1, Time.deltaTime);
                }
            }


        }
        #endregion

        #region 点击选择

        public void DoChoose()
        {

            if (CanChoose())
            {
                _flockStatus = FlockStatusEnum.TOHIDE;

                //_isChoosing = true;

                //  先缩小（向后退）
                RectTransform rect = GetComponent<RectTransform>();
                Vector2 positionInMainPanel = rect.anchoredPosition;

                //  移到后方、缩小、透明
                //rect.DOScale(0.1f, 0.3f);
                Vector3 to = new Vector3(0.2f,0.2f,0.7f);

                var _cardGenPos = GetCardGeneratePosition();

                // 完成缩小与移动后创建十字卡片
                rect.DOScale(0.5f, 0.3f).OnComplete(() =>
                {
                    _flockStatus = FlockStatusEnum.HIDE;
                    gameObject.SetActive(false);

                    //Debug.Log("chose :" + _data_id);

                    _cardAgent = _manager.operateCardManager.CreateNewOperateCard(_data_id, _dataType, _cardGenPos, this);


                    //靠近四周边界需要偏移
                    float w = _cardAgent.GetComponent<RectTransform>().rect.width;
                    float h = _cardAgent.GetComponent<RectTransform>().rect.height;

                    // 如果点击时,出生位置在最左侧
                    if (_cardGenPos.x < w / 2)
                    {
                        _cardGenPos.x = w / 2;
                    }

                    // 出身位置在最右侧
                    if (_cardGenPos.x > _manager.OperationPanel.rect.width - w / 2)
                    {
                        _cardGenPos.x = _manager.OperationPanel.rect.width - w / 2;
                    }

                    // 出生位置在最下侧
                    if (_cardGenPos.y < h / 2)
                    {
                        _cardGenPos.y = h / 2;
                    }

                    // 出生位置在最上侧
                    if (_cardGenPos.y > _manager.OperationPanel.rect.height - h / 2)
                    {
                        _cardGenPos.y = _manager.OperationPanel.rect.height - h / 2;
                    }

                    _cardAgent.GetComponent<RectTransform>().anchoredPosition = _cardGenPos;

                    _cardAgent.GoToFront();

                });


            }
        }

        #endregion

        #region 恢复

        public void DoRecoverAfterChoose()
        {

            if (_flockStatus != FlockStatusEnum.HIDE) {
                return;
            }

            //gameObject == null;
            Debug.Log("status : " + _flockStatus);

            _flockStatus = FlockStatusEnum.RECOVER;

            //// 如果组件已不在原场景，则不进行恢复
            //if (_sceneIndex != _manager.SceneIndex)
            //{
            //    gameObject.SetActive(false);
            //    DestoryAgency();
            //    return;
            //}

            //  将原组件启用
            gameObject.SetActive(true);

            // 调整位置
            RectTransform rect = GetComponent<RectTransform>();
            RectTransform cardRect = _cardAgent.GetComponent<RectTransform>();

            rect.anchoredPosition3D = new Vector3(cardRect.anchoredPosition3D.x + _manager.PanelOffsetX,
                cardRect.anchoredPosition3D.y + _manager.PanelOffsetY,
                cardRect.anchoredPosition3D.z);

            // 恢复原位
            Vector3 to = new Vector3(OriVector2.x, OriVector2.y, 0);
            Tweener t2 = rect.DOAnchorPos3D(to, 0.3f);
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
                   //flockStatus = FlockStatusEnum.OBSOLETE;

                   //Debug.Log("放大动画 kill");
               });

            _flockTweenerManager.Add(FlockTweenerManager.FlockAgent_DoRecoverAfterChoose_DOScale, t);

        }


        #endregion



       /// <summary>
       /// 调整位置的前置条件
       /// </summary>
       /// <returns></returns>
        private bool NeedAdjustPostion()
        {
            //Debug.Log("Check adjust : " + _manager.operateCardManager.EffectAgents.Count);


            if (_flockStatus == FlockStatusEnum.NORMAL) {

                // 当前位置与目标位置一致
                bool NoEffectAgent = (_manager.operateCardManager.EffectAgents.Count == 0);

                if (!NoEffectAgent)
                {

                    //Debug.Log("NoEffectAgent is false");

                    return true;
                }
            }




            Vector2 position = GetComponent<RectTransform>().anchoredPosition;
            bool InOriginPosition = AppUtils.CheckVectorIsEqual(position, NextVector2);

            // 如果没有影响的agent，并且位置没有改变，则不需要调整位置
            if (InOriginPosition)
            {
                _agentMoveStatus = AgentMoveStatus.Regular;
                return false;
            }

            _agentMoveStatus = AgentMoveStatus.Changing;
            return true;
        }


        //  判断目标是否是有效的
        private bool IsEffectiveTarget(CardAgent cardAgent)
        {
            if (!cardAgent.gameObject.activeSelf)
            {
                return false;
            }

            if (cardAgent.CardStatus == CardStatusEnum.HIDE
                || cardAgent.CardStatus == CardStatusEnum.OBSOLETE) {
                return false;
            }



            float effect_width = 300f;
            float effect_height = 300f;

            Vector3 scaleVector3 = cardAgent.GetComponent<RectTransform>().localScale;
            float width = cardAgent.GetComponent<RectTransform>().rect.width;
            float height = cardAgent.GetComponent<RectTransform>().rect.height;

            if (width > effect_width && height > effect_height)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

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
            if (_flockStatus == FlockStatusEnum.NORMAL && _manager.SceneStatus != WallStatusEnum.Cutting)
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
                _flockStatus = FlockStatusEnum.OBSOLETE;
            }



            return result;
        }


        /// <summary>
        /// 判断可以被选择
        /// </summary>
        /// <returns></returns>
        private bool CanChoose() {
            bool canChoose = false;

            if (_flockStatus == FlockStatusEnum.NORMAL
                || _flockStatus == FlockStatusEnum.RUNIN
                || _flockStatus == FlockStatusEnum.STAR) {
                canChoose = true;
            }
            return canChoose;
        }


        private Vector3 GetCardGeneratePosition() {
            Vector3 position = new Vector3();

            var rect = GetComponent<RectTransform>();


            //  获取卡片生成位置
            Vector3 cardGenPosition = new Vector3(rect.anchoredPosition.x - _manager.PanelOffsetX - 1f,
                    rect.anchoredPosition.y - _manager.PanelOffsetY - 1f,
                    200);

            if (_agentContainerType == AgentContainerType.MainPanel)
            {
                cardGenPosition = new Vector3(rect.anchoredPosition.x - _manager.PanelOffsetX - 1f, rect.anchoredPosition.y - _manager.PanelOffsetY - 1f, 200);
            }
            else if (_agentContainerType == AgentContainerType.BackPanel)
            {
                cardGenPosition = new Vector3(rect.anchoredPosition.x - _manager.PanelBackOffsetX - 1f, rect.anchoredPosition.y - _manager.PanelOffsetY - 1f, 200);
            }
            else if (_agentContainerType == AgentContainerType.StarContainer)
            {
                // 获取屏幕坐标
                Vector2 v = RectTransformUtility.WorldToScreenPoint(_manager.starCamera, transform.position);

                // 需要屏幕坐标转为某UGUI容器内的坐标

                Vector2 refp;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(_manager.OperationPanel, v, null, out refp);

                refp = new Vector2(refp.x + _manager.OperationPanel.rect.width / 2, refp.y + _manager.OperationPanel.rect.height / 2);

                cardGenPosition = refp;
            }


            return cardGenPosition;
        }




        /* CollisionMoveBasicAgent 相关 */

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="effectAgents"></param>
        /// <returns></returns>
        public void CalculateEffectedDestination(List<CollisionEffectAgent> effectAgents)
        {
            Vector2 refPosition = GetCollisionRefPosition();

            CollisionEffectAgent targetAgent = null;
            Vector2 targetPosition; // 目标物位置
            float distance = 1000f;

            for (int i = 0; i < effectAgents.Count; i++)
            {
                var item = effectAgents[i];

                if (!item.IsEffective())
                {
                    continue;
                }

                Vector2 effectPosition = item.GetRefPosition();

                float newDistance = Vector2.Distance(refPosition, effectPosition);
                if (newDistance < distance)
                {
                    distance = newDistance;
                    targetAgent = item;
                }
            }

            float w, h;
            if (targetAgent != null)
            {
                w = targetAgent.GetWidth();
                h = targetAgent.GetHeight();

            }
            else
            {
                w = 0;
                h = 0;
            }


            // 获取有效影响范围，是宽度一半以上
            float effectDistance = (w / 2) + (w / 2) * _manager.flockBehaviorConfig.InfluenceMoveFactor;
            // 获取差值，差值越大，则表明两个物体距离越近，MAX（offsest） = effectDistance
            float offset = effectDistance - distance;

            // 进入影响范围
            if (offset >= 0)
            {
                var moveBehavior = targetAgent.GetMoveBehavior();
                targetPosition = targetAgent.GetRefPosition();


                /// 受影响浮块具体实现
                Vector2 to = moveBehavior.CalculatePosition(refPosition,
                    targetPosition, distance,
                    effectDistance, w, h, _manager);

                float sc = moveBehavior.CalculateScale(refPosition,
                            targetPosition, distance,
                            effectDistance, w, h, _manager);


                // 获取缓动方法
                Func<float, float> defaultEasingFunction = EasingFunction.Get(_manager.flockBehaviorConfig.CommonEaseEnum);
                float k = defaultEasingFunction(offset / effectDistance);

                //m_transform?.DOAnchorPos(Vector2.Lerp(refVector2, to, k), Time.deltaTime);
                UpdateNextPosition(to);
                //GetComponent<RectTransform>()?.DOAnchorPos(to, Time.deltaTime);
                //m_transform?.DOScale(Mathf.Lerp(1f, 0.1f, k), Time.deltaTime);
                GetComponent<RectTransform>()?.DOScale(sc, Time.deltaTime);
            }


        }


        /// <summary>
        /// 获取碰撞参考位置
        /// </summary>
        /// <returns>屏幕坐标</returns>
        public Vector3 GetCollisionRefPosition()
        {



            throw new NotImplementedException();
        }

        public void UpdateNextPosition(Vector3 vector)
        {
            NextVector2 = vector;
            MoveFlag = true;
        }

        public void UpdatePosition(List<CollisionEffectAgent> effectAgents)
        {
            // 判断碰撞位置
            CalculateEffectedDestination(effectAgents);

            if (MoveFlag) {
                // 移动到下个位置
                GetComponent<RectTransform>().anchoredPosition = NextVector2;
                MoveFlag = false;
            }
        }

        /* CollisionMoveBasicAgent 相关 结束 */

    }

    /// <summary>
    /// 移动状态
    /// </summary>
    public enum AgentMoveStatus
    {
        Regular, // 正常
        Changing // 变化中
    }


}