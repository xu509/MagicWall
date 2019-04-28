using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enterprise : Generator<Enterprise>
{
    #region Data Parameter

    // 企业 ID
    private int _ent_id;
    public int Ent_id { set { _ent_id = value; } get { return _ent_id; } }

    //  企业名片
    private string _business_card;
    public string Business_card { set { _business_card = value; } get { return _business_card; } }

    // 企业的logo
    private string _logo;
    public string Logo { set { _logo = value; } get { return _logo; } }
    
      // 企业名字
    private string _name;
    public string Name { set { _name = value; } get { return _name; } }

    // catalog
    private string _catalog;
    public string Catalog { set { _catalog = value; } get { return _catalog; } }

    private bool _isCustom;
    public bool IsCustom { set { _isCustom = value; } get { return _isCustom; } }

    //  描述
    private string _description;
    public string Description { set { _description = value; } get { return _description; } }

    //  喜欢数量 - 针对企业
    private int _likes;
    public int likes { set { _likes = value; } get { return _likes; } }

    #endregion

    #region Component Parameter
    private Texture _texture_logo;
    public Texture TextureLogo { set { _texture_logo = value; } get { return _texture_logo; } }

    private Texture _texture_business_card;
    public Texture TextureBusinessCard { set { _texture_business_card = value; } get { return _texture_business_card; } }
    #endregion


    public Enterprise Generator()
    {
        string[] names = new string[] { "百度","可口可乐","谷歌","阿里巴巴","豆瓣","哔哩哔哩","微软","搜狗","印象笔记","迅雷"};
        string[] logos = new string[] {
            "1.jpg","2.jpg","3.jpg","4.jpg","5.jpg","6.jpg","7.jpg","8.jpg","9.jpg","10.jpg",
            "11.jpg","12.jpg","13.jpg","14.jpg","15.jpg","16.jpg","17.jpg","18.jpg","19.jpg","20.jpg",
            "21.jpg","22.jpg","23.jpg","24.jpg","25.jpg","26.jpg","27.jpg","28.jpg","29.jpg","30.jpg",
            "31.jpg","32.jpg"
        };
        string[] descriptions = new string[] {
            "京东商城京东JD.COM-专业的综合网上购物商城，销售超数万品牌、4020万种商品，囊括家电、手机、电脑、母婴、服装等13大品类。秉承客户为先的理念，京东所售商品为正品行货、全国联保、机打发票。",
            "淘宝网是亚太地区较大的网络零售、商圈，由阿里巴巴集团在2003年5月创立。淘宝网是中国深受欢迎的网购零售平台，拥有近5亿的注册用户数，每天有超过6000万的固定访客，同时每天的在线商品数已经超过了8亿件，平均每分钟售出4.8万件商品。",
            "“海澜之家”（英文缩写：HLA）是海澜之家股份有限公司旗下的服装品牌，主要采用连锁零售的模式，销售男性服装、配饰与相关产品。",
            "Unity3D是由Unity Technologies开发的一个让玩家轻松创建诸如三维视频游戏、建筑可视化、实时三维动画等类型互动内容的多平台的综合型游戏开发工具，是一个全面整合的专业游戏引擎。"
        };

        string[] businessCards = new string[] {
            "business-card-1.png",
            "business-card-2.png",
            "business-card-3.png"
        };


        bool[] customs = new bool[] { true,false};

        Enterprise env = new Enterprise();
        int id = Random.Range(1, 10);
        env._ent_id = id;

        int name_index = Random.Range(0, names.Length - 1);
        env._name = names[name_index];

        int logo_index = Random.Range(0, logos.Length - 1);
        env._logo = logos[logo_index];

        env._isCustom = customs[Random.Range(0, 2)];

        env._description = descriptions[Random.Range(0, descriptions.Length - 1)];

        env.likes = Random.Range(1, 1000);

        env._business_card = businessCards[Random.Range(0, businessCards.Length - 1)];

        env.TextureBusinessCard = AppUtils.LoadPNG(MagicWallManager.URL_ASSET + "env\\" + env._business_card);

        return env;
    }
}
