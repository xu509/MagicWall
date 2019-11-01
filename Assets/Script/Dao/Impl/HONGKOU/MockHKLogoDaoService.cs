
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/// <summary>
///    智博会 - 奉贤
/// </summary>
namespace MagicWall
{
    public class MockHKLogoDaoService : MonoBehaviour, IDaoService
    {

        private List<Enterprise> _enterprises;
        private List<Activity> _activities;
        private List<Product> _products;

        private Dictionary<int, Enterprise> _enterpriseMap;
        private Dictionary<int, Product> _productMap;

        private Dictionary<int, List<Activity>> _activityByEidMap;
        private Dictionary<int, List<Product>> _productByEidMap;
        private Dictionary<int, List<Catalog>> _catalogByEidMap;
        private Dictionary<int, List<Video>> _videoByEidMap;

        private bool _hasInit;


        void Awake()
        {


        }

        //
        //  Construct
        //
        protected MockHKLogoDaoService() { }



        public void Init()
        {
            _enterprises = new List<Enterprise>();
            _activities = new List<Activity>();
            _products = new List<Product>();

            _productMap = new Dictionary<int, Product>();
            _enterpriseMap = new Dictionary<int, Enterprise>();
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
            return _enterprises[Random.Range(0, _enterprises.Count)];
        }

