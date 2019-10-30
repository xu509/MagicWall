using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace MagicWall {
    public class ScrollPanelAgent : MonoBehaviour
    {
        [SerializeField] PanelLocationEnum _currentLocation;

        private MagicWallManager _manager;

        private static Vector2 LeftPosition = new Vector2(-236,0);
        private static Vector2 RightPosition = new Vector2(236,0);


        void Awake() {
            _manager = GameObject.Find("MagicWall").GetComponent<MagicWallManager>();
        }


        public void InitCompleted() {

        }

        public void SetData(CrossScrollAgent crossScrollAgent, ScrollData scrollData) {

            var item = GetComponent<ScrollItemAgent>();
            if (item == null)
            {
                // 创建prefab
                item = Instantiate(crossScrollAgent.scrollItemPrefab, transform);
            }
            item.Init(scrollData);
        }

        public void GoOutLocation() {
            if (_manager.screenTypeEnum == ScreenTypeEnum.Screen1080P) {
                if (_currentLocation == PanelLocationEnum.Left)
                {
                    GetComponent<RectTransform>().DOAnchorPos(LeftPosition, 1f);
                }

                if (_currentLocation == PanelLocationEnum.Right)
                {
                    GetComponent<RectTransform>().DOAnchorPos(RightPosition, 1f);
                }
            }
        }

    }
}

