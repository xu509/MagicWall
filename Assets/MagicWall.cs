using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWall : MonoBehaviour
{
    
	#region public parameter
	public RectTransform mainPanel;
	public FlockAgent agentPrefab;

	public FlockBehavior moveBehavior;
	public FlockBehavior scaleBehavior;
	public FlockBehavior reScaleBehavior;
	public FlockBehavior recoverBehavior;


	[SerializeField,Range(1f, 100f)]
	public float driveFactor = 10f;
	[Range(0f, 100f)]
	public float maxSpeed = 5f;
	[SerializeField,Range(0.1f,10f)]
	public float recoverFactor = 0.6f;

	[SerializeField,Range(1f, 20f)]
	public float neighborRadius ; // flock neighbor radius
	#endregion

	#region privite parameter
    List<FlockAgent> agents = new List<FlockAgent>();
	//layout
	int row = 6;
	int column = 30;

	#endregion

    // flock width
	public int flock_width = 106;


    // speed
    float squareMaxSpeed;

    // radius
    [SerializeField, Range(1f, 10f)]
    public float flockRadius = 4.2f;
    // logo 可以辐射的范围
    [SerializeField, Range(1f, 10f)]
    public float logoEffectRadius = 15f;


    Transform wallLogo;
	public Transform refObj;
	public Transform RefObj { get { return refObj; } }


	//缩放状态
	[SerializeField,Range(1f,10f)]
	public float scaleSpeed = 1;  // 缩放的速度

	[SerializeField,Range(1f,10f)]
	public float recoverMoveSpeed = 1;  // 恢复的移动速度

	public float agent_colider_radius = 4f; // 碰撞体半径


    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;

        for (int i = 0; i < row * column; i++) {
            CreateNewAgent(i);
        }

        wallLogo = GameObject.Find("WallLogo").GetComponent<Transform>();
//
//        CreateRefAgent();


    }

    // Update is called once per frame
    void Update()
    {
        // 画布移动

        //transform.position += (Vector3)velocity * Time.deltaTime;
        mainPanel.transform.position += (Vector3)Vector2.left * Time.deltaTime * driveFactor;

        // 添加并删除碰撞体
		foreach (FlockAgent agent in agents) {
			if (HasItemsInNear (agent)) {
				scaleBehavior.DoScale (agent, this);
//				Debug.Log (agent.gameObject.name);	
			} else {
				if (agent.AgentStatus == AgentStatus.MOVING) {
					reScaleBehavior.DoScale (agent, this);
					// Move
//					Vector2 v3 = moveBehavior.CalculateMove(agent,wallLogo,this);
//					agent.Move (v3);


					// 回归原位
					Vector2 v2 = recoverBehavior.CalculateMove(agent,null,this);
//					Debug.Log (agent.name + " : " + v2);

//					agent.Move (v2);

				}
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

        postion.x = (x-1) * flock_width + (flock_width / 2);
        postion.y = (y-1) * flock_width + (flock_width / 2);
        newAgent.GetComponent<RectTransform>().anchoredPosition = postion;

        newAgent.Initialize(this, x, y, postion);
        agents.Add(newAgent);
    }


	// 判断附近是否有 items
	public bool HasItemsInNear(FlockAgent flockAgent){
		bool has = false;

		Collider2D[] contextColliders = Physics2D.OverlapCircleAll(flockAgent.AgentRectTransform.position,neighborRadius);

		foreach (Collider2D c in contextColliders) {
			if(c.gameObject.layer == 9){
				has = true;
			}
		}

		return has;
	}


	// 调整 agent 的状态
	public void AdjustAgentStatus(FlockAgent agent){
		if (agent.AgentStatus == AgentStatus.MOVING) {
			// 判断位置 & 大小
			bool isCorrectPosition = agent.AgentRectTransform.anchoredPosition == agent.TarVector2;
			bool isCorrectScale = agent.scaleFactor == 1f;

			if (isCorrectPosition && isCorrectScale) {
				agent.AgentStatus = AgentStatus.NORMAL;
			}

		
		}


	}

}
