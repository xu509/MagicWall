using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardItemVideoFactory : CardItemFactory
{
    //
    //  生存公司卡片
    //
    public IList<CrossCardCellData> Generator(int id,CardAgent cardAgent)
    {
        List<CrossCardCellData> _cellDatas = new List<CrossCardCellData>();

        for (int i = 0; i < 3; i++)
        {
            Video e = DaoService.Instance.GetVideoDetail();
            CrossCardCellData cd = new CrossCardCellData();
            cd.IsImage = false;
            cd.Id = e.V_id;
            cd.Category = CrossCardCategoryEnum.CATALOG;
            cd.crossCardAgent = cardAgent as CrossCardAgent;

            _cellDatas.Add(cd);
        }

        return _cellDatas;
    }
}
