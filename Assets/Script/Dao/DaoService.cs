
using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;


//
//  数据仓库模块
//

namespace MagicWall
{
    public class DaoService : MonoBehaviour, IDaoService
    {
        private TheDataSource _theDataSource;
        private MagicWallManager _manager;

        MWConfig _config;

        List<Enterprise> _enterprises;
        int _enterpriseIndex;
        List<Activity> _activities;
        int _activityIndex;
        List<Product> _products;
        int _productIndex;

        private IDaoSubService _daoSubService;  // 次实现数据层索引


        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(MagicWallManager manager)
        {
            _theDataSource = TheDataSource.Instance;
            _manager = manager;

            _enterprises = new List<Enterprise>();
            _activities = new List<Activity>();
            _products = new List<Product>();

            _enterpriseIndex = 0;
            _activityIndex = 0;
            _productIndex = 0;


            //// 初始化显示的数据
            //_enterprises = GetEnterprises();
            //_activities = GetActivities();
            //_products = GetProducts();

        }

        #region Enterprise
        public List<Enterprise> GetEnterprises()
        {

            if (_enterprises.Count == 0)
            {
                _enterprises = _daoSubService.GetEnterprises(_config.ThemeId);

                Debug.Log("_enterprises : " + _enterprises.Count);

            }

            return _enterprises;
        }

        public Enterprise GetEnterpriseById(int id)
        {
            Enterprise enterprise = new Enterprise();

            string sql = "select * from company where com_id=" + id + " and status = 1";

            var row = _theDataSource.SelectOne(sql);

            enterprise.Ent_id = Convert.ToInt16(row["com_id"]);
            enterprise.Name = row["name"].ToString();
            bool isCustom = false;
            enterprise.IsCustom = isCustom;
            enterprise.Logo = row["logo"].ToString();

            //enterprise.Description = row["description"].ToString();
            enterprise.Description = row["description"].ToString();
            enterprise.Business_card = row["image_namecard"].ToString();
            enterprise.EnvCards = GetEnvCards(enterprise.Ent_id);

            return enterprise;
        }

        public EnterpriseDetail GetEnterprisesDetail(int com_id)
        {
            EnterpriseDetail enterpriseDetail = new EnterpriseDetail();

            enterpriseDetail.Enterprise = GetEnterpriseById(com_id);

            enterpriseDetail.products = GetProductsByEnvId(com_id);

            enterpriseDetail.catalog = GetCatalogs(com_id);

            enterpriseDetail.activities = GetActivitiesByEnvId(com_id);

            enterpriseDetail.videos = GetVideosByEnvId(com_id);

            return enterpriseDetail;
        }

        public List<string> GetEnvCards(int id)
        {
            //Debug.Log("GetEnvCards：" + id);
            List<string> envCards = new List<string>();

            string sql = "select * from company where com_id=" + id + " and status = 1";

            var row = _theDataSource.SelectOne(sql);
            if (row == null)
            {
                return envCards;
            }
            JsonData data = JsonMapper.ToObject(row["image_card"].ToString());
            for (int i = 0; i < data.Count; i++)
            {
                envCards.Add(data[i].ToString());
            }

            return envCards;
        }

        #endregion


        public List<Activity> GetActivities()
        {
            if (_activities.Count == 0)
            {

                _activities = _daoSubService.GetActivities(_config.ThemeId);
            }

            return _activities;
        }

        public Activity GetActivityDetail(int act_id)
        {
            //Debug.Log("act_id:" + act_id);
            Activity activity = new Activity();

            string sql = "select * from activity where act_id=" + act_id + " and status = 1";

            var row = _theDataSource.SelectOne(sql);
            if (row != null)
            {
                activity.Id = Convert.ToInt16(row["act_id"]);
                activity.Ent_id = Convert.ToInt16(row["com_id"]);
                activity.Name = row["name"].ToString();
                activity.Image = row["image"].ToString();
                activity.ActivityDetails = GetActivityDetails(act_id);
            }

            return activity;
        }

        public List<ActivityDetail> GetActivityDetails(int act_id)
        {
            List<ActivityDetail> activityDetails = new List<ActivityDetail>();
            string sql = "select * from activity where act_id='" + act_id + "'" + " and status = 1";

            var row = _theDataSource.SelectOne(sql);
            string material = row["material"].ToString();

            List<MWMaterial> mWMaterials = (List<MWMaterial>)DaoUtil.ConvertMaterialJson(material);

            for (int i = 0; i < mWMaterials.Count; i++)
            {
                MWMaterial data = mWMaterials[i];
                //Debug.Log(data[i]["cuteffect_id"]);
                ActivityDetail activityDetail = new ActivityDetail();
                if (data.type == "video")
                {
                    activityDetail.Type = 1;
                    activityDetail.Image = data.cover;
                    activityDetail.VideoUrl = data.path;
                }
                else if (data.type == "image")
                {
                    activityDetail.Type = 0;
                    activityDetail.Image = data.path;
                }
                activityDetail.Description = data.description;
                activityDetails.Add(activityDetail);
            }

            return activityDetails;
        }

