using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

namespace MagicWall {
    public class SliceScrollPanelAgent : MonoBehaviour
    {
        [SerializeField] PanelLocationEnum _currentLocation;
        public PanelLocationEnum currentLocation { get { return _currentLocation; } }

        private SliceScrollAgent _sliceScrollAgent;
        private MagicWallManager _manager;

        private Vector2 LeftPosition;
        private Vector2 RightPosition;
        private Vector2 MiddlePosition;

        private float aniTime;
        private float aniFadeTime;

        private float MaxWidth;
        private float MaxHeight;

        void Awake() {
            _manager = GameObject.Find("MagicWall").GetComponent<MagicWallManager>();
            aniTime = 0.5f;
            aniFadeTime = 0.25f;

            if (_manager.screenTypeEnum == ScreenTypeEnum.Screen1080P)
            {
                LeftPosition = new Vector2(-195, 0);
                RightPosition = new Vector2(195, 0);
                MiddlePosition = new Vector2(0, 0);


                MaxWidth = 600f;
                MaxHeight = 600f;
            }
            else if (_manager.screenTypeEnum == ScreenTypeEnum.Screen720P) {
                // TODO
                LeftPosition = new Vector2(-195, 0);
                RightPosition = new Vector2(195, 0);
                MiddlePosition = new Vector2(0, 0);

                MaxWidth = 9f / 16f * 600f;
                MaxHeight = 9f / 16f * 600f;

            }
        }


        public void InitCompleted() {

        }


        public void Init(SliceScrollAgent sliceScrollAgent) {
            _sliceScrollAgent = sliceScrollAgent;

            //AdjustSize()
        }

        public void SetData(ScrollData scrollData) {

            var item = GetComponent<SliceScrollItemAgent>();

            var cover = scrollData.Cover;
            var sprite = SpriteResource.Instance.GetData(MagicWallManager.FileDir + cover);

            //AdjustSize(sprite);

            if (item == null)
            {
                // 创建prefab
                item = Instantiate(_sliceScrollAgent.scrollItemPrefab, transform);
            }
            item.Init(scrollData, _sliceScrollAgent.onScale);

        }

        public void GoOutLocation() {
            if (_manager.screenTypeEnum == ScreenTypeEnum.Screen1080P)
            {
                if (_currentLocation == PanelLocationEnum.Left)
                {
                    GetComponent<RectTransform>().DOAnchorPos(LeftPosition, 1f);
                    var item = GetComponentInChildren<SliceScrollItemAgent>();
                    item?.RecoverFrame();
                }

                if (_currentLocation == PanelLocationEnum.Right)
                {
                    GetComponent<RectTransform>().DOAnchorPos(RightPosition, 1f);
                    var item = GetComponentInChildren<SliceScrollItemAgent>();
                    item?.RecoverFrame();
                }

                if (_currentLocation == PanelLocationEnum.Middle)
                {
                    var item = GetComponentInChildren<SliceScrollItemAgent>();
                    item.SetAsMiddle(() =>
                    {
                    });
                }
            }
            else {
                // TODO 

                if (_currentLocation == PanelLocationEnum.Left)
                {
                    GetComponent<RectTransform>().DOAnchorPos(LeftPosition, 1f);
                    var item = GetComponentInChildren<SliceScrollItemAgent>();
                    item?.RecoverFrame();
                }

                if (_currentLocation == PanelLocationEnum.Right)
                {
                    GetComponent<RectTransform>().DOAnchorPos(RightPosition, 1f);
                    var item = GetComponentInChildren<SliceScrollItemAgent>();
                    item?.RecoverFrame();
                }

                if (_currentLocation == PanelLocationEnum.Middle)
                {
                    var item = GetComponentInChildren<SliceScrollItemAgent>();
                    item.SetAsMiddle(() =>
                    {
                    });
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
                    var item = GetComponentInChildren<SliceScrollItemAgent>();
                    if (item != null) {
                        Destroy(item.gameObject);
                    }
                    
                    // 右移后销毁
                }
                else if (_currentLocation == PanelLocationEnum.Middle)
                {
                    // 移动到左侧
                    var item = GetComponentInChildren<SliceScrollItemAgent>();
                    item.transform.SetParent(_sliceScrollAgent.scrollPanelLeft.transform, true);
                    item.GetComponent<RectTransform>().DOScale(1f, 0);
                    item.RecoverFrame();

                    item.GetComponent<RectTransform>().DOAnchorPos(MiddlePosition, aniTime)
                        .OnComplete(() =>
                        {

                            //AdjustSize();
                        });
                }
                else if (_currentLocation == PanelLocationEnum.Right)
                {
                    // 移动到中间
                    var item = GetComponentInChildren<SliceScrollItemAgent>();
                    item.transform.SetParent(_sliceScrollAgent.scrollPanelMiddle.transform, true);
                    item.GetComponent<RectTransform>().DOScale(1f, 0);
                    item.GetComponent<RectTransform>().DOAnchorPos(MiddlePosition, aniTime)
                        .OnComplete(() =>
                        {
                            item.SetAsMiddle(()=> {
                                updatePositionSuccess.Invoke();
                            });
                            //AdjustSize();
                        });
                }
                else if (_currentLocation == PanelLocationEnum.Prepare)
                {
                    // 移动到右侧
                    var item = GetComponentInChildren<SliceScrollItemAgent>();

                    if (item != null) {
                        item.transform.SetParent(_sliceScrollAgent.scrollPanelRight.transform, true);
                        item.RecoverFrame();

                        item.GetComponent<RectTransform>().DOAnchorPos(MiddlePosition, aniTime)
                            .OnComplete(() =>
                            {
                            //AdjustSize();
                        });
                    }
                   
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

                    var item = GetComponentInChildren<SliceScrollItemAgent>();
                    item.transform.SetParent(_sliceScrollAgent.scrollPanelMiddle.transform, true);
                    item.GetComponent<RectTransform>().DOAnchorPos(MiddlePosition, aniTime)
                        .OnComplete(() =>
                        {
                            item.SetAsMiddle(() => {
                                updatePositionSuccess.Invoke();
                            });
                        });

                    // 右移后销毁
                }
                else if (_currentLocation == PanelLocationEnum.Middle)
                {
                    // 移动到右侧
                    var item = GetComponentInChildren<SliceScrollItemAgent>();
                    item.transform.SetParent(_sliceScrollAgent.scrollPanelRight.transform, true);
                    item.GetComponent<RectTransform>().DOScale(1f, 0);
                    item.RecoverFrame();
                    item.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, aniTime)
                        .OnComplete(() =>
                        {
                        });
                }
                else if (_currentLocation == PanelLocationEnum.Right)
                {
                    var item = GetComponentInChildren<SliceScrollItemAgent>();
                    Destroy(item?.gameObject);

                }
                else if (_currentLocation == PanelLocationEnum.Prepare)
                {
                    // 移动到左侧
                    var item = GetComponentInChildren<SliceScrollItemAgent>();

                    if (item != null)
                    {
                        item.transform.SetParent(_sliceScrollAgent.scrollPanelLeft.transform, true);
                        item.RecoverFrame();
                        item.GetComponent<RectTransform>().DOAnchorPos(MiddlePosition, aniTime)
                            .OnComplete(() =>
                            {
                            });
                    }


                }
            }

        }


    }
}

