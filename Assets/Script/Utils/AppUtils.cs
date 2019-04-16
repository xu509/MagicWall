using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppUtils : MonoBehaviour
{
  
    public static float ConvertToFloat(string str) {
        float r = 0f;

        if (!float.TryParse(str, out r)) {
            r = 0f;
        }
        return r;
    }

}