        public List<Catalog> GetCatalogs(int id)
        {
            List<Catalog> catalogs = new List<Catalog>();
            string sql = "select catalog from company where com_id='" + id + "'" + " and status = 1";

            var row = _theDataSource.SelectOne(sql);
            string catalogStr = row["catalog"].ToString();

            //Debug.Log("id : " + id + " catalogStr : " + catalogStr);

            JsonData data = JsonMapper.ToObject(catalogStr);
            for (int i = 0; i < data.Count; i++)
            {
                Catalog catalog = new Catalog();
                catalog.Img = data[i].ToString();
                //catalog.Description = descriptions[i];
                catalogs.Add(catalog);
            }

            return catalogs;
        }

        /// <summary>
        ///     根据 Config Key 获取配置信息，此时 Global Data 已经初始化
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public AppConfig GetConfigByKey(string key)
        {
            AppConfig appConfig = new AppConfig();

            var config = _manager.globalData.GetConfig();

            appConfig = appConfig.GetConfigByMWConfig(config, key);

            //string sql = "select value from config c where c.key='" + key + "'";
            //var row = _theDataSource.SelectOne(sql);

            //if (row != null)
            //{
            //    appConfig.Value = row["value"].ToString();
            //}
            return appConfig;
        }

        public List<string> GetCustomImage(CustomImageType type)
        {

            throw new System.NotImplementedException();
        }


        public FlockData GetFlockData(DataType type)
        {
            if (type == DataType.env)
            {
                return GetEnterprise();
            }
            else if (type == DataType.product)
            {
                return GetProduct();
            }
            else if (type == DataType.activity)
            {
                return GetActivity();
            }
            return null;
        }

        //
        //  获取首页企业
        //
        public Enterprise GetEnterprise()
        {
            List<Enterprise> enterprises = GetEnterprises();

            Enterprise r = enterprises[_enterpriseIndex];

            int number = _enterpriseIndex + 1;

            if (number == _enterprises.Count)
            {
                _enterpriseIndex = 0;
            }
            else
            {
                _enterpriseIndex = number;
            }

            return r;
        }

        public Product GetProduct()
        {
            List<Product> products = GetProducts();

            var r = products[_productIndex];

            int number = _productIndex + 1;

            if (number == products.Count)
            {
                _productIndex = 0;
            }
            else
            {
                _productIndex = number;
            }

            return r;
        }

        public Activity GetActivity()
        {

            List<Activity> activities = GetActivities();

            var r = activities[_activityIndex];

            int number = _activityIndex + 1;

            if (number == activities.Count)
            {
                _activityIndex = 0;
            }
            else
            {
                _activityIndex = number;
            }

            return r;
        }

        public int GetLikes(int id, CrossCardCategoryEnum category)
        {
            // TODO likes
            return 1;
        }

        public int GetLikesByActivityDetail(int id)
        {
            // TODO likes

            return 1;
        }

        public int GetLikesByProductDetail(int id)
        {
            // TODO likes 

            return 1;
        }

        public Product GetProductDetail(int pro_id)
        {
            Product product = new Product();

            string sql = "select * from product where prod_id=" + pro_id;

            Debug.Log("GetProductDetail : " + sql);

            var row = _theDataSource.SelectOne(sql);

            if (row != null)
            {
                product.Pro_id = Convert.ToInt16(row["prod_id"]);
                product.Ent_id = Convert.ToInt16(row["com_id"]);
                product.Name = row["name"].ToString();
                product.Image = row["image"].ToString();
                product.ProductDetails = GetProductDetails(pro_id);
            }

            return product;
        }

        public List<ProductDetail> GetProductDetails(int pro_id)
        {
            List<ProductDetail> productDetails = new List<ProductDetail>();

            string sql = "select * from product where prod_id=" + pro_id;
            var row = _theDataSource.SelectOne(sql);
            if (row != null)
            {
                string material = row["material"].ToString();

                List<MWMaterial> mWMaterials = (List<MWMaterial>)DaoUtil.ConvertMaterialJson(material);

                for (int i = 0; i < mWMaterials.Count; i++)
                {
                    MWMaterial data = mWMaterials[i];
                    //Debug.Log(data[i]["cuteffect_id"]);
                    ProductDetail productDetail = new ProductDetail();

                    if (data.type == "video")
                    {
                        productDetail.Type = 1;
                        productDetail.Image = data.cover;
                        productDetail.VideoUrl = data.path;
                    }
                    else if (data.type == "image")
                    {
                        productDetail.Type = 0;
                        productDetail.Image = data.path;
                    }
                    productDetail.Description = data.description;
                    productDetails.Add(productDetail);
                }
            }
            return productDetails;
        }

