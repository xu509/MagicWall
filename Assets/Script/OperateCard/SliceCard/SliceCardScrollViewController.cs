using System;
using System.Collections.Generic;
using UnityEngine;
using EasingCore;


namespace MagicWall
{
    public class SliceCardScrollViewController : SliceCardBaseController<SliceCardCellData, SliceCardCellContext>
    {
        IList<SliceCardCellData> _items;
        int _envId; // env id;

        int _currentIndex;
        public int CurrentIndex { set { _currentIndex = value; } get { return _currentIndex; } }

        [SerializeField] SliceScroller scroller = default;
        [SerializeField] GameObject cellPrefab = default;

        Action<int> onSelectionChanged;

        Action OnScrollerOperationed;

        protected override GameObject CellPrefab => cellPrefab;

        void Awake()
        {
            Context.OnCellClicked = SelectCell;
            Context.OnScaleClicked = DoScale;
            Context.OnDescriptionChanged = UpdateDescription;
            Context.OnPlayVideo = OnPlayVideo;
        }

        void Start()
        {
            scroller.OnValueChanged(UpdatePosition);
            scroller.OnSelectionChanged(UpdateSelection);
            scroller.SetOperationAction(OnScrollerOperationed);
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

            UpdateComponents();

        }

        public void UpdateItemData(SliceCardAgent agent)
        {
            // 此时数据传递
            _cardAgent = agent;

        }

        public void UpdateData(IList<SliceCardCellData> items)
        {
            // 此时数据传递
            _items = items;
            UpdateContents(items);
            scroller.SetTotalCount(items.Count);

            // 此时不初始化描述与工具条
            UpdateComponents();

        }

        //
        //  设置 Env Id
        //
        public void UpdateEnvId(int env_id)
        {
            this._envId = env_id;
        }


        public void DoScale(Texture texture)
        {

            _cardAgent.InitScaleAgent(texture);

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


        protected override void UpdateComponents()
        {
            for (int i = 0; i < Pool.Count; i++)
            {
                int Index = Pool[i].Index;

                if (Index == CurrentIndex)
                {
                    Pool[i].UpdateComponentStatus();
                }
                else
                {
                    Pool[i].ClearComponentStatus();
                }
            }

        }


        // 获取当前显示卡片的描述
        public string GetCurrentCardDescription()
        {
            string str = GetCell(_currentIndex).GetCurrentDescription();
            return str;
        }

        public void UpdateDescription(string description)
        {
            //_cardAgent.UpdateDescription(description);
            _cardAgent.UpdateDescription(description);

        }

        public void SetOnScrollerOperated(Action action)
        {
            OnScrollerOperationed = action;

        }

        public void OnPlayVideo(SliceCardCellData cellData)
        {
            SliceCardAgent agent = _cardAgent as SliceCardAgent;

            agent.DoVideo(cellData.VideoUrl, cellData.Description, cellData.Image);
        }


    }
}