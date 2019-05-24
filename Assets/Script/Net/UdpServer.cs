using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UdpServer : Singleton<UdpServer>
{
    private float lastReceiveTime = 0f;
    private bool _hasInit = false;


    //以下默认都是私有的成员
    Socket socket; //目标socket
    EndPoint clientEnd; //客户端
    IPEndPoint ipEnd; //侦听端口
    string recvStr; //接收的字符串
    string sendStr; //发送的字符串
    byte[] recvData = new byte[1024]; //接收的数据，必须为字节
    byte[] sendData = new byte[1024]; //发送的数据，必须为字节
    int recvLen; //接收的数据长度


    //委托队列
    private Queue<Action> asyncQueue = new Queue<Action>();
    private Queue<Action> mainQueue = new Queue<Action>();


    Thread connectThread; //连接线程

    //主线程每次Update执行Function数量
    private static int doUpdate = 5;



    public void Listening() {

        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("您按下了W键");

            MagicWallManager.Instance.Reset();

        }

        if (_hasInit) {
            //SocketReceive();
            //if (CanReceive())
            //{
            //    SocketReceive();
            //}
            DoFunction();
        }


    }


    //初始化
    void InitSocket()
    {
        //定义侦听端口,侦听任何IP
        ipEnd = new IPEndPoint(IPAddress.Any, 9999);
        //定义套接字类型,在主线程中定义
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //服务端需要绑定ip
        socket.Bind(ipEnd);
        //定义客户端
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        clientEnd = (EndPoint)sender;
        print("waiting for UDP dgram");

        ////开启一个线程连接，必须的，否则主线程卡死
        connectThread = new Thread(new ThreadStart(SocketReceive));
        connectThread.Start();
        _hasInit = true;
    }


    void SocketSend(string sendStr)
    {
        //清空发送缓存
        sendData = new byte[1024];
        //数据类型转换
        sendData = Encoding.ASCII.GetBytes(sendStr);
        //发送给指定服务端
        socket.SendTo(sendData, sendData.Length, SocketFlags.None, ipEnd);
    }



    //服务器接收, 每一秒接受一次
    void SocketReceive()
    {
        while (true)
        {
            //对data清零
            recvData = new byte[1024];
            //获取客户端，获取客户端数据，用引用给客户端赋值
            recvLen = socket.ReceiveFrom(recvData, ref clientEnd);
            print("message from: " + clientEnd.ToString()); //打印客户端信息
                                                            //输出接收到的数据
            recvStr = Encoding.ASCII.GetString(recvData, 0, recvLen);
            print(recvStr);

            // TODO 当受到制定信息，则进行处理
            if (true)
            {
                AfterRun();
            }
        }
        

            ////将接收到的数据经过处理再发送出去
            //sendStr = "From Server: " + recvStr;
            //SocketSend(sendStr);
        
    }


    //连接关闭
    void SocketQuit()
    {
        //关闭线程
        if (connectThread != null)
        {
            connectThread.Interrupt();
            connectThread.Abort();
        }
        //最后关闭socket
        if (socket != null)
            socket.Close();
    }

    // Use this for initialization
    void Awake()
    {
        InitSocket(); //在这里初始化

    }


    void OnApplicationQuit()
    {
        SocketQuit();
    }

    // 重置
    public void Reset() {
        _hasInit = false;

        // 停止socket 
        SocketQuit();

        // 参数归零
        lastReceiveTime = 0f;

        // 重置
        InitSocket();

    }


    // 判断是否可接受
    private bool CanReceive() {
        if (Time.time - lastReceiveTime > 1f)
        {
            lastReceiveTime = Time.time;
            return true;
        }
        else {
            return false;
        }


    }

    private void AfterRun() {
        MagicWallManager.Instance.Reset();
    }



    //执行Action(根据线程判断对应的方法)
    private void DoFunction()
    {
        if (Thread.CurrentThread == connectThread)
        {
            if (asyncQueue.Count > 0)
            {
                var func = asyncQueue.Dequeue();
                func();
            }
        }
        else
        {
            if (mainQueue.Count > 0)
            {
                int number = doUpdate;
                do
                {
                    var func = mainQueue.Dequeue();
                    func();
                    number--;
                } while (number > 0 && mainQueue.Count > 0);
            }
        }
    }

}
