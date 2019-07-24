using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

//
//  数据源
//
public class TheDataSource : Singleton<TheDataSource>
{

    //
    //  Parameter
    //
    private ItemDataBase _datas;

    //
    //  Construct
    //
    protected TheDataSource() { }

    //
    //  Awake
    //
    void Awake() {
        _datas = new ItemDataBase();

        InitData();
    }

    //
    //  初始化信息
    //
    public void InitData() {
        
        // 初始化MySql链接，提供MySql接口

    }


    ////
    ////  持久化数据
    ////
    //private void SaveItems() {
    //    // open a new xml file
    //    XmlSerializer serializer = new XmlSerializer(typeof(ItemDataBase));
    //    FileStream stream = new FileStream(Application.dataPath + "/StreamingFiles/data.xml", FileMode.Create);
    //    serializer.Serialize(stream, _datas);
    //    stream.Close();

    //}

    
}

[System.Serializable]
public class ItemDataBase
{

    //通过注解设置的字段会在XML根路径上使用
    [XmlArray("Enterprises")]
    List<Enterprise> _enterprises;


    public List<Enterprise> Enterprises
    {
        set {
            _enterprises = value;
        }
        get {
            return _enterprises;
        }
    }
}


