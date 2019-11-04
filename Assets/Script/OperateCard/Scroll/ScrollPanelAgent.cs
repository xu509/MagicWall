using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

namespace MagicWall {
    public class ScrollPanelAgent : MonoBehaviour
    {
        [SerializeField] PanelLocationEnum _currentLocation;
        public PanelLocationEnum currentLocation { get { return _currentLocation; } }

        private CrossScrollAgent _crossScrollAgent;
        private MagicWallManager _manager;

        private Vector2 LeftPosition;
        private Vector2 RightPosition;
        private Vector2 TopPosition;
        private Vector2 BottomPosition;
        private Vector2 MiddlePosition;

        private float aniTime;
        private float aniFadeTime;


        void Awake() {
            _manager = GameObject.Find("MagicWall").GetComponent<MagicWallManager>();
            aniTime = 0.5f;
            aniFadeTime = 0.25f;

            if (_manager.screenTypeEnum == ScreenTypeEnum.Screen1080P) {
                LeftPosition = new Vector2(-195, 0);
                RightPosition = new Vector2(195, 0);
                MiddlePosition = new Vector2(0, 0);
                TopPosition = new Vector2(0, 130);
                BottomPosition = new Vector2(0, -130);
            }

            if (_currentLocation == PanelLocationEnum.Top)
            {
                GetComponent<RectTransform>().anchoredPosition = TopPosition;
            }
            else if (_currentLocation == PanelLocationEnum.Bottom) {
                GetComponent<RectTransform>().anchoredPosition = BottomPosition;

            }


        }


        public void InitCompleted() {

        }


        public void Init(CrossScrollAgent crossScrollAgent) {
            _crossScrollAgent = crossScrollAgent;
        }

