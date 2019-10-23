
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///    飞跃定制屏模拟数据
/// </summary>
namespace MagicWall
{
    public class MockZhichengDaoService : MonoBehaviour, IDaoService
    {
        [SerializeField]
        MockSceneConfig _mockSceneConfig;

        public MockSceneConfig mockSceneConfig
        {
            set
            {
                _mockSceneConfig = value;
            }
            get
            {
                return _mockSceneConfig;
            }
        }



        private List<Enterprise> _enterprises;
        private List<Activity> _activities;
        private List<Product> _products;

        private Dictionary<int, Product> _productMap;



        void Awake()
        {


        }

        //
        //  Construct
        //
        protected MockZhichengDaoService() { }



        public void Init()
        {
            _enterprises = new List<Enterprise>();
            _activities = new List<Activity>();
            _products = new List<Product>();

            _productMap = new Dictionary<int, Product>();
        }

        public void Reset()
        {
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

            //list.Add("zhicheng\\企业名片1.jpg");
            //list.Add("zhicheng\\企业名片2.jpg");
            //list.Add("zhicheng\\企业名片3.jpg");



            return list;

        }



        //
        //  获取 catalog
        //
        public Catalog GetCatalog(int id)
        {
            throw new System.NotImplementedException();
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

            if (_products == null)
            {
                _productMap = new Dictionary<int, Product>();


                string[] names = {
                "会议室",
                "洽谈厅",
                "星巴克室内环境",
                "星巴克室外环境",
                "园区办公及行政场所",
                "园区大楼欣赏",
                "园区鸟瞰",
                "园区配套设施",
                "园区效果图",
                "智城园区能源保障区",
                "办公环境一览",
                "公司前台一览",
                "机房大厅",
                "监控室操作大厅",
                "人才公寓全貌",
                "食堂一角",
                "小绍兴饭店",
                "星巴克陈列",
                "星巴克大厅",
                "星巴克外貌",
                "园区鸟瞰图",
                "主楼风貌",
            };

                //Dictionary<int, List<string>> images = new Dictionary<int, List<string>>();

                List<string[]> imageCovers = new List<string[]>();

                string[] i = {
                "zhicheng\\会议室\\会议室.jpg",
                "zhicheng\\会议室\\小会议室.jpg"
                };
                imageCovers.Add(i);

                string[] i2 = {
                    "zhicheng\\洽谈厅\\候客区.jpg",
                    "zhicheng\\洽谈厅\\洽谈及候客厅.jpg",
                    "zhicheng\\洽谈厅\\洽谈厅.jpg",
                    "zhicheng\\洽谈厅\\书报阅读厅.jpg",
                    "zhicheng\\洽谈厅\\书报阅读厅一角.jpg",
                    "zhicheng\\洽谈厅\\阅读厅.jpg"
                };
                imageCovers.Add(i2);


                string[] i3 = {
                "zhicheng\\星巴克室内环境\\咖啡洽谈室.jpg",
                "zhicheng\\星巴克室内环境\\星巴克咖啡商品陈列.jpg",
                "zhicheng\\星巴克室内环境\\星巴克咖啡室内一景.jpg",
                "zhicheng\\星巴克室内环境\\星巴克收银台.jpg"
            };
                imageCovers.Add(i3);


                string[] i4 = {
                "zhicheng\\星巴克室外环境\\河边休憩所.jpg",
                "zhicheng\\星巴克室外环境\\星巴克咖啡.jpg",
                "zhicheng\\星巴克室外环境\\星巴克咖啡露天场所.jpg",
                "zhicheng\\星巴克室外环境\\星巴克咖啡一角.jpg",
                "zhicheng\\星巴克室外环境\\星巴克小木屋..jpg"
                 };
                imageCovers.Add(i4);


                string[] i5 = {
                "zhicheng\\园区办公及行政场所\\IMG_0073.jpg",
                "zhicheng\\园区办公及行政场所\\办公区域.jpg",
                "zhicheng\\园区办公及行政场所\\行政楼前台.jpg",
                "zhicheng\\园区办公及行政场所\\前台一角.jpg",
                "zhicheng\\园区办公及行政场所\\园区公司前台.jpg",
            };
                imageCovers.Add(i5);

                string[] i6 = {
                "zhicheng\\园区大楼欣赏\\2号楼风貌.jpg",
                "zhicheng\\园区大楼欣赏\\人才大楼.jpg",
                "zhicheng\\园区大楼欣赏\\人才公寓.jpg",
                "zhicheng\\园区大楼欣赏\\研发大楼.jpg",
                "zhicheng\\园区大楼欣赏\\主楼风貌.jpg",
            };
                imageCovers.Add(i6);

                string[] i7 = {
                "zhicheng\\园区鸟瞰\\鸟瞰智城片区.jpg",
                "zhicheng\\园区鸟瞰\\智城全貌.jpg",
            };
                imageCovers.Add(i7);


                string[] i8 = {
                "zhicheng\\园区配套设施\\露天球场.jpg",
                "zhicheng\\园区配套设施\\食堂一角.jpg",
                "zhicheng\\园区配套设施\\室外休息室.jpg",
                "zhicheng\\园区配套设施\\用餐所.jpg",
                "zhicheng\\园区配套设施\\智城园区食堂.jpg",
            };
                imageCovers.Add(i8);


                string[] i9 = {
                    "zhicheng\\园区效果图\\全景效果图.jpg",
                    "zhicheng\\园区效果图\\智城-创富领地.jpg",
                    "zhicheng\\园区效果图\\智城-创新基地.jpg",
                    "zhicheng\\园区效果图\\智城-创智天地.jpg",
                    "zhicheng\\园区效果图\\智城-人才公寓.jpg",
                    "zhicheng\\园区效果图\\智城-生活配套.jpg",
                    "zhicheng\\园区效果图\\智城-食堂.jpg",
                    "zhicheng\\园区效果图\\智城-星巴克便利店.jpg"
                };
                imageCovers.Add(i9);


                string[] i10 = {
                "zhicheng\\智城园区能源保障区\\机房一角.jpg",
                "zhicheng\\智城园区能源保障区\\园区工房.jpg",
                "zhicheng\\智城园区能源保障区\\园区机房.jpg",
                "zhicheng\\智城园区能源保障区\\园区机房一角.jpg",
                "zhicheng\\智城园区能源保障区\\园区监控室.jpg"
            };
                imageCovers.Add(i10);


                string[] i11 = {
                  "zhicheng\\竖图\\办公环境一览.jpg"
                };
                imageCovers.Add(i11);

                string[] i12 = {
                  "zhicheng\\竖图\\公司前台一览.jpg"
                };
                imageCovers.Add(i12);

                string[] i13 = {
                  "zhicheng\\竖图\\公司前台一览.jpg"
                };
                imageCovers.Add(i13);

                string[] i14 = {
                  "zhicheng\\竖图\\机房大厅.jpg"
                };
                imageCovers.Add(i14);

                string[] i15 = {
                  "zhicheng\\竖图\\监控室操作大厅.jpg"
                };
                imageCovers.Add(i15);

                string[] i16 = {
                  "zhicheng\\竖图\\人才公寓全貌.jpg"
                };
                imageCovers.Add(i16);

                string[] i17 = {
                  "zhicheng\\竖图\\食堂一角.jpg"
                };
                imageCovers.Add(i17);

                string[] i18 = {
                  "zhicheng\\竖图\\小绍兴饭店.jpg"
                };
                imageCovers.Add(i18);

                string[] i19 = {
                  "zhicheng\\竖图\\星巴克陈列.jpg"
                };
                imageCovers.Add(i19);

                string[] i20 = {
                  "zhicheng\\竖图\\星巴克大厅.jpg"
                };
                imageCovers.Add(i20);

                string[] i21 = {
                  "zhicheng\\竖图\\星巴克外貌.jpg"
                };
                imageCovers.Add(i21);

                string[] i22 = {
                  "zhicheng\\竖图\\园区鸟瞰图.jpg"
                };
                imageCovers.Add(i22);

                string[] i23 = {
                  "zhicheng\\竖图\\主楼风貌.jpg"
                };
                imageCovers.Add(i23);


                //  从数据库中获取数据
                _products = new List<Product>();

                for (int x = 0; x < names.Length; x++)
                {
                    var name = names[x];

                    var mats = imageCovers[x];

                    //
                    AddProduct(mats, name, x);

                    //Product product = new Product();
                    //product.Name = name;
                    //product.Pro_id = x;
                    //product.Ent_id = 0;
                    //product.Description = name;
                    //product.Image = mats[0];


                    //List<ProductDetail> productDetails = new List<ProductDetail>();
                    //for (int y = 0; y < mats.Length; y++) {
                    //    var mat = mats[y];

                    //    ProductDetail productDetail = new ProductDetail();
                    //    productDetail.Id = y;
                    //    productDetail.Type = 0;
                    //    productDetail.Pro_id = x;
                    //    productDetail.Description = mat;
                    //    productDetail.Image = mat;
                    //    productDetails.Add(productDetail);
                    //}

                    //product.ProductDetails = productDetails;
                    //_products.Add(product);
                }

                return _products;
            }
            else
            {
                return _products;
            }
        }

