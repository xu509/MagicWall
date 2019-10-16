using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using UnityEngine.UI;



//
//  搜索代理
//
namespace MagicWall
{
    public class SearchAgent : MonoBehaviour
    {
        Action _onClickReturn;
        Action _onClickMove;
        Action _onUpdate;

        [SerializeField, Range(0f, 1f)] float _height_factor;  //高度缩放因素
        [SerializeField] WritePadAgent _writePadAgent;  // 手写板agent
        [SerializeField] RectTransform _associateWordArea; // 联想内容区域
        [SerializeField] AssociateWordAgent _associateWordPrefab; //联想字的prefab
        [SerializeField] RectTransform _associateWordMessagePrefab; //联想字提示的prefab
        [SerializeField] RectTransform _backspaceRect; //退格控件
        [SerializeField] Text _searchText; //搜索词的文本控件
        [SerializeField] SearchResultAgent _searchResultAgentPrefab;   //  搜索结果的prefab
        [SerializeField] RectTransform _searchResultContainer;   //  搜索结果的容器
        [SerializeField] RectTransform _searchAgentContainer;   //  搜索代理的容器

        [SerializeField, Header("Move")] Sprite _sprite_move_active;
        [SerializeField] MoveButtonComponent _moveBtnComponent;
        [SerializeField] Sprite _sprite_move;
        [SerializeField] RectTransform _move_rect;

        // 提示功能相关
        [SerializeField, Header("Question")] RectTransform _questionContainer;
        [SerializeField] QuestionAgent _questionPrefab;
        private bool _showQuestion;
        private QuestionAgent _questionAgent;
        // 提示功能相关 结束


        private string _searchWord; //  搜索词
        private SearchResultAgent _searchResultAgent;    //  搜索结果索引
        private MagicWallManager _manager;  //  主管理器索引
        private FlockAgent _flockAgent; //  原浮块索引
        private CardAgent _cardAgent;   //  原卡片索引

        private bool _doMoving = false;



        private int sessionId; //该会话





        void Start()
        {

            // 初始化
            Init();


        }

        public void Init()
        {
            _searchWord = "";

            _writePadAgent.SetOnRecognizedSuccess(OnRecognizedSuccess);
            _writePadAgent.SetOnRecognizedError(OnRecognizedError);
            _writePadAgent.SetOnUpdated(OnUpdated);

            // 设置控件长宽
            float height = Screen.height * _height_factor;
            float width = height;

            GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

            InitBackspaceStatus();
        }

        public void InitData(MagicWallManager manager, CardAgent cardAgent)
        {
            _manager = manager;
            _cardAgent = cardAgent;
            _flockAgent = cardAgent.OriginAgent;


            _moveBtnComponent.Init(DoMove, cardAgent);

        }

        // Update is called once per frame
        void Update()
        {

        }

        #region 





        #endregion


        //  手写板识别内容后的回调
        private void OnRecognizedSuccess(string[] strs)
        {
            // 清理联想板块
            ClearAssociateWordArea();

            // 增加联想的内容
            if (strs.Length == 0)
            {
                RectTransform item = Instantiate(_associateWordMessagePrefab, _associateWordArea);
                item.GetComponent<Text>().text = "未能识别您的笔迹。";
            }
            else
            {
                int length = strs.Length;
                if (length > 6)
                    length = 6;

                for (int i = 0; i < length; i++)
                {
                    AssociateWordAgent associateWordAgent = Instantiate(_associateWordPrefab, _associateWordArea);
                    associateWordAgent.SetText(strs[i]);
                    // 装载点击事件
                    associateWordAgent.SetOnClickWord(OnClickAssociateWord);

                }
            }

            _onUpdate.Invoke();
        }

        /// <summary>
        ///     识别失败回调
        /// </summary>
        /// <param name="message">消息</param>
        private void OnRecognizedError(string message)
        {
            // 清理联想板块
            ClearAssociateWordArea();

            // 增加联想的内容

            RectTransform item = Instantiate(_associateWordMessagePrefab, _associateWordArea);
            item.GetComponentInChildren<Text>().text = message;

            _onUpdate.Invoke();
        }


        // 清理联想板块
        private void ClearAssociateWordArea()
        {
            foreach (Transform child in _associateWordArea)
            {
                Destroy(child.gameObject);
            }
        }

        // 联想词点击事件
        private void OnClickAssociateWord(string str)
        {
            if (_cardAgent.CardStatus == CardStatusEnum.TODESTORY)
            {
                _onUpdate.Invoke();
            }
            else
            {
                // 将被点击的字添加至搜索框内

                _searchWord += str;

                UpdateSearchWord();

                // 清理联想面板
                ClearAssociateWordArea();

                // 更新退格状态
                InitBackspaceStatus();
            }


            
        }

        //  初始化退格状态
        private void InitBackspaceStatus()
        {
            int count = _searchWord.Length;
            if (count == 0)
            {
                _backspaceRect.gameObject.SetActive(false);
            }
            else
            {
                _backspaceRect.gameObject.SetActive(true);
            }
        }

        #region 自有功能
        /// <summary>
        /// 关闭搜索结果的容器
        /// </summary>
        private void CloseSearchResultContainer(bool doDestory)
        {

            Debug.Log("关闭搜索结果的容器");

            _searchResultContainer.gameObject.SetActive(false);

            if (doDestory)
            {
                Destroy(_searchResultAgent.gameObject);
                _searchResultAgent = null;
            }
        }

        /// <summary>
        /// 打开搜索结果的容器
        /// </summary>
        private void OpenSearchResultContainer()
        {
            _searchResultContainer.gameObject.SetActive(true);
        }

