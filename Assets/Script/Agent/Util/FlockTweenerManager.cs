using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace MagicWall
{
    /// <summary>
    /// 滑块的DoTween 动画管理
    /// </summary>
    public class FlockTweenerManager
    {
        /// <summary>
        ///     卡片 —— 完全删除 —— 移动至后方动画
        /// </summary>
        public static string CardAgent_Destory_Second_DOAnchorPos3D = "CardAgentDestorySecondDOAnchorPos3D";

        public static string CardAgent_Destory_Second_DOScale_IsOrigin = "CardAgentDestorySecondDOScaleIsOrigin";
        public static string CardAgent_Destory_Second_DOAnchorPos3D_IsOrigin = "CardAgentDestorySecondDOAnchorPos3DIsOrigin";

        public static string FlockAgent_DoRecoverAfterChoose_DOScale = "FlockAgentDoRecoverAfterChooseDOScale";
        public static string FlockAgent_DoRecoverAfterChoose_DOAnchorPos3D = "FlockAgentDoRecoverAfterChooseDOAnchorPos3D";
        public static string StarEffect_Starting_DOAnchorPos3DZ = "StarEffectStartingDOAnchorPos3DZ";
        public static string StarEffect_Starting_DOFade_AtStart = "StarEffectStartingDOFadeAtStart";
        public static string StarEffect_Starting_DOFade_AtEnd = "StarEffectStartingDOFadeAtEnd";

        /// <summary>
        ///  体感 点击相关
        /// </summary>
        public static string Kinnect_Choose_Move = "KinnectChooseMove";
        public static string Kinnect_Choose_Scale = "KinnectChooseScale";
        public static string Kinnect_Close = "KinnectClose";
        public static string Kinnect_Close_Cancel = "KinnectCloseCancel";

        /// <summary>
        ///  操作卡片 相关
        /// </summary>
        public static string Card_GoToFront_Move = "CardGoToFrontMove";
        public static string Card_GoToFront_Scale = "CardGoToFrontScale";



        private Dictionary<string, Tweener> _tweenerMap;

        public FlockTweenerManager()
        {
            _tweenerMap = new Dictionary<string, Tweener>();
        }

        public void Add(string key, Tweener tweener)
        {
            if (_tweenerMap.ContainsKey(key))
            {
                _tweenerMap[key].Kill();
                _tweenerMap[key] = tweener;
            }
            else
            {
                _tweenerMap.Add(key, tweener);
            }
        }

        public Tweener Get(string key)
        {
            if (_tweenerMap.ContainsKey(key))
            {
                return _tweenerMap[key];
            }
            else
            {
                return null;
            }
        }



        public void Reset()
        {

            foreach (KeyValuePair<string, Tweener> kvp in _tweenerMap)
            {
                if (kvp.Value.IsActive())
                {
                    Debug.Log(kvp.Key + " kill!");

                    kvp.Value.Kill();
                }
            }
        }


    }
}