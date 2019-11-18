
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///    飞跃定制屏模拟数据
/// </summary>
namespace MagicWall
{
    public class MockShicunDaoService : MonoBehaviour, IDaoService
    {

        private List<Enterprise> _enterprises;
        private List<Activity> _activities;


        private List<Product> _products;
        private List<Product> _products2;

        private List<Product> _wines;

        private Dictionary<int, Product> _productMap;



        void Awake()
        {


        }

        //
        //  Construct
        //
        protected MockShicunDaoService() { }



        public void Init()
        {
            _enterprises = new List<Enterprise>();
            _activities = new List<Activity>();
            _products = new List<Product>();
            _products2 = new List<Product>();

            _productMap = new Dictionary<int, Product>();
        }

        public void Reset()
        {
            Init();
        }


        //
        //  获取首页企业
        //
        public List<Enterprise> GetEnterprises()
        {
            throw new System.NotImplementedException();
        }

        //
        //  获取首页企业
        //
        public Enterprise GetEnterprise()
        {           
            throw new System.NotImplementedException();

        }

        public List<string> GetEnvCards(int id)
        {
            List<string> list = new List<string>();



            return null;

        }


        //
        //  获取 catalogs
        //
        public List<Catalog> GetCatalogs(int id)
        {
            throw new System.NotImplementedException();
        }



        //
        //  获取企业的详细信息
        //
        public EnterpriseDetail GetEnterprisesDetail(int com_id)
        {
            throw new System.NotImplementedException();
        }

        //
        //  获取首页活动
        //
        public List<Activity> GetActivities()
        {
            throw new System.NotImplementedException();
        }

        //
        //  获取首页活动
        //
        public Activity GetActivity()
        {
            throw new System.NotImplementedException();
        }


        //
        //  获取首页活动的详细信息
        //
        public Activity GetActivityDetail(int act_id)
        {
            throw new System.NotImplementedException();
        }

        public List<ActivityDetail> GetActivityDetails(int act_id)
        {
            throw new System.NotImplementedException();
        }


        //
        //  获取首页的产品
        //
        public List<Product> GetProducts()
        {
            return _products;

        }

        private void AddProduct(string[] details, string name, int proid)
        {
            int pro1Id = 70;

            for (int i = 0; i < details.Length; i++)
            {
                Product product = new Product();
                product.Name = name;

                int pro_id = 0;
                int.TryParse(pro1Id.ToString() + proid.ToString() + i.ToString(), out pro_id);
                product.Pro_id = pro_id;
                product.Ent_id = 0;
                product.Description = name;
                product.Image = details[i];

                // 包裹detail

                List<ProductDetail> productDetails = new List<ProductDetail>();
                for (int y = 0; y < details.Length; y++)
                {
                    var mat = details[y];

                    ProductDetail productDetail = new ProductDetail();
                    productDetail.Id = y;
                    productDetail.Type = 0;
                    productDetail.Pro_id = pro_id;
                    productDetail.Description = name;
                    productDetail.Image = mat;                   
                    productDetails.Add(productDetail);
                }

                //// todo 添加视频
                //ProductDetail productDetailVideo = new ProductDetail();
                //productDetailVideo.Id = 99;
                //productDetailVideo.Type = 1;
                //productDetailVideo.Pro_id = pro_id;
                //productDetailVideo.Description = "《中国之造ChinaMade》";
                //productDetailVideo.Image = "shicun\\互动魔墙里的酒图片\\feiyue1_Moment.png";
                //productDetailVideo.VideoUrl = "shicun\\互动魔墙里的酒图片\\feiyue1.mp4";
                //productDetails.Add(productDetailVideo);

                // 调整选中的图片为第一张图片
                //productDetails
                var temp = productDetails[0];
                var tempC = productDetails[i];
                productDetails[0] = tempC;
                productDetails[i] = temp;



                product.ProductDetails = productDetails;


                //Debug.Log(pro_id);

                _productMap.Add(pro_id, product);
                _products.Add(product);
            }
        }

