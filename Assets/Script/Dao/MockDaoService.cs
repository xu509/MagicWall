
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///     模拟数据模块
/// </summary>
public class MockDaoService : MonoBehaviour, IDaoService
{

    private List<Enterprise> _enterprises;
    private List<Activity> _activities;
    private List<Product> _products;


    void Awake()
    {
    }

    //
    //  Construct
    //
    protected MockDaoService() { }



    public void Init() {
        _enterprises = new List<Enterprise>();
        _activities = new List<Activity>();
        _products = new List<Product>();
    }

    public void Reset() {
        Init();
    }

    //
    //  加载信息
    //
    public void LoadInformation()
    {

    }

    //
    //  获取首页企业
    //
    public List<Enterprise> GetEnterprises()
    {
        if (_enterprises == null)
        {
            Enterprise env = new Enterprise();

            //  从数据库中获取数据
            _enterprises = new List<Enterprise>();
            for (int i = 0; i < 100; i++)
            {
                _enterprises.Add(env.Generator());
            }

            return _enterprises;
        }
        else
        {
            return _enterprises;
        }
    }

    //
    //  获取首页企业
    //
    public Enterprise GetEnterprise()
    {
        List<Enterprise> enterprises = GetEnterprises();
        int index = Random.Range(0, enterprises.Count);
        return enterprises[index];
    }

    public List<string> GetEnvCards(int id) {

        // TODO 
        return GetEnterprise().EnvCards;
    }



    //
    //  获取 catalog
    //
    public Catalog GetCatalog(int id)
    {
        return new Catalog().Generator();
    }

    //
    //  获取 catalogs
    //
    public List<Catalog> GetCatalogs(int id)
    {
        List<Catalog> catalogs = new List<Catalog>();
        string[] imgs = { "catalog-1-1.png", "catalog-1-2.png", "catalog-1-3.png", "catalog-1-4.png" };
        string[] descriptions = { "catalog-1-1.png", "catalog-1-2.png", "catalog-1-3.png", "catalog-1-4.png" };
        for (int i = 0; i < imgs.Length; i++) {
            Catalog catalog = new Catalog();
            catalog.Img = "env\\" + imgs[i];
            catalog.Description = descriptions[i];
            catalogs.Add(catalog);
        }
        return catalogs;
    }



    //
    //  获取企业的详细信息
    //
    public EnterpriseDetail GetEnterprisesDetail(int com_id)
    {
        //  基础数据（企业名，企业简介，企业点赞数）
        //  企业名片数据
        //  catelog数据
        //  活动数据
        //  产品数据
        //  视频数据
        EnterpriseDetail enterpriseDetail = new EnterpriseDetail().Generator();

        return enterpriseDetail;
    }

    //
    //  获取首页活动
    //
    public List<Activity> GetActivities()
    {
        if (_activities == null)
        {
            Activity activity = new Activity();

            //  从数据库中获取数据
            _activities = new List<Activity>();
            for (int i = 0; i < 100; i++)
            {
                _activities.Add(activity.Generator());
            }

            return _activities;
        }
        else
        {
            return _activities;
        }
    }

    //
    //  获取首页活动
    //
    public Activity GetActivity()
    {
        List<Activity> enterprises = GetActivities();
        int index = Random.Range(0, _activities.Count);
        return enterprises[index];
    }


    //
    //  获取首页活动的详细信息
    //
    public Activity GetActivityDetail(int act_id)
    {
        Activity activity = new Activity().Generator();
        activity.ActivityDetails = GetActivityDetails(act_id);
        return activity;
    }

