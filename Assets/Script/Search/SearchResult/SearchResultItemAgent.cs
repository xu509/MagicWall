using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  搜索 Items Agent
/// </summary>
public class SearchResultItemAgent : MonoBehaviour
{
    [SerializeField] RawImage _image;

    private int _id; // id
    private MWTypeEnum _type;   //  类型

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init() {

    }

    public void InitData(SearchBean searchBean)
    {
        string address = MagicWallManager.FileDir + searchBean.cover;
        _image.texture = TextureResource.Instance.GetTexture(address);

        _id = searchBean.id;
        _type = searchBean.type;
    }

    /// <summary>
    /// 点击
    /// </summary>
    public void DoClick() {
        //  点开卡片
        Debug.Log("点开卡片");
        //  关闭原来的搜索框

    }

}
