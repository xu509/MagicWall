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

    private string _description;
    public string Description { set { _description = value; } get { return _description; } }

    private string _address;
    public string Address { set { _address = value; } get { return _address; } }

    private string _cover;
    public string Cover { set { _cover = value; } get { return _cover; } }

    public Video Generator()
    {
        string[] descriptions = { "视频1", "视频2" };
        string[] addresses = { "1.mp4", "2.mp4" };
        string[] covers = { "1.png", "2.png" };

        Video video = new Video();
        video._description = descriptions[0];
        video._address = addresses[0];
        video._cover = covers[0];

        Video video2 = new Video();
        video2._description = descriptions[1];
        video2._address = addresses[1];
        video2._cover = covers[1];

        Video[] videos = { video, video2 };
        return videos[Random.Range(0,2)];
    }
}
