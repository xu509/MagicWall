using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Timer
{
    private string _title;
    private List<float> times;


    public Timer(string title) {
        _title = title;
    }

    public void Record() {
        if (times == null) {
            times = new List<float>();
        }
        float time = Time.deltaTime;
        times.Add(time);
    }

    public void Clear() {
        times = new List<float>();
    }

    public void Display() {
        string str = "Timer " + _title + " : ";
        for (int i = 0; i < times.Count; i++) {
            str += " | " + i + " - " + times[i];
        }
        //Debug.Log(str);
    }

}
