﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardItemActivityFactory : CardItemFactory
{
    //
    //  生成活动
    //
    public IList<CrossCardCellData> Generator(int id)
    {
        List<CrossCardCellData> _cellDatas = new List<CrossCardCellData>();

        for (int i = 0; i < 5; i++) {
            Activity e = DaoService.Instance.GetActivityDetail();
            CrossCardCellData cd = new CrossCardCellData();
            cd.ImageTexture = e.TextureImage;
            cd.IsImage = true;
            cd.Id = e.Ent_id;
            _cellDatas.Add(cd);
        }

        return _cellDatas;
    }
}
