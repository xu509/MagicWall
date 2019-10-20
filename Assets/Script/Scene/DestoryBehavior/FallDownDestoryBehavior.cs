using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall
{
    public class FallDownDestoryBehavior : CutEffectDestoryBehavior
    {
        private MagicWallManager _manager;

        private CommonScene _commonScene;

        bool hasInit = false;
        float startTime;
        float totalTime = 10f;
        Action _onDestoryCompleted;


        public void Init(MagicWallManager manager,CommonScene commonScene, Action onDestoryCompleted)
        {
            _manager = manager;
            _commonScene = commonScene;
            _onDestoryCompleted = onDestoryCompleted;
        }

        public void Run()
        {
            if (!hasInit)
            {
                startTime = Time.time;
                _commonScene.runDisplay = false;
                hasInit = true;
            }
            float time = Time.time - startTime;  // 当前已运行的时间;

            var agents = _manager.agentManager.Agents;
            for (int i = 0; i < agents.Count; i++) {
                var agent = agents[i];
                if (agent.flockStatus == FlockStatusEnum.NORMAL)
                {
                    if (agent.fallDwonStartTime == 0f)
                    {
                        agent.fallDwonStartTime = Time.time;
                    }

                    var anchorPosition = agent.GetComponent<RectTransform>().anchoredPosition;
                    float distance = 1f / 2f * _manager.cutEffectConfig.FallDownFactor * time * time;
                    var position = anchorPosition - new Vector2(0, distance);                 
                    agent.SetChangedPosition(position);
                }
                else {

                    Debug.Log(agent.gameObject.name);
                } 
            }





            if (time >= totalTime) {
                _onDestoryCompleted.Invoke();
                hasInit = false;
            }

        }
    }
}