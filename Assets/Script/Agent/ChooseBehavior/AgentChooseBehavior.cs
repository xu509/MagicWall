using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace MagicWall {

    /// <summary>
    /// ref : https://www.yuque.com/u314548/fc6a5l/pvvetv
    /// </summary>
    public class AgentChooseBehavior : MonoBehaviour
    {
        MagicWallManager _manager;

        public void Init(MagicWallManager manager) {
            _manager = manager;
        }



        public void DoChoose(FlockAgent flockAgent) {

            var agents = _manager.kinectManager.kinectAgents;

            if (_manager.useKinect && agents != null && agents.Count != 0)
            {
                DoChooseForKinect(flockAgent);
            }
            else {
                DoChooseForCommon(flockAgent);
            }
        }


        /// <summary>
        /// REF： https://www.yuque.com/u314548/fc6a5l/dozp0e
        /// </summary>
        /// <param name="flockAgent"></param>
        private void DoChooseForKinect(FlockAgent flockAgent) {
            int _data_id = flockAgent.DataId;
            var _dataType = flockAgent.dataTypeEnum;

            CardAgent _cardAgent;

            if (CanChoose(flockAgent)) {
                flockAgent.flockStatus = FlockStatusEnum.TOHIDE;

                var flockAgentPosition = flockAgent.GetComponent<RectTransform>().transform.position;
                RectTransform flockRect = flockAgent.GetComponent<RectTransform>();

                // 获取kinect obj 的位置
                var agents = _manager.kinectManager.kinectAgents;
                var distance = 3000f;
                KinectAgent targetKinectAgent = null;
                for (int i = 0; i < agents.Count; i++) {
                    var kinectPosition = agents[i].GetComponent<RectTransform>().transform.position;
                    var d = Vector2.Distance(kinectPosition, flockAgentPosition);

                    if (d < distance) {
                        targetKinectAgent = agents[i];
                        distance = d;
                    }
                }

                /// 遮罩中存在贴附的卡片
                if (targetKinectAgent.refFlockAgent != null)
                {
                    var refFlockAgent = targetKinectAgent.refFlockAgent;
                    targetKinectAgent.RecoverColliderEffect();

                    // 此逻辑需要优化
                    RemoveRefCard(targetKinectAgent.refFlockAgent);
                    //cardAgent.SetDisableEffect(true);
                    //cardAgent.DoCloseDirect();
                }

                if (targetKinectAgent.status == KinectAgentStatusEnum.Small) {
                    // 恢复大小并打开检测
                    targetKinectAgent.SetDisableEffect(false);
                    targetKinectAgent.status = KinectAgentStatusEnum.Recovering;
                    targetKinectAgent.GetComponent<RectTransform>().DOScale(1f, 0.5f).OnComplete(
                            () => {
                                targetKinectAgent.status = KinectAgentStatusEnum.Hide;
                            }
                        );
                }

                // 点击的卡片移动至遮罩位置，生成卡片并放大
                targetKinectAgent.refFlockAgent = flockAgent;

                var ani_time = 1.5f;
                var scaleAni = flockAgent.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), ani_time - 0.2f);
                var moveAni = flockAgent.transform.DOMove(targetKinectAgent.transform.position, ani_time)
                    .OnUpdate(()=> {})
                    .OnComplete(()=> {

                    flockAgent.transform.SetParent(_manager.OperationPanel);

                    targetKinectAgent.Hide();
                    flockAgent.flockStatus = FlockStatusEnum.HIDE;
                    //Debug.Log(flockAgent.gameObject.name + " status : " + flockAgent.flockStatus);


                    flockAgent.gameObject.SetActive(false);

                    var _cardGenPos = flockAgent.GetComponent<RectTransform>().anchoredPosition;

                    // 创建卡片
                    _cardAgent = _manager.operateCardManager.CreateNewOperateCard(_data_id, _dataType, _cardGenPos, flockAgent);

                    _cardAgent.GoToFront(()=> {
                        targetKinectAgent.SetDisableEffect(true);
                    });
                });

                flockAgent.flockTweenerManager.Add(FlockTweenerManager.Kinnect_Choose_Scale, scaleAni);
                flockAgent.flockTweenerManager.Add(FlockTweenerManager.Kinnect_Choose_Move, moveAni);

            }
        }

        private void DoChooseForCommon(FlockAgent flockAgent)
        {
            int _data_id = flockAgent.DataId;
            var _dataType = flockAgent.dataTypeEnum;

            CardAgent _cardAgent;

            if (CanChoose(flockAgent))
            {
                flockAgent.flockStatus = FlockStatusEnum.TOHIDE;

                //_isChoosing = true;

                //  先缩小（向后退）
                RectTransform rect = flockAgent.GetComponent<RectTransform>();
                Vector2 positionInMainPanel = rect.anchoredPosition;

                //  移到后方、缩小、透明
                //rect.DOScale(0.1f, 0.3f);
                Vector3 to = new Vector3(0.2f, 0.2f, 0.7f);

                var _cardGenPos = GetCardGeneratePosition(flockAgent);

                // 完成缩小与移动后创建十字卡片
                rect.DOScale(0.5f, 0.3f).OnComplete(() =>
                {
                    flockAgent.flockStatus = FlockStatusEnum.HIDE;
                    flockAgent.gameObject.SetActive(false);

                    //Debug.Log("chose :" + _data_id);

                    _cardAgent = _manager.operateCardManager.CreateNewOperateCard(_data_id, _dataType, _cardGenPos, flockAgent);


                    //靠近四周边界需要偏移
                    float w = _cardAgent.GetComponent<RectTransform>().rect.width;
                    float h = _cardAgent.GetComponent<RectTransform>().rect.height;

                    // 如果点击时,出生位置在最左侧
                    if (_cardGenPos.x < w / 2)
                    {
                        _cardGenPos.x = w / 2;
                    }

                    // 出身位置在最右侧
                    if (_cardGenPos.x > _manager.OperationPanel.rect.width - w / 2)
                    {
                        _cardGenPos.x = _manager.OperationPanel.rect.width - w / 2;
                    }

                    // 出生位置在最下侧
                    if (_cardGenPos.y < h / 2)
                    {
                        _cardGenPos.y = h / 2;
                    }

                    // 出生位置在最上侧
                    if (_cardGenPos.y > _manager.OperationPanel.rect.height - h / 2)
                    {
                        _cardGenPos.y = _manager.OperationPanel.rect.height - h / 2;
                    }

                    _cardAgent.GetComponent<RectTransform>().anchoredPosition = _cardGenPos;

                    _cardAgent.GoToFront(()=> {
                    });

                });
            }
        }


        private bool CanChoose(FlockAgent flockAgent) {
            bool canChoose = false;

            var _flockStatus = flockAgent.flockStatus;

            if (_flockStatus == FlockStatusEnum.NORMAL
                || _flockStatus == FlockStatusEnum.RUNIN
                || _flockStatus == FlockStatusEnum.STAR)
            {
                canChoose = true;
            }
            return canChoose;
        }


        private Vector3 GetCardGeneratePosition(FlockAgent flockAgent)
        {
            AgentContainerType _agentContainerType = flockAgent.agentContainerType;

            var rect = flockAgent.GetComponent<RectTransform>();


            //  获取卡片生成位置
            Vector3 cardGenPosition = new Vector3(rect.anchoredPosition.x - _manager.PanelOffsetX - 1f,
                    rect.anchoredPosition.y - _manager.PanelOffsetY - 1f,
                    200);

            if (_agentContainerType == AgentContainerType.MainPanel)
            {
                cardGenPosition = new Vector3(rect.anchoredPosition.x - _manager.PanelOffsetX - 1f, rect.anchoredPosition.y - _manager.PanelOffsetY - 1f, 200);
            }
            else if (_agentContainerType == AgentContainerType.BackPanel)
            {
                cardGenPosition = new Vector3(rect.anchoredPosition.x - _manager.PanelBackOffsetX - 1f, rect.anchoredPosition.y - _manager.PanelOffsetY - 1f, 200);
            }
            else if (_agentContainerType == AgentContainerType.StarContainer)
            {
                var flockTransform = flockAgent.transform;

                // 获取屏幕坐标
                Vector2 v = RectTransformUtility.WorldToScreenPoint(_manager.starCamera, flockTransform.position);

                // 需要屏幕坐标转为某UGUI容器内的坐标

                Vector2 refp;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(_manager.OperationPanel, v, null, out refp);

                refp = new Vector2(refp.x + _manager.OperationPanel.rect.width / 2, refp.y + _manager.OperationPanel.rect.height / 2);

                cardGenPosition = refp;
            }

            return cardGenPosition;
        }






        /// <summary>
        ///     移除依附的卡片
        ///     ref ： https://www.yuque.com/u314548/fc6a5l/dozp0e
        /// </summary>
        /// <param name="flockAgent"></param>
        private void RemoveRefCard(FlockAgent flockAgent) {
            if (flockAgent.flockStatus == FlockStatusEnum.TOHIDE)
            {
                // 停止移动与缩小动画

                flockAgent.flockTweenerManager.Get(FlockTweenerManager.Kinnect_Choose_Move).Kill();
                flockAgent.flockTweenerManager.Get(FlockTweenerManager.Kinnect_Choose_Scale).Kill();                
                flockAgent.RecoverToOriginPosition();

                //Debug.Log(flockAgent.gameObject.name + " status TOHIDE: " + flockAgent.flockStatus);

            }
            else if (flockAgent.flockStatus == FlockStatusEnum.HIDE) {
                if (flockAgent.GetCardAgent.CardStatus == CardStatusEnum.GENERATE)
                {
                    // 停止放大动画并关闭
                    flockAgent.GetCardAgent.SetDisableEffect(true);
                    flockAgent.GetCardAgent.CancelGoToFront(() => { });
                    flockAgent.GetCardAgent.DoCloseDirect();
                }
                else {
                    flockAgent.GetCardAgent.SetDisableEffect(true);
                    flockAgent.GetCardAgent.DoCloseDirect();
                }
            }
        }



    }
}


