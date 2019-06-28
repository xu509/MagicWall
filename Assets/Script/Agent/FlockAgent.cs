using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using EasingUtil;
using System;


public class FlockAgent : MonoBehaviour
{
    protected MagicWallManager _manager;
    protected AgentManager _agentManager;


    #region Data Parameter 
    private bool _data_iscustom; // 是定制的
    private string _data_img;    //背景图片
    private int _data_id; // id
    private MWTypeEnum _type;
    private bool _isCard;
    private AgentContainerType _agentContainerType;


    #endregion

    #region Component Parameter

    private int _sceneIndex;    //  场景的索引

    int x;
    int y;

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
    private Vector2 oriVector2;

    // 生成的位置
    private Vector2 genVector2;

    // 下个移动的位置
    private Vector2 nextVector2;

    // 是否被选中
    private bool _isChoosing = false;

    // 是否被改变
    private bool isChanging = false;

    // 是否正在恢复
    private bool isRecovering = false;

    //
    private bool _StarsCutEffectIsPlaying = false;


    // 卡片代理
    CardAgent _cardAgent;
		
    RectTransform agentRectTransform;

    // 能被影响
    private bool _canEffected = true;

    //  工厂 & 管理器
    ItemsFactory _itemsFactory;


    #endregion

    #region 引用
    public string DataImg { set { _data_img = value; } get { return _data_img; } }
    public int DataId { set { _data_id = value; } get { return _data_id; } }
    public bool DataIsCustom { set { _data_iscustom = value; } get { return _data_iscustom; } }
    public bool IsCard { set { _isCard = value; } get { return _isCard; } }
    public MWTypeEnum type { set { _type = value; } get { return _type; } }
    public AgentContainerType agentContainerType { set { _agentContainerType = value; } get { return _agentContainerType; } }
    public bool CanEffected { set { _canEffected = value; } get { return _canEffected; } }
    public float DelayX { set { delayX = value; } get { return delayX; } }
    public float DelayY { set { delayY = value; } get { return delayY; } }
    public float Delay { set { delay = value; } get { return delay; } }
    public float DelayTime { set { delayTime = value; } get { return delayTime; } }
    public float Duration { set { duration = value; } get { return duration; } }
    public float Width { set { _width = value; } get { return _width; } }
    public float Height { set { _height = value; } get { return _height; } }
    public Vector2 OriVector2 { set { oriVector2 = value; } get { return oriVector2; } }
    public Vector2 GenVector2 { set { genVector2 = value; } get { return genVector2; } }
    public Vector2 NextVector2 { set { nextVector2 = value; } get { return nextVector2; } }
    public bool IsChoosing { set { _isChoosing = value; } get { return _isChoosing; } }
    public bool IsChanging { set { isChanging = value; } get { return isChanging; } }
    public bool IsRecovering { set { isRecovering = value; } get { return isRecovering; } }
    public bool StarsCutEffectIsPlaying { set { _StarsCutEffectIsPlaying = value; } get { return _StarsCutEffectIsPlaying; } }
    public CardAgent GetCardAgent { get { return _cardAgent; } }
    public int SceneIndex { set { _sceneIndex = value; }get { return _sceneIndex; }}

