using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  活动
//
public class Activity : Generator<Activity>
{
    // 企业 ID
    private int ent_id;
    public int Ent_id { set { ent_id = value; } get { return ent_id; } }

    // 活动的封面
    private string _image;
    public string Image { set { _image = value; } get { return _image; } }

    // 活动的标题
    private string _name;
    public string Name { set { _name = value; } get { return _name; } }

    // 活动的描述
    private string _description;
    public string Description { set { _description = value; } get { return _description; } }

    private List<ActivityDetail> _activityDetails;
    public List<ActivityDetail> ActivityDetails { set { _activityDetails = value; } get { return _activityDetails; } }


    private Texture _texture_image;
    public Texture TextureImage { set { _texture_image = value; } get { return _texture_image; } }


    public Activity Generator()
    {
        Activity activity = new Activity();
        activity.ent_id = 1;

        string[] images = {"1.jpg","2.png","3.png","4.png","5.png"
                ,"6.png","7.png","8.png"};
        activity._image = images[Random.Range(0, images.Length - 1)];

        string[] names = { "2018年春夏巴黎时装秀", "巴黎时装周（Paris Fashion Week）1910年，由法国时装协会主办。", "在米兰和伦敦的时装周相当保守，它们更喜欢本土的设计"};
        activity._name = names[Random.Range(0, names.Length - 1)];

        string[] descriptions = { "由法国时装协会主办", "它们更喜欢本土的设计" };
        activity._description = descriptions[Random.Range(0, descriptions.Length - 1)];

        activity._texture_image = AppUtils.LoadPNG(MagicWallManager.URL_ASSET + "activity\\" + activity._image);


        _activityDetails = new List<ActivityDetail>();
        ActivityDetail activityDetail = new ActivityDetail();
        _activityDetails.Add(activityDetail.Generator());
        _activityDetails.Add(activityDetail.Generator());
        _activityDetails.Add(activityDetail.Generator());

        activity.ActivityDetails = _activityDetails;

        return activity;
    }
}
