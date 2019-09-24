using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MagicWall {

    public class EnterpriseAdapter
    {

        public static OperateCardDataCross Transfer(Enterprise enterprise,List<Activity> activities,
            List<Product> products,List<Video> videos,List<Catalog> catalogs) {
            if (enterprise == null)
                return null;

            OperateCardDataCross operateCardDataCross = new OperateCardDataCross();
            operateCardDataCross.Id = enterprise.Ent_id;
            operateCardDataCross.Title = enterprise.Name;
            operateCardDataCross.Cover = enterprise.Business_card;
            operateCardDataCross.DataType = DataTypeEnum.Enterprise;
            operateCardDataCross.Description = enterprise.Description;

            // index;
            Dictionary<CrossCardNavType,List<ScrollData>> dics = new Dictionary<CrossCardNavType, List<ScrollData>> ();

            List<ScrollData> list = new List<ScrollData>();
            ScrollData scrollData = new ScrollData();
            scrollData.Cover = enterprise.Business_card;
            scrollData.Type = 0;
            list.Add(scrollData);
            dics.Add(CrossCardNavType.Index, list);

            // category
            if (catalogs != null && catalogs.Count > 0)
            {
                List<ScrollData> categoryList = new List<ScrollData>();
                for (int i = 0; i < catalogs.Count; i++) {
                    var cat = catalogs[i];
                    ScrollData sd = new ScrollData();
                    sd.Type = 0;
                    sd.Cover = cat.Img;
                    sd.Description = cat.Description;
                    categoryList.Add(sd);
                }
                dics.Add(CrossCardNavType.Category, categoryList);
            }

            // product
            if (products != null && products.Count > 0)
            {
                List<ScrollData> l = new List<ScrollData>();
                for (int i = 0; i < products.Count; i++)
                {
                    var prod = products[i];
                    ScrollData sd = new ScrollData();
                    sd.Type = 0;
                    sd.Cover = prod.Image;
                    sd.Description = prod.Description;
                    l.Add(sd);
                }
                dics.Add(CrossCardNavType.Product, l);
            }

            // activity
            if (activities != null && activities.Count > 0)
            {
                List<ScrollData> l = new List<ScrollData>();
                for (int i = 0; i < activities.Count; i++)
                {
                    var data = activities[i];
                    ScrollData sd = new ScrollData();
                    sd.Type = 0;
                    sd.Cover = data.Image;
                    sd.Description = data.Description;
                    l.Add(sd);
                }
                dics.Add(CrossCardNavType.Activity, l);
            }

            // video
            if (videos != null && videos.Count > 0)
            {
                List<ScrollData> l = new List<ScrollData>();
                for (int i = 0; i < videos.Count; i++)
                {
                    var data = videos[i];
                    ScrollData sd = new ScrollData();
                    sd.Type = 1;
                    sd.Cover = data.Cover;
                    sd.Src = data.Address;
                    sd.Description = data.Description;
                    l.Add(sd);
                }
                dics.Add(CrossCardNavType.Video, l);
            }

            operateCardDataCross.ScrollDic = dics;


            return operateCardDataCross;
        }



    }


}


