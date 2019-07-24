using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardItemIndexFactory : CardItemFactory
{

    MagicWallManager _manager;

    public CardItemIndexFactory(MagicWallManager manager)
    {
        _manager = manager;
    }

    //
    //  生存公司卡片
    //
    public IList<CrossCardCellData> Generator(int id,CardAgent cardAgent)
    {
        List<CrossCardCellData> _cellDatas = new List<CrossCardCellData>();

        Enterprise e = _manager.daoService.GetEnterprisesById(id);
        CrossCardCellData cd = new CrossCardCellData();

        string address = e.Business_card;
        cd.Image = address;
        cd.Description = e.Description;
        cd.magicWallManager = _manager;

        cd.IsImage = true;
        cd.Id = e.Ent_id;
        cd.Category = CrossCardCategoryEnum.INDEX;
        cd.crossCardAgent = cardAgent as CrossCardAgent;

        _cellDatas.Add(cd);
        return _cellDatas;
    }
}
