using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;


//
//  搜索代理
//
public class SearchAgent : MonoBehaviour
{

    [DllImport("hci_hwr")]
    public static extern int hci_hwr_session_start(string str);

    void Start()
    {
        string session_config = "capkey=hwr.local.pinyin";
        int sessionId = hci_hwr_session_start(session_config);
        Debug.Log("hci_hwr" + sessionId);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
