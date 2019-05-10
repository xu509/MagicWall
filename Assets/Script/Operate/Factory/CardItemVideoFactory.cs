using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardItemVideoFactory : CardItemFactory
{
    //
    //  生存公司卡片
    //
    public IList<CrossCardCellData> Generator(int id)
    {
        List<CrossCardCellData> _cellDatas = new List<CrossCardCellData>();

        for (int i = 0; i < 3; i++)
        {
            Video e = DaoService.Instance.GetVideoDetail();
            CrossCardCellData cd = new CrossCardCellData();
            _cellDatas.Add(cd);
        }

        return _cellDatas;
    }
}
