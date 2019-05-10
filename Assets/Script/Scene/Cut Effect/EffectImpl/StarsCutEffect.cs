using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 过场效果 3 从后往前, 星空效果
public class StarsCutEffect : CutEffect
{
    MagicWallManager manager;

    private int row;
    private int column;

    private float generate_agent_interval = 0.05f; // 生成的间隔
    private float last_generate_time = 0f; // 最后生成的时间
    private float animation_duration = 3f;//动画持续时间

    private DisplayBehaviorConfig _displayBehaviorConfig;   //  Display Behavior Config


    //
    //  Init
    //
    protected override void Init()
    {
        //  获取动画的持续时间
        StartingDurTime = 17f;
        DestoryDurTime = 0.5f;

        //  设置显示的时间
        string t = DaoService.Instance.GetConfigByKey(AppConfig.KEY_CutEffectDuring_Stars).Value;
        DisplayDurTime = AppUtils.ConvertToFloat(t);

        //  设置销毁
        DestoryBehavior = new FadeOutDestoryBehavior();

        //  设置运行时间点
        HasDisplaying = false;

        //  初始化 manager
        manager = MagicWallManager.Instance;

        //  初始化 config
        _displayBehaviorConfig = new DisplayBehaviorConfig();
    }

    //
    //  创建产品 | Logo 
    //
    protected override void CreateProductOrLogo()
    {
        // 获取栅格信息
        row = manager.row;
        int h = (int)manager.mainPanel.rect.height;
        int w = (int)manager.mainPanel.rect.width;

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

                Enterprise env = manager.daoService.GetEnterprise();
                Vector2 vector2 = ResetTexture(new Vector2(env.TextureLogo.width, env.TextureLogo.height));

                int middleX = (column - 1) / 2;

                // ori_x;ori_y
                float ori_x, ori_y;
                ori_x = x;
                ori_y = y;

                Vector2 ori_position = new Vector2(ori_x, ori_y);
                Vector2 gen_position = new Vector2(x, y);

                //				FlockAgent go = AgentGenerator.GetInstance ().generator (name, gen_position, ori_position, magicWallManager);
                FlockAgent go = ItemsFactory.Generate(ori_x, ori_y, x, y, i, j, vector2.x, vector2.y, DaoService.Instance.GetEnterprise());
                // 星空效果不会被物理特效影响
                go.CanEffected = false;

                // 将agent的z轴定义在后方
                //RectTransform rect = go.GetComponent<RectTransform>();
                //Vector3 position = rect.anchoredPosition3D;
                //rect.anchoredPosition3D = rect.anchoredPosition3D + new Vector3(0, 0, 3000);

                go.gameObject.SetActive(false);
            }
        }
    }

    Vector2 ResetTexture(Vector2 size)
    {
        print(111);
        //图片宽高
        float w = size.x;
        float h = size.y;
        //组件宽高
        float width;
        float height;
        //if (w > 600 || h > 400)
        //{
        //    w *= 0.9f;
        //    h *= 0.9f;
        //    ResetTexture(new Vector2(w, h));
        //}
        if (w >= h)
        {
            //宽固定
            width = Random.Range(300, 600);
            height = h / w * width;
        }   else
        {
            //高固定
            height = Random.Range(200, 400);
            width = w / h * height;
        }
        return new Vector2(width, height);
    }

    //
    //  创建活动
    //
    protected override void CreateActivity()
    {
        // 获取栅格信息
        row = manager.row;
        int h = (int)manager.mainPanel.rect.height;
        int w = (int)manager.mainPanel.rect.width;

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

                Enterprise env = manager.daoService.GetEnterprise();
                Vector2 vector2 = ResetTexture(new Vector2(env.TextureLogo.width, env.TextureLogo.height));

                int middleX = (column - 1) / 2;

                // ori_x;ori_y
                float ori_x, ori_y;
                ori_x = x;
                ori_y = y;

                Vector2 ori_position = new Vector2(ori_x, ori_y);
                Vector2 gen_position = new Vector2(x, y);

                //				FlockAgent go = AgentGenerator.GetInstance ().generator (name, gen_position, ori_position, magicWallManager);
                FlockAgent go = ItemsFactory.Generate(ori_x, ori_y, x, y, i , j , vector2.x, vector2.y, DaoService.Instance.GetEnterprise());
            
                // 星空效果不会被物理特效影响
                go.CanEffected = false;

                // 将agent的z轴定义在后方
                //RectTransform rect = go.GetComponent<RectTransform>();
                //Vector3 position = rect.anchoredPosition3D;
                //rect.anchoredPosition3D = rect.anchoredPosition3D + new Vector3(0, 0, 3000);

                //go.gameObject.SetActive(false);
            }
        }
    }

    
    public override void Starting() {
        if (Time.time - last_generate_time > generate_agent_interval) {

            // 随机选择
            int count = AgentManager.Instance.Agents.Count;
            int index = Random.Range(0, count - 1);
            FlockAgent agent = AgentManager.Instance.Agents[index];
            // TODO 这里挑错，显示索引的内容已经没了
            agent.gameObject.SetActive(true);

            RectTransform rect = agent.GetComponent<RectTransform>();
            rect.anchoredPosition3D = rect.anchoredPosition3D + new Vector3(0, 0, 3000);

            Vector3 to = new Vector3(agent.OriVector2.x, agent.OriVector2.y, 0);
            agent.GetComponent<RectTransform>().DOAnchorPos3D(to, animation_duration)
                .OnComplete(() => DOAnchorPosCompleteCallback(agent));
            agent.GetComponent<RectTransform>().DOScale(new Vector3(0.5f, 0.5f, 0.5f), animation_duration).From();
            last_generate_time = Time.time;
        }



    }

    public override void OnStartingCompleted(){
        AgentManager.Instance.UpdateAgents();
    }


    #region Tween Callback
    public void DOAnchorPosCompleteCallback(FlockAgent agent)
    {
        RectTransform rect = agent.GetComponent<RectTransform>();
        //RawImage image = agent.GetComponentInChildren<RawImage>();
        if (!agent.IsChoosing)
        {
            //rect.DOScale(1.5f, 0.2f);
            //image.DOFade(0, 0.2F).OnComplete(() => DOFadeCompleteCallback(agent));
            foreach(RawImage rawImage in agent.GetComponentsInChildren<RawImage>())
            {
                rawImage.DOFade(0, 0.5F).OnComplete(() => DOFadeCompleteCallback(agent));
             
            }

        }
    }

    public void DOFadeCompleteCallback(FlockAgent agent)
    {
        agent.gameObject.SetActive(false);
        RectTransform rect = agent.GetComponent<RectTransform>();
        //Image image = agent.GetComponentInChildren<Image>();
        rect.DOScale(1f, Time.deltaTime);
        //image.DOFade(1, Time.deltaTime);
        foreach (RawImage rawImage in agent.GetComponentsInChildren<RawImage>())
        {
            rawImage.DOFade(1, 0);

        }
    }
	#endregion

}
