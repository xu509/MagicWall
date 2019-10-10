using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace MagicWall
{
    public static class BackgroundInvoker<T> where T : BubbleAgent
    {
        public static T CreateBubble(T t, RectTransform container)
        {
            T agent = GameObject.Instantiate(t, container);
            return agent;
        }


    }
}