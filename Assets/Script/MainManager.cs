using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 只在8屏时用
/// 用于判断是否为定制屏幕
/// </summary>
public class MainManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 加载配置表

        bool isCustom = DaoService.Instance.IsCustom();

        if (isCustom)
        {
            SceneManager.LoadScene("CustomScene");
        }
        else {
            SceneManager.LoadScene("SampleScene");
        }

    }

}