        private void AddProduct(string[] details, string name, int proid)
        {

            for (int i = 0; i < details.Length; i++)
            {
                Product product = new Product();
                product.Name = name;

                int pro_id = 0;
                int.TryParse(proid.ToString() + i.ToString(), out pro_id);
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

                // todo 添加视频
                ProductDetail productDetailVideo = new ProductDetail();
                productDetailVideo.Id = 99;
                productDetailVideo.Type = 1;
                productDetailVideo.Pro_id = pro_id;
                productDetailVideo.Description = "《智城宣传片5.18》";
                productDetailVideo.Image = "zhicheng\\video.png";
                productDetailVideo.VideoUrl = "zhicheng\\智城宣传片5.18.mp4";
                productDetails.Add(productDetailVideo);


                product.ProductDetails = productDetails;


                //Debug.Log(pro_id);

                _productMap.Add(pro_id, product);

                _products.Add(product);
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
            /// 已修改为编辑器配置方式
            ///  -》 config / MockSceneConfig 


            List<SceneConfig> items = new List<SceneConfig>();

            var sceneConfigs = _mockSceneConfig.sceneConfigs;

            for (int i = 0; i < sceneConfigs.Count; i++)
            {
                var scene = sceneConfigs[i].sceneType;
                var data = sceneConfigs[i].dataType;
                var time = sceneConfigs[i].durtime;


                if (scene == SceneTypeEnum.Stars && data == DataTypeEnum.Enterprise)
                {
                    continue;
                }

                if (scene == SceneTypeEnum.FrontBackUnfold && data == DataTypeEnum.Enterprise)
                {
                    continue;
                }


                items.Add(sceneConfigs[i]);
            }

            return items;


        }

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

            string[] leftImages = { "zhicheng\\智城第一屏.jpg", "zhicheng\\智城第一屏1.jpg", "zhicheng\\智城第一屏2.jpg" };
            //string[] middleImages = { "m1.jpg", "m2.jpg", "m3.jpg", "m4.jpg", "m5.jpg" };
            string[] rightImages = { "zhicheng\\智城第五屏.jpg" };

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

            //// 公司卡片初始化
            //List<string> list = new List<string>();

            //list.Add("zhicheng\\企业名片1.jpg");
            //list.Add("zhicheng\\企业名片2.jpg");
            //list.Add("zhicheng\\企业名片3.jpg");

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

            _productMap = new Dictionary<int, Product>();
            _products = new List<Product>();

            string[] names = {
                "会议室",
                "洽谈厅",
                "星巴克室内环境",
                "星巴克室外环境",
                "园区办公及行政场所",
                "园区大楼欣赏",
                "园区鸟瞰",
                "园区配套设施",
                "园区效果图",
                "智城园区能源保障区",
                "办公环境一览",
                "公司前台一览",
                "机房大厅",
                "监控室操作大厅",
                "人才公寓全貌",
                "食堂一角",
                "小绍兴饭店",
                "星巴克陈列",
                "星巴克大厅",
                "星巴克外貌",
                "园区鸟瞰图",
                "主楼风貌",
            };

            //Dictionary<int, List<string>> images = new Dictionary<int, List<string>>();

            List<string[]> imageCovers = new List<string[]>();

            string[] i = {
                "zhicheng\\会议室\\会议室.jpg",
                "zhicheng\\会议室\\小会议室.jpg"
                };
            imageCovers.Add(i);

            string[] i2 = {
                    "zhicheng\\洽谈厅\\候客区.jpg",
                    "zhicheng\\洽谈厅\\洽谈及候客厅.jpg",
                    "zhicheng\\洽谈厅\\洽谈厅.jpg",
                    "zhicheng\\洽谈厅\\书报阅读厅.jpg",
                    "zhicheng\\洽谈厅\\书报阅读厅一角.jpg",
                    "zhicheng\\洽谈厅\\阅读厅.jpg"
                };
            imageCovers.Add(i2);


            string[] i3 = {
                "zhicheng\\星巴克室内环境\\咖啡洽谈室.jpg",
                "zhicheng\\星巴克室内环境\\星巴克咖啡商品陈列.jpg",
                "zhicheng\\星巴克室内环境\\星巴克咖啡室内一景.jpg",
                "zhicheng\\星巴克室内环境\\星巴克收银台.jpg"
            };
            imageCovers.Add(i3);


            string[] i4 = {
                "zhicheng\\星巴克室外环境\\河边休憩所.jpg",
                "zhicheng\\星巴克室外环境\\星巴克咖啡.jpg",
                "zhicheng\\星巴克室外环境\\星巴克咖啡露天场所.jpg",
                "zhicheng\\星巴克室外环境\\星巴克咖啡一角.jpg",
                "zhicheng\\星巴克室外环境\\星巴克小木屋..jpg"
                 };
            imageCovers.Add(i4);


            string[] i5 = {
                "zhicheng\\园区办公及行政场所\\IMG_0073.jpg",
                "zhicheng\\园区办公及行政场所\\办公区域.jpg",
                "zhicheng\\园区办公及行政场所\\行政楼前台.jpg",
                "zhicheng\\园区办公及行政场所\\前台一角.jpg",
                "zhicheng\\园区办公及行政场所\\园区公司前台.jpg",
            };
            imageCovers.Add(i5);

            string[] i6 = {
                "zhicheng\\园区大楼欣赏\\2号楼风貌.jpg",
                "zhicheng\\园区大楼欣赏\\人才大楼.jpg",
                "zhicheng\\园区大楼欣赏\\人才公寓.jpg",
                "zhicheng\\园区大楼欣赏\\研发大楼.jpg",
                "zhicheng\\园区大楼欣赏\\主楼风貌.jpg",
            };
            imageCovers.Add(i6);

            string[] i7 = {
                "zhicheng\\园区鸟瞰\\鸟瞰智城片区.jpg",
                "zhicheng\\园区鸟瞰\\智城全貌.jpg",
            };
            imageCovers.Add(i7);


            string[] i8 = {
                "zhicheng\\园区配套设施\\露天球场.jpg",
                "zhicheng\\园区配套设施\\食堂一角.jpg",
                "zhicheng\\园区配套设施\\室外休息室.jpg",
                "zhicheng\\园区配套设施\\用餐所.jpg",
                "zhicheng\\园区配套设施\\智城园区食堂.jpg",
            };
            imageCovers.Add(i8);


            string[] i9 = {
                    "zhicheng\\园区效果图\\全景效果图.jpg",
                    "zhicheng\\园区效果图\\智城-创富领地.jpg",
                    "zhicheng\\园区效果图\\智城-创新基地.jpg",
                    "zhicheng\\园区效果图\\智城-创智天地.jpg",
                    "zhicheng\\园区效果图\\智城-人才公寓.jpg",
                    "zhicheng\\园区效果图\\智城-生活配套.jpg",
                    "zhicheng\\园区效果图\\智城-食堂.jpg",
                    "zhicheng\\园区效果图\\智城-星巴克便利店.jpg"
                };
            imageCovers.Add(i9);


            string[] i10 = {
                "zhicheng\\智城园区能源保障区\\机房一角.jpg",
                "zhicheng\\智城园区能源保障区\\园区工房.jpg",
                "zhicheng\\智城园区能源保障区\\园区机房.jpg",
                "zhicheng\\智城园区能源保障区\\园区机房一角.jpg",
                "zhicheng\\智城园区能源保障区\\园区监控室.jpg"
            };
            imageCovers.Add(i10);


            string[] i11 = {
                  "zhicheng\\竖图\\办公环境一览.jpg"
                };
            imageCovers.Add(i11);

            string[] i12 = {
                  "zhicheng\\竖图\\公司前台一览.jpg"
                };
            imageCovers.Add(i12);

            string[] i13 = {
                  "zhicheng\\竖图\\公司前台一览.jpg"
                };
            imageCovers.Add(i13);

            string[] i14 = {
                  "zhicheng\\竖图\\机房大厅.jpg"
                };
            imageCovers.Add(i14);

            string[] i15 = {
                  "zhicheng\\竖图\\监控室操作大厅.jpg"
                };
            imageCovers.Add(i15);

            string[] i16 = {
                  "zhicheng\\竖图\\人才公寓全貌.jpg"
                };
            imageCovers.Add(i16);

            string[] i17 = {
                  "zhicheng\\竖图\\食堂一角.jpg"
                };
            imageCovers.Add(i17);

            string[] i18 = {
                  "zhicheng\\竖图\\小绍兴饭店.jpg"
                };
            imageCovers.Add(i18);

            string[] i19 = {
                  "zhicheng\\竖图\\星巴克陈列.jpg"
                };
            imageCovers.Add(i19);

            string[] i20 = {
                  "zhicheng\\竖图\\星巴克大厅.jpg"
                };
            imageCovers.Add(i20);

            string[] i21 = {
                  "zhicheng\\竖图\\星巴克外貌.jpg"
                };
            imageCovers.Add(i21);

            string[] i22 = {
                  "zhicheng\\竖图\\园区鸟瞰图.jpg"
                };
            imageCovers.Add(i22);

            string[] i23 = {
                  "zhicheng\\竖图\\主楼风貌.jpg"
                };
            imageCovers.Add(i23);


            //  从数据库中获取数据
            _products = new List<Product>();

            for (int x = 0; x < names.Length; x++)
            {
                var name = names[x];

                var mats = imageCovers[x];

                //
                AddProduct(mats, name, x);

            }

            //throw new System.NotImplementedException();
        }

        public int GetLikes(string path)
        {
            return 1;
            //throw new System.NotImplementedException();
        }

        public bool UpdateLikes(string path)
        {
            return true;
            //throw new System.NotImplementedException();
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
            return GetFlockData(type);
        }

        public List<string> GetMatImageAddresses()
        {
            var result = new List<string>();

            for (int i = 0; i < _products.Count; i++)
            {
                result.Add(_products[i].Image);
            }

            var c1 = GetCustomImage(CustomImageType.LEFT1);
            var c2 = GetCustomImage(CustomImageType.RIGHT);

            for (int i = 0; i < c1.Count; i++)
            {
                result.Add(c1[i]);
            }

            for (int i = 0; i < c2.Count; i++)
            {
                result.Add(c2[i]);
            }


            return result;
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
    }
}