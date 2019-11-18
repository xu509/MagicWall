
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

/// <summary>
///     星空场景
/// </summary>
namespace MagicWall
{
    public class StarScene : IScene
    {
        private MagicWallManager _manager;
        private IDaoService _daoService;
        private float _durtime; // 持续时间
        private DataTypeEnum _dataType; //  场景内容类型
        private SceneUtils _sceneUtil;

        private float _startTime;   //  开始时间
        private bool _isPreparing;   //是否准备完成
        private bool _isEnding;   // 正在执行关闭动画

        private List<FlockAgent> _activeAgents; //活动的 Agents 
        private StarSceneStatusEnum _starSceneStatusEnum;   // 状态

        MagicSceneEnum _magicSceneEnumStatus;

        Action _onSceneCompleted;

        SceneConfig _sceneConfig;



        enum StarSceneStatusEnum
        {
            Init,
            InitCompleted,
            Run,
            RunCompleted,
            End,
            EndCompleted
        }

        public void Init(SceneConfig sceneConfig, MagicWallManager manager,Action onSceneCompleted)
        {
            _manager = manager;
            //_daoService = manager.daoService;
            _durtime = sceneConfig.durtime;
            _dataType = sceneConfig.dataType;
            _daoService = _manager.daoServiceFactory.GetDaoService(sceneConfig.daoTypeEnum);
            //_itemFactory = manager.itemsFactoryAgent.GetItemsFactoryByContentType(_dataType);
            _sceneUtil = new SceneUtils(_manager, sceneConfig.isKinect);

            _onSceneCompleted = onSceneCompleted;
            _sceneConfig = sceneConfig;
        }

        /// <summary>
        ///  持续运行,主流程处理块
        /// </summary>
        /// <returns></returns>
        public bool Run()
        {
            //Debug.Log("_starSceneStatusEnum : " + _starSceneStatusEnum);
            _magicSceneEnumStatus = MagicSceneEnum.Running;

            if (_starSceneStatusEnum == StarSceneStatusEnum.Init)
            {
                if (!_isPreparing)
                {
                    _isPreparing = true;
                    _activeAgents = new List<FlockAgent>();
                    _startTime = Time.time;

                    DoPrepare();

                }
            }

            if (_starSceneStatusEnum == StarSceneStatusEnum.InitCompleted)
            {
                _starSceneStatusEnum = StarSceneStatusEnum.Run;
            }

            if (_starSceneStatusEnum == StarSceneStatusEnum.Run)
            {
                //Debug.Log("[Star] Running");

                DoAnimation();
            }

            if ((_starSceneStatusEnum == StarSceneStatusEnum.Run) && ((Time.time - _startTime) > _durtime))
            {
                //_starSceneStatusEnum = StarSceneStatusEnum.RunCompleted;
                OnRunCompleted();
            }


            return true;
        }


        private void DoPrepare()
        {
            //Debug.Log("[Star] Do Prepare | _durtime: " + _durtime + " / _dataType: " + _dataType);
            _startTime = Time.time;


            _manager.starEffectContainer.gameObject.SetActive(true);
            _manager.starEffectContent.GetComponent<CanvasGroup>().alpha = 0;


            // 创建浮动块
            for (int i = 0; i < _manager.cutEffectConfig.StarEffectAgentsCount; i++)
            {
                CreateNewAgent(true);
            }

            // 显示动画
            _manager.starEffectContent.GetComponent<CanvasGroup>().DOFade(1, 1f);


            // 设置远近关系，Z轴越小越前面
            _activeAgents.Sort(new FlockCompare());
            for (int i = 0; i < _activeAgents.Count; i++)
            {
                int si = _activeAgents.Count - 1 - i;
                _activeAgents[i].GetComponent<RectTransform>().SetSiblingIndex(si);
            }

            //Debug.Log("[Star] Do Prepare Complete");

            _isPreparing = false;
            _starSceneStatusEnum = StarSceneStatusEnum.InitCompleted;

            //Debug.Log("_activeAgents : " + _activeAgents.Count);


        }


        /// <summary>
        /// 执行动画效果
        /// </summary>
        private void DoAnimation()
        {

            List<FlockAgent> agentsNeedClear = new List<FlockAgent>();

            for (int i = 0; i < _activeAgents.Count; i++)
            {

                if (_activeAgents[i].GetComponent<RectTransform>().anchoredPosition3D.z < _manager.cutEffectConfig.StarEffectEndPoint)
                {
                    //  清理agent，
                    agentsNeedClear.Add(_activeAgents[i]);
                    //  创建新 agent
                    FlockAgent agent = CreateNewAgent(false);
                    agent.GetComponent<RectTransform>().SetAsFirstSibling();

                    //Debug.Log("Create star card!");

                }
                else
                {
                    // 移动
                    Vector3 to = new Vector3(0, 0, -(Time.deltaTime * _manager.cutEffectConfig.StarEffectMoveFactor));
                    _activeAgents[i].GetComponent<RectTransform>().transform.Translate(to);

                    // 更新透明度
                    UpdateAlpha(_activeAgents[i]);
                }
            }
            //Debug.Log(_activeAgents.Count);
            for (int i = 0; i < agentsNeedClear.Count; i++)
            {
                ClearAgent(agentsNeedClear[i]);
                //TODO有问题
                _activeAgents.Remove(agentsNeedClear[i]);
            }

        }


