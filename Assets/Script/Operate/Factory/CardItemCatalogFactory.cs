using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardItemCatalogFactory : CardItemFactory
{
    //
    //  生存公司 Category
    //
    public IList<CrossCardCellData> Generator(int id,CardAgent cardAgent)
    {
        List<CrossCardCellData> _cellDatas = new List<CrossCardCellData>();

        for (int i = 0; i < 3; i++)
        {
            Catalog e = DaoService.Instance.GetCatalog();
            CrossCardCellData cd = new CrossCardCellData();

            string address = MagicWallManager.URL_ASSET + "env\\" + e.Img;
            cd.ImageTexture = TextureResource.Instance.GetTexture(address);

            cd.IsImage = true;
            cd.Category = CrossCardCategoryEnum.CATALOG;
            cd.crossCardAgent = cardAgent as CrossCardAgent;
            _cellDatas.Add(cd);
        }

        return _cellDatas;
    }
}