        public List<Product> GetProducts()
        {
            if (_products.Count == 0)
            {
                _products = _daoSubService.GetProducts(_config.ThemeId);

            }

            return _products;
        }

        public List<SceneConfig> GetShowConfigs()
        {
            List<SceneConfig> sceneConfigs = new List<SceneConfig>();

            string showConfigStr = _manager.globalData.GetConfig().ShowConfig;
            SceneTypeEnum[] sceneTypes = new SceneTypeEnum[]
            {
            SceneTypeEnum.CurveStagger,
            SceneTypeEnum.FrontBackUnfold,
            SceneTypeEnum.LeftRightAdjust,
            SceneTypeEnum.MidDisperse,
            SceneTypeEnum.Stars,
            SceneTypeEnum.UpDownAdjustCutEffect,
            };

            DataTypeEnum[] dataTypes = new DataTypeEnum[] {
            DataTypeEnum.Enterprise,
            DataTypeEnum.Activity,
            DataTypeEnum.Product,
        };


            JsonData data = JsonMapper.ToObject(DaoUtil.ConvertShowConfigStr(showConfigStr));

            for (int i = 0; i < data.Count; i++)
            {
                //Debug.Log(data[i]["cuteffect_id"]);
                SceneConfig sceneConfig = new SceneConfig();
                sceneConfig.sceneType = sceneTypes[int.Parse(data[i]["cuteffect_id"].ToString()) - 1];
                sceneConfig.dataType = dataTypes[int.Parse(data[i]["contcom_type"].ToString())];
                // 设置场景时间
                sceneConfig.durtime = GetSceneDurTime(sceneConfig.sceneType);
                //Debug.Log("sceneType:" + sceneConfig.sceneType + "---dataType:" + sceneConfig.dataType + "---durtime:" + sceneConfig.durtime);
                sceneConfigs.Add(sceneConfig);
            }

            return sceneConfigs;
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


        public Video GetVideoDetail(int envId, int index)
        {
            return GetVideosByEnvId(envId)[index];
        }

        public List<Video> GetVideosByEnvId(int envId)
        {
            List<Video> videos = new List<Video>();

            string sql = "select video from company where com_id=" + envId + " and status = 1";

            //Debug.Log("GetVideosByEnvId : " + sql);

            var row = _theDataSource.SelectOne(sql);
            if (row != null)
            {
                string material = row["video"].ToString();

                object mWMaterialsObj = DaoUtil.ConvertMaterialJson(material);
                if (mWMaterialsObj != null)
                {

                    List<MWMaterial> mWMaterials = (List<MWMaterial>)mWMaterialsObj;

                    for (int i = 0; i < mWMaterials.Count; i++)
                    {
                        MWMaterial data = mWMaterials[i];
                        //Debug.Log(data[i]["cuteffect_id"]);
                        Video video = new Video();
                        video.V_id = i;
                        if (data.type == "video")
                        {
                            video.Cover = data.cover;
                            video.Address = data.path;
                        }
                        video.Description = data.description;
                        videos.Add(video);
                    }

                }
            }

            return videos;
        }



        public bool IsCustom()
        {
            return _manager.globalData.GetConfig().ShowType == MWConfig.ShowType_Custom;
        }

        public List<SearchBean> Search(string keys)
        {
            List<SearchBean> searchBeans = new List<SearchBean>();
            if (keys == null || keys.Length == 0)
            {
                return searchBeans;
            }

            string sql = string.Format("select a.act_id as id,a.image, 1 as 'type' from activity a where name like '%{0}%' " +
                "UNION all select b.com_id as id,b.logo as image, 2 as 'type' from company b where name like '%{0}%' " +
                "UNION all select c.prod_id as id,c.image, 3 as 'type' from product c where name like '%{0}%'", keys);
            var rows = _theDataSource.SelectList(sql);

            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                SearchBean searchBean = new SearchBean();
                searchBean.id = Convert.ToInt16(row["id"]);
                searchBean.cover = row["image"].ToString();
                int type = Convert.ToInt16(row["type"]);
                if (type == 1)
                {
                    searchBean.type = DataTypeEnum.Activity;
                }
                else if (type == 2)
                {
                    searchBean.type = DataTypeEnum.Enterprise;
                }
                else if (type == 3)
                {
                    searchBean.type = DataTypeEnum.Product;
                }
                searchBeans.Add(searchBean);
            }

            return searchBeans;
        }

