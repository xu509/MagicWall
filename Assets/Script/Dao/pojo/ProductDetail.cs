using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  产品
//
namespace MagicWall
{
    public class ProductDetail : Generator<ProductDetail>
    {

        private int id;
        public int Id { set { id = value; } get { return id; } }

        private int type; // 0: 图片 / 1： 视频

        public int Type { set { type = value; } get { return type; } }

        // 产品 ID
        private int pro_id;
        public int Pro_id { set { pro_id = value; } get { return pro_id; } }

        // 产品详细
        private string description;
        public string Description { set { description = value; } get { return description; } }

        //  图片
        private string image;
        public string Image { set { image = value; } get { return image; } }

        // 视频地址
        private string videoUrl;

        public string VideoUrl { set { videoUrl = value; } get { return videoUrl; } }


        public bool IsImage()
        {
            return type == 0;
        }

        public bool IsVideo()
        {
            return type == 1;
        }

        public void SetImageType()
        {
            type = 0;
        }

        public void SetVideoType()
        {
            type = 1;
        }

        string[] productImgs = { "product-detail-1-1.png", "product-detail-1-2.png", "product-detail-1-3.png" };

        public ProductDetail Generator()
        {
            ProductDetail productDetail = new ProductDetail();

            string[] descriptions = { "macbook是2015年苹果公司出品的笔记本电脑。",
            "2015年3月9日，苹果春季发布会在美国旧金山芳草地艺术中心召开。发布会上苹果重点发布了全新的Macbook 12英寸新机型，采用全新设计，分为灰、银、金三色，12英寸Retina显分辨率为2304 x 1440，处理器为英特尔酷睿M低功耗处理器。",
            "2018年6月22日，苹果官网宣布推出了适用于MacBook的键盘服务计划，将免费为符合条件的MacBook键盘提供服务。" };

            productDetail.description = descriptions[Random.Range(0, descriptions.Length)];
            productDetail.image = productImgs[Random.Range(0, productImgs.Length)];
            productDetail.pro_id = 1;

            return productDetail;
        }

        public List<string> GetAssetAddressList()
        {
            List<string> list = new List<string>();

            string[] listAry = {
            "product\\detail\\1-1.jpg",
            "product\\detail\\1-2.jpg",
            "product\\detail\\1-3.jpg",
            "product\\detail\\1-4.jpg",
            "product\\detail\\2-1.jpg",
            "product\\detail\\2-2.jpg",
            "product\\detail\\2-3.jpg",
            "product\\detail\\2-4.jpg",
            "product\\detail\\2-5.jpg",
            "product\\detail\\3-1.jpg",
            "product\\detail\\3-2.jpg",
            "product\\detail\\3-3.jpg",
            "product\\detail\\3-4.jpg",
            "product\\detail\\3-5.jpg",
            "product\\detail\\4-1.jpg",
            "product\\detail\\4-2.jpg",
        };


            for (int i = 0; i < listAry.Length; i++)
            {
                string address = listAry[i];
                list.Add(address);
            }

            return list;
        }
    }
}