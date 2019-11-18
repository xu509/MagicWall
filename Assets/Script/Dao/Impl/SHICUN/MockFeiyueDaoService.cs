
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///    飞跃定制屏模拟数据
/// </summary>
namespace MagicWall
{
    public class MockFeiyueDaoService : MonoBehaviour, IDaoService
    {
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
        protected MockFeiyueDaoService() { }



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

            list.Add("feiyue\\企业名片1.jpg");
            list.Add("feiyue\\企业名片2.jpg");
            list.Add("feiyue\\企业名片3.jpg");



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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="details">同产品下的多张图片，如 [“detailImages1”,“detailImages2”]</param>
        /// <param name="name"></param>
        /// <param name="proid"></param>
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
                product.Image = details[i]; // 封面

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



                // todo 添加视频
                ProductDetail productDetailVideo = new ProductDetail();
                productDetailVideo.Id = 99;
                productDetailVideo.Type = 1;
                productDetailVideo.Pro_id = pro_id;
                productDetailVideo.Description = "《中国之造ChinaMade》";
                productDetailVideo.Image = "feiyue\\feiyue1_Moment.png";
                productDetailVideo.VideoUrl = "feiyue\\feiyue1.mp4";
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

            var item = _products[index];

            //Debug.Log("number : " + product.Count + " | " + item.GetId());

            return item;
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
            string[] leftImages = { "feiyue\\第一屏.jpg", "feiyue\\第一屏1.jpg" };
            //string[] middleImages = { "m1.jpg", "m2.jpg", "m3.jpg", "m4.jpg", "m5.jpg" };
            string[] rightImages = { "feiyue\\第五屏.jpg" };

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

            // 公司卡片初始化
            List<string> list = new List<string>();

            list.Add("feiyue\\企业名片1.jpg");
            list.Add("feiyue\\企业名片2.jpg");
            list.Add("feiyue\\企业名片3.jpg");

            enterprise.EnvCards = list;





            return enterprise;
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
            // 初始化数据

            _products = new List<Product>();

            _productMap = new Dictionary<int, Product>();


            string[] names = {
                "ins风法国版帆布鞋",
                "超纤皮休闲百搭板鞋",
                "骑士风高帮帆布鞋",
                "魔术贴学生跑步鞋",
                "经典运动休闲超纤鞋",
                "低帮帆布鞋",
                "低帮女学生帆布鞋",
                "复古潮流豹纹平底鞋",
                "低帮帆布女鞋",
                "大孚飞跃文字快闪小白鞋",
                "半拖懒人复古帆布鞋",
                "高跟马丁靴",
                "飞跃漫威联名款拖鞋",
                "复古原宿帆布小白鞋",
                "休闲情侣鸳鸯鞋",
                "涂鸦迷彩帆布鞋",
                "绿色潮流休闲情侣帆布鞋",
                "经典帆布鞋",
                "手绘街头嘻哈学生板鞋",
                "休闲男鞋低帮情侣学生潮流小白鞋",
            };

            //Dictionary<int, List<string>> images = new Dictionary<int, List<string>>();

            List<string[]> imageCovers = new List<string[]>();

            string[] i = {
                "feiyue\\ins风法国版帆布鞋\\1.jpg",
                "feiyue\\ins风法国版帆布鞋\\2.jpg",
                "feiyue\\ins风法国版帆布鞋\\3.jpg",
                "feiyue\\ins风法国版帆布鞋\\4.jpg",
                "feiyue\\ins风法国版帆布鞋\\5.jpg",
            };
            imageCovers.Add(i);

            string[] i2 = {
                "feiyue\\超纤皮休闲百搭板鞋\\1.jpg",
                "feiyue\\超纤皮休闲百搭板鞋\\2.jpg",
                "feiyue\\超纤皮休闲百搭板鞋\\3.jpg",
                "feiyue\\超纤皮休闲百搭板鞋\\4.jpg"
            };
            imageCovers.Add(i2);


            string[] i3 = {
                "feiyue\\骑士风高帮帆布鞋\\1.jpg",
                "feiyue\\骑士风高帮帆布鞋\\2.jpg",
                "feiyue\\骑士风高帮帆布鞋\\3.jpg"
            };
            imageCovers.Add(i3);