    public List<ActivityDetail> GetActivityDetails(int act_id)
    {

        List<ActivityDetail> activity1s = new List<ActivityDetail>();
        ActivityDetail activity1 = new ActivityDetail();
        activity1.SetImageType();
        activity1.Image = "activity\\detail\\1-1.jpg";
        activity1.Description = "源赖光肩负着家族的未来。幼时经历的一场大火，让他下定决心肃清灵族。一场大战一触即发。";
        activity1s.Add(activity1);
        ActivityDetail activity2 = new ActivityDetail();
        activity2.SetImageType();
        activity2.Image = "activity\\detail\\1-2.jpg";
        activity2.Description = "阴阳师·晴明为了阻止两界之间的战争，创造一个和平共存的世界，他开始调查源氏家族的真相。";
        activity1s.Add(activity2);
        ActivityDetail productDetail5 = new ActivityDetail();
        productDetail5.SetVideoType();
        productDetail5.Image = "video\\1.png";
        productDetail5.Description = "video-1。";
        productDetail5.VideoUrl = "video\\1.mp4";
        activity1s.Add(productDetail5);

        List<ActivityDetail> activity2s = new List<ActivityDetail>();
        ActivityDetail activity11 = new ActivityDetail();
        activity11.SetImageType();
        activity11.Image = "activity\\detail\\2-1.jpg";
        activity11.Description = "作为坂本龙一的学生，肖瀛此次将汇同国内顶尖演奏高手，助力美声男团四位“蓝声”";
        activity2s.Add(activity11);
        ActivityDetail activity12 = new ActivityDetail();
        activity12.SetImageType();
        activity12.Image = "activity\\detail\\2-2.jpg";
        activity12.Description = "玩转咏叹调、宣叙调、世界音乐、自由爵士、古典融合电子……";
        activity2s.Add(activity12);
        ActivityDetail activity13 = new ActivityDetail();
        activity13.SetImageType();
        activity13.Image = "activity\\detail\\2-3.jpg";
        activity13.Description = "并特邀英国巡演版《歌剧魅影》音乐总监、英国皇家音乐学院音乐剧系系主任比约恩•多布拉";
        activity2s.Add(activity13);
        ActivityDetail activity14 = new ActivityDetail();
        activity14.SetVideoType();
        activity14.Image = "video\\2.png";
        activity14.Description = "video-2。";
        activity14.VideoUrl = "video\\2.mp4";
        activity2s.Add(activity14);


        List<ActivityDetail> activity3s = new List<ActivityDetail>();
        ActivityDetail productDetail31 = new ActivityDetail();
        productDetail31.SetImageType();
        productDetail31.Image = "activity\\detail\\3-1.jpg";
        productDetail31.Description = "诗情话剧《漫长的告白》取材于法国著名剧作家埃德蒙·罗斯丹的经典悲喜剧《大鼻子情圣》，导演徐俊倾力呈现浪漫唯美的诗情美学，用诗化的戏剧语言敲响钢琴黑白键间满溢的音乐诗篇，吟唱80年代纯真激荡的时代风情。";
        activity3s.Add(productDetail31);
        ActivityDetail productDetail32 = new ActivityDetail();
        productDetail32.SetImageType();
        productDetail32.Image = "activity\\detail\\3-2.jpg";
        productDetail32.Description = "知名自媒体人傅踢踢、视觉艺术家马良、著名音乐人李泉鼎力加盟，四个来自60后、70后、80后的上海男人首次跨界合作，倾情演绎永不褪色的“上海爱情故事”。";
        activity3s.Add(productDetail32);
        ActivityDetail productDetail33 = new ActivityDetail();
        productDetail33.SetImageType();
        productDetail33.Image = "activity\\detail\\3-3.jpg";
        productDetail33.Description = "故事发生在文艺精神盛行的80年代，在青春激昂的校园里，一群情感真挚，富有理想的青年热诚又果敢地追索着人生的真谛。";
        activity3s.Add(productDetail33);

        List<ActivityDetail>[] pdArra = { activity1s, activity2s, activity3s};
        return pdArra[Random.Range(0, pdArra.Length)];
    }


    //
    //  获取首页的产品
    //
    public List<Product> GetProducts()
    {
        if (_products == null)
        {
            Product product = new Product();

            //  从数据库中获取数据
            _products = new List<Product>();
            for (int i = 0; i < 100; i++)
            {
                _products.Add(product.Generator());
            }

            return _products;
        }
        else
        {
            return _products;
        }
    }

