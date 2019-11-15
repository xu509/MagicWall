using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall {
    public class SliceScrollAgent : MonoBehaviour
    {
        private OperateCardDataSlide _data;
        public OperateCardDataSlide data { get { return _data; } }

        private MagicWallManager _manager;

        private int _index;  // 横向的index，从0开始

        private List<SliceScrollPanelAgent> _scrollPanelAgents;

        private bool _isPrepared = false; // 即可执行操作，在变动时不可进行操作

        Action<ScrollData, ScrollDirectionEnum> _onChanged;  // 修改中
        Action<string> _onScale;
        Action _onInitCompleted;
        Action<string, string, string> _onPlayVideo;
        public Action<string> onScale { get { return _onScale; } }


        [SerializeField] ScrollAreaAgent _scrollAreaAgent;
        [SerializeField] SliceScrollItemAgent _scrollItemPrefab;
        public SliceScrollItemAgent scrollItemPrefab { get { return _scrollItemPrefab; } }

        [SerializeField] SliceScrollPanelAgent _scrollPanelPrepare;
        public SliceScrollPanelAgent scrollPanelPrepare { get { return _scrollPanelPrepare; } }

        [SerializeField] SliceScrollPanelAgent _scrollPanelLeft;
        public SliceScrollPanelAgent scrollPanelLeft { get { return _scrollPanelLeft; } }

        [SerializeField] SliceScrollPanelAgent _scrollPanelRight;
        public SliceScrollPanelAgent scrollPanelRight { get { return _scrollPanelRight; } }

        [SerializeField] SliceScrollPanelAgent _scrollPanelMiddle;
        public SliceScrollPanelAgent scrollPanelMiddle { get { return _scrollPanelMiddle; } }

        public void Init(OperateCardDataSlide data,Action<ScrollData,ScrollDirectionEnum> onChanged,
            Action<string> onScale,Action<string,string,string> onPlayVideo,Action onInitCompleted) {
            _isPrepared = false;
            _data = data;
            _manager = GameObject.Find("MagicWall").GetComponent<MagicWallManager>();
            _onChanged = onChanged;
            _onScale = onScale;
            _onPlayVideo = onPlayVideo;
            _onInitCompleted = onInitCompleted;

            _scrollPanelAgents = new List<SliceScrollPanelAgent>();
            _scrollPanelAgents.Add(_scrollPanelLeft);
            _scrollPanelAgents.Add(_scrollPanelRight);
            _scrollPanelAgents.Add(_scrollPanelMiddle);
            _scrollPanelAgents.Add(_scrollPanelPrepare);
            for (int i = 0; i < _scrollPanelAgents.Count; i++) {
                _scrollPanelAgents[i].Init(this);
            }

            // 设置首图
            var firstData = data.ScrollData[0];
            _scrollPanelMiddle.SetData(firstData);

         
            // 初始化内容
            _index = 0;
        }


        /// <summary>
        /// 补全显示
        /// </summary>
        public void CompleteInit() {
            // 初始化对照nav list
            var datas = _data.ScrollData;

            _scrollPanelMiddle.GoOutLocation();

            // 添加上部与下部的内容
            if (datas.Count == 2)
            {
                // 添加上部
                _scrollPanelRight.SetData(datas[1]);
            }
            else if (datas.Count > 2) {
                // 添加左右两部分
                _scrollPanelRight.SetData(datas[1]);
                _scrollPanelRight.GoOutLocation();

                var count = datas.Count;
                count = count - 1;
                _scrollPanelLeft.SetData(datas[count]);
                _scrollPanelLeft.GoOutLocation();
            }

            _scrollAreaAgent.Init(OnRecognizeDirection);
            _isPrepared = true;
            _onInitCompleted.Invoke();
        }



        void OnRecognizeDirection(ScrollDirectionEnum scrollDirectionEnum) {
            if (_isPrepared) {
                _isPrepared = false;

                // 此处判断是否需要进行动画
                var needR = NeedResponse(scrollDirectionEnum);

                if (!needR) {
                    _isPrepared = true;
                    return;    
                }

                // 设置 prepare agent
                GeneratePrepareAgent(scrollDirectionEnum);

                //Debug.Log("scrollDirectionEnum : " + scrollDirectionEnum);
                for (int i = 0; i < _scrollPanelAgents.Count; i++)
                {
                    _scrollPanelAgents[i].UpdatePosition(scrollDirectionEnum,()=> {
                        _isPrepared = true;
                    });
                }

                //Debug.Log("当前的移动方向： " + scrollDirectionEnum + " 当前的导航位置： " + _index);

                HandleIndexAfterUpdate(scrollDirectionEnum);


                var currentData = _data.ScrollData[_index];

                _onChanged.Invoke(currentData, scrollDirectionEnum);

            }
        }


        /// <summary>
        ///     生产准备的agent
        /// </summary>
        /// <param name="scrollDirectionEnum"></param>
        void GeneratePrepareAgent(ScrollDirectionEnum scrollDirectionEnum) {
            if (scrollDirectionEnum == ScrollDirectionEnum.Left || scrollDirectionEnum == ScrollDirectionEnum.Right) {
                // 左右滑动、既是滑动更改nav

                if (_data.ScrollData.Count == 2)
                {
                    // TODO 

                }
                else if (_data.ScrollData.Count > 2) {
                    var preAgent = _scrollPanelPrepare.GetComponentInChildren<SliceScrollItemAgent>();
                    if (preAgent != null)
                    {
                        Destroy(preAgent.gameObject);
                    }
                    preAgent = Instantiate(_scrollItemPrefab, _scrollPanelPrepare.transform);

                    if (scrollDirectionEnum == ScrollDirectionEnum.Left)
                    {
                        int dataIndex = _index + 2;

                        if (dataIndex >= _data.ScrollData.Count)
                        {
                            int offset = dataIndex - _data.ScrollData.Count;
                            dataIndex = 0 + offset;
                        }
                        Debug.Log("_data.ScrollData[dataIndex]" + dataIndex + " - " + _data.ScrollData[dataIndex]);

                        preAgent.Init(_data.ScrollData[dataIndex], _onScale);
                    }
                    else if (scrollDirectionEnum == ScrollDirectionEnum.Right)
                    { 
                        int dataIndex = _index - 2;
                        if (dataIndex < 0)
                        {
                            int offset = Mathf.Abs(dataIndex);
                            dataIndex = _data.ScrollData.Count - offset;
                        }
                        preAgent.Init(_data.ScrollData[dataIndex], _onScale);
                    }
                }
            }
        }


        /// <summary>
        /// 判断是否需要相应动作
        /// </summary>
        /// <param name="scrollDirectionEnum"></param>
        /// <returns></returns>
        private bool NeedResponse(ScrollDirectionEnum scrollDirectionEnum) {
            bool r = false;
            if (scrollDirectionEnum == ScrollDirectionEnum.Left || scrollDirectionEnum == ScrollDirectionEnum.Right) {
                if (_data.ScrollData.Count > 2)
                {
                    r = true;
                }
                else if (_data.ScrollData.Count == 2) {
                    if (scrollDirectionEnum == ScrollDirectionEnum.Left)
                    {
                        if (_index == 1)
                        {
                            r = false;
                        }
                        else
                        {
                            r = true;
                        }
                    }
                    else {
                        if (_index == 0)
                        {
                            r = false;
                        }
                        else
                        {
                            r = true;
                        }
                    }
                }
                else
                {
                    r = false;
                }
            } 
            return r;
        }

        private void HandleIndexAfterUpdate(ScrollDirectionEnum scrollDirectionEnum) {
            // 处理坐标
            if (scrollDirectionEnum == ScrollDirectionEnum.Left)
            {
                _index = _index + 1;
                if (_index == _data.ScrollData.Count)
                {
                    _index = 0;
                }
                //_index = 0;
            }
            else if (scrollDirectionEnum == ScrollDirectionEnum.Right)
            {
                _index = _index - 1;
                if (_index < 0)
                {
                    _index = _data.ScrollData.Count - 1;
                }
                //_index = 0;
            }


            //Debug.Log("修改后的导航位置： " + _index + " INDEX: " + _index);
        }

        /// <summary>
        ///     当点击中心
        /// </summary>
        public void OnClickMid() {
            var data = _data.ScrollData[_index];

            if (data.Type == 1) {
                _onPlayVideo.Invoke(data.Src, data.Description, data.Cover);

            }
        }

        /// <summary>
        /// 装载内容
        /// </summary>
        void Refresh() {
            
        }

        public Vector2 GetCurrentImage() {
            var item = _scrollPanelMiddle.GetComponentInChildren<SliceScrollItemAgent>();
            return item.GetImageSize();
        }

    }

}
