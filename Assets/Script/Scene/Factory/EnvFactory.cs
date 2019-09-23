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
            width, height, env.Ent_id, env.Logo, env.IsCustom, MWTypeEnum.Enterprise,MagicWall.DataTypeEnum.Enterprise, agentContainerType);

        //  添加到组件袋
        _agentManager.AddItem(newAgent);

        return newAgent;
    }
    #endregion

    #region 生成操作卡片
    public CardAgent GenerateCardAgent(Vector3 genPos, FlockAgent flockAgent, int dataId, bool isActive)
    {
        CardAgent _cardAgent;

        Enterprise enterprise;

        IDaoService daoService = _manager.daoService;
        EnterpriseDetail enterpriseDetail = daoService.GetEnterprisesDetail(dataId);
        enterprise = enterpriseDetail.Enterprise;

        bool flag = false;

        //if (CheckCardIsSample(enterpriseDetail))
        if (flag)
        {
            Debug.Log("Create Single Card");

            _cardAgent = GenerateSingleCard(genPos, flockAgent, dataId, isActive, enterprise);

            Debug.Log("After Single Card");
        }
        else {
            _cardAgent = GenerateCrossCard(genPos, flockAgent, dataId, isActive, enterprise);
        }


        return _cardAgent;
    }
    #endregion


    /// <summary>
    ///     生成十字卡片
    /// </summary>
    /// <param name="genPos"></param>
    /// <param name="flockAgent"></param>
    /// <param name="dataId"></param>
    /// <param name="isActive"></param>
    /// <returns></returns>
    private CardAgent GenerateCrossCard(Vector3 genPos, FlockAgent flockAgent,
        int dataId, bool isActive,Enterprise enterprise) {
        //  创建 Agent
        CrossCardAgent crossCardAgent = _agentManager.GetCrossCardAgent();


        //  定义缩放
        Vector3 scaleVector3 = new Vector3(0.2f, 0.2f, 0.2f);


        // 初始化数据
        crossCardAgent.InitCardData(_manager, dataId, MWTypeEnum.Enterprise, MagicWall.DataTypeEnum.Enterprise, genPos, scaleVector3, flockAgent);
        crossCardAgent.enterpriseType = MWEnterpriseTypeEnum.Cross;

        // 添加到effect agent
        _agentManager.AddEffectItem(crossCardAgent);

        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();

        // 初始化 CrossAgent 数据
        crossCardAgent.InitCrossCardAgent();

        sw.Stop();
        // Debug.Log("GenerateCardAgent Time : " + sw.ElapsedMilliseconds / 1000f);

        crossCardAgent.gameObject.SetActive(isActive);

        return crossCardAgent;
    }

    private CardAgent GenerateSingleCard(Vector3 genPos, FlockAgent flockAgent, 
        int dataId, bool isActive, Enterprise enterprise)
    {
        //  创建 Agent
        var singleCardAgent = _agentManager.GetSingleCardAgent();

        //  定义缩放
        Vector3 scaleVector3 = new Vector3(0.2f, 0.2f, 0.2f);

        // 初始化数据
        singleCardAgent.InitCardData(_manager, dataId, MWTypeEnum.Enterprise,MagicWall.DataTypeEnum.Enterprise, genPos, scaleVector3, flockAgent);
        singleCardAgent.enterpriseType = MWEnterpriseTypeEnum.Single;

        // 添加到effect agent
        _agentManager.AddEffectItem(singleCardAgent);

        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();

        // 初始化 CrossAgent 数据
        singleCardAgent.InitSingleCardAgent(enterprise);

        sw.Stop();
        // Debug.Log("GenerateCardAgent Time : " + sw.ElapsedMilliseconds / 1000f);

        singleCardAgent.gameObject.SetActive(isActive);

        return singleCardAgent;
    }


    private bool CheckCardIsSample(EnterpriseDetail enterpriseDetail) {
        bool _hasCatalog, _hasProduct, _hasActivity, _hasVideo;

        //// 判断几个类型
        _hasCatalog = enterpriseDetail.catalog.Count > 0;
        _hasProduct = enterpriseDetail.products.Count > 0;
        _hasActivity = enterpriseDetail.activities.Count > 0;
        _hasVideo = enterpriseDetail.videos.Count > 0;

        if (_hasCatalog || _hasProduct || _hasActivity || _hasVideo)
        {
            return false;
        }
        else {
            return true;
        }


    }


}
