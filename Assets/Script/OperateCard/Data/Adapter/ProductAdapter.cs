using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MagicWall {

    public class ProductAdapter
    {

        public static OperateCardDataSlide Transfer(Product product,Enterprise enterprises) {
            if (product == null)
                return null;

            OperateCardDataSlide operateCardDataSlide = new OperateCardDataSlide();
            operateCardDataSlide.DataType = DataTypeEnum.Product;
            operateCardDataSlide.Cover = product.Image;
            operateCardDataSlide.Description = product.Description;
            operateCardDataSlide.Id = product.Pro_id;
            operateCardDataSlide.Title = product.Name;


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

            var details = product.ProductDetails;

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