            string[] i4 = {
                "feiyue\\魔术贴学生跑步鞋\\1.jpg",
                "feiyue\\魔术贴学生跑步鞋\\2.jpg",
                "feiyue\\魔术贴学生跑步鞋\\3.jpg",
                "feiyue\\魔术贴学生跑步鞋\\4.jpg"
            };
            imageCovers.Add(i4);


            string[] i5 = {
                "feiyue\\经典运动休闲超纤鞋\\1.jpg",
                "feiyue\\经典运动休闲超纤鞋\\2.jpg",
                "feiyue\\经典运动休闲超纤鞋\\3.jpg",
                "feiyue\\经典运动休闲超纤鞋\\4.jpg",
                "feiyue\\经典运动休闲超纤鞋\\5.jpg",
            };
            imageCovers.Add(i5);

            string[] i6 = {
                "feiyue\\低帮帆布鞋\\1.jpg",
                "feiyue\\低帮帆布鞋\\2.jpg",
                "feiyue\\低帮帆布鞋\\3.jpg",
                "feiyue\\低帮帆布鞋\\4.jpg",
            };
            imageCovers.Add(i6);

            string[] i7 = {
                "feiyue\\低帮女学生帆布鞋\\1.jpg",
                "feiyue\\低帮女学生帆布鞋\\2.jpg",
                "feiyue\\低帮女学生帆布鞋\\3.jpg",
                "feiyue\\低帮女学生帆布鞋\\4.jpg",
            };
            imageCovers.Add(i7);


            string[] i8 = {
                "feiyue\\复古潮流豹纹平底鞋\\1.jpg",
                "feiyue\\复古潮流豹纹平底鞋\\2.jpg",
                "feiyue\\复古潮流豹纹平底鞋\\3.jpg",
            };
            imageCovers.Add(i8);


            string[] i9 = {
                "feiyue\\低帮帆布女鞋\\1.jpg",
                "feiyue\\低帮帆布女鞋\\2.jpg",
                "feiyue\\低帮帆布女鞋\\3.jpg",
                "feiyue\\低帮帆布女鞋\\4.jpg",
            };
            imageCovers.Add(i9);


            string[] i10 = {
                "feiyue\\大孚飞跃文字快闪小白鞋\\8837249.jpg",
                "feiyue\\大孚飞跃文字快闪小白鞋\\8837408.jpg"
            };
            imageCovers.Add(i10);


            string[] i11 = {
                "feiyue\\半拖懒人复古帆布鞋\\1.jpg",
                "feiyue\\半拖懒人复古帆布鞋\\2.jpg",
                "feiyue\\半拖懒人复古帆布鞋\\3.jpg",
                "feiyue\\半拖懒人复古帆布鞋\\4.jpg",
                "feiyue\\半拖懒人复古帆布鞋\\5.jpg",
                "feiyue\\半拖懒人复古帆布鞋\\6.jpg",
                "feiyue\\半拖懒人复古帆布鞋\\7.jpg"
            };
            imageCovers.Add(i11);


            string[] i12 = {
                "feiyue\\高跟马丁靴\\1.jpg",
                "feiyue\\高跟马丁靴\\2.jpg",
                "feiyue\\高跟马丁靴\\3.jpg",
                "feiyue\\高跟马丁靴\\4.jpg",
                "feiyue\\高跟马丁靴\\5.jpg",
                "feiyue\\高跟马丁靴\\6.jpg",
                "feiyue\\高跟马丁靴\\7.jpg",
                "feiyue\\高跟马丁靴\\8.jpg"
            };
            imageCovers.Add(i12);


            string[] i13 = {
                "feiyue\\飞跃漫威联名款拖鞋\\1.jpg",
                "feiyue\\飞跃漫威联名款拖鞋\\2.jpg",
                "feiyue\\飞跃漫威联名款拖鞋\\3.jpg",
                "feiyue\\飞跃漫威联名款拖鞋\\4.jpg",
                "feiyue\\飞跃漫威联名款拖鞋\\5.jpg",
            };
            imageCovers.Add(i13);


