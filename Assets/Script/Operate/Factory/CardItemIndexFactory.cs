using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardItemIndexFactory : CardItemFactory
{
    //
    //  生存公司卡片
    //
    public IList<CrossCardCellData> Generator(int id)
    {
        List<CrossCardCellData> _cellDatas = new List<CrossCardCellData>();

        Enterprise e = DaoService.Instance.GetEnterprise();
        CrossCardCellData cd = new CrossCardCellData();
        cd.ImageTexture = e.TextureBusinessCard;
        cd.IsImage = true;

        _cellDatas.Add(cd);
        return _cellDatas;
    }
}
