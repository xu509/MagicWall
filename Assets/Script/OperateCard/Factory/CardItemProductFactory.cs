using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall
{
    public class CardItemProductFactory : CardItemFactory
    {

        MagicWallManager _manager;

        public CardItemProductFactory(MagicWallManager manager)
        {
            _manager = manager;
        }


        //
        //  生存公司卡片
        //
        public IList<CrossCardCellData> Generator(int id, CardAgent cardAgent)
        {
            var products = _manager.daoService.GetProductsByEnvId(id);

            List<CrossCardCellData> _cellDatas = new List<CrossCardCellData>();

            for (int i = 0; i < products.Count; i++)
            {
                Product e = products[i];
                CrossCardCellData cd = new CrossCardCellData();

                string address = e.Image;
                cd.Image = address;
                cd.Description = e.Description;
                cd.magicWallManager = _manager;

                cd.IsImage = true;
                cd.Id = e.Pro_id;
                cd.Category = CrossCardCategoryEnum.PRODUCT;
                cd.crossCardAgent = cardAgent as CrossCardAgent;

                _cellDatas.Add(cd);
            }

            return _cellDatas;
        }
    }
}