            string[] i14 = {
                "feiyue\\复古原宿帆布小白鞋\\1.jpg",
                "feiyue\\复古原宿帆布小白鞋\\2.jpg",
                "feiyue\\复古原宿帆布小白鞋\\3.jpg",
                "feiyue\\复古原宿帆布小白鞋\\4.jpg",
                "feiyue\\复古原宿帆布小白鞋\\5.jpg",
            };
            imageCovers.Add(i14);


            string[] i15 = {
                "feiyue\\休闲情侣鸳鸯鞋\\1.jpg",
                "feiyue\\休闲情侣鸳鸯鞋\\2.jpg",
                "feiyue\\休闲情侣鸳鸯鞋\\3.jpg",
                "feiyue\\休闲情侣鸳鸯鞋\\4.jpg",
                "feiyue\\休闲情侣鸳鸯鞋\\5.jpg",
                "feiyue\\休闲情侣鸳鸯鞋\\6.jpg",
            };
            imageCovers.Add(i15);


            string[] i16 = {
                "feiyue\\涂鸦迷彩帆布鞋\\1.jpg",
                "feiyue\\涂鸦迷彩帆布鞋\\2.jpg",
                "feiyue\\涂鸦迷彩帆布鞋\\3.jpg",
                "feiyue\\涂鸦迷彩帆布鞋\\4.jpg"
            };
            imageCovers.Add(i16);


            string[] i17 = {
                "feiyue\\绿色潮流休闲情侣帆布鞋\\1.jpg",
                "feiyue\\绿色潮流休闲情侣帆布鞋\\2.jpg",
                "feiyue\\绿色潮流休闲情侣帆布鞋\\3.jpg",
                "feiyue\\绿色潮流休闲情侣帆布鞋\\4.jpg",
                "feiyue\\绿色潮流休闲情侣帆布鞋\\5.jpg",
                "feiyue\\绿色潮流休闲情侣帆布鞋\\6.jpg",
            };
            imageCovers.Add(i17);


            string[] i18 = {
                "feiyue\\经典帆布鞋\\7747427.jpg",
                "feiyue\\经典帆布鞋\\7747787.jpg",
                "feiyue\\经典帆布鞋\\7747957.jpg",
                "feiyue\\经典帆布鞋\\7748486.jpg",
                "feiyue\\经典帆布鞋\\7748971.jpg",
                "feiyue\\经典帆布鞋\\7749427.jpg",
                "feiyue\\经典帆布鞋\\7787236.jpg",
                "feiyue\\经典帆布鞋\\7787656.jpg",
                "feiyue\\经典帆布鞋\\7787783.jpg",
                "feiyue\\经典帆布鞋\\7794456.jpg",
                "feiyue\\经典帆布鞋\\7794807.jpg",
            };
            imageCovers.Add(i18);


            string[] i19 = {
                "feiyue\\手绘街头嘻哈学生板鞋\\1.jpg",
                "feiyue\\手绘街头嘻哈学生板鞋\\2.jpg",
                "feiyue\\手绘街头嘻哈学生板鞋\\3.jpg",
                "feiyue\\手绘街头嘻哈学生板鞋\\4.jpg",
                "feiyue\\手绘街头嘻哈学生板鞋\\5.jpg",
                "feiyue\\手绘街头嘻哈学生板鞋\\6.jpg",
                "feiyue\\手绘街头嘻哈学生板鞋\\7.jpg",
                "feiyue\\手绘街头嘻哈学生板鞋\\8.jpg",
            };
            imageCovers.Add(i19);


            string[] i20 = {
                "feiyue\\休闲男鞋低帮情侣学生潮流小白鞋\\1.jpg",
                "feiyue\\休闲男鞋低帮情侣学生潮流小白鞋\\2.jpg",
                "feiyue\\休闲男鞋低帮情侣学生潮流小白鞋\\3.jpg",
                "feiyue\\休闲男鞋低帮情侣学生潮流小白鞋\\4.jpg"
            };
            imageCovers.Add(i20);


            //  从数据库中获取数据
            for (int x = 0; x < names.Length; x++)
            {
                var name = names[x];
                var mats = imageCovers[x];

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
            var item = GetFlockData(type);

            //Debug.Log("Get by scene : " + item.GetId());


            return item;
        }

        public List<string> GetMatImageAddresses()
        {
            var result = new List<string>();

            for (int i = 0; i < _products.Count; i++) {
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