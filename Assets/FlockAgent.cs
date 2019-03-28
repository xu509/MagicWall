using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
    [SerializeField]
    public int index;
	int x;
	int y;

    // 冲突的 items 
    [SerializeField]
    public Dictionary<string, Transform> confictItems;

    public string[] confictItemsNames;


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
    public float edge_radius = 4f; // 碰撞体半径


    MagicWall agentMagicWall;
    public MagicWall AgentMagicWall { get { return agentMagicWall; } }

    Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }

    RectTransform agentRectTransform;
    public RectTransform AgentRectTransform { get { return agentRectTransform; } }

    public StatusEnum agentStatus = StatusEnum.NORMAL;
    public StatusEnum AgentStatus { get { return agentStatus; } }

    public ScaleStatusEnum agentScaleStatus;


    public Text signTextComponent;
    public Text nameTextComponent;


    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<Collider2D>();
        agentRectTransform = GetComponent<RectTransform>();
        confictItems = new Dictionary<string, Transform>();
        confictItemsNames = new string[10];

        nameTextComponent.text = name;

    }

	public void Initialize(MagicWall magicWall,int index,int x,int y,Vector2 tar)
    {
        agentMagicWall = magicWall;
		this.index = index;
		this.x = x;
		this.y = y;
        this.TarVector2 = tar;
    }

    public void Move(Vector2 velocity)
    {
        // turn agent face the direction that it's going to be moving toward.(箭头的指向向上)
        if(velocity != Vector2.zero){
            transform.up = velocity;
            transform.position += (Vector3)velocity * Time.deltaTime;
        }

    }


	void FixedUpdate(){

        // 判断何时恢复
        if (agentStatus == StatusEnum.CHANGING) {
            if (confictItems.Count > 1)
            {
                DoScale();
            }
            else if (confictItems.Count == 0)
            {
                DoRecover();
            }
        }


        




        //if (confictItems.Count >= 1) {

        //    //Debug.Log("Current Colllider items : " + confictItems.Count);
        //    DoScale();
        //} else if (agentStatus == AgentStatusEnum.changing) {
        //    // 当模块没有接触其他模块，并且位置可能不在
        //    if (confictItems.Count == 0)
        //    {
        //        DoRecover();
        //    }
        //    else if (confictItems.Count == 1) {
        //        //Collider2D[] results = new Collider2D[10];
        //        //ContactFilter2D contactFilter = new ContactFilter2D();

        //        //int i = agentCollider.OverlapCollider(contactFilter, results);
        //        //if (i != 1)
        //        //{
        //        //    confictItems.Clear();
        //        //    for (int j = 0; j < i; j++)
        //        //    {
        //        //        confictItems.Add(results[j].gameObject.name, results[j].gameObject.GetComponent<Transform>());
        //        //    }
        //        //}
        //        DoRecover();

        //    }


        //}




        // show dic
        int index = 0;

        foreach (KeyValuePair<string, Transform> pair in confictItems)
        {
            confictItemsNames[index] = pair.Key;
            index++;
        }

        signTextComponent.text = confictItems.Count.ToString();




    }



	// 1级缩小
	void DoScale(){
        //float width = agentMagicWall.flock_width / 2;

        float toScaleFactor = 0.6f;

        if (scaleFactor != toScaleFactor) {
            scaleFactor = toScaleFactor;

            // 缩小 
            RectTransform rt = GetComponent<RectTransform>();
            rt.DOScale(scaleFactor, 2f);

            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            collider.edgeRadius = edge_radius * scaleFactor;

        }



        //// 如果已缩小至一半，则不再缩小
        //if (scaleFactor > 0.6f)
        //{
        //    scaleFactor -= 0.01f;

        //    // 缩小 
        //    RectTransform rt = GetComponent<RectTransform>();
        //    rt.DOScale(scaleFactor, Time.deltaTime);

        //    BoxCollider2D collider = GetComponent<BoxCollider2D>();
        //    collider.edgeRadius = edge_radius * scaleFactor;

        //    //GetComponentInChildren<Image>().color = Color.blue;

        //}

        //Vector2 v = new Vector2 (width, -width);
        //rt.DOAnchorPos (rt.anchoredPosition + v,Time.deltaTime);

    }

    // 2级缩小
    void DoScale2()
    {
        //float width = agentMagicWall.flock_width / 2;

        float toScaleFactor = 0.4f;

        if (scaleFactor != toScaleFactor)
        {
            scaleFactor = toScaleFactor;

            // 缩小 
            RectTransform rt = GetComponent<RectTransform>();
            rt.DOScale(scaleFactor, 2f);

            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            collider.edgeRadius = edge_radius * scaleFactor;

        }

    }

    // 恢复
    void DoRecover()
    {
        //float width = agentMagicWall.flock_width / 2;
        RectTransform rt = GetComponent<RectTransform>();



        // 恢复位置

        if (rt.anchoredPosition != tarVector2)
        {
            //GetComponentInChildren<Image>().color = Color.grey;

            // 如果此时被

            //Debug.Log("show : " + agentCollider);

            Vector2 to = tarVector2 - rt.anchoredPosition;
            //Debug.Log(to);
            if (to.sqrMagnitude > Vector2.one.sqrMagnitude)
            {
                to = to.normalized;
            }
            rt.DOAnchorPos(rt.anchoredPosition + to, Time.deltaTime);

        }
        else {

            // 恢复大小
            if (scaleFactor < 1f)
            {
                //GetComponentInChildren<Image>().color = Color.red;

                scaleFactor += 0.01f;
                rt.DOScale(scaleFactor, Time.deltaTime);

                BoxCollider2D collider = GetComponent<BoxCollider2D>();
                collider.edgeRadius = edge_radius * scaleFactor;
            }

        }


        if (scaleFactor == 1f && rt.anchoredPosition == tarVector2) {
            agentStatus = StatusEnum.NORMAL;
            //GetComponentInChildren<Image>().color = Color.white;
        }
        //Vector2 v = new Vector2 (width, -width);
        //rt.DOAnchorPos (rt.anchoredPosition + v,Time.deltaTime);
        //BoxCollider2D collider = GetComponent<BoxCollider2D> ();
        //collider.edgeRadius = collider.edgeRadius / 2;
    }




    void OnCollisionEnter2D(Collision2D collision)
    {
        //当碰撞体被触发
        string collision_name = collision.gameObject.name;
        Transform collision_transform = collision.gameObject.GetComponent<Transform>();

        //Debug.Log("collision_name:" + collision_name);

        try
        {
            confictItems.Add(collision_name, collision_transform);
            agentStatus = StatusEnum.CHANGING;
        }
        catch
        {
        }

        //gameObject.GetComponentInChildren<Image>().color = Color.blue;


        //confictItems.Add(collision.gameObject.name, collision.gameObject.GetComponent<Transform>());

    }


    void OnCollisionStay2D(Collision2D collision)
    {
        //gameObject.GetComponentInChildren<Image>().color = Color.red;

    }

    void OnCollisionExit2D(Collision2D collision) {
        confictItems.Remove(collision.gameObject.name);
    }

}

// agent 状态
public enum StatusEnum {
    NORMAL,
    CHANGING
}

// agent 缩放状态
public enum ScaleStatusEnum {
    ONCE,
    TWICE,
    NORMAL
}
