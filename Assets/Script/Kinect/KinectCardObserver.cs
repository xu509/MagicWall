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

                if (kinectAgent.refFlockAgent != null)
                {
                    //Debug.Log("Flock name :" + kinectAgent.refFlockAgent.gameObject.name + " Status - " + kinectAgent.refFlockAgent.flockStatus);
                }
                else {
                    if (kinectAgent.disableEffect)
                    {
                        kinectAgent.Close();
                    }
                }
               
                // 存在点开的卡片
                if (kinectAgent.refFlockAgent !=null && kinectAgent.refFlockAgent.GetCardAgent != null) {
                    var cardAgent = kinectAgent.refFlockAgent.GetCardAgent;
                    //Debug.Log(cardAgent.name + " status :" + cardAgent._cardStatus);

                    if(cardAgent._cardStatus == CardStatusEnum.TODESTORY)
                    {
                        var cardScale = cardAgent.GetComponent<RectTransform>().localScale;
                        kinectAgent.GetComponent<RectTransform>().localScale = cardScale;
                    }

                    if (cardAgent._cardStatus == CardStatusEnum.DESTORY)
                    {
                        //Debug.Log("kinect agent close!");
                        kinectAgent.Close();
                    }

                    if (cardAgent._cardStatus == CardStatusEnum.MOVE)
                    {
                        kinectAgent.transform.position = cardAgent.transform.position;
                    }
                }
               
            }
        }

    }
}