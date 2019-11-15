using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine.SceneManagement;

namespace MagicWall
{
    public class UdpManager : MonoBehaviour
    {
        private MagicWallManager _manager;

        private float lastReceiveTime = 0f;
        private bool _hasInit = false;

        private bool _receMsg = false;
        public bool receMsg { get { return _receMsg; } }


        //以下默认都是私有的成员
        Socket socket; //目标socket
        EndPoint clientEnd; //客户端
        IPEndPoint ipEnd; //侦听端口
        string recvStr; //接收的字符串
        string sendStr; //发送的字符串
        byte[] recvData = new byte[1024]; //接收的数据，必须为字节
        byte[] sendData = new byte[1024]; //发送的数据，必须为字节
        int recvLen; //接收的数据长度

        //
        private Queue<int> _changeSceneQueue;

        //废弃  委托队列
        private Queue<Action> asyncQueue = new Queue<Action>();
        private Queue<Action> mainQueue = new Queue<Action>();


        Thread connectThread; //连接线程

        //主线程每次Update执行Function数量
        private static int doUpdate = 5;


        void Awake() {


            var obj = GameObject.Find("UdpHandler");

            //DontDestroyOnLoad(this);
        }

        public void Init() {
            _changeSceneQueue = new Queue<int>();
            InitSocket(); //在这里初始化
        }



        /// <summary>
        ///     初始化 socket 服务器
        /// </summary>
        void InitSocket()
        {
            Debug.Log("Init Socket!");

            if (!_hasInit) {
                //定义侦听端口,侦听任何IP
                ipEnd = new IPEndPoint(IPAddress.Any, 9999);

                //定义套接字类型,在主线程中定义
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                //服务端需要绑定ip
                socket.Bind(ipEnd);
                //定义客户端
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                clientEnd = (EndPoint)sender;
                print("初始化socket！");

                ////开启一个线程连接，必须的，否则主线程卡死
                connectThread = new Thread(new ThreadStart(SocketReceive));
                connectThread.Start();
                Debug.Log("Init Socket Success!");


                _hasInit = true;
            }

            Debug.Log("Init Socket End !");


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
                //print("message from: " + clientEnd.ToString()); //打印客户端信息
                //输出接收到的数据
                recvStr = Encoding.ASCII.GetString(recvData, 0, recvLen);
                print("接收到信号：" + recvStr);

                int rnumber = -1;

                int.TryParse(recvStr, out rnumber);//2

                if (rnumber > 0) {
                    AddSceneIndex(rnumber);
                }


                // TODO 当受到制定信息，则进行处理
                //if (true)
                //{
                //    AfterRun();
                //}
            }


            ////将接收到的数据经过处理再发送出去
            //sendStr = "From Server: " + recvStr;
            //SocketSend(sendStr);

        }

        private void Update()
        {

            if (_changeSceneQueue.Count > 0) {
                int si = _changeSceneQueue.Dequeue();

                // 1 : feiyue ; 2: fengxian 3: tubu 4: aiqi
                int to;
                if (si == 1)
                {
                    to = 1;
                }
                else if (si == 2)
                {
                    to = 22;
                }
                else if (si == 3)
                {
                    to = 42;
                }
                else {
                    to = 63;
                }

                Debug.Log("si : " + si + " - TO :" + to);


                _manager = GameObject.Find("MagicWall").GetComponent<MagicWallManager>();
                _manager.magicSceneManager.CloseCurrent(() =>
                {
                    _manager.magicSceneManager.JumpTo(to);
                });
            }


            if (Input.GetKeyDown(KeyCode.A)) {
                AddSceneIndex(1);
            }


        }




        void OnApplicationQuit()
        {
            //关闭线程
            if (connectThread != null)
            {
                connectThread.Interrupt();
                connectThread.Abort();
            }

            socket?.Close();
        }



        private void AddSceneIndex(int sindex) {
            _receMsg = true;
            _changeSceneQueue.Enqueue(sindex);
        }


        private void OnDestroy()
        {
        }


    }
}