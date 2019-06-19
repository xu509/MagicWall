using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchResultScrollBarItemAgent : MonoBehaviour
{
    // 最短宽度20，最长宽度50



    private int _index;
    private int _total;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(int index) {
        _index = index;
        //_total = total;
    }

    /// <summary>
    /// 刷新状态
    /// </summary>
    /// <param name="position">1 -> 0</param>
    public void Refresh(float position) {




    }
}
