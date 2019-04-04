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

    // 影响点
    public RectTransform effectTransform;
    public RectTransform EffectTransform
    {
        set
        {
            effectTransform = value;
        }
        get
        {
            return effectTransform;
        }
    }

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

        nameTextComponent.text = name;

    }

	public void Initialize(MagicWallManager magicWall,Vector2 originVector)
    {
        agentMagicWall = magicWall;
        this.oriVector2 = originVector;
    }

    public void Move(Vector2 velocity)
    {
        // turn agent face the direction that it's going to be moving toward.(箭头的指向向上)
        if(velocity != Vector2.zero){
//            transform.up = velocity;

//			agentRigidbody2D.AddForce (velocity * 50,ForceMode2D.Force);

            transform.position += (Vector3)velocity * Time.deltaTime;
        }

    }

	public void MoveToPosition(Vector2 postion){
		agentRectTransform.DOAnchorPos (postion,0.5f);
	}


	void FixedUpdate(){
        if (IsChanging)
        {
            RectTransform m_transform = GetComponent<RectTransform>();

            List<RectTransform> transforms = AgentMagicWall.EffectAgent;
            RectTransform targetTransform = null;

            //获取距离影响点与原点的距离
            float distance = Vector2.Distance(oriVector2, effectTransform.anchoredPosition);

            ////判断是否有多个影响体，如有多个，取距离最近的那个
            //foreach (RectTransform item in transforms)
            //{
            //    float newDistance = Vector2.Distance(oriVector2, item.anchoredPosition);
            //    if (newDistance < distance)
            //    {
            //        distance = newDistance;
            //        targetTransform = item;
            //    }
            //}


            // 判断结束

            //获取影响距离与实际距离的差值
            float offset = AgentMagicWall.TheDistance - distance;

            signTextComponent.text = distance.ToString();
            signTextComponent2.text = "offset : " + offset.ToString();

            // 进入影响范围
            if (offset > 0)
            {
                float m_scale = -(1f / 255f) * offset + 1f;

                if (oriVector2.y > effectTransform.anchoredPosition.y)
                {
                    float to = oriVector2.y + AgentMagicWall.MoveFactor * offset;
                    Vector2 toy = new Vector2(oriVector2.x, to);
                    m_transform.DOAnchorPos(toy, Time.deltaTime);
                }
                else if (oriVector2.y < effectTransform.anchoredPosition.y)
                {
                    float to = oriVector2.y - (AgentMagicWall.MoveFactor * offset);
                    Vector2 toy = new Vector2(oriVector2.x, to);
                    m_transform.DOAnchorPos(toy, Time.deltaTime);
                }

                m_transform.DOScale(m_scale, Time.deltaTime);

                IsChanging = false;

            }
            else
            // 未进入影响范围
            {
                if (IsChanging)
                {
                    Vector2 toy = new Vector2(oriVector2.x, oriVector2.y);
                    m_transform.DOAnchorPos(toy, Time.deltaTime);
                    m_transform.DOScale(1, Time.deltaTime);
                    IsChanging = false;
                }
            }
        }















        //if (!isChoosing) {
        //    List<RectTransform> transforms = AgentMagicWall.EffectAgent;
        //    RectTransform targetTransform = null;
        //    float distance = 10000f;
        //    foreach (RectTransform item in transforms)
        //    {
        //        float newDistance = Vector2.Distance(oriVector2, item.anchoredPosition);
        //        if (newDistance < distance)
        //        {
        //            distance = newDistance;
        //            targetTransform = item;
        //        }
        //    }
        //    float offset = AgentMagicWall.TheDistance - distance;

        //    signTextComponent.text = distance.ToString();
        //    signTextComponent2.text = "offset : " + offset.ToString();

        //    if (distance < AgentMagicWall.TheDistance)
        //    {
        //        // 进入影响范围
        //        if (offset > 0)
        //        {
        //            //Debug.Log (1f / 255f);
        //            float m_scale = -(1f / 255f) * offset + 1f;

        //            if (oriVector2.y > targetTransform.anchoredPosition.y)
        //            {
        //                float to = oriVector2.y + AgentMagicWall.MoveFactor * offset;
        //                Vector2 toy = new Vector2(oriVector2.x, to);
        //                m_transform.DOAnchorPos(toy, Time.deltaTime);
        //            }
        //            else if (oriVector2.y < targetTransform.anchoredPosition.y)
        //            {
        //                float to = oriVector2.y - (AgentMagicWall.MoveFactor * offset);
        //                Vector2 toy = new Vector2(oriVector2.x, to);
        //                m_transform.DOAnchorPos(toy, Time.deltaTime);
        //            }

        //            m_transform.DOScale(m_scale, Time.deltaTime);

        //            IsChanging = true;

        //        }
        //    }
        //    else {

        //        if (IsChanging) {
        //            Vector2 toy = new Vector2(oriVector2.x, oriVector2.y);
        //            m_transform.DOAnchorPos(toy, Time.deltaTime);
        //            m_transform.DOScale(1, Time.deltaTime);
        //            IsChanging = false;
        //        }

        //    }
        //}












        //string str = "";
        //if (agentStatus == AgentStatus.MOVING) {
        //    str = "MOVING";
        //    GetComponentInChildren<Image>().color = Color.blue;

        //    if (leaveTime == 0)
        //        leaveTime = Time.time;

        //}
        //if (agentStatus == AgentStatus.NORMAL) { 
        //    str = "NORMAL";
        //    GetComponentInChildren<Image>().color = Color.white;
        //    leaveTime = 0;
        //}
        //if (agentStatus == AgentStatus.CHOOSING) {
        //    // 当 Agent 被选中时

        //    Transform wallLogo = GameObject.Find("MagicWall").GetComponent<Transform>();
        //    AgentRectTransform.transform.SetParent(wallLogo);
        //    agentRectTransform.gameObject.layer = 9;

        //    Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agentRectTransform.position, 12);
        //    bool hasImport = false;
        //    foreach (Collider2D c in contextColliders)
        //    {
        //        if (c.gameObject.layer == 9 && c.gameObject.name != name)
        //        {
        //            //Debug.Log(" RigidbodyType2D.Static");
        //            hasImport = true;
        //        }
        //    }
        //    if (!hasImport) {
        //        agentRigidbody2D.bodyType = RigidbodyType2D.Static;
        //    }

        //    if (!IsChoosing) {
        //        // 改变大小
        //        AgentRectTransform.transform.DOScale(2f, 2);
        //        IsChoosing = true;

        //        // 微调位置
        //        // 以Y轴中间线为标准，分别上下移动半格
        //        float currenty = AgentRectTransform.anchoredPosition.y;
        //        if (currenty > 320)
        //        {
        //            AgentRectTransform.DOAnchorPosY(currenty - 50, 2f);
        //        }
        //        else
        //        {
        //            AgentRectTransform.DOAnchorPosY(currenty + 50, 2f);
        //        }
        //    }

        //    // 调整碰撞体
        //    if (AgentCollider2D.edgeRadius < 12f)
        //    {
        //        // 1秒内，edge radius 从3到12；
        //        AgentCollider2D.edgeRadius += agentMagicWall.choosingSpeed * Time.deltaTime;
        //        ScaleFactor++;
        //    }



        //}
        ////if (agentStatus == AgentStatus.CHOOSING)
        ////    str = "CHOOSING";
        //Vector2 v2 = AgentRectTransform.anchoredPosition;

        //signTextComponent.text = v2.ToString(); ;
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
