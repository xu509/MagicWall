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

    private Texture texture_image;
    public Texture TextureImage { set { texture_image = value; } get { return texture_image; } }


    public Product Generator()
    {
        Product product = new Product();

        product.pro_id = Random.Range(0, 10);

        product.ent_id = Random.Range(0,2);

        string[] names = {"IPHONE","MAC","椅子","桌子","包包" };
        product.name = names[Random.Range(0,names.Length - 1)];

        string[] images = {
            "1.jpg", "2.jpg", "3.jpg", "4.jpg", "5.jpg","6.jpg", "7.jpg", "8.jpg", "9.jpg",
            "10.jpg", "11.jpg", "12.jpg", "13.jpg", "14.jpg", "15.jpg", "16.jpg", "17.jpg","18.jpg", "19.jpg",
            "20.jpg", "21.jpg", "22.jpg", "23.jpg","24.jpg", "25.jpg", "26.jpg", "27.jpg", "28.jpg", "29.jpg",
            "30.jpg", "31.jpg", "32.jpg", "33.jpg","34.jpg", "35.jpg", "36.jpg", "37.jpg", "38.jpg", "39.jpg",
            "40.jpg", "41.jpg", "42.jpg", "43.jpg","44.jpg", "45.jpg", "46.jpg", "47.jpg", "48.jpg", "49.jpg",
            "50.jpg", "51.jpg", "52.jpg", "53.jpg","54.jpg", "55.jpg", "56.jpg", "57.jpg", "58.jpg", "59.jpg",
            "60.jpg"
        };

        //string[] images = {
        //    "48.jpg", "48.jpg"
        //};

        product.image = images[Random.Range(0, images.Length)];

        product.likes = Random.Range(1, 100);


        string[] descriptions = { "IPHONE", "MAC", "椅子", "桌子", "包包" };
        product.description = descriptions[Random.Range(0, descriptions.Length)];

        return product;
    }
}
