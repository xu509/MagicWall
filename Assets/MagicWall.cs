using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWall : MonoBehaviour
{
    public FlockAgent agentPrefab;
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
    int flock_width = 106;



    // speed
    float squareMaxSpeed;


    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;

        for (int i = 0; i < row * column; i++) {
            CreateNewAgent(i);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (FlockAgent agent in agents) {
            List<Transform> context = new List<Transform>();


            Vector2 move = behavior.CalculateMove(agent, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);

            

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
                                    transform
                                    );
        newAgent.name = "Agent(" + x + "," + y + ")";
        newAgent.Initialize(this);

        postion.x = (x-1) * flock_width + (flock_width / 2);
        postion.y = (y-1) * flock_width + (flock_width / 2);
        newAgent.GetComponent<RectTransform>().anchoredPosition = postion;
        agents.Add(newAgent);
    }



}
