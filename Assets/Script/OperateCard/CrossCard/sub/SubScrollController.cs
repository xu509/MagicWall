using System;
using System.Collections.Generic;
using UnityEngine;
using EasingCore;


namespace MagicWall
{
    public class SubScrollController : SubScrollBaseController<CrossCardCellData, CrossCardScrollViewContext>
    {
        CrossCardScrollViewCell _crossCardScrollViewCell;
        public CrossCardScrollViewCell crossCardScrollViewCell { set { _crossCardScrollViewCell = value; } get { return _crossCardScrollViewCell; } }

        int _currentIndex;
        public int CurrentIndex { set { _currentIndex = value; } get { return _currentIndex; } }

        IList<CrossCardCellData> _items;

        [SerializeField] SubScroller scroller = default;
        [SerializeField] GameObject cellPrefab = default;

        Action<int> onSelectionChanged;

        protected override GameObject CellPrefab => cellPrefab;

        void Awake()
        {
            Context.OnCellClicked = SelectCell;
            Context.OnScaleClicked = ScaleCell;
        }

        void Start()
        {
            scroller.OnValueChanged(UpdatePosition);
            scroller.OnSelectionChanged(UpdateSelection);
        }

        void UpdateSelection(int index)
        {
            if (Context.SelectedIndex == index)
            {
                return;
            }

            CurrentIndex = index;
            Context.SelectedIndex = index;
            Refresh();
            onSelectionChanged?.Invoke(index);

            UpdateComponents();
            // 获取资料，更新


        }

        public void UpdateData(IList<CrossCardCellData> items)
        {
            _items = items;
            UpdateContents(items);
            scroller.SetTotalCount(items.Count);

            // 此时第一次更新数据
            InitComponents();

        }

        public void SelectCell(int index)
        {
            if (index < 0 || index >= ItemsSource.Count || index == Context.SelectedIndex)
            {
                return;
            }

            UpdateSelection(index);
            scroller.ScrollTo(index, 0.35f, Ease.OutCubic);
        }

        public void ScaleCell(Texture texture)
        {
            Debug.Log("ScaleCell");
            //_crossCardScrollViewCell

        }

        public void OnSelectionChanged(Action<int> callback)
        {
            onSelectionChanged = callback;
        }

        protected override void UpdateComponents()
        {
            // 调整前后关系


            List<int> lis = new List<int>();
            int d = 0;

            for (int i = 0; i < Pool.Count; i++)
            {
                int Index = GetCell(i).Index;

                GetCell(i).SetupContext(Context);

                if (Index == CurrentIndex)
                {
                    GetCell(i).UpdateComponentStatus();
                }
                else
                {
                    GetCell(i).ClearComponentStatus();
                }
                
                // 调整一次位置
                int dis = Mathf.Abs(Index - CurrentIndex);
                lis.Add(dis);

                //GetCell(i).SetAsLastPosition();
            }

            UpdateSiblingIndex();

        }

        public override void UpdateAllComponents()
        {
            UpdateComponents();
        }

        public override void ClearAllComponents()
        {
            for (int i = 0; i < Pool.Count; i++)
            {
                GetCell(i).ClearComponentStatus();
            }
        }

        public string GetCurrentDescription()
        {

            string str = "";

            str = GetCell(_currentIndex)?.GetData().Description;
            return str;
        }



        private void InitComponents() {
            for (int i = 0; i < Pool.Count; i++)
            {
                GetCell(i).SetAsLastPosition();
            }
        }



        /// <summary>
        /// 设置层级代码
        /// </summary>
        private void UpdateSiblingIndex() {

            // <int : 插值，int : Pool中索引>
            Dictionary<int, int> dics = new Dictionary<int, int>();

            for (int i = 0; i < Pool.Count; i++)
            {
                int Index = GetCell(i).Index;                

                var a = Mathf.Abs(CurrentIndex - Index);
                
                dics.Add(i, a);
            }

            List<KeyValuePair<int, int>> lst = new List<KeyValuePair<int, int>>(dics);
            lst.Sort(delegate (KeyValuePair<int, int> s1, KeyValuePair<int, int> s2)
            {
                return s1.Value.CompareTo(s2.Value);
            });

            foreach (KeyValuePair<int, int> kvp in lst) {
                Pool[kvp.Key].SetAsLastPosition();
            }
        }



    }
}