using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall {
    public class CrossScrollAgent : MonoBehaviour
    {
        private OperateCardDataCross _data;
        private MagicWallManager _manager;

        private int _index;  // 横向的index，从0开始
        private CrossCardNavType _currentNavType;   //当前的nav

        private List<CrossCardNavType> _navList;
        private List<ScrollPanelAgent> _scrollPanelAgents;


        [SerializeField] ScrollAreaAgent _scrollAreaAgent;
        [SerializeField] ScrollItemAgent _scrollItemPrefab;
        public ScrollItemAgent scrollItemPrefab { get { return _scrollItemPrefab; } }

        [SerializeField] RectTransform _middlePanel;

        [SerializeField] ScrollPanelAgent _scrollPanelTop;
        [SerializeField] ScrollPanelAgent _scrollPanelBottom;
        [SerializeField] ScrollPanelAgent _scrollPanelLeft;
        [SerializeField] ScrollPanelAgent _scrollPanelRight;
        [SerializeField] ScrollPanelAgent _scrollPanelMiddle;

        public void Init(OperateCardDataCross data) {
            _data = data;
            _manager = GameObject.Find("MagicWall").GetComponent<MagicWallManager>();

            _scrollPanelAgents = new List<ScrollPanelAgent>();
            _scrollPanelAgents.Add(_scrollPanelTop);
            _scrollPanelAgents.Add(_scrollPanelBottom);
            _scrollPanelAgents.Add(_scrollPanelLeft);
            _scrollPanelAgents.Add(_scrollPanelRight);
            _scrollPanelAgents.Add(_scrollPanelMiddle);

            // 设置首图
            var firstData = data.ScrollDic[CrossCardNavType.Index];
            Debug.Log("firstData[0] : " + firstData[0].ToString());

            _scrollPanelMiddle.SetData(this, firstData[0]);

            ////_middlePanel.GetComponentInChildren
            //var item = _middlePanel.GetComponent<ScrollItemAgent>();
            //if (item == null) {
            //    // 创建prefab
            //    item = Instantiate(_scrollItemPrefab, _middlePanel);
            //}
            //item.Init(firstData[0]);

            // 初始化内容
            _currentNavType = CrossCardNavType.Index;
            _index = 0;
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
                _scrollPanelTop.SetData(this,_data.ScrollDic[nav][0]);
            }
            else if (_navList.Count > 2) {
                // 添加上与下两部分
                CrossCardNavType nav = _navList[1];
                _scrollPanelLeft.SetData(this, _data.ScrollDic[nav][0]);
                _scrollPanelLeft.GoOutLocation();

                var count = _navList.Count;
                count = count - 1;
                CrossCardNavType nav2 = _navList[count];
                _scrollPanelRight.SetData(this, _data.ScrollDic[nav2][0]);
                _scrollPanelRight.GoOutLocation();
            }



            //Refresh();
            //for (int i = 0; i < _scrollPanelAgents.Count; i++) {
            //    _scrollPanelAgents[i].InitCompleted();
            //}

            _scrollAreaAgent.Init(OnRecognizeDirection);
        }



        void OnRecognizeDirection(ScrollDirectionEnum scrollDirectionEnum) {
            Debug.Log("scrollDirectionEnum : " + scrollDirectionEnum);
        }


        /// <summary>
        /// 装载内容
        /// </summary>
        void Refresh() {
            
        }

    }

}
