﻿
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/// <summary>
///    飞跃定制屏模拟数据（虹口）
/// </summary>
namespace MagicWall
{
    public class MockHKFeiyueDaoService : MonoBehaviour, IDaoService
    {


        private List<Enterprise> _enterprises;
        private List<Activity> _activities;
        private List<Product> _products;

        private Dictionary<int, Product> _productMap;

        private bool _hasInit = false;

        void Awake()
        {


        }

        //
        //  Construct
        //
        protected MockHKFeiyueDaoService() { }



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

            list.Add("feiyue2\\企业名片1.jpg");
            list.Add("feiyue2\\企业名片2.jpg");
            list.Add("feiyue2\\企业名片3.jpg");



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


        public Product GetProduct()
        {
            if (_products.Count > 0)
            {
                int index = Random.Range(0, _products.Count);

                var item = _products[index];

                return item;
            }
            else {
                return null;
            }

            
        }

        //
        //  获取产品详细
        //
        public Product GetProductDetail(int pro_id)
        {
            //Debug.Log("pro_id : " + pro_id);

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
            string[] leftImages = { "feiyue2\\第一屏.jpg", "feiyue2\\第一屏1.jpg" };
            //string[] middleImages = { "m1.jpg", "m2.jpg", "m3.jpg", "m4.jpg", "m5.jpg" };
            string[] rightImages = { "feiyue2\\第五屏.jpg" };

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
            print("飞跃：" + beans.Count);
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
            //Enterprise enterprise = new Enterprise();

            //// 公司卡片初始化
            //List<string> list = new List<string>();

            //list.Add("feiyue2\\企业名片1.jpg");
            //list.Add("feiyue2\\企业名片2.jpg");
            //list.Add("feiyue2\\企业名片3.jpg");

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
            // 初始化数据
            Debug.Log("init Data feiyue kinect");
            if (_hasInit) {
                return;
            }



            _products = new List<Product>();
            _productMap = new Dictionary<int, Product>();

            string pathDir = "HK\\feiyue2";

            if (Directory.Exists(MagicWallManager.FileDir + pathDir))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(MagicWallManager.FileDir + pathDir);
                DirectoryInfo[] directories = dirInfo.GetDirectories();

                for (int i = 0; i < directories.Length; i++) {
                    var directory = directories[i];

                    int pro_id = i;
                    int.TryParse(i.ToString(), out pro_id);

                    AddProduct(directory, pro_id);
            
                }


            }
            else {
                print("初始化文件夹不存在");
            }

            _hasInit = true;
            //Debug.Log("产品总数： " + _products.Count);

        }

        private void AddProduct(DirectoryInfo directoryInfo,int index) {
            // 扫描内部的所有内容
            var fileInfos = directoryInfo.GetFiles();

            //Debug.Log("扫描内部的所有内容");
            //Debug.Log("directoryInfo:" + directoryInfo.FullName);
            //Debug.Log("fileInfos:" + fileInfos.Length);
 
            for (int i = 0; i < fileInfos.Length; i++) {
                var fileInfo = fileInfos[i];

                if (fileInfo.Extension.Contains("jpg") || fileInfo.Extension.Contains("png")) {
                    Product product = new Product();
                    product.Ent_id = 0;

                    int pro_id = 0;
                    int.TryParse(index.ToString() + i.ToString(), out pro_id);

                    product.Pro_id = pro_id;
                    product.Image = "HK\\feiyue2\\" + directoryInfo.Name + "\\" + fileInfo.Name;
                    product.Name = directoryInfo.Name;
                    product.ProductDetails = GetProductDetails(directoryInfo, fileInfo, pro_id);

                    _products.Add(product);
                    _productMap.Add(pro_id, product);
                }
            }
        }



        /// <summary>
        /// 此处有修改，其他dao需要比对此方法
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="fileInfo"></param>
        /// <param name="proId"></param>
        /// <returns></returns>
        private List<ProductDetail> GetProductDetails(DirectoryInfo directoryInfo,FileInfo fileInfo,int proId) {
            List<ProductDetail> productDetails = new List<ProductDetail>();

            var fileInfos = directoryInfo.GetFiles();
            int coverIndex = 0;
            int index = 0;
            for (int i = 0; i < fileInfos.Length; i++)
            {
                if (fileInfos[i].Extension.Contains("jpg") || fileInfos[i].Extension.Contains("png"))
                {
                    var fileName = fileInfos[i].Name.Replace(fileInfos[i].Extension, "");

                    ProductDetail productDetail = new ProductDetail();
                    productDetail.Id = i;
                    productDetail.Pro_id = proId;
                    productDetail.Type = 0;
                    productDetail.Image = "HK\\feiyue2\\" + directoryInfo.Name + "\\" + fileInfos[i].Name; ;                    
                    productDetail.Description = fileName;
                    productDetails.Add(productDetail);


                    //Debug.Log("FULLNAME : " + fileInfo.Name + " - FULLNAME[i]" + fileInfos[i].FullName);

                    if (fileInfo.Name.Equals(fileInfos[i].Name))
                    {
                        coverIndex = index;
                    }
                    index++;
                }
            }

            var temp = productDetails[0];
            var tempC = productDetails[coverIndex];
            productDetails[0] = tempC;
            productDetails[coverIndex] = temp;

            return productDetails;
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
                //ProductDetail productDetailVideo = new ProductDetail();
                //productDetailVideo.Id = 99;
                //productDetailVideo.Type = 1;
                //productDetailVideo.Pro_id = pro_id;
                //productDetailVideo.Description = "《中国之造ChinaMade》";
                //productDetailVideo.Image = "feiyue2\\feiyue1_Moment.png";
                //productDetailVideo.VideoUrl = "feiyue2\\feiyue1.mp4";
                //productDetails.Add(productDetailVideo);


                product.ProductDetails = productDetails;


                //Debug.Log(pro_id);
                _productMap.Add(pro_id, product);

                _products.Add(product);
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