using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


/// <summary>
/// 滑块的DoTween 动画管理
/// </summary>
public class FlockTweenerManager
{
    /// <summary>
    ///     卡片 —— 完全删除 —— 移动至后方动画
    /// </summary>
    public static string CardAgent_Destory_Second_DOAnchorPos3D = "CardAgentDestorySecondDOAnchorPos3D";
    public static string FlockAgent_DoRecoverAfterChoose_DOScale = "FlockAgentDoRecoverAfterChooseDOScale";
    public static string FlockAgent_DoRecoverAfterChoose_DOAnchorPos3D = "FlockAgentDoRecoverAfterChooseDOAnchorPos3D";
    public static string StarEffect_Starting_DOAnchorPos3DZ = "StarEffectStartingDOAnchorPos3DZ";
    public static string StarEffect_Starting_DOFade_AtStart = "StarEffectStartingDOFadeAtStart";
    public static string StarEffect_Starting_DOFade_AtEnd = "StarEffectStartingDOFadeAtEnd";

    private Dictionary<string, Tweener> _tweenerMap;

    public FlockTweenerManager() {
        _tweenerMap = new Dictionary<string, Tweener>();
    }

    public void Add(string key , Tweener tweener) {
        if (_tweenerMap.ContainsKey(key))
        {
            _tweenerMap[key].Kill();
            _tweenerMap[key] = tweener;
        }
        else {
            _tweenerMap.Add(key, tweener);
        }
    }




    public void Reset() {

        foreach (KeyValuePair<string, Tweener> kvp in _tweenerMap)
        {
            if (kvp.Value.IsActive()) {
                kvp.Value.Kill();
            }
        }
    }


}
