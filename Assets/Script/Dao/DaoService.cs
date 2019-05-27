﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaoService : Singleton<DaoService>
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
    protected DaoService() { }

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
    public Catalog GetCatalog()
    {
        return new Catalog().Generator();
    }



    //
    //  获取企业的详细信息
    //
    public EnterpriseDetail GetEnterprisesDetail()
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
        List<ActivityDetail> activityDetails = new List<ActivityDetail>();

        List<ActivityDetail> activity1s = new List<ActivityDetail>();
        ActivityDetail activity1 = new ActivityDetail();
        activity1.Image = "1-1.jpg";
        activity1.Description = "源赖光肩负着家族的未来。幼时经历的一场大火，让他下定决心肃清灵族。一场大战一触即发。";
        activity1s.Add(activity1);
        ActivityDetail activity2 = new ActivityDetail();
        activity2.Image = "1-2.jpg";
        activity2.Description = "阴阳师·晴明为了阻止两界之间的战争，创造一个和平共存的世界，他开始调查源氏家族的真相。";
        activity1s.Add(activity2);

        List<ActivityDetail> activity2s = new List<ActivityDetail>();
        ActivityDetail activity11 = new ActivityDetail();
        activity11.Image = "2-1.jpg";
        activity11.Description = "作为坂本龙一的学生，肖瀛此次将汇同国内顶尖演奏高手，助力美声男团四位“蓝声”";
        activity2s.Add(activity11);
        ActivityDetail activity12 = new ActivityDetail();
        activity12.Image = "2-2.jpg";
        activity12.Description = "玩转咏叹调、宣叙调、世界音乐、自由爵士、古典融合电子……";
        activity2s.Add(activity12);
        ActivityDetail activity13 = new ActivityDetail();
        activity13.Image = "2-3.jpg";
        activity13.Description = "并特邀英国巡演版《歌剧魅影》音乐总监、英国皇家音乐学院音乐剧系系主任比约恩•多布拉";
        activity2s.Add(activity13);


        List<ActivityDetail> activity3s = new List<ActivityDetail>();
        ActivityDetail productDetail31 = new ActivityDetail();
        productDetail31.Image = "3-1.jpg";
        productDetail31.Description = "诗情话剧《漫长的告白》取材于法国著名剧作家埃德蒙·罗斯丹的经典悲喜剧《大鼻子情圣》，导演徐俊倾力呈现浪漫唯美的诗情美学，用诗化的戏剧语言敲响钢琴黑白键间满溢的音乐诗篇，吟唱80年代纯真激荡的时代风情。";
        activity3s.Add(productDetail31);
        ActivityDetail productDetail32 = new ActivityDetail();
        productDetail32.Image = "3-2.jpg";
        productDetail32.Description = "知名自媒体人傅踢踢、视觉艺术家马良、著名音乐人李泉鼎力加盟，四个来自60后、70后、80后的上海男人首次跨界合作，倾情演绎永不褪色的“上海爱情故事”。";
        activity3s.Add(productDetail32);
        ActivityDetail productDetail33 = new ActivityDetail();
        productDetail33.Image = "3-3.jpg";
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
        productDetail1.Image = "1-1.jpg";
        productDetail1.Description = "北欧现代简约风格客厅吊灯三室两厅卧室餐厅套装全屋灯具套餐组合。";
        productDetails1.Add(productDetail1);
        ProductDetail productDetail2 = new ProductDetail();
        productDetail2.Image = "1-2.jpg";
        productDetail2.Description = "轻奢后现代水晶吊灯欧式客厅灯大气餐厅灯饰美式铜灯港式别墅灯具。";
        productDetails1.Add(productDetail2);
        ProductDetail productDetail3 = new ProductDetail();
        productDetail3.Image = "1-3.jpg";
        productDetail3.Description = "雷士照明led吸顶灯客厅灯灯具长方形套餐组合简约现代大气卧室灯。";
        productDetails1.Add(productDetail3);
        ProductDetail productDetail4 = new ProductDetail();
        productDetail4.Image = "1-4.jpg";
        productDetail4.Description = "楼中楼欧式玉石水晶大吊灯客厅别墅大厅大灯酒店新中式美式复式楼。";
        productDetails1.Add(productDetail4);

        List<ProductDetail> productDetails2 = new List<ProductDetail>();
        ProductDetail productDetail20 = new ProductDetail();
        productDetail20.Image = "2-1.jpg";
        productDetail20.Description = "新品促销室内小绿萝盆栽吸除甲醛净化空气苗圃直发办公室桌面。";
        productDetails2.Add(productDetail20);
        ProductDetail productDetail21 = new ProductDetail();
        productDetail21.Image = "2-2.jpg";
        productDetail21.Description = "绿萝吊兰虎皮兰新房子装修家用除甲醛盆栽去异味净化空气室内植物。";
        productDetails2.Add(productDetail21);
        ProductDetail productDetail22 = new ProductDetail();
        productDetail22.Image = "2-3.jpg";
        productDetail22.Description = "红枫盆景常年红植物盆栽室内净化空气办公室盆栽四季常青绿植枫树。";
        productDetails2.Add(productDetail22);
        ProductDetail productDetail23 = new ProductDetail();
        productDetail23.Image = "2-4.jpg";
        productDetail23.Description = "蓝雪花耐热盆栽虹越花开不断阳台花园花苗爬藤植物。";
        productDetails2.Add(productDetail23);
        ProductDetail productDetail24 = new ProductDetail();
        productDetail24.Image = "2-5.jpg";
        productDetail24.Description = "MMH多肉植物组合新手套餐肉肉多肉花植物含白瓷花盆花卉盆栽包邮。";
        productDetails2.Add(productDetail24);

        List<ProductDetail> productDetails3 = new List<ProductDetail>();
        ProductDetail productDetail31 = new ProductDetail();
        productDetail31.Image = "3-1.jpg";
        productDetail31.Description = "法颂女士香水持久淡香 香水学生少女清新自然 浪漫梦境礼盒套装。";
        productDetails3.Add(productDetail31);
        ProductDetail productDetail32 = new ProductDetail();
        productDetail32.Image = "3-2.jpg";
        productDetail32.Description = "美国Calvin Klein卡尔文克雷恩进口卡雷优香水ckone果香男女100ml。";
        productDetails3.Add(productDetail32);
        ProductDetail productDetail33 = new ProductDetail();
        productDetail33.Image = "3-3.jpg";
        productDetail33.Description = "冰希黎流沙金女士香水60ml持久淡香学生少女清新幻彩鎏金网红香水。";
        productDetails3.Add(productDetail33);
        ProductDetail productDetail34 = new ProductDetail();
        productDetail34.Image = "3-4.jpg";
        productDetail34.Description = "车载香水座汽车用香薰车上持久淡香车内固体香膏古龙高档男士摆件。";
        productDetails3.Add(productDetail34);
        ProductDetail productDetail35 = new ProductDetail();
        productDetail35.Image = "3-5.jpg";
        productDetail35.Description = "YSL圣罗兰Mon Paris反转巴黎香水 浪漫失魂果EDP。";
        productDetails3.Add(productDetail35);

        List<ProductDetail> productDetails4 = new List<ProductDetail>();
        ProductDetail productDetail41 = new ProductDetail();
        productDetail41.Image = "4-1.jpg";
        productDetail41.Description = "简域实木餐椅现代简约单人书房椅北欧办公家用书桌靠背椅电脑椅子。";
        productDetails4.Add(productDetail41);
        ProductDetail productDetail42 = new ProductDetail();
        productDetail42.Image = "4-2.jpg";
        productDetail42.Description = "伊姆斯椅子现代简约书桌椅家用餐厅靠背椅电脑椅凳子实木北欧餐椅。";
        productDetails4.Add(productDetail42);


        List<ProductDetail>[] pdArra = { productDetails1 , productDetails2, productDetails3, productDetails4 };
        return pdArra[Random.Range(0, pdArra.Length)];
    }

    public List<Video> GetVideos()
    {

        List<Video> videos = new List<Video>();
        videos.Add(GetVideoDetail());
        videos.Add(GetVideoDetail());
        return videos;
    }

    public Video GetVideoDetail()
    {
        // todo
        return new Video().Generator();
    }

    //
    //  获取config
    //
    public AppConfig GetConfigByKey(string key)
    {
        AppConfig appConfig = new AppConfig();
        appConfig.Value = "20";

        if (key.Equals(AppConfig.KEY_CutEffectDuring_CurveStagger))
        {
            appConfig.Value = "20";
        }
        else if (key.Equals(AppConfig.KEY_CutEffectDuring_LeftRightAdjust))
        {
            appConfig.Value = "20";
        }
        else if (key.Equals(AppConfig.KEY_CutEffectDuring_MidDisperseAdjust))
        {
            appConfig.Value = "20";
        }
        else if (key.Equals(AppConfig.KEY_CutEffectDuring_Stars))
        {
            appConfig.Value = "20";
        }
        else if (key.Equals(AppConfig.KEY_CutEffectDuring_UpDownAdjust))
        {
            appConfig.Value = "20";
        }
        else
        {

        }
        return appConfig;
    }

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
        CutEffect[] effects = new CutEffect[] { new CurveStaggerCutEffect(), new LeftRightAdjustCutEffect(),
            new StarsCutEffect(), new MidDisperseCutEffect() , new UpDownAdjustCutEffect(),new FrontBackUnfoldCutEffect() };
        SceneContentType[] contentTypes = new SceneContentType[] { SceneContentType.env, SceneContentType.product, SceneContentType.activity };
 
         for (int i = 0; i < effects.Length; i++) {
            for (int j = 0; j < contentTypes.Length; j++)
            {
                SceneConfig sceneConfig = new SceneConfig();
                sceneConfig.CutEffect = effects[i];
                sceneConfig.SceneContentType = contentTypes[j];
                sceneConfigs.Add(sceneConfig);
            }
          }
            

        //SceneConfig sceneConfig = new SceneConfig();
        //sceneConfig.CutEffect = new CurveStaggerCutEffect();
        //sceneConfig.SceneContentType = SceneContentType.env;
        //sceneConfigs.Add(sceneConfig);

        //SceneConfig sceneConfig2 = new SceneConfig();
        //sceneConfig2.CutEffect = new MidDisperseCutEffect();
        //sceneConfig2.SceneContentType = SceneContentType.product;
        //sceneConfigs.Add(sceneConfig2);

        //SceneConfig sceneConfig3 = new SceneConfig();
        //sceneConfig3.CutEffect = new StarsCutEffect();
        //sceneConfig3.SceneContentType = SceneContentType.activity;
        //sceneConfigs.Add(sceneConfig3);

        //SceneConfig sceneConfig4 = new SceneConfig();
        //sceneConfig4.CutEffect = new MidDisperseCutEffect();
        //sceneConfig4.SceneContentType = SceneContentType.env;
        //sceneConfigs.Add(sceneConfig4);

        //SceneConfig sceneConfig5 = new SceneConfig();
        //sceneConfig5.CutEffect = new UpDownAdjustCutEffect();
        //sceneConfig5.SceneContentType = SceneContentType.product;
        //sceneConfigs.Add(sceneConfig5);

        //SceneConfig sceneConfig6 = new SceneConfig();
        //sceneConfig6.CutEffect = new FrontBackUnfoldCutEffect();
        //sceneConfig6.SceneContentType = SceneContentType.activity;
        //sceneConfigs.Add(sceneConfig6);

        return sceneConfigs;
    }

}
