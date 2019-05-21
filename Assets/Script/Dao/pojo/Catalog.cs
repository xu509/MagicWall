using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  Catalog
//
public class Catalog : Generator<Catalog>
{
    private int _id;
    public int Id { set { _id = value; } get { return _id; } }

    // 企业 ID
    private int _ent_id;
    public int Ent_id { set { _ent_id = value; } get { return _ent_id; } }

    // img
    private string _img;
    public string Img { set { _img = value; } get { return _img; } }

    private string _description;
    public string Description { set { _description = value; } get { return _description; } }


    // Texture
    private Texture _texture_img;
    public Texture TextureImg { set { _texture_img = value; } get { return _texture_img; } }

    public Catalog Generator()
    {
        Catalog catalog = new Catalog();

        string[] imgs = { "catalog-1-1.png", "catalog-1-2.png", "catalog-1-3.png", "catalog-1-4.png" };
        catalog._img = imgs[Random.Range(0, imgs.Length - 1)];

        string[] descriptions = { "catalog-1-1.png", "catalog-1-2.png", "catalog-1-3.png", "catalog-1-4.png" };
        catalog.Description = descriptions[Random.Range(0, descriptions.Length - 1)];

        return catalog;
    }
}