        public void SetData(ScrollData scrollData) {

            var item = GetComponent<ScrollItemAgent>();
            if (item == null)
            {
                // 创建prefab
                item = Instantiate(_crossScrollAgent.scrollItemPrefab, transform);
            }
            item.Init(scrollData, _crossScrollAgent.onScale);
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

        /// <summary>
        ///   更新位置
        ///   TODO : 当 nav 只有两个时,修改
        /// </summary>
        /// <param name="scrollDirectionEnum"></param>
        public void UpdatePosition(ScrollDirectionEnum scrollDirectionEnum,Action updatePositionSuccess)
        {
            // 左划
            if (scrollDirectionEnum == ScrollDirectionEnum.Left)
            {
                // 左划，更换nav
                if (_currentLocation == PanelLocationEnum.Left)
                {
                    var item = GetComponentInChildren<ScrollItemAgent>();
                    Destroy(item.gameObject);
                    // 右移后销毁
                }
                else if (_currentLocation == PanelLocationEnum.Middle)
                {
                    // 移动到左侧
                    var item = GetComponentInChildren<ScrollItemAgent>();
                    item.transform.SetParent(_crossScrollAgent.scrollPanelLeft.transform, true);
                    item.GetComponent<RectTransform>().DOScale(1f, aniTime);
                    item.GetComponent<RectTransform>().DOAnchorPos(MiddlePosition, aniTime)
                        .OnComplete(() =>
                        {
                            updatePositionSuccess.Invoke();
                        });
                }
                else if (_currentLocation == PanelLocationEnum.Right)
                {
                    // 移动到中间
                    var item = GetComponentInChildren<ScrollItemAgent>();
                    item.transform.SetParent(_crossScrollAgent.scrollPanelMiddle.transform, true);
                    item.GetComponent<RectTransform>().DOScale(1f, aniTime);
                    item.GetComponent<RectTransform>().DOAnchorPos(MiddlePosition, aniTime)
                        .OnComplete(() =>
                        {
                        });
                }
                else if (_currentLocation == PanelLocationEnum.Prepare)
                {
                    // 移动到右侧
                    var item = GetComponentInChildren<ScrollItemAgent>();
                    item.transform.SetParent(_crossScrollAgent.scrollPanelRight.transform, true);

                    item.GetComponent<RectTransform>().DOScale(1f, aniTime);
                    item.GetComponent<RectTransform>().DOAnchorPos(MiddlePosition, aniTime)
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

                    var item = GetComponentInChildren<ScrollItemAgent>();
                    item.transform.SetParent(_crossScrollAgent.scrollPanelMiddle.transform, true);
                    item.GetComponent<RectTransform>().DOScale(1f, aniTime);
                    item.GetComponent<RectTransform>().DOAnchorPos(MiddlePosition, aniTime)
                        .OnComplete(() =>
                        {
                        });

                    // 右移后销毁
                }
                else if (_currentLocation == PanelLocationEnum.Middle)
                {
                    // 移动到右侧
                    var item = GetComponentInChildren<ScrollItemAgent>();
                    item.transform.SetParent(_crossScrollAgent.scrollPanelRight.transform, true);
                    item.GetComponent<RectTransform>().DOScale(1f, aniTime);
                    item.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, aniTime)
                        .OnComplete(() =>
                        {
                            updatePositionSuccess.Invoke();
                        });
                }
                else if (_currentLocation == PanelLocationEnum.Right)
                {
                    var item = GetComponentInChildren<ScrollItemAgent>();
                    Destroy(item.gameObject);

                }
                else if (_currentLocation == PanelLocationEnum.Prepare)
                {
                    // 移动到左侧
                    var item = GetComponentInChildren<ScrollItemAgent>();
                    item.transform.SetParent(_crossScrollAgent.scrollPanelLeft.transform, true);

                    item.GetComponent<RectTransform>().DOScale(1f, aniTime);
                    item.GetComponent<RectTransform>().DOAnchorPos(MiddlePosition, aniTime)
                        .OnComplete(() =>
                        {
                        });
                }
            }

            // 上滑
            else if (scrollDirectionEnum == ScrollDirectionEnum.Top)
            {
                if (_currentLocation == PanelLocationEnum.Bottom)
                {
                    // 移动到中间

                    var item = GetComponentInChildren<ScrollItemAgent>();

                    if (item != null)
                    {
                        item.transform.SetParent(_crossScrollAgent.scrollPanelMiddle.transform, true);
                        item.GetComponent<RectTransform>().DOScale(1f, aniTime);
                        item.GetComponent<RectTransform>().DOAnchorPos(MiddlePosition, aniTime)
                            .OnComplete(() =>
                            {
                            });
                    }
                }
                else if (_currentLocation == PanelLocationEnum.Middle)
                {
                    var item = GetComponentInChildren<ScrollItemAgent>();

                    if (item != null)
                    {
                        item.transform.SetParent(_crossScrollAgent.scrollPanelTop.transform, true);
                        item.GetComponent<RectTransform>().DOScale(1f, aniTime);
                        item.GetComponent<RectTransform>().DOAnchorPos(MiddlePosition, aniTime)
                            .OnComplete(() =>
                            {
                                updatePositionSuccess.Invoke();
                            });
                    }
                }
                else if (_currentLocation == PanelLocationEnum.Top)
                {
                    var item = GetComponentInChildren<ScrollItemAgent>();
                    if (item != null) {
                        Destroy(item.gameObject);
                    }
                }
                else if (_currentLocation == PanelLocationEnum.Prepare)
                {
                    // 移动到中间

                    var item = GetComponentInChildren<ScrollItemAgent>();

                    if (item != null)
                    {
                        item.transform.SetParent(_crossScrollAgent.scrollPanelBottom.transform, true);
                        item.GetComponent<RectTransform>().DOScale(1f, aniTime);
                        item.GetComponent<RectTransform>().DOAnchorPos(MiddlePosition, aniTime)
                            .OnComplete(() =>
                            {

                            });
                    }
                }
            }

            // 下滑
            else if (scrollDirectionEnum == ScrollDirectionEnum.Bottom) {
                if (_currentLocation == PanelLocationEnum.Top)
                {
                    var item = GetComponentInChildren<ScrollItemAgent>();

                    if (item != null)
                    {
                        item.transform.SetParent(_crossScrollAgent.scrollPanelMiddle.transform, true);
                        item.GetComponent<RectTransform>().DOScale(1f, aniTime);
                        item.GetComponent<RectTransform>().DOAnchorPos(MiddlePosition, aniTime)
                            .OnComplete(() =>
                            {
                            });
                    }
                }
                else if (_currentLocation == PanelLocationEnum.Prepare)
                {
                    var item = GetComponentInChildren<ScrollItemAgent>();

                    if (item != null)
                    {
                        item.transform.SetParent(_crossScrollAgent.scrollPanelTop.transform, true);
                        item.GetComponent<RectTransform>().DOScale(1f, aniTime);
                        item.GetComponent<RectTransform>().DOAnchorPos(MiddlePosition, aniTime)
                            .OnComplete(() =>
                            {
                                updatePositionSuccess.Invoke();
                            });
                    }
                }
                else if (_currentLocation == PanelLocationEnum.Bottom)
                {
                    var item = GetComponentInChildren<ScrollItemAgent>();
                    if (item != null) {
                        Destroy(item.gameObject);
                    }
                }
                else if (_currentLocation == PanelLocationEnum.Middle)
                {
                    var item = GetComponentInChildren<ScrollItemAgent>();

                    if (item != null)
                    {
                        item.transform.SetParent(_crossScrollAgent.scrollPanelBottom.transform, true);
                        item.GetComponent<RectTransform>().DOScale(1f, aniTime);
                        item.GetComponent<RectTransform>().DOAnchorPos(MiddlePosition, aniTime)
                            .OnComplete(() =>
                            {
                                updatePositionSuccess.Invoke();
                            });
                    }
                }
            }
        }



