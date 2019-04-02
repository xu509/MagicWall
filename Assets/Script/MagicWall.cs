using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class MagicWall : MonoBehaviour
{
    
	#region public parameter
	public RectTransform mainPanel;
	public FlockAgent agentPrefab;

	//public FlockBehavior moveBehavior;
	//public FlockBehavior scaleBehavior;
	//public FlockBehavior reScaleBehavior;
	//public FlockBehavior recoverBehavior;

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

    Transform wallLogo;
	public Transform WallLogo { get { return wallLogo; } }

    public Transform refObj;
    public Transform RefObj { get { return refObj; } }


    //缩放状态
    [SerializeField, Range(1f, 10f)]
    public float scaleSpeed = 1;  // 缩放的速度

    [SerializeField, Range(1f, 10f)]
    public float recoverMoveSpeed = 1;  // 恢复的移动速度

    [SerializeField, Range(1f, 20f)]
    public float choosingSpeed; // 被选中的 agent 放大的速度

    //layout
    public int row = 6;
    public int column = 30;

	GraphicRaycaster m_Raycaster;
	PointerEventData m_PointerEventData;
	EventSystem m_EventSystem;

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

        wallLogo = GameObject.Find("WallLogo").GetComponent<Transform>();
        //
        //        CreateRefAgent();
        sceneManager = new SceneManager();
        sceneManager.Init(this);

		// Raycaster - event
		m_Raycaster = GetComponent<GraphicRaycaster>();
		m_EventSystem = GetComponent<EventSystem> ();


    }

    // Update is called once per frame
    void Update()
    {
        // 画布移动
        sceneManager.UpdateItems(this);

        if (Input.GetMouseButton(0)) {

			//Set up the new Pointer Event
			m_PointerEventData = new PointerEventData(m_EventSystem);
			//Set the Pointer Event Position to that of the mouse position
			m_PointerEventData.position = Input.mousePosition;

			//Create a list of Raycast Results
			List<RaycastResult> results = new List<RaycastResult>();

			//Raycast using the Graphics Raycaster and mouse click position
			m_Raycaster.Raycast(m_PointerEventData, results);

			//For every result returned, output the name of the GameObject on the Canvas hit by the Ray
			foreach (RaycastResult result in results)
			{
				GameObject go = result.gameObject;
				if (go != null) {
					FlockAgent agent = result.gameObject.transform.parent.gameObject.GetComponent<FlockAgent> ();

					if (agent != null) {
						DoChosenItem (agent);
					}  
				}
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
			if(c.gameObject.layer == 9 && c.gameObject.name != flockAgent.name)
            {
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
                if (!agent.AgentRigidbody2D.simulated) {
                    agent.AgentRigidbody2D.simulated = true;
                }

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


    public void DoChosenItem(FlockAgent agent)
    {
        if (agent.agentStatus != AgentStatus.CHOOSING)
        {
            Debug.Log("[" + name + "] Do Chosen Item !");

            //			agentRigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;

            


            agent.agentStatus = AgentStatus.CHOOSING;
        }
        else
        {

        }



    }



}
