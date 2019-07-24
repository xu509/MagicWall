using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using MySql.Data.MySqlClient;
using System.Data;
using System;
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


    public DataSet SelectWhere(string tableName, string[] items, string[] col, string[] operation, string[] values)
    {

        if (col.Length != operation.Length || operation.Length != values.Length)
        {
            throw new Exception("输入不正确：" + "col.Length != operation.Length != values.Length");
        }
        string query = "SELECT " + items[0];
        for (int i = 1; i < items.Length; ++i)
        {
            query += ", " + items[i];
        }
        query += " FROM " + tableName + " WHERE " + col[0] + operation[0] + "'" + values[0] + "' ";
        for (int i = 1; i < col.Length; ++i)
        {
            query += " AND " + col[i] + operation[i] + "'" + values[0] + "' ";
        }
        return QuerySet(query);
    }

    public static DataSet QuerySet(string sqlString)
    {
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


