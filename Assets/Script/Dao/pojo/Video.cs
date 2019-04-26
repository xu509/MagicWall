using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  视频
//
public class Video : Generator<Video>
{
    // 视频ID
    private int _v_id;
    public int V_id { set { _v_id = value; } get { return _v_id; } }

    private string _address;
    public string Address { set { _address = value; } get { return _address; } }

    public Video Generator()
    {
        Video video = new Video();


        return video;
    }
}
