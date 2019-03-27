using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
    int index;
    public int AgentIndex { get { return index; } }

    int index_x;
    public int AgentIndex_x { get { return index_x; } }

    int index_y;
    public int AgentIndex_y { get { return index_y; } }

    FlockStatus status;
    public FlockStatus AgentStatus{ get { return status; } }




    MagicWall agentMagicWall;
    public MagicWall AgentMagicWall { get { return agentMagicWall; } }

    Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }

    RectTransform agentRectTransform;
    public RectTransform AgentRectTransform { get { return agentRectTransform; } }



    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<Collider2D>();
        agentRectTransform = GetComponent<RectTransform>();


    }

    public void Initialize(MagicWall magicWall,int index,int index_x,int index_y)
    {
        agentMagicWall = magicWall;
        this.index = index;
        this.index_x = index_x;
        this.index_y = index_y;
        this.status = FlockStatus.MOVE;
    }

    public void Move(Vector2 velocity)
    {
        if (velocity == Vector2.zero)
        {
        }
        else {
            // turn agent face the direction that it's going to be moving toward.(箭头的指向向上)
            transform.up = velocity;
            transform.position += (Vector3)velocity * Time.deltaTime;
        }




    }

}

public enum FlockStatus {
    MOVE,SHOW
}
