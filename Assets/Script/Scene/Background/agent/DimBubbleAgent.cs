using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 模糊的背景球 Agent
/// </summary>
public class DimBubbleAgent : BubbleAgent
{
    public override float GetMoveFactor(float minFactor, float maxFactor)
    {
        float range =  _scaleFactorMax - _scaleFactorMin;
        float k = (_scaleFactor - _scaleFactorMin) / range;

        float factor = Mathf.Lerp(minFactor, maxFactor, k);

        return factor;
    }

    public override void Raise(float moveFactor)
    {
        transform.Translate(new Vector3(0, Time.deltaTime * moveFactor, 0));
    }
}
