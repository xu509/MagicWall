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

	Rigidbody2D agentRigidbody2D;

	public AgentStatus agentStatus;
	public AgentStatus AgentStatus{set{agentStatus = value;}get { return agentStatus; } }

    // 目标点
    Vector2 tarVector2;
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




    MagicWall agentMagicWall;
    public MagicWall AgentMagicWall { get { return agentMagicWall; } }

    Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }

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
        agentCollider = GetComponent<Collider2D>();
        agentRectTransform = GetComponent<RectTransform>();

		agentRigidbody2D = GetComponent<Rigidbody2D> ();

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

    }

	public void DoChosenItem(){
		if (agentStatus != AgentStatus.CHOOSING) {
			Debug.Log ("[" + name + "] Do Chosen Item !");

//			agentRigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;
			Transform wallLogo = GameObject.Find("WallLogo").GetComponent<Transform>();
			AgentRectTransform.transform.SetParent(wallLogo);




			agentStatus = AgentStatus.CHOOSING;
		} else {
		
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
