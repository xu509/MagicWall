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
        List<Catalog> catelogs = DaoService.Instance.GetCatalogs(id);

        for (int i = 0; i < catelogs.Count; i++)
        {
            Catalog e = catelogs[i];
            CrossCardCellData cd = new CrossCardCellData();
            cd.Image = MagicWallManager.FileDir + e.Img;
            cd.Description = e.Description;

            cd.IsImage = true;
            cd.Category = CrossCardCategoryEnum.CATALOG;
            cd.crossCardAgent = cardAgent as CrossCardAgent;
            _cellDatas.Add(cd);
        }

        return _cellDatas;
    }
}
