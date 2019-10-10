using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 操作卡片控制器
/// </summary>
/// 

namespace MagicWall {
    public class OperateCardManager : MonoBehaviour
    {
        [SerializeField, Header("UI")] Transform _container;


        [SerializeField,Header("Prefab")] SingleCardAgent _singleCardPrefab;
        public SingleCardAgent singleCardPrefab { get { return _singleCardPrefab; } }

        [SerializeField] SliceCardAgent _sliceCardPrefab;
        public SliceCardAgent sliceCardPrefab { get { return _sliceCardPrefab; } }

        [SerializeField] CrossCardAgent _crossCardPrefab;
        public CrossCardAgent crossCardPrefab { get { return _crossCardPrefab; } }


        //  正在操作的 agents
        List<CardAgent> _effectAgents;
        public List<CardAgent> EffectAgents { get { return _effectAgents; } }


        private MagicWallManager _manager;



        // Start is called before the first frame update
        void Start()
        {
            _effectAgents = new List<CardAgent>();

        }

        // Update is called once per frame
        void Update()
        {
            List<CardAgent> cardAgentNeedDestory = null;

            //Debug.Log("_effectAgents count : " + _effectAgents.Count); 


            // 检测需要关闭的卡片
            for (int i = 0; i < _effectAgents.Count; i++)
            {
                if (_effectAgents[i].CardStatus == MagicWall.CardStatusEnum.OBSOLETE)
                {
                    if (cardAgentNeedDestory == null)
                    {
                        cardAgentNeedDestory = new List<CardAgent>();
                    }

                    cardAgentNeedDestory.Add(_effectAgents[i]);
                }
            }

            // 删除需要删除的卡片
            if (cardAgentNeedDestory != null && cardAgentNeedDestory.Count > 0)
            {
                for (int i = 0; i < cardAgentNeedDestory.Count; i++)
                {
                    var agentToDestory = cardAgentNeedDestory[i];

                    // 从索引中销毁
                    _effectAgents.Remove(agentToDestory);

                    // 物理销毁
                    Destroy(agentToDestory.gameObject);

                    //Debug.Log("Do Destory");

                }
                cardAgentNeedDestory.Clear();
            }

        }

        /// <summary>
        ///  REF: https://www.yuque.com/docs/share/58f46b17-0b98-430b-a14c-4a08e20690e5
        /// </summary>
        private void CloseCardWhenOverNumber() {
            // 当数量超过设定的额度
            if (_effectAgents.Count >= _manager.managerConfig.SelectedItemMaxCount) {
                Debug.Log("打开卡片超过限度");

                CardAgent cardToClose = null;

                for (int i = 0; i < _effectAgents.Count; i++) {
                    var effectAgent = _effectAgents[i];
                    if (effectAgent.CardStatus == CardStatusEnum.NORMAL) {
                        if (cardToClose == null || effectAgent.GetFreeTime() > cardToClose.GetFreeTime()) {
                            cardToClose = effectAgent;
                        }
                    }
                }

                if (cardToClose != null) {
                    Debug.Log("打开卡片超过限度,关闭：" + cardToClose.name);


                    // 直接删除
                    cardToClose.DoCloseDirect();
                }
            }
        }




        /// <summary>
        ///  创建一个新的操作卡片
        /// </summary>
        /// <param name="dataId">数据ID</param>
        /// <param name="dataType">数据类型</param>
        public CardAgent CreateNewOperateCard(int dataId, DataTypeEnum dataType,Vector3 position,FlockAgent refAgent)
        {            
            CloseCardWhenOverNumber();


            CardAgent cardAgent = OperateCardFactoryInstance.
                Generate(_manager, position, _container,dataId, dataType, refAgent);

            Vector3 scaleVector3 = new Vector3(0.1f, 0.1f, 0.1f);
            cardAgent.GetComponent<RectTransform>().localScale = scaleVector3;
            cardAgent.CardStatus = CardStatusEnum.GENERATE;

            EffectAgents.Add(cardAgent);
            return cardAgent;
        }


        public void Init(MagicWallManager manager)
        {
            _manager = manager;
        }


    }

}

