using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFpsWidget : MonoBehaviour
{

    public float f_UpdateInterval = 0.5F;

    private float f_LastInterval;

    private int i_Frames = 0;

    private float f_Fps;


    // Start is called before the first frame update
    void Start()
    {
        f_LastInterval = Time.realtimeSinceStartup;

        i_Frames = 0;
    }

    void OnGUI()
    {
        GUIStyle gStyle = new GUIStyle();
        gStyle.normal.textColor = Color.black;
        gStyle.fontSize = 50;

        float w = Screen.width - 250;

        GUI.Label(new Rect(w, 20, 200, 200), "FPS:" + f_Fps.ToString("f2"), gStyle);
    }

    void Update()
    {
        ++i_Frames;

        if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval)
        {
            f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);

            i_Frames = 0;

            f_LastInterval = Time.realtimeSinceStartup;
        }
    }

}
