using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 过场效果 3 从后往前, 星空效果
public class StarsCutEffect : CutEffect
{
    private int row;
    private int column;

    private float _distance;

    private float generate_agent_interval = 0.5f; // 生成的间隔
    private float last_generate_time = 0f; // 最后生成的时间
    private float animation_duration;//动画持续时间

    private DisplayBehaviorConfig _displayBehaviorConfig;   //  Display Behavior Config


    //
    //  Init
    //
    public override void Init(MagicWallManager manager)
    {
        //  初始化 manager
        _manager = manager;
        _agentManager = _manager.agentManager;
        _daoService = DaoService.Instance;

        //  获取动画的持续时间
        StartingDurTime = 17f;
        DestoryDurTime = 0.5f;

        //  设置显示的时间
        string t = _daoService.GetConfigByKey(AppConfig.KEY_CutEffectDuring_Stars).Value;
        DisplayDurTime = AppUtils.ConvertToFloat(t);

        //  设置销毁
        DestoryBehavior = new FadeOutDestoryBehavior();
        DestoryBehavior.Init(_manager, DestoryDurTime);

        //  设置运行时间点
        HasDisplaying = false;


        //  初始化 config
        _displayBehaviorConfig = new DisplayBehaviorConfig();

        _distance = _manager.managerConfig.StarEffectDistance;
        animation_duration = _manager.managerConfig.StarEffectDistanceTime;
    }

    //
    //  创建产品 | Logo 
    //
    protected override void CreateLogo()
    {
        CreateAgency(DataType.env);
    }

    //
    //  创建活动
    //
    protected override void CreateActivity()
    {
        CreateAgency(DataType.activity);
    }

    /// <summary>
    ///     创建产品
    /// </summary>
    protected override void CreateProduct()
    {
        CreateAgency(DataType.product);

    }


    public override void Starting() {
        if (Time.time - last_generate_time > generate_agent_interval) {

            // 随机选择
            int count = _manager.agentManager.Agents.Count;
            for (int i = 0; i < 15; i++)
            {
                int index = Random.Range(0, count);
                FlockAgent agent = _agentManager.Agents[index];
                if (!agent.StarsCutEffectIsPlaying) {
                    agent.StarsCutEffectIsPlaying = true;
                    agent.gameObject.SetActive(true);
                    agent.GetComponent<RectTransform>().SetAsFirstSibling();
                    //agent.transform.SetAsLastSibling();

                    Vector3 to = new Vector3(agent.OriVector2.x, agent.OriVector2.y, 0);
                    agent.GetComponent<RectTransform>().DOAnchorPos3DZ(0, animation_duration)
                        .OnComplete(() => DOAnchorPosCompleteCallback(agent));
                    //agent.GetComponent<Image>().DOFade(0.2f, animation_duration).From();
                }
                //agent.GetComponent<RectTransform>().DOScale(new Vector3(0.5f, 0.5f, 0.5f), animation_duration).From();
            }

            last_generate_time = Time.time;
        }
    }

    public override void OnStartingCompleted(){
    }


    private void CreateAgency(DataType dataType) {
        // 获取栅格信息
        row = _manager.Row;
        int h = (int)_manager.mainPanel.rect.height;
        int w = (int)_manager.mainPanel.rect.width;

        int itemHeight = _sceneUtil.GetFixedItemHeight();
        column = _sceneUtil.GetColumnNumberByFixedWidth(itemHeight);


        //从左往右，从上往下
        for (int j = 0; j < column; j++)
        {
            for (int i = 0; i < row; i++)
            {
                Vector2 oriVector = _sceneUtil.GetPositionOfSquareItem(itemHeight, i, j);

                FlockData data = _daoService.GetFlockData(dataType);
                Sprite logoSprite = data.GetCoverSprite();

                Vector2 vector2 = AppUtils
                   .ResetTexture(new Vector2(logoSprite.rect.width
                   , logoSprite.rect.height), _manager.displayFactor);


                // ori_x;ori_y
                float ori_x, ori_y;
                ori_x = oriVector.x;
                ori_y = oriVector.y;

                FlockAgent go = ItemsFactory.Generate(ori_x, ori_y, ori_x, ori_y, i, j,
                    vector2.x, vector2.y, data, AgentContainerType.MainPanel);
                //go.NextVector2 = new Vector2(ori_x, ori_y);

                // 将agent的z轴定义在后方
                go.GetComponent<RectTransform>().anchoredPosition3D = go.GetComponent<RectTransform>().anchoredPosition3D + new Vector3(0, 0, _distance);
                go.gameObject.SetActive(false);

                // 星空效果不会被物理特效影响
                go.CanEffected = false;
            }
        }
    }



    #region Tween Callback
    public void DOAnchorPosCompleteCallback(FlockAgent agent)
    {
        if (!agent.IsChoosing)
        {
            agent.GetComponent<Image>().DOFade(0, 0.5f).OnComplete(() => DOFadeCompleteCallback(agent));
        }
    }

    public void DOFadeCompleteCallback(FlockAgent agent)
    {
        RectTransform rect = agent.GetComponent<RectTransform>();

        agent.UpdateImageAlpha(1);
        agent.StarsCutEffectIsPlaying = false;

        // 此时会出现场景已经切换,但是动画并未停止的问题
        if (agent.SceneIndex == _manager.SceneIndex)
        {
            rect.anchoredPosition3D = new Vector3(agent.OriVector2.x, agent.OriVector2.y, _distance);
            agent.gameObject.SetActive(false);
        }
        else {
            //agent.Reset();
        }
    }


    #endregion

    public override string GetID()
    {
        return "StarsCutEffect";
    }

}
