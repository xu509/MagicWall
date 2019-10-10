using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  活动
//

namespace MagicWall
{
    public class ActivityDetail : Generator<ActivityDetail>
    {
        private int _id;
        public int Id { set { _id = value; } get { return _id; } }

        private int type; // 0: 图片 / 1： 视频

        public int Type { set { type = value; } get { return type; } }

        // 企业 ID
        private int ent_id;
        public int Ent_id { set { ent_id = value; } get { return ent_id; } }

        // 活动的封面
        private string _image;
        public string Image { set { _image = value; } get { return _image; } }

        // 活动的描述
        private string _description;
        public string Description { set { _description = value; } get { return _description; } }

        // 视频地址
        private string videoUrl;

        public string VideoUrl { set { videoUrl = value; } get { return videoUrl; } }


        public void SetImageType()
        {
            type = 0;
        }

        public void SetVideoType()
        {
            type = 1;
        }

        public bool IsImage() { return type == 0; }

        public bool IsVideo() { return type == 1; }


        public ActivityDetail Generator()
        {
            ActivityDetail activityDetail = new ActivityDetail();

            activityDetail.Id = Random.Range(0, 10);

            activityDetail.ent_id = Random.Range(0, 2);

            string[] images = { "1-1.png","1-2.png","1-3.png","1-4.png",
                         "2-1.png","2-2.png","2-3.png","2-4.png"};
            activityDetail._image = "activity\\" + images[Random.Range(0, images.Length - 1)];

            string[] descriptions = { "今天特别活动的视频重播即将推出。","我们的最新发布: iPhone X S 超视网膜显示屏现以两种尺寸为你演绎广阔的精彩",
            "观看Apple 最新的主题演讲视频,了解我们所发布的有关产品、服务的各项特别活动。"," 在这样的结果之下,苹果手机的这些新机型销量也非常的“感人”。"};
            activityDetail._description = descriptions[Random.Range(0, descriptions.Length - 1)];

            //activityDetail.TextureImage = AppUtils.LoadPNG(MagicWallManager.URL_ASSET + "activity\\detail\\" + activityDetail._image);

            return activityDetail;
        }

        public List<string> GetAssetAddressList()
        {
            string[] listAry = {
            "activity\\detail\\1-1.jpg",
            "activity\\detail\\1-2.jpg",
            "activity\\detail\\2-1.jpg",
            "activity\\detail\\2-2.jpg",
            "activity\\detail\\2-3.jpg",
            "activity\\detail\\3-1.jpg",
            "activity\\detail\\3-2.jpg",
            "activity\\detail\\3-3.jpg",
        };

            List<string> list = new List<string>();

            for (int i = 0; i < listAry.Length; i++)
            {
                string address = listAry[i];
                list.Add(address);
            }

            return list;
        }
    }
}