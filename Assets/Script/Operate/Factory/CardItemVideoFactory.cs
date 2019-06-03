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

        for (int i = 0; i < 2; i++)
        {
            Video e = DaoService.Instance.GetVideoDetail();
            CrossCardCellData cd = new CrossCardCellData();
            cd.IsImage = false;
            cd.Description = e.Description;
            cd.Id = e.V_id;
            cd.Category = CrossCardCategoryEnum.VIDEO;
            cd.crossCardAgent = cardAgent as CrossCardAgent;
            cd.VideoUrl = e.Address;

            // 设置video的封面
            string address = MagicWallManager.URL_ASSET + "video\\" + e.Cover;
            cd.Image = address;
            
            _cellDatas.Add(cd);
        }

        return _cellDatas;
    }
}
