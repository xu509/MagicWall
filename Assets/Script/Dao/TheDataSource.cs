using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using MySql.Data.MySqlClient;
using System.Data;
using System;
using LitJson;
//
//  数据源
//
public class TheDataSource : Singleton<TheDataSource>
{

    //
    //  Parameter
    //
    private ItemDataBase _datas;


    public static MySqlConnection mySqlConnection;
    ////数据库名称
    //public string database = "iq360_cloud_wall";
    ////数据库IP
    //private string host = "192.168.1.100";
    ////端口
    //private string port = "3306";
    ////用户名
    //private string username = "root";
    ////用户密码
    //private string password = "artvoi";

    private string _tempData = "[{type:'video',path:'/uploads/test.mp4',description:'文字描述1',cover:'/uploads/20190704/4a3d48f0e4123e3b2a3ae7132037315f.png'}," +
        "{type:'video',path:'/uploads/test.mp4',description:'文字描述2',cover:'/uploads/20190704/4ebb6627a34d6bb18712b146217359a2.jpg'}," +
        "{type:'video',path:'/uploads/test.mp4',description:'文字描述3',cover:'/uploads/20190704/26bb2fc075103c1fc59280a11730359f.png'}," +
        "{type:'video',path:'/uploads/test.mp4',description:'文字描述4',cover:'/uploads/20190704/28e7a9879a442fc1923245224e23c1ea.jpg'}," +
        "{type:'video',path:'/uploads/test.mp4',description:'文字描述5',cover:'/uploads/20190704/606f376c596cc97f132ce54a897ad064.jpg'}," +
        "{type:'video',path:'/uploads/test.mp4',description:'文字描述6',cover:'/uploads/20190704/116fcec5f6e1c9c3ec617a80295ffa1c.jpg'}," +
        "{type:'video',path:'/uploads/test.mp4',description:'文字描述7',cover:'/uploads/20190704/9203df4ef4c8e2dd227ae02361f864e8.jpg'}," +
        "{type:'video',path:'/uploads/test.mp4',description:'文字描述8',cover:'/uploads/20190704/9595e33759f9c248112781bbc7291f13.jpg'}," +
        "{type:'video',path:'/uploads/test.mp4',description:'文字描述9',cover:'/uploads/20190704/5331447e01d5441fab9712ed823c996e.png'}," +
        "{type:'video',path:'/uploads/test.mp4',description:'文字描述10',cover:'/uploads/20190704/b6be8f64a52b8b09d5549f2f3537bd66.jpg'}]";




    string sql = "Database=iq360_cloud_wall;Server=192.168.1.100;Uid=root;Password=artvoi;pooling=false;CharSet=utf8;port=3306";
    //
    //  Construct
    //
    protected TheDataSource() { }

    //
    //  Awake
    //
    void Awake() {
        //_datas = new ItemDataBase();


        Debug.Log("JsonData data = JsonMapper.ToObject(_tempData);");
        //JsonData data = JsonMapper.ToObject(_tempData);

        string _t = "{type:'video',path:'/uploads/test.mp4',description:'文字描述1',cover:'/uploads/20190704/4a3d48f0e4123e3b2a3ae7132037315f.png'}";

        MWMaterial m = new MWMaterial();
        m.ConvertJSONToObject(_t);



        //Debug.Log(data.ToString());




        //MySqlCommand

        InitData();
    }

    //
    //  初始化信息
    //
    public void InitData() {

        // 初始化MySql链接，提供MySql接口

        // CONNECT
        try
        {
            mySqlConnection = new MySqlConnection(sql);
            mySqlConnection.Open();
            Debug.Log("服务器连接成功");
        }   
        catch (Exception e)
        {
            throw new Exception("服务器连接失败：" + e.Message.ToString());
        }
    }

    public void CloseSql()
    {
        if (mySqlConnection != null)
        {
            mySqlConnection.Close();
            mySqlConnection.Dispose();
            mySqlConnection = null;
        }
    }


    public static DataSet SelectWhere(string tableName, string[] items, string[] col, string[] operation, string[] values)
    {

        if (col.Length != operation.Length || operation.Length != values.Length)
        {
            throw new Exception("输入不正确：" + "col.Length != operation.Length != values.Length");
        }
        string query = "SELECT" + items[0];
        for (int i = 1; i < items.Length; ++i)
        {
            query += ", " + items[i];
        }
        query += " FROM " + tableName + " WHERE " + col[0] + operation[0] + "'" + values[0] + "' ";
        for (int i = 1; i < col.Length; ++i)
        {
            query += " AND " + col[i] + operation[i] + "'" + values[0] + "' ";
        }
        return GetDataSet(query);
    }

    /// <summary>
    /// 执行sql语句，获取DataSet
    /// </summary>
    /// <param name="sqlString"></param>
    /// <returns></returns>
    public static DataSet GetDataSet(string sqlString)
    {
        //Debug.Log("SQL： " + sqlString);
        if (mySqlConnection.State == ConnectionState.Open)
        {
            DataSet ds = new DataSet();
            try
            {
                MySqlDataAdapter da = new MySqlDataAdapter(sqlString, mySqlConnection);
                da.Fill(ds);
            }
            catch (Exception ee)
            {
                throw new Exception("SQL:" + sqlString + "/n" + ee.Message.ToString());
            }
            finally
            {
            }
            return ds;
        }
        return null;
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


