using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomEntryManager : MonoBehaviour
{
    /// <summary>
    /// 每过一段时间切换scene
    /// </summary>


    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("CustomSceneFiveFeiYue");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("CustomEntryManager");
    }
}
