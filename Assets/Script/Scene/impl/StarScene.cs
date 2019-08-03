using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

/// <summary>
///     星空场景
/// </summary>
public class StarScene : IScene
{
    private MagicWallManager _manager;
    private IDaoService _daoService;
    private float _durtime; // 持续时间
    private DataType _dataType; //  场景内容类型
    private ItemsFactory _itemFactory;  // 工厂
    private SceneUtils _sceneUtil;

    private float _startTime;   //  开始时间
    private bool _isPreparing;   //是否准备完成
    private bool _isEnding;   // 正在执行关闭动画

    private List<FlockAgent> _activeAgents; //活动的 Agents 
    private StarSceneStatusEnum _starSceneStatusEnum = StarSceneStatusEnum.Init;   // 状态

    Action _onRunCompleted;
    Action _onRunEndCompleted;


    enum StarSceneStatusEnum {
        Init,
        InitCompleted,
        Run,
        RunCompleted,
        End,
        EndCompleted
    }



    private void Reset() {
        _starSceneStatusEnum = StarSceneStatusEnum.Init;
        _isEnding = false;
        _isPreparing = false;
        _startTime = Time.time;
        _activeAgents = new List<FlockAgent>();
    }

    public DataType GetDataType()
    {
        return _dataType;
    }

    public void Init(SceneConfig sceneConfig, MagicWallManager manager)
    {
        _manager = manager;
        _daoService = manager.daoService;
        _durtime = sceneConfig.durtime;
        _dataType = sceneConfig.dataType;
        _itemFactory = manager.itemsFactoryAgent.GetItemsFactoryByContentType(_dataType);
        _sceneUtil = new SceneUtils(_manager);


        Reset();
    }

    /// <summary>
    ///  持续运行,主流程处理块
    /// </summary>
    /// <returns></returns>
    public bool Run()
    {
        if (_starSceneStatusEnum == StarSceneStatusEnum.Init) {
            if (!_isPreparing)
            {
                _isPreparing = true;
                DoPrepare();
            }
        }

        if (_starSceneStatusEnum == StarSceneStatusEnum.InitCompleted) {
            _starSceneStatusEnum = StarSceneStatusEnum.Run;
        }

        if (_starSceneStatusEnum == StarSceneStatusEnum.Run)
        {
            DoAnimation();
        }

        if (_starSceneStatusEnum == StarSceneStatusEnum.RunCompleted)
        {
            _starSceneStatusEnum = StarSceneStatusEnum.End;
        }

        if (_starSceneStatusEnum == StarSceneStatusEnum.End)
        {
            if (!_isEnding)
            {
                _isEnding = true;
                DoEnd();
            }
        }

        if (_starSceneStatusEnum == StarSceneStatusEnum.EndCompleted)
        {
            Reset();
            return false;
        }

        if ((_starSceneStatusEnum == StarSceneStatusEnum.Run) && ((Time.time - _startTime) > _durtime)) {
            _starSceneStatusEnum = StarSceneStatusEnum.RunCompleted;
        }

        return true;
    }


    private void DoPrepare() {
        //Debug.Log("Do Prepare");
        _startTime = Time.time;


        _manager.starEffectContainer.gameObject.SetActive(true);
        _manager.starEffectContent.GetComponent<CanvasGroup>().alpha = 0;


        // 创建浮动块
        for (int i = 0; i < _manager.managerConfig.StarEffectAgentsCount; i++)
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


        _starSceneStatusEnum = StarSceneStatusEnum.InitCompleted;
    }


    /// <summary>
    /// 执行动画效果
    /// </summary>
    private void DoAnimation()
    {

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

    private void DoEnd()
    {
        //Debug.Log("Do End");


        // 淡出
        _manager.starEffectContent.GetComponent<CanvasGroup>()
            .DOFade(0, 2f)
            .OnComplete(() => {
                _manager.starEffectContainer.gameObject.SetActive(false);
                _manager.Clear();
                _starSceneStatusEnum = StarSceneStatusEnum.EndCompleted;
            });

    }


    private FlockAgent CreateNewAgent(bool randomZ)
    {
        // 获取数据
        FlockData data = _daoService.GetFlockData(_dataType);

        // 获取出生位置
        Vector2 randomPosition = UnityEngine.Random.insideUnitSphere;

        Vector3 position;

        position.x = (randomPosition.x / 2 + 0.5f) * _manager.GetScreenRect().x;
        position.y = (randomPosition.y / 2 + 0.5f) * _manager.GetScreenRect().y;


        // 获取长宽
        Sprite logoSprite = data.GetCoverSprite();
        float width = _sceneUtil.ResetTexture(new Vector2(logoSprite.rect.width, logoSprite.rect.height)).x;
        float height = _sceneUtil.ResetTexture(new Vector2(logoSprite.rect.width, logoSprite.rect.height)).y;

        FlockAgent go = _itemFactory.Generate(position.x, position.y, position.x, position.y, 0, 0,
         width, height, data, AgentContainerType.StarContainer);
        go.UpdateImageAlpha(0);

        // 星空效果不会被物理特效影响
        go.CanEffected = false;

        // 设置Z轴

        float z;
        if (randomZ)
        {
            z = Mathf.Lerp(_manager.managerConfig.StarEffectOriginPoint, _manager.managerConfig.StarEffectEndPoint,
                UnityEngine.Random.Range(0f, 1f));
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
        _activeAgents.Remove(agent);
        _manager.agentManager.ClearAgent(agent);
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
    }

    public void SetOnRunCompleted(Action onRunCompleted)
    {
        _onRunCompleted = onRunCompleted;
    }

    public void RunEnd()
    {
        throw new NotImplementedException();
    }

    public void OnRunEndCompleted()
    {
    }

    public void SetOnRunEndCompleted(Action onRunEndCompleted)
    {
        _onRunEndCompleted = onRunEndCompleted;
    }

}
