using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAgent : MonoBehaviour
{
    [SerializeField, Range(0, 500)] private int _forceFactor;
    [SerializeField] private WallType _wallType;

    private bool _isPhysicsMoving = false;

    enum WallType {
        Top,
        Left,
        Right,
        Bottom
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void OnTriggerEnter2D(Collider2D col)
    {
        ToDoMove(col);
    }
    // 当持续在触发范围内发生时调用
    void OnTriggerStay2D(Collider2D other)
    {
        ToDoMove(other);
    }
    // 离开触发范围会调用一次
    void OnTriggerExit2D(Collider2D other)
    {
        //if (other.gameObject.layer == 5)
        //{
        //    other.gameObject.GetComponent<Rigidbody2D>().Sleep();
        //}
    }



    private void ToDoMove(Collider2D col) {

        if (col.gameObject.layer == 5)
        {
            CardAgent cardAgent = col.gameObject.GetComponent<CardAgent>();


            if (!cardAgent.isPhysicsMoving) {
                col.gameObject.GetComponent<Rigidbody2D>().WakeUp();
                col.gameObject.GetComponent<Rigidbody2D>().AddForce(GetVector2());
                cardAgent.isPhysicsMoving = true;
            }
        }
    }


    private Vector2 GetVector2() {
        Vector2 to = Vector2.zero;

        if (_wallType == WallType.Top)
        {
            to = new Vector2(0, -100 * _forceFactor);
        }
        else if (_wallType == WallType.Bottom) {
            to = new Vector2(0, 100 * _forceFactor);
        }
        else if (_wallType == WallType.Left)
        {
            to = new Vector2(100 * _forceFactor,0);
        }
        else if (_wallType == WallType.Right)
        {
            to = new Vector2(-100 * _forceFactor,0);
        }
        return to;

    }


}