    public Product GetProduct()
    {
        List<Product> product = GetProducts();
        int index = Random.Range(0, _products.Count);
        return _products[index];
    }

    //
    //  获取产品详细
    //
    public Product GetProductDetail(int pro_id)
    {
        Product product = new Product().Generator();
        product.ProductDetails = GetProductDetails(pro_id);
        return product;
    }

    public List<ProductDetail> GetProductDetails(int pro_id) {
        List<ProductDetail> productDetails = new List<ProductDetail>();

        List<ProductDetail> productDetails1 = new List<ProductDetail>();
        ProductDetail productDetail1 = new ProductDetail();
        productDetail1.SetImageType();
        productDetail1.Image = "product\\detail\\1-1.jpg";
        productDetail1.Description = "[type1] 北欧现代简约风格客厅吊灯三室两厅卧室餐厅套装全屋灯具套餐组合。";
        productDetails1.Add(productDetail1);
        ProductDetail productDetail2 = new ProductDetail();
        productDetail2.SetImageType();
        productDetail2.Image = "product\\detail\\1-2.jpg";
        productDetail2.Description = "[type1] 轻奢后现代水晶吊灯欧式客厅灯大气餐厅灯饰美式铜灯港式别墅灯具。";
        productDetails1.Add(productDetail2);
        ProductDetail productDetail3 = new ProductDetail();
        productDetail3.SetImageType();
        productDetail3.Image = "product\\detail\\1-3.jpg";
        productDetail3.Description = "[type1] 雷士照明led吸顶灯客厅灯灯具长方形套餐组合简约现代大气卧室灯。";
        productDetails1.Add(productDetail3);
        ProductDetail productDetail4 = new ProductDetail();
        productDetail4.SetImageType();
        productDetail4.Image = "product\\detail\\1-4.jpg";
        productDetail4.Description = "[type1] 楼中楼欧式玉石水晶大吊灯客厅别墅大厅大灯酒店新中式美式复式楼。";
        productDetails1.Add(productDetail4);
        ProductDetail productDetail5 = new ProductDetail();
        productDetail5.SetVideoType();
        productDetail5.Image = "video\\1.png";
        productDetail5.Description = "video\\video-1。";
        productDetail5.VideoUrl = "1.mp4";
        productDetails1.Add(productDetail5);
        ProductDetail productDetail6 = new ProductDetail();
        productDetail6.SetImageType();
        productDetail6.Image = "product\\detail\\1-3.jpg";
        productDetail6.Description = "[type1] 雷士照明led吸顶灯客厅灯灯具长方形套餐组合简约现代大气卧室灯。";
        productDetails1.Add(productDetail6);

        List<ProductDetail> productDetails2 = new List<ProductDetail>();
        ProductDetail productDetail20 = new ProductDetail();
        productDetail20.SetImageType();
        productDetail20.Image = "product\\detail\\2-1.jpg";
        productDetail20.Description = "[type2] 新品促销室内小绿萝盆栽吸除甲醛净化空气苗圃直发办公室桌面。";
        productDetails2.Add(productDetail20);
        ProductDetail productDetail21 = new ProductDetail();
        productDetail21.SetImageType();
        productDetail21.Image = "product\\detail\\2-2.jpg";
        productDetail21.Description = "[type2] 绿萝吊兰虎皮兰新房子装修家用除甲醛盆栽去异味净化空气室内植物。";
        productDetails2.Add(productDetail21);
        ProductDetail productDetail22 = new ProductDetail();
        productDetail22.SetImageType();
        productDetail22.Image = "product\\detail\\2-3.jpg";
        productDetail22.Description = "[type2] 红枫盆景常年红植物盆栽室内净化空气办公室盆栽四季常青绿植枫树。";
        productDetails2.Add(productDetail22);
        ProductDetail productDetail23 = new ProductDetail();
        productDetail23.SetImageType();
        productDetail23.Image = "product\\detail\\2-4.jpg";
        productDetail23.Description = "[type2] 蓝雪花耐热盆栽虹越花开不断阳台花园花苗爬藤植物。";
        productDetails2.Add(productDetail23);
        ProductDetail productDetail24 = new ProductDetail();
        productDetail24.SetImageType();
        productDetail24.Image = "product\\detail\\2-5.jpg";
        productDetail24.Description = "[type2] MMH多肉植物组合新手套餐肉肉多肉花植物含白瓷花盆花卉盆栽包邮。";
        productDetails2.Add(productDetail24);
        ProductDetail productDetail25 = new ProductDetail();
        productDetail25.SetVideoType();
        productDetail25.Image = "video\\1.png";
        productDetail25.Description = "[type2] video-1。";
        productDetail25.VideoUrl = "video\\1.mp4";
        productDetails2.Add(productDetail25);


        List<ProductDetail> productDetails3 = new List<ProductDetail>();
        ProductDetail productDetail31 = new ProductDetail();
        productDetail31.SetImageType();
        productDetail31.Image = "product\\detail\\3-1.jpg";
        productDetail31.Description = "[type3] 法颂女士香水持久淡香 香水学生少女清新自然 浪漫梦境礼盒套装。";
        productDetails3.Add(productDetail31);
        ProductDetail productDetail32 = new ProductDetail();
        productDetail32.SetImageType();
        productDetail32.Image = "product\\detail\\3-2.jpg";
        productDetail32.Description = "[type3] 美国Calvin Klein卡尔文克雷恩进口卡雷优香水ckone果香男女100ml。";
        productDetails3.Add(productDetail32);
        ProductDetail productDetail33 = new ProductDetail();
        productDetail33.SetImageType();
        productDetail33.Image = "product\\detail\\3-3.jpg";
        productDetail33.Description = "[type3] 冰希黎流沙金女士香水60ml持久淡香学生少女清新幻彩鎏金网红香水。";
        productDetails3.Add(productDetail33);
        ProductDetail productDetail34 = new ProductDetail();
        productDetail34.SetImageType();
        productDetail34.Image = "product\\detail\\3-4.jpg";
        productDetail34.Description = "[type3] 车载香水座汽车用香薰车上持久淡香车内固体香膏古龙高档男士摆件。";
        productDetails3.Add(productDetail34);
        ProductDetail productDetail35 = new ProductDetail();
        productDetail35.SetImageType();
        productDetail35.Image = "product\\detail\\3-5.jpg";
        productDetail35.Description = "[type3] YSL圣罗兰Mon Paris反转巴黎香水 浪漫失魂果EDP。";
        productDetails3.Add(productDetail35);
        ProductDetail productDetail36 = new ProductDetail();
        productDetail36.SetVideoType();
        productDetail36.Image = "video\\1.png";
        productDetail36.Description = "[type3] video-1。";
        productDetail36.VideoUrl = "video\\1.mp4";
        productDetails3.Add(productDetail36);

        List<ProductDetail> productDetails4 = new List<ProductDetail>();
        ProductDetail productDetail41 = new ProductDetail();
        productDetail41.Image = "product\\detail\\4-1.jpg";
        productDetail41.Description = "简域实木餐椅现代简约单人书房椅北欧办公家用书桌靠背椅电脑椅子。";
        productDetails4.Add(productDetail41);
        ProductDetail productDetail42 = new ProductDetail();
        productDetail42.Image = "product\\detail\\4-2.jpg";
        productDetail42.Description = "伊姆斯椅子现代简约书桌椅家用餐厅靠背椅电脑椅凳子实木北欧餐椅。";
        productDetails4.Add(productDetail42);


        List<ProductDetail>[] pdArra = { productDetails1 , productDetails2, productDetails3, productDetails4 };
        return pdArra[Random.Range(0, pdArra.Length)];
    }

