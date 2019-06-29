using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 企业工厂
/// </summary>
public class ActivityFactory :MonoBehaviour, ItemsFactory
{

    // Manager
    private MagicWallManager _manager;

    // Agent Manager
    private AgentManager _agentManager;

    void Awake() {

    }

    public ActivityFactory() {


    }

    public void Init(MagicWallManager manager)
    {
        _manager = manager;
        _agentManager = _manager.agentManager;
    }


    #region 生成浮动块
    //
    //  生成env的浮动组件
    //  - 长宽相等
    //  - 生成在动画前
    //  - 生成在动画后
    //
    public FlockAgent Generate(float gen_x, float gen_y, float ori_x, float ori_y, int row, int column, float width, 
        float height, BaseData data, AgentContainerType agentContainerType)
    {
        width = (int)width;
        height = (int)height;

        Activity activity = data as Activity;

        //  创建 Agent
        FlockAgent newAgent = _agentManager.GetFlockAgent(agentContainerType);

        //  命名
        newAgent.name = "Agent(" + (row + 1) + "," + (column + 1) + ")";


        //  定面板位置
        Vector2 ori_position = new Vector2(ori_x, ori_y);

        //  初始化内容
        newAgent.Initialize(_manager,ori_position, new Vector2(gen_x, gen_y), row + 1, column + 1,
            width, height, activity.Ent_id, activity.Image, false, MWTypeEnum.Activity, agentContainerType);

        //  添加到组件袋
        _agentManager.Agents.Add(newAgent);

        return newAgent;
    }
    #endregion

    #region 生成滑动卡片
    public CardAgent GenerateCardAgent(Vector3 genPos, FlockAgent flockAgent, int dataId, bool isActive)
    {
        // 0.45 比例
        float height = _manager.mainPanel.rect.height * _manager.operateCardScaleFactory;

        //  创建 Agent
        SliceCardAgent sliceCardAgent = _agentManager.GetSliceCardAgent();
        sliceCardAgent.GetComponent<RectTransform>().sizeDelta = new Vector2(height, height);

        //  定义缩放
        Vector3 scaleVector3 = new Vector3(0.2f, 0.2f, 0.2f);

        //  初始化卡片基础数据
        sliceCardAgent.InitCardData(_manager, dataId, MWTypeEnum.Activity, genPos, scaleVector3, flockAgent);

        //  初始化数据
        sliceCardAgent.InitSliceCard();

        // 添加到effect agent
        _agentManager.AddEffectItem(sliceCardAgent);

        // 设置显示状态
        sliceCardAgent.gameObject.SetActive(isActive);

        return sliceCardAgent;
    }
    #endregion

}
