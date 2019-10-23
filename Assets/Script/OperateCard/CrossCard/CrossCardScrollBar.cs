using System;
using System.Collections.Generic;
using UnityEngine;
using EasingCore;


namespace MagicWall
{
    public class CrossCardScrollBar : FancyScrollView<CrossCardCellData, CrossCardScrollViewContext>
    {
        int _currentIndex;
        public int CurrentIndex { set { _currentIndex = value; } get { return _currentIndex; } }

        [SerializeField] ScrollerDefault scroller = default;
        [SerializeField] GameObject cellPrefab = default;
        [SerializeField] RectTransform signRect;

        Action<int> onSelectionChanged;
        Action onScrollOperated;


        protected override GameObject CellPrefab => cellPrefab;

        void Awake()
        {
            Context.OnCellClicked = SelectCell;
        }

        void Start()
        {
            scroller.OnValueChanged(UpdatePosition);
            scroller.OnSelectionChanged(UpdateSelection);
            scroller.SetOnOperatedUpdate(onScrollOperated);
        }

        void UpdateSelection(int index)
        {
            if (Context.SelectedIndex == index)
            {
                return;
            }
            _currentIndex = index;
            Context.SelectedIndex = index;
            Refresh();
            onSelectionChanged?.Invoke(index);
        }

        public void UpdateData(IList<CrossCardCellData> items)
        {
            UpdateContents(items);
            scroller.SetTotalCount(items.Count);
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

        public void OnSelectionChanged(Action<int> callback)
        {
            onSelectionChanged = callback;
        }

        public FancyScrollViewCell<CrossCardCellData, CrossCardScrollViewContext> GetCell(int index)
        {

            float newindex = CircularPosition(index, Pool.Count);

            return Pool[(int)newindex];
        }


        public void UpdateComponents()
        {

            // 更新单个的状态
            for (int i = 0; i < Pool.Count; i++)
            {
                CrossCardScrollBarCell cell = GetCell(i) as CrossCardScrollBarCell;
                cell.UpdateComponent(signRect);
            }

            // 调整标记的位置

        }




        private float CircularPosition(float p, int size)
        {
            if (size < 1)
            {
                return 0;
            }
            else
            {
                if (p < 0)
                {
                    return size - 1 + (p + 1) % size;
                }
                else
                {
                    return p % size;
                }

            }
        }

        public void SetScrollOperatedAction(Action action)
        {
            onScrollOperated = action;
        }


    }
}