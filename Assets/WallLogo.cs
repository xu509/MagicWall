using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallLogo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionExit2D(Collision2D collision)
    {

        Rigidbody2D rd = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rd != null) {
            rd.simulated = false;
            
            Debug.Log(collision.gameObject.name);
            Debug.Log("Destory RigidBody2D");
        }



        //collision.gameObject

    }
}
