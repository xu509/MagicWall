using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomEntryManager : MonoBehaviour
{
    /// <summary>
    /// 每过一段时间切换scene
    /// </summary>
    int number;



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("number:" + number);
        number++;
        SceneManager.LoadScene("CustomSceneFiveFeiYue");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("CustomEntryManager");
    }
}
