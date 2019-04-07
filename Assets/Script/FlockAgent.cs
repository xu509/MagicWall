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

    // 原位
    public Vector2 oriVector2;
	public Vector2 OriVector2
    {
		set {
            oriVector2 = value;
		}
		get {
			return oriVector2;
		}
	}

	// 下个移动的位置
	private Vector2 nextVector2;
	public Vector2 NextVector2
	{
		set {
			nextVector2 = value;
		}
		get {
			return nextVector2;
		}
	}






	// 是否被选中
    public bool isChoosing = false;
    public bool IsChoosing
    {
        set
        {
            isChoosing = value;
        }
        get
        {
            return isChoosing;
        }
    }

	// 是否被改变
    public bool isChanging = false;
    public bool IsChanging
    {
        set
        {
            isChanging = value;
        }
        get
        {
            return isChanging;
        }
    }
		
    MagicWallManager agentMagicWall;
    public MagicWallManager AgentMagicWall { get { return agentMagicWall; } }

    RectTransform agentRectTransform;
    public RectTransform AgentRectTransform { get { return agentRectTransform; } }

    public Text signTextComponent;
    public Text nameTextComponent;
    public Text signTextComponent2;


    // Start is called before the first frame update
    void Start()
    {
        agentRectTransform = GetComponent<RectTransform>();
//        nameTextComponent.text = name;
    }

	public void Initialize(MagicWallManager magicWall,Vector2 originVector)
    {
        agentMagicWall = magicWall;
        this.oriVector2 = originVector;
    }

    public void Move(Vector2 velocity)
    {
//		GetComponent<RectTransform> ().DOAnchorPos (velocity, Time.deltaTime);

    }

	public void MoveToPosition(Vector2 postion){
//		agentRectTransform.DOAnchorPos (updatePosition(postion),Time.deltaTime);
	}


	void FixedUpdate(){

		// 判断是否在影响物的边上
//		if(AgentMagicWall.EffectAgent.Count > 0){
//			updatePosition ();
//		}

		if (IsChoosing) {
			GetComponent<Image> ().color = Color.black;
		}


    }

	//
	// 更新位置
	//
	public void updatePosition(){
		Vector2 refVector2; // 参照的目标位置
		if(AgentMagicWall.Status == WallStatusEnum.Cutting){
			// 当前场景正在切换时，参考位置为目标的下个移动位置
			refVector2 = NextVector2;
		} else{
			//当前场景为正常展示时，参考位置为固定位置
			refVector2 = oriVector2;
		}

		// 如果是被选中的，则不要移动
		if (IsChoosing){
			return;
		}

			
		RectTransform m_transform = GetComponent<RectTransform>();
		List<RectTransform> transforms = AgentMagicWall.EffectAgent;
		RectTransform targetTransform = null;


		//判断是否有多个影响体，如有多个，取距离最近的那个
		float distance = 1000f;
		foreach (RectTransform item in transforms)
		{
			float newDistance = Vector2.Distance(refVector2, item.anchoredPosition);
		    if (newDistance < distance)
		    {
		        distance = newDistance;
		        targetTransform = item;
		    }
		}

		//获取距离影响点与原点的距离
//		float distance = Vector2.Distance(oriVector2, effectTransform.anchoredPosition);

		// 判断结束

		//获取影响距离与实际距离的差值
		float offset = AgentMagicWall.TheDistance - distance;
		//            signTextComponent.text = distance.ToString();
		//            signTextComponent2.text = "offset : " + offset.ToString();


		// 进入影响范围
		if (offset > 0)
		{
			m_transform.gameObject.GetComponent<Image> ().color = Color.blue;
			float m_scale = -(1f / 255f) * offset + 1f;

			if (refVector2.y > targetTransform.anchoredPosition.y)
			{
				float to = refVector2.y + AgentMagicWall.MoveFactor * offset;
				Vector2 toy = new Vector2(refVector2.x, to);
				m_transform.DOAnchorPos(toy, Time.deltaTime);
			}
			else if (refVector2.y < targetTransform.anchoredPosition.y)
			{
				float to = refVector2.y - (AgentMagicWall.MoveFactor * offset);
				Vector2 toy = new Vector2(refVector2.x, to);
				m_transform.DOAnchorPos(toy, Time.deltaTime);
			}

			m_transform.DOScale(m_scale, Time.deltaTime);

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
				m_transform.gameObject.GetComponent<Image> ().color = Color.green;

				IsChanging = false;
//				return toy;
//			}
		}

	}







    void OnCollisionEnter2D(Collision2D collision)
    {

    }


    void OnCollisionStay2D(Collision2D collision)
    {
        //gameObject.GetComponentInChildren<Image>().color = Color.red;

    }

    void OnCollisionExit2D(Collision2D collision) {
//        confictItems.Remove(collision.gameObject.name);
    }




}


public enum AgentStatus{
	NORMAL,MOVING,CHOOSING
}
