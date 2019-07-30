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


    public MySqlConnection mySqlConnection;
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



    string _connectStr = "Database=iq360_cloud_wall;Server=192.168.1.100;Uid=root;Password=artvoi;pooling=false;CharSet=utf8;port=3306";
    //
    //  Construct
    //
    protected TheDataSource() { }

    //
    //  Awake
    //
    void Awake() {
        //_datas = new ItemDataBase();


        //JsonData data = JsonMapper.ToObject(_tempData);

        //string _t = "{type:'video',path:'/uploads/test.mp4',description:'文字描述1',cover:'/uploads/20190704/4a3d48f0e4123e3b2a3ae7132037315f.png'}";

        //List<MWMaterial> items = (List<MWMaterial>)DaoUtil.ConvertMaterialJson(_tempData);

        //foreach (MWMaterial mWMaterial in items) {
        //    Debug.Log("mWMaterial : " + mWMaterial.ToString());
            
        //}





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
            if (mySqlConnection == null || mySqlConnection.State != ConnectionState.Open) {
                mySqlConnection = new MySqlConnection(_connectStr);
                mySqlConnection.Open();
            }
        }   
        catch (Exception e)
        {
            throw new Exception("服务器连接失败：" + e.Message.ToString());
        }
    }


    //public static DataSet SelectWhere(string tableName, string[] items, string[] col, string[] operation, string[] values)
    //{

    //    if (col.Length != operation.Length || operation.Length != values.Length)
    //    {
    //        throw new Exception("输入不正确：" + "col.Length != operation.Length != values.Length");
    //    }
    //    string query = "SELECT" + items[0];
    //    for (int i = 1; i < items.Length; ++i)
    //    {
    //        query += ", " + items[i];
    //    }
    //    query += " FROM " + tableName + " WHERE " + col[0] + operation[0] + "'" + values[0] + "' ";
    //    for (int i = 1; i < col.Length; ++i)
    //    {
    //        query += " AND " + col[i] + operation[i] + "'" + values[0] + "' ";
    //    }
    //    return GetDataSet(query);
    //}

    /// <summary>
    /// 执行sql语句，获取DataSet
    /// </summary>
    /// <param name="sqlString"></param>
    /// <returns></returns>
    //public DataSet GetDataSet(string sqlString)
    //{
    //    //Debug.Log("SQL： " + sqlString);
    //    if (mySqlConnection.State == ConnectionState.Open)
    //    {
    //        DataSet ds = new DataSet();
    //        try
    //        {
    //            MySqlDataAdapter da = new MySqlDataAdapter(sqlString, mySqlConnection);
    //            da.Fill(ds);
    //        }
    //        catch (Exception ee)
    //        {
    //            throw new Exception("SQL:" + sqlString + "/n" + ee.Message.ToString());
    //        }
    //        finally
    //        {
    //        }
    //        return ds;
    //    }
    //    return null;
    //}


    /// <summary>
    /// 关闭数据库链接
    /// </summary>
    public void CloseSql()
    {
        if (mySqlConnection != null && mySqlConnection.State == ConnectionState.Open)
        {
            mySqlConnection.Close();
            mySqlConnection.Dispose();
            mySqlConnection = null;
        }
    }


    /// <summary>
    ///  链接数据库
    /// </summary>
    private void ConnectDataBase() {
        // CONNECT
        try
        {
            if (mySqlConnection == null || mySqlConnection.State != ConnectionState.Open)
            {
                mySqlConnection = new MySqlConnection(_connectStr);
                mySqlConnection.Open();
            }
            Debug.Log("服务器连接成功");
        }
        catch (Exception e)
        {
            throw new Exception("服务器连接失败：" + e.Message.ToString());
        }
    }



    /// <summary>
    /// 查询单个数据
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public Dictionary<string,object> SelectOne(string sql) {
        Dictionary<string, object> result = null;

        try
        {
            ConnectDataBase();

            if (mySqlConnection.State == ConnectionState.Open)
            {
                DataSet ds = new DataSet();
                MySqlDataAdapter da = new MySqlDataAdapter(sql, mySqlConnection);
                da.Fill(ds);

                DataTable table = ds.Tables[0];

                if (table.Rows.Count == 1)
                {
                    result = new Dictionary<string, object>();
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        result.Add(table.Columns[i].ColumnName, table.Columns[i].ToString());
                    }
                    return result;
                }
                else if (table.Rows.Count > 1)
                {
                    Debug.LogError("[查询失败] 查询到多个结果：" + sql);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("查询失败：" + e.Message.ToString() + " \n sql:" + sql);
        }
        finally {
            CloseSql();
        }

        return result;
    }


    /// <summary>
    /// 查询列表数据
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public List<Dictionary<string, object>> SelectList(string sql)
    {
        List<Dictionary<string, object>> result = null;

        try
        {
            ConnectDataBase();

            if (mySqlConnection.State == ConnectionState.Open)
            {
                DataSet ds = new DataSet();
                MySqlDataAdapter da = new MySqlDataAdapter(sql, mySqlConnection);
                da.Fill(ds);

                DataTable table = ds.Tables[0];
                result = new List<Dictionary<string, object>>();

                for (int i = 0; i < table.Rows.Count; i++) {
                    Dictionary<string, object> rowDic = new Dictionary<string, object>();
                    for (int j = 0; j < table.Columns.Count; j++) {
                        rowDic.Add(table.Columns[j].ColumnName, table.Rows[i][j].ToString());
                    }
                    result.Add(rowDic);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("查询失败：" + e.Message.ToString() + " \n sql:" + sql);
        }
        finally
        {
            CloseSql();
        }


        return result;
    }






}



