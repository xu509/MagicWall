using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall {
    public class CrossScrollAgent : MonoBehaviour
    {
        private OperateCardDataCross _data;
        public OperateCardDataCross data { get { return _data; } }

        private MagicWallManager _manager;

        private int _index;  // 横向的index，从0开始
        private CrossCardNavType _currentNavType;   //当前的nav
        private int _navIndex;

        private List<CrossCardNavType> _navList;
        private List<ScrollPanelAgent> _scrollPanelAgents;

        private bool _isPrepared = false; // 即可执行操作，在变动时不可进行操作

        Action<ScrollData, CrossCardNavType, ScrollDirectionEnum> _onChanged;  // 修改中
        Action<string> _onScale;  
        public Action<string> onScale { get { return _onScale; } }


        [SerializeField] ScrollAreaAgent _scrollAreaAgent;
        [SerializeField] ScrollItemAgent _scrollItemPrefab;
        public ScrollItemAgent scrollItemPrefab { get { return _scrollItemPrefab; } }

        [SerializeField] RectTransform _middlePanel;

        [SerializeField] ScrollPanelAgent _scrollPanelPrepare;
        public ScrollPanelAgent scrollPanelPrepare { get { return _scrollPanelPrepare; } }
        [SerializeField] ScrollPanelAgent _scrollPanelTop;
        public ScrollPanelAgent scrollPanelTop { get { return _scrollPanelTop; } }

        [SerializeField] ScrollPanelAgent _scrollPanelBottom;
        public ScrollPanelAgent scrollPanelBottom { get { return _scrollPanelBottom; } }

        [SerializeField] ScrollPanelAgent _scrollPanelLeft;
        public ScrollPanelAgent scrollPanelLeft { get { return _scrollPanelLeft; } }

        [SerializeField] ScrollPanelAgent _scrollPanelRight;
        public ScrollPanelAgent scrollPanelRight { get { return _scrollPanelRight; } }

        [SerializeField] ScrollPanelAgent _scrollPanelMiddle;
        public ScrollPanelAgent scrollPanelMiddle { get { return _scrollPanelMiddle; } }

        public void Init(OperateCardDataCross data,Action<ScrollData, CrossCardNavType,ScrollDirectionEnum> onChanged,Action<string> onScale) {
            _isPrepared = false;
            _data = data;
            _manager = GameObject.Find("MagicWall").GetComponent<MagicWallManager>();
            _onChanged = onChanged;
            _onScale = onScale;

            _scrollPanelAgents = new List<ScrollPanelAgent>();
            _scrollPanelAgents.Add(_scrollPanelTop);
            _scrollPanelAgents.Add(_scrollPanelBottom);
            _scrollPanelAgents.Add(_scrollPanelLeft);
            _scrollPanelAgents.Add(_scrollPanelRight);
            _scrollPanelAgents.Add(_scrollPanelMiddle);
            _scrollPanelAgents.Add(_scrollPanelPrepare);
            for (int i = 0; i < _scrollPanelAgents.Count; i++) {
                _scrollPanelAgents[i].Init(this);
            }

            // 设置首图
            var firstData = data.ScrollDic[CrossCardNavType.Index];
            Debug.Log("firstData[0] : " + firstData[0].ToString());

            _scrollPanelMiddle.SetData(firstData[0]);

            //_middlePanel.GetComponentInChildren
            //var item = _middlePanel.GetComponent<ScrollItemAgent>();
            //if (item == null) {
            //    // 创建prefab
            //    item = Instantiate(_scrollItemPrefab, _middlePanel);
            //}
            //item.Init(firstData[0]);

            // 初始化内容
            _currentNavType = CrossCardNavType.Index;
            _index = 0;
            _navIndex = 0;
        }


        /// <summary>
        /// 补全显示
        /// </summary>
        public void CompleteInit() {
            // 初始化对照nav list
            _navList = new List<CrossCardNavType>();
            _navList.Add(CrossCardNavType.Index);

            if (_data.ScrollDic.ContainsKey(CrossCardNavType.CataLog)) {
                _navList.Add(CrossCardNavType.CataLog);
            }

            if (_data.ScrollDic.ContainsKey(CrossCardNavType.Product))
            {
                _navList.Add(CrossCardNavType.Product);
            }

            if (_data.ScrollDic.ContainsKey(CrossCardNavType.Activity))
            {
                _navList.Add(CrossCardNavType.Activity);
            }

            if (_data.ScrollDic.ContainsKey(CrossCardNavType.Video))
            {
                _navList.Add(CrossCardNavType.Video);
            }

            // 添加上部与下部的内容
            if (_navList.Count == 2)
            {
                // 添加上部
                CrossCardNavType nav = _navList[1];
                _scrollPanelTop.SetData(_data.ScrollDic[nav][0]);
            }
            else if (_navList.Count > 2) {
                // 添加左右两部分
                CrossCardNavType nav = _navList[1];
                _scrollPanelRight.SetData( _data.ScrollDic[nav][0]);
                _scrollPanelRight.GoOutLocation();

                var count = _navList.Count;
                count = count - 1;
                CrossCardNavType nav2 = _navList[count];
                _scrollPanelLeft.SetData( _data.ScrollDic[nav2][0]);
                _scrollPanelLeft.GoOutLocation();
            }


            _scrollAreaAgent.Init(OnRecognizeDirection);
            _isPrepared = true;
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

                //Debug.Log("当前的移动方向： " + scrollDirectionEnum + " 当前的导航位置： " + _navIndex);

                HandleIndexAfterUpdate(scrollDirectionEnum);


                var navType = _navList[_navIndex];
                var scrollData = _data.ScrollDic[navType][_index];
                _onChanged.Invoke(scrollData,navType, scrollDirectionEnum);
                Debug.Log("当前的NAV: " + _navList[_navIndex]);
            }
        }


        /// <summary>
        ///     生产准备的agent
        /// </summary>
        /// <param name="scrollDirectionEnum"></param>
        void GeneratePrepareAgent(ScrollDirectionEnum scrollDirectionEnum) {
            if (scrollDirectionEnum == ScrollDirectionEnum.Left || scrollDirectionEnum == ScrollDirectionEnum.Right) {
                // 左右滑动、既是滑动更改nav
                if (_navList.Count == 2)
                {
                    // TODO 

                    //var preAgent = _scrollPanelPrepare.GetComponentInChildren<ScrollItemAgent>();
                    //if (preAgent != null)
                    //{
                    //    Destroy(preAgent.gameObject);
                    //}
                    //preAgent = Instantiate(_scrollItemPrefab, _scrollPanelPrepare.transform);

                    //if (scrollDirectionEnum == ScrollDirectionEnum.Left)
                    //{
                    //    int toIndex = _navIndex - 1;
                    //    if (toIndex < 0)
                    //    {
                    //        toIndex = _navList.Count - 2;
                    //    }

                    //    Debug.Log("Prepare - " + _navList[toIndex]);

                    //    preAgent.Init(_data.ScrollDic[_navList[toIndex]][0]);
                    //}
                    //else
                    //{
                    //    int toIndex = _navIndex + 2;
                    //    if (toIndex >= _navList.Count)
                    //    {
                    //        toIndex = 0;
                    //    }
                    //    Debug.Log("Prepare - " + _navList[toIndex]);
                    //    preAgent.Init(_data.ScrollDic[_navList[toIndex]][0]);
                    //}
                }
                else if (_navList.Count > 2) {
                    var preAgent = _scrollPanelPrepare.GetComponentInChildren<ScrollItemAgent>();
                    if (preAgent != null)
                    {
                        Destroy(preAgent.gameObject);
                    }
                    preAgent = Instantiate(_scrollItemPrefab, _scrollPanelPrepare.transform);

                    if (scrollDirectionEnum == ScrollDirectionEnum.Left)
                    {
                        int dataIndex = _navIndex + 2;

                        if (dataIndex >= _navList.Count)
                        {
                            int offset = dataIndex - _navList.Count;
                            dataIndex = 0 + offset;
                        }
                        preAgent.Init(_data.ScrollDic[_navList[dataIndex]][0], _onScale);
                    }
                    else if (scrollDirectionEnum == ScrollDirectionEnum.Right)
                    { 
                        int dataIndex = _navIndex - 2;
                        if (dataIndex < 0)
                        {
                            int offset = Mathf.Abs(dataIndex);
                            dataIndex = _navList.Count - offset;
                        }
                        Debug.Log("Prepare - " + _navList[dataIndex]);
                        preAgent.Init(_data.ScrollDic[_navList[dataIndex]][0], _onScale);
                    }
                    else if (scrollDirectionEnum == ScrollDirectionEnum.Top) {
                        var items = _data.ScrollDic[_navList[_navIndex]];
                        if (items.Count == 1)
                        {
                            return;
                        }
                        else if (items.Count == 2)
                        {

                        }
                        else {
                            int dataIndex = _index + 1;
                        }                       
                    }

                }
            }
            else
            {
                var datas = _data.ScrollDic[_navList[_navIndex]];
                if (datas.Count == 2)
                {

                }
                else if (datas.Count > 2) {
                    var preAgent = _scrollPanelPrepare.GetComponentInChildren<ScrollItemAgent>();
                    if (preAgent != null)
                    {
                        Destroy(preAgent.gameObject);
                    }
                    preAgent = Instantiate(_scrollItemPrefab, _scrollPanelPrepare.transform);
                    int dataIndex;
                    if (scrollDirectionEnum == ScrollDirectionEnum.Top)
                    {
                        dataIndex = _index + 2;
                        if (dataIndex >= datas.Count)
                        {
                            int offset = dataIndex - datas.Count;
                            dataIndex = 0 + offset;
                        }
                        Debug.Log("[当前的Index] : "  + _index  + " -> [目标的Index]:" + dataIndex);

                    }
                    else {
                        dataIndex = _index - 2;
                        if (dataIndex < 0)
                        {
                            int offset = Mathf.Abs(dataIndex);
                            dataIndex = datas.Count - offset;
                        }
                    }

                    Debug.Log("数据索引: " + dataIndex);

                    preAgent.Init(datas[dataIndex], _onScale);
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
                if (_navList.Count > 2)
                {
                    r = true;
                }
                else if (_navList.Count == 2) {
                    if (scrollDirectionEnum == ScrollDirectionEnum.Left)
                    {
                        if (_navIndex == 1)
                        {
                            r = false;
                        }
                        else
                        {
                            r = true;
                        }
                    }
                    else {
                        if (_navIndex == 0)
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
            } else if (scrollDirectionEnum == ScrollDirectionEnum.Top || scrollDirectionEnum == ScrollDirectionEnum.Bottom) {
                var datas = _data.ScrollDic[_navList[_navIndex]];
                if (datas.Count > 2)
                {
                    r = true;
                }
                else if (datas.Count == 2) {
                    if (scrollDirectionEnum == ScrollDirectionEnum.Top)
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
            }
            return r;
        }

        private void HandleIndexAfterUpdate(ScrollDirectionEnum scrollDirectionEnum) {
            // 处理坐标
            if (scrollDirectionEnum == ScrollDirectionEnum.Left)
            {
                _navIndex = _navIndex + 1;
                if (_navIndex == _navList.Count)
                {
                    _navIndex = 0;
                }
                _index = 0;
                _scrollPanelTop.UpdateUpDownContent(_navIndex, _navList);
                _scrollPanelBottom.UpdateUpDownContent(_navIndex, _navList);
            }
            else if (scrollDirectionEnum == ScrollDirectionEnum.Right)
            {
                _navIndex = _navIndex - 1;
                if (_navIndex < 0)
                {
                    _navIndex = _navList.Count - 1;
                }
                _index = 0;
                _scrollPanelTop.UpdateUpDownContent(_navIndex, _navList);
                _scrollPanelBottom.UpdateUpDownContent(_navIndex, _navList);
            }
            else if (scrollDirectionEnum == ScrollDirectionEnum.Bottom) {
                var datas = _data.ScrollDic[_navList[_navIndex]];

                _index = _index - 1;
                if (_index < 0)
                {
                    _index = datas.Count - 1;
                }
            }
            else if (scrollDirectionEnum == ScrollDirectionEnum.Top)
            {
                var datas = _data.ScrollDic[_navList[_navIndex]];

                _index = _index + 1;
                if (_index == datas.Count)
                {
                    _index = 0;
                }
            }

            Debug.Log("修改后的导航位置： " + _navIndex + " INDEX: " + _index);
        }


        /// <summary>
        /// 装载内容
        /// </summary>
        void Refresh() {
            
        }

    }

}
