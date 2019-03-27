using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
	int index;
	int x;
	int y;

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

	public void Initialize(MagicWall magicWall,int index,int x,int y)
    {
        agentMagicWall = magicWall;
		this.index = index;
		this.x = x;
		this.y = y;
    }

    public void Move(Vector2 velocity)
    {
        // turn agent face the direction that it's going to be moving toward.(箭头的指向向上)
        if(velocity != Vector2.zero){
            transform.up = velocity;
            transform.position += (Vector3)velocity * Time.deltaTime;
        }

    }


	void Update(){

	}



	// 向右下缩小
	void DoScaleRD(){
		float width = agentMagicWall.flock_width / 2;

		// 缩小
		RectTransform rt = GetComponent<RectTransform>();

		//缩小至右下
		rt.DOScale (0.5f, Time.deltaTime);
		Vector2 v = new Vector2 (width, -width);
		rt.DOAnchorPos (rt.anchoredPosition + v,Time.deltaTime);
		BoxCollider2D collider = GetComponent<BoxCollider2D> ();
		collider.edgeRadius = collider.edgeRadius / 2;
	}



}
