using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
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

    public void Initialize(MagicWall magicWall)
    {
        agentMagicWall = magicWall;
    }

    public void Move(Vector2 velocity)
    {
        // turn agent face the direction that it's going to be moving toward.(箭头的指向向上)
        if(velocity != Vector2.zero){
            transform.up = velocity;
            transform.position += (Vector3)velocity * Time.deltaTime;
        }

    }

}
