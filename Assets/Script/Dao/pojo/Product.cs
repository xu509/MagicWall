using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  产品
//
public class Product : Generator<Product>
{
    //  产品 ID
    private int pro_id;
    public int Pro_id { set { pro_id = value; } get { return pro_id; } }

    //  企业 ID
    private int ent_id;
    public int Ent_id { set { ent_id = value; } get { return ent_id; } }
    
    //  产品名字
    private string name;
    public string Name { set { name = value; } get { return name; } }

    //  封面图片
    private string image;
    public string Image { set { image = value; } get { return image; } }

    //  喜欢的数量
    private int likes;
    public int Likes { set { likes = value; } get { return likes; } }

    //  产品详细
    private List<ProductDetail> productDetails;
    public List<ProductDetail> ProductDetails { set { productDetails = value; } get { return productDetails; } }


    // Component

    private Texture texture_image;
    public Texture TextureImage { set { texture_image = value; } get { return texture_image; } }


    public Product Generator()
    {
        Product product = new Product();

        product.pro_id = 1;
        product.ent_id = 1;

        string[] names = {"IPHONE","MAC","椅子","桌子","包包" };
        product.name = names[Random.Range(0,names.Length - 1)];

        string[] images = { "1.png", "2.png", "3.png", "4.png", "5.png",
            "6.png", "7.png", "8.png", "9.png", "10.png", "11.png" };
        product.image = images[Random.Range(0, images.Length - 1)];

        product.likes = Random.Range(1, 100);

        product.texture_image = AppUtils.LoadPNG(MagicWallManager.URL_ASSET + "product\\" + product.image);

        // detail
        ProductDetail productDetail = new ProductDetail();
        List<ProductDetail> productDetailses = new List<ProductDetail>();
        productDetailses.Add(productDetail.Generator());
        productDetailses.Add(productDetail.Generator());
        productDetailses.Add(productDetail.Generator());
        productDetailses.Add(productDetail.Generator());
        product.productDetails = productDetailses;


        return product;
    }
}
