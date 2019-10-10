using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MagicWall {

    public class ActivityAdapter
    {

        public static OperateCardDataSlide Transfer(Activity activity,Enterprise enterprises) {
            if (activity == null)
                return null;

            OperateCardDataSlide operateCardDataSlide = new OperateCardDataSlide();
            operateCardDataSlide.DataType = DataTypeEnum.Activity;
            operateCardDataSlide.Cover = activity.Image;
            operateCardDataSlide.Description = activity.Description;
            operateCardDataSlide.Id = activity.Id;
            operateCardDataSlide.Title = activity.Name;


            if (enterprises != null) {
                var envcards = enterprises.EnvCards;
                List<ExtraCardData> extraCardDatas = new List<ExtraCardData>();
                for (int i = 0; i < envcards.Count; i++) {
                    ExtraCardData extraCardData = new ExtraCardData();
                    extraCardData.Cover = envcards[i];
                    extraCardDatas.Add(extraCardData);
                }
                operateCardDataSlide.ExtraCardData = extraCardDatas;
            }

            var details = activity.ActivityDetails;

            if (details != null && details.Count > 0) {
                List<ScrollData> scrollDatas = new List<ScrollData>();

                for (int i = 0; i < details.Count; i++) {
                    ScrollData scrollData = new ScrollData();
                    scrollData.Cover = details[i].Image;
                    scrollData.Type = details[i].Type;
                    scrollData.Description = details[i].Description;
                    scrollData.Src = details[i].VideoUrl;
                    scrollDatas.Add(scrollData);
                }
                operateCardDataSlide.ScrollData = scrollDatas;
            }
         
            return operateCardDataSlide;
        }

    }


}