    #region 设置效果与运行时间

    //
    //  获取config
    //
    public AppConfig GetConfigByKey(string key)
    {
        AppConfig appConfig = new AppConfig();
        appConfig.Value = "20";

        if (key.Equals(AppConfig.KEY_CutEffectDuring_CurveStagger))
        {
            appConfig.Value = "10";
        }
        else if (key.Equals(AppConfig.KEY_CutEffectDuring_LeftRightAdjust))
        {
            appConfig.Value = "10";
        }
        else if (key.Equals(AppConfig.KEY_CutEffectDuring_MidDisperseAdjust))
        {
            appConfig.Value = "10";
        }
        else if (key.Equals(AppConfig.KEY_CutEffectDuring_Stars))
        {
            appConfig.Value = "20";
        }
        else if (key.Equals(AppConfig.KEY_CutEffectDuring_UpDownAdjust))
        {
            appConfig.Value = "10";
        }
        else if (key.Equals(AppConfig.KEY_CutEffectDuring_FrontBackUnfold))
        {
            appConfig.Value = "10";
        }
        else
        {

        }
        return appConfig;
    }

    /// <summary>
    ///     获取场景持续时间
    /// </summary>
    /// <param name="sceneTypeEnum"></param>
    /// <returns></returns>
    public float GetSceneDurTime(SceneTypeEnum sceneTypeEnum)
    {
        string key = "";

        if (sceneTypeEnum == SceneTypeEnum.CurveStagger)
        {
            key = AppConfig.KEY_CutEffectDuring_CurveStagger;
        }
        else if (sceneTypeEnum == SceneTypeEnum.FrontBackUnfold)
        {
            key = AppConfig.KEY_CutEffectDuring_FrontBackUnfold;
        }
        else if (sceneTypeEnum == SceneTypeEnum.LeftRightAdjust)
        {
            key = AppConfig.KEY_CutEffectDuring_LeftRightAdjust;
        }
        else if (sceneTypeEnum == SceneTypeEnum.MidDisperse)
        {
            key = AppConfig.KEY_CutEffectDuring_MidDisperseAdjust;
        }
        else if (sceneTypeEnum == SceneTypeEnum.Stars)
        {
            key = AppConfig.KEY_CutEffectDuring_Stars;
        }
        else if (sceneTypeEnum == SceneTypeEnum.UpDownAdjustCutEffect)
        {
            key = AppConfig.KEY_CutEffectDuring_UpDownAdjust;
        }

        string durTime = GetConfigByKey(key).Value;
        float d = AppUtils.ConvertToFloat(durTime);

        return d;
    }
    #endregion

