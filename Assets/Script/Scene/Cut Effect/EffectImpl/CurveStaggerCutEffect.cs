using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 过场效果 1 ，曲线麻花效果
public class CurveStaggerCutEffect : CutEffect
{
    private MagicWallManager manager;

    private int _row;
    private int _column;
    private float _itemHeight;  // item height
    private float _itemWidth;   // item width
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
        StartingDurTime = 3f;
        _startingTimeWithOutDelay = StartingDurTime;
        DestoryDurTime = 0.5f;

        //  设置显示的时间
        string t = DaoService.Instance.GetConfigByKey(AppConfig.KEY_CutEffectDuring_CurveStagger).Value;
        DisplayDurTime = AppUtils.ConvertToFloat(t);

        // 获取Display的动画
        DisplayBehavior = new GoLeftDisplayBehavior();

        // 获取销毁的动画
        DestoryBehavior = new FadeOutDestoryBehavior();

        //  初始化 manager
        manager = MagicWallManager.Instance;

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
    //  创建产品 | Logo 
    //
    protected override void CreateProductOrLogo() {
        _row = manager.Row;
        int h = (int)manager.mainPanel.rect.height;
        int w = (int)manager.mainPanel.rect.width;
        int gap = 10;

        float itemHeight = h / _row - gap;
        float itemWidth = itemHeight;

        _column = Mathf.CeilToInt(w / itemWidth);

        //从左往右，从下往上
        for (int i = 0; i < _row; i++)
        {
            for (int j = 0; j < _column; j++)
            {
                float x = j * (itemWidth + gap) + itemWidth / 2;
                float y = i * (itemHeight + gap) + itemHeight / 2;

                int middleY = _row / 2;
                int middleX = _column / 2;

                float delayX = j * 0.06f;
                float delayY;

                // 定义源位置
                float ori_x, ori_y;

                if (i < middleY)
                {
                    delayY = System.Math.Abs(middleY - i) * 0.3f;
                    ori_x = (_column + middleY - i - 1) * (itemWidth + gap) + itemWidth / 2;
                    ori_y = (_column - j - middleY) * (itemHeight + gap) + itemHeight / 2;
                    //the_RectTransform.DOLocalMove(new Vector3((column + middleY - i - 1) * (itemWidth + gap) + itemWidth / 2, (column - j - middleY) * (itemHeight + gap) + itemHeight / 2, 0), dur_time - delayX + delayY).SetEase(Ease.InOutQuad).From();
                }
                else
                {
                    delayY = (System.Math.Abs(middleY - i) + 1) * 0.3f;
                    ori_x = (_column + i - middleY) * (itemWidth + gap) + itemWidth / 2;
                    ori_y = -(_column - j - middleY) * (itemHeight + gap) + itemHeight / 2;
                    //the_RectTransform.DOLocalMove(new Vector3((column + i - middleY) * (itemWidth + gap) + itemWidth / 2, -(column - j - middleY) * (itemHeight + gap) + itemHeight / 2, 0), dur_time - delayX + delayY).SetEase(Ease.InOutQuad).From();
                }

                //生成 agent
                FlockAgent go = ItemsFactory.Generate(ori_x, ori_y, x, y, i, j, itemWidth, itemHeight);
                //AgentManager.Instance.CreateNewAgent(ori_x, ori_y, x, y, i, j, itemWidth, itemHeight);

                // 装载延迟参数
                go.DelayX = delayX;
                go.DelayY = delayY;

                // 调整大小
                RectTransform the_RectTransform = go.GetComponent<RectTransform>();
                the_RectTransform.sizeDelta = new Vector2(itemWidth, itemWidth);

                // 生成透明度动画
                go.GetComponentInChildren<RawImage>().DOFade(0, StartingDurTime - delayX + delayY).From();

                // 获取启动动画的延迟时间
                if ((delayY - delayX) > _startDelayTime) {
                    _startDelayTime = delayY - delayX;
                }

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
    protected override void CreateActivity() {
        _row = manager.Row;
        int h = (int)manager.mainPanel.rect.height;
        int w = (int)manager.mainPanel.rect.width;
        int gap = 10;

        _itemHeight = h / _row - gap;
        _itemWidth = _itemHeight;

        _column = Mathf.CeilToInt(w / _itemWidth);

        //从左往右，从下往上
        for (int i = 0; i < _row; i++)
        {
            for (int j = 0; j < _column; j++)
            {
                float x = j * (_itemWidth + gap) + _itemWidth / 2;
                float y = i * (_itemHeight + gap) + _itemHeight / 2;

                int middleY = _row / 2;
                int middleX = _column / 2;

                float delayX = j * 0.06f;
                float delayY;

                // 定义源位置
                float ori_x, ori_y;

                if (i < middleY)
                {
                    delayY = System.Math.Abs(middleY - i) * 0.3f;
                    ori_x = (_column + middleY - i - 1) * (_itemWidth + gap) + _itemWidth / 2;
                    ori_y = (_column - j - middleY) * (_itemHeight + gap) + _itemHeight / 2;
                    //the_RectTransform.DOLocalMove(new Vector3((column + middleY - i - 1) * (itemWidth + gap) + itemWidth / 2, (column - j - middleY) * (itemHeight + gap) + itemHeight / 2, 0), dur_time - delayX + delayY).SetEase(Ease.InOutQuad).From();
                }
                else
                {
                    delayY = (System.Math.Abs(middleY - i) + 1) * 0.3f;
                    ori_x = (_column + i - middleY) * (_itemWidth + gap) + _itemWidth / 2;
                    ori_y = -(_column - j - middleY) * (_itemHeight + gap) + _itemHeight / 2;
                    //the_RectTransform.DOLocalMove(new Vector3((column + i - middleY) * (itemWidth + gap) + itemWidth / 2, -(column - j - middleY) * (itemHeight + gap) + itemHeight / 2, 0), dur_time - delayX + delayY).SetEase(Ease.InOutQuad).From();
                }

                //生成 agent
                FlockAgent go = ItemsFactory.Generate(ori_x, ori_y, x, y, i, j, _itemWidth, _itemHeight);

                // 装载延迟参数
                go.DelayX = delayX;
                go.DelayY = delayY;

                // 调整大小
                RectTransform the_RectTransform = go.GetComponent<RectTransform>();
                the_RectTransform.sizeDelta = new Vector2(_itemWidth, _itemWidth);

                // 生成透明度动画
                go.GetComponentInChildren<Image>().DOFade(0, StartingDurTime - delayX + delayY).From();

                // 获取启动动画的延迟时间
                if ((delayY - delayX) > _startDelayTime)
                {
                    _startDelayTime = delayY - delayX;
                }
            }
        }

        // 调整启动动画的时间
        StartingDurTime += _startDelayTime;
    }

    public override void Starting() {
        for (int i = 0; i < AgentManager.Instance.Agents.Count; i++) {
            FlockAgent agent = AgentManager.Instance.Agents[i];
            //Vector2 agent_vector2 = agent.GetComponent<RectTransform> ().anchoredPosition;
            Vector2 agent_vector2 = agent.GenVector2;
            Vector2 ori_vector2 = agent.OriVector2;

            float run_time = (_startingTimeWithOutDelay - agent.DelayX + agent.DelayY) - _timeBetweenStartAndDisplay; // 动画运行的总时间

            //Ease.InOutQuad
            float time = Time.time - StartTime;  // 当前已运行的时间;

            if (time > run_time) {
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

        //  初始化表现形式
        _displayBehaviorConfig.Row = _row;
        _displayBehaviorConfig.Column = _column;
        _displayBehaviorConfig.ItemWidth = _itemWidth;
        _displayBehaviorConfig.ItemHeight = _itemHeight;
        _displayBehaviorConfig.SceneContentType = sceneContentType;
        _displayBehaviorConfig.Page = _page;
        _displayBehaviorConfig.ItemsFactory = ItemsFactory;
        DisplayBehavior.Init(_displayBehaviorConfig);
    }

    public override void OnStartingCompleted(){
        Debug.Log("Starting is Completed");
        AgentManager.Instance.UpdateAgents();
    }



}
