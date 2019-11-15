using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall {

    /// <summary>
    ///   顶部索引
    /// </summary>
    public class ScrollBarAgent : MonoBehaviour
    {

        private bool _isPrepared = false; // 即可执行操作，在变动时不可进行操作
        private List<CrossCardNavType> _navs;
        private int _index;

        private List<ScrollBarPanelAgent> _scrollBarPanelAgents;


        [SerializeField] ScrollBarItemAgent _scrollBarItemPrefab;
        public ScrollBarItemAgent scrollBarItemPrefab { get { return _scrollBarItemPrefab; } }

        [SerializeField] ScrollBarPanelAgent _leftBarPanelAgent;
        public ScrollBarPanelAgent leftBarPanelAgent { get { return _leftBarPanelAgent; } }

        [SerializeField] ScrollBarPanelAgent _middleBarPanelAgent;
        public ScrollBarPanelAgent middleBarPanelAgent { get { return _middleBarPanelAgent; } }

        [SerializeField] ScrollBarPanelAgent _rightBarPanelAgent;
        public ScrollBarPanelAgent rightBarPanelAgent { get { return _rightBarPanelAgent; } }

        [SerializeField] ScrollBarPanelAgent _prepareBarPanelAgent;
        public ScrollBarPanelAgent prepareBarPanelAgent { get { return _prepareBarPanelAgent; } }



        public void Init(List<CrossCardNavType> navs,Action<ScrollDirectionEnum> onScroll) {
            // 初始化
            _navs = navs;

            if (navs.Count == 1)
            {
                _leftBarPanelAgent.gameObject.SetActive(false);
                _rightBarPanelAgent.gameObject.SetActive(false);

                middleBarPanelAgent.gameObject.SetActive(true);
                middleBarPanelAgent.Init(this);
                middleBarPanelAgent.SetData(navs[0]);
            }
            else if (navs.Count == 2)
            {
                _leftBarPanelAgent.gameObject.SetActive(true);

                middleBarPanelAgent.gameObject.SetActive(true);
                middleBarPanelAgent.Init(this);
                middleBarPanelAgent.SetData(navs[0]);

                _rightBarPanelAgent.gameObject.SetActive(true);
                _rightBarPanelAgent.Init(this);
                _rightBarPanelAgent.SetData(navs[1]);

            }
            else if (navs.Count > 2) {

                _leftBarPanelAgent.gameObject.SetActive(true);
                _leftBarPanelAgent.Init(this);
                middleBarPanelAgent.gameObject.SetActive(true);
                middleBarPanelAgent.Init(this);
                _rightBarPanelAgent.gameObject.SetActive(true);
                _rightBarPanelAgent.Init(this);
                _prepareBarPanelAgent.Init(this);


                middleBarPanelAgent.SetData(navs[0]);
                _rightBarPanelAgent.SetData(navs[1]);
                _leftBarPanelAgent.SetData(navs[navs.Count - 1]);
            }

            _index = 0;


            _scrollBarPanelAgents = new List<ScrollBarPanelAgent>();
            _scrollBarPanelAgents.Add(_leftBarPanelAgent);
            _scrollBarPanelAgents.Add(_rightBarPanelAgent);
            _scrollBarPanelAgents.Add(_middleBarPanelAgent);
            _scrollBarPanelAgents.Add(_prepareBarPanelAgent);

            _isPrepared = true;
        }


        public void TurnLeft() {
            if (_navs.Count == 1)
            {
                return;
            }
            else if (_navs.Count == 2)
            {
                if (_index == 1)
                {
                    return;
                }
                else
                {
                    DoChange(ScrollDirectionEnum.Left);
                }
            }
            else if (_navs.Count > 2) {
                GeneratePrepareAgent(ScrollDirectionEnum.Left);
                DoChange(ScrollDirectionEnum.Left);
            }
        }

        public void TurnRight() {
            if (_navs.Count == 1)
            {
                return;
            }
            else if (_navs.Count == 2)
            {
                if (_index == 0)
                {
                    return;
                }
                else
                {
                    DoChange(ScrollDirectionEnum.Right);
                }
            }
            else if (_navs.Count > 2)
            {
                GeneratePrepareAgent(ScrollDirectionEnum.Right);
                DoChange(ScrollDirectionEnum.Right);
            }
        }


        private void DoChange(ScrollDirectionEnum scrollDirectionEnum) {
            if (!_isPrepared) {
                return;
            }
            _isPrepared = false;


            for (int i = 0; i < _scrollBarPanelAgents.Count; i++) {
                _scrollBarPanelAgents[i].UpdatePosition(scrollDirectionEnum,()=> {
                    _isPrepared = true;
                });
            }


            // 更新坐标地址
            if (scrollDirectionEnum == ScrollDirectionEnum.Left)
            {
                _index++;

                if (_index == _navs.Count)
                {
                    _index = 0;
                }
            }
            else if (scrollDirectionEnum == ScrollDirectionEnum.Right) {
                _index--;
                if (_index < 0) {
                    _index = _navs.Count - 1;
                }
            }

            
        }

        private void GeneratePrepareAgent(ScrollDirectionEnum scrollDirectionEnum) {

            var preAgent = _prepareBarPanelAgent.GetComponentInChildren<ScrollBarItemAgent>();
            if (preAgent != null)
            {
                Destroy(preAgent.gameObject);
            }
            preAgent = Instantiate(_scrollBarItemPrefab, _prepareBarPanelAgent.transform);

            if (scrollDirectionEnum == ScrollDirectionEnum.Left)
            {
                int dataIndex = _index + 2;

                if (dataIndex >= _navs.Count)
                {
                    int offset = dataIndex - _navs.Count;
                    dataIndex = 0 + offset;
                }
                preAgent.Init(_navs[dataIndex]);
                
            }
            else if (scrollDirectionEnum == ScrollDirectionEnum.Right) {

                int dataIndex = _index - 2;
                if (dataIndex < 0)
                {
                    int offset = Mathf.Abs(dataIndex);
                    dataIndex = _navs.Count - offset;
                }
                preAgent.Init(_navs[dataIndex]);
                ;
            }

        }







    }

}
