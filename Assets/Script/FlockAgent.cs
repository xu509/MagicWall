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

    // 离开本位的时间
    public float LeaveTime { set { leaveTime = value; } get { return leaveTime; } }
    public float leaveTime = 0f;


    Rigidbody2D agentRigidbody2D;
    public Rigidbody2D AgentRigidbody2D { get{return agentRigidbody2D;}}

    BoxCollider2D agentCollider2D;
    public BoxCollider2D AgentCollider2D {
        set
        {
            agentCollider2D = value;
        }
        get
        {
            return agentCollider2D;
        }
    }

    public AgentStatus agentStatus;
	public AgentStatus AgentStatus{set{agentStatus = value;}get { return agentStatus; } }

    // 目标点
    public Vector2 tarVector2;
	public Vector2 TarVector2 {
		set {
			tarVector2 = value;
		}
		get {
			return tarVector2;
		}
	}

    // 缩放因子
    public float scaleFactor = 1.0f;
	public float ScaleFactor
	{
		set
		{
			scaleFactor = value;
		}
		get
		{
			return scaleFactor;
		}
	}
    public bool isScale = false;
    public bool IsScale
    {
        set
        {
            isScale = value;
        }
        get
        {
            return isScale;
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

    //在迅速回位
    public bool isRunning = false;
    public bool IsRunning
    {
        set
        {
            isRunning = value;
        }
        get
        {
            return isRunning;
        }
    }



    MagicWall agentMagicWall;
    public MagicWall AgentMagicWall { get { return agentMagicWall; } }

    RectTransform agentRectTransform;
    public RectTransform AgentRectTransform { get { return agentRectTransform; } }

    public Text signTextComponent;
    public Text nameTextComponent;

    // Last Collision time;
    private float lastCollisionInstant = 0;
    const float collisionInterval = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        agentRectTransform = GetComponent<RectTransform>();
        agentRigidbody2D = GetComponent<Rigidbody2D> ();
        agentCollider2D = GetComponent<BoxCollider2D>();

        nameTextComponent.text = name;

    }

	public void Initialize(MagicWall magicWall,int x,int y,Vector2 tar)
    {
        agentMagicWall = magicWall;
		this.x = x;
		this.y = y;
        this.TarVector2 = tar;
		this.agentStatus = AgentStatus.NORMAL;
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
        string str = "";
        if (agentStatus == AgentStatus.MOVING) {
            str = "MOVING";
            GetComponentInChildren<Image>().color = Color.blue;

            if (leaveTime == 0)
                leaveTime = Time.time;

        }
        if (agentStatus == AgentStatus.NORMAL) { 
            str = "NORMAL";
            GetComponentInChildren<Image>().color = Color.white;
            leaveTime = 0;
        }
        if (agentStatus == AgentStatus.CHOOSING) {
            // 当 Agent 被选中时

            Transform wallLogo = GameObject.Find("MagicWall").GetComponent<Transform>();
            AgentRectTransform.transform.SetParent(wallLogo);
            agentRectTransform.gameObject.layer = 9;

            Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agentRectTransform.position, 12);
            bool hasImport = false;
            foreach (Collider2D c in contextColliders)
            {
                if (c.gameObject.layer == 9 && c.gameObject.name != name)
                {
                    //Debug.Log(" RigidbodyType2D.Static");
                    hasImport = true;
                }
            }
            if (!hasImport) {
                agentRigidbody2D.bodyType = RigidbodyType2D.Static;
            }





            if (!IsChoosing) {
                // 改变大小
                AgentRectTransform.transform.DOScale(2f, 2);


                // 微调位置
                // 以Y轴中间线为标准，分别上下移动半格
                float currenty = AgentRectTransform.anchoredPosition.y;
                Debug.Log(AgentRectTransform.anchoredPosition);
                if (currenty > 320)
                {
                    AgentRectTransform.DOAnchorPosY(currenty - 50, 2f);
                }
                else
                {
                    AgentRectTransform.DOAnchorPosY(currenty + 50, 2f);
                }

                IsChoosing = true;

            }

            if (AgentCollider2D.edgeRadius < 12f)
            {
                // 1秒内，edge radius 从3到12；
                AgentCollider2D.edgeRadius += agentMagicWall.choosingSpeed * Time.deltaTime;
                ScaleFactor++;
            }

        }
        //if (agentStatus == AgentStatus.CHOOSING)
        //    str = "CHOOSING";
        Vector2 v2 = AgentRectTransform.anchoredPosition;

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
