using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;

public class MagicWallManager : MonoBehaviour
{
    
	#region public parameter
	public RectTransform mainPanel;
	public FlockAgent agentPrefab;

    List<FlockAgent> agents = new List<FlockAgent>();
    public List<FlockAgent> Agents { get { return agents; } }

    // TODO 

    List<RectTransform> effectAgent;
    public List<RectTransform> EffectAgent { get { return effectAgent; } }

    // 被改变的agents,改变发生点 vector2
    List<Dictionary<FlockAgent,Vector2>> changeAgents;
    public List<Dictionary<FlockAgent, Vector2>> ChangeAgents { get { return changeAgents; } }

    [Range(0, 6000)]
    public float TheDistance;   // 影响距离
    [Range(0, 20)]
    public float MoveFactor; // 移动因素
    [Range(0, 20)]
    public float ScaleFactor; // 缩放因素

    // OVER



    // flock width
    public int flock_width = 106;

    [SerializeField,Range(1f, 100f)]
	public float MoveFactor_Panel;

    //顶部logo
 //   Transform wallLogo;
	//public Transform WallLogo { get { return wallLogo; } }

    public Transform refObj;
    public Transform RefObj { get { return refObj; } }


    //layout
    public int row = 6;
    public int column = 15;

	GraphicRaycaster m_Raycaster;
	PointerEventData m_PointerEventData;
	EventSystem m_EventSystem;

    public BackgroundManager backgroundManager;


    #endregion



    #region PRIVATE
	[SerializeField]
	private WallStatusEnum status;
	public WallStatusEnum Status{get { return status;} set { status = value;}}

    #endregion

    SceneManager sceneManager;



    // Start is called before the first frame update
    void Start()
    {

        effectAgent = new List<RectTransform>();
        changeAgents = new List<Dictionary<FlockAgent, Vector2>>();
        //wallLogo = GameObject.Find("WallLogo").GetComponent<Transform>();

        #region 创建场景管理器
        sceneManager = new SceneManager();
        sceneManager.Init(this);
        #endregion

        #region 初始化背景管理器
        backgroundManager.init();
        #endregion

        // Raycaster - event
        m_Raycaster = GetComponent<GraphicRaycaster>();
		m_EventSystem = GetComponent<EventSystem> ();

//		DOTween.defaultAutoKill = true;


    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        #region 开启背景效果
        backgroundManager.run();
        #endregion

        #region 开启场景效果
        sceneManager.UpdateItems();
        #endregion


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
//			Debug.Log(results.Count);

			FlockAgent choseFlockAgent = null;

			foreach (RaycastResult result in results)
			{
				GameObject go = result.gameObject;
	
				if (go.GetComponent<FlockAgent>() != null) {
					choseFlockAgent = go.GetComponent<FlockAgent>();
//					Debug.Log ("Do choose - " + agent.name);
//					FlockAgent agent = result.gameObject.transform.parent.gameObject.GetComponent<FlockAgent> ();
//
//					if (agent != null) {
//
//						DoChosenItem (agent);
//					}  
				}
			}
				
			if (choseFlockAgent != null) {
				DoChosenItem (choseFlockAgent);
			}


				
        }
    }

//    //  创建一个新Agent
//    public void CreateNewAgent(int index) {
//        //设置位置
//        int real_index = index + 1;
//
//        Vector2 postion = new Vector2();
//
//        int y = real_index / column + 1;
//        if (real_index % column == 0) {
//            y--;
//        }
//       
//        int x = real_index % column;
//        if (x == 0) {
//            x = column;
//        }
//        
//        FlockAgent newAgent = Instantiate(
//                                    agentPrefab,
//                                    mainPanel
//                                    );
//        newAgent.name = "Agent(" + x + "," + y + ")";
//
//        postion.x = (x-1) * flock_width + (flock_width / 2);
//        postion.y = (y-1) * flock_width + (flock_width / 2);
//        newAgent.GetComponent<RectTransform>().anchoredPosition = postion;
//
//        newAgent.Initialize(this, postion);
//        agents.Add(newAgent);
//    }

//    //  创建一个新Agent
    public FlockAgent CreateNewAgent(float x,float y,float tar_x,float tar_y,int row,int column)
    {
        //设置位置
        FlockAgent newAgent = Instantiate(
                                    agentPrefab,
                                    mainPanel
                                    );
        newAgent.name = "Agent(" + row + "," + column + ")";

        Vector2 postion = new Vector2(x, y);
        newAgent.GetComponent<RectTransform>().anchoredPosition = postion;

        Vector2 ori_position = new Vector2(tar_x, tar_y);

        newAgent.Initialize(this, ori_position);
        agents.Add(newAgent);
        return newAgent;
    }



    //// 判断附近是否有 items
    //public bool HasItemsInNear(FlockAgent flockAgent){
    //	bool has = false;

    //       //Debug.Log("flockAgent.AgentRectTransform.position : " + flockAgent.AgentRectTransform.position + " | neighborRadius : " + neighborRadius);

    //       if (flockAgent.AgentRectTransform == null) {
    //           return has;
    //       }


    //       Collider2D[] contextColliders = Physics2D.OverlapCircleAll(flockAgent.AgentRectTransform.position,neighborRadius);

    //	foreach (Collider2D c in contextColliders) {
    //		if(c.gameObject.layer == 9 && c.gameObject.name != flockAgent.name)
    //           {
    //			has = true;
    //		}
    //	}

    //	return has;
    //}


    // TO DO DESTORY
    public void DoDestory() {
        // 删除 MainPanel下所有的东西
        foreach (FlockAgent agent in agents)
        {
            if (!agent.IsChoosing) {
                Destroy(agent.gameObject);
            }   
        }
        agents.Clear();
        mainPanel.anchoredPosition = Vector3.zero;

    }

    #region 选中 agent
    public void DoChosenItem(FlockAgent agent)
    {
		Debug.Log ("Status : " + Status + " agent name : " + agent.name);	
        if (!agent.IsChoosing)
        {
//            Debug.Log("[" + agent.name + "] Do Chosen Item !");
			// 将被选中的 agent 加入列表
            agent.IsChoosing = true;
			effectAgent.Add(agent.GetComponent<RectTransform>());
			updateAgents ();



//            //Debug.Log("The Distance : " + TheDistance);
//            foreach (FlockAgent item in Agents)
//            {
//                RectTransform agent_rect = agent.GetComponent<RectTransform>();
//                RectTransform item_rect = item.GetComponent<RectTransform>();
//                float dis = Vector2.Distance(agent_rect.anchoredPosition, item_rect.anchoredPosition);
//
//                if (dis < TheDistance) {
//                    if (!item.isChoosing) {
//                        item.isChanging = true;
//                        item.effectTransform = agent_rect;
//						item.updatePosition ();
//                    }
//                }
//            }
        }
        else
        {

        }
    }
    #endregion

	public void updateAgents(){
		foreach(FlockAgent ag in Agents){
			ag.updatePosition ();
		}
	}





}

public enum ItemType {
    env,activity,product
}

public enum WallStatusEnum{
	Cutting,Displaying
}

