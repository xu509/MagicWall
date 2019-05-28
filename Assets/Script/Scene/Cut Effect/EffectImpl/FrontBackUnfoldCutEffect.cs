using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 过场效果 前后层展开
public class FrontBackUnfoldCutEffect : CutEffect
{
    private MagicWallManager _manager;


    private int _page;  // 页码

    private DisplayBehaviorConfig _displayBehaviorConfig;   //  Display Behavior Config
    private float _startDelayTime = 0f;  //启动的延迟时间
    private float _startingTimeWithOutDelay;
    private float _timeBetweenStartAndDisplay = 0.5f; //完成启动动画与表现动画之间的时间

    //
    //  Init
    //
    protected override void Init()
    {
        //  获取持续时间
        StartingDurTime = 1.5f;
        _startingTimeWithOutDelay = StartingDurTime;
        DestoryDurTime = 0.5f;

        //  设置显示的时间
        string t = DaoService.Instance.GetConfigByKey(AppConfig.KEY_CutEffectDuring_CurveStagger).Value;
        DisplayDurTime = AppUtils.ConvertToFloat(t);

        // 获取Display的动画
        DisplayBehavior = new FrontBackGoLeftDisplayBehavior();

        // 获取销毁的动画
        DestoryBehavior = new FadeOutDestoryBehavior();
        DestoryBehavior.Init(DestoryDurTime);

        //  初始化 manager
        _manager = MagicWallManager.Instance;

        //  初始化 config
        _displayBehaviorConfig = new DisplayBehaviorConfig();

    }

    //
    private void CutEffectUpdateCallback(FlockAgent go)
    {
        if (go.name == "Agent(1,1)")
        {
            Debug.Log(go.GetComponent<RectTransform>().anchoredPosition);
        }
        //		go.updatePosition ();
    }

    //
    //  创建 Logo 
    //
    protected override void CreateLogo()
    {
        int _row = _manager.Row;
        int _column = ItemsFactory.GetSceneColumn();
        float itemWidth = ItemsFactory.GetItemWidth();
        float itemHeight = ItemsFactory.GetItemHeight();
        float gap = ItemsFactory.GetSceneGap();

        //从左往右，从下往上
        for (int i = 0; i < _row; i++)
        {
            for (int j = 0; j < _column; j++)
            {
                //float x = j * (itemWidth + gap) + itemWidth / 2 + gap;
                //float y = i * (itemHeight + gap) + itemHeight / 2 + gap;

                Vector2 vector2 = ItemsFactory.GetOriginPosition(i, j);
                float x = vector2.x;
                float y = vector2.y;


                int middleY = _row / 2;
                int middleX = _column / 2;

                // 定义源位置
                float ori_x, ori_y;
                ori_x = MagicWallManager.Instance.mainPanel.rect.width + itemWidth + gap;
                ori_y = y;

                bool front = (i + j) % 2 == 0 ? true : false;
                //float offsetX = (z == 0) ? 0 : -500;
                //x = x + offsetX;
                //生成 agent
                FlockAgent go;
                if (front)
                {
                    go = ItemsFactory.Generate(ori_x, ori_y, x, y, i, j, itemWidth, itemHeight, DaoService.Instance.GetEnterprise(), _manager.mainPanel);

                }   else
                {
                    go = ItemsFactory.Generate(ori_x, ori_y, x, y, i, j, itemWidth, itemHeight, DaoService.Instance.GetEnterprise(), _manager.backPanel);
                    go.GetComponent<RawImage>().DOFade(0.2f, 0);
                }

                // 装载延迟参数
                go.DelayX = 0;
                go.DelayY = 0;

                // 调整大小
                RectTransform the_RectTransform = go.GetComponent<RectTransform>();
                the_RectTransform.sizeDelta = new Vector2(itemWidth, itemWidth);

                // 将agent的z轴定义在后方
                //Vector3 position = the_RectTransform.anchoredPosition3D;
                //the_RectTransform.anchoredPosition3D = the_RectTransform.anchoredPosition3D + new Vector3(0, 0, z);

                //if (z != 0)
                //{
                //    the_RectTransform.SetAsFirstSibling();
                //}

                // 装载进 pagesAgents
                int colUnit = Mathf.CeilToInt(_column * 1.0f / 4);
                _page = Mathf.CeilToInt((j + 1) * 1.0f / colUnit);
                _displayBehaviorConfig.AddFlockAgentToAgentsOfPages(_page, go);

            }
        }

        // 调整启动动画的时间
        StartingDurTime += _startDelayTime;
    }

