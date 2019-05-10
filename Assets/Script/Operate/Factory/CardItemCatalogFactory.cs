using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardItemCatalogFactory : CardItemFactory
{
    //
    //  生存公司卡片
    //
    public IList<CrossCardCellData> Generator(int id)
    {
        List<CrossCardCellData> _cellDatas = new List<CrossCardCellData>();

        for (int i = 0; i < 3; i++)
        {
            Catalog e = DaoService.Instance.GetCatalog();
            CrossCardCellData cd = new CrossCardCellData();
            cd.ImageTexture = e.TextureImg;
            _cellDatas.Add(cd);
        }

        return _cellDatas;
    }
}
