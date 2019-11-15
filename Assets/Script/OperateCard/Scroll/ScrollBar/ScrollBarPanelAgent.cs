using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace MagicWall {
    public class ScrollBarPanelAgent : MonoBehaviour
    {
        [SerializeField] PanelLocationEnum _currentLocation;

        private ScrollBarAgent _scrollBarAgent;
        private MagicWallManager _manager;

        private float aniTime;
        private float aniFadeTime;

        void Awake() {
            aniTime = 0.5f;
            aniFadeTime = 0.25f;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init(ScrollBarAgent scrollBarAgent)
        {
            _scrollBarAgent = scrollBarAgent;
        }

        public void SetData(CrossCardNavType navType)
        {

            var item = GetComponent<ScrollBarItemAgent>();
            if (item == null)
            {
                // 创建prefab
                item = Instantiate(_scrollBarAgent.scrollBarItemPrefab, transform);
            }
            item.Init(navType);
        }

        public void UpdatePosition(ScrollDirectionEnum scrollDirectionEnum, Action updatePositionSuccess)
        {
            if (scrollDirectionEnum == ScrollDirectionEnum.Left) {

                if (_currentLocation == PanelLocationEnum.Left)
                {
                    var item = GetComponentInChildren<ScrollBarItemAgent>();
                    Destroy(item.gameObject);
                    // 右移后销毁
                }
                else if (_currentLocation == PanelLocationEnum.Middle)
                {
                    // 移动到左侧
                    var item = GetComponentInChildren<ScrollBarItemAgent>();
                    item.transform.SetParent(_scrollBarAgent.leftBarPanelAgent.transform, true);
                    item.GetComponent<RectTransform>().DOScale(1f, aniTime);
                    item.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, aniTime)
                        .OnComplete(() =>
                        {
                            updatePositionSuccess.Invoke();
                        });
                }
                else if (_currentLocation == PanelLocationEnum.Right)
                {
                    // 移动到中间
                    var item = GetComponentInChildren<ScrollBarItemAgent>();
                    item.transform.SetParent(_scrollBarAgent.middleBarPanelAgent.transform, true);
                    item.GetComponent<RectTransform>().DOScale(1f, aniTime);
                    item.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, aniTime)
                        .OnComplete(() =>
                        {
                        });
                }
                else if (_currentLocation == PanelLocationEnum.Prepare)
                {
                    // 移动到右侧
                    var item = GetComponentInChildren<ScrollBarItemAgent>();
                    item.transform.SetParent(_scrollBarAgent.rightBarPanelAgent.transform, true);

                    item.GetComponent<RectTransform>().DOScale(1f, aniTime);
                    item.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, aniTime)
                        .OnComplete(() =>
                        {
                        });
                }

            }
            // 右滑
            else if (scrollDirectionEnum == ScrollDirectionEnum.Right)
            {

                // 向右移动至中间
                if (_currentLocation == PanelLocationEnum.Left)
                {
                    // 移动到中间
                    Debug.Log("右侧移动至中间");

                    var item = GetComponentInChildren<ScrollBarItemAgent>();
                    item.transform.SetParent(_scrollBarAgent.middleBarPanelAgent.transform, true);
                    item.GetComponent<RectTransform>().DOScale(1f, aniTime);
                    item.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, aniTime)
                        .OnComplete(() =>
                        {
                        });

                    // 右移后销毁
                }
                else if (_currentLocation == PanelLocationEnum.Middle)
                {
                    // 移动到右侧
                    var item = GetComponentInChildren<ScrollBarItemAgent>();
                    item.transform.SetParent(_scrollBarAgent.rightBarPanelAgent.transform, true);

                    item.GetComponent<RectTransform>().DOScale(1f, aniTime);
                    item.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, aniTime)
                        .OnComplete(() =>
                        {
                            updatePositionSuccess.Invoke();
                        });
                }
                else if (_currentLocation == PanelLocationEnum.Right)
                {
                    var item = GetComponentInChildren<ScrollBarItemAgent>();
                    Destroy(item.gameObject);

                }
                else if (_currentLocation == PanelLocationEnum.Prepare)
                {
                    // 移动到左侧
                    var item = GetComponentInChildren<ScrollBarItemAgent>();
                    item.transform.SetParent(_scrollBarAgent.leftBarPanelAgent.transform, true);

                    item.GetComponent<RectTransform>().DOScale(1f, aniTime);
                    item.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, aniTime)
                        .OnComplete(() =>
                        {
                        });
                }
            }



        }
    }

}
