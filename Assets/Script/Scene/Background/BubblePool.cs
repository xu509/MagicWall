using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 气球对象池
/// </summary>
public class BubblePool
{
    #region 单例
    private static BubblePool instance;
    private BubblePool(int total)
    {
        _pool = new Queue<BubbleAgent>();
        _total = total;
    }
    public static BubblePool GetInstance(int total)
    {
        if (instance == null)
        {
            instance = new BubblePool(total);
        }
        return instance;
    }

    #endregion

    /// <summary>
    /// 对象池
    /// </summary>
    private Queue<BubbleAgent> _pool;


    /// <summary>
    /// 对象池总数
    /// </summary>
    private int _total;



    /// <summary>
    /// 从对象池中获取对象
    /// </summary>
    /// <param name="objName"></param>
    /// <returns></returns>
    public BubbleAgent GetObj(BubbleType bubbleType)
    {
        // 如果对象池内无剩余可用对象，则不进行返回
        if (_pool.Count == 0) {


        }


        //结果对象
        GameObject result = null;
        //判断是否有该名字的对象池
        if (pool.ContainsKey(objName))
        {
            //对象池里有对象
            if (pool[objName].Count > 0)
            {
                //获取结果
                result = pool[objName][0];
                //激活对象
                result.SetActive(true);
                //从池中移除该对象
                pool[objName].Remove(result);
                //返回结果
                return result;
            }
        }
        //如果没有该名字的对象池或者该名字对象池没有对象

        GameObject prefab = null;
        //如果已经加载过该预设体
        if (prefabs.ContainsKey(objName))
        {
            prefab = prefabs[objName];
        }
        else     //如果没有加载过该预设体
        {
            //加载预设体
            prefab = Resources.Load<GameObject>("Prefabs/" + objName);
            //更新字典
            prefabs.Add(objName, prefab);
        }

        //生成
        result = UnityEngine.Object.Instantiate(prefab);
        //改名（去除 Clone）
        result.name = objName;
        //返回
        return result;
    }

    /// <summary>
    /// 回收对象到对象池
    /// </summary>
    /// <param name="objName"></param>
    public void RecycleObj(GameObject obj)
    {
        //设置为非激活
        obj.SetActive(false);
        //判断是否有该对象的对象池
        if (pool.ContainsKey(obj.name))
        {
            //放置到该对象池
            pool[obj.name].Add(obj);
        }
        else
        {
            //创建该类型的池子，并将对象放入
            pool.Add(obj.name, new List<GameObject>() { obj });
        }

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

public enum BubbleType {
    Clear,Dim
}