    public MagicWallManager manager{set{ _manager = value; } }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        agentRectTransform = GetComponent<RectTransform>();
//        nameTextComponent.text = name;
    }


    /// <summary>
    ///     初始化基础数据
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="dataId"></param>
    /// <param name="type"></param>
    /// <param name="isCard"></param>
    protected void InitBase(MagicWallManager manager,int dataId, MWTypeEnum type, bool isCard) {
        _manager = manager;
        _agentManager = _manager.agentManager;
        _data_id = dataId;
        _type = type;
        _isCard = isCard;
    }


    /// <summary>
    ///         初始化 Agent 信息,在生成浮动块时调用
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="originVector">在屏幕上显示的位置</param>
    /// <param name="genVector">出生的位置</param>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="dataId"></param>
    /// <param name="dataImg"></param>
    /// <param name="dataIsCustom"></param>
    /// <param name="dataType"></param>
	public virtual void Initialize(MagicWallManager manager,Vector2 originVector,Vector2 genVector,int row,
        int column,float width,float height,int dataId,string dataImg,bool dataIsCustom, 
        MWTypeEnum dataType,AgentContainerType agentContainerType)
    {
        InitBase(manager,dataId, dataType,false);

        _manager = manager;

        OriVector2 = originVector;

        //  出生位置
        GenVector2 = genVector;
        GetComponent<RectTransform>().anchoredPosition = genVector;


        x = row;
        y = column;
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
        _sceneIndex = _manager.SceneIndex;

        // 定义工厂
        if (dataType == MWTypeEnum.Enterprise)
        {
            //_itemsFactory = new EnvFactory();
            _itemsFactory = _manager.itemsFactoryAgent.envFactory;

        }
        else if (dataType == MWTypeEnum.Product)
        {

            //_itemsFactory = new ProductFactory();
            _itemsFactory = _manager.itemsFactoryAgent.productFactory;

        }
        else {
            //_itemsFactory = new ActivityFactory();
            _itemsFactory = _manager.itemsFactoryAgent.activityFactory;

        }

        _agentContainerType = agentContainerType;
        //_itemsFactory.Init(_manager);
    }

    #region 更新位置
    public void updatePosition()
    {
        // 如果是被选中，并打开的
        if (IsChoosing) {
            return;
        }

        if (isRecovering) {
            return;
        }

        if (!CanEffected)
        {
            return;
        }
        else {
            // 当需要判断位置时

            if (NeedAdjustPostion()) {
                UpdatePositionEffect();
            }
        }
    }



    private void UpdatePositionEffect(){

		Vector2 refVector2; // 参照的目标位置
		if(_manager.Status == WallStatusEnum.Cutting){
			// 当前场景正在切换时，参考位置为目标的下个移动位置
			refVector2 = NextVector2;
		} else{
			//当前场景为正常展示时，参考位置为固定位置
			refVector2 = oriVector2;
		}
        Vector2 refVector2WithOffset = refVector2 - new Vector2(_manager.PanelOffsetX, _manager.PanelOffsetY); //获取带偏移量的参考位置

        // 此时的坐标位置可能已处于偏移状态
		RectTransform m_transform = GetComponent<RectTransform>();

        if (m_transform == null) {
            return;
        }



        // 获取施加影响的目标物
        //  判断是否有多个影响体，如有多个，取距离最近的那个
        List<FlockAgent> transforms = _agentManager.EffectAgent;
        FlockAgent targetAgent = null;
        Vector2 targetVector2; // 目标物位置
        float distance = 1000f;

		foreach (FlockAgent item in transforms)
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
        float w,h;
        if (targetAgent != null)
        {
            Vector3 scaleVector3 = targetAgent.GetComponent<RectTransform>().localScale;
            w = targetAgent.Width * scaleVector3.x;
            h = targetAgent.Height * scaleVector3.y;

        }
        else {
            w = 0;
            h = 0;
        }
        // 判断结束

        // 获取有效影响范围，是宽度一半以上
        float effectDistance = (w / 2) + (w / 2) * _manager.InfluenceMoveFactor;
        // 获取差值，差值越大，则表明两个物体距离越近，MAX（offsest） = effectDistance
        float offset = effectDistance - distance;

        // 进入影响范围
        if (offset >= 0)
		{
            targetVector2 = targetAgent.GetComponent<RectTransform>().anchoredPosition;

            // 获取offset_x;offset_y
            float offset_x = Mathf.Abs(refVector2WithOffset.x - targetVector2.x);
            float offset_y = Mathf.Abs(refVector2WithOffset.y - targetVector2.y);

            float m_scale = -(1f / effectDistance) * offset + 1f;

            //
            //  上下移动的偏差值
            //
            float move_offset_y = offset_y * ((h / 2) / effectDistance);
            move_offset_y += h / 10 * _manager.InfluenceMoveFactor;

            float move_offset_x = offset_x * ((w / 2) / effectDistance);
            move_offset_x += w / 10 * _manager.InfluenceMoveFactor;

            //show_move_offset_x = move_offset_x;
            //show_move_offset_y = move_offset_y;


            float to_y,to_x;
            if (refVector2WithOffset.y > targetVector2.y)
            {
                to_y = refVector2.y + move_offset_y;
            }
            else if (refVector2WithOffset.y < targetVector2.y)
            {
                to_y = refVector2.y - move_offset_y;
            }
            else {
                to_y = refVector2.y;
            }

            if (refVector2WithOffset.x > targetVector2.x)
            {
                //m_transform.gameObject.GetComponentInChildren<Image>().color = Color.red;
                to_x = refVector2.x + move_offset_x;
            }
            else if (refVector2WithOffset.x < targetVector2.x)
            {
                //m_transform.gameObject.GetComponentInChildren<Image>().color = Color.grey;
                to_x = refVector2.x - move_offset_x;
            }
            else {
                to_x = refVector2.x;
            }

            Vector2 to = new Vector2(to_x, to_y); //目标位置

            // offset：影响距离  /  effectDistance： 最大影响距离

            //float overshootOrAmplitude = 3f;
            //float k = (offset = offset / effectDistance - 1f) * offset * ((overshootOrAmplitude + 1f) * offset + overshootOrAmplitude) + 1f;

            // 获取缓动方法
            Func<float, float> defaultEasingFunction = EasingFunction.Get(_manager.InfluenceEaseEnum);
            float k = defaultEasingFunction(offset / effectDistance);

            m_transform?.DOAnchorPos(Vector2.Lerp(refVector2, to, k), Time.deltaTime);
            m_transform?.DOScale(Mathf.Lerp(1f, 0.1f, k), Time.deltaTime);
            
			IsChanging = true;
		}
		else
			// 未进入影响范围
		{
            Vector2 toy = new Vector2(refVector2.x, refVector2.y);
			m_transform?.DOAnchorPos(toy, Time.deltaTime);

            if (m_transform.localScale != Vector3.one) {
                m_transform?.DOScale(1, Time.deltaTime);
            }
        }


	}
    #endregion

    #region 点击选择

    public void DoChoose() {

        if (!_isChoosing)
        {
            _isChoosing = true;

            //  先缩小（向后退）
            RectTransform rect = GetComponent<RectTransform>();
            Vector2 positionInMainPanel = rect.anchoredPosition;

            //  移到后方、缩小、透明
            rect.DOScale(0.1f, 0.3f);
            Vector3 to = new Vector3(rect.anchoredPosition.x, rect.anchoredPosition.y, 200);
            Vector3 cardGenPosition = new Vector3(rect.anchoredPosition.x - _manager.PanelOffsetX - 1f, rect.anchoredPosition.y - _manager.PanelOffsetY - 1f, 200);

            // 同时创建十字卡片，加载数据，以防因加载数据引起的卡顿
            _cardAgent = _itemsFactory.GenerateCardAgent(cardGenPosition, this, _data_id, false);

            //靠近四周边界需要偏移
            float w = _cardAgent.GetComponent<RectTransform>().sizeDelta.x;
            float h = _cardAgent.GetComponent<RectTransform>().sizeDelta.y; 

            if (cardGenPosition.x < w / 2)
            {
                cardGenPosition.x = w / 2;
            }
            if (cardGenPosition.x > _manager.mainPanel.rect.width - w/2)
            {
                cardGenPosition.x = _manager.mainPanel.rect.width - w / 2;
            }
            if (cardGenPosition.y < h / 2)
            {
                cardGenPosition.y = h / 2;
            }
            if (cardGenPosition.y > _manager.mainPanel.rect.height - h / 2)
            {
                cardGenPosition.y = _manager.mainPanel.rect.height - h / 2;
            }

            // 完成缩小与移动后创建十字卡片
            rect.DOAnchorPos3D(to, 0.3f).OnComplete(() => {
                rect.gameObject.SetActive(false);
                _cardAgent.GoToFront();
            });
        }
    }

    #endregion

    #region 恢复

    public void DoRecoverAfterChoose()
    {
        IsRecovering = true;

        // 如果组件已不在原场景，则不进行恢复
        if (_sceneIndex != _manager.SceneIndex) {
            gameObject.SetActive(false);
            DestoryAgency();
            return;
        }

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
        rect.DOAnchorPos3D(to, 0.3f);

        // 放大至原大小
        Vector3 scaleVector3 = Vector3.one;

        // 在放大动画开始前，标记该组件为不被选择的
        IsChoosing = false;

        GetComponent<RectTransform>().DOScale(scaleVector3, 1f)
           .OnUpdate(() =>
           {
               Width = GetComponent<RectTransform>().sizeDelta.x;
               Height = GetComponent<RectTransform>().sizeDelta.y;
           }).OnComplete(() => {
               IsRecovering = false;
           });

    }


    #endregion

    protected void DoDestoryOnCompleteCallBack(FlockAgent agent)
    {

        // 进行销毁
        if (typeof(CrossCardAgent).IsAssignableFrom(agent.GetType())) {
            _agentManager.RemoveItemFromEffectItems(agent as CardAgent);
            CardAgent ca = agent as CardAgent;

            ca.DestoryAgency();
            ca.OriginAgent.DestoryAgency();
            //Destroy(ca.gameObject);
            //Destroy(ca.OriginAgent.gameObject);

        }
        else if (typeof(SliceCardAgent).IsAssignableFrom(agent.GetType()))
        {
            _agentManager.RemoveItemFromEffectItems(agent as CardAgent);
            CardAgent ca = agent as CardAgent;

            ca.DestoryAgency();
            ca.OriginAgent.DestoryAgency();
            //Destroy(ca.gameObject);
            //Destroy(ca.OriginAgent.gameObject);

        }
        else if (typeof(FlockAgent).IsAssignableFrom(agent.GetType())) {
            //Destroy(agent.gameObject);
            agent.DestoryAgency();
        }

    }




    //  判断是否需要调整位置
    private bool NeedAdjustPostion() {
        // 当前位置与目标位置一致
        bool NoEffectAgent = _agentManager.EffectAgent.Count == 0;
        //bool InOriginPosition = GetComponent<RectTransform>().anchoredPosition == NextVector2;
        bool InOriginPosition = false;

        // 如果没有影响的agent，并且位置没有改变，则不需要调整位置
        if (NoEffectAgent && InOriginPosition)
            return false;


        return true;
    }


    //  判断目标是否是有效的
    private bool IsEffectiveTarget(FlockAgent flockAgent)
    {
        if (!flockAgent.gameObject.activeSelf)
        {
            return false;
        }

        float effect_width = 300f;
        float effect_height = 300f;

        Vector3 scaleVector3 = flockAgent.GetComponent<RectTransform>().localScale;
        float width = flockAgent.GetComponent<RectTransform>().rect.width;
        float height = flockAgent.GetComponent<RectTransform>().rect.height;

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
    public void DestoryAgency() {
        // 清除 Dotween 代理

        // 删除 Gameobject
        _agentManager.DestoryAgent(this);
    }


    public void UpdateImageAlpha(float alpha) {
        Color color = GetComponent<Image>().color;
        color.a = alpha;
        GetComponent<Image>().color = color;

    }


    /// <summary>
    ///     重置 Agent
    /// </summary>
    public void Reset() {
        if (!IsCard) {

            // 透明度调整
            //GetComponent<RawImage>().color = new Color(255, 255, 255, 255);
            if (GetComponent<Image>().color != new Color(255, 255, 255, 255)) {
                GetComponent<Image>().color = new Color(255, 255, 255, 255);
            }
            
            // 设置 scale
            Vector3 scale = new Vector3(1, 1, 1);
            if (GetComponent<RectTransform>().localScale != scale)
            {
                GetComponent<RectTransform>().localScale = scale;
            }

            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;


            GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);

            GetComponent<Image>().sprite = null;

        }
    }


    /// <summary>
    /// 检查是否需要回收
    /// </summary>
    public bool CheckIsNeedRecycle()
    {
        var result = false;

        // 如果在运行中的 flock，已经远远离开屏幕，则进行销毁
        if (_manager.Status != WallStatusEnum.Cutting)
        {
            Vector3 position = Camera.main.WorldToScreenPoint(GetComponent<RectTransform>().transform.position);
            if (position.x + (Width * 4) < 0)
            {
                //_agentManager.ClearAgent(this);
                result = true;
            }
            else if (position.x - (Width * 4) > Screen.width)
            {
                //_agentManager.ClearAgent(this);
                result = true;

            }
            else if (position.y + (Height * 4) < 0)
            {
                //_agentManager.ClearAgent(this);
                result = true;

            }
            else if (position.y - (Height * 4) > Screen.height)
            {
                //_agentManager.ClearAgent(this);
                result = true;
            }
        }

        return result;
    }



}



