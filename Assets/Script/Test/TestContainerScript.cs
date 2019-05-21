using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;



public class TestContainerScript : MonoBehaviour
{
    [SerializeField] TestScript prefab;
    [SerializeField] Transform p;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(prefab, p);

    }


}