        public List<Activity> GetActivitiesByEnvId(int envid)
        {
            List<Activity> activities = new List<Activity>();

            string sql = "select * from activity where com_id=" + envid + " and status = 1";
            var rows = _theDataSource.SelectList(sql);

            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                Activity activity = new Activity();
                activity.Id = Convert.ToInt16(row["act_id"]);
                activity.Ent_id = Convert.ToInt16(row["com_id"]);
                activity.Name = row["name"].ToString();
                activity.Image = row["image"].ToString();
                activity.ActivityDetails = GetActivityDetails(activity.Id);
                activities.Add(activity);
            }

            return activities;
        }

        public List<Product> GetProductsByEnvId(int envid)
        {
            List<Product> products = new List<Product>();

            string sql = "select * from product where com_id=" + envid + " and status = 1";
            var rows = _theDataSource.SelectList(sql);

            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                Product product = new Product();
                product.Pro_id = Convert.ToInt16(row["prod_id"]);
                product.Ent_id = Convert.ToInt16(row["com_id"]);
                product.Name = row["name"].ToString();
                product.Image = row["image"].ToString();
                product.ProductDetails = GetProductDetails(product.Pro_id);
                products.Add(product);
            }

            return products;
        }

        /// <summary>
        ///  TODO config 的获取方法可能需要修改
        /// </summary>
        /// <returns></returns>
        public MWConfig GetConfig()
        {
            MWConfig mWConfig = new MWConfig();
            string sql = "select * from config";

            var rows = _theDataSource.SelectList(sql);

            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i]["key"].ToString() == "show_type")
                {
                    mWConfig.ShowType = int.Parse(rows[i]["value"].ToString());
                }
                if (rows[i]["key"].ToString() == "image_background")
                {
                    mWConfig.ImageBackground = int.Parse(rows[i]["value"].ToString());
                }
                if (rows[i]["key"].ToString() == "show_animation")
                {
                    mWConfig.ShowAnimation = int.Parse(rows[i]["value"].ToString());
                }
                if (rows[i]["key"].ToString() == "cuteffectduring_curvestagger")
                {
                    mWConfig.CutEffectDuringCurvestagger = int.Parse(rows[i]["value"].ToString());
                }
                if (rows[i]["key"].ToString() == "cuteffectduring_leftrightadjust")
                {
                    mWConfig.CutEffectDuringLeftRightAdjust = int.Parse(rows[i]["value"].ToString());
                }
                if (rows[i]["key"].ToString() == "cuteffectduring_middisperse")
                {
                    mWConfig.CutEffectDuringMidDisperse = int.Parse(rows[i]["value"].ToString());
                }
                if (rows[i]["key"].ToString() == "cuteffectduring_stars")
                {
                    mWConfig.CutEffectDuringStars = int.Parse(rows[i]["value"].ToString());
                }
                if (rows[i]["key"].ToString() == "cuteffectduring_updownadjust")
                {
                    mWConfig.CutEffectDuringUpDownAdjust = int.Parse(rows[i]["value"].ToString());
                }
                if (rows[i]["key"].ToString() == "cuteffectduring_frontbackrightpullopen")
                {
                    mWConfig.CutEffectDuringFrontBackRightPullOpen = int.Parse(rows[i]["value"].ToString());
                }
                if (rows[i]["key"].ToString() == "show_config")
                {
                    mWConfig.ShowConfig = rows[i]["value"].ToString();
                }
                if (rows[i]["key"].ToString() == "theme_id")
                {
                    mWConfig.ThemeId = int.Parse(rows[i]["value"].ToString());
                }
            }


            return mWConfig;
        }

        public void InitData()
        {
            // 初始化
            _config = _manager.globalData.GetConfig();

            if (_config.ShowType == MWConfig.ShowType_Common)
            {
                // 初始化
                _daoSubService = new CommonSubDaoService();
            }
            else
            {
                _daoSubService = new CustomSubDaoService();
            }

            GetEnterprises();
            GetActivities();
            GetProducts();

        }


        public int GetLikes(string path)
        {
            var likes = _theDataSource.GetLikeDataBase();
            int r = 0;

            for (int i = 0; i < likes.list.Count; i++)
            {
                var like = likes.list[i];
                if (like.Path == path)
                {
                    r = like.Number;
                    break;
                }
            }

            return r;
        }

        public bool UpdateLikes(string path)
        {
            //Debug.Log("更新喜欢数:" + path);
            try
            {
                var likes = _theDataSource.GetLikeDataBase();

                bool hasPath = false;

                for (int i = 0; i < likes.list.Count; i++)
                {
                    var like = likes.list[i];
                    if (like.Path == path)
                    {
                        hasPath = true;
                        like.Number = like.Number + 1;
                        break;
                    }
                }

                if (!hasPath)
                {
                    var like = new Like();
                    like.Path = path;
                    like.Number = 1;
                    likes.list.Add(like);
                }


                _theDataSource.SaveLikes();
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
            finally
            {

            }

            return false;
        }

        public FlockData GetFlockData(DataTypeEnum type)
        {
            throw new NotImplementedException();
        }
    }
}