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

    float delayX;
    public float DelayX { set { delayX = value; } get { return delayX; } }

    float delayY;
    public float DelayY { set { delayY = value; } get { return delayY; } }

    float delay;
    public float Delay { set { delay = value; } get { return delay; } }

    float delayTime;
    public float DelayTime { set { delayTime = value; } get { return delayTime; } }

    // 宽度
    [SerializeField]
    float width;
    public float Width { set { width = value;} get { return width;} }

    // 高度
    [SerializeField]
    float height;
    public float Height { set { height = value; } get { return height;} }

    // 原位
    [SerializeField]
    Vector2 oriVector2;
	public Vector2 OriVector2 { set { oriVector2 = value; } get { return oriVector2; } }

    // 生成的位置
    Vector2 genVector2;
    public Vector2 GenVector2{ set { genVector2 = value; } get { return genVector2; } }

    // 下个移动的位置
    [SerializeField]
    Vector2 nextVector2;
	public Vector2 NextVector2 { set { nextVector2 = value; } get { return nextVector2; } }

	// 是否被选中
    public bool isChoosing = false;
    public bool IsChoosing { set { isChoosing = value; } get { return isChoosing; } }

	// 是否被改变
    public bool isChanging = false;
    public bool IsChanging { set { isChanging = value; } get { return isChanging; } }
		
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
	public void Initialize(Vector2 originVector,Vector2 genVector,int row,int column)
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

		if (IsChoosing) {
			GetComponentInChildren<Image> ().color = Color.black;
		}

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
        Vector2 refVector2WithOffset = refVector2 - new Vector2(manager.PanelOffset, 0); //获取带偏移量的参考位置
        showRefVector2 = refVector2;
        showRefVector2WithOffset = refVector2WithOffset;

        Vector2 targetVector2; // 目标物位置


        // 如果是被选中的，则不要移动
        if (IsChoosing){
			return;
		}

        // 此时的坐标位置可能已处于偏移状态
		RectTransform m_transform = GetComponent<RectTransform>();

        //判断是否有多个影响体，如有多个，取距离最近的那个
        List<FlockAgent> transforms = AgentManager.Instance.EffectAgent;
        FlockAgent targetAgent = null;
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
        float w;
        if (targetAgent != null)
        {
            showTargetVector2 = targetAgent.GetComponent<RectTransform>().anchoredPosition;
            w = targetAgent.Width;
        }
        else {
            w = 0;
        }
        // 判断结束

        //
        //获取影响距离与实际距离的差值
        //
        //
        //  
        //
        float effectDistance = (w / 2) + w * MagicWallManager.Instance.InfluenceFactor;
        float offset = effectDistance - distance;
        signTextComponent.text = "OFFSET : " + offset.ToString();
        //signTextComponent2.text = "ed : " + effectDistance.ToString();


        // 进入影响范围
        if (offset >= 0)
		{

            targetVector2 = targetAgent.GetComponent<RectTransform>().anchoredPosition;


            m_transform.gameObject.GetComponentInChildren<Image> ().color = Color.blue;
            float m_scale = -(1f / effectDistance) * offset + 1f;

            float randomFactor = Random.Range(0, 1);

            float move_offset = offset * (((w / 2) + randomFactor * (w/10)) / effectDistance);
            //float move_offset = (offset /= 200) * offset * offset ;

            signTextComponent2.text = "move_offset : " + move_offset.ToString();
            showMoveOffset = move_offset;

            //float y1 = refVector2WithOffset.y;
            //float x1 = refVector2WithOffset.x;
            //float y2 = targetAgent.GetComponent<RectTransform>().anchoredPosition.y;
            //float x2 = targetAgent.GetComponent<RectTransform>().anchoredPosition.x;

            //float k = (y1 - y2) / (x1 - x2);
            //float b = ((x1 * y2) - (x2 * y1)) / (x1 - x2);

            //Vector2 move = MagicWallManager.Instance.InfluenceMoveFactor * offset

            //Vector2 normlizeMove = (refVector2WithOffset -
            //    targetAgent.GetComponent<RectTransform>().anchoredPosition).normalized;
            //normlizeMove = normlizeMove * (MagicWallManager.Instance.InfluenceMoveFactor);
            //normlizeMove += refVector2WithOffset;

            //m_transform.DOAnchorPos(normlizeMove, Time.deltaTime);


            if (refVector2.y >= targetVector2.y)
            {
                float to = refVector2.y + move_offset;
                Vector2 toy = new Vector2(refVector2.x, to);
                m_transform.DOAnchorPos(toy, Time.deltaTime);
            }
            else if (refVector2.y < targetVector2.y)
            {
                float to = refVector2.y - move_offset;
                Vector2 toy = new Vector2(refVector2.x, to);
                m_transform.DOAnchorPos(toy, Time.deltaTime);
            }

            // 尝试向外扩散


            m_transform.DOScale(m_scale, 1f);
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
				m_transform.gameObject.GetComponentInChildren<Image> ().color = Color.green;

				IsChanging = false;
//				return toy;
//			}
		}

	}


}


