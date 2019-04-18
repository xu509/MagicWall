using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
    int x;
    int y;

    private float delayX;
    public float DelayX { set { delayX = value; } get { return delayX; } }

    private float delayY;
    public float DelayY { set { delayY = value; } get { return delayY; } }

    private float delay;
    public float Delay { set { delay = value; } get { return delay; } }

    private float delayTime;
    public float DelayTime { set { delayTime = value; } get { return delayTime; } }

    // 宽度
    [SerializeField]
    private float width;
    public float Width { set { width = value; } get { return width; } }

    // 高度
    [SerializeField]
    private float height;
    public float Height { set { height = value; } get { return height; } }

    // 原位
    [SerializeField]
    private Vector2 oriVector2;
    public Vector2 OriVector2 { set { oriVector2 = value; } get { return oriVector2; } }

    // 生成的位置
    private Vector2 genVector2;
    public Vector2 GenVector2 { set { genVector2 = value; } get { return genVector2; } }

    // 下个移动的位置
    [SerializeField]
    private Vector2 nextVector2;
    public Vector2 NextVector2 { set { nextVector2 = value; } get { return nextVector2; } }

    // 是否被选中
    private bool isChoosing = false;
    public bool IsChoosing { set { isChoosing = value; } get { return isChoosing; } }

    // 是否被改变
    private bool isChanging = false;
    public bool IsChanging { set { isChanging = value; } get { return isChanging; } }

    // 卡片代理
    CardAgent cardAgent;
    public CardAgent GetCardAgent{ get {return cardAgent; }}
		
    RectTransform agentRectTransform;
    public RectTransform AgentRectTransform { get { return agentRectTransform; } }

    public Text signTextComponent;
    public Text nameTextComponent;
    public Text signTextComponent2;

    [SerializeField]
    Vector2 showTargetVector2;
    [SerializeField]
    Vector2 showRefVector2;
    [SerializeField]
    Vector2 showRefVector2WithOffset;
    [SerializeField]
    float showMoveOffset;

    // Start is called before the first frame update
    void Start()
    {
        agentRectTransform = GetComponent<RectTransform>();
//        nameTextComponent.text = name;
    }

    //
    //  初始化 Agent 信息
    //      originVector : 在屏幕上显示的位置
    //      genVector ： 出生的位置
    //
	public virtual void Initialize(Vector2 originVector,Vector2 genVector,int row,int column)
    {
        OriVector2 = originVector;
        GenVector2 = genVector;
        x = row;
        y = column;

        // 定义 agent 的名字
        nameTextComponent.text = row + " - " + column;
    }

    public void Move(Vector2 velocity)
    {
//		GetComponent<RectTransform> ().DOAnchorPos (velocity, Time.deltaTime);

    }

	public void MoveToPosition(Vector2 postion){
//		agentRectTransform.DOAnchorPos (updatePosition(postion),Time.deltaTime);
	}


	void FixedUpdate(){

		//if (IsChoosing) {
		//	GetComponentInChildren<Image> ().color = Color.black;
		//}

    }

	//
	// 更新位置
	//
	public void updatePosition(){
        MagicWallManager manager = MagicWallManager.Instance;

		Vector2 refVector2; // 参照的目标位置
		if(manager.Status == WallStatusEnum.Cutting){
			// 当前场景正在切换时，参考位置为目标的下个移动位置
			refVector2 = NextVector2;
		} else{
			//当前场景为正常展示时，参考位置为固定位置
			refVector2 = oriVector2;
		}
        Vector2 refVector2WithOffset = refVector2 - new Vector2(manager.PanelOffsetX, manager.PanelOffsetY); //获取带偏移量的参考位置
        showRefVector2 = refVector2;
        showRefVector2WithOffset = refVector2WithOffset;

        // 如果是被选中的，则不要移动
        if (IsChoosing){
			return;
		}

        // 此时的坐标位置可能已处于偏移状态
		RectTransform m_transform = GetComponent<RectTransform>();

        // 获取施加影响的目标物
        //  判断是否有多个影响体，如有多个，取距离最近的那个
        List<FlockAgent> transforms = AgentManager.Instance.EffectAgent;
        FlockAgent targetAgent = null;
        Vector2 targetVector2; // 目标物位置
        float distance = 1000f;

		foreach (FlockAgent item in transforms)
		{
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
            showTargetVector2 = targetAgent.GetComponent<RectTransform>().anchoredPosition;
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
        float effectDistance = (w / 2) + (w / 2) * MagicWallManager.Instance.InfluenceFactor;
        // 获取差值，差值越大，则表明两个物体距离越近，MAX（offsest） = effectDistance
        float offset = effectDistance - distance;
        signTextComponent.text = "OFFSET : " + offset.ToString();
        //signTextComponent2.text = "ed : " + effectDistance.ToString();

        // 进入影响范围
        if (offset >= 0)
		{
            targetVector2 = targetAgent.GetComponent<RectTransform>().anchoredPosition;
            //m_transform.gameObject.GetComponentInChildren<Image>().color = Color.blue;
            float m_scale = -(1f / effectDistance) * offset + 1f;

            //
            //  上下移动
            //
            float move_offset = offset * ((h / 2) / effectDistance);
            showMoveOffset = move_offset;
            move_offset += h/10 * manager.InfluenceMoveFactor;

            float move_offset_x = offset * ((w / 2) / effectDistance);
            move_offset_x += w / 10 * manager.InfluenceMoveFactor;

            signTextComponent2.text = "mo: " + move_offset.ToString() + " / " + move_offset_x.ToString();

            float to_y,to_x;
            if (refVector2.y > targetVector2.y)
            {
                to_y = refVector2.y + move_offset;
            }
            else if (refVector2.y < targetVector2.y)
            {
                to_y = refVector2.y - move_offset;
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

            //float k = offset / effectDistance;
            float overshootOrAmplitude = 3f;
            float k = (offset = offset / effectDistance - 1f) * offset * ((overshootOrAmplitude + 1f) * offset + overshootOrAmplitude) + 1f;

            m_transform.DOAnchorPos(Vector2.Lerp(refVector2, to, k), 0.5f);
            m_transform.DOScale(Mathf.Lerp(1f, 0.3f, k), Time.deltaTime);
            
            //
            // 尝试向外扩散
            //
            //Vector2 toV = refVector2WithOffset + (refVector2WithOffset - targetVector2).normalized * offset * manager.InfluenceMoveFactor;
            //float k = offset / effectDistance;
            //Vector2 to = Vector2.Lerp(refVector2WithOffset, toV, k);
            //m_transform.DOAnchorPos(to, Time.deltaTime);

			IsChanging = true;
		}
		else
			// 未进入影响范围
		{
//			if (IsChanging)
//			{
				Vector2 toy = new Vector2(refVector2.x, refVector2.y);
				m_transform.DOAnchorPos(toy, Time.deltaTime);
				m_transform.DOScale(1, Time.deltaTime);
				//m_transform.gameObject.GetComponentInChildren<Image> ().color = Color.green;

				IsChanging = false;
//				return toy;
//			}
		}

	}


}