    public int GetLikesByProductDetail(int id)
    {
        int likes = Random.Range(1, 50);
        return likes;
    }

    public int GetLikesByActivityDetail(int id)
    {
        int likes = Random.Range(1, 50);
        return likes;
    }


    public int GetLikes(int id, CrossCardCategoryEnum category)
    {
        int likes = Random.Range(1, 50);
        return likes;
    }

    //
    //  获取显示配置
    //
    public List<SceneConfig> GetShowConfigs()
    {

        List<SceneConfig> sceneConfigs = new List<SceneConfig>();

        // Real 
        //CutEffect[] effects = new CutEffect[] {
        //    //new FrontBackUnfoldCutEffect(),
        //    new CurveStaggerCutEffect(),
        //    new MidDisperseCutEffect(),
        //    new StarsCutEffect(),
        //    //new LeftRightAdjustCutEffect(),
        //    //new UpDownAdjustCutEffect(),
        //};

        SceneTypeEnum[] sceneTypes = new SceneTypeEnum[]
        {
            SceneTypeEnum.CurveStagger,
            SceneTypeEnum.FrontBackUnfold,
            SceneTypeEnum.LeftRightAdjust,
            SceneTypeEnum.MidDisperse,
            SceneTypeEnum.Stars,
            SceneTypeEnum.UpDownAdjustCutEffect,
        };


        //SceneContentType[] contentTypes = new SceneContentType[] { SceneContentType.product, SceneContentType.activity };
        DataType[] dataTypes = new DataType[] {
            DataType.env,
            DataType.activity,
            DataType.product,
        };
        //SceneContentType[] contentTypes = new SceneContentType[] { SceneContentType.activity };

        for (int i = 0; i < sceneTypes.Length; i++)
        {
            for (int j = 0; j < dataTypes.Length; j++)
            {
                SceneConfig sceneConfig = new SceneConfig();

                if (sceneTypes[i] == SceneTypeEnum.Stars && dataTypes[j] == DataType.env) {
                    continue;
                }

                if (sceneTypes[i] == SceneTypeEnum.FrontBackUnfold && dataTypes[j] == DataType.env)
                {
                    continue;
                }

                sceneConfig.sceneType = sceneTypes[i];
                sceneConfig.dataType = dataTypes[j];

                // 设置场景时间
                sceneConfig.durtime = GetSceneDurTime(sceneTypes[i]);
                sceneConfigs.Add(sceneConfig);
            }
        }

        return sceneConfigs;
    }

