using System;
using System.Collections.Generic;
using UnityEngine;
using EasingCore;

namespace MagicWall {
    public class CrossCardScrollViewController : CrossCardBaseController<CrossCardCellData, CrossCardScrollViewContext>
    {
        IList<CrossCardCellData> _items;
        int _envId; // env id;

        int _currentIndex;
        public int CurrentIndex { set { _currentIndex = value; } get { return _currentIndex; } }

        [SerializeField] Scroller scroller = default;
        [SerializeField] GameObject cellPrefab = default;

        Action<int> onSelectionChanged;
        Action onScrollOperated;

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

        public void UpdateItemData(CrossCardAgent agent)
        {
            // 此时数据传递
            _cardAgent = agent;

        }

        public void UpdateData(IList<CrossCardCellData> items)
        {
            // 此时数据传递
            _items = items;

            UpdateContents(items);

            scroller.SetTotalCount(items.Count);

            UpdateComponents();

            scroller.isPrepared = true;
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
            //Debug.Log(index + "---" + ItemsSource.Count + "---" + Context.SelectedIndex);
            if (index < 0 || index >= ItemsSource.Count || index == Context.SelectedIndex)
            {
                Debug.Log(111111);
                return;
            }

            UpdateSelection(index);
            scroller.ScrollTo(index, 0.35f, Ease.OutCubic);

        }

        public void OnSelectionChanged(Action<int> callback)
        {
            onSelectionChanged = callback;
        }


        public override void UpdateComponents()
        {
            for (int i = 0; i < Pool.Count; i++)
            {
                int Index = GetCell(i).Index;

                if (Index == CurrentIndex)
                {
                    GetCell(i).UpdateComponentStatus();
                }
                else
                {
                    GetCell(i).ClearComponentStatus();
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
            _cardAgent.UpdateDescription(description);
        }

        public void OnPlayVideo(CrossCardCellData cellData)
        {
            CrossCardAgent agent = _cardAgent as CrossCardAgent;
            agent.DoVideo(cellData.VideoUrl, cellData.Description, cellData.Image);
        }


        public void SetScrollOperatedAction(Action action)
        {
            onScrollOperated = action;
        }

    }
}
