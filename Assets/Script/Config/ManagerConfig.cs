﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerConfig : ScriptableObject
{
    [Range(100f, 10000f), Header("星空效果，最远距离")]
    public float StarEffectDistance;

    [Range(2f, 10f), Header("星空效果，动画时间")]
    public float StarEffectDistanceTime;



}