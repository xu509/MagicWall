using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  活动
//
public class ActivityDetail : Generator<ActivityDetail>
{
    // 企业 ID
    private int ent_id;
    public int Ent_id { set { ent_id = value; } get { return ent_id; } }

    // 活动的封面
    private string _image;
    public string Image { set { _image = value; } get { return _image; } }

    // 活动的描述
    private string _description;
    public string Description { set { _description = value; } get { return _description; } }

    
    private Texture _texture_image;
    public Texture TextureImage { set { _texture_image = value; } get { return _texture_image; } }


    public ActivityDetail Generator()
    {
        ActivityDetail activityDetail = new ActivityDetail();

        activityDetail.ent_id = 1;

        string[] images = { "1-1.png","1-2.png","1-3.png","1-4.png",
                         "2-1.png","2-2.png","2-3.png","2-4.png"};
        activityDetail._image = images[Random.Range(0, images.Length - 1)];

        string[] descriptions = { "今天特别活动的视频重播即将推出。","我们的最新发布: iPhone X S 超视网膜显示屏现以两种尺寸为你演绎广阔的精彩",
            "观看Apple 最新的主题演讲视频,了解我们所发布的有关产品、服务的各项特别活动。"," 在这样的结果之下,苹果手机的这些新机型销量也非常的“感人”。"};
        activityDetail._description = descriptions[Random.Range(0,descriptions.Length - 1)];

        activityDetail.TextureImage = AppUtils.LoadPNG(MagicWallManager.URL_ASSET + "activity\\detail\\" + activityDetail._image);

        return activityDetail;
    }
}
