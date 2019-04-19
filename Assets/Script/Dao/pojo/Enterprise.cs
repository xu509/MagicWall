using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enterprise : Generator<Enterprise>
{
    // 企业 ID
    private int ent_id;
    public int Ent_id { set { ent_id = value; } get { return ent_id; } }

    // 企业的logo
    private string logo;
    public string Logo { set { logo = value; } get { return logo; } }
    
      // 企业名字
    private string name;
    public string Name { set { name = value; } get { return name; } }

    // catalog
    private string catalog;
    public string Catalog { set { catalog = value; } get { return catalog; } }

    public Enterprise Generator()
    {
        string[] names = new string[] { "百度","可口可乐","谷歌","阿里巴巴","豆瓣","哔哩哔哩","微软","搜狗","印象笔记","迅雷"};
        string[] logos = new string[] {
            "1.jpg","2.jpg","3.jpg","4.jpg","5.jpg","6.jpg","7.jpg","8.jpg","9.jpg","10.jpg",
            "11.jpg","12.jpg","13.jpg","14.jpg","15.jpg","16.jpg","17.jpg","18.jpg","19.jpg","20.jpg",
            "21.jpg","22.jpg","23.jpg","24.jpg","25.jpg","26.jpg","27.jpg","28.jpg","29.jpg","30.jpg",
            "31.jpg","32.jpg"
        };

        Enterprise env = new Enterprise();
        int id = Random.Range(1, 10);
        env.ent_id = id;

        int name_index = Random.Range(1, names.Length - 1);
        env.name = names[name_index];

        int logo_index = Random.Range(1, logos.Length - 1);
        env.logo = logos[logo_index];

        return env;
    }
}
