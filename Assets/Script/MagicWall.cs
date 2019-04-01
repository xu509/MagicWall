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

    // flock width
    public int flock_width = 106;

    [SerializeField,Range(1f, 100f)]
	public float driveFactor = 10f;
	[Range(0f, 100f)]
	public float maxSpeed = 5f;
	[SerializeField,Range(0.1f,10f)]
	public float recoverFactor = 0.6f;

	[SerializeField,Range(1f, 20f)]
	public float neighborRadius ; // flock neighbor radius

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
    [SerializeField, Range(1f, 10f)]
    public float scaleSpeed = 1;  // 缩放的速度

    [SerializeField, Range(1f, 10f)]
    public float recoverMoveSpeed = 1;  // 恢复的移动速度

    public float agent_colider_radius = 4f; // 碰撞体半径

    //layout
    public int row = 6;
    public int column = 30;
    #endregion



    #region PRIVATE
    List<FlockAgent> agents = new List<FlockAgent>();
    public List<FlockAgent> Agents { get {return agents; } }


    // speed
    float squareMaxSpeed;
    #endregion

    SceneManager sceneManager;

    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;

        //for (int i = 0; i < row * column; i++) {
        //    CreateNewAgent(i);
        //}

        wallLogo = GameObject.Find("WallLogo").GetComponent<Transform>();
        //
        //        CreateRefAgent();
        sceneManager = new SceneManager();
        sceneManager.Init(this);


    }

    // Update is called once per frame
    void Update()
    {
        // 画布移动
        sceneManager.UpdateItems(this);

        // 添加并删除碰撞体
        //foreach (FlockAgent agent in agents) {
        //	if (HasItemsInNear (agent)) {
        //		scaleBehavior.DoScale (agent, this);
        //	} else {
        //		if (agent.AgentStatus == AgentStatus.MOVING) {
        //			// 回归原位
        //			Vector2 v2 = recoverBehavior.CalculateMove(agent,null,this);

        //                  reScaleBehavior.DoScale(agent, this);
        //              }
        //          }
        //}
        if (Input.GetMouseButton(0)) {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,Camera.main.nearClipPlane));

            Debug.Log("Near clip plane : " + Camera.main.nearClipPlane + " | mousePos : " + mousePos + " | Input.mousePosition : " + Input.mousePosition);


        

            Debug.DrawRay(mousePos, Camera.main.transform.forward * 100 , Color.green);

            if (Physics.Raycast(ray,out hit)) {
                Debug.Log("Input.GetMouseButton(0)");

                Debug.Log(hit.transform.name);

            }
        }
    }

    //  创建一个新Agent
    public void CreateNewAgent(int index) {
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

        //Debug.Log("flockAgent.AgentRectTransform.position : " + flockAgent.AgentRectTransform.position + " | neighborRadius : " + neighborRadius);

        if (flockAgent.AgentRectTransform == null) {
            return has;
        }


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

    // TO DO DESTORY
    public void DoDestory() {
        // 删除 MainPanel下所有的东西
        foreach (FlockAgent agent in agents)
        {
            if (agent.AgentStatus != AgentStatus.CHOOSING) {
                Destroy(agent.gameObject);
            }   
        }
        agents.Clear();
        mainPanel.anchoredPosition = Vector3.zero;

    }



}