        private void AddProduct2(string[] details, string name, int proid)
        {
            int pro2Id = 80;

            for (int i = 0; i < details.Length; i++)
            {
                Product product = new Product();
                product.Name = name;

                int pro_id = 0;
                int.TryParse(pro2Id.ToString() + proid.ToString() + i.ToString(), out pro_id);
                product.Pro_id = pro_id;
                product.Ent_id = 0;
                product.Description = name;
                product.Image = details[i];

                // 包裹detail

                List<ProductDetail> productDetails = new List<ProductDetail>();
                for (int y = 0; y < details.Length; y++)
                {
                    var mat = details[y];

                    ProductDetail productDetail = new ProductDetail();
                    productDetail.Id = y;
                    productDetail.Type = 0;
                    productDetail.Pro_id = pro_id;
                    productDetail.Description = name;
                    productDetail.Image = mat;
                    productDetails.Add(productDetail);
                }

                // 调整选中的图片为第一张图片
                //productDetails
                var temp = productDetails[0];
                var tempC = productDetails[i];
                productDetails[0] = tempC;
                productDetails[i] = temp;

                product.ProductDetails = productDetails;


                //Debug.Log(pro_id);

                _productMap.Add(pro_id, product);
                _products2.Add(product);
            }
        }


        private void AddWines(string[] details, string name, int proid)
        {
            int wineId = 90;


            for (int i = 0; i < details.Length; i++)
            {
                Product product = new Product();
                product.Name = name;

                int pro_id = 0;
                int.TryParse(wineId.ToString() + proid.ToString() + i.ToString(), out pro_id);
                product.Pro_id = pro_id;
                product.Ent_id = 0;
                product.Description = name;
                product.Image = details[i];

                // 包裹detail

                List<ProductDetail> productDetails = new List<ProductDetail>();
                for (int y = 0; y < details.Length; y++)
                {
                    var mat = details[y];

                    ProductDetail productDetail = new ProductDetail();
                    productDetail.Id = y;
                    productDetail.Type = 0;
                    productDetail.Pro_id = pro_id;
                    productDetail.Description = name;
                    productDetail.Image = mat;
                    productDetails.Add(productDetail);
                }

                // 调整选中的图片为第一张图片
                //productDetails
                var temp = productDetails[0];
                var tempC = productDetails[i];
                productDetails[0] = tempC;
                productDetails[i] = temp;

                product.ProductDetails = productDetails;
                //Debug.Log(pro_id);

                _productMap.Add(pro_id, product);
                _wines.Add(product);
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
            var product = _productMap[pro_id];

            return product;
        }

        public List<ProductDetail> GetProductDetails(int pro_id)
        {
            List<ProductDetail> productDetails = new List<ProductDetail>();

            return productDetails;
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
                appConfig.Value = "40";
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
            appConfig.Value = "10";

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


        public bool IsCustom()
        {
            //TODO
            int number = Random.Range(0, 5);
            return number > 2;

            return true;

        }




        //
        //  TODO 获取定制屏所配置的图片
        //
        public List<string> GetCustomImage(CustomImageType type)
        {
            string[] leftImages = { "shicun\\互动魔墙里的酒图片\\第一屏.jpg", "shicun\\互动魔墙里的酒图片\\第一屏1.jpg" };
            //string[] middleImages = { "m1.jpg", "m2.jpg", "m3.jpg", "m4.jpg", "m5.jpg" };
            string[] rightImages = { "shicun\\互动魔墙里的酒图片\\第五屏.jpg" };

            if (type == CustomImageType.LEFT1)
            {
                List<string> images = new List<string>();
                int size = leftImages.Length;
                for (int i = 0; i < size; i++)
                {
                    images.Add(leftImages[i]);
                }
                return images;
            }
            //else if (type == CustomImageType.LEFT2)
            //{
            //    List<string> images = new List<string>();
            //    int size = Random.Range(1, 4);
            //    size = 5;
            //    for (int i = 0; i < size; i++)
            //    {
            //        images.Add("custom\\" + middleImages[i]);
            //    }
            //    return images;
            //}
            else
            {
                List<string> images = new List<string>();
                int size = rightImages.Length;
                for (int i = 0; i < size; i++)
                {
                    images.Add(rightImages[i]);
                }
                return images;
            }
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="keys">关键词</param>
        /// <returns></returns>
        public List<SearchBean> Search(string keys)
        {
            //Debug.Log("搜索KEYS ：" + keys);

            List<SearchBean> beans = new List<SearchBean>();

            for (int i = 0; i < _products.Count; i++)
            {
                var name = _products[i].Name;
                if (name.Contains(keys))
                {
                    SearchBean bean = new SearchBean();
                    bean.type = DataTypeEnum.Product;
                    bean.id = _products[i].Pro_id;
                    bean.cover = _products[i].Image;
                    beans.Add(bean);
                }
            }

            for (int i = 0; i < _products2.Count; i++)
            {
                var name = _products2[i].Name;
                if (name.Contains(keys))
                {
                    SearchBean bean = new SearchBean();
                    bean.type = DataTypeEnum.Product;
                    bean.id = _products2[i].Pro_id;
                    bean.cover = _products2[i].Image;
                    beans.Add(bean);
                }
            }

            for (int i = 0; i < _wines.Count; i++)
            {
                var name = _wines[i].Name;
                if (name.Contains(keys))
                {
                    SearchBean bean = new SearchBean();
                    bean.type = DataTypeEnum.Wine;
                    bean.id = _wines[i].Pro_id;
                    bean.cover = _wines[i].Image;
                    beans.Add(bean);
                }
            }

            return beans;
        }


        /// <summary>
        ///     获得浮动块数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
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

        public Enterprise GetEnterpriseById(int id)
        {
            Enterprise enterprise = new Enterprise();

            // 公司卡片初始化
            //List<string> list = new List<string>();

            //list.Add("shicun\\互动魔墙里的酒图片\\企业名片1.jpg");
            //list.Add("shicun\\互动魔墙里的酒图片\\企业名片2.jpg");
            //list.Add("shicun\\互动魔墙里的酒图片\\企业名片3.jpg");

            //enterprise.EnvCards = list;





            return null;
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

        public MWConfig GetConfig()
        {
            //Debug.Log("Mock Config");
            return new MWConfig();
        }

        public void InitData()
        {
            //初始化数据
            _productMap = new Dictionary<int, Product>();
            _products = null;
            _products2 = null;
            _wines = null;


            #region 设置酒庄产品
            if (_products == null)
            {

                string[] names = {
                "酒会活动",
                "酒窖",
                "酒具用品",
                "卡利酒庄",
                "丽隆红酒包装礼盒",
                "丽隆红酒Chateau de Rochemorin先哲堡",
                "丽隆红酒Chateau La Louvere露浮堡",
                "默顿红酒主视觉",
                "青园别院",
                "元庐会所"
            };

                //Dictionary<int, List<string>> images = new Dictionary<int, List<string>>();

                List<string[]> imageCovers = new List<string[]>();



                string[] i2 = {
                "shicun\\互动魔墙里的酒图片\\酒会活动\\第七届国际研讨班.jpg",
                "shicun\\互动魔墙里的酒图片\\酒会活动\\第五届亚洲葡萄酒大赛部分专家评委.JPG",
                "shicun\\互动魔墙里的酒图片\\酒会活动\\第五届亚洲葡萄酒大赛部分专家评委合影.JPG",
                "shicun\\互动魔墙里的酒图片\\酒会活动\\国际顶级葡萄酒品牌默顿专场品鉴晚宴.jpg",
                "shicun\\互动魔墙里的酒图片\\酒会活动\\西北农林科技大学副校长、中国葡萄酒专家李华.jpg"
            };
                imageCovers.Add(i2);


                string[] i3 = {
                "shicun\\互动魔墙里的酒图片\\酒窖\\酒窖-01.JPG",
                "shicun\\互动魔墙里的酒图片\\酒窖\\酒窖-03.JPG",
                "shicun\\互动魔墙里的酒图片\\酒窖\\酒窖-04.JPG",
                "shicun\\互动魔墙里的酒图片\\酒窖\\酒窖-05.jpg",
                "shicun\\互动魔墙里的酒图片\\酒窖\\酒窖-06.JPG"
            };
                imageCovers.Add(i3);


                string[] i4 = {
                "shicun\\互动魔墙里的酒图片\\酒具用品\\酒柜-大.jpg",
                "shicun\\互动魔墙里的酒图片\\酒具用品\\开瓶器.jpg",
                "shicun\\互动魔墙里的酒图片\\酒具用品\\双支皮盒.jpg",
                "shicun\\互动魔墙里的酒图片\\酒具用品\\调酒器具.jpg",
                "shicun\\互动魔墙里的酒图片\\酒具用品\\为了达到适宜饮用温度，起泡酒、干白通常需要在冰桶中存放.jpg"
            };
                imageCovers.Add(i4);


                string[] i5 = {
                "shicun\\互动魔墙里的酒图片\\卡利酒庄\\卡利酒庄.JPG",
                "shicun\\互动魔墙里的酒图片\\卡利酒庄\\卡利酒庄2.JPG",
                "shicun\\互动魔墙里的酒图片\\卡利酒庄\\卡利酒庄3.JPG",
                "shicun\\互动魔墙里的酒图片\\卡利酒庄\\葡萄园.JPG",
                "shicun\\互动魔墙里的酒图片\\卡利酒庄\\葡萄园2.JPG",
            };
                imageCovers.Add(i5);


                string[] i7 = {
                "shicun\\互动魔墙里的酒图片\\丽隆红酒包装礼盒\\包装礼盒.jpg"
            };
                imageCovers.Add(i7);


                string[] i8 = {
                "shicun\\互动魔墙里的酒图片\\丽隆红酒Chateau de Rochemorin先哲堡\\先哲堡-1.jpg",
                "shicun\\互动魔墙里的酒图片\\丽隆红酒Chateau de Rochemorin先哲堡\\先哲堡-2.jpg",
                "shicun\\互动魔墙里的酒图片\\丽隆红酒Chateau de Rochemorin先哲堡\\先哲堡-3.jpg",
                "shicun\\互动魔墙里的酒图片\\丽隆红酒Chateau de Rochemorin先哲堡\\先哲堡-4.jpg",
                "shicun\\互动魔墙里的酒图片\\丽隆红酒Chateau de Rochemorin先哲堡\\先哲堡-5.jpg",
            };
                imageCovers.Add(i8);


                string[] i9 = {
                "shicun\\互动魔墙里的酒图片\\丽隆红酒Chateau La Louvere露浮堡\\露浮堡-1.jpg",
                "shicun\\互动魔墙里的酒图片\\丽隆红酒Chateau La Louvere露浮堡\\露浮堡-2.jpg",
                "shicun\\互动魔墙里的酒图片\\丽隆红酒Chateau La Louvere露浮堡\\露浮堡-3.jpg",
                "shicun\\互动魔墙里的酒图片\\丽隆红酒Chateau La Louvere露浮堡\\露浮堡-4.jpg",
                "shicun\\互动魔墙里的酒图片\\丽隆红酒Chateau La Louvere露浮堡\\露浮堡-12.jpg",
            };
                imageCovers.Add(i9);


                string[] i16 = {
                "shicun\\互动魔墙里的酒图片\\默顿红酒主视觉\\主视觉-1.jpg",
                "shicun\\互动魔墙里的酒图片\\默顿红酒主视觉\\主视觉-2.jpg",
                "shicun\\互动魔墙里的酒图片\\默顿红酒主视觉\\主视觉-3.jpg"
            };
                imageCovers.Add(i16);


                string[] i17 = {
                "shicun\\互动魔墙里的酒图片\\青园别院\\青园别院-2.jpg",
                "shicun\\互动魔墙里的酒图片\\青园别院\\青园别院-3.jpg",
                "shicun\\互动魔墙里的酒图片\\青园别院\\青园别院-4.jpg",
                "shicun\\互动魔墙里的酒图片\\青园别院\\青园别院-6.jpg",
                "shicun\\互动魔墙里的酒图片\\青园别院\\青园别院-8.jpg"
            };
                imageCovers.Add(i17);


                string[] i18 = {
                "shicun\\互动魔墙里的酒图片\\元庐会所\\元庐-侧面.jpg",
                "shicun\\互动魔墙里的酒图片\\元庐会所\\元庐-茶室.jpg",
                "shicun\\互动魔墙里的酒图片\\元庐会所\\元庐-房间1.jpg",
                "shicun\\互动魔墙里的酒图片\\元庐会所\\元庐-房间2.jpg",
                "shicun\\互动魔墙里的酒图片\\元庐会所\\元庐-客厅.jpg"
            };
                imageCovers.Add(i18);



                //  从数据库中获取数据
                _products = new List<Product>();

                for (int x = 0; x < names.Length; x++)
                {
                    var name = names[x];

                    var mats = imageCovers[x];

                    //
                    AddProduct(mats, name, x);

                }
            }
            #endregion

            #region 设置农产品
            if (_products2 == null)
            {
                string[] names = {
                "奉贤黄桃",
                "拾村大米",
                "有机蔬菜",
                "拾村风光",
                "拾村风光2"
                };

                //Dictionary<int, List<string>> images = new Dictionary<int, List<string>>();

                List<string[]> imageCovers = new List<string[]>();

                string[] i = {
                "shicun\\互动魔墙里的农产品\\奉贤黄桃\\图1.jpg",
                "shicun\\互动魔墙里的农产品\\奉贤黄桃\\图2.jfif",
                "shicun\\互动魔墙里的农产品\\奉贤黄桃\\图3.jpg",
                "shicun\\互动魔墙里的农产品\\奉贤黄桃\\图4.jpg",
                "shicun\\互动魔墙里的农产品\\奉贤黄桃\\图5.jpg",
            };
                imageCovers.Add(i);

                string[] i2 = {
                "shicun\\互动魔墙里的农产品\\拾村大米\\图1.jpg",
                "shicun\\互动魔墙里的农产品\\拾村大米\\图2.JPG",
                "shicun\\互动魔墙里的农产品\\拾村大米\\图3.JPG",
                "shicun\\互动魔墙里的农产品\\拾村大米\\图4.jpg",
                "shicun\\互动魔墙里的农产品\\拾村大米\\图5.jpg"
            };
                imageCovers.Add(i2);


                string[] i3 = {
                "shicun\\互动魔墙里的农产品\\有机蔬菜\\图1.jpg",
                "shicun\\互动魔墙里的农产品\\有机蔬菜\\图2.jpg",
                "shicun\\互动魔墙里的农产品\\有机蔬菜\\图3.jpg",
                "shicun\\互动魔墙里的农产品\\有机蔬菜\\图4.jpg",
                "shicun\\互动魔墙里的农产品\\有机蔬菜\\图5.jpg"
            };
                imageCovers.Add(i3);

                string[] i4 = {
                    "shicun\\互动魔墙里的农产品\\拾村风光\\1.jpg",
                    "shicun\\互动魔墙里的农产品\\拾村风光\\2.jpg",
                    "shicun\\互动魔墙里的农产品\\拾村风光\\3.jpg",
                    "shicun\\互动魔墙里的农产品\\拾村风光\\4.jpg",
                    "shicun\\互动魔墙里的农产品\\拾村风光\\5.jpg"
                 };
                imageCovers.Add(i4);

                string[] i5 = {
                    "shicun\\互动魔墙里的农产品\\拾村风光2\\1.jpg",
                    "shicun\\互动魔墙里的农产品\\拾村风光2\\2.jpg",
                    "shicun\\互动魔墙里的农产品\\拾村风光2\\3.jpg",
                    "shicun\\互动魔墙里的农产品\\拾村风光2\\4.jpg",
                 };
                imageCovers.Add(i5);

                //  从数据库中获取数据
                _products2 = new List<Product>();

                for (int x = 0; x < names.Length; x++)
                {
                    var name = names[x];

                    var mats = imageCovers[x];

                    //
                    AddProduct2(mats, name, x);

                }
            }
            #endregion

            #region 设置啤酒

            if (_wines == null)
            {
                List<string[]> wineCovers = new List<string[]>();

                _wines = new List<Product>();

                string[] wineNames = {
                     "GCC红酒",
                    "梅多克1855列级庄1级庄",
                    "梅多克1855列级庄2级庄",
                    "梅多克1855列级庄3级庄",
                    "梅多克1855列级庄4级庄",
                    "梅多克1855列级庄5级庄",
                    "丽隆红酒",
                    "默顿红酒"
                };

                string[] i = {
                "shicun\\互动魔墙里的酒图片\\GCC红酒\\杜扎克2005.jpg",
                "shicun\\互动魔墙里的酒图片\\GCC红酒\\凯隆世家2006.jpg",
                "shicun\\互动魔墙里的酒图片\\GCC红酒\\鲁臣世家2009.jpg",
                "shicun\\互动魔墙里的酒图片\\GCC红酒\\智利桑雅2007.jpg",
            };
                wineCovers.Add(i);



                string[] i10 = {
                    "shicun\\互动魔墙里的酒图片\\梅多克1855列级庄1级庄\\Chateau-Haut-Brion奥比昂庄园.jpg",
                    "shicun\\互动魔墙里的酒图片\\梅多克1855列级庄1级庄\\Chateau-Lafite-Rothschild拉菲庄园.jpg",
                    "shicun\\互动魔墙里的酒图片\\梅多克1855列级庄1级庄\\Chateau-Latour拉图庄园.jpg",
                    "shicun\\互动魔墙里的酒图片\\梅多克1855列级庄1级庄\\Chateau-Margaux玛歌庄园.jpg",
                    "shicun\\互动魔墙里的酒图片\\梅多克1855列级庄1级庄\\Chateau-Mouton-Rothschild木桐庄园.jpg"
                };
                wineCovers.Add(i10);


                string[] i11 = {
                    "shicun\\互动魔墙里的酒图片\\梅多克1855列级庄2级庄\\Chateau-Brane-Cantenac布莱恩康特纳庄园.jpg",
                    "shicun\\互动魔墙里的酒图片\\梅多克1855列级庄2级庄\\Chateau-Ducru-Boeaucaillou宝嘉龙庄园.jpg",
                    "shicun\\互动魔墙里的酒图片\\梅多克1855列级庄2级庄\\Chateau-Gruaud-Larose金玫瑰庄园.jpg",
                    "shicun\\互动魔墙里的酒图片\\梅多克1855列级庄2级庄\\Chateau-Leoville-Barton巴顿庄园.jpg",
                    "shicun\\互动魔墙里的酒图片\\梅多克1855列级庄2级庄\\Chateau-Rauzan-Gassies露仙歌庄园.jpg"
                };
                wineCovers.Add(i11);


                string[] i12 = {
                    "shicun\\互动魔墙里的酒图片\\梅多克1855列级庄3级庄\\Chateau-Calon-Segur凯隆世家酒庄.jpg",
                    "shicun\\互动魔墙里的酒图片\\梅多克1855列级庄3级庄\\Chateau-Cantenac-Brown肯德布朗庄园.jpg",
                    "shicun\\互动魔墙里的酒图片\\梅多克1855列级庄3级庄\\Chateau-La-Lagune拉拉贡酒庄.jpg"
                };
                wineCovers.Add(i12);


                string[] i13 = {
                    "shicun\\互动魔墙里的酒图片\\梅多克1855列级庄4级庄\\Chateau-Beychevelle龙船庄园.jpg",
                    "shicun\\互动魔墙里的酒图片\\梅多克1855列级庄4级庄\\Chateau-Duhart-Milon-Rothschild都夏美隆酒庄.jpg",
                    "shicun\\互动魔墙里的酒图片\\梅多克1855列级庄4级庄\\Chateau-La-Tour-Carnet拉图嘉利庄园.jpg",
                    "shicun\\互动魔墙里的酒图片\\梅多克1855列级庄4级庄\\Chateau-Pouget宝爵庄园.jpg",
                    "shicun\\互动魔墙里的酒图片\\梅多克1855列级庄4级庄\\Chateau-Prieure-Lichine荔仙庄园.jpg"
                };
                wineCovers.Add(i13);


                string[] i14 = {
                    "shicun\\互动魔墙里的酒图片\\梅多克1855列级庄5级庄\\Chateau-Batailley巴特利庄园.jpg",
                    "shicun\\互动魔墙里的酒图片\\梅多克1855列级庄5级庄\\Chateau-Belgrave百家富庄园.jpg",
                    "shicun\\互动魔墙里的酒图片\\梅多克1855列级庄5级庄\\Chateau-Camensac卡门萨克庄园.jpg",
                    "shicun\\互动魔墙里的酒图片\\梅多克1855列级庄5级庄\\Chateau-Cantemerle佳得美庄园.jpg",
                    "shicun\\互动魔墙里的酒图片\\梅多克1855列级庄5级庄\\Chateau-Clerc-Milon克拉米伦庄园.jpg",
                };
                wineCovers.Add(i14);

                string[] i15 = {
                    "shicun\\互动魔墙里的酒图片\\丽隆红酒\\Chateau-de-Rochemorin先哲堡.jpg",
                    "shicun\\互动魔墙里的酒图片\\丽隆红酒\\Chateau-La-Louvere露浮堡.jpg",
                    "shicun\\互动魔墙里的酒图片\\丽隆红酒\\L'Inedit意娜迪.jpg",
                    "shicun\\互动魔墙里的酒图片\\丽隆红酒\\露浮丽隆.jpg",
                };
                wineCovers.Add(i15);

                string[] i16 = {
                "shicun\\互动魔墙里的酒图片\\默顿红酒\\72年树龄顶级西拉.jpg",
                "shicun\\互动魔墙里的酒图片\\默顿红酒\\北部西拉.jpg",
                "shicun\\互动魔墙里的酒图片\\默顿红酒\\董事精选.jpg",
                "shicun\\互动魔墙里的酒图片\\默顿红酒\\金标绿庄定制版.jpg",
                "shicun\\互动魔墙里的酒图片\\默顿红酒\\绿色庄园灰比诺干白.jpg",
                "shicun\\互动魔墙里的酒图片\\默顿红酒\\绿色庄园西拉.jpg",
                "shicun\\互动魔墙里的酒图片\\默顿红酒\\默顿特酿merton's-blend.jpg",
                "shicun\\互动魔墙里的酒图片\\默顿红酒\\南部西拉.jpg",
                "shicun\\互动魔墙里的酒图片\\默顿红酒\\晚收麝香利口.jpg",
                "shicun\\互动魔墙里的酒图片\\默顿红酒\\五星西拉.jpg"
            };
                wineCovers.Add(i16);




                //  从数据库中获取数据


                for (int x = 0; x < wineNames.Length; x++)
                {
                    var name = wineNames[x];

                    var mats = wineCovers[x];

                    AddWines(mats, name, x);

                }
            }




            #endregion

        }


        public FlockData GetFlockData(DataTypeEnum type)
        {
            if (type == DataTypeEnum.Enterprise)
            {
                return GetEnterprise();
            }
            else if (type == DataTypeEnum.Product)
            {
                return GetProduct();
            }
            else if (type == DataTypeEnum.Activity)
            {
                return GetActivity();
            }
            return null;
        }

        public FlockData GetFlockDataByScene(DataTypeEnum type, int sceneIndex)
        {
            if (type == DataTypeEnum.Wine)
            {
                return _wines[Random.Range(0, _wines.Count)];
            }
            else {
                if (sceneIndex % 2 == 0)
                {
                    return _products[Random.Range(0, _products.Count)];
                }
                else
                {
                    return _products2[Random.Range(0, _products2.Count)];
                }
            }



        }

        public List<string> GetMatImageAddresses()
        {
            List<string> addresses = new List<string>();


            for (int i = 0; i < _products.Count; i++) {
                addresses.Add(_products[i].Image);
            }

            for (int i = 0; i < _products2.Count; i++)
            {
                addresses.Add(_products2[i].Image);
            }

            for (int i = 0; i < _wines.Count; i++)
            {
                addresses.Add(_wines[i].Image);
            }

            return addresses;
        }

        public List<string> GetLeftImagesForVBI6S()
        {
            throw new System.NotImplementedException();
        }

        public List<string> GetRigetImagesForVBI6S()
        {
            throw new System.NotImplementedException();
        }

        public List<string> GetVideosForVBI6S()
        {
            throw new System.NotImplementedException();
        }

        public List<string> GetImagesForVideoPanel8Screen(VideoPanel8Type type)
        {
            throw new System.NotImplementedException();
        }

        public List<string> GetImageForImageBothSide(VideoPanel8Type type)
        {
            throw new System.NotImplementedException();
        }
    }
}