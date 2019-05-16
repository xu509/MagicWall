using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardItemActivityFactory : CardItemFactory
{
    //
    //  生成活动
    //
    public IList<CrossCardCellData> Generator(int id,CardAgent cardAgent)
    {
        List<CrossCardCellData> _cellDatas = new List<CrossCardCellData>();

        for (int i = 0; i < 5; i++) {
            Activity e = DaoService.Instance.GetActivityDetail();
            CrossCardCellData cd = new CrossCardCellData();

            string address = MagicWallManager.URL_ASSET + "activity\\" + e.Image;
            cd.ImageTexture = TextureResource.Instance.GetTexture(address);

            cd.IsImage = true;
            cd.Id = e.Ent_id;
            cd.Category = CrossCardCategoryEnum.ACTIVITY;
            cd.crossCardAgent = cardAgent as CrossCardAgent;
            _cellDatas.Add(cd);
        }

        return _cellDatas;
    }
}
