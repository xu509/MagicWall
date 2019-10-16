using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
///     kinect 卡片观察器
///     ref ： https://www.yuque.com/u314548/fc6a5l/dozp0e
/// </summary>

namespace MagicWall { 

    public class KinectCardObserver : MonoBehaviour
    {
        List<KinectAgent> kinectAgents;

        private MagicWallManager _manager;


        public void Init(MagicWallManager manager) {
            _manager = manager;
        }



        /// <summary>
        /// 观察中
        /// </summary>
        public void Observering() {
            kinectAgents = _manager.kinectManager.kinectAgents;

            for (int i = 0; i < kinectAgents.Count; i++)
            {
                var kinectAgent = kinectAgents[i];

                // 存在点开的卡片
                if (kinectAgent.refFlockAgent !=null && kinectAgent.refFlockAgent.GetCardAgent != null) {
                    var cardAgent = kinectAgent.refFlockAgent.GetCardAgent;
                    if(cardAgent._cardStatus == CardStatusEnum.TODESTORY)
                    {
                        var cardScale = cardAgent.GetComponent<RectTransform>().localScale;
                        kinectAgent.GetComponent<RectTransform>().localScale = cardScale;
                    }

                    if (cardAgent._cardStatus == CardStatusEnum.DESTORY)
                    {
                        kinectAgent.Close();
                    }

                    if (cardAgent._cardStatus == CardStatusEnum.MOVE)
                    {
                        //kinectAgent.Close();
                        kinectAgent.transform.position = cardAgent.transform.position;
                    }
                }
               
            }
        }

    }
}