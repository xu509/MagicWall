
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using System.Data;
using System;

//
//  数据源
//
public class TheDataSource : Singleton<TheDataSource>
{
    private bool _showLog = true;


    public MySqlConnection mySqlConnection;

    /// <summary>
    ///  公司测试环境
    /// </summary>
    //private static string _sqlStr = "Database=iq360_cloud_wall;"
    //            + "Server=192.168.1.100"
    //            + ";Uid=root;"
    //            + "pooling=false;"
    //            + "Password=artvoi; pooling=false;CharSet=utf8"
    //            + ";port=3306";

    // 家
    private static string _sqlStr = "Database=MagicWall;"
            + "Server=116.85.26.230"
            + ";Uid=root;"
            + "pooling=false;"
            + "Password=; pooling=false;CharSet=utf8"
            + ";port=3306";



    //
    //  Construct
    //
    protected TheDataSource() { }

    //
    //  Awake
    //
    void Awake() {

        InitData();
    }

    //
    //  初始化信息
    //
    public void InitData() {

        // CONNECT
        try
        {

            if (mySqlConnection == null || mySqlConnection.State != ConnectionState.Open) {
                mySqlConnection = new MySqlConnection(_sqlStr);
                mySqlConnection.Open();
            }
        }   
        catch (Exception e)
        {
            throw new Exception("服务器连接失败：" + e.Message.ToString());
        }
    }


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
                mySqlConnection = new MySqlConnection(_sqlStr);
                mySqlConnection.Open();
            }
            //Debug.Log("服务器连接成功");
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
        if (_showLog) {
            Debug.Log("sql : " + sql);
        }

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
                        result.Add(table.Columns[i].ColumnName, table.Rows[0][i].ToString());
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

        if (_showLog)
        {
            Debug.Log("sql : " + sql);
        }

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