    public bool IsCustom() {
        //TODO
        int number = Random.Range(0, 5);
        return number > 2;

        return true;

    }




    //
    //  TODO 获取定制屏所配置的图片
    //
    public List<string> GetCustomImage(CustomImageType type) {
        string[] leftImages = { "l1.jpg", "l2.jpg", "l3.jpg" };
        string[] middleImages = { "m1.jpg", "m2.jpg", "m3.jpg", "m4.jpg", "m5.jpg" };
        string[] rightImages = { "r1.jpg"};

        if (type == CustomImageType.LEFT1) {
            List<string> images = new List<string>();
            int size = Random.Range(1, 4);
            size = 3;
            for (int i = 0; i < size; i++) {
                images.Add("custom\\" + leftImages[i]);
            }
            return images;
        }
        else if (type == CustomImageType.LEFT2)
        {
            List<string> images = new List<string>();
            int size = Random.Range(1, 4);
            size = 5;
            for (int i = 0; i < size; i++)
            {
                images.Add("custom\\" + middleImages[i]);
            }
            return images;
        }
        else
        {
            List<string> images = new List<string>();
            int size = Random.Range(1, 4);
            size = 1;
            for (int i = 0; i < size; i++)
            {
                images.Add("custom\\" + rightImages[i]);
            }
            return images;
        }
    }

    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="keys">关键词</param>
    /// <returns></returns>
    public List<SearchBean> Search(string keys) {
        Debug.Log("搜索KEYS ：" + keys);

        int count =  Random.Range(0,5) > 2 ? 0 : Random.Range(5, 20);

        List<SearchBean> beans = new List<SearchBean>();
        for(int i = 0; i < count; i++)
        {
            beans.Add(new SearchBean().Generator());
        }

        return beans;
    }


    /// <summary>
    ///     获得浮动块数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public FlockData GetFlockData(DataType type) {
        if (type == DataType.env)
        {
            return GetEnterprise();
        }
        else if (type == DataType.product)
        {
            return GetProduct();
        }
        else if (type == DataType.activity) {
            return GetActivity();
        }
        return null;
    }

    public Enterprise GetEnterprisesById(int id)
    {
        return GetEnterprise();
    }

    public Video GetVideoDetail(int envId, int index)
    {
        return new Video().Generator();
    }

    public List<Video> GetVideosByEnvId(int envId)
    {
        return GetEnterprisesDetail(envId).videos;

    }

    public List<Activity> GetActivitiesByEnvId(int envid)
    {
        var activities = new List<Activity>();
        for (int i = 0; i < 5; i++)
        {
            Activity e = GetActivityDetail(i);
            activities.Add(e);
        }

        return activities;
    }

    public List<Product> GetProductsByEnvId(int envid)
    {
        var products = new List<Product>();
        for (int i = 0; i < 5; i++)
        {
            Product e = GetProductDetail(i);
            products.Add(e);
        }

        return products;
    }
}