        public List<string> GetEnvCards(int id)
        {
            List<string> list = new List<string>();

            //list.Add("ZBH\\fengxian\\企业名片1.jpg");
            //list.Add("ZBH\\fengxian\\企业名片2.jpg");
            //list.Add("ZBH\\fengxian\\企业名片3.jpg");



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
            if (_catalogByEidMap.ContainsKey(id))
            {
                return _catalogByEidMap[id];
            }
            else
            {
                return null;
            }


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

            string[] leftImages = { "ZBH\\fengxian\\智城第一屏.jpg", "ZBH\\fengxian\\智城第一屏1.jpg", "ZBH\\fengxian\\智城第一屏2.jpg" };
            //string[] middleImages = { "m1.jpg", "m2.jpg", "m3.jpg", "m4.jpg", "m5.jpg" };
            string[] rightImages = { "ZBH\\fengxian\\智城第五屏.jpg" };

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

            for (int i = 0; i < _enterprises.Count; i++)
            {
                var name = _enterprises[i].Name;
                if (name.Contains(keys))
                {
                    SearchBean bean = new SearchBean();
                    bean.type = DataTypeEnum.Enterprise;
                    bean.id = _enterprises[i].Ent_id;
                    bean.cover = _enterprises[i].Business_card;
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
            return _enterpriseMap[id];
        }

        public Video GetVideoDetail(int envId, int index)
        {
            return new Video().Generator();
        }

        public List<Video> GetVideosByEnvId(int envId)
        {
            if (_videoByEidMap.ContainsKey(envId))
            {
                return _videoByEidMap[envId];
            }
            else
            {
                return new List<Video>();
            }
        }

        public List<Activity> GetActivitiesByEnvId(int envid)
        {
            if (_activityByEidMap.ContainsKey(envid))
            {
                return _activityByEidMap[envid];
            }
            else
            {
                return new List<Activity>();
            }


        }

        public List<Product> GetProductsByEnvId(int envid)
        {
            if (_productByEidMap.ContainsKey(envid))
            {
                return _productByEidMap[envid];
            }
            else
            {
                return null;
            }

        }

        public MWConfig GetConfig()
        {
            //Debug.Log("Mock Config");
            return new MWConfig();
        }

        /// <summary>
        /// 【网盘链接】https://pan.baidu.com/s/10bKtdl9sWjM437p8xFWCZA&shfl=sharepset 
        //【提取码】puri
        /// </summary>
        public void InitData()
        {
            print("Init Data");

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            if (_hasInit)
                return;


            _enterpriseMap = new Dictionary<int, Enterprise>();
            _enterprises = new List<Enterprise>();
            _activityByEidMap = new Dictionary<int, List<Activity>>();
            _productByEidMap = new Dictionary<int, List<Product>>();
            _catalogByEidMap = new Dictionary<int, List<Catalog>>();
            _videoByEidMap = new Dictionary<int, List<Video>>();

            string enterprisePath = "ZBH\\fengxian\\";

            if (Directory.Exists(MagicWallManager.FileDir + enterprisePath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(MagicWallManager.FileDir + enterprisePath);
                DirectoryInfo[] directoryInfos = dirInfo.GetDirectories();


                for (int i = 0; i < directoryInfos.Length; i++)
                {
                    string name = directoryInfos[i].Name;
                    AddEnterprise(name, i + 1);
                }
            }

            sw.Stop();
            Debug.Log("Time : " + sw.ElapsedMilliseconds / 1000f);

            print("Init Data End");
            _hasInit = true;

        }


        //
        private void AddEnterprise(string name, int ent_id)
        {
            Enterprise enterprise = new Enterprise();
            enterprise.Ent_id = ent_id;
            enterprise.Name = name;

            //string logoPath = "ZBH\\fengxian\\" + name + "\\logo.png";            
            //string businessCardPath = "ZBH\\fengxian\\" + name + "\\bcard.png";

            // logo path
            //enterprise.Logo = logoPath;
            bool hasLogo = AddEnterpriseLogo(name, enterprise);

            if (hasLogo)
            {
                // Add Catalog
                AddCatalogByEnterprise(name, ent_id);

                // Add Product
                AddProductByEnterprise(name, ent_id);

                // Add Activity
                AddActivityByEnterprise(name, ent_id);


                //增加公司名片
                AddBusinessCard(name, enterprise);

                //增加公司视频
                AddVideo(name, ent_id);

                _enterpriseMap.Add(ent_id, enterprise);
                _enterprises.Add(enterprise);
            }

        }

        private bool AddEnterpriseLogo(string name, Enterprise enterprise)
        {
            bool hasLogo = false;
            string enterprisePath = "ZBH\\fengxian\\" + name;

            if (Directory.Exists(MagicWallManager.FileDir + enterprisePath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(MagicWallManager.FileDir + enterprisePath);
                FileInfo[] files = dirInfo.GetFiles();

                List<string> bcards = new List<string>();
                string logoPath = "";

                for (int i = 0; i < files.Length; i++)
                {
                    if (CheckFileIsImage(files[i].Extension))
                    {
                        var fileName = files[i].Name;
                        logoPath = enterprisePath + "\\" + fileName;
                        hasLogo = true;
                    }
                }

                if (hasLogo)
                {
                    enterprise.Logo = logoPath;
                }
                else
                {
                    Debug.Log(name + " 没有logo图");
                }
            }
            return hasLogo;

        }

        private void AddCatalogByEnterprise(string name, int ent_id)
        {
            bool hasCatalog = false;

            string catalogDirPath = "ZBH\\fengxian\\" + name + "\\catalog";

            //print("PATH :" + (MagicWallManager.FileDir + catalogDirPath));

            if (Directory.Exists(MagicWallManager.FileDir + catalogDirPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(MagicWallManager.FileDir + catalogDirPath);
                FileInfo[] files = dirInfo.GetFiles();

                List<Catalog> catalogs = new List<Catalog>();
                for (int i = 0; i < files.Length; i++)
                {
                    var fileName = files[i].Name;

                    if (CheckFileIsImage(files[i].Extension))
                    {
                        hasCatalog = true;

                        var fileNameWithoutExt = fileName.Replace(files[i].Extension, "");
                        Catalog catalog = new Catalog();
                        catalog.Ent_id = ent_id;
                        catalog.Description = fileNameWithoutExt;
                        catalog.Id = i;
                        catalog.Img = catalogDirPath + "\\" + fileName;
                        catalogs.Add(catalog);
                    }


                }

                if (hasCatalog)
                {
                    _catalogByEidMap.Add(ent_id, catalogs);
                }

            }
        }

        private void AddProductByEnterprise(string name, int ent_id)
        {
            bool hasProduct = false;

            string catalogDirPath = "ZBH\\fengxian\\" + name + "\\产品";

            if (Directory.Exists(MagicWallManager.FileDir + catalogDirPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(MagicWallManager.FileDir + catalogDirPath);
                FileInfo[] files = dirInfo.GetFiles();

                List<Product> products = new List<Product>();
                for (int i = 0; i < files.Length; i++)
                {
                    var fileName = files[i].Name;
                    if (CheckFileIsImage(fileName))
                    {
                        hasProduct = true;

                        var fileNameWithoutExt = fileName.Replace(files[i].Extension, "");

                        Product product = new Product();
                        product.Ent_id = ent_id;
                        product.Description = fileNameWithoutExt;
                        product.Image = catalogDirPath + "\\" + fileName;
                        product.Pro_id = i;
                        product.Name = fileNameWithoutExt;
                        products.Add(product);
                    }
                }

                if (hasProduct)
                {
                    _productByEidMap.Add(ent_id, products);
                }
            }
        }

        private void AddActivityByEnterprise(string name, int ent_id)
        {
            bool hasActivity = false;

            string activityDirPath = "ZBH\\fengxian\\" + name + "\\活动";

            if (Directory.Exists(MagicWallManager.FileDir + activityDirPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(MagicWallManager.FileDir + activityDirPath);
                FileInfo[] files = dirInfo.GetFiles();

                List<Activity> activities = new List<Activity>();
                for (int i = 0; i < files.Length; i++)
                {
                    var fileName = files[i].Name;
                    if (CheckFileIsImage(fileName))
                    {
                        hasActivity = true;

                        var fileNameWithoutExt = fileName.Replace(files[i].Extension, "");

                        Activity activity = new Activity();
                        activity.Ent_id = ent_id;
                        activity.Id = i;
                        activity.Image = activityDirPath + "\\" + fileName;
                        activity.Name = fileNameWithoutExt;
                        activity.Description = fileNameWithoutExt;
                        activities.Add(activity);
                    }
                }

                if (hasActivity)
                {
                    _activityByEidMap.Add(ent_id, activities);
                }
            }
        }

        private void AddBusinessCard(string name, Enterprise enterprise)
        {
            string catalogDirPath = "ZBH\\fengxian\\" + name + "\\企业名片";

            if (Directory.Exists(MagicWallManager.FileDir + catalogDirPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(MagicWallManager.FileDir + catalogDirPath);
                FileInfo[] files = dirInfo.GetFiles();

                List<string> bcards = new List<string>();
                string busincessCarcdPath = "";


                for (int i = 0; i < files.Length; i++)
                {
                    var fileName = files[i].Name;
                    var fileNameWithoutExt = fileName.Replace(files[i].Extension, "");
                    busincessCarcdPath = catalogDirPath + "\\" + fileName;
                }

                enterprise.Business_card = busincessCarcdPath;
            }
        }

        private void AddVideo(string name, int ent_id)
        {
            bool hasVideo = false;

            string videoDirPath = "ZBH\\fengxian\\" + name + "\\视频";

            if (Directory.Exists(MagicWallManager.FileDir + videoDirPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(MagicWallManager.FileDir + videoDirPath);
                FileInfo[] files = dirInfo.GetFiles();

                List<Video> videos = new List<Video>();
                List<FileInfo> videoFiles = new List<FileInfo>();
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Extension == "mp4")
                    {
                        videoFiles.Add(files[i]);
                    }
                }
                foreach (var item in videoFiles)
                {
                    var fileNameWithoutExt = item.Name.Replace(item.Extension, "");
                    Video video = new Video();
                    video.V_id = ent_id;
                    video.Description = item.Name;
                    video.Address = videoDirPath + "\\" + item.Name;
                    video.Cover = videoDirPath + "" + fileNameWithoutExt + "";
                    videos.Add(video);
                }
                if (hasVideo)
                {
                    _videoByEidMap.Add(ent_id, videos);
                }
            }
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

            for (int i = 0; i < _enterprises.Count; i++)
            {
                var e = _enterprises[i];

                result.Add(e.Logo);

                if (e.Business_card != null && e.Business_card.Length > 0)
                {
                    result.Add(e.Business_card);
                    //Debug.Log("Enterprise : " + e.Name + " bcard : " + e.Business_card + " length : " + e.Business_card.Length);

                }

                // add products
                if (_productByEidMap.ContainsKey(e.Ent_id))
                {
                    var products = _productByEidMap[e.Ent_id];
                    for (int j = 0; j < products.Count; j++)
                    {
                        result.Add(products[j].Image);

                        //Debug.Log("Product : " + products[j].Name + " - " + products[j].Image);

                    }
                }

                if (_activityByEidMap.ContainsKey(e.Ent_id))
                {
                    // add activies
                    var activies = _activityByEidMap[e.Ent_id];
                    for (int j = 0; j < activies.Count; j++)
                    {
                        result.Add(activies[j].Image);

                        //Debug.Log("Activity : " + activies[j].Name + " - " + activies[j].Image);

                    }
                }

            }

            //var c1 = GetCustomImage(CustomImageType.LEFT1);
            //var c2 = GetCustomImage(CustomImageType.RIGHT);

            //for (int i = 0; i < c1.Count; i++)
            //{
            //    result.Add(c1[i]);
            //}

            //for (int i = 0; i < c2.Count; i++)
            //{
            //    result.Add(c2[i]);
            //}


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


        private bool CheckFileIsImage(string fileName)
        {
            if (fileName.Contains(".png"))
            {
                return true;
            }
            if (fileName.Contains(".jpg"))
            {
                return true;
            }
            return false;
        }
    }
}