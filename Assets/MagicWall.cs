using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWall : MonoBehaviour
{
    public RectTransform mainPanel;


    public FlockAgent agentPrefab;
    public FlockAgent refPrefab;

    List<FlockAgent> agents = new List<FlockAgent>();

    public FlockBehavior behavior;

    [SerializeField,Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(0f, 100f)]
    public float maxSpeed = 5f;


    //layout
    int row = 6;
    int column = 30;

    // flock width
    public int flock_width = 106;
    public RectTransform Ref_Agent { get { return GameObject.Find("RefAgent").GetComponent<RectTransform>(); } }



    // speed
    float squareMaxSpeed;

    // radius
    [SerializeField, Range(1f, 10f)]
    public float flockRadius = 4.2f;
    // logo 可以辐射的范围
    [SerializeField, Range(1f, 10f)]
    public float logoEffectRadius = 15f;


    Transform wallLogo;



    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;

        for (int i = 0; i < row * column; i++) {
            CreateNewAgent(i);
        }

        wallLogo = GameObject.Find("WallLogo").GetComponent<Transform>();

        CreateRefAgent();


    }

    // Update is called once per frame
    void Update()
    {
        // 画布移动

        //transform.position += (Vector3)velocity * Time.deltaTime;
        mainPanel.transform.position += (Vector3)Vector2.left * Time.deltaTime;

        // 添加并删除碰撞体
        //List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(wallLogo.position, logoEffectRadius);
        foreach (Collider2D c in contextColliders)
        {
            if (c != wallLogo.GetComponent<Collider2D>())
            {
                Rigidbody2D rb2 = c.gameObject.GetComponent<Rigidbody2D>();
                if (rb2 == null)
                {
                    rb2 = c.gameObject.AddComponent<Rigidbody2D>();
                    rb2.gravityScale = 0;
                }
                else {
                    rb2.simulated = true;
                }
            }
        }



        // 检测 agent
        foreach (FlockAgent agent in agents) {
            List<Transform> context = new List<Transform>();
            if (agent.AgentStatus == FlockStatus.MOVE) {
                Vector2 move = behavior.CalculateMove(agent, context, this);
                move *= driveFactor;
                if (move.sqrMagnitude > squareMaxSpeed)
                {
                    move = move.normalized * maxSpeed;
                }
                agent.Move(move);
            }

        }
    }

    //  创建一个新Agent
    void CreateNewAgent(int index) {
        //设置位置
        int real_index = index + 1;

        Vector2 postion = new Vector2();

        int y = real_index / column + 1;
        if (real_index % column == 0) {
            y--;
        }
       
        int x = real_index % column;
        if (x == 0) {
            x = column;
        }


        FlockAgent newAgent = Instantiate(
                                    agentPrefab,
                                    mainPanel
                                    );
        newAgent.name = "Agent(" + x + "," + y + ")";
        newAgent.Initialize(this,index,x,y);

        postion.x = (x-1) * flock_width + (flock_width / 2);
        postion.y = (y-1) * flock_width + (flock_width / 2);
        newAgent.GetComponent<RectTransform>().anchoredPosition = postion;
        agents.Add(newAgent);
    }

    //  创建一个ref
    void CreateRefAgent()
    {
        //设置位置

        FlockAgent newAgent = Instantiate(
                                    refPrefab,
                                    mainPanel
                                    );
        newAgent.name = "RefAgent";
        newAgent.Initialize(this, -1, 0, 0);
        agents.Add(newAgent);
    }



}