    //
    //  创建活动
    //
    protected override void CreateActivity()
    {
        int _row = 6;
        int _column = 35;
        float itemWidth = 250;
        float itemHeight = 250;
        float gap = ItemsFactory.GetSceneGap();

        //从左往右，从下往上
        for (int i = 0; i < _row; i++)
        {
            for (int j = 0; j < _column; j++)
            {
                float x = j * (itemWidth + gap) + itemWidth / 2 + gap;
                float y = i * (itemHeight + gap) + itemHeight / 2 + gap;

                Activity activity = DaoService.Instance.GetActivity();
                Vector2 v2 = AppUtils.ResetTexture(new Vector2(activity.TextureImage.width, activity.TextureImage.height));
                //Vector2 vector2 = ItemsFactory.GetOriginPosition(i, j);
                //float x = vector2.x;
                //float y = vector2.y;


                int middleY = _row / 2;
                int middleX = _column / 2;

                // 定义源位置
                float ori_x, ori_y;
                ori_x = MagicWallManager.Instance.mainPanel.rect.width + itemWidth + gap;
                ori_y = y;

                bool front = (i + j) % 2 == 0 ? true : false;
                //float offsetX = (z == 0) ? 0 : -500;
                //x = x + offsetX;
                //生成 agent
                FlockAgent go;
                if (front)
                {
                    go = ItemsFactory.Generate(ori_x, ori_y, x, y, i, j, itemWidth, itemHeight, activity, _manager.mainPanel);

                }   else
                {
                    go = ItemsFactory.Generate(ori_x, ori_y, x, y, i, j, itemWidth, itemHeight, activity, _manager.backPanel);
                    go.GetComponent<RawImage>().DOFade(0.2f, 0);
                }

                // 装载延迟参数
                go.DelayX = 0;
                go.DelayY = 0;

                // 调整大小
                RectTransform the_RectTransform = go.GetComponent<RectTransform>();
                the_RectTransform.sizeDelta = new Vector2(v2.x, v2.y);

                // 将agent的z轴定义在后方
                //Vector3 position = the_RectTransform.anchoredPosition3D;
                //the_RectTransform.anchoredPosition3D = the_RectTransform.anchoredPosition3D + new Vector3(0, 0, z);

                // 装载进 pagesAgents
                int colUnit = Mathf.CeilToInt(_column * 1.0f / 4);
                _page = Mathf.CeilToInt((j + 1) * 1.0f / colUnit);
                _displayBehaviorConfig.AddFlockAgentToAgentsOfPages(_page, go);

            }
        }

        // 调整启动动画的时间
        StartingDurTime += _startDelayTime;
    }

    public override void Starting()
    {
        for (int i = 0; i < AgentManager.Instance.Agents.Count; i++)
        {
            FlockAgent agent = AgentManager.Instance.Agents[i];
            //Vector2 agent_vector2 = agent.GetComponent<RectTransform> ().anchoredPosition;
            Vector2 agent_vector2 = agent.GenVector2;
            Vector2 ori_vector2 = agent.OriVector2;

            float run_time = (_startingTimeWithOutDelay - agent.DelayX + agent.DelayY) - _timeBetweenStartAndDisplay; // 动画运行的总时间

            //Ease.InOutQuad
            float time = Time.time - StartTime;  // 当前已运行的时间;

            if (time > run_time)
            {
                agent.updatePosition();
                continue;
            }

            // 模拟 DOTWEEN InOutQuad
            if ((time /= run_time * 0.5f) < 1f)
            {
                time = 0.5f * time * time;
            }
            else
            {
                time = -0.5f * ((time -= 1f) * (time - 2f) - 1f);
            }

            Vector2 to = Vector2.Lerp(agent_vector2, ori_vector2, time);

            agent.NextVector2 = to;
            agent.updatePosition();
        }

    }

