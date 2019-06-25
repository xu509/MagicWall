using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  产品
//
public class Product : BaseData,Generator<Product>
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

    //  产品描述
    private string description;
    public string Description { set { description = value; } get { return description; } }

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

    public Texture TextureImage
    {
        get
        {
            string path = MagicWallManager.FileDir + "product\\" + Image;
            return TextureResource.Instance.GetTexture(path);
        }
     }


    public Product Generator()
    {
        Product product = new Product();

        product.pro_id = Random.Range(0, 10);

        product.ent_id = Random.Range(0,2);

        string[] names = {"IPHONE","MAC","椅子","桌子","包包" };
        product.name = names[Random.Range(0,names.Length - 1)];

        string[] images = new string[121];

        for (int i = 0; i < 121; i++) {
            images[i] = (i + 1) + ".png";
        }


        //string[] images = {
        //    "48.jpg", "48.jpg"
        //};

        product.image = "product\\" + images[Random.Range(0, images.Length)];

        product.likes = Random.Range(1, 100);


        string[] descriptions = { "IPHONE", "MAC", "椅子", "桌子", "包包" };
        product.description = descriptions[Random.Range(0, descriptions.Length)];

        return product;
    }
}