        private FlockAgent CreateNewAgent(bool randomZ)
        {
            if (!randomZ) {
                //Debug.Log("添加星空块。");
            }


            // 获取数据
            //FlockData data = _daoService.GetFlockData(_dataType,_manager);\

            FlockData data = _daoService.GetFlockDataByScene(_dataType,_manager.SceneIndex);

            // 获取出生位置
            Vector2 randomPosition = UnityEngine.Random.insideUnitSphere;

            Vector3 position = new Vector3();

            position.x = (randomPosition.x / 2 + 0.5f) * _manager.GetScreenRect().x;
            position.y = (randomPosition.y / 2 + 0.5f) * _manager.GetScreenRect().y;


            // 获取长宽
            Sprite logoSprite = data.GetCoverSprite();
            float width = _sceneUtil.ResetTexture(new Vector2(logoSprite.rect.width, logoSprite.rect.height)).x;
            float height = _sceneUtil.ResetTexture(new Vector2(logoSprite.rect.width, logoSprite.rect.height)).y;

            //FlockAgent go = _itemFactory.Generate(position.x, position.y, position.x, position.y, 0, 0,
            // width, height, data, AgentContainerType.StarContainer);
            FlockAgent go = FlockAgentFactoryInstance.Generate(_manager,position, AgentContainerType.StarContainer,
                position.x,position.y,0,0,width,height,data, _sceneConfig.daoTypeEnum);

            go.UpdateImageAlpha(0);

            // 星空效果不会被物理特效影响
            //go.CanEffected = false;
            go.flockStatus = FlockStatusEnum.STAR;
            go.isStarEffect = true;

            // 设置Z轴

            float z;
            if (randomZ)
            {
                z = Mathf.Lerp(_manager.cutEffectConfig.StarEffectOriginPoint, _manager.cutEffectConfig.StarEffectEndPoint,
                    UnityEngine.Random.Range(0f, 1f));
            }
            else
            {
                z = _manager.cutEffectConfig.StarEffectOriginPoint;
            }

            go.GetComponent<RectTransform>().anchoredPosition3D = go.GetComponent<RectTransform>().anchoredPosition3D + new Vector3(0, 0, z);
            go.Z = z;
            go.name = "Agent-" + Mathf.RoundToInt(go.Z);

            _activeAgents.Add(go);

            return go;
        }



        /// <summary>
        ///     清理agent
        /// </summary>
        /// <param name="agent"></param>
        private void ClearAgent(FlockAgent agent)
        {
            // 清理出实体袋
            agent.flockStatus = FlockStatusEnum.OBSOLETE;    
            
        }



        /// <summary>
        ///     更新透明度
        /// </summary>
        /// <param name="agent"></param>
        private void UpdateAlpha(FlockAgent agent)
        {
            float z = agent.GetComponent<RectTransform>().anchoredPosition3D.z;

            // 判断Z在距离中的位置
            float distance = Mathf.Abs(_manager.cutEffectConfig.StarEffectOriginPoint - _manager.cutEffectConfig.StarEffectEndPoint);
            float offset = Mathf.Abs(z - _manager.cutEffectConfig.StarEffectOriginPoint) / distance;

            // 当OFFSET 位于前 1/10 或后 1/10 时，更新透明度
            if (offset < 0.05)
            {
                float k = Mathf.Abs(offset - 0.05f);
                float alpha = Mathf.Lerp(1, 0, k / 0.05f);
                agent.UpdateImageAlpha(alpha);
            }
            else if (offset > 0.95)
            {
                float k = Mathf.Abs(1 - offset);
                float alpha = Mathf.Lerp(0, 1, k / 0.05f);
                agent.UpdateImageAlpha(alpha);
            }
            else
            {
                agent.UpdateImageAlpha(1);
            }
        }


        /// <summary>
        ///     实体比较器
        /// </summary>
        class FlockCompare : IComparer<FlockAgent>
        {
            public int Compare(FlockAgent x, FlockAgent y)
            {
                return x.Z.CompareTo(y.Z);
            }
        }


        public void OnRunCompleted()
        {
            if (!_isEnding)
            {
                _isEnding = true;
                //_magicSceneEnumStatus = MagicSceneEnum.RunningEnd;

                _manager.starEffectContent.GetComponent<CanvasGroup>()
                    .DOFade(0, 2f)
                    .OnComplete(() =>
                    {
                        _manager.starEffectContainer.gameObject.SetActive(false);
                        _onSceneCompleted.Invoke();
                        _starSceneStatusEnum = StarSceneStatusEnum.Init;
                        _magicSceneEnumStatus = MagicSceneEnum.Running;

                        _isPreparing = false;
                        _isEnding = false;
                        //_starSceneStatusEnum = StarSceneStatusEnum.EndCompleted;
                    });


            }


        }



        public MagicSceneEnum GetSceneStatus()
        {

            return _magicSceneEnumStatus;

        }

        DataTypeEnum IScene.GetDataType()
        {
            throw new NotImplementedException();
        }

        public void RunEnd(Action onEndCompleted)
        {
            throw new NotImplementedException();
        }

        public SceneConfig GetSceneConfig()
        {
            return _sceneConfig;
        }
    }
}