        /// <summary>
        ///     当前的navindex
        /// </summary>
        /// <param name="navindex"></param>
        public void UpdateUpDownContent(int navindex, List<CrossCardNavType> navList) {
            // 隐藏
            //Debug.Log("Do Fade");

            GetComponent<CanvasGroup>().DOFade(0f, aniFadeTime)
                .OnComplete(()=> {

                    var navE = navList[navindex];
                    var datas = _crossScrollAgent.data.ScrollDic[navE];

                    //Debug.Log("当前的nav ： " + navE);
                    //Debug.Log("当前的datas ： " + datas.Count);


                    if (datas.Count == 2)
                    {
                        if (_currentLocation == PanelLocationEnum.Bottom)
                        {
                            // 获取数据
                            var index = datas.Count - 1;
                            var item = GetComponentInChildren<ScrollItemAgent>();
                            if (item != null)
                            {
                                Destroy(item.gameObject);
                            }
                            item = Instantiate(_crossScrollAgent.scrollItemPrefab, transform);
                            item.Init(datas[index], _crossScrollAgent.onScale);
                            GetComponent<CanvasGroup>().DOFade(0.2f, aniFadeTime);
                        }
                        else if (_currentLocation == PanelLocationEnum.Top) {
                            GetComponent<CanvasGroup>().DOFade(0.2f, aniFadeTime);
                            var item = GetComponentInChildren<ScrollItemAgent>();
                            if (item != null)
                            {
                                Destroy(item.gameObject);
                            }
                        }

                    }
                    else if (datas.Count > 2)
                    {
                        if (_currentLocation == PanelLocationEnum.Top)
                        {
                            // 获取数据
                            var index = datas.Count - 1;
                            var item = GetComponentInChildren<ScrollItemAgent>();
                            if (item != null) {
                                Destroy(item.gameObject);
                            }
                            item = Instantiate(_crossScrollAgent.scrollItemPrefab,transform);
                            item.Init(datas[index], _crossScrollAgent.onScale);
                            GetComponent<CanvasGroup>().DOFade(0.2f, aniFadeTime);
                        }
                        else if (_currentLocation == PanelLocationEnum.Bottom)
                        {
                            var index = 1;
                            var item = GetComponentInChildren<ScrollItemAgent>();
                            if (item != null)
                            {
                                Destroy(item.gameObject);
                            }
                            item = Instantiate(_crossScrollAgent.scrollItemPrefab, transform);
                            item.Init(datas[index], _crossScrollAgent.onScale);
                            GetComponent<CanvasGroup>().DOFade(0.2f, aniFadeTime);

                        }
                    }
                });







            //_crossScrollAgent.data.ScrollDic
        }


    }
}