    public override void OnStartingCompleted()
    {
        //  初始化表现形式
        int _row = _manager.Row;
        int _column = ItemsFactory.GetSceneColumn();
        float _itemWidth = ItemsFactory.GetItemWidth();
        float _itemHeight = ItemsFactory.GetItemHeight();

        _displayBehaviorConfig.Row = _row;
        _displayBehaviorConfig.Column = _column;
        _displayBehaviorConfig.ItemWidth = _itemWidth;
        _displayBehaviorConfig.ItemHeight = _itemHeight;
        _displayBehaviorConfig.SceneContentType = sceneContentType;
        _displayBehaviorConfig.Page = _page;
        _displayBehaviorConfig.ItemsFactory = ItemsFactory;
        DisplayBehavior.Init(_displayBehaviorConfig);

    }

    protected override void CreateProduct()
    {
        int _row = 6;
        int _column = 35;
        float itemWidth = 250;
        float itemHeight = 250;
        float gap = ItemsFactory.GetSceneGap();

        //从左往右，从下往上
        for (int i = 0; i < _row; i++)
        {
            for (int j = 0; j < _column; j++)
            {
                float x = j * (itemWidth + gap) + itemWidth / 2 + gap;
                float y = i * (itemHeight + gap) + itemHeight / 2 + gap;

                Product product = DaoService.Instance.GetProduct();
                Vector2 v2 = AppUtils.ResetTexture(new Vector2(product.TextureImage.width, product.TextureImage.height));
                //Vector2 vector2 = ItemsFactory.GetOriginPosition(i, j);
                //float x = vector2.x;
                //float y = vector2.y;


                int middleY = _row / 2;
                int middleX = _column / 2;

                // 定义源位置
                float ori_x, ori_y;
                ori_x = MagicWallManager.Instance.mainPanel.rect.width + itemWidth + gap;
                ori_y = y;

                bool front = (i + j) % 2 == 0 ? true : false;
                //float offsetX = (z == 0) ? 0 : -500;
                //x = x + offsetX;
                //生成 agent
                FlockAgent go;
                if (front)
                {
                    go = ItemsFactory.Generate(ori_x, ori_y, x, y, i, j, itemWidth, itemHeight, product, _manager.mainPanel);

                }
                else
                {
                    go = ItemsFactory.Generate(ori_x, ori_y, x, y, i, j, itemWidth, itemHeight, product, _manager.backPanel);
                    go.GetComponent<RawImage>().DOFade(0.2f, 0);
                }

                // 装载延迟参数
                go.DelayX = 0;
                go.DelayY = 0;

                // 调整大小
                RectTransform the_RectTransform = go.GetComponent<RectTransform>();
                the_RectTransform.sizeDelta = new Vector2(v2.x, v2.y);

                // 将agent的z轴定义在后方
                //Vector3 position = the_RectTransform.anchoredPosition3D;
                //the_RectTransform.anchoredPosition3D = the_RectTransform.anchoredPosition3D + new Vector3(0, 0, z);

                //if (z != 0)
                //{
                //    the_RectTransform.SetAsFirstSibling();
                //}

                // 装载进 pagesAgents
                int colUnit = Mathf.CeilToInt(_column * 1.0f / 4);
                _page = Mathf.CeilToInt((j + 1) * 1.0f / colUnit);
                _displayBehaviorConfig.AddFlockAgentToAgentsOfPages(_page, go);

            }
        }

        // 调整启动动画的时间
        StartingDurTime += _startDelayTime;
    }

}
