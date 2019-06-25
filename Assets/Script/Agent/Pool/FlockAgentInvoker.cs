using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public static class FlockAgentInvoker<T> where T : FlockAgent
{
    public static T CreateAgent(T t, RectTransform container) {
        T agent = GameObject.Instantiate(t, container);
        return agent;
    }
   

}
