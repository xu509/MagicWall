using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace MagicWall
{
    public class FallDownDestoryBehavior : CutEffectDestoryBehavior
    {
        private MagicWallManager _manager;

        private CommonScene _commonScene;

        bool hasInit = false;
        float startTime;
        float totalTime = 13f;
        Action _onDestoryCompleted;

        List<FlockAgent> _flockAgents;

        private bool logoOriginalIsActive;
        private Sprite logoOriginalSprite;
        private Vector2 logoOriginalSize;
        private Color logoOriginalColor;
        private Vector2 customLogoSize = new Vector2(4341, 994);
        //掉落时间到一半时logo淡入，掉落完以后logo淡出（速度100时掉落完需要11s）
        private float fallTotalTime = 11f;
        private bool isShow = false;//logo 但淡入
        private bool isHide = false;//logo 淡出
        private float fadeTime = 3f;//淡入淡出时间

        public void Init(MagicWallManager manager,CommonScene commonScene, Action onDestoryCompleted)
        {
            _manager = manager;
            _commonScene = commonScene;
            _onDestoryCompleted = onDestoryCompleted;


            logoOriginalIsActive = _manager.BgLogo.gameObject.activeInHierarchy;
            logoOriginalSprite = _manager.BgLogo.GetComponent<Image>().sprite;
            logoOriginalSize = _manager.BgLogo.GetComponent<RectTransform>().sizeDelta;
            logoOriginalColor = _manager.BgLogo.GetComponent<Image>().color;
        }

        public void Run()
        {
            if (!hasInit)
            {
                // 此处初始化
                _manager.BgLogo.gameObject.SetActive(true);
                _manager.BgLogo.GetComponent<Image>().sprite = _manager.CustomLogoSprite;
                _manager.BgLogo.sizeDelta = customLogoSize;
                _manager.BgLogo.GetComponent<Image>().color = Color.white;
                _manager.BgLogo.GetComponent<Image>().CrossFadeAlpha(0, 0, true);

                fallTotalTime = _manager.cutEffectConfig.FallDownTotalTime;
                fadeTime = _manager.cutEffectConfig.FallDownLogoFadeTime;
                totalTime = fallTotalTime + fadeTime;

                isShow = false;
                isHide = false;

                startTime = Time.time;
                _commonScene.runDisplay = false;

                // 进行排序
                Sort();

                hasInit = true;
            }
            float time = Time.time - startTime;  // 当前已运行的时间;

            //var agents = _manager.agentManager.Agents;
            for (int i = 0; i < _flockAgents.Count; i++) {
                // 右边先，有快有慢

                var agent = _flockAgents[i];

                var aniTime = time - agent.fallDelayTime;

                if (aniTime < 0)
                {
                    continue;
                }
                else {

                    var anchorPosition = agent.OriVector2;
                    var speed = agent.fallSpeed;


                    float distance = 1f / 2f * speed * aniTime * aniTime;
                    var position = anchorPosition - new Vector2(0, distance);
                    agent.SetChangedPosition(position);

                }

            }
            if (time > fallTotalTime/2 && isShow == false)
            {
                _manager.BgLogo.GetComponent<Image>().CrossFadeAlpha(1, fadeTime, true);
                isShow = true;
            }
            if (time > fallTotalTime && isHide == false)
            {
                _manager.BgLogo.GetComponent<Image>().CrossFadeAlpha(0, fadeTime, true);
                isHide = true;
            }
            if (time >= totalTime) {
                // 效果结束时
                _manager.BgLogo.gameObject.SetActive(logoOriginalIsActive);
                _manager.BgLogo.GetComponent<Image>().sprite = logoOriginalSprite;
                _manager.BgLogo.sizeDelta = logoOriginalSize;
                _manager.BgLogo.GetComponent<Image>().color = logoOriginalColor;

                _manager.BgLogo.GetComponent<Image>().CrossFadeAlpha(1, 0, true);

                _onDestoryCompleted.Invoke();
                hasInit = false;
            }
        }

        void Sort() {
            _flockAgents = new List<FlockAgent>();

            for (int i = 0; i < _manager.agentManager.Agents.Count; i++) {
                _flockAgents.Add(_manager.agentManager.Agents[i]);
            }

            var refPoints = _manager.mainPanel.GetComponent<RectTransform>().transform.position
                + new Vector3(_manager.mainPanel.rect.width / 2, _manager.mainPanel.rect.height / 2,0);

            //Debug.Log(refPoints);

            _flockAgents.Sort((a, b) =>
            {
                var ad = Vector2.Distance(a.transform.position, refPoints);
                var bd = Vector2.Distance(b.transform.position, refPoints);
                return Mathf.RoundToInt(ad - bd);
            });

            float delaymin = 0f;
            float delaymax = _manager.cutEffectConfig.FallDownGapTime;

            for (int i = 0; i < _flockAgents.Count; i++) {
                float factor = (float)i / (float)(_flockAgents.Count - 1);

                // 设置下落的延迟时间
                FlockAgent flockAgent = _flockAgents[i];

                float nosieMax = delaymax;

                if (UnityEngine.Random.Range(0, 2) > 0) {
                    nosieMax = delaymax + _manager.cutEffectConfig.FallDownNoise;
                }

                flockAgent.fallDelayTime = Mathf.Lerp(delaymin, nosieMax, factor);

                // 这里是设置速度
                var speed = _manager.cutEffectConfig.FallDownSpeed;
                flockAgent.fallSpeed = speed;

            }


        }


    }
}