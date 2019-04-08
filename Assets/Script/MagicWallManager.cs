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

    // 面板的差值
    float panelOffset = 0f;
    public float PanelOffset { get { return panelOffset; } set { panelOffset = value; } }

    // OVER



    // flock width
    float itemWidth;
    public float ItemWidth { get { return itemWidth; } set { itemWidth = value; } }

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

    private RectTransform operationPanel;


    // 上一次点击的时间
    float lastClickDownTime = 0f;

    // 按下与抬起的间隔
    float clickIntervalTime = 0.5f;

    // 选中的agent
    FlockAgent chooseFlockAgent = null;


    #endregion
    SceneManager sceneManager;



    // Start is called before the first frame update
    void Start()
    {
        effectAgent = new List<RectTransform>();
        changeAgents = new List<Dictionary<FlockAgent, Vector2>>();
        operationPanel = GameObject.Find("OperatePanel").GetComponent<RectTransform>();
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



        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("GetMouseButtonDown");


            if (lastClickDownTime == 0f)
            {
                lastClickDownTime = Time.time;
            }

            chooseFlockAgent = getAgentsByMousePosition();
            Debug.Log("chooseFlockAgent : " + chooseFlockAgent);

        }

        if (Input.GetMouseButton(0))
        {
            Debug.Log("GetMouseButton");

            if ((Time.time - lastClickDownTime) > clickIntervalTime)
            {
                Debug.Log("recognize Drag");

                // 此处为拖拽事件
                if (chooseFlockAgent != null)
                {
                    DoDragItem(chooseFlockAgent);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("GetMouseButtonUp");

            if ((Time.time - lastClickDownTime) < clickIntervalTime)
            {
                Debug.Log("recognize click");

                // 此处为点击事件
                if (chooseFlockAgent != null)
                {
                    DoChosenItem(chooseFlockAgent);
                }

            }

            lastClickDownTime = 0f;
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
        newAgent.GenVector2 = postion;

        newAgent.Initialize(this, ori_position,postion,row,column);
        agents.Add(newAgent);
        return newAgent;
    }

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
        if (!agent.IsChoosing)
        {
            // 将选中的 agent 放入操作层
            agent.transform.parent = operationPanel;
            agent.GetComponent<RectTransform>().anchoredPosition -= new Vector2(PanelOffset , 0 );

            // 将被选中的 agent 加入列表
            agent.IsChoosing = true;
			effectAgent.Add(agent.GetComponent<RectTransform>());

            // 选中后的动画效果，逐渐变大

            Vector2 newSizeDelta = new Vector2(ItemWidth * 2, ItemWidth * 2);

            agent.AgentRectTransform.DOSizeDelta(newSizeDelta, 2f).OnUpdate(() => DoSizeDeltaUpdateCallBack(agent));


			//updateAgents ();
        }
        else
        {
            agent.transform.parent = mainPanel;
            agent.IsChoosing = false;
            effectAgent.Remove(agent.GetComponent<RectTransform>());

            Vector2 newSizeDelta = new Vector2(ItemWidth, ItemWidth);
            agent.AgentRectTransform.DOSizeDelta(newSizeDelta, 2f).OnUpdate(() => DoSizeDeltaUpdateCallBack(agent));
            agent.GetComponent<RectTransform>().DOAnchorPos(agent.OriVector2,2f);




        }
    }
    #endregion

    void DoSizeDeltaUpdateCallBack(FlockAgent agent) {
        //Debug.Log(agent.AgentRectTransform.sizeDelta.x);
        agent.Width = agent.AgentRectTransform.sizeDelta.x;

    }


    public void DoDragItem(FlockAgent agent) {
        if (agent.IsChoosing) {

            //Vector3 v = Camera.main.WorldToScreenPoint(Input.mousePosition);
            //Debug.Log(Input.mousePosition);
            //Input.mousePosition

            agent.GetComponent<RectTransform>().DOAnchorPos((Vector2)Input.mousePosition, Time.deltaTime);
            updateAgents();

        }
    }

    


    #region 更新所有的 agent
    public void updateAgents(){
            //Debug.Log("DOING UPDATEAGENTS!");
            foreach (FlockAgent ag in Agents)
            {
                ag.updatePosition();
            }
        		
	}
    #endregion

    #region 根据鼠标点击位置获取 agent
    FlockAgent getAgentsByMousePosition() {
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        FlockAgent choseFlockAgent = null;

        foreach (RaycastResult result in results)
        {
            GameObject go = result.gameObject;

            // 通过layer取到agents的子图片
            if (go.layer == 10)
            {
                //Debug.Log(go.name);
                choseFlockAgent = go.transform.parent.GetComponent<FlockAgent>();
            }
            if (go.GetComponent<FlockAgent>() != null)
            {
                choseFlockAgent = go.GetComponent<FlockAgent>();
            }
        }
        return choseFlockAgent;
    }
    #endregion


    #region 获取两块画布的偏移量
    public void updateOffsetOfCanvas() {
        if (status == WallStatusEnum.Cutting)
        {
            PanelOffset = 0f;
        }
        else {
            Vector2 v1 = mainPanel.anchoredPosition;
            Vector2 v2 = operationPanel.anchoredPosition;

            float offset = (v2.x - v1.x);

            PanelOffset = offset;
        }

        //Debug.Log("OFFSET IS " + PanelOffset);
    }
    #endregion




}

public enum ItemType {
    env,activity,product
}

public enum WallStatusEnum{
	Cutting,Displaying
}

