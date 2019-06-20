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
        // 获取栅格信息
        row = _manager.Row;
        int h = (int)_manager.mainPanel.rect.height;
        int w = (int)_manager.mainPanel.rect.width;

        int gap = 10;

        int itemWidth = h / row - gap;
        int itemHeight = itemWidth;

        // 从后往前的效果列数不需要很多
        column = w / itemWidth;

        //从左往右，从上往下
        for (int j = 0; j < column; j++)
        {
            for (int i = 0; i < row; i++)
            {


                float x = j * (itemWidth + gap) + itemWidth / 2;
                float y = i * (itemHeight + gap) + itemHeight / 2;

                Enterprise env = _manager.daoService.GetEnterprise();
                

                Vector2 vector2 = AppUtils
                    .ResetTexture(new Vector2(TextureResource.Instance.GetTexture(MagicWallManager.FileDir + "\\logo\\" + env.Logo).width
                    , TextureResource.Instance.GetTexture(MagicWallManager.FileDir + "\\logo\\" + env.Logo).height), _manager.displayFactor);

                int middleX = (column - 1) / 2;

                // ori_x;ori_y
                float ori_x, ori_y;
                ori_x = x;
                ori_y = y;

                Vector2 ori_position = new Vector2(ori_x, ori_y);
                Vector2 gen_position = new Vector2(x, y);

                //				FlockAgent go = AgentGenerator.GetInstance ().generator (name, gen_position, ori_position, magicWallManager);
                FlockAgent go = ItemsFactory.Generate(ori_x, ori_y, x, y, i, j, vector2.x, vector2.y, env, _manager.mainPanel);
                // 星空效果不会被物理特效影响
                go.CanEffected = false;

                // 将agent的z轴定义在后方
                RectTransform rect = go.GetComponent<RectTransform>();
                rect.anchoredPosition3D = rect.anchoredPosition3D + new Vector3(0, 0, _distance);

                go.gameObject.SetActive(false);
            }
        }
    }

    //
    //  创建活动
    //
    protected override void CreateActivity()
    {
        // 获取栅格信息
        row = _manager.Row;
        int h = (int)_manager.mainPanel.rect.height;
        int w = (int)_manager.mainPanel.rect.width;

        int gap = 10;

        int itemWidth = h / row - gap;
        int itemHeight = itemWidth;

        // 从后往前的效果列数不需要很多
        column = w / itemWidth;

        //从左往右，从上往下
        for (int j = 0; j < column; j++)
        {
            for (int i = 0; i < row; i++)
            {
                float x = j * (itemWidth + gap) + itemWidth / 2;
                float y = i * (itemHeight + gap) + itemHeight / 2;
                
                Activity activity = _daoService.GetActivity();
                Vector2 vector2 = AppUtils.ResetTexture(new Vector2(activity.TextureImage.width, activity.TextureImage.height)
                    , _manager.displayFactor);

                int middleX = (column - 1) / 2;

                // ori_x;ori_y
                float ori_x, ori_y;
                ori_x = x;
                ori_y = y;

                Vector2 ori_position = new Vector2(ori_x, ori_y);
                Vector2 gen_position = new Vector2(x, y);

                //				FlockAgent go = AgentGenerator.GetInstance ().generator (name, gen_position, ori_position, magicWallManager);
                FlockAgent go = ItemsFactory.Generate(ori_x, ori_y, x, y, i , j , vector2.x, vector2.y, activity, _manager.mainPanel);
            
                // 星空效果不会被物理特效影响
                go.CanEffected = false;

                // 将agent的z轴定义在后方
                RectTransform rect = go.GetComponent<RectTransform>();
                rect.anchoredPosition3D = rect.anchoredPosition3D + new Vector3(0, 0, _distance);

                go.gameObject.SetActive(false);
            }
        }
    }

    
    public override void Starting() {
        if (Time.time - last_generate_time > generate_agent_interval) {

            // 随机选择
            int count = _manager.agentManager.Agents.Count;
            for (int i=0; i<15; i++)
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
                    agent.GetComponent<RawImage>().DOFade(0.2f, animation_duration).From();
                }
                //agent.GetComponent<RectTransform>().DOScale(new Vector3(0.5f, 0.5f, 0.5f), animation_duration).From();
            }

            last_generate_time = Time.time;
        }



    }

    public override void OnStartingCompleted(){
    }


    #region Tween Callback
    public void DOAnchorPosCompleteCallback(FlockAgent agent)
    {
        RectTransform rect = agent.GetComponent<RectTransform>();
        RawImage image = agent.GetComponent<RawImage>();
        if (!agent.IsChoosing)
        {
            //rect.DOScale(1.5f, 0.2f);
            image.DOFade(0, 0.5F).OnComplete(() => DOFadeCompleteCallback(agent));
            //foreach (RawImage rawImage in agent.GetComponentsInChildren<RawImage>())
            //{
            //    rawImage.DOFade(0, 1).OnComplete(() => DOFadeCompleteCallback(agent));
             
            //}

        }
    }

    public void DOFadeCompleteCallback(FlockAgent agent)
    {
        RectTransform rect = agent.GetComponent<RectTransform>();
        RawImage image = agent.GetComponent<RawImage>();
        //rect.DOScale(1f, Time.deltaTime);
        image.DOFade(1, 0);
        rect.anchoredPosition3D = new Vector3(agent.OriVector2.x, agent.OriVector2.y, _distance);
        agent.StarsCutEffectIsPlaying = false;
        agent.GetComponent<RawImage>().DOFade(1, 0);

        agent.gameObject.SetActive(false);

        //foreach (RawImage rawImage in agent.GetComponentsInChildren<RawImage>())
        //{
        //    rawImage.DOFade(1, 0);

        //}
    }

    protected override void CreateProduct()
    {
        // 获取栅格信息
        row = _manager.Row;
        int h = (int)_manager.mainPanel.rect.height;
        int w = (int)_manager.mainPanel.rect.width;

        int gap = 10;

        int itemWidth = h / row - gap;
        int itemHeight = itemWidth;

        // 从后往前的效果列数不需要很多
        column = w / itemWidth;

        //从左往右，从上往下
        for (int j = 0; j < column; j++)
        {
            for (int i = 0; i < row; i++)
            {
                float x = j * (itemWidth + gap) + itemWidth / 2;
                float y = i * (itemHeight + gap) + itemHeight / 2;

                Product product = _daoService.GetProduct();
                Vector2 vector2 = AppUtils.ResetTexture(new Vector2(product.TextureImage.width, product.TextureImage.height)
                    , _manager.displayFactor);

                int middleX = (column - 1) / 2;

                // ori_x;ori_y
                float ori_x, ori_y;
                ori_x = x;
                ori_y = y;

                Vector2 ori_position = new Vector2(ori_x, ori_y);
                Vector2 gen_position = new Vector2(x, y);

                //				FlockAgent go = AgentGenerator.GetInstance ().generator (name, gen_position, ori_position, magicWallManager);
                FlockAgent go = ItemsFactory.Generate(ori_x, ori_y, x, y, i, j, vector2.x, vector2.y, product, _manager.mainPanel);

                // 星空效果不会被物理特效影响
                go.CanEffected = false;

                // 将agent的z轴定义在后方
                RectTransform rect = go.GetComponent<RectTransform>();
                rect.anchoredPosition3D = rect.anchoredPosition3D + new Vector3(0, 0, _distance);

                go.gameObject.SetActive(false);
            }
        }
    }


    #endregion

}
