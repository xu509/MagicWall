using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  产品
//
public class ProductDetail : Generator<ProductDetail>
{
    // 产品 ID
    private int pro_id;
    public int Pro_id { set { pro_id = value; } get { return pro_id; } }

    // 产品详细
    private string description;
    public string Description { set { description = value; } get { return description; } }

    //  图片
    private string image;
    public string Image { set { image = value; } get { return image; } }


    // Texture
    private Texture texture_image;
    public Texture TextureImage { set { texture_image = value; } get { return texture_image; } }


    public ProductDetail Generator()
    {
        ProductDetail productDetail = new ProductDetail();

        string[] productImgs = { "product-detail-1-1.png", "product-detail-1-2.png", "product-detail-1-3.png" };
        string[] descriptions = { "macbook是2015年苹果公司出品的笔记本电脑。",
            "2015年3月9日，苹果春季发布会在美国旧金山芳草地艺术中心召开。发布会上苹果重点发布了全新的Macbook 12英寸新机型，采用全新设计，分为灰、银、金三色，12英寸Retina显分辨率为2304 x 1440，处理器为英特尔酷睿M低功耗处理器。",
            "2018年6月22日，苹果官网宣布推出了适用于MacBook的键盘服务计划，将免费为符合条件的MacBook键盘提供服务。" };

        productDetail.description = descriptions[Random.Range(0,descriptions.Length - 1)];
        productDetail.image = productImgs[Random.Range(0, productImgs.Length - 1)];
        productDetail.pro_id = 1;

        productDetail.texture_image = AppUtils.LoadPNG(MagicWallManager.URL_ASSET + "//product//detail//" + productDetail.image);


        return productDetail;
    }
}
