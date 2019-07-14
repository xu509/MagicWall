using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 企业工厂
/// </summary>
public class EnvFactory : Singleton<EnvFactory>, ItemsFactory
{
    // Manager
    private MagicWallManager _manager;

    // Agent Manager
    private AgentManager _agentManager;


    void Awake() {
    }

    public void Init(MagicWallManager manager)
    {
        _manager = manager;
        _agentManager = _manager.agentManager;
    }


    public EnvFactory() {


    }

    #region 生成浮动块
    //
    //  生成env的浮动组件
    //  - 长宽相等
    //  - 生成在动画前
    //  - 生成在动画后
    //
    public FlockAgent Generate(float gen_x, float gen_y, float ori_x, float ori_y,
        int row, int column, float width, float height, BaseData data, AgentContainerType agentContainerType)
    {
        width = (int)width;
        height = (int)height;
        Enterprise env = data as Enterprise;

        //  创建 Agent
        FlockAgent newAgent = _agentManager.GetFlockAgent(agentContainerType);
        //  命名
        newAgent.name = "Agent(" + (row + 1) + "," + (column + 1) + ")";

        //  定出生位置
        Vector2 postion = new Vector2(gen_x, gen_y);

        //  定面板位置
        Vector2 ori_position = new Vector2(ori_x, ori_y);

        //  初始化内容
        newAgent.Initialize(_manager, ori_position, postion, row + 1, column + 1,
            width, height, env.Ent_id, env.Logo, env.IsCustom, MWTypeEnum.Enterprise, agentContainerType);

        //  添加到组件袋
        _agentManager.AddItem(newAgent);

        return newAgent;
    }
    #endregion

    #region 生成十字卡片
    public CardAgent GenerateCardAgent(Vector3 genPos, FlockAgent flockAgent, int dataId, bool isActive)
    {

        //  创建 Agent
        CrossCardAgent crossCardAgent = _agentManager.GetCrossCardAgent();
            

        //  定义缩放
        Vector3 scaleVector3 = new Vector3(0.2f, 0.2f, 0.2f);



        // 初始化数据
        crossCardAgent.InitCardData(_manager, dataId, MWTypeEnum.Enterprise, genPos, scaleVector3, flockAgent);

        // 添加到effect agent
        _agentManager.AddEffectItem(crossCardAgent);

        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();

        // 初始化 CrossAgent 数据
        crossCardAgent.InitCrossCardAgent();

        sw.Stop();
        Debug.Log("GenerateCardAgent Time : " + sw.ElapsedMilliseconds / 1000f);

        crossCardAgent.gameObject.SetActive(isActive);

        Debug.Log("crossCardAgent is prepared");

        return crossCardAgent;
    }
    #endregion


}