        /// <summary>
        /// 关闭搜索代理的容器 
        /// </summary>
        private void CloseSearchAgentContainer(bool doDestory)
        {
            _searchAgentContainer.gameObject.SetActive(false);

            if (doDestory)
            {
                //_searchResultAgent = null;
                Destroy(_searchAgentContainer.gameObject);
            }
        }

        /// <summary>
        /// 打开搜索代理的容器
        /// </summary>
        private void OpenSearchAgentContainer()
        {
            _searchAgentContainer.gameObject.SetActive(true);
        }

        #endregion

        #region Search Result 代理功能
        /// <summary>
        ///     点击搜索结果控件的回退功能
        /// </summary>
        private void OnClickSearchResultReturnBtn()
        {

            // 关闭新打开的结果窗口
            CloseSearchResultContainer(true);

            //  打开原来的Search窗口
            OpenSearchAgentContainer();

        }

        //  Search Result 点击移动的功能
        private void OnClickSearchResultMoveBtn()
        {
            DoMove();
        }

        #endregion


        // 退格功能
        public void DoBackspace()
        {
            if (_cardAgent.CardStatus == CardStatusEnum.TODESTORY)
            {
                _onUpdate.Invoke();
            }
            else
            {
                _searchWord = _searchWord.Substring(0, _searchWord.Length - 1);

                UpdateSearchWord();

                InitBackspaceStatus();
                _onUpdate.Invoke();

            }

        }


        #region 点击搜索

        // 搜索功能
        public void DoSearch()
        {
            if (_cardAgent.CardStatus == CardStatusEnum.TODESTORY)
            {
                _onUpdate.Invoke();
            }
            else {
                CloseSearchAgentContainer(false);

                //  获取查询词，进行搜索，得到 SearchBean 列表
                List<SearchBean> searchBeans = _manager.daoService.Search(_searchWord);

                //  生成搜索结果控件，并进行初始化
                if (_searchResultAgent == null)
                {
                    Debug.Log("_searchResultAgent == null");

                    _searchResultAgent = Instantiate(_searchResultAgentPrefab, _searchResultContainer) as SearchResultAgent;
                    _searchResultAgent.Init(_onUpdate);
                }
                else
                {
                    _searchResultAgent.Init(_onUpdate);
                }

                //  搜索结果控件进行加载数据
                _searchResultAgent.InitData(searchBeans, _searchWord, _manager, _cardAgent);


                //  装载事件代理
                _searchResultAgent.SetOnClickMoveBtn(OnClickSearchResultMoveBtn);
                _searchResultAgent.SetOnClickReturnBtn(OnClickSearchResultReturnBtn);
                _searchResultAgent.SetOnClickSearchResultItem(OnClickSearchResultItem);

                OpenSearchResultContainer();
            }
        }

        // 点击回退
        public void DoReturn()
        {
            if (_cardAgent.CardStatus == CardStatusEnum.TODESTORY)
            {
                _onUpdate.Invoke();
            }
            else
            {
                _onClickReturn.Invoke();
            }
        }

        /// <summary>
        /// 搜索功能
        /// </summary>
        public void DoMove()
        {
            //if (!_doMoving)
            //{
            //    _move_rect.GetComponent<Image>().sprite = _sprite_move_active;
            //}
            //else {
            //    _move_rect.GetComponent<Image>().sprite = _sprite_move;

            //}

            //_doMoving = !_doMoving;

            _onClickMove.Invoke();
        }

        /// <summary>
        /// 点击搜索结果的 Item
        /// </summary>
        /// <param name="searchBean"></param>
        private void OnClickSearchResultItem(SearchBean searchBean)
        {

            //  将 SearchAgent 关闭
            CloseSearchAgentContainer(true);
            CloseSearchResultContainer(true);


            // 将card的显示关系打开
            _cardAgent.RecoverContainerAfterSearch();

            _cardAgent.CardStatus = CardStatusEnum.OBSOLETE;

            //_manager.agentManager.RemoveItemFromEffectItems(_cardAgent); // 将影响实体清除

            //  打开新的卡片
            //ItemsFactory itemsFactory = GetItemFactory(searchBean.type);
            Vector3 genVector3 = _cardAgent.GetComponent<RectTransform>().anchoredPosition;

            CardAgent cardAgent = _manager.operateCardManager.CreateNewOperateCard(searchBean.id,searchBean.type,genVector3,_cardAgent.OriginAgent);

           //itemsFactory.GenerateCardAgent(genVector3, null, searchBean.id, true);
            cardAgent.GoToFront(()=> {

            });

        }



        #endregion

        #region 事件代理装载
        public void OnClickReturn(Action action)
        {
            _onClickReturn = action;
        }

        public void OnClickMove(Action action)
        {
            _onClickMove = action;
        }

        public void OnUpdate(Action action) {
            _onUpdate = action;
        }
        #endregion

        private void UpdateSearchWord()
        {
            _searchText.text = _searchWord;
        }


        private void OnUpdated() {
            _onUpdate.Invoke();
        }


        #region 提示内容
        public void DoQuestion()
        {
            if (_cardAgent.CardStatus == CardStatusEnum.TODESTORY)
            {
                _onUpdate.Invoke();
            }
            else {

                if (_showQuestion)
                {
                    _questionAgent?.CloseReminder();
                }
                else
                {
                    _questionAgent = Instantiate(_questionPrefab, _questionContainer);
                    _questionAgent.Init(OnQuestionClose);
                    _questionAgent.ShowReminder(QuestionTypeEnum.SearchPanel);
                    _showQuestion = true;
                }
            }
        }

        private void OnQuestionClose()
        {
            _showQuestion = false;
        }

        #endregion



    }
}