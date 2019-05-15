using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardItemProductFactory : CardItemFactory
{
    //
    //  生存公司卡片
    //
    public IList<CrossCardCellData> Generator(int id)
    {
        List<CrossCardCellData> _cellDatas = new List<CrossCardCellData>();

        for (int i = 0; i < 3; i++)
        {
            Product e = DaoService.Instance.GetProductDetail();
            CrossCardCellData cd = new CrossCardCellData();
            cd.ImageTexture = e.TextureImage;
            cd.IsImage = true;
            _cellDatas.Add(cd);
        }

        return _cellDatas;
    }
}
