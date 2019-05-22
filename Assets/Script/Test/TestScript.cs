using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;



public class TestScript : MonoBehaviour
{
    [SerializeField] RectTransform videoContainer;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] RawImage videoContent;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = "file://E:/workspace/MagicWall/Assets/Files/env/video/1.mp4";

        StartCoroutine(PlayVideo());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlayVideo()
    {
        Debug.Log("Play Video !");

        videoPlayer.Prepare();

        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while (!videoPlayer.isPrepared)
        {
            Debug.Log("Wait prepared");
            yield return waitForSeconds;
            break;
        }

        Debug.Log("videoPlayer.isPrepared !");

        if (videoPlayer.isPrepared)
        {
            videoContent.texture = videoPlayer.texture;
        }

        videoPlayer.Play();
        //audioSource.Play();
    }
}
