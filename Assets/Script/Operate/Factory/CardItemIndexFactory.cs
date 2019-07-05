using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardItemIndexFactory : CardItemFactory
{
    //
    //  生存公司卡片
    //
    public IList<CrossCardCellData> Generator(int id,CardAgent cardAgent)
    {
        List<CrossCardCellData> _cellDatas = new List<CrossCardCellData>();

        Enterprise e = DaoService.Instance.GetEnterprise();
        CrossCardCellData cd = new CrossCardCellData();

        string address = e.Business_card;
        cd.Image = address;
        cd.Description = e.Description;

        cd.IsImage = true;
        cd.Id = e.Ent_id;
        cd.Category = CrossCardCategoryEnum.INDEX;
        cd.crossCardAgent = cardAgent as CrossCardAgent;


        _cellDatas.Add(cd);
        return _cellDatas;
    }
}
