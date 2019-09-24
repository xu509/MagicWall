using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 过场效果 3 从后往前, 星空效果
namespace MagicWall
{

    /// <summary>
    /// 已弃用
    /// </summary>
    public class StarsCutEffect : CutEffect
    {
        private bool doStartEffect = false;
        private DataTypeEnum _dataType;
        private List<FlockAgent> _activeAgents; //活动的 Agents 


        //
        //  Init
        //
        public override void Init(MagicWallManager manager)
        {
            //  初始化 manager
            _manager = manager;
            _agentManager = _manager.agentManager;
            _daoService = manager.daoService;
            doStartEffect = false;

            //  获取动画的持续时间
            StartingDurTime = 20f;
            DestoryDurTime = 0.5f;

            //  设置显示的时间
            string t = _daoService.GetConfigByKey(AppConfig.KEY_CutEffectDuring_Stars).Value;
            DisplayDurTime = AppUtils.ConvertToFloat(t);

            //  设置销毁
            DestoryBehavior = new FadeOutDestoryBehavior();
            DestoryBehavior.Init(_manager, DestoryDurTime);

            //  设置运行时间点
            HasDisplaying = false;

            _activeAgents = new List<FlockAgent>();



        }



        public override void Starting()
        {

            if (!doStartEffect)
            {
                doStartEffect = true;
                _manager.mainPanel.GetComponent<CanvasGroup>().DOFade(1, 1f);
            }


            List<FlockAgent> agentsNeedClear = new List<FlockAgent>();

            for (int i = 0; i < _activeAgents.Count; i++)
            {
                if (_activeAgents[i].GetComponent<RectTransform>().anchoredPosition3D.z < _manager.managerConfig.StarEffectEndPoint)
                {
                    //  清理agent，
                    agentsNeedClear.Add(_activeAgents[i]);
                    //  创建新 agent
                    FlockAgent agent = CreateNewAgent(false);
                    agent.GetComponent<RectTransform>().SetAsFirstSibling();
                }
                else
                {
                    // 移动
                    Vector3 to = new Vector3(0, 0, -(Time.deltaTime * _manager.managerConfig.StarEffectMoveFactor));
                    _activeAgents[i].GetComponent<RectTransform>().transform.Translate(to);

                    // 更新透明度
                    UpdateAlpha(_activeAgents[i]);
                }
            }

            for (int i = 0; i < agentsNeedClear.Count; i++)
            {
                ClearAgent(agentsNeedClear[i]);

            }

        }



        public override void OnStartingCompleted()
        {
            Debug.Log("OnStartingCompleted");
        }

        /// <summary>
        ///     初始状态
        ///         -   内容此时有进行至一半
        /// </summary>
        /// <param name="dataType"></param>
        private void CreateAgency()
        {
            // 随机生成
            for (int i = 0; i < _manager.managerConfig.StarEffectAgentsCount; i++)
            {
                CreateNewAgent(true);
            }

            // 设置远近关系，Z轴越小越前面
            _activeAgents.Sort(new FlockCompare());
            for (int i = 0; i < _activeAgents.Count; i++)
            {
                int si = _activeAgents.Count - 1 - i;
                _activeAgents[i].GetComponent<RectTransform>().SetSiblingIndex(si);
            }

            _manager.mainPanel.GetComponent<CanvasGroup>().alpha = 0;
        }



        public override string GetID()
        {
            return "StarsCutEffect";
        }



        private FlockAgent CreateNewAgent(bool randomZ)
        {

            // 获取数据
            FlockData data = _daoService.GetFlockData(_dataType);

            // 获取出生位置
            Vector2 randomPosition = Random.insideUnitSphere;

            Vector3 position;

            position.x = (randomPosition.x / 2 + 0.5f) * _manager.GetScreenRect().x;
            position.y = (randomPosition.y / 2 + 0.5f) * _manager.GetScreenRect().y;


            // 获取长宽
            Sprite logoSprite = data.GetCoverSprite();
            float width = _sceneUtil.ResetTexture(new Vector2(logoSprite.rect.width, logoSprite.rect.height)).x;
            float height = _sceneUtil.ResetTexture(new Vector2(logoSprite.rect.width, logoSprite.rect.height)).y;

            FlockAgent go = null;
            go.UpdateImageAlpha(0);

            // 星空效果不会被物理特效影响
            go.CanEffected = false;

            // 设置Z轴

            float z;
            if (randomZ)
            {
                z = Mathf.Lerp(_manager.managerConfig.StarEffectOriginPoint, _manager.managerConfig.StarEffectEndPoint, Random.Range(0f, 1f));
            }
            else
            {
                z = _manager.managerConfig.StarEffectOriginPoint;
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
            float distance = Mathf.Abs(_manager.managerConfig.StarEffectOriginPoint - _manager.managerConfig.StarEffectEndPoint);
            float offset = Mathf.Abs(z - _manager.managerConfig.StarEffectOriginPoint) / distance;

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

        protected override void CreateAgents(DataTypeEnum dataType)
        {
            throw new System.NotImplementedException();
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


    }
}